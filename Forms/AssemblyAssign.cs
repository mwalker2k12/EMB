using IHC;
using SubSonic;
using System;
using System.Data;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class AssemblyAssign : System.Web.UI.Page
{

    private int isirCatg = -1;

    private string lotNo = "";
    private int wcId;
    protected CustomControls_WiControl WiControl1;

    protected void DropDownList4_DataBound(object sender, EventArgs e)
    {
        foreach (ListItem item in this.DropDownList4.Items)
        {
            item.Text = new Utility().ProperCase(item.Text);
        }
    }

    protected void InsertButton_Click(object sender, EventArgs e)
    {
        try
        {
            IHC.IsirAssemblyAssignment assignment = new IHC.IsirAssemblyAssignment();
                assignment.IsirAssyNo = this.lblAssy.Text;
                assignment.IsirEmpId = this.DropDownList4.SelectedValue;
                assignment.IsirIsIsir = bool.Parse(this.RadioButtonList1.SelectedValue);
                assignment.IsirLocNo = this.DropDownList5.SelectedValue;
                assignment.IsirMoNo = this.lblMo.Text;
                assignment.IsirMoQty = decimal.Parse(this.lblQty.Text);
                assignment.IsirDteAssn = new DateTime?(DateTime.Now);
          
            assignment.Save();
            if (assignment.IsirIsIsir)
            {
                Email.SendAssyAssignMail(assignment.IsirMoNo, assignment.IsirEmpId, assignment.IsirLocNo);
            }
            string s = "Assembly Assignment has been saved. Number " + assignment.IsirNo.ToString();
            base.Response.Redirect("../Default.aspx?message=" + base.Server.HtmlEncode(s));
        }
        catch (Exception exception)
        {
            this.ErrorHandler1.TypeOfMessage = MessageTypes.MessageType.Error;
            this.ErrorHandler1.ErrorMessage = exception.Message;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string lotNumber = base.Request.QueryString["mo"].ToString();
        this.lotNo = lotNumber;
        DataTable fsMoInfo = new DAL().GetFsMoInfo(lotNumber);
        string str2 = fsMoInfo.Rows[0]["ORDER_QTY"].ToString();
        string str3 = fsMoInfo.Rows[0]["ITEM"].ToString();
        this.lblMo.Text = lotNumber;
        this.lblAssy.Text = str3;
        this.lblQty.Text = str2;
        DataTable fsInspectionInfo = new DAL().GetFsInspectionInfo(lotNumber);
        fsInspectionInfo.Rows[0]["ASSY_CATG"].ToString();
        if (fsInspectionInfo.Rows[0]["ASSY_CATG"].ToString().Trim() == "")
        {
            this.lblAssyCatg.Text = "U - " + ReadOnlyRecord<AssemblyCategoryCode>.FetchByID("U").AssyDesc;
        }
        else
        {
            this.lblAssyCatg.Text = fsInspectionInfo.Rows[0]["ASSY_CATG"].ToString().Trim() + " - " 
                + ReadOnlyRecord<AssemblyCategoryCode>.FetchByID(fsInspectionInfo.Rows[0]["ASSY_CATG"].ToString().Trim()).AssyDesc;
        }
        bool flag = fsInspectionInfo.Rows[0]["BOARD"].ToString().Trim() == "BD";
        if (!flag)
        {
            //flag = fsInspectionInfo.Rows[0]["BOARD"].ToString().Trim() == "BIP";
            //flag = fsInspectionInfo.Rows[0]["BOARD"].ToString().Trim() == "BYR";
        }
        switch (fsInspectionInfo.Rows[0]["ASSY_CATG"].ToString().Trim())
        {
            case "H":
                if (flag)
                {
                    this.isirCatg = 1;
                }
                else
                {
                    this.isirCatg = 2;
                }
                break;

            case "B":
                this.isirCatg = 3;
                break;

            case "A":
                this.isirCatg = 2;
                break;

            case "M":
                this.isirCatg = 4;
                break;
        }
        switch (this.isirCatg)
        {
            case 1:
                this.wcId = 0x29;
                break;

            case 2:
                this.wcId = 0x2c;
                break;

            case 3:
                this.wcId = 0x2f;
                break;

            case 4:
                this.wcId = 13;
                break;
        }
        if (this.RadioButtonList1.SelectedValue == "True")
        {
            this.srcEmployees.SelectCommand = "SELECT ET_EMPLOYEES.EMP_NO, ET_EMPLOYEES.EMP_NO + ' - ' + EMP_NAME FROM\r\n                                                ET_EMPLOYEES,ET_ISIR_APPROVALS \r\n                                                WHERE \r\n                                                ET_ISIR_APPROVALS.EMP_NO = ET_EMPLOYEES.EMP_NO \r\n                                                AND\r\n                                                ISIR_CENTER_ID = " + this.isirCatg.ToString() + " AND(EMP_TERM_DTE IS NULL\r\n                                                OR LTRIM(RTRIM(CONVERT(VARCHAR(10),EMP_TERM_DTE))) = '')\r\n                                                ORDER BY EMP_NAME DESC";
        }
        else if (this.RadioButtonList1.SelectedValue == "False")
        {
            this.srcEmployees.SelectCommand = "SELECT EMP_NO, EMP_NO + ' - ' + EMP_NAME FROM\r\n                                                    ET_EMPLOYEES,ET_EMP_TRAINING_LEVELS \r\n                                                    WHERE \r\n                                                    ET_EMP_LVL_EMP_ID = EMP_NO\r\n                                                    AND\r\n                                                    ET_EMP_LVL_WC_ID = " + this.wcId + "\r\n                                                     AND\r\n                                                    EMP_TERM_DTE IS NULL\r\n                                                    OR LTRIM(RTRIM(CONVERT(VARCHAR(10),EMP_TERM_DTE))) = ''\r\n                                                    ORDER BY EMP_NAME DESC";
        }
        this.DropDownList4.DataBind();
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.DropDownList4.Items.Clear();
        this.DropDownList4.Items.Add(new ListItem("[Select]", "select"));
        if (this.RadioButtonList1.SelectedValue == "True")
        {
            this.srcEmployees.SelectCommand = "SELECT ET_EMPLOYEES.EMP_NO, ET_EMPLOYEES.EMP_NO + ' - ' + EMP_NAME FROM\r\n                                                ET_EMPLOYEES,ET_ISIR_APPROVALS \r\n                                                WHERE \r\n                                                ET_ISIR_APPROVALS.EMP_NO = ET_EMPLOYEES.EMP_NO \r\n                                                AND\r\n                                                ISIR_CENTER_ID = " + this.isirCatg.ToString() + " AND(EMP_TERM_DTE IS NULL\r\n                                                OR LTRIM(RTRIM(CONVERT(VARCHAR(10),EMP_TERM_DTE))) = '')\r\n                                                ORDER BY EMP_NAME DESC";
        }
        else
        {
            this.srcEmployees.SelectCommand = "SELECT EMP_NO, EMP_NO + ' - ' + EMP_NAME FROM\r\n                                                    ET_EMPLOYEES,ET_EMP_TRAINING_LEVELS \r\n                                                    WHERE \r\n                                                    ET_EMP_LVL_EMP_ID = EMP_NO\r\n                                                    AND\r\n                                                    ET_EMP_LVL_WC_ID = " + this.wcId + "\r\n                                                     AND\r\n                                                    EMP_TERM_DTE IS NULL\r\n                                                    OR LTRIM(RTRIM(CONVERT(VARCHAR(10),EMP_TERM_DTE))) = ''\r\n                                                    ORDER BY EMP_NAME DESC";
        }
        this.DropDownList4.DataBind();
    }


}

