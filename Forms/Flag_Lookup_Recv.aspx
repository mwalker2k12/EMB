<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Flag_Lookup_Recv.aspx.cs"  Inherits="Flag_Lookup_Recv" %>

<%@ Register Src="../CustomControls/ErrorHandler.ascx" TagName="ErrorHandler" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Navigator</title>
    <link href="../ihc.css" rel="stylesheet" type="text/css" />
     <script language="javascript" type="text/javascript">
       function validateDdl(sender, args)
         {
            if(args.Value.toString() == "select")
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
            
         }


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
    &nbsp; &nbsp;
<div id="selectionPanel">
<table class="raPanelHeader" style="width:100%">
<tr>
<td style="width:10%"></td>
<td style="width:80%; text-align:center"><b>Inspection Flag Lookup</b></td>
<td style="width:10%"></td>

</tr>
</table>

        <div id="selectonTable" class="raPanel">
            &nbsp;&nbsp;<table class="raSelectPanelContent">
                <tr>
                    <td style="width:91px; height: 21px;">
                        Part Number:</td>
                    <td style="height: 21px" colspan="2">
                        <asp:TextBox ID="txtPartNbr" runat="server" MaxLength="35" Width="150px" />&nbsp;
                        <cc1:AutoCompleteExtender MinimumPrefixLength="2" CompletionInterval="500" ServiceMethod="GetPartNumbers" TargetControlID="txtPartNbr" ID="AutoCompleteExtender1" runat="server">
                        </cc1:AutoCompleteExtender>
                    </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 91px; height: 21px">
                        Status:</td>
                    <td colspan="2" style="height: 21px">
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="raDropDownList">
                            <asp:ListItem>(select)</asp:ListItem>
                            <asp:ListItem Value="0">Active</asp:ListItem>
                            <asp:ListItem Value="1">Closed</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 100px; color: #000000; height: 21px">
                    </td>
                </tr>
            </table><br />
            &nbsp;<asp:Button CssClass="raButton" ID="LinkButton1" Text="Search" runat="server" OnClick="LinkButton1_Click"></asp:Button><br />
            &nbsp;<br />
        </div>
    <br />
    <br />
    <table class="raPanelHeader" style="width:100%">
        <tr>
            <td style="width:10%">
            </td>
            <td style="width:80%; text-align:center">
                <b>Results</b></td>
            <td style="width:10%">
            </td>
        </tr>
    </table>
    <div id="Div1" runat="server" class="raPanel">
        <br />
           <asp:GridView CssClass="raDataGrid"  ID="GridView1" runat="server" 
            AllowSorting="True" OnRowCreated="GridView1_RowCreated" 
            AutoGenerateColumns="False" PageSize="15" EnableModelValidation="True"   >
            <HeaderStyle ForeColor="Black" CssClass="raDataGridHeader" />
            <AlternatingRowStyle CssClass="altRow" />
            <SelectedRowStyle CssClass="selectedRow" />
            <EmptyDataTemplate>
                <div style="width:100%; height:25px; text-align:center">
                    <h3>No Results Found.</h3>
                </div>
            </EmptyDataTemplate>
                   <Columns>
                   <asp:TemplateField>
                     <ItemTemplate>
                    
                    </ItemTemplate>
                   </asp:TemplateField>
                   <asp:BoundField DataField="PART_NO" HeaderText="PN" >
                       <ItemStyle Width="75px" />
                   </asp:BoundField>
                       <asp:BoundField DataField="STATUS" HeaderText="Status" />
               </Columns>
        </asp:GridView>
        <br />
        &nbsp;</div>
    <uc1:ErrorHandler ID="ErrorHandler1" runat="server" />
    &nbsp;
        </div>
        </div>
    </form>
</body>
</html>
