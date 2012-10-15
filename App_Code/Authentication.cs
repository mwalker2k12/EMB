using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;

/// <summary>
/// Summary description for Authentication
/// </summary>
public class Authentication
{
    public enum PermissionType
    {
        View,
        Edit
    }
    public Authentication()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static void Log(string type, string identifier, string notes)
    {
        try
        {
            IHC.TransactionLog logEntry = new IHC.TransactionLog();
            logEntry.TransactionComputer = HttpContext.Current.Server.MachineName.ToString();
            logEntry.TransactionIdentifier = identifier;
            logEntry.TransactionNotes = notes;
            logEntry.TransactionType = type;
            logEntry.TransactionUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            logEntry.Save();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static void ActivatePermissions(string pageKey, Page page)
    {
        if (WebConfigurationManager.AppSettings["SecurityEnabled"].ToString() == "true")
        {
            try
            {
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                IHC.UserPermissionCollection permission = new IHC.UserPermissionController().FetchByQuery(new SubSonic.Query(IHC.Tables.UserPermission)
                                            .WHERE(IHC.UserPermission.Columns.UserId, userName)
                                            .AND(IHC.UserPermission.Columns.UserPageCde, pageKey)
                                            .AND(IHC.UserPermission.Columns.UserPermissionX, 1));

                IHC.UserPermissionCollection permissionV = new IHC.UserPermissionController().FetchByQuery(new SubSonic.Query(IHC.Tables.UserPermission)
                                            .WHERE(IHC.UserPermission.Columns.UserId, userName)
                                            .AND(IHC.UserPermission.Columns.UserPageCde, pageKey)
                                            .AND(IHC.UserPermission.Columns.UserPermissionX, 0));

                if (permission.Count < 1 && permissionV.Count > 0)
                {
                    page.Form.Disabled = true;
                    CreateViewOnly(page);

                }
                else
                {
                    if (permissionV.Count < 1 && permission.Count < 1)
                    {
                        //no permissions at all!
                        page.Controls.Clear();
                        page.Response.Write("<center><img style='height:150px;width:250px' src='../Images/denied.gif'/></center>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("You do not have permission to view this page.");
            }
        }
    }
    public static void GoToDefault(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Redirect("../Default.aspx?message=" + "");
    }
    public static void CreateViewOnly(Control cntrl)
    {
        foreach (Control c in cntrl.Controls)
        {
            switch (c.GetType().ToString())
            {
                case "System.Web.UI.WebControls.TextBox":
                    TextBox t = (TextBox)c;
                    t.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.Button":
                    Button b = (Button)c;
                    b.Visible = false;
                    break;
                case "System.Web.UI.WebControls.DropDownList":
                    DropDownList ddl = (DropDownList)c;
                    ddl.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.CheckBoxList":
                    CheckBoxList cbl = (CheckBoxList)c;
                    cbl.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.CheckBox":
                    CheckBox cb = (CheckBox)c;
                    cb.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.RadioButton":
                    RadioButton rb = (RadioButton)c;
                    rb.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.RadioButtonList":
                    RadioButtonList rbl = (RadioButtonList)c;
                    rbl.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.LinkButton":
                    LinkButton lb = (LinkButton)c;
                    lb.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.ImageButton":
                    ImageButton ib = (ImageButton)c;
                    ib.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.FileUpload":
                    FileUpload fu = (FileUpload)c;
                    fu.Enabled = false;
                    break;
                case "System.Web.UI.WebControls.GridView":
                    GridView gv = (GridView)c;
                    gv.Enabled = false;
                    break;
                default:
                    break;
            }
            if (c.HasControls()) { CreateViewOnly(c); }
        }
    }
}
