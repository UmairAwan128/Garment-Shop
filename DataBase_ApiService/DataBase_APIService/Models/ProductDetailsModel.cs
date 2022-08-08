using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class ProductDetailsModel
    {
        public ProductDetailsModel()
        {
            ColorList = new List<ColorsModel>();
            SizeList = new List<SizesModel>();
            ProductImg = new List<ImagesModel>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public List<ImagesModel> ProductImg { get; set; }
        public string Discription { get; set; }
        public Nullable<DateTime> LaunchingDate { get; set; }
        public List<ColorsModel> ColorList { get; set; }
        public List<SizesModel> SizeList { get; set; }
        public FabricsModel Fabric { get; set; }

    }
}