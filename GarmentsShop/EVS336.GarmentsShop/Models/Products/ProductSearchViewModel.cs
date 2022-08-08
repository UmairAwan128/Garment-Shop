using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models.Products
{
    public class ProductSearchViewModel
    {
        public ProductSearchViewModel() {
            products = new List<ProductSummaryModel>();
        }
        public List<ProductSummaryModel> products { get; set; }
        public string searchTerm { get; set; }
        
        public Pager pager { get; set; }
    }
}