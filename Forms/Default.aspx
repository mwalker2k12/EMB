<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
            font-weight: bold;
        }
        .auto-style1
        {
            font-weight: bold;
            height: 21px;
            width: 164px;
        }
        .auto-style2
        {
            height: 21px;
            width: 164px;
        }
        .auto-style3
        {
            height: 21px;
        }
        .auto-style4
        {
            width: 100px;
            height: 21px;
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
<td style="width:80%; text-align:center"><b>Receive Emballage </b></td>
<td style="width:10%"></td>

</tr>
</table>
<div id="selectionPanel">

        <div id="selectonTable" runat="server" class="raPanel">
            <strong><span style="color: #993333"></span></strong><br />
            <table class="raSelectPanelContent">
                <tr>
                    <td class="auto-style1">
                        E-Container #</td>
                    <td style="height: 21px" class="style1">
                        Qty Received</td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        500 Small Box</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt500" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                    </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        750 Small-Med Box</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt750" runat="server" CssClass="raTextBox" MaxLength="35" Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        780 Med-Large Box</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt780" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        840 Large Box</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt840" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        1 Pallet L (4&#39;x2.75&#39;)</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt1" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        91 Pallet Lid (Pallet L)</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt841" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        2 Pallet K (2.75&#39; x 2&#39;)</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt2" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        92 Pallet LId (Pallet K)</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt92" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        71 Wooden Separator (Pallet L)</td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txt71" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="color: #000000; " class="auto-style4">
                        </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        72 Wooden Separator (Pallet K)</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt72" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        21 Side Rails (Pallet L)</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt21" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        22 Side Rails (Pallet K)</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txt22" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="50px"></asp:TextBox>
                        </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                        &nbsp;</td>
                </tr>
                </table>
 
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
