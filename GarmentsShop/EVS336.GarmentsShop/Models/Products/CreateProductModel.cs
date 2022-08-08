using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models.Products
{
    public class CreateProductModel
    {
        public CreateProductModel()
        {
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

        public ICollection<ImagesModel> ProductImages { get; set; }

        public ICollection<int> ColorsOfferedId { get; set; }

        public ICollection<int> SizesOfferedId { get; set; }

        public int FabricId { get; set; }

    }
}