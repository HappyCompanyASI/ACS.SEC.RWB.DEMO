using System;
using System.Threading.Tasks;
using System.Web.Security;

namespace ACS.SEC.RWB.DEMO
{
    public partial class Logout : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // 新增檢查:只有帶 confirm=1 才執行登出
            if (Request.QueryString["confirm"] != "1")
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            await ProcessLogoutAsync();
        }

        private async Task ProcessLogoutAsync()
        {
            FormsAuthentication.SignOut();

            System.Threading.Tasks.Task<ACS.CIP.API.Web.WObj.RsltMsgBase<string>> oRsltTasks = null;
            ACS.CIP.API.Web.WObj.RsltMsgBase<string> oRsltMsg = null;

            try
            {
                // 呼叫 API 登出
                oRsltTasks = GlobalHelper.MyWClient.LogoutAsync();
                oRsltMsg = await oRsltTasks;

                if (oRsltMsg.ok == false)
                {
                    // 處理特定錯誤碼 - 這些狀況視為已登出
                    if (oRsltMsg.errCode != null)
                    {
                        if (oRsltMsg.errCode == "CM0014" ||
                            oRsltMsg.errCode == "SR0001" ||
                            oRsltMsg.errCode == "SR0002")
                        {
                            // 這些錯誤碼表示已經登出，清除 Session 並轉向
                            Session.Clear();
                            Response.Redirect("Login.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                            return;
                        }

                        // 記錄其他錯誤
                        new ACS.CIP.API.Web.WCall.Util.Log(
                            new Exception(GlobalHelper.GetErrCodeMsg(
                                oRsltMsg.errCode,
                                oRsltMsg.errMsg,
                                oRsltMsg.ckPoint)));
                    }
                }

                // 登出成功或已處理錯誤，清除 Session 並轉向
                Session.Clear();
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
                // 即使發生錯誤也清除 Session 並轉向登入頁
                Session.Clear();
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            finally
            {
                oRsltTasks = null;
                oRsltMsg = null;
            }
        }
    }
}