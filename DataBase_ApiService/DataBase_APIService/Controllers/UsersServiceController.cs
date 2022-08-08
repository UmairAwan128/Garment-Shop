using DataBase_APIService.Models;
using DataBaseEntityClasses.GarmentsShop;
using DataBaseEntityClasses.UsersMgt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DataBase_APIService.Controllers
{
    public class UsersServiceController : ApiController
    {
        //called as
        //http://localhost:56018/services/UsersService/?loginid=Umair&password=123
        public IHttpActionResult Get(string loginid, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginid) || string.IsNullOrWhiteSpace(password))
                    return BadRequest("invalid JSON data");
                UserModel model = new UsersHandler().GetUser(loginid, password).ToModel();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult Post(SignUpModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name)) return BadRequest("invalid JSON data");
                new UsersHandler().AddUser(model.ToEntity());
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


    }
}