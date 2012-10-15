using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.IO;
using System.Web.Configuration;
using System.Collections.Generic;


/// <summary>
/// Summary description for Email
/// </summary>
public class Email
{
	public Email()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    public static void SendAssyAssignMail(string moNo, string empNo, string locNo)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.EtEmployee emp = IHC.EtEmployee.FetchByID(empNo);
            IHC.IsirLocation loc = IHC.IsirLocation.FetchByID(locNo);

            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/AssyAssign.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            html = html.Replace("[mo]", moNo);
            html = html.Replace("[emp]", new Utility().ProperCase(emp.EmpName));
            html = html.Replace("[loc]", loc.IsirLocNo + " - " + loc.IsirLocDesc);

            MailMessage message = new MailMessage();

            message.Subject = "ISIR M/O " + moNo;
            message.From = new MailAddress("server@indharness.com");
            message.To.Add(new DAL().GetEmailsByPosition("Admin").Rows[0]["EMAIL_ADDRESS"].ToString());
            message.To.Add(new DAL().GetEmailsByPosition("Manufacturing Managerx").Rows[0]["EMAIL_ADDRESS"].ToString());
            message.To.Add(new DAL().GetEmailsByPosition("Quality Manager").Rows[0]["EMAIL_ADDRESS"].ToString());
            message.To.Add(new DAL().GetEmailsByPosition("Engineering Manager").Rows[0]["EMAIL_ADDRESS"].ToString());

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }

            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "IHCDC01.IHCI.INDHARNESS.COM";
            client.Send(message);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }
    public static void SendMaintenaceVendorComplete(int WoNo)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.WoLog wo = IHC.WoLog.FetchByID(WoNo);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/MP/mwoVendor.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            MailMessage message = new MailMessage();

            html = html.Replace("[WO_NO]", "WO" + wo.WoWoNbr.ToString());
            html = html.Replace("[COMMENTS]", wo.WoComments);
            html = html.Replace("[EXPLAINATION]", wo.WoExplanation);
            html = html.Replace("[MAINT_NO]", "MI" + wo.MaintenanceId.ToString());
            html = html.Replace("[SERVICE_DTE]", DateTime.Parse(wo.WoSvcDte.ToString()).ToShortDateString());
            html = html.Replace("[EQUIP_NO]", wo.Equipment.EquipItemNO2);
            html = html.Replace("[EQUIP_DESC]", wo.Equipment.EquipDesc);
            html = html.Replace("[EQUIP_MFG]", wo.Equipment.EquipMfgName);
            html = html.Replace("[EQUIP_MODEL]", wo.Equipment.EquipMfgModel);
            html = html.Replace("[EQUIP_SERIAL]", wo.Equipment.EquipSerialNo);
            html = html.Replace("[EQUIP_DIE]", wo.Equipment.EquipDieNo);
            html = html.Replace("[EQUIP_LOC]", wo.Equipment.WorkCenterCode.WcDesc);
            html = html.Replace("[VEND_NAME]", wo.WoVendName);
            html = html.Replace("[VEND_CONTACT]", wo.WoVendContact);
            html = html.Replace("[BUS_PHONE_NO]", wo.WoBusPhoneNbr);
            html = html.Replace("[CELL_PHONE_NO]", wo.WoCellNbr);
            html = html.Replace("[VEND_EMAIL]", wo.WoEmailAddr);
            html = html.Replace("[MAINT_DESC]", "<br/>" + wo.MaintenanceInstruction.MaintDesc + "<br/>" + wo.WoDesc);


            message.Subject = "MAINTENANCE WORK ORDER TO OUTSIDE VENDOR - " + "WO" + wo.WoWoNbr.ToString();
            message.From = new MailAddress("server@indhrness.com");
            message.CC.Add(new DAL().GetEmailsByPosition("Admin").Rows[0]["EMAIL_ADDRESS"].ToString());

            DataTable ccDt = new DAL().GetEmailsByPosition("Manufacturing Manager,Maintenance");
            foreach (DataRow row in ccDt.Rows)
            {
                message.To.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
            }

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }

            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            client.Send(message);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }
    public static void SendMaintenacePartialComplete(int WoNo)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.WoLog wo = IHC.WoLog.FetchByID(WoNo);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/MP/mwoPartial.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            MailMessage message = new MailMessage();

            html = html.Replace("[WO_NO]", "WO" + wo.WoWoNbr.ToString());
            html = html.Replace("[COMMENTS]", wo.WoComments);
            html = html.Replace("[EXPLAINATION]", wo.WoExplanation);
            html = html.Replace("[MAINT_NO]", "MI" +  wo.MaintenanceId.ToString());
            html = html.Replace("[SERVICE_DTE]", DateTime.Parse(wo.WoSvcDte.ToString()).ToShortDateString());
            html = html.Replace("[EQUIP_NO]", wo.Equipment.EquipItemNO2);
            html = html.Replace("[EQUIP_DESC]", wo.Equipment.EquipDesc);
            html = html.Replace("[EQUIP_MFG]", wo.Equipment.EquipMfgName);
            html = html.Replace("[EQUIP_MODEL]", wo.Equipment.EquipMfgModel);
            html = html.Replace("[EQUIP_SERIAL]", wo.Equipment.EquipSerialNo);
            html = html.Replace("[EQUIP_DIE]", wo.Equipment.EquipDieNo);
            html = html.Replace("[EQUIP_LOC]", wo.Equipment.WorkCenterCode.WcDesc);
            html = html.Replace("[MAINT_DESC]", "<br/>" + wo.MaintenanceInstruction.MaintDesc + "<br/>" + wo.WoDesc);


            message.Subject = "MAINTENANCE WORK ORDER PARTIALLY COMPLETED - " + "WO" + wo.WoWoNbr.ToString();
            message.From = new MailAddress("server@indhrness.com");
            message.CC.Add(new DAL().GetEmailsByPosition("Admin").Rows[0]["EMAIL_ADDRESS"].ToString());

            DataTable ccDt = new DAL().GetEmailsByPosition("Manufacturing Manager,Maintenance");
            foreach (DataRow row in ccDt.Rows)
            {
                message.To.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
            }

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }

            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            client.Send(message);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }
    public static void SendMaintenaceComplete(int WoNo)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.WoLog wo = IHC.WoLog.FetchByID(WoNo);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/MP/mwoCompleted.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            MailMessage message = new MailMessage();

            html = html.Replace("[WO_NO]", "WO" + wo.WoWoNbr.ToString());
            html = html.Replace("[COMMENTS]", wo.WoComments);
            html = html.Replace("[MAINT_NO]", "MI" + wo.MaintenanceId.ToString());
            html = html.Replace("[SERVICE_DTE]",DateTime.Parse(wo.WoSvcDte.ToString()).ToShortDateString());
            html = html.Replace("[EQUIP_NO]", wo.Equipment.EquipItemNO2);
            html = html.Replace("[EQUIP_DESC]", wo.Equipment.EquipDesc);
            html = html.Replace("[EQUIP_MFG]", wo.Equipment.EquipMfgName);
            html = html.Replace("[EQUIP_MODEL]", wo.Equipment.EquipMfgModel);
            html = html.Replace("[EQUIP_SERIAL]", wo.Equipment.EquipSerialNo);
            html = html.Replace("[EQUIP_DIE]", wo.Equipment.EquipDieNo);
            html = html.Replace("[EQUIP_LOC]", wo.Equipment.WorkCenterCode.WcDesc);
            html = html.Replace("[MAINT_DESC]","<br/>" + wo.MaintenanceInstruction.MaintDesc + "<br/>" + wo.WoDesc);


            message.Subject = "MAINTENANCE WORK ORDER COMPLETED - " + "WO" + wo.WoWoNbr.ToString();
            message.From = new MailAddress("server@indhrness.com");
            message.CC.Add(new DAL().GetEmailsByPosition("Admin").Rows[0]["EMAIL_ADDRESS"].ToString());

            DataTable ccDt = new DAL().GetEmailsByPosition("Manufacturing Manager,Maintenance");
            foreach (DataRow row in ccDt.Rows)
            {
                message.To.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
            }

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }

            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            client.Send(message);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }
    public static void SendUnschedMaintenance(int WoNo,string urgency)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.WoLog wo = IHC.WoLog.FetchByID(WoNo);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/MP/maintEmail.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            MailMessage message = new MailMessage();
            string server = ConfigurationManager.AppSettings["Server"].ToString();
            string folderPath = @"\\" + server + @"\doc\EquipmentDocuments\" + wo.Equipment.EquipItemNo + @"\";
            DirectoryInfo di = new DirectoryInfo(folderPath);
            FileInfo[] files = di.GetFiles();
            string attachments = "";
            int i = 1;
            foreach(FileInfo file in files)
            {
                Attachment att = new Attachment(file.FullName);
                message.Attachments.Add(att);
                attachments +=  i.ToString() + ".  " + file.Name + "<br/>";
                i+=1;
            }
            //get temp attachments
            string tempFolder = @"\\" + server + @"\doc\UnscheduledMaintenanceDocuments\";
            di = new DirectoryInfo(tempFolder);
            files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                if(file.Name.StartsWith(wo.WoWoNbr.ToString()))
                {
                    Attachment att = new Attachment(file.FullName);
                    message.Attachments.Add(att);
                    attachments += i.ToString() + ".  " + file.Name + "<br/>";
                    i += 1;
                }
            }

            html = html.Replace("[WO_NO]", "WO" + wo.WoWoNbr.ToString());
            html = html.Replace("[URGENCY]", urgency);
            html = html.Replace("[PROB]", wo.WoDesc);
            html = html.Replace("[EQUIP_NO]", wo.Equipment.EquipItemNO2);
            html = html.Replace("[EQUIP_DESC]", wo.Equipment.EquipDesc);
            html = html.Replace("[EQUIP_MFG]", wo.Equipment.EquipMfgName);
            html = html.Replace("[EQUIP_MODEL]", wo.Equipment.EquipMfgModel);
            html = html.Replace("[EQUIP_SERIAL]", wo.Equipment.EquipSerialNo);
            html = html.Replace("[EQUIP_DIE]", wo.Equipment.EquipDieNo);
            html = html.Replace("[EQUIP_LOC]", wo.Equipment.WorkCenterCode.WcDesc);
            html = html.Replace("[MAINT_DESC]", "<br/>" + wo.WoDesc);
            html = html.Replace("[DOCS]", attachments);


            message.Subject = "UNSCHEDULED MAINTENANCE WORK ORDER - " + "WO" + wo.WoWoNbr.ToString();
            message.From = new MailAddress("server@indhrness.com");
            message.CC.Add(new DAL().GetEmailsByPosition("Admin").Rows[0]["EMAIL_ADDRESS"].ToString());

            DataTable ccDt = new DAL().GetEmailsByPosition("Manufacturing Manager,Maintenance");
            foreach (DataRow row in ccDt.Rows)
            {
                message.To.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
            }

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }

            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            client.Send(message);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }
    public static void SendRmoToVendor(int rmoNumber, string emailAddress)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.RmoLog rmo = IHC.RmoLog.FetchByID(rmoNumber);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/RMO/EmailVendorRMO.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            html = html.Replace("[vendor]", rmo.RmoVendorNme);
            html = html.Replace("[vendorPartNbr]", rmo.RmoVendorPartNbr);
            html = html.Replace("[ihcPartNbr]", rmo.IhcNcmLog.IncmPartNbr);
            html = html.Replace("[quantity]", rmo.IhcNcmLog.IncmQty.ToString());
            html = html.Replace("[initBy]", rmo.RmoInitBy);
            html = html.Replace("[rmoNumber]", rmo.RmoRmoNBR2);
            html = html.Replace("[vendorDisp]", rmo.VendorDispCode.VendorDispDesc);
            html = html.Replace("[credit]", rmo.RmoCreditAmt.ToString());
            html = html.Replace("[replacementPo]", rmo.RmoReplacementPoNbr);
            html = html.Replace("[reason]", rmo.IhcNcmLog.IncmReason);
            MailMessage message = new MailMessage();

            message.Subject = "RMO TO VENDOR - " + rmo.RmoRmoNBR2;
            message.From = new MailAddress("mriggins@indharness.com");
            message.CC.Add(new DAL().GetEmailsByPosition("Admin").Rows[0]["EMAIL_ADDRESS"].ToString());
            message.To.Add(emailAddress);
            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }

            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            client.Send(message); 
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }
    public static void SendNewEditRmoEmail(int rmoNumber)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.RmoLog rmo = IHC.RmoLog.FetchByID(rmoNumber);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/RMO/EditRMO.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            //DataTable dt = new DataTable();
            //dt = new Utility().GetTable((System.Data.SqlClient.SqlDataReader)IHC.IhcNcmLog.FetchByParameter(IHC.IhcNcmLog.Columns.IncmNcmNbr, ncmNumber));

            //string table = SubSonic.Utilities.Utility.DataTableToHTML(dt, "100%");
            //html += "<br/><br/><br/>" + table;    
            html = html.Replace("[rmoNumber]", rmo.RmoRmoNBR2);
            html = html.Replace("[initDte]", rmo.RmoInitDte.ToShortDateString());
            html = html.Replace("[partNumber]", rmo.IhcNcmLog.IncmPartNbr);
            html = html.Replace("[quantity]", rmo.IhcNcmLog.IncmQty.ToString());
            html = html.Replace("[vendorDisp]", rmo.VendorDispCode.VendorDispDesc);
            html = html.Replace("[ncmNumber]", rmo.RmoNcmNbr.ToString());
            MailMessage message = new MailMessage();
            
            message.Subject = "NOTIFICATION OF RMO EDIT - " + rmo.RmoRmoNBR2;
            message.From = new MailAddress("server@indharness.com");
            message.To.Add(new DAL().GetEmailsByPosition("Buyer").Rows[0]["EMAIL_ADDRESS"].ToString());

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }
            
            DataTable ccDt = new DAL().GetEmailsByPosition("Purchasing Clerk,Quality Manager,Admin");
            foreach (DataRow row in ccDt.Rows)
            {
                message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
            }
            if (rmo.RmoCreditAmt != null)
            {
                if (rmo.RmoCreditAmt.ToString().Trim() != "")
                {
                    message.CC.Add(new DAL().GetEmailsByPosition("Customer Service").Rows[0]["EMAIL_ADDRESS"].ToString());
                }
            }
            if (rmo.IhcNcmLog.IhcRmoDispositionCode.DispositionDesc.Contains("Vendor"))
            {
                message.CC.Add(new DAL().GetEmailsByPosition("Receiving Clerk").Rows[0]["EMAIL_ADDRESS"].ToString());
            }
            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            if (rmo.VendorDispCode != null)
            {
                if (!(rmo.VendorDispCode.VendorDispDesc == "Pending")) { client.Send(message); }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }

    public static void SendShipRmoEmail(int rmoNumber)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.RmoLog rmo = IHC.RmoLog.FetchByID(rmoNumber);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/RMO/ShipRMO.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            //DataTable dt = new DataTable();
            //dt = new Utility().GetTable((System.Data.SqlClient.SqlDataReader)IHC.IhcNcmLog.FetchByParameter(IHC.IhcNcmLog.Columns.IncmNcmNbr, ncmNumber));

            //string table = SubSonic.Utilities.Utility.DataTableToHTML(dt, "100%");
            //html += "<br/><br/><br/>" + table;    
            html = html.Replace("[rmoNumber]", rmo.RmoRmoNBR2);
            html = html.Replace("[rmoInitDte]", rmo.RmoInitDte.ToShortDateString());
            html = html.Replace("[partNumber]", rmo.IhcNcmLog.IncmPartNbr);
            html = html.Replace("[qty]", rmo.IhcNcmLog.IncmQty.ToString());
            html = html.Replace("[ncmNumber]", rmo.IhcNcmLog.IncmNcmNbr.ToString());
            html = html.Replace("[vendorDisp]",rmo.VendorDispCode.VendorDispDesc);
            html = html.Replace("[shipNotes]", rmo.RmoShipNotes);

            if (rmo.RmoShipDTE1 != null)
            {
                html = html.Replace("[row1]", HttpContext.Current.Server.HtmlDecode(String.Format("<tr><td style=\"text-align:center;border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;border-bottom: black 1px solid\">{0}</td><td style=\"border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; height: 23px\">{1}</td></tr>", DateTime.Parse(rmo.RmoShipDTE1.ToString()).ToShortDateString(), rmo.RmoSHIPQTY1.ToString())));
            }
            else
            {
                html = html.Replace("[row1]", "");
            }

            if (rmo.RmoShipDTE2 != null)
            {
                html = html.Replace("[row2]", HttpContext.Current.Server.HtmlDecode(String.Format("<tr><td style=\"text-align:center;border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;border-bottom: black 1px solid\">{0}</td><td style=\"border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; height: 23px\">{1}</td></tr>", DateTime.Parse(rmo.RmoShipDTE2.ToString()).ToShortDateString(), rmo.RmoSHIPQTY2.ToString())));
            }
            else
            {
                html = html.Replace("[row2]", "");
            }

            if (rmo.RmoSHIPDTE3 != null)
            {
                html = html.Replace("[row3]", HttpContext.Current.Server.HtmlDecode(String.Format("<tr><td style=\"text-align:center;border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid;border-bottom: black 1px solid\">{0}</td><td style=\"border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; height: 23px\">{1}</td></tr>", DateTime.Parse(rmo.RmoSHIPDTE3.ToString()).ToShortDateString(), rmo.RmoSHIPQTY3.ToString())));
            }
            else
            {
                html = html.Replace("[row3]", "");
            }

            MailMessage message = new MailMessage();
            
            message.Subject = "NOTIFICATION OF RMO - " + rmo.RmoRmoNBR2;
            message.From = new MailAddress("server@indharness.com");
            message.To.Add(new DAL().GetEmailsByPosition("Buyer").Rows[0]["EMAIL_ADDRESS"].ToString());
            DataTable ccDt = new DAL().GetEmailsByPosition("Quality Manager,Purchasing Clerk,Receiving Clerk,Admin");
            foreach (DataRow row in ccDt.Rows)
            {
                message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
            }

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }
            
            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            client.Send(message);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }

    public static void SendNewRmoEmail(int rmoNumber)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.RmoLog rmo = IHC.RmoLog.FetchByID(rmoNumber);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/RMO/NewRMO.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            //DataTable dt = new DataTable();
            //dt = new Utility().GetTable((System.Data.SqlClient.SqlDataReader)IHC.IhcNcmLog.FetchByParameter(IHC.IhcNcmLog.Columns.IncmNcmNbr, ncmNumber));

            //string table = SubSonic.Utilities.Utility.DataTableToHTML(dt, "100%");
            //html += "<br/><br/><br/>" + table;    
            html = html.Replace("[rmoNumber]", rmo.RmoRmoNBR2);
            html = html.Replace("[initBy]", rmo.RmoInitBy);
            html = html.Replace("[partNumber]", rmo.IhcNcmLog.IncmPartNbr);
            html = html.Replace("[quantity]", rmo.IhcNcmLog.IncmQty.ToString());
            html = html.Replace("[rmoReason]", rmo.IhcNcmLog.IncmReason);
            MailMessage message = new MailMessage();
            
            message.Subject = "NOTIFICATION OF RMO - " + rmo.RmoRmoNBR2;
            message.From = new MailAddress("server@indharness.com");
            message.To.Add(new DAL().GetEmailsByPosition("Buyer").Rows[0]["EMAIL_ADDRESS"].ToString());
            DataTable ccDt = new DAL().GetEmailsByPosition("Quality Manager,Purchasing Clerk,Admin");
            foreach (DataRow row in ccDt.Rows)
            {
                message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
            }

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }
            
            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            client.Send(message);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }
    public static void SendNewIncmEmail(int ncmNumber)
    {
        FileStream fs = null;
        StreamReader sr = null;
        try
        {
            IHC.IhcNcmLog ncm = IHC.IhcNcmLog.FetchByID(ncmNumber);
            fs = new FileStream(HttpContext.Current.Server.MapPath(@"../EmailTemplates/INCM/NewINCM.htm"), FileMode.Open);
            sr = new StreamReader(fs);
            string html = sr.ReadToEnd();

            //DataTable dt = new DataTable();
            //dt = new Utility().GetTable((System.Data.SqlClient.SqlDataReader)IHC.IhcNcmLog.FetchByParameter(IHC.IhcNcmLog.Columns.IncmNcmNbr, ncmNumber));

            //string table = SubSonic.Utilities.Utility.DataTableToHTML(dt, "100%");
            //html += "<br/><br/><br/>" + table;

            html = html.Replace("[ncmNumber]", ncm.IncmNcmNbr.ToString());
            html = html.Replace("[initBy]", ncm.IncmInitBy);
            html = html.Replace("[partNumber]",ncm.IncmPartNbr);
            html = html.Replace("[quantity]", ncm.IncmQty.ToString());
            html = html.Replace("[ncmReason]",ncm.IncmReason);
            MailMessage message = new MailMessage();
            
            message.Subject = "NOTIFICATION OF INTERNAL NCM - " + ncm.IncmNcmNbr.ToString();
            message.From = new MailAddress("server@indharness.com");
            message.To.Add(new DAL().GetEmailsByPosition("Quality Manager").Rows[0]["EMAIL_ADDRESS"].ToString());
            DataTable ccDt = new DAL().GetEmailsByPosition("Admin");
            foreach (DataRow row in ccDt.Rows)
            {
                message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
            }

            IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            if (initiator != null)
            {
                message.CC.Add(initiator.EmailAddress);
            }
            
            message.IsBodyHtml = true;
            message.Body = html;
            SmtpClient client = new SmtpClient();
            client.Host = "ihc1.IHCI.INDHARNESS.COM";
            client.Send(message);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            fs.Close();
        }

    }
    public static void SendRaReceivedEmail(string raNumber,bool changed)
    {
        DataTable dt = new DAL().GetRaEmailInfo(raNumber);
        if (dt.Rows[0]["RA_NCM_NBR"].ToString().Trim() != "")
        {
            SendRaWithNcm(dt,raNumber,changed);
        }
        else
        {
            SendRaWithoutNcm(dt, raNumber, changed);
        }
    }
    private static void SendRaWithNcm(DataTable dt,string raNumber,bool changed)
    {
        MailMessage message = new MailMessage();
        
        message.From = new MailAddress("server@indharness.com");
        message.To.Add(new DAL().GetEmailsByPosition("Quality Manager").Rows[0]["EMAIL_ADDRESS"].ToString());
        DataTable ccDt = new DAL().GetEmailsByPosition("Receiving Clerk,Admin");
        foreach (DataRow row in ccDt.Rows)
        {
            message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
        }
        if (changed)
        {
            message.Subject = "REVISED NOTIFICATION RECEIPT OF RETURNED GOODS - " + raNumber;
        }
        else
        {
            message.Subject = "NOTIFICATION OF RECEIPT OF RETURNED GOODS - " + raNumber;
        }
        message.Priority = MailPriority.High;
        message.IsBodyHtml = true;

        //respondEmails = respondEmails.Remove(respondEmails.Length - 1, 1);
        //message.Body += "<h2>This is a test email- With NCM.</h2><br/><br/>";
        message.Body += "<b>R/A Number:</b> " + dt.Rows[0]["RA_RA_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer:</b> " + new DAL().GetCustomerNameById(dt.Rows[0]["RA_CUST_CDE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Date:</b> " + Utility.GetDateString(dt.Rows[0]["RA_ISSUE_DTE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Quantity:</b> " + dt.Rows[0]["RA_QTY"].ToString() + "<br/>";
        message.Body += "<b>Customer Part Number:</b> " + dt.Rows[0]["RA_CUST_PART_NBR"].ToString() + "<br/>";
        message.Body += "<b>NCM Number:</b> " + dt.Rows[0]["RA_NCM_NBR"].ToString() + "<br/>";
        message.Body += "<b>Return PO Number:</b> " + dt.Rows[0]["RA_RETURN_PO_NBR"].ToString() + "<br/>";
        message.Body += "<br/><br/>";
        message.Body += @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>The following items have been received against the R/A number listed above:<o:p></o:p></span></p>";
        DataTable dtParts = new DAL().GetRaIhcPartsDataTable(raNumber);
        message.Body += "<table>";
        message.Body += "<tr><th style=\"border:solid 1px black;background-color:WhiteSmoke\">Part Number</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Receipt Qty</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Receipt Date</th></tr>";

        int actualRecQty = 0;

        foreach (DataRow row in dtParts.Rows)
        {
            actualRecQty += Utility.GetIntFromString(row["RA_RECIEPT_QTY1"].ToString()) + Utility.GetIntFromString(row["RA_RECIEPT_QTY2"].ToString()) + Utility.GetIntFromString(row["RA_RECIEPT_QTY3"].ToString());
            if (row["RA_IHC_PART_NBR1"].ToString() != "") { message.Body += "<tr><td style=\"border: 1px solid black\">" + row["RA_IHC_PART_NBR1"].ToString() + "</td><td style=\"text-align:center;border: 1px solid black\">" + row["RA_RECIEPT_QTY1"].ToString() + "<td style=\"border: 1px solid black\">" + Utility.GetDateString(row["RA_RECEIPT_DTE1"].ToString()) + "</td><tr/>"; }
            if (row["RA_IHC_PART_NBR2"].ToString() != "") { message.Body += "<tr><td style=\"border: 1px solid black\">" + row["RA_IHC_PART_NBR2"].ToString() + "</td><td style=\"text-align:center;border: 1px solid black\">" + row["RA_RECIEPT_QTY2"].ToString() + "<td style=\"border: 1px solid black\">" + Utility.GetDateString(row["RA_RECEIPT_DTE2"].ToString()) + "</td><tr/>"; }
            if (row["RA_IHC_PART_NBR3"].ToString() != "") { message.Body += "<tr><td style=\"border: 1px solid black\">" + row["RA_IHC_PART_NBR3"].ToString() + "</td><td style=\"text-align:center;border: 1px solid black\">" + row["RA_RECIEPT_QTY3"].ToString() + "<td style=\"border: 1px solid black\">" + Utility.GetDateString(row["RA_RECEIPT_DTE3"].ToString()) + "</td><tr/>"; }
        }

        message.Body += "</table><br/>";
        message.Body += "<b>Reason For Return:</b> " + dt.Rows[0]["RA_RETURN_DESC"].ToString() + "<br/><br/><br/>";
        if (Utility.GetIntFromString(dt.Rows[0]["RA_QTY"].ToString()) != actualRecQty)
        {
            message.Body += "<b style=\"color:red;font-size:16\">NOTE: ADDITIONAL ACTION REQUIRED:</b><br/>";
            message.Body += "<b style=\"color:red;font-size:16\">Quanity Received does not match issued quantity.</b><br/>";
        }
        SmtpClient client = new SmtpClient();
        client.Host = "ihc1.IHCI.INDHARNESS.COM";
        client.Send(message);
    }
    private static void SendRaWithoutNcm(DataTable dt, string raNumber,bool changed)
    {
        MailMessage message = new MailMessage();
        
        message.From = new MailAddress("server@indharness.com");
        message.To.Add(new DAL().GetEmailsByPosition("Sales").Rows[0]["EMAIL_ADDRESS"].ToString());
        DataTable ccDt = new DAL().GetEmailsByPosition("Receiving Clerk,Admin");
        foreach (DataRow row in ccDt.Rows)
        {
            message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
        }

        IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        if (initiator != null)
        {
            message.CC.Add(initiator.EmailAddress);
        }
            
        if (changed)
        {
            message.Subject = "REVISED NOTIFICATION OF RETURNED GOODS - " + raNumber;
        }
        else 
        { 
            message.Subject = "NOTIFICATION OF RECEIPT OF RETURNED GOODS - " + raNumber;
        }
        message.Priority = MailPriority.High;
        message.IsBodyHtml = true;

        //respondEmails = respondEmails.Remove(respondEmails.Length - 1, 1);
        //message.Body += "<h2>This is a test email- NO NCM.</h2><br/><br/>";
        message.Body += "<b>R/A Number:</b> " + dt.Rows[0]["RA_RA_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer:</b> " + new DAL().GetCustomerNameById(dt.Rows[0]["RA_CUST_CDE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Date:</b> " + Utility.GetDateString(dt.Rows[0]["RA_ISSUE_DTE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Quantity:</b> " + dt.Rows[0]["RA_QTY"].ToString() + "<br/>";
        message.Body += "<b>Customer Part Number:</b> " + dt.Rows[0]["RA_CUST_PART_NBR"].ToString() + "<br/>";
        message.Body += "<b>Return PO Number:</b> " + dt.Rows[0]["RA_RETURN_PO_NBR"].ToString() + "<br/>";
        message.Body += "<br/><br/>";
        message.Body += @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>The following items have been received against the R/A number listed above:<o:p></o:p></span></p>";
        DataTable dtParts = new DAL().GetRaIhcPartsDataTable(raNumber);
        message.Body += "<table>";
        message.Body += "<tr><th style=\"border:solid 1px black;background-color:WhiteSmoke\">Part Number</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Receipt Qty</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Receipt Date</th></tr>";

        int actualRecQty = 0;

        foreach (DataRow row in dtParts.Rows)
        {
            actualRecQty += Utility.GetIntFromString(row["RA_RECIEPT_QTY1"].ToString()) + Utility.GetIntFromString(row["RA_RECIEPT_QTY2"].ToString()) + Utility.GetIntFromString(row["RA_RECIEPT_QTY3"].ToString());
            if (row["RA_IHC_PART_NBR1"].ToString() != "") { message.Body += "<tr><td style=\"border: 1px solid black\">" + row["RA_IHC_PART_NBR1"].ToString() + "</td><td style=\"text-align:center;border: 1px solid black\">" + row["RA_RECIEPT_QTY1"].ToString() + "<td style=\"border: 1px solid black\">" + Utility.GetDateString(row["RA_RECEIPT_DTE1"].ToString()) + "</td><tr/>"; }
            if (row["RA_IHC_PART_NBR2"].ToString() != "") { message.Body += "<tr><td style=\"border: 1px solid black\">" + row["RA_IHC_PART_NBR2"].ToString() + "</td><td style=\"text-align:center;border: 1px solid black\">" + row["RA_RECIEPT_QTY2"].ToString() + "<td style=\"border: 1px solid black\">" + Utility.GetDateString(row["RA_RECEIPT_DTE2"].ToString()) + "</td><tr/>"; }
            if (row["RA_IHC_PART_NBR3"].ToString() != "") { message.Body += "<tr><td style=\"border: 1px solid black\">" + row["RA_IHC_PART_NBR3"].ToString() + "</td><td style=\"text-align:center;border: 1px solid black\">" + row["RA_RECIEPT_QTY3"].ToString() + "<td style=\"border: 1px solid black\">" + Utility.GetDateString(row["RA_RECEIPT_DTE3"].ToString()) + "</td><tr/>"; }
        }
        message.Body += "</table><br/>";
        message.Body += "<b>Reason For Return:</b> " + dt.Rows[0]["RA_RETURN_DESC"].ToString() + "<br/><br/><br/>";
        if (Utility.GetIntFromString(dt.Rows[0]["RA_QTY"].ToString()) != actualRecQty)
        {
            message.Body += "<b style=\"color:red;font-size:16\">NOTE: ADDITIONAL ACTION REQUIRED:</b><br/>";
            message.Body += "<b style=\"color:red;font-size:16\">Quanity Received does not match issued quantity.</b><br/>";
        }
        SmtpClient client = new SmtpClient();
        client.Host = "ihc1.IHCI.INDHARNESS.COM";
        client.Send(message);
    }
    public static void SendAdvanceRroEmail(string raNumber)
    {
        MailMessage message = new MailMessage();
        
        message.From = new MailAddress("server@indharness.com");
        message.To.Add(new DAL().GetEmailsByPosition("Engineering Manager").Rows[0]["EMAIL_ADDRESS"].ToString());
        DataTable ccDt = new DAL().GetEmailsByPosition("Sales,Quality Manager,Customer Service,Admin");
        foreach (DataRow row in ccDt.Rows)
        {
            message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
        }

        IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        if (initiator != null)
        {
            message.CC.Add(initiator.EmailAddress);
        }
            
        message.Subject = "NOTIFICATION OF ADVANCE REPAIR/REWORK DOCUMENTATION - " + raNumber;
        message.Priority = MailPriority.High;
        message.IsBodyHtml = true;

        DataTable dt = new DAL().GetRaEmailInfo(raNumber);
        //message.Body += "<h2>This is a test email.</h2><br/><br/>";
        message.Body += "<b>R/A Number:</b> " + dt.Rows[0]["RA_RA_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer:</b> " + new DAL().GetCustomerNameById(dt.Rows[0]["RA_CUST_CDE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Date:</b> " + Utility.GetDateString(dt.Rows[0]["RA_ISSUE_DTE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Quantity:</b> " + dt.Rows[0]["RA_QTY"].ToString() + "<br/>";
        message.Body += "<b>Customer Part Number:</b> " + dt.Rows[0]["RA_CUST_PART_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer Non-Conformance Number:</b> " + dt.Rows[0]["RA_NCM_NBR"].ToString() + "<br/>";
        message.Body += "<br/><br/>";
        message.Body += @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>A request has been made for Rework/Repair documentation in advance for the above R/A number.<o:p></o:p></span></p>";
        message.Body += "<b>Repair/Rework Requirements:</b> " + dt.Rows[0]["RA_RETURN_DESC"].ToString() + "<br/>";
        SmtpClient client = new SmtpClient();
        client.Host = "ihc1.IHCI.INDHARNESS.COM";
        client.Send(message);
    }
    public static void SendRroWithOutNcm(string raNumber)
    {
        MailMessage message = new MailMessage();
        
        message.From = new MailAddress("server@indharness.com");
        message.To.Add(new DAL().GetEmailsByPosition("Engineering Manager").Rows[0]["EMAIL_ADDRESS"].ToString());
        DataTable ccDt = new DAL().GetEmailsByPosition("Sales,Quality Manager,Customer Service,Admin");
        foreach (DataRow row in ccDt.Rows)
        {
            message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
        }

        IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        if (initiator != null)
        {
            message.CC.Add(initiator.EmailAddress);
        }
            
        DataTable dt = new DAL().GetRaEmailInfo(raNumber);

        message.Subject = "NOTIFICATION OF REPAIR/REWORK ORDER REQUEST - " + raNumber;
        message.Priority = MailPriority.High;
        message.IsBodyHtml = true;


        //message.Body += "<h2>This is a test email.</h2><br/><br/>";
        message.Body += "<b>R/A Number:</b> " + dt.Rows[0]["RA_RA_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer:</b> " + new DAL().GetCustomerNameById(dt.Rows[0]["RA_CUST_CDE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Date:</b> " + Utility.GetDateString(dt.Rows[0]["RA_ISSUE_DTE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Quantity:</b> " + dt.Rows[0]["RA_QTY"].ToString() + "<br/>";
        message.Body += "<b>Customer Part Number:</b> " + dt.Rows[0]["RA_CUST_PART_NBR"].ToString() + "<br/>";
        message.Body += "<br/><br/>";
        message.Body += @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>Rework/Repair Order requests have been generated for the following part numbers:<o:p></o:p></span></p><br/>";

        DataTable dt2 = new DAL().GetRroEmailInfo(raNumber);
        message.Body += "<table>";
        message.Body += "<tr><th style=\"border:solid 1px black;background-color:WhiteSmoke\">RRO Number</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Part Number</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Receipt Qty</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Receipt Date</th><th style=\"border:solid 1px black;background-color:WhiteSmoke\">Billable?</th><th style=\"border:solid 1px black;background-color:WhiteSmoke\">Record Labor?</th></tr>";
        foreach (DataRow row in dt2.Rows)
        {
            message.Body += "<tr><td style=\"border: 1px solid black\">" + row["RR_RA_NBR"].ToString() + "-" + Utility.Pad(row["RR_RO_NBR"].ToString()) + "</td><td style=\"border: 1px solid black\">" + row["RR_IHC_PART_NBR"].ToString() + "</td><td style=\"border: 1px solid black\">" + row["RR_QTY"].ToString() + "</td><td style=\"border: 1px solid black\">" + Utility.GetDateString(row["RR_REC_DTE"].ToString()) + "</td></td><td style=\"border: 1px solid black\">" + (row["RR_BILLABLE"].ToString().ToLower() == "true" ? "Yes" : "No") + "</td><td style=\"border: 1px solid black\">" + (row["RR_RECORD_LABOR"].ToString().ToLower() == "true" ? "Yes" : "No") + "</td><tr/>";
        }
        message.Body += "</table><br/>";
        message.Body += "<b>Repair/Rework Requirements:</b> " + dt.Rows[0]["RA_RETURN_DESC"].ToString() + "<br/>";
        SmtpClient client = new SmtpClient();
        client.Host = "ihc1.IHCI.INDHARNESS.COM";
        client.Send(message);
    }
    public static void SendRroWithNcm(string raNumber)
    {
        MailMessage message = new MailMessage();
        
        message.From = new MailAddress("server@indharness.com");
        message.To.Add(new DAL().GetEmailsByPosition("Engineering Manager").Rows[0]["EMAIL_ADDRESS"].ToString());
        DataTable ccDt = new DAL().GetEmailsByPosition("Sales,Quality Manager,Customer Service,Admin");
        foreach (DataRow row in ccDt.Rows)
        {
            message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
        }

        IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        if (initiator != null)
        {
            message.CC.Add(initiator.EmailAddress);
        }
            
        DataTable dt = new DAL().GetRaEmailInfo(raNumber);

        message.Subject = "NOTIFICATION OF REPAIR/REWORK ORDER REQUEST - " + raNumber;
        message.Priority = MailPriority.High;
        message.IsBodyHtml = true;


        //message.Body += "<h2>This is a test email.</h2><br/><br/>";
        message.Body += "<b>R/A Number:</b> " + dt.Rows[0]["RA_RA_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer:</b> " + new DAL().GetCustomerNameById(dt.Rows[0]["RA_CUST_CDE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Date:</b> " + Utility.GetDateString(dt.Rows[0]["RA_ISSUE_DTE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Quantity:</b> " + dt.Rows[0]["RA_QTY"].ToString() + "<br/>";
        message.Body += "<b>Customer Part Number:</b> " + dt.Rows[0]["RA_CUST_PART_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer Non-Conformance Number:</b> " + dt.Rows[0]["RA_NCM_NBR"].ToString() + "<br/>";
        message.Body += "<br/><br/>";
        message.Body += @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>Rework/Repair Order requests have been generated for the following part numbers:<o:p></o:p></span></p><br/>";

        DataTable dt2 = new DAL().GetRroEmailInfo(raNumber);
        message.Body += "<table>";
        message.Body += "<tr><th style=\"border:solid 1px black;background-color:WhiteSmoke\">RRO Number</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Part Number</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Receipt Qty</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Receipt Date</th><th style=\"border:solid 1px black;background-color:WhiteSmoke\">Billable?</th><th style=\"border:solid 1px black;background-color:WhiteSmoke\">Record Labor?</th></tr>";
        foreach (DataRow row in dt2.Rows)
        {
            message.Body += "<tr><td style=\"border: 1px solid black\">" + row["RR_RA_NBR"].ToString() + "-" + Utility.Pad(row["RR_RO_NBR"].ToString()) + "</td><td style=\"border: 1px solid black\">" + row["RR_IHC_PART_NBR"].ToString() + "</td><td style=\"border: 1px solid black\">" + row["RR_QTY"].ToString() + "</td><td style=\"border: 1px solid black\">" + Utility.GetDateString(row["RR_REC_DTE"].ToString()) + "</td></td><td style=\"border: 1px solid black\">" + (row["RR_BILLABLE"].ToString().ToLower() == "true" ? "Yes" : "No") + "</td><td style=\"border: 1px solid black\">" + (row["RR_RECORD_LABOR"].ToString().ToLower() == "true" ? "Yes" : "No") + "</td><tr/>";
        }
        message.Body += "</table><br/>";
        message.Body += "<b>Repair/Rework Requirements:</b> " + dt.Rows[0]["RA_RETURN_DESC"].ToString() + "<br/><br/>";
        message.Body += "<b>Customer Notes:</b> " + dt.Rows[0]["NCM_NOTES"].ToString() + "<br/><br/>";
        message.Body += "<b>Initial Investigation:</b> " + dt.Rows[0]["NCM_INIT_INVESTIGATION"].ToString() + "<br/><br/>";
        SmtpClient client = new SmtpClient();
        client.Host = "ihc1.IHCI.INDHARNESS.COM";
        client.Send(message);
    }
    public static void SendRaEmail(string raNumber,string customerEmail, string customerName)
    {
        MailMessage message = new MailMessage();
        
        message.From = new MailAddress("server@indharness.com");
        message.To.Add(customerEmail);
        DataTable ccDt = new DAL().GetEmailsByPosition("Sales,Quality Manager,Customer Service,Admin");
        foreach (DataRow row in ccDt.Rows)
        {
            message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
        }

        IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        if (initiator != null)
        {
            message.CC.Add(initiator.EmailAddress);
        }
            
        message.Subject = "NOTIFICATION OF RETURN AUTHORIZATION - " + raNumber;
        message.Priority = MailPriority.High;
        message.IsBodyHtml = true;

        DataTable dtEmails = new DAL().GetEmailsByPosition("Sales,Customer Service");
        string respondEmails = "";
        foreach (DataRow row in dtEmails.Rows)
        {
            respondEmails += row["EMAIL_ADDRESS"].ToString() + ";";
        }
        respondEmails = respondEmails.Remove(respondEmails.Length - 1, 1);
        DataTable dt = new DAL().GetRaEmailInfo(raNumber);
        //message.Body += "<h2>This is a test email.</h2><br/><br/>";
        message.Body += "<b>R/A Number:</b> " + dt.Rows[0]["RA_RA_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer:</b> " + new DAL().GetCustomerNameById(dt.Rows[0]["RA_CUST_CDE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Date:</b> " + Utility.GetDateString(dt.Rows[0]["RA_ISSUE_DTE"].ToString()) + "<br/>";
        message.Body += "<b>Issue Quantity:</b> " + dt.Rows[0]["RA_QTY"].ToString() + "<br/>";
        message.Body += "<b>Customer Part Number:</b> " + dt.Rows[0]["RA_CUST_PART_NBR"].ToString() + "<br/>";
        message.Body += "<b>Customer Non-Conformance Number:</b> " + dt.Rows[0]["RA_NCM_NBR"].ToString() + "<br/>";
        message.Body += "<br/><br/>";
        message.Body += @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>Dear " + customerName + ",<o:p></o:p></span></p>" +
                        @"" +
                        @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'><o:p>&nbsp;</o:p></span></p>" +
                        @"" +
                        @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>Please complete the following two steps to ensure this return is processed " +
                        @"correctly in a timely manner:&nbsp;" +
                        @"<o:p></o:p></span></p>" +
                        @"" +
                        @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'><o:p>&nbsp;</o:p></span></p>" +
                        @"" +
                        @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>1. FORWARD this email to "+ respondEmails +
                        @" to <b>advise your Return P/O number and debit amount (if any)" +
                        @"associated with this return</b>.&nbsp; <o:p></o:p></span></p>" +
                        @"" +
                         @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>2. RECORD the R/A number listed above on your " +
                        @"shipping documentation when these goods are returned to Industrial Harness company.<o:p></o:p></span></p>" +
                        @"" +
                        @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'><o:p>&nbsp;</o:p></span></p>" +
                        @"" +
                        @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>Thank you.<o:p></o:p></span></p><br/>";
        SmtpClient client = new SmtpClient();

        client.Host = "ihc1.IHCI.INDHARNESS.COM";
        client.Send(message);
    }
    public static void SendIrEmail(string irNumber)
    {
        MailMessage message = new MailMessage();
        
        message.From = new MailAddress("server@indharness.com");
        message.To.Add(new DAL().GetEmailsByPosition("Engineering Manager").Rows[0]["EMAIL_ADDRESS"].ToString());
        DataTable dt = new DAL().GetIrEmailInfo(irNumber);
        string recips = (dt.Rows[0]["IR_INIT_BY"].ToString().Trim() != "Ihc" ?
            "Sales,Quality Manager,Customer Service,Admin" : "Quality Manager,Customer Service,Admin");

        DataTable ccDt = new DAL().GetEmailsByPosition(recips);
        foreach (DataRow row in ccDt.Rows)
        {
            message.CC.Add(new MailAddress(row["EMAIL_ADDRESS"].ToString()));
        }

        IHC.IhcEmailPosition initiator = IHC.IhcEmailPosition.FetchByID(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        if (initiator != null)
        {
            message.CC.Add(initiator.EmailAddress);
        }
            
        message.Subject = "NOTIFICATION OF INTERNAL REPAIR/REWORK ORDER REQUEST - " + irNumber;
        message.Priority = MailPriority.High;
        message.IsBodyHtml = true;


        //message.Body += "<h2>This is a test email.</h2><br/><br/>";
        message.Body += "<b>I/R Number:</b> " + dt.Rows[0]["IR_IR_NBR"].ToString() + "<br/>";
        message.Body += "<b>Issue Date:</b> " + Utility.GetDateString(dt.Rows[0]["IR_ISSUE_DTE"].ToString()) + "<br/>";
        message.Body += "<b>IHC Part Number:</b> " + dt.Rows[0]["IR_IHC_PART_NBR"].ToString() + "<br/>";
        message.Body += "<b>Billable:</b> " + (dt.Rows[0]["IR_BILLABLE"].ToString().ToLower() == "true" ? "Yes" : "No") + "<br/>";
        message.Body += "<b>Record Labor:</b> " + (dt.Rows[0]["IR_RECORD_LABOR"].ToString().ToLower() == "true" ? "Yes" : "No") + "<br/>";
        message.Body += "<br/><br/>";
        message.Body += @"<p class=MsoNormal><span style='font-size:11.0pt;font-family:""Calibri"",""sans-serif"";" +
                        @"color:#1F497D'>Internal Rework/Repair Order requests have been generated for the following part numbers:<o:p></o:p></span></p><br/>";

        DataTable dt2 = new DAL().GetIrRroEmailInfo(irNumber);
        message.Body += "<table>";
        message.Body += "<tr><th style=\"border:solid 1px black;background-color:WhiteSmoke\">RRO Number</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Part Number</th><th style=\"width:100px;border:solid 1px black;background-color:WhiteSmoke\">Qty</th></tr>";
        foreach (DataRow row in dt2.Rows)
        {
            message.Body += "<tr><td style=\"border: 1px solid black\">" + row["IHC_IR_NBR"].ToString() + "-" + Utility.Pad(row["IHC_IR_RO_NBR"].ToString()) + "</td><td style=\"border: 1px solid black\">" + row["IHC_IR_IHC_PART_NBR"].ToString() + "</td><td style=\"border: 1px solid black\">" + row["IHC_IR_QTY"].ToString() + "<tr/>";
        }
        message.Body += "</table><br/>";
        message.Body += "<b>Repair/Rework Requirements:</b> " + dt.Rows[0]["IR_REWORK_REQ"].ToString() + "<br/>";
        SmtpClient client = new SmtpClient();
        client.Host = "ihc1.IHCI.INDHARNESS.COM";
        client.Send(message);

    }
  
}
