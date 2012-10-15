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

public partial class Shipping : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        new DAL().UpdateEmbShipments();
        new DAL().UpdateOhQty();
        this.Button1.CssClass = "raButtonWide";
        this.Button1.Text = "Shipments Saved.";
        this.Button1.Enabled = false;
    }
}