using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class SubCategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }
        
        public string ImageUrl { get; set; }
    }
}