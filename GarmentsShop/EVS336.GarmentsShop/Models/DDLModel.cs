using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EVS336.GarmentsShop.Models
{
    public class DDLModel
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public string GlyphIcon { get; set; }

        public IEnumerable<SelectListItem> Values { get; set; }
    }
}