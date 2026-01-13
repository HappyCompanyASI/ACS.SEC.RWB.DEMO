using System;
using System.Web.Script.Services;
using System.Web.Services;

namespace ACS.SEC.RWB.DEMO
{
    [ScriptService] // [重要] 沒加這個，前端 fetch 就會報 401 錯誤
    public partial class EquipState : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 初始化時不需處理數據，交由前端 AJAX 輪詢 WebMethod
        }

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

        [WebMethod(EnableSession = true)]
        public static MonitorData GetRealtimeData()
        {
            var rand = new Random();

            // 1. 模擬產量數據
            int currentOutput = 20350 + rand.Next(0, 50); // 模擬產量緩慢增加
            int currentTarget = 11305;

            // 計算績效百分比
            double perf = ((double)(currentOutput - currentTarget) / currentTarget) * 100;

            // 2. 模擬設備原始數值 (Raw Data)
            // 假設 Line 1 範圍在 45 ~ 52.5，Line 2 範圍在 42.5 ~ 50
            double l1 = 45 + (rand.NextDouble() * 7.5);
            double l2 = 42.5 + (rand.NextDouble() * 7.5);

            // 3. 封裝並回傳數據
            return new MonitorData
            {
                Output = currentOutput,
                Target = currentTarget,
                Perf = Math.Round(perf, 1),

                // 折線圖顯示原始數值
                Line1Value = Math.Round(l1, 2),
                Line2Value = Math.Round(l2, 2),

                // 儀表板顯示轉換後的百分比 (0-100)
                // 這裡的算法應與您設備的警示範圍 (Min/Max) 對齊
                Line1Gauge = Math.Round(((l1 - 45) / 7.5) * 100, 0),
                Line2Gauge = Math.Round(((l2 - 42.5) / 7.5) * 100, 0),

                TimeLabel = DateTime.Now.ToString("HH:mm:ss")
            };
        }
    }
}