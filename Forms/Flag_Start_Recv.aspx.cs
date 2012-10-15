using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;


public partial class Flag_Start_Recv : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["flag"] != null)
        {
            this.TextBox1.Text = Request.QueryString["flag"].ToString();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        string flagNo = this.TextBox1.Text.Trim();
        flagNo = flagNo.Replace("IF", "");
        Response.Redirect("Flag_Recv.aspx?flag=" + flagNo + "&type=edit");
    }
}