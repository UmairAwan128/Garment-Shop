﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            CartProducts = new List<ProductModel>();
            CartProductIDs = new List<int>();
        }

        public List<ProductModel> CartProducts { get; set; }

        public List<int> CartProductIDs { get; set; }
    }
}