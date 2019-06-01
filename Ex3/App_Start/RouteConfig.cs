/*By: Shany-yael Dagan 307894899, Racheli Copperman 315597575 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /* default route */
            routes.MapRoute("Default", url: "{action}/{ip}/{port}/{time}/{total}/{name}", //and here the second line to the default option - choose which, can be one of the other options. so wont collapse when running automatically 
                defaults: new { controller = "First", action = "OpenView", ip = "127.0.0.1", port = "5405", time = -1, total = 0, name = "" });
        }
    }
}
