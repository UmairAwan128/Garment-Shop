using DataBase_APIService.Models;
using DataBaseEntityClasses.GarmentsShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataBase_APIService.Controllers
{
    [EnableCors("*", "*", "*")]
    public class CategoriesServiceController : ApiController
    {

        //Get all:   /services/CategoriesService
        //Get by id:   /services/CategoriesService/?id=1
        //Get by code:  /services/CategoriesService/?code=92

        public IHttpActionResult Get()
        {
            try
            {
                //as in method we are not geetinf parameter other way would be use queryString
                //to get the parameter and its data passed in the URL while calling as..
                var data = Request.GetQueryNameValuePairs();
                if (data.Count() == 0) //if no parameter passed in URL return all Records
                {
                    List<CategoryModel> modelList = new GarmentsHandler().GetCategories().ToModelList();
                    return Ok(modelList);
                }
                if (data.Count() == 1)
                {
                    switch (data.First().Key)
                    {
                        case "id":
                            int Id = Convert.ToInt32(data.First().Value);
                            if (Id <= 0) return BadRequest("id must be a positive number");

                            return Ok(new GarmentsHandler().GetCategory(Id).ToModel());
                        case "Parent":  
                            try
                            {
                                int DepId = Convert.ToInt32(data.First().Value);
                                List<CategoryModel> modelList = new GarmentsHandler().GetCategories(DepId).ToModelList();
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
        

        [HttpPost]
        public IHttpActionResult Post(CategoryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name)) return BadRequest("invalid JSON data");
                new GarmentsHandler().AddCategory(model.ToEntity());  ////
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        [HttpPut]
        public IHttpActionResult Put(CategoryModel model)
        {
            Category c = new Category();
            try
            {
                c.Name = model.Name;
                c.Department = new Department { Id = model.Parent };
                new GarmentsHandler().UpdateCategory(model.Id, c);
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
                new GarmentsHandler().DeleteCategory(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
