<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Log.aspx.cs" Inherits="Log" %>

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
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSource1" EnableModelValidation="True" 
                >
                <AlternatingRowStyle CssClass="altRow" />
                <Columns>
                    <asp:BoundField DataField="EMB_REC_DTE" HeaderText="Receipt&lt;br/&gt;Date" 
                        HtmlEncode="False" SortExpression="EMB_REC_DTE" />
                    <asp:BoundField DataField="x500" HeaderText="500&lt;br/&gt;Small Box" 
                        HtmlEncode="False" SortExpression="x500" />
                    <asp:BoundField DataField="x750" HeaderText="750&lt;br/&gt;Small-Med Box" 
                        HtmlEncode="False" SortExpression="x750" />
                    <asp:BoundField DataField="x780" HeaderText="780&lt;br/&gt;Med-Large Box" 
                        HtmlEncode="False" SortExpression="x780" />
                    <asp:BoundField DataField="x840" HeaderText="840&lt;br/&gt;Large Box" 
                        HtmlEncode="False" SortExpression="x840" />
                    <asp:BoundField DataField="x1" HeaderText="1 Pallet L&lt;br/&gt;(4'x2.75')" 
                        HtmlEncode="False" SortExpression="x1" />
                    <asp:BoundField HeaderText="91&lt;br/&gt;Pallet Lid&lt;br/&gt;Pallet L " 
                        HtmlEncode="False" DataField="x91" />
                    <asp:BoundField DataField="x2" HeaderText="2 Pallet K&lt;br/&gt;(2.75' x 2')" 
                        HtmlEncode="False" SortExpression="x2" />
                    <asp:BoundField DataField="x92" HeaderText="92&lt;br/&gt;Pallet Lid&lt;br/&gt;Pallet K" 
                        HtmlEncode="False" SortExpression="x92" />
                    <asp:BoundField DataField="x71" HeaderText="71 Wooden Separator&lt;br/&gt; Pallet L" HtmlEncode="False" />
                    <asp:BoundField DataField="x72" HeaderText="72 Wooden Separator &lt;br/&gt;Pallet K" HtmlEncode="False" />
                    <asp:BoundField DataField="x21" HeaderText="21 Side Rails &lt;br/&gt;Pallet L" HtmlEncode="False" />
                    <asp:BoundField DataField="x22" HeaderText="22 Side Rails &lt;br/&gt;Pallet K" HtmlEncode="False" />
                </Columns>
                <HeaderStyle CssClass="raDataGridHeader" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" 
                
                SelectCommand="SELECT [EMB_REC_DTE], [x1], [x2], [x92], [x500], [x750], [x780], [x840],[x91],x71,x72,x21,x22  FROM [EMB_REC_LOG]">
            </asp:SqlDataSource>
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
