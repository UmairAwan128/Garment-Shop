using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models
{
    public class CityModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ProvinceModel Province { get; set; }
    }
}