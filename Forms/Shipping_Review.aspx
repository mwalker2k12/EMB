<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Shipping_Review.aspx.cs" Inherits="Shipping" %>

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
    
<script language="javascript" type="text/javascript">
// <![CDATA[

function btnPrint0_onclick() {

}

// ]]>
</script>
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
<td style="width:80%; text-align:center"><b>Emballage Shipped Report</b></td>
<td style="width:10%"></td>

</tr>
</table>
<div id="selectionPanel">

        <div id="selectonTable" runat="server" class="raPanel">
            <strong><span style="color: #993333"></span></strong><br />
            
            <b>Container Shipments</b><br />
            <br />
            
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSource1" 
                >
                <AlternatingRowStyle CssClass="altRow" />
                <Columns>
                    <asp:BoundField DataField="EMB_DATE" HeaderText="Ship Date" 
                        SortExpression="EMB_DATE" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EMB_CUST_NME" HeaderText="Ship To" 
                        SortExpression="EMB_CUST_NME" />
                    <asp:BoundField DataField="EMB_SHIP_TO_ID" HeaderText="Ship To ID" 
                        SortExpression="EMB_SHIP_TO_ID" />
                    <asp:BoundField DataField="500" HeaderText="500" 
                        SortExpression="500" ReadOnly="True" />
                    <asp:BoundField DataField="750" HeaderText="750" 
                        SortExpression="750" ReadOnly="True" />
                    <asp:BoundField DataField="780" HeaderText="780" 
                        SortExpression="780" ReadOnly="True" />
                    <asp:BoundField DataField="840" HeaderText="840" ReadOnly="True" 
                        SortExpression="840" />
                    <asp:BoundField DataField="751" HeaderText="751" SortExpression="751" />
                    <asp:BoundField DataField="781" HeaderText="781" SortExpression="781" />
                    <asp:BoundField DataField="841" HeaderText="841" SortExpression="841" />
                </Columns>
                <HeaderStyle CssClass="raDataGridHeader" />
            </asp:GridView>
            
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" SelectCommand="

SELECT
EMB_DATE
,EMB_CUST_NME
,EMB_SHIP_TO_ID
,ISNULL(SUM([500]),0) AS [500]
,ISNULL(SUM([750]),0) AS [750]
,ISNULL(SUM([780]),0) AS [780]
,ISNULL(SUM([840]),0) AS [840]
,ISNULL(SUM([750]),0) AS [751]
,ISNULL(SUM([780]),0) AS [781]
,ISNULL(SUM([840]),0) AS [841]
FROM
(
SELECT
*
FROM 
EMB_SHIPMENT2


PIVOT
(
	SUM(EMB_BIN_QTY)
	FOR [EMB_PKG_TYPE] IN ([500],[750],[780],[840],[751],[781],[841])
) AS P
) AS A
WHERE
CAST(EMB_DATE AS VARCHAR(11)) = CAST(GETDATE() AS VARCHAR(11))
 --EMB_DATE = '4/28/2011'
AND EMB_CUST_NME IS NOT NULL
GROUP BY EMB_DATE
,EMB_CUST_NME
,EMB_SHIP_TO_ID
" onselecting="SqlDataSource1_Selecting"></asp:SqlDataSource>
            
            <br />
            <b>Pallet / Lid Shipments</b><br />
            <br />
            
            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSource2" DataKeyNames="EMB_PKG_NO" 
                >
                <AlternatingRowStyle CssClass="altRow" />
                <Columns>
                    <asp:BoundField DataField="EMB_DATE" HeaderText="Ship Date" 
                        SortExpression="EMB_DATE" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EMB_PKG_NO" HeaderText="Pkg Type" 
                        SortExpression="EMB_PKG_NO" ReadOnly="True" />
                    <asp:BoundField DataField="EMB_PKG_DESC" HeaderText="Desc" 
                        SortExpression="EMB_PKG_DESC" />
                    <asp:BoundField DataField="EMB_BIN_QTY" HeaderText="Qty " 
                        SortExpression="EMB_BIN_QTY" />
                    <asp:BoundField DataField="EMB_SHIP_TO_ID" HeaderText="Ship To ID" 
                        SortExpression="EMB_SHIP_TO_ID" />
                </Columns>
                <HeaderStyle CssClass="raDataGridHeader" />
            </asp:GridView>
            
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" SelectCommand="SELECT
EMB_DATE
,EMB_PKG_NO
,EMB_PKG_DESC
,EMB_BIN_QTY
,EMB_SHIP_TO_ID
FROM EMB_SHIPMENT2,EMB_PACKAGE_TYPES
WHERE EMB_PKG_TYPE = EMB_PKG_NO 
AND  CAST(EMB_DATE AS VARCHAR(11)) = CAST(GETDATE() AS VARCHAR(11))
--AND EMB_DATE BETWEEN '4/28/2011 2:00 AM' AND '4/28/2011 9:00 PM'
AND EMB_PKG_TYPE IN ('1','2','92','91')
AND EMB_BIN_QTY &gt; 0"></asp:SqlDataSource>
            <br />
            <br />
            <br />
            
            <br />
            <hr />
            <table>
            <tr>
            <td><input class="raButton" type="button" id="btnPrint" onclick="window.print
()" value="Print">&nbsp;</td>
            <td><asp:Button ID="Button1" CssClass="raButton" runat="server" Text="Save" 
                onclick="Button1_Click" /></td>
                </tr>
                </table>
      
        </div>
    <uc1:ErrorHandler ID="ErrorHandler1" runat="server" />
    <br />
    <br />
    </div>
        </div>
    </form>
</body>
</html>
