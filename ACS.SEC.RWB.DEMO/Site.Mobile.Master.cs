using System;

namespace ACS.SEC.RWB.DEMO
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var isAuth = Context.User != null
                         && Context.User.Identity != null
                         && Context.User.Identity.IsAuthenticated;

            phNavMobile.Visible = isAuth;

            if (isAuth)
                litUserNameMobile.Text = Server.HtmlEncode(Context.User.Identity.Name);
        }
    }
}