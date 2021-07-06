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
            bool loadedBrands = false;

            try
            {
                // clear out the old rows
                _db.BrandItems.RemoveRange(_db.BrandItems);
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
                    await _db.BrandItems.AddAsync(brnd);
                    await _db.SaveChangesAsync();
                }

                loadedBrands = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrands;
        }


        private async Task<bool> loadMenuItems(dynamic jsonObjectArray)
        {
            bool loadedItems = false;
            try
            {
                List<Brand> brands = _db.BrandItems.ToList();
                // clear out the old
                _db.BrandItems.RemoveRange(_db.BrandItems);
                await _db.SaveChangesAsync();
                foreach (JsonElement element in jsonObjectArray.EnumerateArray())
                {
                    Product item = new Product();
                    //item.brand = element.GetProperty("BRAND").GetString();
                    item.ProductName = element.GetProperty("Product Name").GetString();
                    item.GraphicName = element.GetProperty("Image Name").GetString();
                    item.CostPrice = (decimal)Convert.ToSingle(element.GetProperty("Cost Price").GetString());
                    item.MSRP = Convert.ToInt32(element.GetProperty("MSRP").GetString());
                    item.QtyOnHand = Convert.ToInt32(element.GetProperty("QTY On Hand").GetString());
                    item.QtyOnBackOrder = Convert.ToInt32(element.GetProperty("Qty On Back Order").GetString());
                    item.Description = element.GetProperty("Description").GetString();
                    string cat = element.GetProperty("CATEGORY").GetString();
                    // add the FK here
                    foreach (Brand brand in brands)
                    {
                        if (brand.Name == cat)
                        {
                            item.Brand = brand;
                            break;
                        }
                    }
                    await _db.Products.AddAsync(item);
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
