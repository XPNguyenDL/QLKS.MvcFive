using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLKS.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute(
            //    name: "HotelInfo",
            //    url: "khach-san-{hotelName}",
            //    defaults: new
            //    {
            //        controller = "HotelInfo",
            //        action = "Index",
            //        id = UrlParameter.Optional
            //    },
            //    namespaces: new[] { "QLKS.WebApp.Controllers" }
            //);
            //routes.MapRoute(
            //    name: "ApiHotelInfoApi",
            //    url: "khach-san-{id}/{controller}/{action}",
            //    defaults: new
            //    {
            //        controller = "HotelInfo",
            //        action = "Blog",
            //        id = UrlParameter.Optional
            //    },
            //    namespaces: new[] { "QLKS.WebApp.Controllers" }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "QLKS.WebApp.Controllers" }
            );
        }
    }
}
