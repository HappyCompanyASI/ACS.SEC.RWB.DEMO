using System;
using System.Web.Security;

namespace ACS.SEC.RWB.DEMO
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var account = txtAccount.Text.Trim();
            var password = txtPassword.Text;

            var ok = (account == "admin" && password == "123");

            if (!ok)
            {
                lblError.Text = "帳號或密碼錯誤";
                return;
            }

            // ✅ 這行會：有 ReturnUrl 就回去；沒有就去 defaultUrl (EquipState.aspx)
            FormsAuthentication.RedirectFromLoginPage(account, false);
        }
    }
}