using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EVS336.GarmentsShop.Models;
using EVS336.GarmentsShop.Models.Categories;
using EVS336.GarmentsShop.Models.Products;
using EVS336.GarmentsShop.Models.Users;
using Newtonsoft.Json;

namespace EVS336.GarmentsShop.Controllers
{
    public class ProductsController : Controller
    {
        string apiUrl, apiUrlPagination, apiUrlSubCategories, apiUrlCategory, apiUrlDepartment, apiUrlColors, apiUrlSize, apiUrlFabrics;
        HttpClient client;  //for this we Added "Microsoft.Net.Http" Package from Nuget Package Manager

        public ProductsController()
        {
            apiUrlSubCategories = WebUtil.LocalHost + "services/SubCategoriesService/";   //defined name of localHost
            apiUrlCategory = WebUtil.LocalHost + "services/CategoriesService/";
            apiUrl = WebUtil.LocalHost + "services/ProductsService/";   //defined name of localHost
            apiUrlDepartment = WebUtil.LocalHost + "services/DepartmentService/";   //defined name of localHost
            apiUrlColors = WebUtil.LocalHost + "services/ColorsServices/";   //defined name of localHost
            apiUrlFabrics = WebUtil.LocalHost + "services/FabricsService/";   //defined name of localHost
            apiUrlSize = WebUtil.LocalHost + "services/SizesService/";   //defined name of localHost
            apiUrlPagination = WebUtil.LocalHost + "services/ProductPagination/";
            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<ActionResult> Manage()
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,products" });

            ProductSearchViewModel model = new ProductSearchViewModel();
            
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                model.products = JsonConvert.DeserializeObject<List<ProductSummaryModel>>(responseData);
            }
            return View(model);
        }

