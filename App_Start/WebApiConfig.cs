using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Genuine_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //string origin = "http://localhost:4200";

            //EnableCorsAttribute cors = new EnableCorsAttribute(origin, "accept,content-type,origin,x-my-header", "*");

           // config.EnableCors(cors);

           
            config.Routes.MapHttpRoute(
           name: "MapByAction",
           routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });




        }
    }
}
