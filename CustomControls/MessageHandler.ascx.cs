using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class CustomControls_MessageHandler : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private string _infoMessage;

    public string InfoMessage
    {
        get { return _infoMessage; }
        set { this.errorLabel.Text = value;  _infoMessage = value; }
    }
}
