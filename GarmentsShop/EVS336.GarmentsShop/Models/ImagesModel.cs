using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop.Models
{
    public class ImagesModel
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public int Priority { get; set; }
        public string Url { get; set; }
    }
}