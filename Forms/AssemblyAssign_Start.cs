using System;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class AssemblyAssign_Start : System.Web.UI.Page
{


    protected void InsertButton_Click(object sender, EventArgs e)
    {
        if (new DAL().LotNumberExists(this.txtName.Text.Trim()))
        {
            base.Response.Redirect("AssemblyAssign.aspx?mo=" + this.txtName.Text.Trim());
        }
        else
        {
            this.ErrorHandler1.TypeOfMessage = MessageTypes.MessageType.Error;
            this.ErrorHandler1.ErrorMessage = "M/O number does not exist in Fourth Shift";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }


}

