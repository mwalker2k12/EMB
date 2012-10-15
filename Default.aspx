<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="CustomControls/MessageHandler.ascx" TagName="MessageHandler" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ihc.css" rel="stylesheet" type="text/css" />
    <title>Navigator</title>
</head>
<body>
    <form id="form1" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <uc1:MessageHandler ID="MessageHandler1"  runat="server" />
        &nbsp;<br />
 
    </form>
</body>
</html>
