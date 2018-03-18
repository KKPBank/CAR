<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridviewPageController.ascx.cs" Inherits="Cas.LogServce.Controls.GridviewPageController" %>
<asp:Panel id="pnPageControl" runat="server">
    <table cellpadding="3" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:LinkButton ID="lnbFirst" runat="server" OnClientClick="showWait()" OnClick="lnbFirst_Click">[<<]</asp:LinkButton>
                <asp:LinkButton ID="lnbBack" runat="server" OnClientClick="showWait()" OnClick="lnbBack_Click">[<]</asp:LinkButton>
                หน้าที่
                <asp:DropDownList ID="cmbPage" runat="server" onchange="showWait()" CssClass="Dropdownlist"  Width="60px" AutoPostBack="True" OnSelectedIndexChanged="cmbPage_SelectedIndexChanged">
                </asp:DropDownList>
                จาก&nbsp;<asp:Label ID="lblTotalPage" runat="server"></asp:Label>&nbsp;หน้า 
                <asp:LinkButton ID="lnbNext" runat="server" OnClientClick="showWait()" OnClick="lnbNext_Click">[>]</asp:LinkButton>
                <asp:LinkButton ID="lnbLast" runat="server" OnClientClick="showWait()" OnClick="lnbLast_Click">[>>]</asp:LinkButton>
            </td>
            <td style="text-align:right;">
                <asp:Label ID="lblSummary" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>