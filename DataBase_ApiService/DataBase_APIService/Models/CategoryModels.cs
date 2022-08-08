using DataBaseEntityClasses.GarmentsShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //Parent contains the DepartmentId connected to this category 
        public int Parent { get; set; }

        public string DepartmentName { get; set; }

        //will contain the Products referenced to this Category
        public int ProductsConnected { get; set; }

    }

    public class EditCategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string SelectedParent { get; set; }
            
    }
}