using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class ProductSummaryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public string ImageUrl { get; set; }
        public string AlternateImageUrl { get; set; }
    }

    public class ProductModel
    {
        public ProductModel() {

            ProductImages = new List<ImagesModel>();

            ColorsOfferedId = new List<int>();

            SizesOfferedId = new List<int>();

        }

    public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public Nullable<DateTime> LaunchingDate { get; set; }

        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public int DepartmentId { get; set; }

        public ICollection<ImagesModel> ProductImages { get; set; }

        public ICollection<int> ColorsOfferedId { get; set; }

        public ICollection<int> SizesOfferedId { get; set; }

        public int FabricId { get; set; }

    }

}