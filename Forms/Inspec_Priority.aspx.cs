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

public class Item
{
    private string _pn;
    private int _ohQty;
    private int _ohEmbQty;

    public string pn
    {
        get { return _pn; }
        set { _pn = value; }
    }
}
public partial class Inspec_Priority : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        IDataReader reader = IHC.EmbInspPriority.FetchAll();
        while (reader.Read())
        {
            string partNumber = reader.GetString(1);
            DateTime dte = reader.GetDateTime(3);
            //Response.Write("pn: " + partNumber + "<br/>");
            //Response.Write("date: " + dte.ToShortDateString());
            if (new DAL().GetITMR(partNumber, dte) > 0)
            {
                //Response.Write(partNumber + " Needs removed");
                //itmr done, delete inspector
                IHC.EmbInspPriority temp = IHC.EmbInspPriority.FetchByID(reader.GetInt32(2));
                IHC.EmbInspPriority.Delete(temp.InspPriId);
            }
        }
        if (!this.IsPostBack)
        {
            ////package allocator part
            List<string> items = new List<string>();

            DataTable dt2 = new DAL().GetAllocStuff();
            //Response.Write(dt2.Rows.Count.ToString());
            //get distinct parts in table
            foreach (DataRow row in dt2.Rows)
            {
                bool added = false;
                foreach (string i in items)
                {
                    if (i == row["ITEM"].ToString())
                    {
                        added = true;
                    }
                    else
                    {
                        added = false;
                    }
                }
                if (added == false)
                {
                    items.Add(row["ITEM"].ToString());
                }
            }

            foreach (string item in items)
            {
                int inspQty = Int32.Parse(new DAL().GetOhQty(item).ToString());
                int embQty = 0;

                if (new DAL().GetEmbQty(item).ToString() != "")
                {
                    embQty = Int32.Parse(new DAL().GetEmbQty(item));
                }
                inspQty = inspQty - embQty;
                //Response.Write("item: " + item + "<br/>insp qty: " + inspQty.ToString() + "<br/>emb qty: " + embQty.ToString());

                foreach (DataRow row in dt2.Rows)
                {
                    if (row["ITEM"].ToString() == item)
                    {
                        int pcsPer = 1;
                        int qtyDue = Int32.Parse(row["ORDER_QTY"].ToString());
                        if (row["PCS_PER"].ToString() != "")
                        {
                            pcsPer = Int32.Parse(row["PCS_PER"].ToString());
                        }

                        if (row["CUST_CLAS2"].ToString() == "E" && (embQty > qtyDue || embQty == qtyDue))
                        {
                            //if emb and emb qty available, use this first
                            if (row["ITEM"].ToString() == item)
                            {
                                if (embQty == qtyDue || embQty > qtyDue)
                                {
                                    //Response.Write("FOUND " + pcsPer.ToString());
                                    row["ALLOC"] = qtyDue.ToString();
                                    embQty -= qtyDue;
                                }
                                else
                                {
                                    if (embQty < qtyDue)
                                    {
                                        if (embQty > pcsPer || embQty == pcsPer)
                                        {
                                            int pcs = 0;
                                            while ((embQty > pcsPer || embQty == pcsPer) && pcsPer != 0)
                                            {
                                                pcs += pcsPer;
                                                embQty -= pcs;
                                            }
                                            row["ALLOC"] = pcs;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["ITEM"].ToString() == item)
                            {
                                if (inspQty == qtyDue || inspQty > qtyDue)
                                {
                                    row["ALLOC"] = qtyDue.ToString();
                                    inspQty -= qtyDue;
                                }
                                else
                                {
                                    if (inspQty < qtyDue)
                                    {
                                        if (inspQty > pcsPer || inspQty == pcsPer)
                                        {
                                            int pcs = 0;
                                            while ((inspQty > pcsPer || inspQty == pcsPer) && pcsPer != 0)
                                            {
                                                pcs += pcsPer;
                                                inspQty -= pcs;
                                            }
                                            row["ALLOC"] = pcs;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //INSPECTION QTY ALLOCATIONS
            foreach (DataRow row in dt2.Rows)
            {
                string pn = row["ITEM"].ToString();
                IHC.InspFlagController CTRL = new IHC.InspFlagController();
                IHC.InspFlagCollection COLL = CTRL.FetchByQuery(new SubSonic.Query("INSP_FLAG")
                    .WHERE("PART_NO", pn).AND("STATUS",0));
                if (COLL.Count > 0)
                {
                    row["ITEM_CLAS5"] = "Y";
                }
                int alloc = 0;
                int pcsPer = 1;
                int qtyDue = Int32.Parse(row["ORDER_QTY"].ToString());
                if (row["PCS_PER"].ToString() != "")
                {
                    pcsPer = Int32.Parse(row["PCS_PER"].ToString());
                }
                if (row["ALLOC"].ToString().Trim() != "")
                {
                    alloc = Int32.Parse(row["ALLOC"].ToString());
                }
                if (alloc == qtyDue)
                {
                    row.Delete();
                }
            }
            dt2.AcceptChanges();

            foreach (string item in items)
            {
                int inspQty = Int32.Parse(new DAL().GetInspQty(item).ToString());

                //Response.Write("item: " + item + "<br/>insp qty: " + inspQty.ToString() + "<br/>emb qty: " + embQty.ToString());

               foreach (DataRow row in dt2.Rows)
               {
                    if (row["ITEM"].ToString() == item)
                    {
                        int pcsPer = 1;
                        int qtyDue = Int32.Parse(row["ORDER_QTY"].ToString());
                        if (row["PCS_PER"].ToString() != "")
                        {
                            pcsPer = Int32.Parse(row["PCS_PER"].ToString());
                        }

                        if (row["ITEM"].ToString() == item)
                        {
                            if (inspQty == qtyDue || inspQty > qtyDue)
                            {
                                row["ALLOC"] = qtyDue.ToString();
                                inspQty -= qtyDue;
                            }
                            else
                            {
                                if (inspQty < qtyDue)
                                {
                                    if (row["PCS_PER"].ToString().Trim() != "")
                                    {
                                        if (inspQty > pcsPer || inspQty == pcsPer)
                                        {
                                            int pcs = 0;
                                            while ((inspQty > pcsPer || inspQty == pcsPer) && pcsPer != 0)
                                            {
                                                pcs += pcsPer;
                                                inspQty -= pcs;
                                            }
                                            row["ALLOC"] = pcs;
                                        }
                                    }
                                    else
                                    {
                                        if (inspQty > 0)
                                        {
                                            row["ALLOC"] = inspQty;
                                            inspQty = 0;
                                        }
                                        else
                                        {
                                            row["ALLOC"] = "";
                                            inspQty = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                dt2.AcceptChanges();

            }
            ////end package allocator part
            dt2.AcceptChanges();
            //dt2.DefaultView.Sort = "REQD_DATE,[ITEM]";

            //stuff for temp table for weird sorting query
            DataTable sorted = new DAL().TruncateAndCopyInspectionPriorityTable(dt2);
            //end stuff for temp table for weird sorting query

            this.GridView1.DataSource = sorted;// dt2.DefaultView.ToTable();
            this.GridView1.DataBind();
           


            //DataTable dt = new DAL().GetInspStuff();
            //this.GridView1.DataSource = dt;
            //this.GridView1.DataBind();

            foreach (GridViewRow r in this.GridView1.Rows)
            {
                //promise to ship 
                if (r.RowType == DataControlRowType.DataRow)
                {
                    string partNumber = r.Cells[7].Text;
                    //if (new DAL().PartialMo(partNumber))
                    //{
                    //    // r.BackColor = System.Drawing.Color.Red;
                    //}
                    DataTable dt3 = new DAL().PartialMo2(partNumber);
                    if (dt3.Rows.Count > 0)
                    {
                        string open = dt3.Rows[0]["OPEN_QTY"].ToString();
                        string order = dt3.Rows[0]["ORDER_QTY"].ToString();
                        if (Int32.Parse(open) < Int32.Parse(order))
                        {
                            r.Cells[18].Text = "Partial MO - " + open + " pieces still open.";
                        }
                    }
                    string moNumber = r.Cells[9].Text;
                    string coNumber = r.Cells[10].Text;

                    DataTable dt = new DAL().GetPromiseToShip(partNumber, coNumber);
                    //Response.Write("MO: " + moNumber);
                    //Response.Write("<br/>PN: " + partNumber);

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["HC_PROD_DTE"].ToString().Trim() != "")
                        {
                            r.Cells[1].Text = DateTime.Parse(dt.Rows[0][2].ToString()).ToShortDateString();
                            r.Cells[2].Text = dt.Rows[0][1].ToString();
                        }

                            if (dt.Rows[0]["HC_PROD_DTE2"].ToString().Trim() != "")
                            {
                                r.Cells[3].Text = DateTime.Parse(dt.Rows[0][4].ToString()).ToShortDateString();
                                r.Cells[4].Text = dt.Rows[0][3].ToString();
                            }

                            if (dt.Rows[0]["HC_PROD_DTE3"].ToString().Trim() != "")
                            {
                                r.Cells[5].Text = DateTime.Parse(dt.Rows[0][6].ToString()).ToShortDateString();
                                r.Cells[6].Text = dt.Rows[0][5].ToString();
                            }
                    }
                    //GREY OUT
                    //GREYED OUT FOR MORVS
                    int qty1 = 0, qty2 = 0, qty3 = 0;
                    bool one = false, two = false, three = false;
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["HC_PROD_QTY"] != null && dt.Rows[0]["HC_PROD_QTY"].ToString().Trim() != "")
                        {
                            Int32.TryParse(dt.Rows[0]["HC_PROD_QTY"].ToString().Trim(), out qty1);
                            one = true;
                        }
                        if (dt.Rows[0]["HC_PROD_QTY2"] != null && dt.Rows[0]["HC_PROD_QTY2"].ToString().Trim() != "")
                        {
                            Int32.TryParse(dt.Rows[0]["HC_PROD_QTY2"].ToString().Trim(), out qty2);
                            two = true;
                        }
                        if (dt.Rows[0]["HC_PROD_QTY3"] != null && dt.Rows[0]["HC_PROD_QTY3"].ToString().Trim() != "")
                        {
                            Int32.TryParse(dt.Rows[0]["HC_PROD_QTY3"].ToString().Trim(), out qty3);
                            three = true;
                        }
                    }
                    IHC.HotlistTblController ctrl = new IHC.HotlistTblController();
                    IHC.HotlistTblCollection col = ctrl.FetchByQuery(new SubSonic.Query("HOTLIST_TBL")
                        .WHERE("HC_CO_NO",coNumber).AND("HC_PART_NO",partNumber));

                    //generate list of parts for morv table to preload for faster loading
                    string partsForMorvs = "";
                    foreach (IHC.HotlistTbl hlpart in col)
                    {
                        partsForMorvs = partsForMorvs + "'" + hlpart.HcPartNo + "',";
                    }
                    partsForMorvs = partsForMorvs + "'xxxxxxxxx'";
                    new DAL().LoadMorvs(partsForMorvs);

                    if (col.Count > 0)
                    {
                        IHC.HotlistTbl item = col[0];

                        //morvs
                        int morvs = 0;
                       
                        if (Session["morvs" + item.HcMoNo] != null)
                        {
                            morvs = Int32.Parse(Session["morvs" + item.HcMoNo].ToString());
                        }
                        else
                        {
                            morvs = new DAL().GetMorvs2(item.HcPartNo, DateTime.Parse(item.HcSavedDte.ToString()), item.HcMoNo);
                        }

                        if (one)
                        {
                            if (qty1 < morvs || qty1 == morvs)
                            {
                                r.Cells[1].BackColor = System.Drawing.Color.LightGray;
                                r.Cells[2].BackColor = System.Drawing.Color.LightGray;
  
                                morvs = morvs - qty1;
                            }
                        }

                        if (two)
                        {
                            if (qty2 < morvs || qty2 == morvs)
                            {
                                r.Cells[3].BackColor = System.Drawing.Color.LightGray;
                                r.Cells[4].BackColor = System.Drawing.Color.LightGray;

                                morvs = morvs - qty2;
                            }
                        }

                        if (three)
                        {
                            if (qty3 < morvs || qty3 == morvs)
                            {
                                r.Cells[5].BackColor = System.Drawing.Color.LightGray;
                                r.Cells[6].BackColor = System.Drawing.Color.LightGray;

                                morvs = morvs - qty3;
                            }
                        }

                        Session["morvs" + item.HcMoNo] = morvs.ToString();
               
                        //morvs
                        //Response.Write("found");
                    }
                    //GREY OUT
                }

                if (r.RowType == DataControlRowType.DataRow && r.RowIndex > 0 && r.RowIndex < GridView1.Rows.Count)
                {
                    //Response.Write("row: " + rowText);
                    //Response.Write("<br/>rowabove: " + rowTextAbove);

                    foreach (GridViewRow rw in GridView1.Rows)
                    {
                        if (rw.RowIndex < r.RowIndex)
                        {
                            string rowText = r.Cells[7].Text;
                            string rowTextAbove = rw.Cells[7].Text;

                            if (rowText == rowTextAbove)
                            {
                                //already done
                                r.Cells[7].Text = "";
                                //r.Cells[1].Text = "";
                                //r.Cells[2].Text = "";
                                r.Cells[8].Text = "";
                                //r.Cells[5].Text = "";
                                r.Cells[0].Controls.Clear();
                                r.BackColor = System.Drawing.Color.WhiteSmoke;
                            }
                        }
                    }
                 
                }
                if (r.RowType == DataControlRowType.DataRow && r.RowIndex > 0 && r.RowIndex < GridView1.Rows.Count)
                {
                    string rowText = r.Cells[9].Text;
                    string rowTextAbove = GridView1.Rows[r.RowIndex - 1].Cells[9].Text;

                    //Response.Write("row: " + rowText);
                    //Response.Write("<br/>rowabove: " + rowTextAbove);

                    if (rowText == rowTextAbove)
                    {
                        //already done
                        //r.Cells[5].Text = "";
                        //r.Cells[6].Text = "";
                        //r.Cells[4].Text = "";
                    }
                }
            }
        }
        if (this.IsPostBack)
        {
            foreach (GridViewRow r in this.GridView1.Rows)
            {
                if (((DropDownList)r.Cells[0].FindControl("DropDownList1")).SelectedIndex == 0 && r.Cells[7].Text.Trim() == "")
                {
                    r.Cells[0].Controls.Clear();
                }
            }
        }
        //Response.Write(this.GridView1.Rows[2].Cells[3].Text);
        foreach (GridViewRow rxp in this.GridView1.Rows)
        {
            if ((DropDownList)rxp.Cells[0].FindControl("DropDownList1") != null)
            {
                if (new DAL().GetInspector(rxp.Cells[7].Text) != "")
                {
                    //Response.Write("found row " + r.RowIndex.ToString());
                    DropDownList dl = (DropDownList)rxp.Cells[0].FindControl("DropDownList1");
                    dl.SelectedValue = new DAL().GetInspector(rxp.Cells[7].Text);
                    dl.Enabled = false;
                }
            }
        }
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
    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        //new DAL().TruncateEmbInspection();
        ////GET INSPECTOR NAME & PART NUMBER
        //foreach (GridViewRow r in this.GridView1.Rows)
        //{
        //    if (r.Cells[0].FindControl("DropDownList1") != null)
        //    {
        //        DropDownList dl = (DropDownList)r.Cells[0].FindControl("DropDownList1");
        //        string inspector = dl.SelectedValue;
        //        string partNumber = r.Cells[3].Text;
        //        IHC.EmbInspPriority ip = new IHC.EmbInspPriority();
        //        ip.InspName = inspector;
        //        ip.PartNo = partNumber;
        //        ip.Save();
        //    }

        //}
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells[20].Text.Trim() == "Y")
        {
            e.Row.BackColor = System.Drawing.Color.Red;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[14].BackColor = System.Drawing.Color.Yellow;
            e.Row.Cells[15].BackColor = System.Drawing.Color.Yellow;
            e.Row.Cells[16].BackColor = System.Drawing.Color.Yellow;
 
        }

        e.Row.Cells[14].Attributes.Add("onmouseover", e.Row.Cells[14].ToolTip = "Pkg Type");
        e.Row.Cells[15].Attributes.Add("onmouseover", e.Row.Cells[15].ToolTip = "Pcs Per Pkg");
        e.Row.Cells[16].Attributes.Add("onmouseover", e.Row.Cells[16].ToolTip = "Insp Pkg Qty");
        e.Row.Cells[17].Attributes.Add("onmouseover", e.Row.Cells[17].ToolTip = "VM DF");
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell oTableCell = new TableCell();

            //Add Department
            oTableCell.Text = "";
            oTableCell.ColumnSpan = 1;
            oGridViewRow.Cells.Add(oTableCell);

            //Add Employee            oTableCell = new TableCell();
            oTableCell.Text = "Promise to Ship";
            oTableCell.ColumnSpan = 6;
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableCell();
            oTableCell.Text = "";
            oTableCell.ColumnSpan = 2;
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableCell();
            oTableCell.Text = "Unfulfilled Customer Demand";
            oTableCell.ColumnSpan = 5;
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableCell();
            oTableCell.Text = "";
            oTableCell.ColumnSpan = 1;
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableCell();
            oTableCell.Text = "Package Allocator";
            oTableCell.ColumnSpan = 3;
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableCell();
            oTableCell.Text = "";
            oTableCell.ColumnSpan = 4;
            oGridViewRow.Cells.Add(oTableCell);
            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //new DAL().TruncateEmbInspection();
        //GET INSPECTOR NAME & PART NUMBER
        foreach (GridViewRow r in this.GridView1.Rows)
        {
            if (r.Cells[0].FindControl("DropDownList1") != null)
            {
                DropDownList dl = (DropDownList)r.Cells[0].FindControl("DropDownList1");
               
                    string inspector = dl.SelectedValue;
                    string partNumber = r.Cells[7].Text;
                    IHC.EmbInspPriority ip = new IHC.EmbInspPriority();
                    ip.InspName = inspector;
                    ip.PartNo = partNumber;
                    ip.InspDate = DateTime.Now;
                    if (dl.SelectedValue != "select" && dl.SelectedValue != "(select inspector)")
                    {
                        if (new DAL().GetInspector(partNumber) == "")
                        {
                            ip.Save();
                        }
                    }
            }
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
    }
}