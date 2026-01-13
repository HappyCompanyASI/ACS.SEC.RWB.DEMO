using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace ACS.SEC.RWB.DEMO
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            //settings.AutoRedirectMode = RedirectMode.Permanent;
            // 將這裡從 Permanent 改為 Off
            settings.AutoRedirectMode = RedirectMode.Off;
            routes.EnableFriendlyUrls(settings);
        }
    }
}
