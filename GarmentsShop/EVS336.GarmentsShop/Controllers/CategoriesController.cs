using EVS336.GarmentsShop;
using EVS336.GarmentsShop.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVS336.GarmentsShop.Models;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using EVS336.GarmentsShop.Models.Users;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EVS336.GarmentsShop.Controllers
{
    public class CategoriesController : Controller
    {
        string apiUrl,apiUrlDepartment,apiUrlSubCategories;
        HttpClient client;  //for this we Added "Microsoft.Net.Http" Package from Nuget Package Manager

        public CategoriesController() {
            //apiUrl = "http://localhost:56018/services/CategoriesService";  //below line and this both are same
            apiUrl = WebUtil.LocalHost + "services/CategoriesService/";   //defined name of localHost
            //in WebUtil so just change there and every where in project will be changed automatically 
            apiUrlDepartment = WebUtil.LocalHost + "services/DepartmentService/";   //defined name of localHost
            apiUrlSubCategories = WebUtil.LocalHost + "services/SubCategoriesService/";   //defined name of localHost

            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<ActionResult> manage()
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!( (user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,categories" });

            List<CategoryModel> modelList = new List<CategoryModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<CategoryModel>>(responseData);
            }
            
            return View(modelList);
        }
        
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,categories" });

            List<DepartmentModel> modelList = new List<DepartmentModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlDepartment);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<DepartmentModel>>(responseData);
            }

            ViewBag.ParentCategories = modelList.ToSelectItemList();
            return PartialView("~/Views/Categories/_Create.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection data)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                  return RedirectToAction("login", "users", new { rurl = "manage,categories" });

            CategoryModel c = new CategoryModel();
            try
            {
                c.Name = data["name"];
                c.Parent = data["parent"];
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync<CategoryModel>(apiUrl, c);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData.Add("AlertMessage", new AlertModel($"{c.Name} added successfully", AlertModel.AlertType.Success));
                }
                else {
                    TempData.Add("AlertMessage", new AlertModel($"Failed to add {c.Name}", AlertModel.AlertType.Error));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();
                
            }
            return RedirectToAction("manage");
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,categories" });

            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + $"?id={id}");
            CategoryModel model = new CategoryModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<CategoryModel>(responseData);
            }

            List<DepartmentModel> modelList = new List<DepartmentModel>();
            responseMessage = await client.GetAsync(apiUrlDepartment);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<DepartmentModel>>(responseData);
            }
            ViewBag.ParentCategories = modelList.ToSelectItemList();

            return PartialView("~/Views/Categories/_Edit.cshtml",model);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(CategoryModel model)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,categories" });
            try
            {
                HttpResponseMessage responseMessage = await client.PutAsJsonAsync<CategoryModel>(apiUrl, model);
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData.Add("AlertMessage", new AlertModel($"{model.Name} updated successfully", AlertModel.AlertType.Success));
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

                TempData.Add("AlertMessage", new AlertModel($"Failed to update {model.Name}", AlertModel.AlertType.Error));
            }
            return RedirectToAction("manage");
        }


        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,categories" });

            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + $"?id={id}");
            CategoryModel model = new CategoryModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<CategoryModel>(responseData);
            }

            DeleteModel m = new DeleteModel { Id = model.Id, Name = model.Name, Parent = model.Parent };
            return PartialView("~/Views/Categories/_Delete.cshtml", m);
        }


        [HttpPost]
        public async Task<ActionResult> Delete(DeleteModel model)
        {
            UserSessionModel user = Session[WebUtil.CURRENT_USER] as UserSessionModel;
            if (!((user != null) && (user.RoleId == (WebUtil.ADMIN_ROLE))))
                return RedirectToAction("login", "users", new { rurl = "manage,categories" });
            try
            {
                HttpResponseMessage responseMessage = await client.DeleteAsync(apiUrl + $"?id={model.Id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    TempData.Add("AlertMessage", new AlertModel($"{model.Name} deleted successfully", AlertModel.AlertType.Success));
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

                TempData.Add("AlertMessage", new AlertModel($"Failed to delete {model.Name}", AlertModel.AlertType.Error));
            }
            return RedirectToAction("manage");
        }

        [HttpGet]
        public async Task<ActionResult> Level2DDL(int id)
        {
            //first pass id pass id to DepartmentAPi to get Department then pass that department
            // to categoriesApi to get all categories for that dep 
            //here we needto convert it to SelectItemList

            DDLModel model = new DDLModel { Name="Categories", Caption="- Categories -", GlyphIcon= "glyphicon-tags" };
            List<CategoryModel> modelList = new List<CategoryModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl+ $"?Parent={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<CategoryModel>>(responseData);
            }

            model.Values = modelList.ToSelectItemList();
            return PartialView("~/Views/Shared/_DDLView.cshtml", model);
        }

        [HttpGet]
        public async Task<ActionResult> Level3DDL(int id)
        {
            DDLModel model = new DDLModel { Name = "SubCategories", Caption = "- Sub Categories -", GlyphIcon = "glyphicon-tags" };
            List<SubCategoryModel> modelList = new List<SubCategoryModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlSubCategories + $"?CategoryId={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<SubCategoryModel>>(responseData);
            }
            model.Values = modelList.ToSelectItemList();
            return PartialView("~/Views/Shared/_DDLView.cshtml", model);
        }
        
    }
}