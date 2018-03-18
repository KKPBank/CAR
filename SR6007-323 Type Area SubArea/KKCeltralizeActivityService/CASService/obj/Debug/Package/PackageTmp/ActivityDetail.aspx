<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityDetail.aspx.cs" Inherits="Cas.LogServce.ActivityDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controls/TextDateMask.ascx" TagPrefix="uc1" TagName="TextDateMask" %>
<%@ Register Src="~/Controls/GridviewPageController.ascx" TagPrefix="uc1" TagName="GridviewPageController" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAS Activity Details</title>
    <link href="css/CASStyle.css" rel="stylesheet" />
    <link rel="shortcut icon" href="images/favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="updMain">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hidUserId" />
                <div id="divSearch" runat="server">
                    <div class="errorTxT"><asp:Label runat="server" ID="lblError" ViewStateMode="Disabled" ForeColor="Red"></asp:Label></div>
                    <div class="casTitle">Customer Information</div>
                    <div class="searchBox">
                        <table>
                            <tr>
                                <td class="title">Subscription ID</td>
                                <td><asp:TextBox runat="server" ID="txtSSubscription"></asp:TextBox></td>
                                <td class="title">Lead ID</td>
                                <td><asp:DropDownList runat="server" ID="cmbLeadId"><asp:ListItem Value="">ทั้งหมด</asp:ListItem></asp:DropDownList><asp:TextBox Visible="false" runat="server" ID="txtSLead"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="title">Ticket ID</td>
                                <td><asp:DropDownList runat="server" ID="cmbTicketId"><asp:ListItem Value="">ทั้งหมด</asp:ListItem></asp:DropDownList><asp:TextBox runat="server" Visible="false" ID="txtSTicket"></asp:TextBox></td>
                                <td class="title">Contract ID</td>
                                <td><asp:DropDownList runat="server" ID="cmbContractId"><asp:ListItem Value="">ทั้งหมด</asp:ListItem></asp:DropDownList><asp:TextBox Visible="false" runat="server" ID="txtSContract"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="title">Subscription Type</td>
                                <td><asp:DropDownList runat="server" ID="cmbSSubscriptionType"><asp:ListItem Value="0">ทั้งหมด</asp:ListItem></asp:DropDownList></td>
                                <td class="title">Non-Customer ID</td>
                                <td><asp:DropDownList runat="server" ID="cmbNoncusId"><asp:ListItem Value="">ทั้งหมด</asp:ListItem></asp:DropDownList><asp:TextBox Visible="false" runat="server" ID="txtSNonCustomer"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="title">SR ID</td>
                                <td><asp:DropDownList runat="server" ID="cmbSrId"><asp:ListItem Value="">ทั้งหมด</asp:ListItem></asp:DropDownList><asp:TextBox Visible="false" runat="server" ID="txtSSR"></asp:TextBox></td>
                                <td class="title">Reference App ID</td>
                                <td><asp:DropDownList runat="server" ID="cmbReferenceAppID"><asp:ListItem Value="">ทั้งหมด</asp:ListItem></asp:DropDownList><asp:TextBox Visible="false" runat="server" ID="txtSReferenceApp"></asp:TextBox></td>
                            </tr>
                        </table>

                    </div>
                    <div class="casTitle">Condition Information</div>
                    <div class="searchBox">
                        <table>
                            <tr>
                                <td class="title">Channel</td>
                                <td><asp:DropDownList runat="server" ID="cmbSChannel"><asp:ListItem Value="">ทั้งหมด</asp:ListItem></asp:DropDownList></td>
                                <td class="title">System</td>
                                <td><asp:DropDownList runat="server" ID="cmbSSystem"><asp:ListItem Value="">ทั้งหมด</asp:ListItem></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="title">Activity Date</td>
                                <td>
                                    <uc1:TextDateMask runat="server" id="tdmFrom" Width="115px" /> 
                                    to
                                    <uc1:TextDateMask runat="server" id="tdmTo" Width="115px" /> 
                                </td>
                                <td class="title">Type</td>
                                <td><asp:DropDownList runat="server" ID="cmbSType"><asp:ListItem Value="0">ทั้งหมด</asp:ListItem></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="title">Activity Type</td>
                                <td>
                                    <asp:DropDownList ID="cmbSActivityType" runat="server">
                                        <asp:ListItem Value="">ทั้งหมด</asp:ListItem>
                                        <asp:ListItem>Todo</asp:ListItem>
                                        <asp:ListItem>Call Back</asp:ListItem>
                                        <asp:ListItem>Call Outbound</asp:ListItem>
                                        <asp:ListItem>Call Inbound</asp:ListItem>
                                        <asp:ListItem>E-Mail Outbound</asp:ListItem>
                                        <asp:ListItem>E-Mail Inbound</asp:ListItem>
                                        <asp:ListItem>Batch Inbound</asp:ListItem>
                                        <asp:ListItem>Mailing</asp:ListItem>
                                        <asp:ListItem>Appointment</asp:ListItem>
                                        <asp:ListItem>FYI</asp:ListItem>
                                        <asp:ListItem>SMS Sending</asp:ListItem>
                                        <asp:ListItem>Fax Inbound</asp:ListItem>
                                        <asp:ListItem>Fax Outbound</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="title">Campaign</td>
                                <td>
                                    <asp:DropDownList ID="cmbSCampaign" runat="server">
                                        <asp:ListItem Value="">ทั้งหมด</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="title">Product</td>
                                <td>
                                    <asp:DropDownList ID="cmbSProduct" runat="server">
                                        <asp:ListItem Value="">ทั้งหมด</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="title">Product Group</td>
                                <td>
                                    <asp:DropDownList ID="cmbSProductGroup" runat="server">
                                        <asp:ListItem Value="">ทั้งหมด</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="title">Area</td>
                                <td>
                                    <asp:DropDownList ID="cmbSArea" runat="server">
                                        <asp:ListItem Value="0">ทั้งหมด</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="title">Sub Area</td>
                                <td>
                                    <asp:DropDownList ID="cmbSSubArea" runat="server">
                                        <asp:ListItem Value="0">ทั้งหมด</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trStatus" visible="false">
                                <td class="title">Status</td>
                                <td>
                                    <asp:TextBox ID="txtSStatus" runat="server"></asp:TextBox>
                                </td>
                                <td class="title">Sub Status</td>
                                <td>
                                    <asp:TextBox ID="txtSSubStatus" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        &nbsp;
                        <hr />
                        &nbsp;
                        <div style="text-align: center;">
                            <asp:Button runat="server" Text="Search" ID="btnSearch" OnClick="BtnSearch_Click" OnClientClick="showWait()" />
                            <asp:Button runat="server" Text="Clear" ID="btnClear" OnClick="BtnClear_Click" />
                        </div>
                    </div>
                    <div class="result">
                        <uc1:GridviewPageController runat="server" ID="ctlPageControlTop" Visible="false" />
                        <asp:GridView runat="server" AutoGenerateColumns="false" ID="gvMain" Width="100%" CssClass="resultTable" OnRowDataBound="GvMain_RowDataBound" EmptyDataText="** ไม่พบรายการ **" >
                            <Columns>
                                <asp:TemplateField HeaderText="No">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Activity Date" DataField="ActivityDateTime" HtmlEncode="false" DataFormatString="{0:d MMM yyyy HH:mm:ss}" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Lead Id" DataField="LeadID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Ticket Id" DataField="TicketID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Contract Id" DataField="ContractID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Non-Customer Id" DataField="NoneCustomerID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="SR Id" DataField="SrID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Reference App Id" DataField="ReferenceAppID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="PDM Product Group Id" DataField="PDMProductGroupID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="PDM Product Sub Group Id" DataField="PDMProductSubGroupID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="PDM Product Id" DataField="PDMProductID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="PDM Campaign Id" DataField="PDMCampaignID" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="System" DataField="SystemName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Type" DataField="TypeName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Campaign" DataField="CampaignName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Product Group" DataField="ProductGroupName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Product" DataField="ProductName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Channel" DataField="ChannelName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="ActivityType" DataField="ActivityTypeName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Area" DataField="AreaName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Sub Area" DataField="SubAreaName" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Status" DataField="Status" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Sub Status" DataField="SubStatus" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnbViewDetail" CommandArgument='<%# Eval("ActivityID") %>' OnClick="LnbViewDetail_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                        <uc1:GridviewPageController runat="server" ID="ctlPageControlBot" Visible="false" />
                    </div>
                    <div class="resultdetail">
                        <input type="hidden" name="selectedtab" id="selectedtab" />
                        <ajaxToolkit:TabContainer ID="tabDetails" runat="server" Width="100%" ActiveTabIndex="0" TabStripPlacement="Top" CssClass="ajax__tab_xp" Height="300px" Visible="false" ScrollBars="Auto" >
                            <ajaxToolkit:TabPanel ID="tabActivity" HeaderText="Activity Information" runat="server" >
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gvActivity" AutoGenerateColumns="false" EmptyDataText="** ไม่พบข้อมูล **" CssClass="detailTable">
                                        <Columns>
                                            <asp:BoundField HeaderText="Title" DataField="DataLabel">
                                                <ItemStyle Width="250px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Details" DataField="DataValue" HtmlEncode="false">
                                                <ItemStyle Width="400px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="tabCustomer" runat="server" HeaderText="Customer Information">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gvCustomer" AutoGenerateColumns="false" EmptyDataText="** ไม่พบข้อมูล **" CssClass="detailTable">
                                        <Columns>
                                            <asp:BoundField HeaderText="Title" DataField="DataLabel">
                                                <ItemStyle Width="250px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Details" DataField="DataValue" HtmlEncode="false">
                                                <ItemStyle Width="400px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="tabProduct" runat="server" HeaderText="Product Information">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gvProduct" AutoGenerateColumns="false" EmptyDataText="** ไม่พบข้อมูล **" CssClass="detailTable">
                                        <Columns>
                                            <asp:BoundField HeaderText="Title" DataField="DataLabel">
                                                <ItemStyle Width="250px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Details" DataField="DataValue" HtmlEncode="false">
                                                <ItemStyle Width="400px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="tabContract" runat="server" HeaderText="Contract Information">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gvContract" AutoGenerateColumns="false" EmptyDataText="** ไม่พบข้อมูล **" CssClass="detailTable">
                                        <Columns>
                                            <asp:BoundField HeaderText="Title" DataField="DataLabel">
                                                <ItemStyle Width="250px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Details" DataField="DataValue" HtmlEncode="false">
                                                <ItemStyle Width="400px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="tabOfficer" runat="server" HeaderText="Officer Information">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gvOfficer" AutoGenerateColumns="false" EmptyDataText="** ไม่พบข้อมูล **" CssClass="detailTable">
                                        <Columns>
                                            <asp:BoundField HeaderText="Title" DataField="DataLabel">
                                                <ItemStyle Width="250px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Details" DataField="DataValue" HtmlEncode="false">
                                                <ItemStyle Width="400px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </div>        
                 </div>
                <asp:Label runat="server" ID="lblUnauthorize" ForeColor="Red" Visible="false">Unauthorized Access</asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="updModal" UpdateMode="Always"><ContentTemplate>
        <div id="zModal-BG"></div>
        <div id="zModal"><asp:Image runat="server" ID="imgWait" ImageUrl="~/images/wait.gif" ImageAlign="AbsMiddle" /> กรุณารอสักครู่</div>
    </ContentTemplate></asp:UpdatePanel>
    </form>
    <script type="text/javascript">
        function showWait() {
            var winW, winH;
            if (document.compatMode == 'CSS1Compat' &&
                 document.documentElement &&
                 document.documentElement.offsetWidth) {
                winW = document.documentElement.offsetWidth;
                winH = document.documentElement.offsetHeight;
            }
            if (window.innerWidth && window.innerHeight) {
                winW = window.innerWidth;
                winH = window.innerHeight;
            }

            var mw = (winW / 2) - 100;
            var mh = (winH / 2) - 100;

            el = document.getElementById('zModal');
            el2 = document.getElementById('zModal-BG');
            el.style.left = mw + 'px';
            el.style.visibility = "visible";
            el.style.display = "block";
            el2.style.visibility = "visible";
            el2.style.display = "block";

        }

        function hideWait() {
            el = document.getElementById('zModal');
            el2 = document.getElementById('zModal-BG');
            el.style.visibility = "hidden";
            el.style.display = "none";
            el2.style.visibility = "hidden";
            el2.style.display = "none";
        }
         

    </script>
</body>
</html>
