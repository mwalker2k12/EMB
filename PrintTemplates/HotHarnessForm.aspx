<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HotHarnessForm.aspx.cs" Inherits="PrintTemplates_HotHarnessForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Assembly Form</title>
     <script language="javascript" type="text/javascript">
       function redireccionar() {
    setTimeout("location.href='../Default.aspx'", 3000);
  }
    </script>
</head>
<body onload="window.print(); redireccionar();">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="text-align: center" width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblPartNo" runat="server" Font-Bold="True" Font-Size="70pt" Text="HOT.....HOT....." ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblQty" runat="server" Font-Bold="True" Font-Size="70pt" Text="......HOT........." ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblOk" runat="server" Font-Bold="True" Font-Size="70pt" ForeColor="Red"
                        Text=".............HOT......."></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDate" runat="server" Font-Bold="True" Font-Size="70pt" Text="GIVE TO:" ForeColor="Red"></asp:Label>&nbsp;
                    <asp:Label ID="lblNme" runat="server" Font-Bold="True" Font-Size="70pt" ForeColor="Blue"
                        Text="name"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="70pt" Text="THANKS." ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
