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
using System.Data.SqlClient;

public partial class Flag_Lookup : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {


    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetPartNumbers(string prefixText, int count)
    {
        DAL da = new DAL();
        return da.GetFsPartNumbers(prefixText);
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl1 = new HyperLink();
                hl1.ID = "hl1";
                hl1.Target = "application";
                hl1.Attributes.Add("onclick", "window.close();");
                hl1.Text = DataBinder.Eval(e.Row.DataItem, "FLAG_NO2").ToString();
                hl1.NavigateUrl = Request.QueryString["PREVPAGE"].ToString() + "?flag=" + DataBinder.Eval(e.Row.DataItem, "FLAG_NO2").ToString();
                e.Row.Cells[0].Controls.Add(hl1);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        this.GridView1.DataSource = new DAL().FlagLookup(this.txtPartNbr.Text.Trim(),this.DropDownList1.SelectedValue );
        this.GridView1.DataBind();
    }

}
