using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models.Products
{
    public class ProductDetailsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public List<string> ImageUrls { get; set; }
        
    }
}