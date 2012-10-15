<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Copy of Inspec_Priority.aspx.cs" Inherits="Inspec_Priority" %>

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
<td style="width:80%; text-align:center"><b>Inspection Priority Report</b></td>
<td style="width:10%"></td>

</tr>
</table>
<div id="selectionPanel">

        <div id="selectonTable" runat="server" class="raPanel">
            <br /><div style="width: 100%; height: 400px; overflow: scroll">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                EnableModelValidation="True" onrowdatabound="GridView1_RowDataBound" HeaderStyle-CssClass="container" 
                onrowcreated="GridView1_RowCreated" onselectedindexchanged="GridView1_SelectedIndexChanged" 
                >
                <AlternatingRowStyle CssClass="altRow" />
                <Columns>
                    <asp:TemplateField HeaderText="Inspect&lt;br/&gt;In&lt;br/&gt;Process">
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="raDropDownList" 
                            DataSourceID="SqlDataSource1" DataTextField="EMP_NAME" 
                            DataValueField="EMP_NAME" AppendDataBoundItems="True" AutoPostBack="True" 
                            onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                            <asp:ListItem Value="select">(select inspector)</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" 
                            SelectCommand="SELECT
EMP_NAME
FROM ET_EMPLOYEES
WHERE EMP_CURR_WC_ID = 71"></asp:SqlDataSource>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PTS&lt;br/&gt;Date"></asp:TemplateField>
                    <asp:TemplateField HeaderText="PTS&lt;br/&gt;Qty"></asp:TemplateField>
                    <asp:TemplateField HeaderText="PTS&lt;br/&gt;Date 2">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PTS&lt;br/&gt;Qty 2">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PTS&lt;br/&gt;Date 3">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PTS&lt;br/&gt;Qty 3">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ITEM" HeaderText="Part No" SortExpression="ITEM" />
                    <asp:BoundField DataField="INSP_QTY" 
                        HeaderText="Avail To&lt;br/&gt;Inspect&lt;br/&gt;Qty" HtmlEncode="False" 
                        SortExpression="INSP_QTY" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CUST_ID" HeaderText="C/O&lt;br/&gt;Source" 
                        HtmlEncode="False" />
                    <asp:BoundField DataField="CO_NUMBER" HeaderText="C/O Number" />
                    <asp:BoundField DataField="REQD_DATE" DataFormatString="{0:d}" 
                        HeaderText="Next Due&lt;br/&gt;Date" HtmlEncode="False" 
                        SortExpression="REQD_DATE" />
                    <asp:BoundField DataField="DEMANDTYPE" HeaderText="Demand&lt;br/&gt;Type" 
                        HtmlEncode="False" SortExpression="DEMANDTYPE" />
                    <asp:BoundField DataField="ORDER_QTY" HeaderText="Qty Due" 
                        SortExpression="ORDER_QTY" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ITEM_REF1" HeaderText="VM&lt;br/&gt;DF" 
                        HtmlEncode="False" SortExpression="ITEM_REF1" />
                    <asp:BoundField DataField="PCS_PER" HeaderText="Pcs Per&lt;br/&gt;Pkg" 
                        HtmlEncode="False" SortExpression="PCS_PER" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Insp/Pkg&lt;br/&gt;Qty" HtmlEncode="False" 
                        DataField="ALLOC" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PKG_TYPE" HeaderText="Pkg&lt;br/&gt;Type" 
                        HtmlEncode="False" SortExpression="PKG_TYPE" />
                    <asp:BoundField />
                    <asp:BoundField DataField="STK_ROOM" />
                    <asp:BoundField DataField="ITEM_CLAS5" HeaderText="Insp.&lt;br/&gt;Audit" 
                        HtmlEncode="False" />
                </Columns>
                <HeaderStyle CssClass="raDataGridHeader" />
            </asp:GridView></div>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:NewFSConnectionString %>" 
                ProviderName="<%$ ConnectionStrings:NewFSConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT
ITEM_ITEMLIST.ITEM,ITEM_ITEMLIST.INSP_QTY
,ORDER_CUSTOMERDETAIL.CUST_ID,REQD_DATE,'C/O' AS 'DEMANDTYPE',ORDER_CUSTOMERDETAIL.ORDER_QTY,ITEM_ITEMLIST.ITEM_REF1
,CASE
WHEN CUST_CLAS2 &lt;&gt; '' THEN PKG_TYPE
ELSE 'CTN'
END AS PKG_TYPE
,CASE
WHEN CUST_CLAS2 &lt;&gt; '' THEN PCS_PER
ELSE NULL
END AS PCS_PER

