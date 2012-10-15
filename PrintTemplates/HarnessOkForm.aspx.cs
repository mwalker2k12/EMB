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

public partial class PrintTemplates_HarnessOkForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string name = Request.QueryString["name"].ToString();
        string qty = Request.QueryString["qty"].ToString();
        string lotNo = Request.QueryString["lotNo"].ToString();
        string pn = "";

        DataTable dt = new DAL().GetFsManufactDetailByLotNo(lotNo);
        pn = dt.Rows[0]["ITEM"].ToString();

        this.lblPartNo.Text = pn;
        this.lblQty.Text = qty + " PCS";
        this.lblDate.Text = DateTime.Now.ToShortDateString();
        this.lblName.Text = "-" + name + "-";
    }
}
