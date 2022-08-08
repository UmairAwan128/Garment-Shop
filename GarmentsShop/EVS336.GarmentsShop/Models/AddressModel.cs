using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models
{
    public class AddressModel
    {
        public AddressModel() {
            City = new CityModel();
        }
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public CityModel City { get; set; }
    }
}