        public async Task<ActionResult> ProductTable(string search, int? pageNo)
        {  // we set pageNo as nullable so that we can also call this action without passing pageNo also 

            ProductSearchViewModel model = new ProductSearchViewModel();
            try {
                HttpResponseMessage responseMessage = await client.GetAsync(apiUrlPagination + $"?search={search}&pageNo={pageNo}");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<ProductSearchViewModel>(responseData);
                }
            }
            catch (Exception ex) {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();
            }
            return PartialView("~/Views/Products/_ProductTable.cshtml",model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,products" });

            List<DepartmentModel> DepmodelList = new List<DepartmentModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlDepartment);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                DepmodelList = JsonConvert.DeserializeObject<List<DepartmentModel>>(responseData);
            }
            ViewBag.DepartmentList = DepmodelList.ToSelectItemList();

            List<FabricsModel> fabmodelList = new List<FabricsModel>();
            HttpResponseMessage responseMessagefab = await client.GetAsync(apiUrlFabrics);
            if (responseMessagefab.IsSuccessStatusCode)
            {
                var responseData = responseMessagefab.Content.ReadAsStringAsync().Result;
                fabmodelList = JsonConvert.DeserializeObject<List<FabricsModel>>(responseData);
            }
            ViewBag.FabricsList = fabmodelList.ToSelectItemList();

            List<ColorsModel> colorsmodelList = new List<ColorsModel>();
            HttpResponseMessage responseMessagecolors = await client.GetAsync(apiUrlColors);
            if (responseMessagecolors.IsSuccessStatusCode)
            {
                var responseData = responseMessagecolors.Content.ReadAsStringAsync().Result;
                colorsmodelList = JsonConvert.DeserializeObject<List<ColorsModel>>(responseData);
            }
            ViewBag.ColorsList = colorsmodelList.ToSelectItemList();

            List<SizesModel> sizesmodelList = new List<SizesModel>();
            HttpResponseMessage responseMessagesizes = await client.GetAsync(apiUrlSize);
            if (responseMessagesizes.IsSuccessStatusCode)
            {
                var responseData = responseMessagesizes.Content.ReadAsStringAsync().Result;
                sizesmodelList = JsonConvert.DeserializeObject<List<SizesModel>>(responseData);
            }
            ViewBag.SizesList = sizesmodelList.ToSelectItemList();

            return PartialView("~/Views/Products/_Create.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection data)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,products" });

            CreateProductModel p = new CreateProductModel();
            try
            {
                p.SubCategoryId = Convert.ToInt32(data["SubCategories"]) ;
                p.Name = data["Name"];
                p.Price = Convert.ToSingle(data["Price"]);
                p.LaunchingDate = Convert.ToDateTime(data["LaunchDate"]);

                
                p.FabricId = Convert.ToInt32(data["Fabric"]);
                p.Description = data["Description"];

                
                long uno = DateTime.Now.Ticks;
                int counter = 0;
                foreach (string fcName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fcName];
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                    {
                        string url = $"/DataImages/Products/{uno}_{++counter}{file.FileName.Substring(file.FileName.LastIndexOf("."))}";
                        string path = Request.MapPath(url);
                        file.SaveAs(path);
                        //Add here images to Images Table directly using imageServiceController
                        p.ProductImages.Add(new ImagesModel { Url = url, Priority = counter });
                        }   
                }

                var sizeIdList = data["Sizes"].Split(',');
                foreach (var sizeId in sizeIdList)
                {
                    p.SizesOfferedId.Add(Convert.ToInt32(sizeId));
                }

                var colorIdList = data["Colors"].Split(',');
                foreach (var colorId in colorIdList)
                {
                    p.ColorsOfferedId.Add(Convert.ToInt32(colorId));
                }


                HttpResponseMessage responseMessage = await client.PostAsJsonAsync<CreateProductModel>(apiUrl, p);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData.Add("AlertMessage", new AlertModel($"{p.Name} added successfully", AlertModel.AlertType.Success));
                }
                else
                {
                    TempData.Add("AlertMessage", new AlertModel($"Failed to add {p.Name}", AlertModel.AlertType.Error));
                }
            
            }
            catch(Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();
                TempData.Add("AlertMessage", new AlertModel($"Failed to add {p.Name}", AlertModel.AlertType.Error));
            }
            return RedirectToAction("manage");
        }



        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,Products" });

            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + $"?id={id}");
            ProductModel model = new ProductModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<ProductModel>(responseData);
            }

            List<DepartmentModel> DepmodelList = new List<DepartmentModel>();
            HttpResponseMessage responseMessageDep = await client.GetAsync(apiUrlDepartment);
            if (responseMessageDep.IsSuccessStatusCode)
            {
                var responseData = responseMessageDep.Content.ReadAsStringAsync().Result;
                DepmodelList = JsonConvert.DeserializeObject<List<DepartmentModel>>(responseData);
            }
            ViewBag.DepartmentList = DepmodelList.ToSelectItemList();


            List<CategoryModel> modelList = new List<CategoryModel>();
            HttpResponseMessage responseMessageCat = await client.GetAsync(apiUrlCategory + $"?Parent={model.DepartmentId}");
            if (responseMessageCat.IsSuccessStatusCode)
            {
                var responseData = responseMessageCat.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<CategoryModel>>(responseData);
            }
            ViewBag.CategoryList = modelList.ToSelectItemList();

            List<SubCategoryModel> modelListSubCat = new List<SubCategoryModel>();
            HttpResponseMessage responseMessageSubCat = await client.GetAsync(apiUrlSubCategories + $"?CategoryId={model.CategoryId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessageSubCat.Content.ReadAsStringAsync().Result;
                modelListSubCat = JsonConvert.DeserializeObject<List<SubCategoryModel>>(responseData);
            }
            ViewBag.SubCategoryList = modelListSubCat.ToSelectItemList();

            List<FabricsModel> fabmodelList = new List<FabricsModel>();
            HttpResponseMessage responseMessagefab = await client.GetAsync(apiUrlFabrics);
            if (responseMessagefab.IsSuccessStatusCode)
            {
                var responseData = responseMessagefab.Content.ReadAsStringAsync().Result;
                fabmodelList = JsonConvert.DeserializeObject<List<FabricsModel>>(responseData);
            }
            ViewBag.FabricsList = fabmodelList.ToSelectItemList();

            List<ColorsModel> colorsmodelList = new List<ColorsModel>();
            HttpResponseMessage responseMessagecolors = await client.GetAsync(apiUrlColors);
            if (responseMessagecolors.IsSuccessStatusCode)
            {
                var responseData = responseMessagecolors.Content.ReadAsStringAsync().Result;
                colorsmodelList = JsonConvert.DeserializeObject<List<ColorsModel>>(responseData);
            }
            ViewBag.ColorsList = colorsmodelList.ToSelectItemList();

            List<SizesModel> sizesmodelList = new List<SizesModel>();
            HttpResponseMessage responseMessagesizes = await client.GetAsync(apiUrlSize);
            if (responseMessagesizes.IsSuccessStatusCode)
            {
                var responseData = responseMessagesizes.Content.ReadAsStringAsync().Result;
                sizesmodelList = JsonConvert.DeserializeObject<List<SizesModel>>(responseData);
            }
            ViewBag.SizesList = sizesmodelList.ToSelectItemList();

            return PartialView("~/Views/Products/_Edit.cshtml", model);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(FormCollection data)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,categories" });
            
            CreateProductModel p = new CreateProductModel();
            try
            {
                p.Id = Convert.ToInt32(data["Id"]);
                p.SubCategoryId = Convert.ToInt32(data["SubCategories"]);
                p.Name = data["Name"];
                p.Price = Convert.ToSingle(data["Price"]);
                p.LaunchingDate = Convert.ToDateTime(data["LaunchDate"]);
                
                p.FabricId = Convert.ToInt32(data["Fabric"]);
                p.Description = data["Description"];
                
                long uno = DateTime.Now.Ticks;
                int counter = 0;
                foreach (string fcName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fcName];
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                    {
                        string url = $"/DataImages/Products/{uno}_{++counter}{file.FileName.Substring(file.FileName.LastIndexOf("."))}";
                        string path = Request.MapPath(url);
                        file.SaveAs(path);
                        //Add here images to Images Table directly using imageServiceController
                        p.ProductImages.Add(new ImagesModel { Url = url, Priority = counter });
                    }
                }

                var sizeIdList = data["Sizes"].Split(',');
                foreach (var sizeId in sizeIdList)
                {
                    p.SizesOfferedId.Add(Convert.ToInt32(sizeId));
                }

                var colorIdList = data["Colors"].Split(',');
                foreach (var colorId in colorIdList)
                {
                    p.ColorsOfferedId.Add(Convert.ToInt32(colorId));
                }


                HttpResponseMessage responseMessage = await client.PutAsJsonAsync<CreateProductModel>(apiUrl, p);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData.Add("AlertMessage", new AlertModel($"{p.Name} updated successfully", AlertModel.AlertType.Success));
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                //Trace.WriteLine($"{c.Id},{c.Name}");
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();

                TempData.Add("AlertMessage", new AlertModel($"Failed to update {p.Name}", AlertModel.AlertType.Error));
            }
            return RedirectToAction("manage");
        }


        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,products" });
            try
            {
                HttpResponseMessage responseMessage = await client.DeleteAsync(apiUrl + $"?id={id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData.Add("AlertMessage", new AlertModel($"product deleted successfully", AlertModel.AlertType.Success));
                }
            }


            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();

                TempData.Add("AlertMessage", new AlertModel($"Failed to delete product", AlertModel.AlertType.Error));
            }
            return RedirectToAction("manage");
        }


        [HttpGet]

        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + $"?id={id}");
            ProductDetailsModel model = new ProductDetailsModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<ProductDetailsModel>(responseData);
            }

            return PartialView("~/Views/Products/_Details.cshtml",model);
        }
        [HttpGet]


        //Why not works
        public async Task<FabricsModel> Fabric(int id)
        {
               FabricsModel fabmodel = new FabricsModel();
             
            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(apiUrlFabrics + $"?id={id}");
                if (responseMessage.IsSuccessStatusCode) //always stop here
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    fabmodel = JsonConvert.DeserializeObject<FabricsModel>(responseData);
                }
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();

                TempData.Add("AlertMessage", new AlertModel($"Failed to delete product", AlertModel.AlertType.Error));
            }
            
            return fabmodel;
        }
        
    }
}