using EVS336.GarmentsShop.Models.Products;
using EVS336.GarmentsShop.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {

            CartProducts = new List<ProductModel>();
        }

        public List<ProductModel> CartProducts { get; set; }

        public UserSessionModel User { get; set; }

    }
}