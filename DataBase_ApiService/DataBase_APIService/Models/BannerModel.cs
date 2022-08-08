using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBase_APIService.Models
{
    public class BannerModel
    {
        public int Id { get; set; }

        public string Heading { get; set; }

        public string SubHeading { get; set; }

        public string ImageUrl { get; set; }

    }
}