using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.DAL.Domain_Classes
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class Product //: ControllerBase
    {
        public string Id { get; set; }
        public Brand Brand { get; set; }
        public byte[] Timer { get; set; }
        public string ProductName { get; set; }
        public string GraphicName { get; set; }
        [Column (TypeName = "money")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal MSRP { get; set; }
        public int QtyOnHand { get; set; }
        public int QtyOnBackOrder { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
    }
}
