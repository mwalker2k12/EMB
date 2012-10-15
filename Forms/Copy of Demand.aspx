<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Copy of Demand.aspx.cs" Inherits="Demand" %>

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
    <style type="text/css">
        .style1
        {
            height: 22px;
        }
        .style3
        {
            height: 19px;
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
<td style="border:solid 1px black" style="width:10%"></td>
<td style="width:80%; text-align:center">Emballage Shipped Report</td>
<td style="width:10%"></td>

</tr>
</table>
<div id="selectionPanel">

        <div id="selectonTable" runat="server" class="raPanel">
            <uc2:Demand ID="Demand1" PkgNo="500" runat="server" />
            <br />
            <table>
            <tr>
            <td style="border:solid 1px black" class="style1">Emballage Part # </td><td style="border:solid 1px black" class="style1">500</td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >Wk#1</td>
                <td style="border:solid 1px black" >Wk#2</td>
                <td style="border:solid 1px black" >Wk#3</td>
                <td style="border:solid 1px black" >Wk#4</td>
                <td style="border:solid 1px black" >Wk#5</td>
                <td style="border:solid 1px black" >Wk#6</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Firm Rqmts</td>
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
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Forecast Rqmts</td>
                <td style="border:solid 1px black" >&nbsp;&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Total Demand</td>
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
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >On Hand</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            </table>
            <br />
            <br />
             <table>
            <tr>
            <td style="border:solid 1px black" class="style1">Emballage Part # </td><td style="border:solid 1px black" class="style1">750&nbsp;</td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >Wk#1</td>
                <td style="border:solid 1px black" >Wk#2</td>
                <td style="border:solid 1px black" >Wk#3</td>
                <td style="border:solid 1px black" >Wk#4</td>
                <td style="border:solid 1px black" >Wk#5</td>
                <td style="border:solid 1px black" >Wk#6</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Firm Rqmts</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label7" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label8" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label9" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label10" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label11" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label12" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Forecast Rqmts</td>
                <td style="border:solid 1px black" >&nbsp;&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Total Demand</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label31" runat="server" Text="0"></asp:Label>
                </td>
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
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >On Hand</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            </table>
            <br />
            
            <br />
            <table>
            <tr>
            <td style="border:solid 1px black" class="style1">Emballage Part # </td><td style="border:solid 1px black" class="style1">780&nbsp;</td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >Wk#1</td>
                <td style="border:solid 1px black" >Wk#2</td>
                <td style="border:solid 1px black" >Wk#3</td>
                <td style="border:solid 1px black" >Wk#4</td>
                <td style="border:solid 1px black" >Wk#5</td>
                <td style="border:solid 1px black" >Wk#6</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Firm Rqmts</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label13" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label14" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label15" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label16" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label17" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label18" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="border:solid 1px black" class="style3" ></td>
                <td style="border:solid 1px black" class="style3" >Forecast Rqmts</td>
                <td style="border:solid 1px black" class="style3" >&nbsp;&nbsp;</td>
                <td style="border:solid 1px black" class="style3" ></td>
                <td style="border:solid 1px black" class="style3" ></td>
                <td style="border:solid 1px black" class="style3" ></td>
                <td style="border:solid 1px black" class="style3" ></td>
                <td style="border:solid 1px black" class="style3" ></td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Total Demand</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label37" runat="server" Text="0"></asp:Label>
                </td>
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
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >On Hand</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            </table>
            <BR />
            <br />
            <table>
            <tr>
            <td style="border:solid 1px black" class="style1">Emballage Part # </td><td style="border:solid 1px black" class="style1">
                840</td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
                <td style="border:solid 1px black" class="style1"></td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >Wk#1</td>
                <td style="border:solid 1px black" >Wk#2</td>
                <td style="border:solid 1px black" >Wk#3</td>
                <td style="border:solid 1px black" >Wk#4</td>
                <td style="border:solid 1px black" >Wk#5</td>
                <td style="border:solid 1px black" >Wk#6</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Firm Rqmts</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label19" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label20" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label21" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label22" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label23" runat="server" Text="0"></asp:Label>
                </td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label24" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Forecast Rqmts</td>
                <td style="border:solid 1px black" >&nbsp;&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            <tr>
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >Total Demand</td>
                <td style="border:solid 1px black" >
                    <asp:Label ID="Label43" runat="server" Text="0"></asp:Label>
                </td>
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
            <td style="border:solid 1px black" >&nbsp;</td><td style="border:solid 1px black" >On Hand</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
                <td style="border:solid 1px black" >&nbsp;</td>
            </tr>
            </table>
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
