<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityLogin.aspx.cs" Inherits="Cas.LogServce.ActivityLogin" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAS Activity Login</title>
    <link href="css/CASStyle.css" rel="stylesheet" />
    <link rel="shortcut icon" href="images/favicon.ico" />
    <style type="text/css">
        center {
            position: relative;
        }

        .v-align {
            position: absolute;
            left: 50%;
            top: 50%;
            -webkit-transform: translate(-50%, -50%);
            -moz-transform: translate(-50%, -50%);
            -ms-transform: translate(-50%, -50%);
            -o-transform: translate(-50%, -50%);
            transform: translate(-50%, -50%);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="center">
            <div class="v-align">
                <fieldset style="width: 500px;" id="pnlCriteria" class="CriteriaLegend">
                    <asp:Label runat="server" ID="lblUnauthorize" ForeColor="Red" Visible="false">Unauthorized Access</asp:Label>
                    <asp:Login ID="Login1" runat="server" OnAuthenticate="LoginAuthenticate">
                        <LayoutTemplate>
                            <table style="margin-left: auto; margin-right: auto">
                                <tr>
                                    <td colspan="2" style="height: 10px;"></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Font-Bold="true" Font-Size="13px">Windows Username:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server" CssClass="TextboxStyle" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ForeColor="Red"
                                            ControlToValidate="UserName" ErrorMessage="User Name is required."
                                            ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Font-Bold="true" Font-Size="13px">Windows Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="TextboxStyle"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ForeColor="Red"
                                            ControlToValidate="Password" ErrorMessage="Password is required."
                                            ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 10px;"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="~/images/btnLogin.gif" CommandName="Login" ValidationGroup="Login1" OnClientClick="showWait()" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center" colspan="2">
                                        <br />
                                        <span style="color: Red;">คุณสามารถเข้าระบบได้ด้วย Account Windows ของคุณ<br />
                                            และห้ามกรอกรหัสผ่านผิดเกิน 3 ครั้ง</span>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:Login>
                </fieldset>
            </div>
        </div>
    </form>
</body>
</html>
