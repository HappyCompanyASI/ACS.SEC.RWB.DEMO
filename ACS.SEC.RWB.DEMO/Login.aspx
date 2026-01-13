<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ACS.SEC.RWB.DEMO.Login" Async="true" %>

<!DOCTYPE html>
<html lang="zh">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>

    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Microsoft JhengHei", sans-serif;
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }

        .login-container {
            background: white;
            border-radius: 16px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
            width: 100%;
            max-width: 520px;
            padding: 56px 48px;
        }

        .logo-container {
            text-align: center;
            margin-bottom: 24px;
        }

        .logo {
            width: 80px;
            height: 80px;
            background: #2d7a3e;
            border-radius: 16px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            margin-bottom: 16px;
        }

        .logo-icon {
            width: 48px;
            height: 48px;
            fill: white;
        }

        h1 {
            font-size: 28px;
            font-weight: 600;
            color: #1a1a1a;
            margin-bottom: 8px;
        }

        .subtitle {
            color: #666;
            font-size: 14px;
            margin-bottom: 32px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-label {
            display: block;
            font-size: 14px;
            font-weight: 500;
            color: #333;
            margin-bottom: 8px;
        }

        .form-input {
            width: 100%;
            padding: 16px 18px;
            font-size: 16px;
            border: 1px solid #ddd;
            border-radius: 8px;
            outline: none;
            transition: all 0.3s;
        }

            .form-input:focus {
                border-color: #2d7a3e;
                box-shadow: 0 0 0 3px rgba(45, 122, 62, 0.1);
            }

            .form-input::placeholder {
                color: #aaa;
            }

        .login-button {
            width: 100%;
            padding: 16px;
            background: #2d7a3e;
            color: white;
            font-size: 18px;
            font-weight: 600;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: all 0.3s;
            margin-top: 8px;
        }

            .login-button:hover {
                background: #246830;
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(45, 122, 62, 0.3);
            }

            .login-button:active {
                transform: translateY(0);
            }

        .footer {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 24px;
            padding-top: 24px;
            border-top: 1px solid #eee;
        }

        .version {
            font-size: 12px;
            color: #999;
        }

        .forgot-password {
            font-size: 12px;
            color: #2d7a3e;
            text-decoration: none;
        }

            .forgot-password:hover {
                text-decoration: underline;
            }

        .copyright {
            text-align: center;
            margin-top: 32px;
            font-size: 13px;
            color: #999;
        }

        @media (max-width: 768px) {
            body {
                padding: 12px;
            }

            .login-container {
                max-width: 100%; 
                padding: 40px 24px; 
                border-radius: 12px; 
            }

            .logo {
                width: 80px;
                height: 80px;
                margin-bottom: 16px;
            }

            .logo-icon {
                width: 48px;
                height: 48px;
            }

            h1 {
                font-size: 26px;
            }

            .subtitle {
                font-size: 14px;
                margin-bottom: 28px;
            }

            .form-group {
                margin-bottom: 20px;
            }

            .form-input {
                padding: 14px 16px;
                font-size: 16px; 
            }

            .login-button {
                padding: 15px;
                font-size: 17px;
            }

            .copyright {
                font-size: 12px;
                margin-top: 20px;
            }
        }

       
        @media (max-width: 375px) {
            body {
                padding: 8px; 
            }

            .login-container {
                padding: 32px 20px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="logo-container">
                <div class="logo">
                    <svg class="logo-icon" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-5 14H7v-2h7v2zm3-4H7v-2h10v2zm0-4H7V7h10v2z" />
                    </svg>
                </div>
                <h1>Mr. OPX Login</h1>
                <p class="subtitle">請登入以管理您的廠機狀況</p>
            </div>

            <!-- 錯誤訊息 -->
            <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>

            <div class="form-group">
                <label class="form-label">帳號</label>
                <input type="text" id="txtUsername" runat="server" class="form-input" placeholder="請輸入使用者名稱" />
            </div>

            <div class="form-group">
                <label class="form-label">密碼</label>
                <input type="password" id="txtPassword" runat="server" class="form-input" placeholder="請輸入密碼" />
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="登入" CssClass="login-button" OnClick="btnLogin_Click" />

            <div class="footer">
                <span class="version">v1.2.4</span>
                <a href="#" class="forgot-password">忘記密碼?</a>
            </div>
        </div>

        <div class="copyright">
            © 2024 Mr. OPX Industrial Solutions
        </div>
    </form>

    <!-- JavaScript -->
    <script>
        // 你的 JS 代碼
    </script>
</body>
</html>