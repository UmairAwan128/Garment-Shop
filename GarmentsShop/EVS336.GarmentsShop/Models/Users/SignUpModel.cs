using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models.Users
{
    public class SignUpModel
    {
        public SignUpModel()
        {
            Address = new AddressModel();
            Role = new RolesModel();
        }
        public int Id { get; set; }

        public string LoginId { get; set; }

        public string Password { get; set; }

        public string ContactNumber { get; set; }

        public AddressModel Address { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public Nullable<DateTime> BirthDate { get; set; }

        public string Name { get; set; }

        public string SecurityQuestion { get; set; }

        public string SecurityAnswer { get; set; }

        public RolesModel Role { get; set; }

    }
}