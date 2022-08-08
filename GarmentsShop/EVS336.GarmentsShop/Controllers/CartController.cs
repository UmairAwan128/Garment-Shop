using EVS336.GarmentsShop.Models;
using EVS336.GarmentsShop.Models.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace EVS336.GarmentsShop.Controllers
{
    public class CartController : Controller
    {

        string apiUrlProductDetails;
        HttpClient client;  //for this we Added "Microsoft.Net.Http" Package from Nuget Package Manager

        public CartController()
        {
            apiUrlProductDetails = WebUtil.LocalHost + "services/ProductDetails/";   //defined name of localHost
            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrlProductDetails);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        [HttpGet]
        public async Task<ActionResult> CartItem(int id)
        {    
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrlProductDetails + $"?id={id}");
            CartModel model = new CartModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<CartModel>(responseData);
            }

            ViewBag.ProductSel = model;
            ViewBag.ColorsList = model.ColorList.ToSelectItemList();
            ViewBag.SizeList =  model.SizeList.ToSelectItemList();
            return PartialView("~/Views/Home/_CartLayout.cshtml");
        }
    }
}