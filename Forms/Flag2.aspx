<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Flag2.aspx.cs" Inherits="Flag" %>

<%@ Register Src="../CustomControls/WiControl.ascx" TagName="WiControl" TagPrefix="uc4" %>

<%@ Register Src="../CustomControls/ErrorHandler.ascx" TagName="ErrorHandler" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Navigator</title>
    <link href="../ihc.css" rel="stylesheet" type="text/css" />
  


    <style type="text/css">
   
        .style1
        {
            width: 63%;
        }
        .style2
        {
            width: 101px;
        }
        .style3
        {
        }
        .style4
        {
            width: 147px;
        }
        .style5
        {
            width: 133px;
        }
        .style6
        {
            width: 101px;
            height: 26px;
        }
        .style7
        {
            width: 133px;
            height: 26px;
        }
        .style8
        {
            width: 147px;
            height: 26px;
        }
        .style9
        {
            height: 26px;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
<div id="mainDiv">
    <div id="menu">
    </div>

   <%-- <div class="logo">
		<h1>Industrial Harness:<span class="orange"><i>NCM Log</i></span></h1>
	</div>
<hr />
<br />
<div class="separator"></div>--%>



    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc4:WiControl ID="WiControl1" runat="server" PageId="WNCM_I" />
    <br />
<table class="raPanelHeader" style="width:100%">
<tr>
<td style="width:10%"></td>
<td style="width:80%; text-align:center"><b>Inspection Flag</b></td>
<td style="width:10%"></td>

</tr>
</table>
<div id="selectionPanel">

        <div id="selectonTable" runat="server" class="raPanel">
            <table class="raSelectPanelContent"">
                <tr>
                      <td style="width:91px; height: 21px;">
                        Part Number:</td>
                    <td style="height: 21px" colspan="2">
                        <asp:TextBox ID="txtPartNbr" runat="server" MaxLength="35" Width="150px" />&nbsp;
                        <cc1:AutoCompleteExtender MinimumPrefixLength="2" CompletionInterval="500" ServiceMethod="GetPartNumbers" TargetControlID="txtPartNbr" ID="AutoCompleteExtender1" runat="server">
                        </cc1:AutoCompleteExtender>
                    </td>
 
                </tr>
                <tr>
                    <td class="style2">
                       
                        Entered By:</td><td class="style5">
               
                         <asp:TextBox ID="txtEntBy" runat="server" MaxLength="30" Width="50px"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtEntBy_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtEntBy">
                        </cc1:AutoCompleteExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ControlToValidate="txtEntBy" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    <td class="style4">
               
                         &nbsp;</td>
                    <td>
               
                         &nbsp;</td>
                </tr>
                <tr>
                    <td class="style6">
                       
                        Entry Date:</td><td class="style7">
               
                         <asp:TextBox ID="txtEntDATE" runat="server" MaxLength="30" Width="75px"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtEntDATE_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtEntDATE">
                        </cc1:AutoCompleteExtender>
                         <asp:ImageButton ID="ImageButton5" runat="server" 
                             ImageUrl="~/Images/calendar.png" />
                         <cc1:CalendarExtender ID="CalendarExtender6" runat="server" 
                             PopupButtonID="ImageButton5" TargetControlID="txtEntDATE">
                         </cc1:CalendarExtender>
                         <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" 
                             Mask="99/99/9999" MaskType="date" TargetControlID="txtEntDATE">
                         </cc1:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ControlToValidate="txtEntDATE" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style8">
               
                         </td>
                    <td class="style9">
               
                         </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Reason for Inspection Flag:</td>
                    <td class="style4">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style3" colspan="2">
               
                         <asp:TextBox ID="txtReason" runat="server" MaxLength="200" Width="235px" 
                            Height="90px" TextMode="MultiLine"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtReason_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtReason">
                        </cc1:AutoCompleteExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                            ControlToValidate="txtReason" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    <td>
               
                         &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" runat="server" CssClass="raButtonWide" 
                            Text="Save New Insp. Flag" onclick="Button1_Click" />
                    </td>
                    <td class="style4">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2" >
                        Insp. History</td><td class="style5">
               
                         <asp:TextBox ID="txtdte1" runat="server" MaxLength="30" Width="75px"></asp:TextBox>

                          <asp:ImageButton ID="ImageButton01" runat="server" 
                            ImageUrl="~/Images/calendar.png" />
                        <cc1:CalendarExtender ID="CalendarExtender5" runat="server" 
                            PopupButtonID="ImageButton01" TargetControlID="txtdte1">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" 
                            Mask="99/99/9999" MaskType="date" TargetControlID="txtdte1">
                        </cc1:MaskedEditExtender>

                        <cc1:AutoCompleteExtender ID="txtdte1_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtdte1">
                        </cc1:AutoCompleteExtender>
                        <cc1:TextBoxWatermarkExtender ID="water1" runat="server" TargetControlID="txtdte1" WatermarkText="(date)" /></td>
                    <td class="style4">
               
                         <asp:TextBox ID="txtdte2" runat="server" MaxLength="30" Width="75px"></asp:TextBox>

                           <asp:ImageButton ID="ImageButton2" runat="server" 
                            ImageUrl="~/Images/calendar.png" />
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                            PopupButtonID="ImageButton2" TargetControlID="txtdte2">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                            Mask="99/99/9999" MaskType="date" TargetControlID="txtdte2">
                        </cc1:MaskedEditExtender>

                        <cc1:AutoCompleteExtender ID="txtdte2_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtdte2">
                        </cc1:AutoCompleteExtender><cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtdte2" WatermarkText="(date)" /></td>
                    <td>
               
                         <asp:TextBox ID="txtdte3" runat="server" MaxLength="30" Width="75px"></asp:TextBox>

                           <asp:ImageButton ID="ImageButton3" runat="server" 
                            ImageUrl="~/Images/calendar.png" />
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
                            PopupButtonID="ImageButton3" TargetControlID="txtdte3">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
                            Mask="99/99/9999" MaskType="date" TargetControlID="txtdte3">
                        </cc1:MaskedEditExtender>

                        <cc1:AutoCompleteExtender ID="txtdte3_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtdte3">
                        </cc1:AutoCompleteExtender><cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtdte3" WatermarkText="(date)" /></td>
                </tr>
                <tr>
                    <td class="style2" >
                        &nbsp;</td><td class="style5">
               
                         <asp:TextBox ID="txtqty1" runat="server" MaxLength="30" Width="50px"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtqty1_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtqty1">
                        </cc1:AutoCompleteExtender><cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtqty1" WatermarkText="(qty)" /></td>
                    <td class="style4">
               
                         <asp:TextBox ID="txtqty2" runat="server" MaxLength="30" Width="50px"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtqty2_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtqty2">
                        </cc1:AutoCompleteExtender><cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtqty2" WatermarkText="(qty)" /></td>
                    <td>
               
                         <asp:TextBox ID="txtqty3" runat="server" MaxLength="30" Width="50px"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtqty3_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtqty3">
                        </cc1:AutoCompleteExtender><cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="txtqty3" WatermarkText="(qty)" /></td>
                </tr>
                <tr>
                    <td class="style2" >
                        &nbsp;</td><td class="style5">
               
                         <asp:TextBox ID="txtnotes1" runat="server" MaxLength="30" Width="138px" 
                            Height="40px" TextMode="MultiLine"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" TargetControlID="txtnotes1" WatermarkText="(notes)" />
                        <cc1:AutoCompleteExtender ID="txtnotes1_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtnotes1">
                        </cc1:AutoCompleteExtender></td>
                    <td class="style4">
               
                         <asp:TextBox ID="txtnotes2" runat="server" MaxLength="30" Width="138px" 
                            Height="40px" TextMode="MultiLine"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtnotes2_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtnotes2">
                        </cc1:AutoCompleteExtender></td>
                    <td>
               
                         <asp:TextBox ID="txtnotes3" runat="server" MaxLength="30" Width="138px" 
                            Height="40px" TextMode="MultiLine"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtnotes3_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtnotes3">
                        </cc1:AutoCompleteExtender></td>
                </tr>
                <tr>
                    <td>Insp. Status:</td>
                    <td class="style5">
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="raDropDownList">
                            <asp:ListItem Value="select">[Select]</asp:ListItem>
                            <asp:ListItem Value="0">Active</asp:ListItem>
                            <asp:ListItem Value="1">Closed</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" 
                            ClientValidationFunction="validateDdl" ControlToValidate="DropDownList1" 
                            ErrorMessage="*"></asp:CustomValidator>
                    </td>
                    <td class="style4">
               
                         &nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="style5" id="td1" runat="server">
               
                         <asp:TextBox ID="txtcloseddte" runat="server" MaxLength="30" Width="75px"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="txtcloseddte_AutoCompleteExtender" runat="server" 
                            CompletionInterval="500" MinimumPrefixLength="2" ServiceMethod="GetPartNumbers" 
                            TargetControlID="txtcloseddte">
                        </cc1:AutoCompleteExtender>

                         <asp:ImageButton ID="ImageButton4" runat="server" 
                            ImageUrl="~/Images/calendar.png" />
                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" 
                            PopupButtonID="ImageButton4" TargetControlID="txtcloseddte">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" 
                            Mask="99/99/9999" MaskType="date" TargetControlID="txtcloseddte">
                        </cc1:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                            ControlToValidate="txtcloseddte" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style4">
               
                         &nbsp;</td>
                </tr>
            </table>
            <br />
            <br />
            <hr />
            &nbsp; &nbsp;&nbsp;&nbsp;<br />
      
        </div>
    <uc1:ErrorHandler ID="ErrorHandler1" runat="server" />
    <br />
    <br />
    </div>
        </div>
    </form>
</body>
</html>
