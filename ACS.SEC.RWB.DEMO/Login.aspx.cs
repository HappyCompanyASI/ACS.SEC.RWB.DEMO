using System;
using System.Threading.Tasks;
using System.Web.Security;

namespace ACS.SEC.RWB.DEMO
{
    public partial class Login : System.Web.UI.Page
    {
        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            await ProcessLoginAsync();
        }

        /// <summary>
        /// 處理登入流程
        /// </summary>
        private async Task ProcessLoginAsync()
        {
            System.Threading.Tasks.Task<
                ACS.CIP.API.Web.WObj.RsltMsgBase<
                ACS.CIP.API.Web.WObj.Data.Security.RsltLogin>> oRsltTasks = null;

            ACS.CIP.API.Web.WObj.RsltMsgBase<
                ACS.CIP.API.Web.WObj.Data.Security.RsltLogin> oRsltMsg = null;

            try
            {
                // 隱藏錯誤訊息
                lblError.Visible = false;

                // 停用控制項
                txtUsername.Disabled = true;
                txtPassword.Disabled = true;
                btnLogin.Enabled = false;

                // 呼叫 API 登入
                oRsltTasks = GlobalHelper.MyWClient.LoginAsync(
                    txtUsername.Value,
                    txtPassword.Value);
                oRsltMsg = await oRsltTasks;

                if (oRsltMsg.ok == false)
                {
                    // 登入失敗，顯示錯誤訊息
                    if (oRsltMsg.errCode != null)
                    {
                        lblError.Text = GlobalHelper.GetErrCodeMsg(
                            oRsltMsg.errCode,
                            oRsltMsg.errMsg,
                            oRsltMsg.ckPoint);
                    }
                    else
                    {
                        lblError.Text = "登入失敗";
                    }
                    lblError.Visible = true;
                }
                else
                {
                    // 登入成功,建立 Forms Authentication Ticket
                    FormsAuthentication.SetAuthCookie(oRsltMsg.data.userName, false);

                    // 設定 Session 並轉向
                    Session["User"] = oRsltMsg.data.userName;
                    Response.Redirect("EquipState.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
                lblError.Text = "發生錯誤：" + ex.Message;
                lblError.Visible = true;
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
