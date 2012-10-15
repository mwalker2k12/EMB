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

public partial class Demand: System.Web.UI.Page
{
    int oh500 = 0;
    int oh750 = 0;
    int oh780 = 0;
    int oh840 = 0;
    int oq500 = 0;
    int oq750 = 0;
    int oq780 = 0;
    int oq840 = 0;

    public int GetOnHand(string pkgNo)
    {
        int retVal = 0;
        IHC.EmbPackageType pkg = IHC.EmbPackageType.FetchByID(pkgNo);
        retVal = Int32.Parse(pkg.EmbOhQty.ToString());
        return retVal;
    }
    public int GetOrderQty(string pkgNo)
    {
        int retVal = 0;
        IHC.EmbPackageType pkg = IHC.EmbPackageType.FetchByID(pkgNo);
        retVal = Int32.Parse(pkg.EmbOrderQty.ToString());
        return retVal;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        oh500 = GetOnHand("500");
        oq500 = GetOrderQty("500");
    
        //other ones

        DataTable dt = new DAL().GetEmballageDemand("500");
        foreach (DataRow r in dt.Rows)
        {
            if (r["WEEK"].ToString() == "1")
            {
                this.Label1.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "2")
            {
                this.Label2.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "3")
            {
                this.Label3.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "4")
            {
                this.Label4.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "5")
            {
                this.Label5.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "6")
            {
                this.Label6.Text = r["BINS"].ToString();
            }
        }
        //750
        dt = new DAL().GetEmballageDemand("750");
        foreach (DataRow r in dt.Rows)
        {
            if (r["WEEK"].ToString() == "1")
            {
                this.Label7.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "2")
            {
                this.Label8.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "3")
            {
                this.Label9.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "4")
            {
                this.Label10.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "5")
            {
                this.Label11.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "6")
            {
                this.Label12.Text = r["BINS"].ToString();
            }
        }
        //780
        dt = new DAL().GetEmballageDemand("780");
        foreach (DataRow r in dt.Rows)
        {
            if (r["WEEK"].ToString() == "1")
            {
                this.Label13.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "2")
            {
                this.Label14.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "3")
            {
                this.Label15.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "4")
            {
                this.Label16.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "5")
            {
                this.Label17.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "6")
            {
                this.Label18.Text = r["BINS"].ToString();
            }
        }
        //840
        dt = new DAL().GetEmballageDemand("840");
        foreach (DataRow r in dt.Rows)
        {
            if (r["WEEK"].ToString() == "1")
            {
                this.Label19.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "2")
            {
                this.Label20.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "3")
            {
                this.Label21.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "4")
            {
                this.Label22.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "5")
            {
                this.Label23.Text = r["BINS"].ToString();
            }
            if (r["WEEK"].ToString() == "6")
            {
                this.Label24.Text = r["BINS"].ToString();
            }
        }

        //TOTAL DEMAND

        //temp variable for forecast
        int temp = 0;
        //end temp

        int d500_1, d500_2, d500_3, d500_4, d500_5, d500_6;
        int d750_1, d750_2, d750_3, d750_4, d750_5, d750_6;
        int d780_1, d780_2, d780_3, d780_4, d780_5, d780_6;
        int d840_1, d840_2, d840_3, d840_4, d840_5, d840_6;

        d500_1 = Int32.Parse(this.Label1.Text);
        d500_2 = Int32.Parse(this.Label2.Text);
        d500_3 = Int32.Parse(this.Label3.Text);
        d500_4 = Int32.Parse(this.Label4.Text);
        d500_5 = Int32.Parse(this.Label5.Text);
        d500_6 = Int32.Parse(this.Label6.Text);

        d750_1 = Int32.Parse(this.Label7.Text);
        d750_2 = Int32.Parse(this.Label8.Text);
        d750_3 = Int32.Parse(this.Label9.Text);
        d750_4 = Int32.Parse(this.Label10.Text);
        d750_5 = Int32.Parse(this.Label11.Text);
        d750_6 = Int32.Parse(this.Label12.Text);

        d780_1 = Int32.Parse(this.Label13.Text);
        d780_2 = Int32.Parse(this.Label14.Text);
        d780_3 = Int32.Parse(this.Label15.Text);
        d780_4 = Int32.Parse(this.Label16.Text);
        d780_5 = Int32.Parse(this.Label17.Text);
        d780_6 = Int32.Parse(this.Label18.Text);

        d840_1 = Int32.Parse(this.Label19.Text);
        d840_2 = Int32.Parse(this.Label20.Text);
        d840_3 = Int32.Parse(this.Label21.Text);
        d840_4 = Int32.Parse(this.Label22.Text);
        d840_5 = Int32.Parse(this.Label23.Text);
        d840_6 = Int32.Parse(this.Label24.Text);


        this.Label25.Text = (d500_1 + temp).ToString();
        this.Label26.Text = (d500_2 + temp).ToString();
        this.Label27.Text = (d500_3 + temp).ToString();
        this.Label28.Text = (d500_4 + temp).ToString();
        this.Label29.Text = (d500_5 + temp).ToString();
        this.Label30.Text = (d500_6 + temp).ToString();

        this.Label31.Text = (d750_1 + temp).ToString();
        this.Label32.Text = (d750_2 + temp).ToString();
        this.Label33.Text = (d750_3 + temp).ToString();
        this.Label34.Text = (d750_4 + temp).ToString();
        this.Label35.Text = (d750_5 + temp).ToString();
        this.Label36.Text = (d750_6 + temp).ToString();

        this.Label37.Text = (d780_1 + temp).ToString();
        this.Label38.Text = (d780_2 + temp).ToString();
        this.Label39.Text = (d780_3 + temp).ToString();
        this.Label40.Text = (d780_4 + temp).ToString();
        this.Label41.Text = (d780_5 + temp).ToString();
        this.Label42.Text = (d780_6 + temp).ToString();

        this.Label43.Text = (d840_1 + temp).ToString();
        this.Label44.Text = (d840_2 + temp).ToString();
        this.Label45.Text = (d840_3 + temp).ToString();
        this.Label46.Text = (d840_4 + temp).ToString();
        this.Label47.Text = (d840_5 + temp).ToString();
        this.Label48.Text = (d840_6 + temp).ToString();

        //END TOTAL DEMAND

        //ON HAND & ORDER QTY


    }
}