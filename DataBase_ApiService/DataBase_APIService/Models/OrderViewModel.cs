using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {

            CartProducts = new List<ProductModel>();
        }

        public List<ProductModel> CartProducts { get; set; }

        public UserModel User { get; set; }

    }
}