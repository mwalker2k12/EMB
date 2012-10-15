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

public partial class CustomControls_WiControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.sourceWi.SelectParameters["PAGE_ID"].DefaultValue = this._pageId;
    }
    private string _pageId;

    public string PageId
    {
        get { return _pageId; }
        set { _pageId = value; }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        int wiId = Int32.Parse(((LinkButton)sender).CommandArgument.ToString());
        IHC.WorkInstruction wi = IHC.WorkInstruction.FetchByID(wiId);
        Response.AddHeader("Content-Type", "application/msword; filename=" + wi.WiTitle + ".doc");
        Response.BinaryWrite(wi.WiBytes);
        Response.End();
    }
}
