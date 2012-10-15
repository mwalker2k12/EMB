using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CustomControls_Demand : System.Web.UI.UserControl
{
    private string _pkgNo;
    public int oh1 = 0;
    public int oq1 = 0;
    public int orq = 0;

    public string PkgNo
    {
        get { return _pkgNo; }
        set {_pkgNo = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Label31.Text = _pkgNo;

        oh1 = GetOnHand(_pkgNo);
        oq1 = GetOrderQty(_pkgNo);
        orq = GetOrderQty(_pkgNo);
        //other ones

        DataTable dt = new DAL().GetEmballageDemand(_pkgNo);
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
        //forecast
        DataTable dt2 = new DAL().GetEmballageForecast(_pkgNo);
        foreach (DataRow r2 in dt2.Rows)
        {
            if (r2["WEEK"].ToString() == "1")
            {
                this.Label50.Text = r2["BINS"].ToString();
            }
            if (r2["WEEK"].ToString() == "2")
            {
                this.Label51.Text = r2["BINS"].ToString();
            }
            if (r2["WEEK"].ToString() == "3")
            {
                this.Label52.Text = r2["BINS"].ToString();
            }
            if (r2["WEEK"].ToString() == "4")
            {
                this.Label53.Text = r2["BINS"].ToString();
            }
            if (r2["WEEK"].ToString() == "5")
            {
                this.Label54.Text = r2["BINS"].ToString();
            }
            if (r2["WEEK"].ToString() == "6")
            {
                this.Label55.Text = r2["BINS"].ToString();
            }
        }

        //TOTAL DEMAND

        //temp variable for forecast
        int f1 = Int32.Parse(this.Label50.Text);
        int f2 = Int32.Parse(this.Label51.Text);
        int f3 = Int32.Parse(this.Label52.Text);
        int f4 = Int32.Parse(this.Label53.Text);
        int f5 = Int32.Parse(this.Label54.Text);
        int f6 = Int32.Parse(this.Label55.Text);

        //end temp

        int d1, d2, d3, d4, d5, d6;

        d1 = Int32.Parse(this.Label1.Text) + f1;
        d2 = Int32.Parse(this.Label2.Text) + f2;
        d3 = Int32.Parse(this.Label3.Text) + f3;
        d4 = Int32.Parse(this.Label4.Text) + f4;
        d5 = Int32.Parse(this.Label5.Text) + f5;
        d6 = Int32.Parse(this.Label6.Text) + f6;

        this.Label25.Text = (d1 ).ToString();
        this.Label26.Text = (d2 ).ToString();
        this.Label27.Text = (d3 ).ToString();
        this.Label28.Text = (d4 ).ToString();
        this.Label29.Text = (d5 ).ToString();
        this.Label30.Text = (d6 ).ToString();

        //END TOTAL DEMAND

        //ON HAND & ORDER QTY

        int oh2, oh3, oh4, oh5, oh6;
        int oq2, oq3, oq4, oq5, oq6;
        oq2 = oq3 = oq4 = oq5 = oq6 = oq1;
        int s1, s2, s3, s4, s5, s6;

        this.Label32.Text = oh1.ToString();
        s1 = d1 - oh1;
        this.Label38.Text = s1.ToString();
        if (s1 > 1)
        {
            while (oq1 < s1)
            {
                oq1 += orq;
            }
            this.Label44.Text = oq1.ToString();
        }
        else
        {
            oq1 = 0;
        }

        //calc remainder of on hand demand etc
        oh2 = oh1 - d1 + oq1;
        this.Label33.Text = oh2.ToString();
        s2 = d2 - oh2;
        this.Label39.Text = s2.ToString();
        if (s2 >1)
        {
            while (oq2 < s2)
            {
                oq2 += orq;
            }
            this.Label45.Text = oq2.ToString();
        }
        else
        {
            oq2 = 0;
        }

        oh3 = oh2 - d2 + oq2;
        this.Label34.Text = oh3.ToString();
        s3 = d3 -oh3;
        this.Label40.Text = s3.ToString();
        if (s3 >1)
        {
            while (oq3 < s3)
            {
                oq3 += orq;
            }
            this.Label46.Text = oq3.ToString();
        }
        else
        {
            oq3 = 0;
        }

        oh4 = oh3 - d3 + oq3;
        this.Label35.Text = oh4.ToString();
        s4 = d4 - oh4;
        this.Label41.Text = s4.ToString();
        if (s4 >1)
        {
            while (oq4 < s4)
            {
                oq4 += orq;
            }
            this.Label47.Text = oq4.ToString();
        }
        else
        {
            oq4 = 0;
        }

        oh5 = oh4 - d4 + oq4;
        this.Label36.Text = oh5.ToString();
        s5 = d5 - oh5;
        this.Label42.Text = s5.ToString();
        if (s5 >1)
        {
            while (oq5 < s5)
            {
                oq5 += orq;
            }
            this.Label48.Text = oq5.ToString();
        }
        else
        {
            oq5 = 0;
        }

        oh6 = oh5 - d5 + oq5;
        this.Label37.Text = oh6.ToString();
        s6 = d6 -oh6;
        this.Label43.Text = s6.ToString();
        if (s6 >1)
        {
            while (oq6 < s6)
            {
                oq6 += orq;
            }
            this.Label49.Text = oq6.ToString();
        }
        else
        {
            oq6 = 0;
        }

    }
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
}
