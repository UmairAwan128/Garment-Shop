using DataBase_APIService.Models;
using DataBaseEntityClasses.GarmentsShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataBase_APIService.Controllers
{
    public class SizesServiceController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                //as in method we are not geetinf parameter other way would be use queryString
                //to get the parameter and its data passed in the URL while calling as..
                var data = Request.GetQueryNameValuePairs();
                if (data.Count() == 0) //if no parameter passed in URL return all Records
                {
                    List<SizesModel> modelList = new GarmentsHandler().GetSizes().ToModelList();
                    return Ok(modelList);
                }
                if (data.Count() == 1)
                {
                    switch (data.First().Key)
                    {
                        case "id":
                            int Id = Convert.ToInt32(data.First().Value);
                            if (Id <= 0) return BadRequest("id must be a positive number");
                            return Ok(new GarmentsHandler().GetSize(Id).ToModel());

                        default:
                            return BadRequest("invalid parameter in query string");
                    }
                }
                return BadRequest("too many parameters in query string");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
