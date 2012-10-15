<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Shipping.aspx.cs" Inherits="Shipping" %>

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
            width: 100%;
        }
        .auto-style1
        {
            height: 17px;
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
<td style="width:80%; text-align:center">Emballage Shipped Report</td>
<td style="width:10%"></td>

</tr>
</table>
<div id="selectionPanel">

        <div id="selectonTable" runat="server" class="raPanel">
            <strong><span style="color: #993333"></span></strong><br />
            
            <br />
            <br />
            
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSource2" DataKeyNames="EMB_SHIP_ID" 
                ondatabound="GridView2_DataBound" onrowcreated="GridView2_RowCreated" 
                onrowdatabound="GridView2_RowDataBound" EnableModelValidation="True" 
                >
                <AlternatingRowStyle CssClass="altRow" />
                <Columns>
                    <asp:CommandField ShowEditButton="True" />
                    <asp:BoundField DataField="EMB_SHIP_ID" HeaderText="EMB_SHIP_ID" 
                        InsertVisible="False" ReadOnly="True" SortExpression="EMB_SHIP_ID" 
                        Visible="False" />
                    <asp:TemplateField HeaderText="Item" SortExpression="EMB_ITEM">
                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("EMB_ITEM") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("EMB_ITEM") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ship Qty" SortExpression="EMB_SHIP_QTY">
                        <EditItemTemplate>
                             <asp:Label ID="Label2" runat="server" Text='<%# Bind("EMB_SHIP_QTY") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("EMB_SHIP_QTY") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pcs Per" SortExpression="EMB_PCS_PER">
                        <EditItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("EMB_PCS_PER") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("EMB_PCS_PER") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pkg Type" SortExpression="EMB_PKG_TYPE">
                        <EditItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("EMB_PKG_TYPE") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("EMB_PKG_TYPE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EMB_BIN_QTY" HeaderText="# Bins" 
                        SortExpression="EMB_BIN_QTY" />
                    <asp:BoundField DataField="ROWCOLOR" HeaderText="ROWCOLOR" 
                        SortExpression="ROWCOLOR" Visible="False" />
                </Columns>
                <HeaderStyle CssClass="raDataGridHeader" />
            </asp:GridView>
            
            <hr />
            
            <table class="style1">
                <tr>
                    <td>
            <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="True" 
                CssClass="raDropDownList" DataSourceID="SqlDataSource1" 
                DataTextField="FS_SHIP_ID_NO" DataValueField="FS_SHIP_ID_NO">
                <asp:ListItem Value="(select ship to id)"></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" SelectCommand="SELECT 
FS_SHIP_ID_NO,
FS_SHIP_ID_NO + ' - ' + ADDRESS
FROM FORECAST_SHIP_ID_CONVERSION"></asp:SqlDataSource>
            <br />
            <table>
            <tr>
            <td>1 Pallet L (4'x2.75'):</td><td><asp:TextBox ID="TextBox1" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>91 Pallet Lid (Pallet L)</td><td>
                <asp:TextBox ID="TextBox4" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>2 Pallet K (2.75&#39; x 2&#39;)</td><td><asp:TextBox ID="TextBox2" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>92 Pallet Lid (Pallet K)</td><td><asp:TextBox ID="TextBox3" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td class="auto-style1">71 Wooden Separator (Pallet L)</td><td class="auto-style1"><asp:TextBox ID="TextBox13" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td valign="middle">72 Wooden Separator (Pallet K)
                </td><td><asp:TextBox ID="TextBox14" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            </table>
                    </td>
                    <td>
            <asp:DropDownList ID="DropDownList2" runat="server" AppendDataBoundItems="True" 
                CssClass="raDropDownList" DataSourceID="SqlDataSource3" 
                DataTextField="FS_SHIP_ID_NO" DataValueField="FS_SHIP_ID_NO">
                <asp:ListItem Value="(select ship to id)"></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" SelectCommand="SELECT 
FS_SHIP_ID_NO,
FS_SHIP_ID_NO + ' - ' + ADDRESS
FROM FORECAST_SHIP_ID_CONVERSION"></asp:SqlDataSource>
            <br />
            <table>
            <tr>
            <td>1 Pallet L (4'x2.75'):</td><td>
                <asp:TextBox ID="TextBox5" runat="server" 
                    CssClass="raTextBox" Width="50px" Height="16px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>91 Pallet Lid (Pallet L)</td><td>
                <asp:TextBox ID="TextBox8" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>2 Pallet K (2.75&#39; x 2&#39;)</td><td>
                <asp:TextBox ID="TextBox6" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>92 Pallet Lid (Pallet K)</td><td>
                <asp:TextBox ID="TextBox7" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>71 Wooden Separator (Pallet L)</td><td>
                <asp:TextBox ID="TextBox15" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>72 Wooden Separator (Pallet K)
                </td><td>
                <asp:TextBox ID="TextBox16" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            </table>
                    </td>
                    <td>
            <asp:DropDownList ID="DropDownList3" runat="server" AppendDataBoundItems="True" 
                CssClass="raDropDownList" DataSourceID="SqlDataSource4" 
                DataTextField="FS_SHIP_ID_NO" DataValueField="FS_SHIP_ID_NO">
                <asp:ListItem Value="(select ship to id)"></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" SelectCommand="SELECT 
FS_SHIP_ID_NO,
FS_SHIP_ID_NO + ' - ' + ADDRESS
FROM FORECAST_SHIP_ID_CONVERSION"></asp:SqlDataSource>
            <br />
            <table>
            <tr>
            <td>1 Pallet L (4'x2.75'):</td><td>
                <asp:TextBox ID="TextBox9" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>91 Pallet Lid (Pallet L)</td><td>
                <asp:TextBox ID="TextBox12" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>2 Pallet K (2.75&#39; x 2&#39;)</td><td>
                <asp:TextBox ID="TextBox10" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>92 Pallet Lid (Pallet K)</td><td>
                <asp:TextBox ID="TextBox11" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>71 Wooden Separator (Pallet L)</td><td>
                <asp:TextBox ID="TextBox17" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td>72 Wooden Separator (Pallet K)
                </td><td>
                <asp:TextBox ID="TextBox18" runat="server" 
                    CssClass="raTextBox" Width="50px"></asp:TextBox>
                </td>
            </tr>
            </table>
                    </td>
                </tr>
            </table>
            
            <br />
            <br />
            <br />
            <br />
            
            <br />
            <asp:Button  CssClass="raButtonWide" ID="UpdateButton" runat="server" CausesValidation="True"
                 Text="View Report" onclick="UpdateButton_Click"></asp:Button>&nbsp; &nbsp;&nbsp;&nbsp;<br />
      
        </div>
    <uc1:ErrorHandler ID="ErrorHandler1" runat="server" />
    <br />
    <br />
    </div>
        </div>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>" SelectCommand="
SELECT
EMB_SHIP_ID
,EMB_ITEM 
,EMB_SHIP_QTY 
,EMB_PCS_PER 
,EMB_PKG_TYPE 
,EMB_BIN_QTY ,CAST(EMB_DATE AS VARCHAR(11))
,CASE
WHEN 
CAST(EMB_SHIP_QTY AS INT) % CAST(EMB_PCS_PER AS INT) &lt;&gt; 0 
--AND CAST(EMB_SHIP_QTY AS INT) % CAST(EMB_PCS_PER AS INT) &gt; 0
--AND EMB_PCS_PER &lt; EMB_SHIP_QTY
THEN 'red'
END AS ROWCOLOR
FROM EMB_SHIPMENT2
WHERE 
CAST(EMB_DATE AS VARCHAR(11)) = CAST(GETDATE() AS VARCHAR(11))
-- EMB_DATE BETWEEN '4/28/2011 12:00 AM' AND '4/28/2011 9:00 PM'" UpdateCommand="UPDATE
EMB_SHIPMENTS
SET EMB_BIN_QTY = @EMB_BIN_QTY
WHERE EMB_SHIP_ID = @EMB_SHIP_ID"></asp:SqlDataSource>
    </form>
</body>
</html>
