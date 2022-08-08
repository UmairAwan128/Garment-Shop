using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class SignUpModel
    {
        public int Id { get; set; }

        public string LoginId { get; set; }

        public string Password { get; set; }

        public string ContactNumber { get; set; }

        public virtual AddressModel Address { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public Nullable<DateTime> BirthDate { get; set; }

        public string Name { get; set; }

        public string SecurityQuestion { get; set; }

        public string SecurityAnswer { get; set; }

        public virtual RolesModel Role { get; set; }

    }
}