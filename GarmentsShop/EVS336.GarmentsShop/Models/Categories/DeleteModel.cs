using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EVS336.GarmentsShop.Models.Categories
{
    public class DeleteModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }

    }
}