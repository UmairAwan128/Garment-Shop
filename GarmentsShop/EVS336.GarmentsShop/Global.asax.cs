using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EVS336.GarmentsShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            string fname = Server.MapPath("~/logs/errors.log");
            FileStream fs = new FileStream(fname, FileMode.Append, FileAccess.Write, FileShare.Read);
            TextWriterTraceListener mylistner = new TextWriterTraceListener(fs);
            Trace.Listeners.Add(mylistner);
        }
    }
}
