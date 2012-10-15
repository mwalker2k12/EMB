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
        

        if (!this.IsPostBack)
        {
            new DAL().DeleteEmbShipments();
            //new DAL().TruncateEmbShipments();

            this.TextBox1.Text = "0";
            this.TextBox2.Text = "0";
            this.TextBox3.Text = "0";
            this.TextBox4.Text = "0";
            this.TextBox5.Text = "0";
            this.TextBox6.Text = "0";
            this.TextBox7.Text = "0";
            this.TextBox8.Text = "0";
            this.TextBox9.Text = "0";
            this.TextBox10.Text = "0";
            this.TextBox11.Text = "0";
            this.TextBox12.Text = "0";
            this.TextBox13.Text = "0";
            this.TextBox14.Text = "0";
            this.TextBox15.Text = "0";
            this.TextBox16.Text = "0";
            this.TextBox17.Text = "0";
            this.TextBox18.Text = "0";

            DataTable dt = new DAL().GetEmbShipments();
            foreach (DataRow r in dt.Rows)
            {
                IHC.EmbSHIPMENT2 s = new IHC.EmbSHIPMENT2();
                s.EmbBinQty = Int32.Parse(r["BINS"].ToString());
                s.EmbCustNme = r["SHIP_TO_NM"].ToString();
                s.EmbDate = DateTime.Parse(r["SHPMT_DATE"].ToString());
                s.EmbPcsPer = Int32.Parse(r["PCS_PER"].ToString());
                s.EmbPkgType = r["PKG_TYPE"].ToString();
                s.EmbShipQty = Int32.Parse(r["SHIP_QTY"].ToString());
                s.EmbShipToId = r["SHIP_TO_ID"].ToString();
                s.EmbItem = r["ITEM"].ToString();
                s.Save();
            }
            this.DataBind();
        }
    }
    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        //add new package types etc.
        IHC.EmbSHIPMENT2 s = new IHC.EmbSHIPMENT2();
        s.EmbShipToId = DropDownList1.SelectedValue;
        s.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        s.EmbPkgType = "1";
        s.EmbBinQty = Int32.Parse(this.TextBox1.Text);
        
        IHC.EmbSHIPMENT2 s1 = new IHC.EmbSHIPMENT2();
        s1.EmbShipToId = DropDownList1.SelectedValue;
        s1.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        s1.EmbPkgType = "2";
        s1.EmbBinQty = Int32.Parse(this.TextBox2.Text);
        
        IHC.EmbSHIPMENT2 s2 = new IHC.EmbSHIPMENT2();
        s2.EmbShipToId = DropDownList1.SelectedValue;
        s2.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        s2.EmbPkgType = "92";
        s2.EmbBinQty = Int32.Parse(this.TextBox3.Text);

        IHC.EmbSHIPMENT2 s3 = new IHC.EmbSHIPMENT2();
        s3.EmbShipToId = DropDownList1.SelectedValue;
        s3.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        s3.EmbPkgType = "91";
        s3.EmbBinQty = Int32.Parse(this.TextBox4.Text);

        IHC.EmbSHIPMENT2 s12 = new IHC.EmbSHIPMENT2();
        s12.EmbShipToId = DropDownList1.SelectedValue;
        s12.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        s12.EmbPkgType = "71";
        s12.EmbBinQty = Int32.Parse(this.TextBox13.Text);

        IHC.EmbSHIPMENT2 s13 = new IHC.EmbSHIPMENT2();
        s13.EmbShipToId = DropDownList1.SelectedValue;
        s13.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        s13.EmbPkgType = "72";
        s13.EmbBinQty = Int32.Parse(this.TextBox14.Text);

        s2.Save();
        s.Save();
        s1.Save();
        s3.Save();
        s12.Save();
        s13.Save();

        if (this.DropDownList2.SelectedIndex > 0)
        {
            IHC.EmbSHIPMENT2 s4 = new IHC.EmbSHIPMENT2();
            s4.EmbShipToId = DropDownList2.SelectedValue;
            s4.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s4.EmbPkgType = "1";
            s4.EmbBinQty = Int32.Parse(this.TextBox5.Text);

            IHC.EmbSHIPMENT2 s5 = new IHC.EmbSHIPMENT2();
            s5.EmbShipToId = DropDownList2.SelectedValue;
            s5.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s5.EmbPkgType = "2";
            s5.EmbBinQty = Int32.Parse(this.TextBox6.Text);

            IHC.EmbSHIPMENT2 s6 = new IHC.EmbSHIPMENT2();
            s6.EmbShipToId = DropDownList2.SelectedValue;
            s6.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s6.EmbPkgType = "92";
            s6.EmbBinQty = Int32.Parse(this.TextBox7.Text);

            IHC.EmbSHIPMENT2 s7 = new IHC.EmbSHIPMENT2();
            s7.EmbShipToId = DropDownList2.SelectedValue;
            s7.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s7.EmbPkgType = "91";
            s7.EmbBinQty = Int32.Parse(this.TextBox8.Text);

            IHC.EmbSHIPMENT2 s14 = new IHC.EmbSHIPMENT2();
            s14.EmbShipToId = DropDownList2.SelectedValue;
            s14.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s14.EmbPkgType = "71";
            s14.EmbBinQty = Int32.Parse(this.TextBox15.Text);


            IHC.EmbSHIPMENT2 s15 = new IHC.EmbSHIPMENT2();
            s15.EmbShipToId = DropDownList2.SelectedValue;
            s15.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s15.EmbPkgType = "72";
            s15.EmbBinQty = Int32.Parse(this.TextBox16.Text);

            s4.Save();
            s5.Save();
            s6.Save();
            s7.Save();
            s14.Save();
            s15.Save();
        }

        if (this.DropDownList3.SelectedIndex > 0)
        {
            IHC.EmbSHIPMENT2 s8 = new IHC.EmbSHIPMENT2();
            s8.EmbShipToId = DropDownList3.SelectedValue;
            s8.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s8.EmbPkgType = "1";
            s8.EmbBinQty = Int32.Parse(this.TextBox9.Text);

            IHC.EmbSHIPMENT2 s9 = new IHC.EmbSHIPMENT2();
            s9.EmbShipToId = DropDownList3.SelectedValue;
            s9.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s9.EmbPkgType = "2";
            s9.EmbBinQty = Int32.Parse(this.TextBox10.Text);

            IHC.EmbSHIPMENT2 s10 = new IHC.EmbSHIPMENT2();
            s10.EmbShipToId = DropDownList3.SelectedValue;
            s10.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s10.EmbPkgType = "92";
            s10.EmbBinQty = Int32.Parse(this.TextBox11.Text);

            IHC.EmbSHIPMENT2 s11 = new IHC.EmbSHIPMENT2();
            s11.EmbShipToId = DropDownList3.SelectedValue;
            s11.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s11.EmbPkgType = "91";
            s11.EmbBinQty = Int32.Parse(this.TextBox12.Text);

            IHC.EmbSHIPMENT2 s16 = new IHC.EmbSHIPMENT2();
            s16.EmbShipToId = DropDownList3.SelectedValue;
            s16.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s16.EmbPkgType = "71";
            s16.EmbBinQty = Int32.Parse(this.TextBox17.Text);

            IHC.EmbSHIPMENT2 s17 = new IHC.EmbSHIPMENT2();
            s17.EmbShipToId = DropDownList3.SelectedValue;
            s17.EmbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            s17.EmbPkgType = "72";
            s17.EmbBinQty = Int32.Parse(this.TextBox18.Text);

            s8.Save();
            s9.Save();
            s10.Save();
            s11.Save();
            s16.Save();
            s17.Save();

        }
        //new DAL().UpdateOhQty();

        Response.Redirect("Shipping_Review.aspx");
    }
    protected void GridView2_DataBound(object sender, EventArgs e)
    {

    }
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "ROWCOLOR").ToString() == "red")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
    {
       
    }
}