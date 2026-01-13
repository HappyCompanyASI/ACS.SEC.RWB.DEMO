using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.SEC.RWB.DEMO
{
    /// <summary>
    /// 全域協助類別 (從 WinForms 移植)
    /// </summary>
    public static class GlobalHelper
    {
        #region =====[Private]Variable=====

        /// <summary>
        /// 錯誤碼清單
        /// </summary>
        private static Dictionary<string, string> m_ErrCodeMsg = new Dictionary<string, string>();

        #endregion =====[Private]Variable=====

        #region =====[Public]Property=====

        /// <summary>
        /// 使用的WebClient
        /// </summary>
        public static ACS.CIP.API.Web.WCall.WClient MyWClient { get; set; }

        #endregion =====[Public]Property=====

        #region =====[Private]Function=====

        #region InitErrCodeMsg()//void

        /// <summary>
        /// 初始化錯誤碼清單
        /// </summary>
        private static void InitErrCodeMsg()
        {
            try
            {
                if (m_ErrCodeMsg.Count <= 0)
                {
                    m_ErrCodeMsg.Add("CM0001", "使用的uri格式錯誤");
                    m_ErrCodeMsg.Add("CM0002", "未輸入組織代碼");
                    m_ErrCodeMsg.Add("CM0003", "未輸入登入帳號");
                    m_ErrCodeMsg.Add("CM0004", "未輸入登入密碼");
                    m_ErrCodeMsg.Add("CM0005", "無法取得與Server端互換金鑰的加密器建立參數");
                    m_ErrCodeMsg.Add("CM0006", "與Server端互換金鑰的加密失敗");
                    m_ErrCodeMsg.Add("CM0007", "無法取得登入用的Ticket");
                    m_ErrCodeMsg.Add("CM0008", "無法建立傳送給SERVER互換金鑰的訊息加密器，建立參數異常");
                    m_ErrCodeMsg.Add("CM0009", "登入失敗");
                    m_ErrCodeMsg.Add("CM0010", "與遠端伺服器通訊發生異常，請檢查網路狀態");
                    m_ErrCodeMsg.Add("CM0011", "登入失敗，發生異常錯誤!");
                    m_ErrCodeMsg.Add("CM0012", "無法取得與Server間的通訊加密金鑰");
                    m_ErrCodeMsg.Add("CM0013", "無法建立與Server間的通訊加密器");
                    m_ErrCodeMsg.Add("CM0014", "不需進行登出(未登入)");
                    m_ErrCodeMsg.Add("CM0015", "發生例外錯誤，請檢查Client log");
                    m_ErrCodeMsg.Add("CM0016", "無法提供Server端互換金鑰的加密器建立參數");
                    m_ErrCodeMsg.Add("CM0017", "無法解譯Server端傳送的訊息");
                    m_ErrCodeMsg.Add("CM0018", "尚未登出");
                    m_ErrCodeMsg.Add("CM0019", "回應異常，收到空的回應訊息");
                    m_ErrCodeMsg.Add("CM0020", "使用者已強制停止");
                    m_ErrCodeMsg.Add("CM0021", "設定檔異常，無法建立Client物件");

                    m_ErrCodeMsg.Add("SR0001", "尚未登入");
                    m_ErrCodeMsg.Add("SR0002", "帳號已強制登出(IP異常)");
                    m_ErrCodeMsg.Add("SR0003", "客戶端訊息未加密，必須加密才可傳送");
                    m_ErrCodeMsg.Add("SR0004", "客戶端訊息不可加密，Server端未開啟加密功能");
                    m_ErrCodeMsg.Add("SR0005", "目前已是登入狀態");
                    m_ErrCodeMsg.Add("SR0006", "客戶端訊息無法解密(RSA)");
                    m_ErrCodeMsg.Add("SR0007", "客戶端加密訊息未提供解密所需的資訊(RSA)");
                    m_ErrCodeMsg.Add("SR0008", "客戶端加密訊息無法解譯，通訊用加密器遺失，請重新登入");
                    m_ErrCodeMsg.Add("SR0009", "處理客戶端訊息時發生例外錯誤，請檢查紀錄檔");
                    m_ErrCodeMsg.Add("SR0010", "伺服器端訊息無法加密，通訊用加密器遺失，請重新登入");
                    m_ErrCodeMsg.Add("SR0011", "伺服器端訊息無法壓縮，壓縮格式設定錯誤");
                    m_ErrCodeMsg.Add("SR0012", "處理伺服器端訊息時發生例外錯誤，請檢查紀錄檔");
                    m_ErrCodeMsg.Add("SR0013", "只允許使用POST呼叫");
                    m_ErrCodeMsg.Add("SR0014", "處理伺服器未允許明碼登入");
                    m_ErrCodeMsg.Add("SR0015", "處理伺服器未允許通訊內容不加密");
                    m_ErrCodeMsg.Add("SR0016", "當登入時指定使用高安全性連線，所有通訊內容必須加密");
                    m_ErrCodeMsg.Add("SR0017", "未使用高安全性連線時，只允許登出目前的帳號");

                    m_ErrCodeMsg.Add("SC0001", "登入失敗，伺服器發生異常，請查詢記錄檔");
                    m_ErrCodeMsg.Add("SC0002", "無法取得登入用Ticket，伺服器發生異常，請查詢記錄檔");
                    m_ErrCodeMsg.Add("SC0003", "客戶端未提供互換金鑰加密器建立參數");
                    m_ErrCodeMsg.Add("SC0004", "登入用Ticket不合法");
                    m_ErrCodeMsg.Add("SC0005", "無法登出其他使用者");
                    m_ErrCodeMsg.Add("SC0006", "登出失敗，伺服器發生異常，請查詢記錄檔");

                    m_ErrCodeMsg.Add("EC0003", "未指定起始時間或起始時間格式錯誤");
                    m_ErrCodeMsg.Add("EC0004", "未指定結束時間或起始時間格式錯誤");

                    m_ErrCodeMsg.Add("VC0000", "參數數量錯誤");
                    m_ErrCodeMsg.Add("VC0001", "未指定變因的資料密度");

                    //TAG功能
                    m_ErrCodeMsg.Add("TF0001", "找不到指定的TAG設定");
                    m_ErrCodeMsg.Add("TF0002", "指定的TAG({0})類型({1})無法進行要求的運算({2})");
                    m_ErrCodeMsg.Add("EU0004", "部分回應異常(用於集合式查詢)");

                    //通用錯誤訊息
                    m_ErrCodeMsg.Add("UT0001", "取得資料時發生例外錯誤，請檢查紀錄檔");
                    m_ErrCodeMsg.Add("UT0002", "伺服器端發生例外錯誤，請檢查紀錄檔");
                    m_ErrCodeMsg.Add("UT0003", "未提供任何要求資訊，無法搜尋資料");
                    m_ErrCodeMsg.Add("UT0004", "分頁查詢資料時每頁上限筆數不可超過{0}筆");
                    m_ErrCodeMsg.Add("UT0005", "查詢資料時時間範圍不可超過{0}小時");
                    m_ErrCodeMsg.Add("UT0006", "查詢資料時時間範圍不可超過{0}天");
                    m_ErrCodeMsg.Add("UT0007", "查詢資料時時間範圍不可超過{0}月");
                    m_ErrCodeMsg.Add("UT0008", "查詢資料時時間範圍不可超過{0}年");
                    m_ErrCodeMsg.Add("UT0009", "無產品授權");
                    m_ErrCodeMsg.Add("UT0010", "參數{0}不可為NULL");
                    m_ErrCodeMsg.Add("UT0011", "參數{0}異常：{1}");
                    m_ErrCodeMsg.Add("UT0012", "參數數量錯誤");
                    m_ErrCodeMsg.Add("UT0013", "[Exception]：{0} | [StackTrace]：{1}");
                    m_ErrCodeMsg.Add("UT0014", "資料處理({0})逾時，超過{1}秒，已強制中止");
                    m_ErrCodeMsg.Add("UT0015", "找不到資料");
                    m_ErrCodeMsg.Add("UT0016", "無法取得群組代碼");
                }
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
            }
        }

        #endregion InitErrCodeMsg()//void

        #region GetErrCodeMsg(string errCode, string[] errMsg, string errPoint)//string

        /// <summary>
        /// 取得錯誤訊息
        /// </summary>
        /// <param name="errCode">錯誤碼</param>
        /// <param name="errMsg">錯誤訊息</param>
        /// <param name="errPoint">錯誤點</param>
        /// <returns></returns>
        public static string GetErrCodeMsg(string errCode, string[] errMsg, string errPoint)
        {
            string sMsg = null;
            try
            {
                InitErrCodeMsg();
                if (m_ErrCodeMsg.TryGetValue(errCode, out sMsg))
                {
                    if (errMsg != null &&
                        errMsg.Length > 0 &&
                        sMsg.IndexOf("{") >= 0)
                    {
                        sMsg = string.Format(sMsg, errMsg);
                    }
                    if (errPoint != null &&
                        errPoint.Trim() != "")
                    {
                        return string.Format("{0}({1})", sMsg, errCode);
                    }
                    else
                    {
                        return string.Format("{0}({1}, {2})", sMsg, errCode, errPoint);
                    }
                }
                return string.Format("發生錯誤：{0}", errCode);
            }
            catch (Exception ex)
            {
                new ACS.CIP.API.Web.WCall.Util.Log(ex);
                return ex.Message;
            }
        }

        #endregion GetErrCodeMsg(string errCode, string[] errMsg, string errPoint)//string

        #endregion =====[Private]Function=====
    }
}