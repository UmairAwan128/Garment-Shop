﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class ProductSearchViewModel
    {
        public ProductSearchViewModel()
        {
            products = new List<ProductSummaryModel>();
        }
        public List<ProductSummaryModel> products { get; set; }
        public string searchTerm { get; set; }

        //for simple pagination
        //public int PageNo { get; set; }

        public Pager pager { get; set; }
    }
}