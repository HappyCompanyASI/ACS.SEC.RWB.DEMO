using System;
using System.Configuration;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using ACS.SEC.RWB.DEMO.CIPLib;

namespace ACS.SEC.RWB.DEMO
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e) 
            => DemoHelpers.OnApplicationStart();

        protected void Application_End(object sender, EventArgs e) 
            => DemoHelpers.OnApplicationEnd();
    }
}