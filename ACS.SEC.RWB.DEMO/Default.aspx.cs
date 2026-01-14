using System;
using System.Web.UI;

namespace ACS.SEC.RWB.DEMO
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) 
                return;
            Response.Redirect("~/Pages/ProductionStatus.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}