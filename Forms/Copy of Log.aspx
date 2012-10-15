<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Copy of Log.aspx.cs" Inherits="Log" %>

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
                DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="MO_NUMBER" HeaderText="MO_NUMBER" ReadOnly="True" 
                        SortExpression="MO_NUMBER" />
                    <asp:BoundField DataField="ORDER_QTY" HeaderText="ORDER_QTY" 
                        SortExpression="ORDER_QTY" />
                    <asp:BoundField DataField="ITEM" HeaderText="ITEM" ReadOnly="True" 
                        SortExpression="ITEM" />
                    <asp:BoundField DataField="INSP_QTY" HeaderText="INSP_QTY" 
                        SortExpression="INSP_QTY" />
                    <asp:BoundField DataField="CUST_ID" HeaderText="CUST_ID" ReadOnly="True" 
                        SortExpression="CUST_ID" />
                        <asp:BoundField DataField="REQD_DATE" HeaderText="REQD_DATE" ReadOnly="True" 
                        SortExpression="REQD_DATE" />
             
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:NewFSConnectionString %>" 
                SelectCommand="SELECT DISTINCT
MO_NUMBER,ORDER_MANUFACTDETAIL.ORDER_QTY,ITEM_ITEMLIST.ITEM,ITEM_ITEMLIST.INSP_QTY
,CUST_ID,REQD_DATE,'C/O' AS 'DEMANDTYPE'
FROM ORDER_MANUFACTDETAIL,ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL
WHERE 
ORDER_MANUFACTDETAIL.ITEM = ITEM_ITEMLIST.ITEM
AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
AND ORDER_MANUFACTDETAIL.LN_STA = '4'  
AND ITEM_ITEMLIST.INSP_QTY &gt; 0
ORDER  
BY ITEM_ITEMLIST.ITEM,MO_NUMBER">
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
