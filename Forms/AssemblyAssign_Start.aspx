<%@ page language="C#" autoeventwireup="true"  CodeFile="AssemblyAssign_Start.cs" Inherits="AssemblyAssign_Start" %>

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
            <table class="raSelectPanelContent">
                <tr>
                    <td style="width: 75px; height: 21px">
                        M/O No:</td>
                    <td colspan="2" style="height: 21px">
                        <asp:TextBox ID="txtName" runat="server" CssClass="raTextBox" MaxLength="35" 
                            Width="75px"></asp:TextBox></td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <asp:Button  CssClass="raButton" ID="UpdateButton" runat="server" CausesValidation="True"
                OnClick="InsertButton_Click" Text="Continue"></asp:Button>&nbsp; &nbsp;&nbsp;&nbsp;<br />
        </div>
    <uc1:ErrorHandler ID="ErrorHandler1" runat="server" />
    <br />
    <br />
    </div>
        </div>
    </form>
</body>
</html>
