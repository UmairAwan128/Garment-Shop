using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public virtual CityModel City { get; set; }
    }
}