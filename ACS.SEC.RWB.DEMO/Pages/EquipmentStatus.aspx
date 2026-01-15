<%@ Page Title="設備狀況" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EquipmentStatus.aspx.cs" Inherits="ACS.SEC.RWB.DEMO.Pages.EquipmentStatus" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>

        <div style="margin: 16px 0;">
            <div>Data1：<span id="txtData1">--</span></div>
            <div id="txtStatus" style="margin-top: 8px;"></div>
        </div>

        <button id="btnReload" type="button">重新載入</button>
        <button id="btnFetch" type="button">手動更新</button>
    </main>

    <script>
        function setStatus(msg) {
            document.getElementById("txtStatus").textContent = msg || "";
        }

        async function updateDashboard() {
            const controller = new AbortController();
            const timer = setTimeout(() => controller.abort(), 8000); // 8 秒逾時

            try {
                setStatus("讀取中...");

                const response = await fetch('EquipmentStatus.aspx/GetRealtimeData', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    credentials: 'include',
                    body: JSON.stringify({}),
                    signal: controller.signal
                });

                // 先讀文字，避免你以為是 JSON 但其實回 Login HTML
                const raw = await response.text();

                if (!response.ok) {
                    setStatus(`HTTP ${response.status}：${raw.slice(0, 200)}`);
                    return;
                }

                let res;
                try {
                    res = JSON.parse(raw);
                } catch (e) {
                    setStatus(`不是 JSON（可能被導到 Login 或回傳錯誤頁）：${raw.slice(0, 200)}`);
                    return;
                }

                const data = res && res.d;
                if (!data) {
                    setStatus("找不到 res.d（PageMethod 格式不符）");
                    return;
                }

                document.getElementById("txtData1").textContent = data.Data1;
                setStatus("更新成功");
            }
            catch (err) {
                if (err.name === "AbortError") {
                    setStatus("逾時：後端 8 秒內沒有回應（request pending）");
                    return;
                }
                console.error(err);
                setStatus("抓取失敗：" + err.message);
            }
            finally {
                clearTimeout(timer);
            }
        }

        updateDashboard();

        // 你要輪詢就打開這行
        // setInterval(updateDashboard, 2000);






        //整個頁面重新整理
        //會重新跑一次：
        //重新 GET 這個 aspx
        //重新跑 Page_Load
        //重新載入 CSS / JS（看快取）
        //重新建立整個 DOM
        //等於你按瀏覽器的 F5
        //會「回到初始狀態」：你畫面上的輸入、捲軸位置、暫存的 JS 變數都會被清掉（除非瀏覽器保留）
        document.getElementById("btnReload").onclick = () => location.reload();


        //不刷新整頁
        //只做一件事：用 fetch 去打 EquipmentStatus.aspx / GetRealtimeData 拿新資料，然後把結果更新到某些元素上
        //Page_Load 不會跑、頁面不會重新載入、捲軸位置不會變、其他 UI 狀態會保留
        document.getElementById("btnFetch").onclick = () => updateDashboard();
    </script>
</asp:Content>
