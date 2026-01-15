using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.UI;

using ACS.SEC.RWB.DEMO.CIPLib;

namespace ACS.SEC.RWB.DEMO.Pages
{
    public partial class EquipmentStatus : Page
    {
        public class ViewData
        {
         　public int Data1 { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static async Task<ViewData> GetRealtimeData()
        {
            return null;
        }

       
    }
}