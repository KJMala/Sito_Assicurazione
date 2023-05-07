using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RCA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Application["UserOnline"] = 0;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Application["UserOnline"] = Convert.ToInt16(Application["UserOnline"]) + 1;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application["UserOnline"] = Convert.ToInt16(Application["UserOnline"]) - 1;
        }
    }
}
