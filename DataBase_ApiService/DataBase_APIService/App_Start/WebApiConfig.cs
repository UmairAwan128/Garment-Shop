using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DataBase_APIService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // we enabled here CrossOriginResourceSharing so our this API/Project data/controller
            //will be accessible in other MVC applications
              config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();
 
            config.Routes.MapHttpRoute(
                name: "oneApi",
                //we edditted here the routeTemplate "api/{controller}/{id}" as "services/{controller}/{id}" 
                routeTemplate: "services/{controller}/{paramOne}"
            );

            config.Routes.MapHttpRoute(
                  name: "twoApi",
                //we edditted here the routeTemplate "api/{controller}/{id}" as "services/{controller}/{id}" 
                routeTemplate: "services/{controller}/{paramOne}/{paramTwo}"
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //we edditted here the routeTemplate "api/{controller}/{id}" as "services/{controller}/{id}" 
                routeTemplate: "services/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
