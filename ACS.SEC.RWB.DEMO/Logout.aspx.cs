using System;
using System.Threading.Tasks;
using System.Web.Security;
using ACS.SEC.RWB.DEMO.CIPLib;

namespace ACS.SEC.RWB.DEMO
{
    public partial class Logout : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["confirm"] != "1")
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            await DemoHelpers.ProcessLogoutAsync(Session);

            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}