using ACS.SEC.RWB.DEMO.CIPLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Services;
using System.Web.Services;

namespace ACS.SEC.RWB.DEMO.Pages
{
    // 定義傳回前端的 JSON 資料格式
    // 屬性名稱需與前端 JavaScript 中的 data.XXXX 保持一致
    public class MonitorData
    {
        //Panel 1 產量目標績效
        public int Output { get; set; }        // 產量
        public int Target { get; set; }        // 目標
        public double Perf { get; set; }       // 績效 (%)

        //Panel 2 即時監控
        public double Line1Value { get; set; } // 用於Line 1 折線圖最新值
        public double Line1Gauge { get; set; } // 用於Line 1 圓環百分比 (0-100)
        public double Line2Value { get; set; } // 用於Line 2 折線圖最新值
        public double Line2Gauge { get; set; } // 用於Line 2 圓環百分比 (0-100)

        //Panel 3 產線之產能比較
        public string TimeLabel { get; set; }  // 當前時間標籤 (HH:mm:ss)
    }

    // 產能比較數據格式
    public class CompareDataResponse
    {
        public List<int> Line1Today { get; set; }       // Line1 當日24小時數據
        public List<int> Line1Yesterday { get; set; }   // Line1 前一日24小時數據
        public List<int> Line2Today { get; set; }       // Line2 當日24小時數據
        public List<int> Line2Yesterday { get; set; }   // Line2 前一日24小時數據
        public string CurrentDate { get; set; }         // 當前日期
        public string PreviousDate { get; set; }        // 前一日期
    }

    [ScriptService] // [重要] 沒加這個，前端 fetch 就會報 401 錯誤
    public partial class ProductionStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 初始化時不需處理數據，交由前端 AJAX 輪詢 WebMethod
        }

        [WebMethod(EnableSession = true)]
        public static MonitorData GetRealtimeData()
        {

            // 產量與目標數據 
            var currentOutput = Task.Run(async () => await DemoHelpers.LoadOutputDataAsync()).GetAwaiter().GetResult();
            var currentTarget = Task.Run(async () => await DemoHelpers.LoadTargetDataAsync()).GetAwaiter().GetResult();

            // 績效百分比
            var perf = ((double)currentOutput / currentTarget) * 100;

            // 即時監控
            var l1 = Task.Run(async () => await DemoHelpers.LoadL1DataAsync()).GetAwaiter().GetResult();
            var l2 = Task.Run(async () => await DemoHelpers.LoadL2DataAsync()).GetAwaiter().GetResult();

            // 封裝並回傳數據
            return new MonitorData
            {
                // 產量目標績效
                Output = currentOutput,
                Target = currentTarget,
                Perf = Math.Round(perf, 1),

                // 折線圖
                Line1Value = Math.Round(l1, 2),
                Line2Value = Math.Round(l2, 2),

                // 儀表板
                Line1Gauge = Math.Round(l1, 0),
                Line2Gauge = Math.Round(l2, 0),

                TimeLabel = DateTime.Now.ToString("HH:mm:ss")
            };
        }

        [WebMethod(EnableSession = true)]
        public static CompareDataResponse GetCompareData(string date, string lineFilter)
        {
            // 解析日期
            DateTime selectedDate;
            if (!DateTime.TryParse(date, out selectedDate))
            {
                selectedDate = DateTime.Today;
            }

            // 計算前一天
            DateTime previousDate = selectedDate.AddDays(-1);

            // 根據 lineFilter 決定要載入哪些產線數據
            var response = new CompareDataResponse
            {
                CurrentDate = selectedDate.ToString("yyyy-MM-dd"),
                PreviousDate = previousDate.ToString("yyyy-MM-dd")
            };

            // 載入數據（使用假數據）
            if (lineFilter == "all" || lineFilter == "line1")
            {
                response.Line1Today = Task.Run(async () =>
                    await DemoHelpers.LoadCompareDataAsync(response.CurrentDate, "line1")
                ).GetAwaiter().GetResult();

                response.Line1Yesterday = Task.Run(async () =>
                    await DemoHelpers.LoadCompareDataAsync(response.PreviousDate, "line1")
                ).GetAwaiter().GetResult();
            }

            if (lineFilter == "all" || lineFilter == "line2")
            {
                response.Line2Today = Task.Run(async () =>
                    await DemoHelpers.LoadCompareDataAsync(response.CurrentDate, "line2")
                ).GetAwaiter().GetResult();

                response.Line2Yesterday = Task.Run(async () =>
                    await DemoHelpers.LoadCompareDataAsync(response.PreviousDate, "line2")
                ).GetAwaiter().GetResult();
            }

            return response;
        }
    }
}