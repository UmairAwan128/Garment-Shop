using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models.Users
{
    public class UserSessionModel
    {
        public int Id { get; set; }

        public string LoginId { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

    }
}