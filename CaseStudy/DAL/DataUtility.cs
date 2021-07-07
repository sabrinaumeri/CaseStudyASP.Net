using CaseStudy.DAL.Domain_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CaseStudy.DAL
{
    public class DataUtility
    {
        private AppDbContext _db;

        public DataUtility(AppDbContext context)
        {
            _db = context;
        }

        public async Task<bool> loadElectronicInfoFromWebToDb(string stringJson)
        {
            bool brandsLoaded = false;
            bool productsLoaded = false;

            try
            {// an element that is typed as dynamic is assumed to support any operation
                dynamic objectJson = JsonSerializer.Deserialize<Object>(stringJson);
                brandsLoaded = await loadBrands(objectJson);
                productsLoaded = await loadMenuItems(objectJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return brandsLoaded && productsLoaded;
        }


        private async Task<bool> loadBrands(dynamic jsonObjectArray)
        {
            bool loadedProducts = false;

            try
            {
                // clear out the old rows
                _db.Brands.RemoveRange(_db.Brands);
                await _db.SaveChangesAsync();

                List<String> allBrands = new List<String>();

                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    if (element.TryGetProperty("BRAND", out JsonElement menuItemJson))
                    {
                        allBrands.Add(menuItemJson.GetString());
                    }
                }

                IEnumerable<String> brands = allBrands.Distinct<String>();

                foreach (string catname in brands)
                {
                    Brand brnd = new Brand();
                    brnd.Name = catname;
                    await _db.Brands.AddAsync(brnd);
                    await _db.SaveChangesAsync();
                }

                loadedProducts = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedProducts;
        }


        private async Task<bool> loadMenuItems(dynamic jsonObjectArray)
        {
            bool loadedItems = false;
            try
            {
                List<Brand> brands = _db.Brands.ToList();
                // clear out the old
                _db.ProductItems.RemoveRange(_db.ProductItems);
                await _db.SaveChangesAsync();
                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    Product item = new Product();
                    //item.brand = element.GetProperty("BRAND").GetString();
                    item.Id = element.GetProperty("ID").GetString();
                    item.ProductName = element.GetProperty("PRODUCT NAME").GetString();
                    item.GraphicName = element.GetProperty("GRAPHIC NAME").GetString();
                    item.CostPrice = Convert.ToDecimal(element.GetProperty("COST PRICE").GetString()); //(decimal)Convert.ToSingle(element.GetProperty("COST PRICE").GetString());
                    item.MSRP = Convert.ToDecimal(element.GetProperty("MSRP").GetString());
                    item.QtyOnHand = Convert.ToInt32(element.GetProperty("QUANTITY ON HAND").GetString());
                    item.QtyOnBackOrder = Convert.ToInt32(element.GetProperty("QUANTITY ON BACK ORDER").GetString());
                    item.Description = element.GetProperty("DESCRIPTION").GetString();
                    string cat = element.GetProperty("BRAND").GetString();
                    // add the FK here
                    foreach (Brand brand in brands)
                    {
                        if (brand.Name == cat)
                        {
                            item.Brand = brand;
                            break;
                        }
                    }
                    await _db.ProductItems.AddAsync(item);
                    await _db.SaveChangesAsync();
                }
                loadedItems = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedItems;
        }

    }
}
