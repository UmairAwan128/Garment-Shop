using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string LoginId { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

    }
}