FROM ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL,ITEM_INFO,CUSTOMER_DATA
WHERE  ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
AND ORDER_CUSTOMERDETAIL.CUST_ID = CUSTOMER_DATA.CUST_ID
AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
AND ITEM_ITEMLIST.INSP_QTY &gt; 0
AND ORDER_CUSTOMERDETAIL.LN_STA = '4'
--irs forecast
UNION

SELECT ITEM_NO AS 'ITEM',ITEM_ITEMLIST.INSP_QTY,'IRS' AS 'CUST_ID',DTE AS 'REQD_DATE','FCST' AS 'DEMANDTYPE',QTY AS 'ORDER_QTY'
,'' AS 'ITEM_REF1'
,PKG_TYPE
,CASE
WHEN CUST_CLAS2 &lt;&gt; '' THEN PCS_PER
ELSE NULL
END AS PCS_PER
FROM IHC_IRS_FORECAST,ITEM_INFO,CUSTOMER_DATA,ITEM_ITEMLIST
WHERE ITEM_NO = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ITEM_NO
AND CUSTOMER_DATA.CUST_ID = 'IRS'
AND ITEM_NO IN 
(SELECT DISTINCT
ITEM_ITEMLIST.ITEM
FROM ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL,ITEM_INFO,CUSTOMER_DATA
WHERE  ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
AND ITEM_ITEMLIST.INSP_QTY &gt; 0
AND ORDER_CUSTOMERDETAIL.LN_STA = '4')

--grove forecast
UNION

SELECT ITEM_NO AS 'ITEM',ITEM_ITEMLIST.INSP_QTY,'GRO' AS 'CUST_ID',DTE AS 'REQD_DATE','FCST' AS 'DEMANDTYPE',QTY AS 'ORDER_QTY'
,'' AS 'ITEM_REF1'
,PKG_TYPE
,CASE
WHEN CUST_CLAS2 &lt;&gt; '' THEN PCS_PER
ELSE NULL
END AS PCS_PER
FROM IHC_GRO_FORECAST,ITEM_INFO,CUSTOMER_DATA,ITEM_ITEMLIST

WHERE ITEM_NO = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ITEM_NO
AND CUSTOMER_DATA.CUST_ID = 'GRO'
AND ITEM_NO IN 
(SELECT DISTINCT
ITEM_ITEMLIST.ITEM
FROM ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL,ITEM_INFO
WHERE  ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
AND ITEM_ITEMLIST.INSP_QTY &gt; 0
AND ORDER_CUSTOMERDETAIL.LN_STA = '4')

--GENERIC FORECAST
UNION

SELECT ITEM_NO AS 'ITEM',ITEM_ITEMLIST.INSP_QTY,'GEN' AS 'CUST_ID',DTE AS 'REQD_DATE','FCST' AS 'DEMANDTYPE',QTY AS 'ORDER_QTY'
,'' AS 'ITEM_REF1'
,PKG_TYPE,
CASE
WHEN CUST_CLAS2 &lt;&gt; '' THEN PCS_PER
ELSE NULL
END AS PCS_PER

FROM IHC_GEN_FORECAST,ITEM_INFO,CUSTOMER_DATA,ITEM_ITEMLIST

WHERE ITEM_NO = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ITEM_NO
AND ITEM_NO IN 
(SELECT DISTINCT
ITEM_ITEMLIST.ITEM
FROM ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL,ITEM_INFO
WHERE  ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
AND ITEM_ITEMLIST.INSP_QTY &gt; 0
AND ORDER_CUSTOMERDETAIL.LN_STA = '4')

ORDER BY ITEM_ITEMLIST.ITEM,DEMANDTYPE,REQD_DATE




"></asp:SqlDataSource>
            <br />
            <hr />
            <asp:Button  CssClass="raButton" ID="UpdateButton" runat="server" CausesValidation="True"
                Text="Save" onclick="UpdateButton_Click" Visible="False"></asp:Button>&nbsp; &nbsp;&nbsp;&nbsp;<br />
      
        </div>
    <uc1:ErrorHandler ID="ErrorHandler1" runat="server" />
    <br />
    <br />
    </div>
        </div>
    </form>
</body>
</html>
