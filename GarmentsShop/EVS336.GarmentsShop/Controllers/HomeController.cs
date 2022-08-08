using EVS336.GarmentsShop.Models;
using EVS336.GarmentsShop.Models.Categories;
using EVS336.GarmentsShop.Models.Products;
using EVS336.GarmentsShop.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EVS336.GarmentsShop.Controllers
{
    
    public class HomeController : Controller
    {

        string apiUrl, apiUrlCategories, apiUrlSubCategory, apiUrlBanner, apiUrlDepartment, apiUrlProducts, apiUrlOrders;
        HttpClient client;  //for this we Added "Microsoft.Net.Http" Package from Nuget Package Manager

        public HomeController()
        {
            apiUrlDepartment = WebUtil.LocalHost + "services/DepartmentService/";   //defined name of localHost
            apiUrl = WebUtil.LocalHost + "services/ProductsService/";   //defined name of localHost
            apiUrlBanner = WebUtil.LocalHost + "services/BannersService/";
            apiUrlCategories = WebUtil.LocalHost + "services/CategoriesService/";
            apiUrlProducts = WebUtil.LocalHost + "services/ProductsService/";
            apiUrlOrders = WebUtil.LocalHost + "services/OrdersService/";
            apiUrlSubCategory = WebUtil.LocalHost + "services/SubCategoriesService/";
            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {

            List<BannerModel> BannermodelList = new List<BannerModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlBanner);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                BannermodelList = JsonConvert.DeserializeObject<List<BannerModel>>(responseData);
            }
            ViewBag.BannersList = BannermodelList;

            List<DepartmentModel> DepmodelList = new List<DepartmentModel>();
            HttpResponseMessage responseMessage1 = await client.GetAsync(apiUrlDepartment);
            if (responseMessage1.IsSuccessStatusCode)
            {
                var responseData = responseMessage1.Content.ReadAsStringAsync().Result;
                DepmodelList = JsonConvert.DeserializeObject<List<DepartmentModel>>(responseData);
            }
            ViewBag.DepartmentList = DepmodelList;

            
            List<CategoryModel> categorymodelList = new List<CategoryModel>();
            HttpResponseMessage responseMessagecategory = await client.GetAsync(apiUrlCategories);
            if (responseMessagecategory.IsSuccessStatusCode)
            {
                var responseData = responseMessagecategory.Content.ReadAsStringAsync().Result;
                categorymodelList = JsonConvert.DeserializeObject<List<CategoryModel>>(responseData);
            }
            ViewBag.CategoriesName = categorymodelList;

            List<ProductSummaryModel> C1modelList = new List<ProductSummaryModel>();
            List<List<ProductSummaryModel>> productByCatModel = new List<List<ProductSummaryModel>>();
            
            foreach (var cat in categorymodelList)
            {

                HttpResponseMessage responseMessageProductByCat = await client.GetAsync(apiUrl + $"?catId={cat.Id}");
                if (responseMessageProductByCat.IsSuccessStatusCode)
                {
                    var responseData = responseMessageProductByCat.Content.ReadAsStringAsync().Result;
                    C1modelList = JsonConvert.DeserializeObject<List<ProductSummaryModel>>(responseData);
                }
                productByCatModel.Add(C1modelList);
            }
            ViewBag.productByCatModel = productByCatModel;
            
            List<ProductSummaryModel> modelList = new List<ProductSummaryModel>();
            HttpResponseMessage responseMessage2 = await client.GetAsync(apiUrl);
            if (responseMessage2.IsSuccessStatusCode)
            {
                var responseData = responseMessage2.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<ProductSummaryModel>>(responseData);
            }
            return View(modelList);
        }

        [HttpGet]
        public ActionResult Admin()
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!( (user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,categories" });

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetCategoriesList(int id)
        {
            List<CategoryModel> modelList = new List<CategoryModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlCategories + $"?Parent={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<CategoryModel>>(responseData);
            }
            return PartialView("~/Views/Shared/_NavListView.cshtml", modelList);
        }

        [HttpGet]
        public async Task<ActionResult> GetSubCategoriesList(int id)
        {
            List<SubCategoryModel> modelList = new List<SubCategoryModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlSubCategory + $"?CategoryId={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<SubCategoryModel>>(responseData);
            }
            return PartialView("~/Views/Shared/_NavCompListView.cshtml", modelList);
        }


        public async Task<ActionResult> Checkout()
        {
            CheckoutViewModel checkoutViewModel = new CheckoutViewModel();

            var cartProductsCookie = Request.Cookies["CartProducts"];
            if (cartProductsCookie != null)
            {

                checkoutViewModel.CartProductIDs = cartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                //as the CartProductIDs will have duplication so remove them
                List<int> productsOrdered = new List<int>();
                foreach (int id in checkoutViewModel.CartProductIDs) {
                    if (!productsOrdered.Contains(id)){  //if its not in the list then Add it 
                        productsOrdered.Add(id);
                    }
                    //else ignore it
                }

                foreach (var id in productsOrdered) { 
                    HttpResponseMessage responseMessage = await client.GetAsync(apiUrlProducts + $"?id={id}");
                    ProductModel model = new ProductModel();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<ProductModel>(responseData);
                        checkoutViewModel.CartProducts.Add(model);
                    }
                }

            }
            return View(checkoutViewModel);
        }

        //it is the Post 
        public async Task<ActionResult> PostOrder()
        {
            OrderViewModel Model = new OrderViewModel();
            UserSessionModel currentUser = Session[WebUtil.CURRENT_USER] as UserSessionModel;

            var cartProductsCookie = Request.Cookies["CartProducts"];
            if (cartProductsCookie != null)
            {

                var CartProductIDs = cartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                //as the CartProductIDs will have duplication so remove them
                List<int> productsOrdered = new List<int>();
                foreach (int id in CartProductIDs)
                {
                    if (!productsOrdered.Contains(id))
                    {  //if its not in the list then Add it 
                        productsOrdered.Add(id);
                    }
                    //else ignore it
                }

                foreach (var id in productsOrdered)
                {
                    HttpResponseMessage responseMessage = await client.GetAsync(apiUrlProducts + $"?id={id}");
                    ProductModel model = new ProductModel();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<ProductModel>(responseData);
                        Model.CartProducts.Add(model);
                    }
                }

                Model.User = currentUser;
                
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync<OrderViewModel>(apiUrlOrders, Model);
                    if (responseMessage.IsSuccessStatusCode)
                    {

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

                }
            
            }
            else {


            }

            return RedirectToAction("manage");

        }

    }
}