using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class ProvinceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CountryModel Country { get; set; }

    }
}