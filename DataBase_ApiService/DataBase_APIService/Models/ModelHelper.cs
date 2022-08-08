using DataBaseEntityClasses;
using DataBaseEntityClasses.GarmentsShop;
using DataBaseEntityClasses.UsersMgt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public static class ModelHelper
    {
        
        public static UserModel ToModel(this User entity)
        {
            return new UserModel { LoginId=entity.LoginId, Password=entity.Password , Id=entity.Id, RoleId = entity.Role.Id};
        }

        public static DepartmentModel ToModel(this Department entity)
        {
            return new DepartmentModel { Id=entity.Id, Name=entity.Name , ImageUrl = entity.ImageUrl };
        }

        public static List<DepartmentModel> ToModelList(this List<Department> entitiesList)
        {
            List<DepartmentModel> modelsList = new List<DepartmentModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }

        public static SizesModel ToModel(this Size entity)
        {
            return new SizesModel { Id = entity.Id, Name = entity.Name };
        }

        public static List<SizesModel> ToModelList(this List<Size> entitiesList)
        {
            List<SizesModel> modelsList = new List<SizesModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }


        public static FabricsModel ToModel(this Fabric entity)
        {
            return new FabricsModel { Id = entity.Id, Name = entity.Name };
        }

        public static List<FabricsModel> ToModelList(this List<Fabric> entitiesList)
        {
            List<FabricsModel> modelsList = new List<FabricsModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }

        public static CountryModel ToModel(this Country entity)
        {
            return new CountryModel { Id = entity.Id, Name = entity.Name, Code = entity.Code };
        }


        public static List<CountryModel> ToModelList(this List<Country> entitiesList)
        {
            List<CountryModel> modelsList = new List<CountryModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }

        public static ProvinceModel ToModel(this Province entity)
        {
            return new ProvinceModel { Id = entity.Id, Name = entity.Name, Country = entity.Country.ToModel() };
        }

        public static List<ProvinceModel> ToModelList(this List<Province> entitiesList)
        {
            List<ProvinceModel> modelsList = new List<ProvinceModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }


        public static CityModel ToModel(this City entity)
        {
            return new CityModel { Id = entity.Id, Name = entity.Name, Province=entity.Province.ToModel() };
        }


        public static List<CityModel> ToModelList(this List<City> entitiesList)
        {
            List<CityModel> modelsList = new List<CityModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }


        public static AddressModel ToModel(this Address entity)
        {
            return new AddressModel { Id = entity.Id, StreetAddress=entity.StreetAddress, City= entity.City.ToModel()};
        }


        public static List<AddressModel> ToModelList(this List<Address> entitiesList)
        {
            List<AddressModel> modelsList = new List<AddressModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }




        public static ColorsModel ToModel(this Color entity)
        {
            return new ColorsModel { Id = entity.Id, Name = entity.Name};
        }

        public static List<ColorsModel> ToModelList(this List<Color> entitiesList)
        {
            List<ColorsModel> modelsList = new List<ColorsModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }


        public static RolesModel ToModel(this Role entity)
        {
            return new RolesModel { Id = entity.Id, Name = entity.Name };
        }

        public static List<RolesModel> ToModelList(this List<Role> entitiesList)
        {
            List<RolesModel> modelsList = new List<RolesModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }


        public static Country ToEntity(this CountryModel model)
        {
            return new Country { Id = model.Id, Name = model.Name, Code = model.Code };
        }


        public static User ToEntity(this UserModel model)
        {
            return new User {
                               Id = model.Id, LoginId = model.LoginId, Password = model.Password,
                               Role = (from c in new GarmentsHandler().GetRoles()
                                       where c.Id == model.RoleId
                                       select c).FirstOrDefault()
                            };
        }


        public static Role ToEntity(this RolesModel model)
        {
            return new Role { Id = model.Id, Name = model.Name };
        }

        public static Category ToEntity(this CategoryModel model)
        {
            return new Category { Id = model.Id, Name = model.Name,
                Department = (from c in new GarmentsHandler().GetDeparments()
                              where c.Id == model.Parent
                              select c).FirstOrDefault()
            };

        }

        public static Order ToEntity(this OrderViewModel model)
        {
            Order order = new Order();
            foreach (var product in model.CartProducts) {
                order.Products.Add(product.ToEntity());
            }
            order.User = model.User.ToEntity();
            return order;
        }


        public static Product ToEntity(this ProductModel model)
        {
            Product p = new Product();

            p.Id = model.Id;
            p.Name = model.Name;
            p.SubCategory = (from c in new GarmentsHandler().GetSubCategories()
                             where c.Id == model.SubCategoryId
                             select c).FirstOrDefault();
            p.Price = model.Price;
            p.Description = model.Description;
            p.LaunchingDate = model.LaunchingDate;
            p.Fabric = (from c in new GarmentsHandler().GetFabrics()
                        where c.Id == model.FabricId
                        select c).FirstOrDefault();


            foreach (var entity in model.ColorsOfferedId)
            {
                p.ColorsOffered.Add( (from c in new GarmentsHandler().GetColors()
                                       where c.Id == entity
                                       select c).FirstOrDefault() );
            }
            foreach (var entity in model.SizesOfferedId)
            {
                p.SizesOffered.Add((from c in new GarmentsHandler().GetSizes()
                                    where c.Id == entity
                                    select c).FirstOrDefault());
            }
            foreach (var ImageModel in model.ProductImages)
            {
                p.Images.Add(ImageModel.ToEntity());
            }
            
            return p;
     
        }


        public static User ToEntity(this SignUpModel model)
        {
            User p = new User();

            p.Id = model.Id;
            p.Name = model.Name;
            p.Role = (from c in new GarmentsHandler().GetRoles()
                             where c.Id == model.Role.Id
                             select c).FirstOrDefault();

            Address address = new Address();
            address.StreetAddress = model.Address.StreetAddress;
            address.City = (from c in new LocationsHandler().GetCities()
                            where c.Id == model.Address.City.Id
                            select c).FirstOrDefault();
            p.Address = address;

            p.BirthDate = model.BirthDate;
            p.ContactNumber = model.ContactNumber;
            p.Email = model.Email;
            p.ImageUrl = model.ImageUrl;
            p.LoginId = model.LoginId;
            p.Password = model.Password;
            
            p.SecurityAnswer = model.SecurityAnswer;
            p.SecurityQuestion = model.SecurityQuestion;
            return p;

        }


        public static ProductImage ToEntity(this ImagesModel model)
        {
            return new ProductImage { Id = model.Id,Url = model.Url, Caption=model.Caption, Priority=model.Priority };
        }



        public static Address ToEntity(this AddressModel model)
        {
            return new Address { Id = model.Id, StreetAddress=model.StreetAddress , City = model.City.ToEntity() };
        }

        public static City ToEntity(this CityModel model)
        {
            return new City { Id = model.Id, Name=model.Name, Province = model.Province.ToEntity() };
        }

        public static Province ToEntity(this ProvinceModel model)
        {
            return new Province { Id = model.Id, Name = model.Name, Country=model.Country.ToEntity() };
        }


        public static CategoryModel ToModel(this Category entity)
        {
            CategoryModel cm = new CategoryModel();
            cm.Id = entity.Id;
            cm.Name = entity.Name;
            cm.Parent = entity.Department.Id;
            cm.DepartmentName = entity.Department.Name;
            cm.ProductsConnected = new GarmentsHandler().GetProductsCount(cm.Id);
            return cm;
        }


        public static List<CategoryModel> ToModelList(this List<Category> entityList)
        {
            List<CategoryModel> modelList = new List<CategoryModel>();
            foreach (var entity in entityList)
            {
                //CategoryModel model = ModelHelper.ToModel(entity);
                //CategoryModel model = entity.ToModel();

                modelList.Add(entity.ToModel());
            }
            modelList.TrimExcess();
            return modelList;
        }

        public static SubCategoryModel ToModel(this SubCategory entity)
        {
            return new SubCategoryModel { Id = entity.Id, Name = entity.Name, CategoryId=entity.Category.Id, ImageUrl= entity.ImageUrl };
        }


        public static List<SubCategoryModel> ToModelList(this List<SubCategory> entityList)
        {
            List<SubCategoryModel> modelList = new List<SubCategoryModel>();
            foreach (var entity in entityList)
            {
                //CategoryModel model = ModelHelper.ToModel(entity);
                //CategoryModel model = entity.ToModel();

                modelList.Add(entity.ToModel());
            }
            modelList.TrimExcess();
            return modelList;
        }

        public static BannerModel ToModel(this Banner entity)
        {
            return new BannerModel { Id = entity.Id, Heading = entity.Heading, SubHeading = entity.SubHeading, ImageUrl = entity.ImageUrl };
        }


        public static List<BannerModel> ToModelList(this List<Banner> entityList)
        {
            List<BannerModel> modelList = new List<BannerModel>();
            foreach (var entity in entityList)
            {
                modelList.Add(entity.ToModel());
            }
            modelList.TrimExcess();
            return modelList;
        }

        public static ProductSummaryModel ToSummaryModel(this Product entity)
        {
            ProductSummaryModel model = new ProductSummaryModel { Id = entity.Id, Name = entity.Name, Price = entity.Price };
            model.ImageUrl = (entity.Images.Count() > 0) ? entity.Images.First().Url : "/images/products/nophoto.png";
            model.AlternateImageUrl = (entity.Images.Count() > 1) ? entity.Images.Skip(1).First().Url : "/images/products/nophoto.png";
            return model;
        }

        public static ProductModel ToModel(this Product entity)
        {
            ProductModel p = new ProductModel();
            p.Id = entity.Id;
            p.Name = entity.Name;
            p.Price = entity.Price;
            
            p.SubCategoryId = entity.SubCategory.Id;
            p.DepartmentId = entity.SubCategory.Category.Department.Id;
            p.CategoryId = entity.SubCategory.Category.Id;

            p.Price = entity.Price;
            p.Description = entity.Description;
            p.LaunchingDate = entity.LaunchingDate;
            p.FabricId = entity.Fabric.Id;


            foreach (var color in entity.ColorsOffered)
            {
                p.ColorsOfferedId.Add(color.Id);
            }

            foreach (var size in entity.SizesOffered)
            {
                p.SizesOfferedId.Add(size.Id);

            }
            foreach (var Image in entity.Images)
            {
                p.ProductImages.Add(Image.ToModel());
            }

            return p;
  
        }

        public static List<ProductDetailsModel> ToDetailsModelList(this List<Product> entityList)
        {
            List<ProductDetailsModel> modelList = new List<ProductDetailsModel>();
            foreach (var entity in entityList)
            {
                modelList.Add(entity.ToDetailsModel());
            }
            modelList.TrimExcess();
            return modelList;
        }

        public static ProductDetailsModel ToDetailsModel(this Product entity)
        {
            ProductDetailsModel p = new ProductDetailsModel();
            p.ID = entity.Id;
            p.Name = entity.Name;
            p.Price = entity.Price;

          
            p.Price = entity.Price;
            p.Discription = entity.Description;
            p.LaunchingDate = entity.LaunchingDate;
            p.Fabric = entity.Fabric.ToModel();


            foreach (var color in entity.ColorsOffered)
            {
                p.ColorList.Add(color.ToModel());
            }

            foreach (var size in entity.SizesOffered)
            {
                p.SizeList.Add(size.ToModel());

            }
            foreach (var Image in entity.Images)
            {
                p.ProductImg.Add(Image.ToModel());
            }

            return p;

        }

        public static ImagesModel ToModel(this ProductImage entity)
        {
            return new ImagesModel { Id = entity.Id, Caption= entity.Caption, Priority=entity.Priority, Url = entity.Url };
        }


        public static List<ProductSummaryModel> ToSummaryModelList(this List<Product> entityList)
        {
            List<ProductSummaryModel> modelList = new List<ProductSummaryModel>();
            foreach (var entity in entityList)
            {
                modelList.Add(entity.ToSummaryModel());
            }
            modelList.TrimExcess();
            return modelList;
        }

        
    }
}