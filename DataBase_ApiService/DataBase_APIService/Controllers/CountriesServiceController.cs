using DataBase_APIService.Models;

using DataBaseEntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataBase_APIService.Controllers
{
    //WebApi can only have one Get/Post/Put/Delete Method other can be only its ovverride 

    [EnableCors("*", "*", "*")]
    public class CountriesServiceController : ApiController
    {

        // this one Get method is created in such a way that if it can be passed either code/Id for
        // specific country and if no parameter passed then all records will be returned..
        //for testing Get of this API Application either use  "Fiddler" or enter url in browser
        //to access the data in this way..e.g..
        //Get all:   /services/CountriesService
        //Get by id:   /services/CountriesService/?id=1
        //Get by code:  /services/CountriesService/?code=92
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
                    List<CountryModel> modelList = new LocationsHandler().GetCountries().ToModelList();
                    return Ok(modelList);
                }
                if (data.Count() == 1)
                {
                    switch (data.First().Key)
                    {
                        case "id":
                            int countryId = Convert.ToInt32(data.First().Value);
                            if (countryId <= 0) return BadRequest("id must be a positive number");
                            return Ok(new LocationsHandler().GetCountry(countryId).ToModel());
                        case "code":
                            int countryCode = Convert.ToInt32(data.First().Value);
                            if (countryCode <= 0) return BadRequest("code must be from 1 to 176");
                            var result = from c in new LocationsHandler().GetCountries()
                                         where c.Code == countryCode
                                         select c;
                            if (result != null)
                            {
                                return Ok(result.First().ToModel());
                            }
                            return NotFound();
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

 
        

        // or either create an override for the above method as

        //[HttpGet]
        //public IHttpActionResult Get(int id)
        //{
        //    try
        //    {
        //        CountryModel country = new LocationsHandler().GetCountry(id).ToModel();
        //        return Ok(country);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        [HttpPost]
        public IHttpActionResult Post(CountryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name)) return BadRequest("invalid JSON data");
                new LocationsHandler().AddCountry(model.ToEntity());
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult Put(CountryModel country)
        {
            try
            {
                if (country.Id <= 0) return BadRequest("id can't be negative");
                new LocationsHandler().UpdateCountry(country.Id, country.ToEntity());
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[DisableCors]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("id can't be negative");
                new LocationsHandler().DeleteCountry(new Country { Id = id });
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
