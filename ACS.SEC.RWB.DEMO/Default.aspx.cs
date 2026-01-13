using System;
using System.Web.UI;

namespace ACS.SEC.RWB.DEMO
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/EquipState", true);
        }
    }
}