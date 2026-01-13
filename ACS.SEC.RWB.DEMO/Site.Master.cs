using System;

namespace ACS.SEC.RWB.DEMO
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var isAuth = Context.User != null
                         && Context.User.Identity != null
                         && Context.User.Identity.IsAuthenticated;

            phNav.Visible = isAuth;

            if (isAuth)
                litUserName.Text = Server.HtmlEncode(Context.User.Identity.Name);
        }
    }
}