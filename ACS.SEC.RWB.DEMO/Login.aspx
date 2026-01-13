<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ACS.SEC.RWB.DEMO.Login" %>

<!DOCTYPE html>
<html lang="zh">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>

    <!-- 如果你要套 Bootstrap（可留） -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body { background:#eee; margin:0; }
        .wrap { min-height:100vh; display:flex; align-items:center; justify-content:center; padding:16px; }
        .cardx { width:380px; max-width:100%; background:#fff; padding:18px; border-radius:12px; box-shadow:0 10px 25px rgba(0,0,0,.15); }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="wrap">
        <div class="cardx">
            <h3 class="mb-3">登入</h3>

            <div class="mb-3">
                <label class="form-label">帳號</label>
                <asp:TextBox ID="txtAccount" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label class="form-label">密碼</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
            </div>

            <div class="d-grid">
                <asp:Button ID="btnLogin" runat="server" Text="登入" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
            </div>

            <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mt-3" />
        </div>
    </div>
</form>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>