<%@ Page Title="ProductionStatus" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductionStatus.aspx.cs" Inherits="ACS.SEC.RWB.DEMO.Pages.ProductionStatus" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.1/dist/chart.umd.min.js"></script>

    <style>
        /* 100% 保留您原始的所有 CSS 樣式 */
        .opx-page { min-height: calc(100vh - 120px); padding: 18px 18px 28px 18px; background: radial-gradient(1200px 500px at 20% 0%, #f4f4f4 0%, #d6d6d6 55%, #c9c9c9 100%); }
        .opx-card { background: #fff; border-radius: 8px; border: 1px solid rgba(0,0,0,.08); box-shadow: 0 6px 18px rgba(0,0,0,.12); overflow: hidden; }
        .opx-card-body { padding: 18px; }
        .opx-titlebar { background: linear-gradient(#2f7a1e, #1f5c11); color: #fff; font-weight: 900; text-align: center; padding: 10px 12px; font-size: 28px; letter-spacing: 1px; }
        .kpi-top { display: flex; align-items: center; justify-content: space-between; min-height: 120px; }
        .kpi-label { font-size: 56px; font-weight: 900; color: #111; }
        .kpi-value { font-size: 44px; font-weight: 900; color: #111; }
        .kpi-split { display: grid; grid-template-columns: 1fr 1fr; border-top: 2px solid #2f7a1e; }
        .kpi-cell { padding: 14px 16px; border-right: 2px solid #2f7a1e; }
        .kpi-cell:last-child { border-right: none; }
        .kpi-cell .t { font-size: 26px; font-weight: 900; color: #111; margin-bottom: 8px; }
        .kpi-cell .v { font-size: 22px; font-weight: 900; color: #111; }
        .monitor-wrap { padding: 14px 16px 16px 16px; }
        .gauge-block { display: grid; grid-template-columns: 1fr; gap: 16px; justify-items: center; }
        .gauge-label { font-weight: 800; margin-bottom: 6px; }
        .gauge-wrap { position: relative; width: 220px; height: 115px; margin: 0 auto; }
        .gauge-canvas { width: 100% !important; height: 100% !important; }
        .gauge-ticks { position: absolute; left: -5px; right: -5px; bottom: -5px; height: 20px; font-size: 16px; color: #b58d3b; font-weight: bold; display: flex; justify-content: space-between; }
        .filters { display: flex; align-items: center; gap: 10px; padding: 10px 12px; border-bottom: 1px solid rgba(0,0,0,.08); background: #fafafa; flex-wrap: wrap; }
        .filters .mini { height: 32px; padding: 0 10px; border-radius: 6px; border: 1px solid rgba(0,0,0,.18); }
        .chart-pad { padding: 10px 12px 14px 12px; }
        .chart-fixed-height { height: 260px; }
        @media (max-width: 576px) {
            .kpi-label { font-size: 44px; } .kpi-value { font-size: 34px; } .opx-titlebar { font-size: 22px; }
            .gauge-block { grid-template-columns: 1fr 1fr; gap: 10px; }
            .gauge-wrap { width: 190px; height: 125px; }
            .gauge-canvas { width: 190px !important; height: 105px !important; }
            .chart-fixed-height { height: 320px; }
        }
    </style>

    <div class="opx-page">
        <div class="container-fluid p-0">
            <div class="row g-3">
                <div class="col-12 col-lg-4">
                    <div class="opx-card">
                        <div class="opx-card-body">
                            <div class="kpi-top">
                                <div class="kpi-label">產量</div>
                                <div class="kpi-value" id="kpiOutput">--</div>
                            </div>
                        </div>
                        <div class="kpi-split">
                            <div class="kpi-cell">
                                <div class="t">目標</div>
                                <div class="v" id="kpiTarget">--</div>
                            </div>
                            <div class="kpi-cell">
                                <div class="t">績效</div>
                                <div class="v" id="kpiPerf">--%</div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-12 col-lg-8">
                    <div class="opx-card">
                        <div class="opx-titlebar">即時監控</div>
                        <div class="monitor-wrap">
                            <div class="row g-3 align-items-stretch">
                                <div class="col-12 col-lg-8">
                                    <div class="chart-fixed-height">
                                        <canvas id="realtimeLine"></canvas>
                                    </div>
                                </div>
                                <div class="col-12 col-lg-4">
                                    <div class="gauge-block h-100">
                                        <div class="text-center">
                                            <div class="gauge-label">Line1</div>
                                            <div class="gauge-wrap">
                                                <canvas class="gauge-canvas" id="gauge1"></canvas>
                                                <div class="gauge-ticks"><span class="t0">0</span><span class="t100">100</span></div>
                                            </div>
                                        </div>
                                        <div class="text-center">
                                            <div class="gauge-label">Line2</div>
                                            <div class="gauge-wrap">
                                                <canvas class="gauge-canvas" id="gauge2"></canvas>
                                                <div class="gauge-ticks"><span class="t0">0</span><span class="t100">100</span></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-12">
                    <div class="opx-card">
                        <div class="opx-titlebar">產線之產能比較</div>
                        <div class="filters">
                            <input id="filterDate" class="mini" type="date" value="2025-12-12" />
                            <select id="filterType" class="mini"><option value="day" selected>日報</option><option value="shift">班別</option><option value="hour">小時</option></select>
                            <select id="filterLine" class="mini"><option value="all" selected>顯示全部</option><option value="line1">Device1</option><option value="line2">Device2</option></select>
                        </div>
                        <div class="chart-pad">
                            <div class="chart-fixed-height"><canvas id="compareChart"></canvas></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        // --- 工具函式 ---
        const fmtNumber = (n) => n.toLocaleString();
        const clamp = (n, min, max) => Math.max(min, Math.min(max, n));

        const CenterTextPlugin = {
            id: "centerText",
            afterDraw(chart, args, opts) {
                if (!opts || typeof opts.text === "undefined") return;
                const { ctx, chartArea: { left, right, bottom } } = chart;
                ctx.save();
                ctx.font = "bold 26px sans-serif";
                ctx.fillStyle = "#111";
                ctx.textAlign = "center";
                ctx.textBaseline = "bottom";
                ctx.fillText(opts.text, (left + right) / 2, bottom - 2);
                ctx.restore();
            }
        };

        function createGauge(canvasId) {
            return new Chart(document.getElementById(canvasId), {
                type: "doughnut",
                data: { datasets: [{ data: [0, 100], backgroundColor: ["#4CAF50", "#EAEAEA"], borderWidth: 0 }] },
                options: {
                    responsive: true, maintainAspectRatio: false, cutout: "65%", rotation: -90, circumference: 180,
                    layout: { padding: { bottom: 20, left: 10, right: 10 } },
                    plugins: { legend: { display: false }, centerText: { text: "0" } }
                },
                plugins: [CenterTextPlugin]
            });
        }

        let g1 = createGauge("gauge1");
        let g2 = createGauge("gauge2");
        const MAX_POINTS = 16;

        const realtimeLine = new Chart(document.getElementById("realtimeLine"), {
            type: "line",
            data: {
                labels: Array(MAX_POINTS).fill(""), datasets: [
                    { label: "Line1", data: Array(MAX_POINTS).fill(null), borderColor: "#4CAF50", tension: 0.25 },
                    { label: "Line2", data: Array(MAX_POINTS).fill(null), borderColor: "#2196F3", tension: 0.25 }
                ]
            },
            options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { position: "bottom" } } }
        });

        // [Chart.js 說明] 產能比較圖表 - 柱狀圖配置
        const compareChart = new Chart(document.getElementById("compareChart"), {
            type: 'bar',
            data: {
                labels: Array.from({ length: 24 }, (_, i) => i + 1), // X軸：1-24小時
                datasets: [
                    // Line1 當日數據（深橙色）
                    { label: 'Device1@2025-12-12', data: Array(24).fill(0), backgroundColor: '#FF8C42', borderColor: '#FF8C42', borderWidth: 1 },
                    // Line1 前日數據（淺橙色）
                    { label: 'Device1@2025-12-11', data: Array(24).fill(0), backgroundColor: '#FFB380', borderColor: '#FFB380', borderWidth: 1 },
                    // Line2 當日數據（深青色）
                    { label: 'Device2@2025-12-12', data: Array(24).fill(0), backgroundColor: '#4FC3C8', borderColor: '#4FC3C8', borderWidth: 1 },
                    // Line2 前日數據（淺青色）
                    { label: 'Device2@2025-12-11', data: Array(24).fill(0), backgroundColor: '#8FD8DA', borderColor: '#8FD8DA', borderWidth: 1 }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom', // 圖例位置在底部
                        labels: { font: { size: 12 } }
                    }
                },
                scales: {
                    x: { title: { display: true, text: '(小時)' } },
                    y: { beginAtZero: true, title: { display: true, text: '產能' } }
                }
            }
        });

        // --- 核心：對齊您的 MonitorData 結構 ---
        async function updateDashboard() {
            try {
                const response = await fetch('ProductionStatus.aspx/GetRealtimeData', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    credentials: 'include', // [關鍵] 讓 AJAX 請求攜帶目前的登入憑證 (Cookie)
                    body: JSON.stringify({})
                });


                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }


                const res = await response.json();
                const data = res.d; // 這裡對應後端的 MonitorData 物件


                // [修復核心]：檢查 data 是否存在，避免 TypeError
                if (data) {
                    document.getElementById("kpiOutput").innerText = data.Output.toLocaleString();
                    document.getElementById("kpiTarget").innerText = data.Target.toLocaleString();
                    document.getElementById("kpiPerf").innerText = data.Perf + "%";

                    // 1. 更新 KPI (使用後端計算好的 Perf)
                    document.getElementById("kpiOutput").innerText = fmtNumber(data.Output);
                    document.getElementById("kpiTarget").innerText = fmtNumber(data.Target);
                    document.getElementById("kpiPerf").innerText = data.Perf + "%";

                    // 2. 更新折線圖 (使用後端給的 TimeLabel 和 Value)
                    realtimeLine.data.labels.push(data.TimeLabel);
                    realtimeLine.data.datasets[0].data.push(data.Line1Value);
                    realtimeLine.data.datasets[1].data.push(data.Line2Value);

                    if (realtimeLine.data.labels.length > MAX_POINTS) {
                        realtimeLine.data.labels.shift();
                        realtimeLine.data.datasets.forEach(ds => ds.data.shift());
                    }
                    realtimeLine.update('none');

                    // 3. 更新儀表板 (直接使用後端算好的 Gauge 0-100)
                    updateGauge(g1, data.Line1Gauge);
                    updateGauge(g2, data.Line2Gauge);
                }
            } catch (err) {
                console.error("抓取資料失敗，請檢查 401 權限:", err);
            }
        }

        function updateGauge(chart, val) {
            const v = clamp(Math.round(val), 0, 100);
            chart.data.datasets[0].data = [v, 100 - v];
            chart.options.plugins.centerText.text = String(v);
            chart.update();
        }

        // [新增] 載入產能比較圖表數據
        async function loadCompareChart() {
            try {
                const date = document.getElementById("filterDate").value;
                const lineFilter = document.getElementById("filterLine").value;

                const response = await fetch('ProductionStatus.aspx/GetCompareData', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    credentials: 'include',
                    body: JSON.stringify({ date: date, lineFilter: lineFilter })
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const res = await response.json();
                const data = res.d; // CompareDataResponse 物件

                if (data) {
                    // [Chart.js 說明] 更新圖表的 datasets
                    // datasets[0] = Line1 當日, datasets[1] = Line1 前日
                    // datasets[2] = Line2 當日, datasets[3] = Line2 前日

                    // 根據產線篩選器決定要顯示哪些數據
                    if (lineFilter === "all") {
                        // 顯示全部產線
                        compareChart.data.datasets[0].data = data.Line1Today || Array(24).fill(0);
                        compareChart.data.datasets[0].label = `Device1@${data.CurrentDate}`;
                        compareChart.data.datasets[1].data = data.Line1Yesterday || Array(24).fill(0);
                        compareChart.data.datasets[1].label = `Device1@${data.PreviousDate}`;
                        compareChart.data.datasets[2].data = data.Line2Today || Array(24).fill(0);
                        compareChart.data.datasets[2].label = `Device2@${data.CurrentDate}`;
                        compareChart.data.datasets[3].data = data.Line2Yesterday || Array(24).fill(0);
                        compareChart.data.datasets[3].label = `Device2@${data.PreviousDate}`;
                    } else if (lineFilter === "line1") {
                        // 只顯示 Line1，Line2 設為空數據
                        compareChart.data.datasets[0].data = data.Line1Today || Array(24).fill(0);
                        compareChart.data.datasets[0].label = `Device1@${data.CurrentDate}`;
                        compareChart.data.datasets[1].data = data.Line1Yesterday || Array(24).fill(0);
                        compareChart.data.datasets[1].label = `Device1@${data.PreviousDate}`;
                        compareChart.data.datasets[2].data = Array(24).fill(0);
                        compareChart.data.datasets[3].data = Array(24).fill(0);
                    } else if (lineFilter === "line2") {
                        // 只顯示 Line2，Line1 設為空數據
                        compareChart.data.datasets[0].data = Array(24).fill(0);
                        compareChart.data.datasets[1].data = Array(24).fill(0);
                        compareChart.data.datasets[2].data = data.Line2Today || Array(24).fill(0);
                        compareChart.data.datasets[2].label = `Device2@${data.CurrentDate}`;
                        compareChart.data.datasets[3].data = data.Line2Yesterday || Array(24).fill(0);
                        compareChart.data.datasets[3].label = `Device2@${data.PreviousDate}`;
                    }

                    // [Chart.js 說明] 更新圖表顯示
                    compareChart.update();
                }
            } catch (err) {
                console.error("載入產能比較數據失敗:", err);
            }
        }

        // 啟動即時監控更新
        setInterval(updateDashboard, 5000);
        updateDashboard();

        // 初始載入產能比較圖表
        loadCompareChart();

        // [新增] 篩選器事件綁定
        document.getElementById("filterDate").onchange = loadCompareChart;
        document.getElementById("filterLine").onchange = loadCompareChart;
    </script>
</asp:Content>
