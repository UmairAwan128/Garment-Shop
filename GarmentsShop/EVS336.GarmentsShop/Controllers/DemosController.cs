using EVS336.GarmentsShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EVS336.GarmentsShop.Controllers
{
    public class DemosController : Controller
    {
        public ActionResult Index()
        {
            //ViewResult 
            //PartialView
            //JsonResult
            //FileResult

            //ViewResult v = new ViewResult();
            //v.ViewName = "/Views/Demos/index.cshtml";
            //return v;

            //ViewData.Add("Message", "It is a test message"); // old
            ViewBag.Message = "It is a test message";


            IndexModel m = new IndexModel
            {
                CategoryName = "Category-1",
                ProductNames = new List<string> { "aaa","bbb","ccc","eee" }
            };

            // controller to controller in case of request forwarding 
            //TempData.Add("Message", "it is a test message"); 
            return View(m);
        }

        
    }
}