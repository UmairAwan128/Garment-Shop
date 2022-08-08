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
    public class ProductsServiceController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                //as in method we are not geetinf parameter other way would be use queryString
                //to get the parameter and its data passed in the URL while calling as..
                var data = Request.GetQueryNameValuePairs();
                if (data.Count() == 0) //if no parameter passed in URL return all Records
                {
                    List<ProductSummaryModel> modelList = new GarmentsHandler().GetProducts().ToSummaryModelList();
                    return Ok(modelList);
                }
                if (data.Count() == 1)
                {
                    switch (data.First().Key)
                    {
                        case "id":
                            int Id = Convert.ToInt32(data.First().Value);
                            if (Id <= 0) return BadRequest("id must be a positive number");
                            return Ok(new GarmentsHandler().GetProduct(Id).ToModel());
                        case "catId":
                            int catId = Convert.ToInt32(data.First().Value);
                            if (catId <= 0) return BadRequest("id must be a positive number");
                            return Ok(new GarmentsHandler().GetProductsbyCategory(catId).ToSummaryModelList());       
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
        public IHttpActionResult Post(ProductModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name)) return BadRequest("invalid JSON data");
                new GarmentsHandler().AddProduct(model.ToEntity());
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPut]
        public IHttpActionResult Put(ProductModel model)
        {
            try
            {
                new GarmentsHandler().UpdateProduct(model.Id, model.ToEntity());
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
                new GarmentsHandler().DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}

