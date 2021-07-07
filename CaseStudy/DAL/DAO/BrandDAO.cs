using CaseStudy.DAL.Domain_Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.DAL.DAO
{
    public class BrandDAO
    {
        private AppDbContext _db;
        public BrandDAO(AppDbContext ctx)
        {
            _db = ctx;
        }

        public async Task<List<Brand>> GetAll()
        {
            return await _db.Brands.ToListAsync<Brand>();
        }
    }
}
