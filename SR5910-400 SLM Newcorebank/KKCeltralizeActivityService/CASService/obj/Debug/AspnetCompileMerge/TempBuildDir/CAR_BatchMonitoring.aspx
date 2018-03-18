<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CAR_BatchMonitoring.aspx.cs" Inherits="Cas.LogServce.CAR_BatchMonitoring" Theme="SkinFile" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controls/TextDateMask.ascx" TagPrefix="uc1" TagName="TextDateMask" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAR Batch Monitoring</title>
    <link href="css/CASStyle.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <link rel="shortcut icon" href="images/favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ScriptManager>
        <asp:UpdatePanel ID="uplSearchResult" runat="server">
            <ContentTemplate>
                <div>
                    <fieldset style="text-align: left; width: 800px;" id="pnlCriteria" class="CriteriaLegend" runat="server">
                        <legend id="Legend1" class="CriteriaHeader" runat="server">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/hSearchCriteria.png" />
                        </legend>
                        <table style="text-align: left; width:100%;" border="0">
                            <tr>
                                <td style="width: 20px"></td>
                                <td style="width: 15%; text-align: right;">
                                    <asp:Label ID="lblProductGroup" runat="server" Text="Batch Data :" CssClass="LabelStyle"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <uc1:TextDateMask runat="server" ID="tdmBatchDate" Width="115px" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: right;"></td>
                                <td style="text-align: left">
                                    <br />
                                    <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/images/btnSearch.gif" OnClick="ibtnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <fieldset style="text-align: left; width: 98%;" id="Fieldset1" class="CriteriaLegend" runat="server">
                        <legend id="Legend2" class="CriteriaHeader" runat="server">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/hSearchResult.png" />
                        </legend>
                        <br />
                        <asp:GridView ID="grdBatchMaster" runat="server" AutoGenerateColumns="false" SkinID="GeneralView" DataKeyNames="BATCH_CODE" Width="100%"
                            OnRowDataBound="GrdBatchMaster_RowDataBound"
                            OnRowCommand="GrdBatchMaster_RowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle Width="1%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    <ItemTemplate>
                                        <img runat="server" id="imgLog" onclick="return false;" alt="" style="cursor: pointer" src="images/plus_log.png" />
                                        <asp:Panel ID="pnlBatchLog" runat="server" Style="display: none">
                                            <asp:GridView ID="grdBatchLog" runat="server" AutoGenerateColumns="false" SkinID="GeneralView" AllowPaging="true" Width="100%"
                                                OnRowDataBound="GrdBatchLog_RowDataBound" 
                                                OnPageIndexChanging="GrdBatchLog_PageIndexChanging"
                                                OnRowCommand="GrdBatchLog_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderStyle Width="1%"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                        <ItemTemplate>
                                                            <img runat="server" id="imgLogDetail" onclick="return false;" alt="" style="cursor: pointer" src="images/plus_logdetail.png" />
                                                            <asp:Panel ID="pnlBatchLogDetail" runat="server" Style="display: none">
                                                                <asp:GridView ID="grdBatchLogDetail" AllowPaging="true" runat="server" AutoGenerateColumns="false" SkinID="GeneralView" Width="500"
                                                                    OnPageIndexChanging="GrdBatchLogDetail_PageIndexChanging">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="REFERENCE_NO" HeaderText="Reference No">
                                                                            <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CHANNEL_ID" HeaderText="Channel Id">
                                                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="100px" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RESPONSE_CODE" HeaderText="Response Code">
                                                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RESPONSE_MESSAGE" HeaderText="Response Message">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SYSTEM_CODE" HeaderText="System Code">
                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SERVICE_NAME" HeaderText="Service Name">
                                                        <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="3%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="REFERENCE_CODE" HeaderText="Reference Code">
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BATCH_ROUND" HeaderText="Round">
                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FILE_NAME" HeaderText="File Name">
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="START_TIME" HeaderText="Start Time">
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="END_TIME" HeaderText="End Time">
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TOTAL_HEADER" HeaderText="Total Header">
                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TOTAL_DETAIL" HeaderText="Total Detail">
                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TOTAL_COMPLETE" HeaderText="Total Complete">
                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TOTAL_FAIL" HeaderText="Total Fail">
                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="STATUS" HeaderText="Status">
                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ERROR_DETAIL" HeaderText="Error Detail">
                                                        <HeaderStyle HorizontalAlign="Center"/>
                                                        <ItemStyle HorizontalAlign="Left"/>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Run">
                                                        <HeaderStyle Width="1%" HorizontalAlign="Center" ></HeaderStyle>
                                                        <ItemStyle Width="1%" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnRun" runat="server" ImageUrl="~/images/run.png" CommandName="RunLogCommand"
                                                                CommandArgument='<%# Eval("BATCH_LOG_ID") %>' AlternateText="Run" ToolTip="Run" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BATCH_NAME" HeaderText="Batch Name">
                                    <HeaderStyle Width="45%" HorizontalAlign="Center" />
                                    <ItemStyle Width="45%" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="START_TIME" HeaderText="Start Time">
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="END_TIME" HeaderText="End Time">
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle Width="15%" HorizontalAlign="center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STATUS" HeaderText="Status">
                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                    <ItemStyle Width="10%" HorizontalAlign="center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Run">
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnRun" runat="server" ImageUrl="~/images/run.png" CommandName="RunCommand"
                                            CommandArgument='<%# Eval("BATCH_CODE") %>' AlternateText="Run" ToolTip="Run" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="GridViewNoData">
                                    <br />
                                    <asp:Label ID="lblEmptyData" runat="server" Text="No data found." CssClass="LabelStyle"></asp:Label>
                                </div>
                            </EmptyDataTemplate>
                            <PagerTemplate />
                        </asp:GridView>
                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <ajaxToolkit:UpdatePanelAnimationExtender ID="upae" BehaviorID="animation" runat="server" TargetControlID="uplSearchResult">
            <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="onUpdating();" />  
                 </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onUpdated();" /> 
                </Parallel> 
            </OnUpdated>
            </Animations>
        </ajaxToolkit:UpdatePanelAnimationExtender>
        <!-- Show Processing -->
        <asp:UpdatePanel runat="server" ID="upProcessing">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnDisplayProcessing" CssClass="displayNone" />
                <asp:Panel runat="server" ID="pnlDisplayProcessing" Style="display: none; width: 400px;" CssClass="modalPopupProcessing">
                    <table style="height: 100px; width: 100%;">
                        <tr>
                            <td style="text-align: center; vertical-align: middle">
                                <asp:Image runat="server" ID="imgWait" ImageAlign="AbsMiddle" ImageUrl="~/images/Processing.gif" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="mpeProcessing" runat="server" TargetControlID="btnDisplayProcessing" PopupControlID="pnlDisplayProcessing" BackgroundCssClass="modalBackground" DropShadow="false">
                </ajaxToolkit:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- / Show Processing -->
    </form>
</body>
</html>
<script type="text/javascript">
    var objImage;
    $("[src*=plus_log]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '15'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "images/minus_log.png");
    });
    $("[src*=minus_log]").live("click", function () {
        $(this).attr("src", "images/plus_log.png");
        //ซ่อน batch log ที่แสดงจากการกดปุ่ม [+] จากข้างบน
        $(this).closest("tr").next().remove();
    });
    $("[src*=plus_logdetail]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '8'>" + $(this).next().html().next().html() + "</td></tr>")
        $(this).attr("src", "images/minus_logdetail.png");
    });
    $("[src*=minus_logdetail]").live("click", function () {
        $(this).attr("src", "images/plus_logdetail.png");
        //ซ่อน batch log detail ที่แสดงจากการกดปุ่ม [+] จากข้างบน
        $(this).closest("tr").next().remove();
    });

    function ExpandGrid(imgName) {
        document.getElementById(imgName).click();
    }

    function onUpdating() {
        var modal = $find('mpeProcessing');
        modal.show();
    }

    function onUpdated() {
        var modal = $find('mpeProcessing');
        modal.hide();
    }
</script>
