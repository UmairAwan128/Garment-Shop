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
    public class ProductPaginationController : ApiController
    {
        //this action will return table/data of Product Table according to PageNo passed to it
        // Pagination: Let say we have created our project and we have added thousands of products in it now in the View of ProductTable
        //i.e when we call to show/get all the products it will take very long time to get all products and then show them in the table
        //also in this case the ProductTable will be very lengthy so the solution is pagination i.e so request to get only first few records
        //say first 10 so it will only get first 10 and show them we will do this as by telling the controler a pageNo and accroding to this pageNo
        //the next 10 records are differntiated i.e say we again request it to get the next 10 records so now it will skip first 10 and take
        //next 10 records and this time we will be passed pageNo=2 so we gotted that skip first 10 retrieve the next 10 record ..

        //called as
        //http://localhost:56018/services/UsersService/?loginid=Umair&password=123
        public IHttpActionResult Get(string search, int? pageNo)
        {
            ProductSearchViewModel model = new ProductSearchViewModel();

            try
            {
                model.searchTerm = search;
                pageNo = (pageNo.HasValue && pageNo.Value > 0) ? pageNo : 1; //here we are using conditionalOperator i.e if(pageNo Has a Value and its > 0){ then assign it that value} else{ i.e not passed so assign it 1 } 
                                                                             // pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo : 1 : 1;  //is same as above but it has nexted if so two seperate if and two else bot have 1 
                //now as the View(ProductTable.cshtml) will be calling this Action which has two buttons previous,Next page but how can he identify that currently we are on which page
                //so for this we will pass this pageNo to View by putting it in the Model class so we will assign the pageNo-1 value to Previous and pageNo+1 to Next Button so 

                //for simple pagination
                //model.PageNo = pageNo.Value;

                var totalRecords = new GarmentsHandler().GetProductsCount(search);

                model.products = (new GarmentsHandler().GetProducts(search, pageNo.Value)).ToSummaryModelList(); //get the products from product table according to this pageNo 

                if (model.products != null)
                {//if there we have any products in Db then do this

                    model.pager = new Pager(totalRecords, pageNo, 3);
                    return Ok(model);
                }
             
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(model);
        }

    }
}
