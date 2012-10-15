<%@ page language="C#" autoeventwireup="true" inherits="AssemblyAssign" CodeFile="AssemblyAssign.cs" %>

<%@ Register Src="../CustomControls/WiControl.ascx" TagName="WiControl" TagPrefix="uc4" %>

<%@ Register Src="../CustomControls/ErrorHandler.ascx" TagName="ErrorHandler" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Navigator</title>
    <link href="../ihc.css" rel="stylesheet" type="text/css" />
     <script language="javascript" type="text/javascript" src="../JScript.js">
  
    </script>

    <link href="../ihc.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 21px;
            width: 280px;
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
    <br />
<table class="raPanelHeader" style="width:100%">
<tr>
<td style="width:10%"></td>
<td style="width:80%; text-align:center"><b>Assembly Assignment</b></td>
<td style="width:10%"></td>

</tr>
</table>
<div id="selectionPanel">

        <div id="selectonTable" runat="server" class="raPanel">
            <strong><span style="color: #993333"></span></strong><br />
                              <asp:UpdatePanel runat="server" ID="ud1">
                    <ContentTemplate>
            <table class="raSelectPanelContent">
                <tr>
                    <td style="width: 114px; height: 21px">
                        M/O No:</td>
                    <td class="style1">
                        <asp:Label ID="lblMo" runat="server" CssClass="raLabel" Text="Label" 
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 114px; height: 21px">
                        Assembly No:</td>
                    <td class="style1">
                        <asp:Label ID="lblAssy" runat="server" CssClass="raLabel" Text="Label" 
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 114px; height: 21px">
                        M/O Qty:</td>
                    <td class="style1">
                        <asp:Label ID="lblQty" runat="server" CssClass="raLabel" Text="Label" 
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 114px; height: 21px">
                        Assy Category:</td>
                    <td class="style1">
                        <asp:Label ID="lblAssyCatg" runat="server" CssClass="raLabel" Font-Bold="True" 
                            Text="Label"></asp:Label>
                    </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>

                    <td style="width: 114px; height: 21px">
                        I.S.I.R.?</td>
                    <td class="style1">
                  
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                            RepeatDirection="Horizontal" AutoPostBack="True" 
                            Font-Bold="True" 
                            onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                            <asp:ListItem Value="False">No</asp:ListItem>
                        </asp:RadioButtonList>
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="RadioButtonList1" ErrorMessage="*"></asp:RequiredFieldValidator>
                        
                    </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 114px; height: 21px">
                        Assembler:</td>
                    <td style="vertical-align: middle;" >
                        <asp:DropDownList ID="DropDownList4" runat="server" CssClass="raDropDownList" 
                            DataSourceID="srcEmployees" DataTextField="Column1" DataValueField="EMP_NO" 
                            ondatabound="DropDownList4_DataBound" AppendDataBoundItems="True" 
                            Font-Bold="True" 
                            >
                            <asp:ListItem Value="select">[Select]</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" 
                            ClientValidationFunction="validateDdl" ControlToValidate="DropDownList4" 
                            ErrorMessage="*"></asp:CustomValidator>
                    </td>
           
                    <td style="width: 100px; color: #000000; height: 21px">
                        <asp:SqlDataSource ID="srcEmployees" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td style="width: 114px; height: 21px">
                        Assembly Location:</td>
                    <td style="vertical-align: middle;" class="style1">
                        <asp:DropDownList ID="DropDownList5" runat="server" AppendDataBoundItems="True" 
                            CssClass="raDropDownList" DataSourceID="srcLocations" DataTextField="Column1" 
                            DataValueField="ISIR_LOC_NO" Font-Bold="True" 
                            ondatabound="DropDownList4_DataBound">
                            <asp:ListItem Value="select">[Select]</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" 
                            ClientValidationFunction="validateDdl" ControlToValidate="DropDownList5" 
                            ErrorMessage="*"></asp:CustomValidator>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        <asp:SqlDataSource ID="srcLocations" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" SelectCommand="SELECT ISIR_LOC_NO, LTRIM(STR(ISIR_LOC_NO)) + ' - ' + ISIR_LOC_DESC
FROM ISIR_LOCATIONS
ORDER BY ISIR_LOC_NO ASC"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>
                     </ContentTemplate>
                        </asp:UpdatePanel>
            <br />
            <hr />
            <asp:Button  CssClass="raButton" ID="UpdateButton" runat="server" CausesValidation="True"
                OnClick="InsertButton_Click" Text="Save"></asp:Button>&nbsp; &nbsp;&nbsp;&nbsp;<br />
      
        </div>
    <uc1:ErrorHandler ID="ErrorHandler1" runat="server" />
    <br />
    <br />
    </div>
        </div>
    </form>
</body>
</html>
