using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EVS336.GarmentsShop
{
    public class WebUtil
    {
        public static readonly int ADMIN_ROLE;
        public static readonly string CURRENT_USER;
        public static readonly string MY_COOKIE;
        public static readonly string LocalHost;
        static WebUtil()
        {
            CURRENT_USER = "CurrentUser";
            ADMIN_ROLE = 1;
            MY_COOKIE = "Info";
            LocalHost = "http://localhost:56018/";
        }
    }
}