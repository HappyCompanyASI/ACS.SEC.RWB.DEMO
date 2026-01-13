using System;
using System.Configuration;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace ACS.SEC.RWB.DEMO
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // 初始化 WClient
            try
            {
                string apiServerUri = ConfigurationManager.AppSettings["ApiServerUri"];
                if (!string.IsNullOrEmpty(apiServerUri))
                {
                    // 移除結尾的斜線
                    while (apiServerUri.Length > 0 && apiServerUri.EndsWith("/"))
                    {
                        apiServerUri = apiServerUri.Substring(0, apiServerUri.Length - 1);
                    }

                    // 加上 /api 後綴 (與 WinForm 行為一致)
                    apiServerUri = string.Format("{0}/api", apiServerUri);

                    // 設定安全連線類型 (從 Web.config 讀取或使用預設值)
                    string securityType = ConfigurationManager.AppSettings["ConnSecurityType"];
                    ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType connSecurityType =
                        ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType.Safest;

                    if (!string.IsNullOrEmpty(securityType))
                    {
                        switch (securityType.ToLower())
                        {
                            case "plaintext":
                                connSecurityType = ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType.PlainText;
                                break;
                            case "login":
                                connSecurityType = ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType.Login;
                                break;
                            case "safest":
                            default:
                                connSecurityType = ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType.Safest;
                                break;
                        }
                    }

                    // 是否啟用壓縮 (從 Web.config 讀取或使用預設值)
                    bool isEnableZip = true;
                    string enableZip = ConfigurationManager.AppSettings["EnableZip"];
                    if (!string.IsNullOrEmpty(enableZip))
                    {
                        bool.TryParse(enableZip, out isEnableZip);
                    }

                    // 建立 WClient 實例 (直接使用設定的 URI，不加上 /api)
                    GlobalHelper.MyWClient = new ACS.CIP.API.Web.WCall.WClient(
                        apiServerUri,
                        connSecurityType,
                        isEnableZip);
                }
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            // 應用程式結束時清理 WClient
            try
            {
                if (GlobalHelper.MyWClient != null)
                {
                    GlobalHelper.MyWClient.Dispose();
                    GlobalHelper.MyWClient = null;
                }
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
            }
        }
    }
}