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


public partial class Flag : System.Web.UI.Page
{
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetPartNumbers(string prefixText, int count)
    {
        DAL da = new DAL();
        return da.GetFsPartNumbers(prefixText);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (Request.QueryString["flag"] != null)
            {
                this.txtPartNbr.Enabled = false;
                this.txtEntBy.Enabled = false;
                this.txtEntDATE.Enabled = false;
                this.ImageButton5.Enabled = false;
                this.Button1.Text = this.Button1.Text.Replace("Save New", "Update");
            }
            else
            {
                this.DropDownList1.SelectedIndex = 1;
                this.td1.Visible = false;
                this.RequiredFieldValidator7.Enabled = false;
            }

            if (Request.QueryString["type"] != null)
            {
                IHC.InspFlag theflag = IHC.InspFlag.FetchByID(Request.QueryString["flag"].ToString());
                this.txtPartNbr.Text = theflag.PartNo;
                this.txtEntDATE.Text = DateTime.Parse(theflag.EntryDte.ToString()).ToShortDateString();
                this.txtEntBy.Text = theflag.EnteredBy;
                this.txtReason.Text = theflag.Reason;
                this.DropDownList1.SelectedValue = theflag.Status.ToString();
                if (theflag.StatusDte != null)
                {
                    this.txtcloseddte.Text = DateTime.Parse(theflag.StatusDte.ToString()).ToShortDateString();
                }
                DateTime date1, date2, date3;
                Int32 q1, q2, q3;

                if (theflag.InspDTE1 != null)
                {
                    this.txtdte1.Text = DateTime.Parse(theflag.InspDTE1.ToString()).ToShortDateString();
                }
                if (theflag.InspDTE2 != null)
                {
                    this.txtdte2.Text = DateTime.Parse(theflag.InspDTE2.ToString()).ToShortDateString();
                }
                if (theflag.InspDTE3 != null)
                {
                    this.txtdte3.Text = DateTime.Parse(theflag.InspDTE3.ToString()).ToShortDateString();
                }

                if (theflag.InspQTY1 != null)
                {
                    this.txtqty1.Text = theflag.InspQTY1.ToString();
                }
                if (theflag.InspQTY2 != null)
                {
                    this.txtqty2.Text = theflag.InspQTY2.ToString();
                }
                if (theflag.InspQTY3 != null)
                {
                    this.txtqty3.Text = theflag.InspQTY3.ToString();
                }
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["flag"] == null)
        {
            if (!new DAL().PartNumberExists(this.txtPartNbr.Text.Trim())) { throw new Exception("Part number does not exist in Fourth Shift."); }

            //string type = Request.QueryString["type"].ToString();
            IHC.InspFlag flag = new IHC.InspFlag();
            flag.PartNo = this.txtPartNbr.Text.Trim();
            flag.EntryDte = DateTime.Parse(this.txtEntDATE.Text.Trim());
            flag.EnteredBy = this.txtEntBy.Text.Trim();
            flag.Reason = this.txtReason.Text.Trim();
            flag.Status = Int32.Parse(this.DropDownList1.SelectedValue);
            if (this.txtdte1.Text.Trim() != "") { flag.InspDTE1 = DateTime.Parse(this.txtdte1.Text.Trim()); }
            if (this.txtdte2.Text.Trim() != "") { flag.InspDTE2 = DateTime.Parse(this.txtdte2.Text.Trim()); }
            if (this.txtdte3.Text.Trim() != "") { flag.InspDTE3 = DateTime.Parse(this.txtdte3.Text.Trim()); }

            if (this.txtqty1.Text.Trim() != "") { flag.InspQTY1 = Int32.Parse(this.txtqty1.Text.Trim()); }
            if (this.txtqty2.Text.Trim() != "") { flag.InspQTY2 = Int32.Parse(this.txtqty2.Text.Trim()); }
            if (this.txtqty3.Text.Trim() != "") { flag.InspQTY3 = Int32.Parse(this.txtqty3.Text.Trim()); }

            flag.InspNOTE1 = this.txtnotes1.Text.Trim();
            flag.InspNOTE2 = this.txtnotes2.Text.Trim();
            flag.InspNOTE3 = this.txtnotes3.Text.Trim();

            //flag.StatusDte = DateTime.Parse(this.txtcloseddte.Text.Trim());
            flag.Save();

            Response.Redirect("../default.aspx?message=Flag has been saved. IF" + flag.FlagNo.ToString());
        }
        else
        {
            IHC.InspFlag flag = IHC.InspFlag.FetchByID(Request.QueryString["flag"].ToString());
            flag.PartNo = this.txtPartNbr.Text.Trim();
            flag.EntryDte = DateTime.Parse(this.txtEntDATE.Text.Trim());
            flag.EnteredBy = this.txtEntBy.Text.Trim();
            flag.Reason = this.txtReason.Text.Trim();
            flag.Status = Int32.Parse(this.DropDownList1.SelectedValue);
            if (this.txtdte1.Text.Trim() != "") { flag.InspDTE1 = DateTime.Parse(this.txtdte1.Text.Trim()); }
            if (this.txtdte2.Text.Trim() != "") { flag.InspDTE2 = DateTime.Parse(this.txtdte2.Text.Trim()); }
            if (this.txtdte3.Text.Trim() != "") { flag.InspDTE3 = DateTime.Parse(this.txtdte3.Text.Trim()); }

            if (this.txtqty1.Text.Trim() != "") { flag.InspQTY1 = Int32.Parse(this.txtqty1.Text.Trim()); }
            if (this.txtqty2.Text.Trim() != "") { flag.InspQTY2 = Int32.Parse(this.txtqty2.Text.Trim()); }
            if (this.txtqty3.Text.Trim() != "") { flag.InspQTY3 = Int32.Parse(this.txtqty3.Text.Trim()); }

            flag.InspNOTE1 = this.txtnotes1.Text.Trim();
            flag.InspNOTE2 = this.txtnotes2.Text.Trim();
            flag.InspNOTE3 = this.txtnotes3.Text.Trim();

            flag.StatusDte = DateTime.Parse(this.txtcloseddte.Text.Trim());
            
            flag.Save();

            Response.Redirect("../default.aspx?message=Flag has been saved. IF" + flag.FlagNo.ToString());
        }

    }
}