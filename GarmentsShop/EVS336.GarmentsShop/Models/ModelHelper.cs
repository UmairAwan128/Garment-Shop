using EVS336;
using EVS336.GarmentsShop;
using EVS336.GarmentsShop.Controllers;
using EVS336.GarmentsShop.Models.Categories;
using EVS336.GarmentsShop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EVS336.GarmentsShop.Models
{
    public static class ModelHelper
    {
        
        public static List<SelectListItem> ToSelectItemList(this List<DepartmentModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text=entity.Name, Value=Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }
        public static List<SelectListItem> ToSelectItemList(this List<FabricsModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }


        public static List<SelectListItem> ToSelectItemList(this List<RolesModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }

        public static List<SelectListItem> ToSelectItemList(this List<CountryModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }
        public static List<SelectListItem> ToSelectItemList(this List<ColorsModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }

        public static List<SelectListItem> ToSelectItemList(this List<SizesModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }
        public static List<SelectListItem> ToSelectItemList(this List<CategoryModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }

        public static List<SelectListItem> ToSelectItemList(this List<SubCategoryModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }

        public static List<SelectListItem> ToSelectItemList(this List<ProvinceModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }

        public static List<SelectListItem> ToSelectItemList(this List<CityModel> entityList)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var entity in entityList)
            {
                tempList.Add(new SelectListItem { Text = entity.Name, Value = Convert.ToString(entity.Id) });
            }
            tempList.TrimExcess();
            return tempList;
        }


    }
}