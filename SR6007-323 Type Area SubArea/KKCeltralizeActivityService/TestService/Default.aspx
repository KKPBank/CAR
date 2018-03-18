<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestService.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" ID="btnTest" Text="Test" OnClick="btnTest_Click" /><br />
        <asp:TextBox runat="server" ID="txtResult" TextMode="MultiLine" Height="200px" Width="500px"></asp:TextBox>
        <hr />
        <asp:Button runat="server" ID="btnTestInqurey" Text="Test Inquery" OnClick="btnTestInqurey_Click" />
        <asp:GridView runat="server" ID="gvMain"></asp:GridView>
        <asp:Button runat="server" ID="btnGenTxt" Text="Gen Text" OnClick="btnGenTxt_Click" />
        <asp:Button runat="server" ID="btnGenObj" Text="Gen Object" OnClick="btnGenObj_Click" />
    </div>
    </form>
</body>
</html>
