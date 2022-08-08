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
    public class SubCategoriesServiceController : ApiController
    {
        //no need
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var data = Request.GetQueryNameValuePairs();
                if (data.Count() == 0) //if no parameter passed in URL return all Records
                {
                    List<SubCategoryModel> modelList = new GarmentsHandler().GetSubCategories().ToModelList();
                    return Ok(modelList);
                }
                if (data.Count() == 1)
                {
                    switch (data.First().Key)
                    {
                        case "id":
                            int Id = Convert.ToInt32(data.First().Value);
                            if (Id <= 0) return BadRequest("id must be a positive number");
                            return Ok(new GarmentsHandler().GetSubCategory(Id).ToModel());
                        case "CategoryId":
                            try
                            {
                                int catId = Convert.ToInt32(data.First().Value);
                                List<SubCategoryModel> modelList = new GarmentsHandler().GetSubCategories(catId).ToModelList();
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
