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


public partial class CustomControls_ErrorHandler : System.Web.UI.UserControl
{
    private string errorMessage = string.Empty;

    private MessageTypes.MessageType messageType;

    public MessageTypes.MessageType TypeOfMessage
    {
        get
        {
            return messageType;
        }
        set
        {
            messageType = value;
            if (value == MessageTypes.MessageType.Error)
            {
                this.errorLabel.ForeColor = System.Drawing.Color.Red;
            }
            if (value == MessageTypes.MessageType.Information)
            {
                this.errorLabel.ForeColor = System.Drawing.Color.DarkGreen;
            }

        }
    }
    public string ErrorMessage
    {
        get
        {
            return errorMessage;
        }
        set
        {
            errorMessage = value;
            this.errorLabel.Text = value;
            //if (value == "")
            //{
            //    this.errors.Visible = false;
            //}
            //else
            //{
            //    this.errors.Visible = true;
            //}
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
