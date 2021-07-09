using CaseStudy.DAL.Domain_Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.DAL.DAO
{
    public class ProductItemDAO
    {
        private AppDbContext _db;
        public ProductItemDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Product>> GetAllByCategory(int id)
        {
            return await _db.ProductItems.Where(item => item.Brand.Id == id).ToListAsync();
        }

    }
}
