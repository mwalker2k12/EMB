<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Demand.ascx.cs" Inherits="CustomControls_Demand" %>
<style type="text/css">

        td 
{
    font-size:x-small !important;
	color: #000;
}

td {
	font-size: 9pt;
	font-family: Arial, Verdana, sans-serif;
	font-weight: normal;
	}
	
    .style2
    {
        font-weight: bold;
    }
	
</style>
            <table>
            <tr>
            <td class="style2" >Emballage Part # </td><tdclass="style1">
                <td><asp:Label Font-Bold="true" ID="Label31" runat="server" Text="pkgno"></asp:Label>
                </td>
                <td ></td>
                <td ></td>
                <td ></td>
                <td ></td>
                <td ></td>
                <td ></td>
            </tr>
            <tr>
            <td  >&nbsp;</td><td  >&nbsp;</td>
                <td style="border:solid 1px black" class="style2" >Wk#1</td>
                <td style="border:solid 1px black" class="style2" >Wk#2</td>
                <td style="border:solid 1px black" class="style2" >Wk#3</td>
                <td style="border:solid 1px black" class="style2" >Wk#4</td>
                <td style="border:solid 1px black" class="style2" >Wk#5</td>
                <td style="border:solid 1px black" class="style2" >Wk#6</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" 
                    class="style2" >Firm Rqmts</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label1" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label2" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label3" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label4" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label5" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label6" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" 
                    class="style2" >Forecast Rqmts</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label50" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label51" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label52" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label53" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label54" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label55" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" 
                    class="style2" >Total Demand</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label25" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label26" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label27" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label28" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label29" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label30" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" 
                    class="style2" >On Hand</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label32" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label33" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label34" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label35" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label36" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label37" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" 
                    class="style2" >
                Shortage</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label38" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label39" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label40" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label41" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label42" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label43" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="border:solid 1px black" ></td><td style="border:solid 1px black" ></td>
                <td style="border:solid 1px black" ></td>
                <td style="border:solid 1px black" ></td>
                <td style="border:solid 1px black" ></td>
                <td style="border:solid 1px black" ></td>
                <td style="border:solid 1px black" ></td>
                <td style="border:solid 1px black" ></td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" 
                    class="style2" >Order 
                Qty</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label44" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label45" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label46" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label47" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label48" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label49" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            </table>
            