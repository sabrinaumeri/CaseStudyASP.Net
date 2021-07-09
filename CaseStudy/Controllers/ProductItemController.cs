using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.Domain_Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        AppDbContext _db;
        public ProductItemController(AppDbContext context)
        {
            _db = context;
        }
        [Route("{catid}")]
        public async Task<ActionResult<List<Product>>> Index(int catid)
        {
            ProductItemDAO dao = new ProductItemDAO(_db);
            List<Product> itemsForCategory = await dao.GetAllByCategory(catid);
            return itemsForCategory;
        }

    }
}
