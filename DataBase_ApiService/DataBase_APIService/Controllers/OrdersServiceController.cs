using DataBase_APIService.Models;
using DataBaseEntityClasses.GarmentsShop;
using DataBaseEntityClasses.UsersMgt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataBase_APIService.Controllers
{
    public class OrdersServiceController : ApiController
    {

        [HttpPost]
        public IHttpActionResult Post(OrderViewModel Model)
        {
            try
            {
                if (Model == null) return BadRequest("invalid JSON data");

                new GarmentsHandler().AddOrder(Model.ToEntity());
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
