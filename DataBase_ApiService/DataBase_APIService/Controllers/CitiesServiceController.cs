using DataBase_APIService.Models;
using DataBaseEntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataBase_APIService.Controllers
{
    public class CitiesServiceController : ApiController
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
                    List<CityModel> modelList = new LocationsHandler().GetCities().ToModelList();
                    return Ok(modelList);
                }
                if (data.Count() == 1)
                {
                    switch (data.First().Key)
                    {
                        case "id":
                            int cityId = Convert.ToInt32(data.First().Value);
                            if (cityId <= 0) return BadRequest("id must be a positive number");
                            return Ok(new LocationsHandler().GetCity(cityId).ToModel());
                        case "Parent":
                            try
                            {
                                int provinceId = Convert.ToInt32(data.First().Value);
                                List<CityModel> modelList = new LocationsHandler().GetCities(provinceId).ToModelList();
                                return Ok(modelList);
                            }
                            catch (Exception ex)
                            {
                                return InternalServerError(ex);
                            }
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
