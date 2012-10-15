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

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void InsertButton_Click(object sender, EventArgs e)
    {

        int q1 = StringToInt(this.txt1.Text);
        int q2 = StringToInt(this.txt2.Text);
        int q500 = StringToInt(this.txt500.Text);
        int q750 = StringToInt(this.txt750.Text);
        int q780 = StringToInt(this.txt780.Text);
        int q840 = StringToInt(this.txt840.Text);
        int q92 = StringToInt(this.txt92.Text);
        int q91 = StringToInt(this.txt841.Text);
        int q71 = StringToInt(this.txt71.Text);
        int q72 = StringToInt(this.txt72.Text);
        int q21 = StringToInt(this.txt21.Text);
        int q22 = StringToInt(this.txt22.Text);

        IHC.EmbRecLog lg = new IHC.EmbRecLog();
        lg.X1 = q1;
        lg.X2 = q2;
        lg.X500 = q500;
        lg.X750 = q750;
        lg.X780 = q780;
        lg.X840 = q840;
        lg.X92 = q92;
        lg.X91 = q91;
        lg.X71 = q71;
        lg.X72 = q72;
        lg.X21 = q21;
        lg.X22 = q22;
        lg.EmbRecDte = DateTime.Now;
        lg.Save();

        IHC.EmbPackageType p1 = IHC.EmbPackageType.FetchByID(1);
        p1.EmbOhQty = p1.EmbOhQty + q1;
        p1.Save();

        IHC.EmbPackageType p2 = IHC.EmbPackageType.FetchByID(2);
        p2.EmbOhQty = p2.EmbOhQty + q2;
        p2.Save();

        IHC.EmbPackageType p500 = IHC.EmbPackageType.FetchByID(500);
        p500.EmbOhQty = p500.EmbOhQty + q500;
        p500.Save();

        IHC.EmbPackageType p750 = IHC.EmbPackageType.FetchByID(750);
        p750.EmbOhQty = p750.EmbOhQty + q750;
        p750.Save();

        IHC.EmbPackageType p751 = IHC.EmbPackageType.FetchByID(751);
        p751.EmbOhQty = p751.EmbOhQty + q750;
        p751.Save();

        IHC.EmbPackageType p780 = IHC.EmbPackageType.FetchByID(780);
        p780.EmbOhQty = p780.EmbOhQty + q780;
        p780.Save();

        IHC.EmbPackageType p781 = IHC.EmbPackageType.FetchByID(781);
        p781.EmbOhQty = p781.EmbOhQty + q780;
        p781.Save();

        IHC.EmbPackageType p840 = IHC.EmbPackageType.FetchByID(840);
        p840.EmbOhQty = p840.EmbOhQty + q840;
        p840.Save();

        IHC.EmbPackageType p841 = IHC.EmbPackageType.FetchByID(841);
        p841.EmbOhQty = p841.EmbOhQty + q840;
        p841.Save();


        IHC.EmbPackageType p92 = IHC.EmbPackageType.FetchByID(92);
        p92.EmbOhQty = p92.EmbOhQty + q92;
        p92.Save();

        IHC.EmbPackageType p91 = IHC.EmbPackageType.FetchByID(91);
        p91.EmbOhQty = p91.EmbOhQty + q91;
        p91.Save();

        IHC.EmbPackageType p71 = IHC.EmbPackageType.FetchByID(71);
        p71.EmbOhQty = p71.EmbOhQty + q71;
        p71.Save();

        IHC.EmbPackageType p72 = IHC.EmbPackageType.FetchByID(72);
        p72.EmbOhQty = p72.EmbOhQty + q72;
        p72.Save();

        IHC.EmbPackageType p21 = IHC.EmbPackageType.FetchByID(21);
        p21.EmbOhQty = p21.EmbOhQty + q21;
        p21.Save();

        IHC.EmbPackageType p22 = IHC.EmbPackageType.FetchByID(22);
        p22.EmbOhQty = p22.EmbOhQty + q22;
        p22.Save();


        Response.Redirect("../default.aspx?message=Receipts have been saved.");
        
    }
    public int StringToInt(string s)
    {
        int retVal = 0;
        if (s == "")
        {
            retVal = 0;
        }
        else
        {
            retVal = Int32.Parse(s);
        }
        return retVal;
    }
}