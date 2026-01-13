using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace ACS.SEC.RWB.DEMO
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}