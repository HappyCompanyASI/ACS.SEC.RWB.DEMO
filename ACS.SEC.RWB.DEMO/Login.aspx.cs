using System;
using System.Security.Principal;
using System.Web.Security;
using ACS.SEC.RWB.DEMO.CIPLib;

namespace ACS.SEC.RWB.DEMO
{
    public partial class Login : System.Web.UI.Page
    {
        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // 隱藏錯誤訊息
                lblError.Visible = false;

                // 停用控制項
                txtUsername.Disabled = true;
                txtPassword.Disabled = true;
                btnLogin.Enabled = false;

                var result = await DemoHelpers.ProcessLoginAsync(
                    txtUsername.Value,
                    txtPassword.Value);
                if (!result.Success)
                {
                    lblError.Text = result.ErrorMessage;
                    lblError.Visible = true;
                    return;
                }

                //「優先導回 ReturnUrl」，沒有才用 web.config 的 defaultUrl
                FormsAuthentication.RedirectFromLoginPage(result.UserName, false);
            }
            finally
            {
                // 恢復控制項
                txtUsername.Disabled = false;
                txtPassword.Disabled = false;
                btnLogin.Enabled = true;
                txtPassword.Value = "";
            }
        }
    }
}