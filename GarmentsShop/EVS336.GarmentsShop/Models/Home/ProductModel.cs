using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models.Home
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
    }
}