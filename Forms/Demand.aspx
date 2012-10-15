<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demand.aspx.cs" Inherits="Demand" %>

<%@ Register Src="../CustomControls/WiControl.ascx" TagName="WiControl" TagPrefix="uc4" %>

<%@ Register Src="../CustomControls/ErrorHandler.ascx" TagName="ErrorHandler" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../CustomControls/Demand.ascx" tagname="Demand" tagprefix="uc2" %>

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
<td style="border:solid 1px black" style="width:10%"></td>
<td style="width:80%; text-align:center">Emballage Demand</td>
<td style="width:10%"></td>

</tr>
</table>
<div id="selectionPanel">

        <div id="selectonTable" runat="server" class="raPanel">
            <table>
            <tr><td>
            <uc2:Demand ID="Demand1" PkgNo="500" runat="server" />
            </td><td>&nbsp;&nbsp;&nbsp;</td>
                <td>
            <uc2:Demand ID="Demand2" PkgNo="750" runat="server" />
                </td></tr>
            <tr><td>
                &nbsp;</td><td>&nbsp;</td>
                <td>
                    &nbsp;</td></tr>
            <tr><td>
            <uc2:Demand ID="Demand3" PkgNo="780" runat="server" />
            </td><td>&nbsp;&nbsp;&nbsp;</td>
                <td>
            <uc2:Demand ID="Demand4" PkgNo="840" runat="server" />
                </td></tr>
            </table>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        
            <hr />
           
      
        </div>
    <uc1:ErrorHandler ID="ErrorHandler1" runat="server" />
    <br />
    <br />
    </div>
        </div>
    </form>
</body>
</html>
