using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ACS.SEC.RWB.DEMO.CIPLib
{
    public sealed class LoginResult
    {
        public bool Success { get; private set; }
        public string UserName { get; private set; }
        public string ErrorMessage { get; private set; }

        private LoginResult() { }

        public static LoginResult Ok(string userName)
        {
            return new LoginResult
            {
                Success = true,
                UserName = userName,
                ErrorMessage = null
            };
        }

        public static LoginResult Fail(string errorMessage)
        {
            return new LoginResult
            {
                Success = false,
                UserName = null,
                ErrorMessage = errorMessage
            };
        }
    }
    public sealed class LogoutResult
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }

        private LogoutResult() { }

        public static LogoutResult Ok()
        {
            return new LogoutResult
            {
                Success = true,
                ErrorMessage = null
            };
        }

        public static LogoutResult Fail(string errorMessage)
        {
            return new LogoutResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
    internal static class DemoHelpers
    {
        #region Public
        public static void OnApplicationStart()
        {
            // Route / Bundle
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // WClient
            InitializeWClientFromConfig();
        }
        public static void OnApplicationEnd()
        {
            CleanupWClient();
        }
        public static async Task<LoginResult> ProcessLoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return LoginResult.Fail("請輸入帳號");
            if (string.IsNullOrWhiteSpace(password))
                return LoginResult.Fail("請輸入密碼");

            ACS.CIP.API.Web.WObj.RsltMsgBase<
                ACS.CIP.API.Web.WObj.Data.Security.RsltLogin> oRsltMsg = null;

            try
            {
                var task = GlobalHelper.MyWClient.LoginAsync(username, password);
                oRsltMsg = await task;

                if (oRsltMsg == null)
                    return LoginResult.Fail("登入失敗：回傳資料為空");

                if (oRsltMsg.ok == false)
                {
                    if (oRsltMsg.errCode != null)
                    {
                        var msg = GlobalHelper.GetErrCodeMsg(
                            oRsltMsg.errCode,
                            oRsltMsg.errMsg,
                            oRsltMsg.ckPoint);

                        return LoginResult.Fail(msg);
                    }

                    return LoginResult.Fail("登入失敗");
                }

                // 成功：回傳 userName
                return LoginResult.Ok(oRsltMsg.data.userName);
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
                return LoginResult.Fail("發生錯誤：" + ex.Message);
            }
        }
        public static async Task<LogoutResult> ProcessLogoutAsync(HttpSessionState session)
        {
            // 先登出 FormsAuth（就算 API 失敗也要先切掉本地登入狀態）
            FormsAuthentication.SignOut();

            try
            {
                // MyWClient 未初始化：仍視為已登出（本地已 SignOut）
                if (GlobalHelper.MyWClient == null)
                {
                    if (session != null) session.Clear();
                    return LogoutResult.Ok();
                }

                var oRsltMsg = await GlobalHelper.MyWClient.LogoutAsync();

                // API 回傳為空：仍清掉 Session，當成登出完成
                if (oRsltMsg == null)
                {
                    if (session != null) session.Clear();
                    return LogoutResult.Ok();
                }

                if (oRsltMsg.ok == false)
                {
                    // 這些狀況視為已登出
                    if (oRsltMsg.errCode == "CM0014" ||
                        oRsltMsg.errCode == "SR0001" ||
                        oRsltMsg.errCode == "SR0002")
                    {
                        if (session != null) session.Clear();
                        return LogoutResult.Ok();
                    }

                    // 其他錯誤：記錄，但仍清 Session（避免卡住）
                    if (oRsltMsg.errCode != null)
                    {
                        var msg = GlobalHelper.GetErrCodeMsg(
                            oRsltMsg.errCode,
                            oRsltMsg.errMsg,
                            oRsltMsg.ckPoint);

                        new ACS.CIP.API.Web.WCall.Util.Log(new Exception(msg));

                        if (session != null) session.Clear();
                        return LogoutResult.Fail(msg);
                    }

                    new ACS.CIP.API.Web.WCall.Util.Log(new Exception("登出失敗"));
                    if (session != null) session.Clear();
                    return LogoutResult.Fail("登出失敗");
                }

                // 成功
                if (session != null) session.Clear();
                return LogoutResult.Ok();
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);

                // 就算 API 掛了，也清 Session（本地已 SignOut）
                if (session != null) session.Clear();
                return LogoutResult.Fail("發生錯誤：" + ex.Message);
            }
        }
        public static async Task<int> LoadOutputDataAsync()
        {
            try
            {
                // 檢查 WClient 是否已初始化
                if (GlobalHelper.MyWClient == null)
                {
                    new ACS.CIP.API.Web.WCall.Util.Log(new Exception("GlobalHelper.MyWClient 尚未初始化"));
                    return 0;
                }

                var tagList = new List<string> { "FI_12012" };
                var startTime = "2000-01-01 00:00:00";

                var result = await ACS.CIP.API.Web.WCall.Helper.Tags.GetTagLastValueFromUserGrpAsync(
                    GlobalHelper.MyWClient,
                    tagList,
                    startTime,
                    null,
                    null
                );

                if (result.ok && result.data?.lastValue != null && result.data.lastValue.Count > 0)
                {
                    var row = result.data.lastValue[0];

                    // 取得值(優先順序:custVal > digitalVal > stringVal)
                    var value = row.custVal ?? row.digitalVal ?? (object)row.stringVal;

                    if (value != null && double.TryParse(value.ToString(), out var numericValue))
                    {
                        return (int)(numericValue * 100); // 回傳整數值
                    }
                }

                return 0; // 無數據時回傳 0
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
                return 0; // 發生錯誤時回傳 0
            }
        }
        public static async Task<int> LoadTargetDataAsync()
        {
            try
            {
                // 檢查 WClient 是否已初始化
                if (GlobalHelper.MyWClient == null)
                {
                    new ACS.CIP.API.Web.WCall.Util.Log(new Exception("GlobalHelper.MyWClient 尚未初始化"));
                    return 0;
                }

                var tagList = new List<string> { "FI_14009" };
                var startTime = "2000-01-01 00:00:00";

                var result = await ACS.CIP.API.Web.WCall.Helper.Tags.GetTagLastValueFromUserGrpAsync(
                    GlobalHelper.MyWClient,
                    tagList,
                    startTime,
                    null,
                    null
                );

                if (result.ok && result.data?.lastValue != null && result.data.lastValue.Count > 0)
                {
                    var row = result.data.lastValue[0];

                    // 取得值(優先順序:custVal > digitalVal > stringVal)
                    var value = row.custVal ?? row.digitalVal ?? (object)row.stringVal;

                    if (value != null && double.TryParse(value.ToString(), out var numericValue))
                    {
                        return (int)(numericValue * 300); // 回傳整數值
                    }
                }

                return 0; // 無數據時回傳 0
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
                return 0; // 發生錯誤時回傳 0
            }
        }
        public static async Task<double> LoadL1DataAsync()
        {
            try
            {
                // 檢查 WClient 是否已初始化
                if (GlobalHelper.MyWClient == null)
                {
                    new ACS.CIP.API.Web.WCall.Util.Log(new Exception("GlobalHelper.MyWClient 尚未初始化"));
                    return 0;
                }

                var tagList = new List<string> { "DEMO_enthalpy" };
                var startTime = "2000-01-01 00:00:00";

                var result = await ACS.CIP.API.Web.WCall.Helper.Tags.GetTagLastValueFromUserGrpAsync(
                    GlobalHelper.MyWClient,
                    tagList,
                    startTime,
                    null,
                    null
                );

                if (result.ok && result.data?.lastValue != null && result.data.lastValue.Count > 0)
                {
                    var row = result.data.lastValue[0];

                    // 取得值(優先順序:custVal > digitalVal > stringVal)
                    var value = row.custVal ?? row.digitalVal ?? (object)row.stringVal;

                    return value != null ? Convert.ToDouble(value) : 0;
                }

                return 0; // 無數據時回傳 0
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
                return 0; // 發生錯誤時回傳 0
            }
        }
        public static async Task<double> LoadL2DataAsync()
        {
            try
            {
                // 檢查 WClient 是否已初始化
                if (GlobalHelper.MyWClient == null)
                {
                    new ACS.CIP.API.Web.WCall.Util.Log(new Exception("GlobalHelper.MyWClient 尚未初始化"));
                    return 0;
                }

                var tagList = new List<string> { "DEMO_humid" };
                var startTime = "2000-01-01 00:00:00";

                var result = await ACS.CIP.API.Web.WCall.Helper.Tags.GetTagLastValueFromUserGrpAsync(
                    GlobalHelper.MyWClient,
                    tagList,
                    startTime,
                    null,
                    null
                );

                if (result.ok && result.data?.lastValue != null && result.data.lastValue.Count > 0)
                {
                    var row = result.data.lastValue[0];

                    // 取得值(優先順序:custVal > digitalVal > stringVal)
                    var value = row.custVal ?? row.digitalVal ?? (object)row.stringVal;

                    return value != null ? Convert.ToDouble(value) : 0;
                }

                return 0; // 無數據時回傳 0
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
                return 0; // 發生錯誤時回傳 0
            }
        }
        #endregion

        #region Private
        private static void InitializeWClientFromConfig()
        {
            try
            {
                var apiServerUri = ConfigurationManager.AppSettings["ApiServerUri"];
                if (string.IsNullOrEmpty(apiServerUri))
                    return;

                apiServerUri = TrimEndSlash(apiServerUri);

                // 加上 /api 後綴 (與 WinForm 行為一致)
                apiServerUri = string.Format("{0}/api", apiServerUri);

                // ConnSecurityType
                var securityType = ConfigurationManager.AppSettings["ConnSecurityType"];
                var connSecurityType = ParseConnSecurityType(securityType);

                // EnableZip
                var isEnableZip = true;
                var enableZip = ConfigurationManager.AppSettings["EnableZip"];
                if (!string.IsNullOrEmpty(enableZip))
                {
                    bool.TryParse(enableZip, out isEnableZip);
                }

                GlobalHelper.MyWClient = new ACS.CIP.API.Web.WCall.WClient(
                    apiServerUri,
                    connSecurityType,
                    isEnableZip);
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
            }
        }
        private static void CleanupWClient()
        {
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
        private static string TrimEndSlash(string url)
        {
            while (url.Length > 0 && url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }
            return url;
        }
        private static ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType ParseConnSecurityType(string securityType)
        {
            var connSecurityType = ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType.Safest;

            if (string.IsNullOrEmpty(securityType))
                return connSecurityType;

            switch (securityType.ToLower())
            {
                case "plaintext":
                    return ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType.PlainText;
                case "login":
                    return ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType.Login;
                case "safest":
                default:
                    return ACS.CIP.API.Web.WObj.Data.Enums.ConnSecurityType.Safest;
            }
        }
        #endregion
    }
}