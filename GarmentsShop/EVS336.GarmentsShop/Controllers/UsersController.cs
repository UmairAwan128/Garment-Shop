using EVS336.GarmentsShop.Models;
using EVS336.GarmentsShop.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EVS336.GarmentsShop.Controllers
{
    public class UsersController : Controller
    {
        string apiUrl, apiUrlRole,apiUrlCountry,apiUrlProvince, apiUrlCity;
        HttpClient client;  //for this we Added "Microsoft.Net.Http" Package from Nuget Package Manager

        public UsersController() {
            apiUrl = WebUtil.LocalHost + "services/UsersService/";   //defined name of localHost
            apiUrlRole = WebUtil.LocalHost + "/services/RolesService/";
            apiUrlCountry = WebUtil.LocalHost + "services/CountriesService/";
            apiUrlProvince = WebUtil.LocalHost + "services/ProvinceService/";
            apiUrlCity = WebUtil.LocalHost + "services/CitiesService/";

            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        } 
        
        [HttpGet]
        //public ActionResult Login(string rurl)
        public async Task<ActionResult> Login()
        {
            string qsData= Request.QueryString["rurl"];
            ViewBag.ReturnUrl = qsData;

            HttpCookie myCookie = Request.Cookies[WebUtil.MY_COOKIE];

            if (myCookie != null)
            {
                myCookie.Expires = DateTime.Today.AddDays(3);
                Response.SetCookie(myCookie);

                string[] loginData = myCookie.Value.Split(',');
               
                HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + $"?loginid={loginData[0]}&password={loginData[1]}");
                UserSessionModel currentUser = new UserSessionModel();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    currentUser = JsonConvert.DeserializeObject<UserSessionModel>(responseData);
                }

                if (currentUser != null)
                {
                    Session.Add(WebUtil.CURRENT_USER, currentUser);
                    if (qsData != null)
                    {
                        string[] qsParts = qsData.Split(',');
                        if (qsParts.Length == 2)
                        {
                            return RedirectToAction(qsParts[0], qsParts[1]);
                        }
                    }
                    if (currentUser.RoleId == WebUtil.ADMIN_ROLE)
                    {
                        return RedirectToAction("admin", "home");
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + $"?loginid={model.LoginId}&password={model.Password}");
            UserSessionModel currentUser = new UserSessionModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                currentUser = JsonConvert.DeserializeObject<UserSessionModel>(responseData);
            }
            
            if (model.LoginAttemptNo != 3)
            {
                ViewBag.LoginAttempts = (model.LoginAttemptNo) + 1;
            }
            if (model.LoginAttemptNo == 3)
            {
                return RedirectToAction("RecoverPassword");
            }

            if (currentUser != null)
            {
                Session.Add(WebUtil.CURRENT_USER, currentUser);

                if (model.RememberMe)
                {
                    if (Request.Browser.Cookies) //if cookies in client browser are enabled then true so get in this if{ }
                    {
                        HttpCookie c = new HttpCookie(WebUtil.MY_COOKIE);
                        c.Expires = DateTime.Today.AddDays(3);
                        c.Value = $"{currentUser.LoginId},{currentUser.Password}";
                        Response.SetCookie(c);
                    }
                }

                string qsData = Request.QueryString["ReturnUrl"];
                if (qsData != null)
                {
                    string[] qsParts = qsData.Split(',');
                    if (qsParts.Length == 2)
                    {
                        return RedirectToAction(qsParts[0],qsParts[1]);
                    }
                }


                if (currentUser.RoleId == WebUtil.ADMIN_ROLE)
                {
                    return RedirectToAction("admin", "home");
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            HttpCookie temp = Request.Cookies[WebUtil.MY_COOKIE];
            if (temp != null)
            {
                temp.Expires = DateTime.Now;
                Response.SetCookie(temp);
            }
            return RedirectToAction("login");
        }



        [HttpGet]
        //public ActionResult Login(string rurl)
        public async Task<ActionResult> SignUp()
        {

            try {
                List<RolesModel> rolesModelList = new List<RolesModel>();
                HttpResponseMessage responseMessage = await client.GetAsync(apiUrlRole);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    rolesModelList = JsonConvert.DeserializeObject<List<RolesModel>>(responseData);
                }
                ViewBag.rolesList = rolesModelList.ToSelectItemList();


                List<CountryModel> countryModelList = new List<CountryModel>();
                HttpResponseMessage responseMessage1 = await client.GetAsync(apiUrlCountry);
                if (responseMessage1.IsSuccessStatusCode)
                {
                    var responseData = responseMessage1.Content.ReadAsStringAsync().Result;
                    countryModelList = JsonConvert.DeserializeObject<List<CountryModel>>(responseData);
                }
                ViewBag.CountriesList = countryModelList.ToSelectItemList();
            }
            catch (Exception ex) {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);

            }
            return PartialView("~/Views/Users/SignUp.cshtml");
        }

        [HttpPost]

        public async Task<ActionResult> SignUp(FormCollection data)
        {
            
            SignUpModel p = new SignUpModel();
            try
            {
                p.LoginId = data["LoginId"];
                p.Password = data["Password"];
                p.Name = data["Name"];
                p.Email = data["Email"];
                p.ContactNumber = data["ContactNumber"];
                p.SecurityQuestion = data["SecurityQuestion"];
                p.SecurityAnswer = data["SecurityAnswer"];

                p.Address.StreetAddress = data["StreetAddress"];
                p.Role.Id = Convert.ToInt32(data["Role"]);
                p.Address.City.Id = Convert.ToInt32(data["Cities"]);
                p.BirthDate = Convert.ToDateTime(data["BirthDate"]);

                HttpResponseMessage responseMessage = await client.PostAsJsonAsync<SignUpModel>(apiUrl, p);
                if (responseMessage.IsSuccessStatusCode)
                {
                    HttpResponseMessage responseMessage2 = await client.GetAsync(apiUrl + $"?loginid={p.LoginId}&password={p.Password}");
                    UserSessionModel currentUser = new UserSessionModel();
                    if (responseMessage2.IsSuccessStatusCode)
                    {
                        var responseData = responseMessage2.Content.ReadAsStringAsync().Result;
                        currentUser = JsonConvert.DeserializeObject<UserSessionModel>(responseData);
                    }
             
                    if (currentUser != null)
                    {
                        Session.Add(WebUtil.CURRENT_USER, currentUser);

                    }
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

            return RedirectToAction("index", "home");

        }


        [HttpGet]
        public async Task<ActionResult> CitiesDDL(int Id)
        {
            DDLModel model = new DDLModel { Name = "Cities", Caption = "- Select City -", GlyphIcon = "glyphicon-tags" };
            List<CityModel> modelList = new List<CityModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlCity + $"?Parent={Id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<CityModel>>(responseData);
            }
            model.Values = modelList.ToSelectItemList();
            return PartialView("~/Views/Shared/_DDLView.cshtml", model);
        }

        public async Task<ActionResult> ProvincesDDL(int Id)
        {
            DDLModel model = new DDLModel { Name = "Province", Caption = "- Province -", GlyphIcon = "glyphicon-tags" };
            List<ProvinceModel> modelList = new List<ProvinceModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlProvince + $"?Parent={Id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<ProvinceModel>>(responseData);
            }

            model.Values = modelList.ToSelectItemList();
            return PartialView("~/Views/Shared/_DDLView.cshtml", model);
        }

    }
}