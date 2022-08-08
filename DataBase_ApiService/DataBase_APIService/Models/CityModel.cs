using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class CityModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ProvinceModel Province { get; set; }

    }
}