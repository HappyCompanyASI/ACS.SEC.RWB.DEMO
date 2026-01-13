using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACS.SEC.RWB.DEMO
{
    internal static class EquipStateHelpers
    {
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
                string startTime = "2000-01-01 00:00:00";

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
                    object value = row.custVal ?? row.digitalVal ?? (object)row.stringVal;

                    if (value != null && double.TryParse(value.ToString(), out double numericValue))
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
    }
}