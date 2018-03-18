<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextDateMask.ascx.cs" Inherits="Cas.LogServce.Controls.TextDateMask" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 
<asp:TextBox ID="txtDate" runat="server" Width="100px" CssClass="Textbox"  OnTextChanged="txtDate_OnTextChanged"  ></asp:TextBox>
<asp:ImageButton runat="Server" ID="imgCalendar" ImageUrl="~/Images/calendar.gif" Width="16px" ToolTip="Click to show calendar" ImageAlign="Absmiddle" />
<ajaxToolkit:CalendarExtender ID="txtDate_CalendarExtender" runat="server"
    TargetControlID="txtDate" PopupButtonID="imgCalendar" Format="dd/MM/yyyy" ClearTime="False" >
</ajaxToolkit:CalendarExtender>
<ajaxToolkit:MaskedEditExtender ID="txtDate_MaskedEditExtender" runat="server"
    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
    Mask="99/99/9999" MaskType="Date" TargetControlID="txtDate" 
    UserDateFormat="DayMonthYear" CultureName="en-US" ClearTextOnInvalid="True" 
    ErrorTooltipEnabled="True"
    MessageValidatorTip="False" Century="1900">
</ajaxToolkit:MaskedEditExtender>