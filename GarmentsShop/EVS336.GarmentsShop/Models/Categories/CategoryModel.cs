using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models.Categories
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Parent contains the DepartmentId connected to this category 
        public string Parent { get; set; }

        public string DepartmentName { get; set; }

        //will contain the Products referenced to this Category
        public int ProductsConnected { get; set; }

    }
}