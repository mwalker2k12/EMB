using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Data.Odbc;

/// <summary>
/// Summary description for DAL
/// </summary>
public class DAL
{
    public DAL()
    {

    }
    public DataTable TruncateAndCopyInspectionPriorityTable(DataTable inputTable)
    {
        DataTable retVal = new DataTable();

        SqlConnection conn = GetConnection();
        try
        {
            string truncateSql = @"TRUNCATE TABLE INSPECTION_PRIORITY_TABLE";
            SqlCommand truncateCommand = GetCommand(conn, truncateSql);
            conn.Open();
            truncateCommand.ExecuteNonQuery();

            //copy input table to temp inspection table
            SqlBulkCopy copy = new SqlBulkCopy(conn);
            copy.DestinationTableName = "INSPECTION_PRIORITY_TABLE";
            copy.WriteToServer(inputTable);

            //get sorted table to return
            string selectSql = @"
                    SELECT ITEM, MIN(REQD_DATE) AS DATE#1 INTO #TEMP_INSPECTION
                    FROM INSPECTION_PRIORITY_TABLE
                    GROUP BY ITEM
                    ORDER BY ITEM, MIN(REQD_DATE)

                    SELECT DISTINCT * 
                    FROM #TEMP_INSPECTION LEFT JOIN INSPECTION_PRIORITY_TABLE  ON #TEMP_INSPECTION.ITEM = INSPECTION_PRIORITY_TABLE.ITEM
                    ORDER BY #TEMP_INSPECTION.DATE#1, #TEMP_INSPECTION.ITEM, INSPECTION_PRIORITY_TABLE.REQD_DATE

                    DROP TABLE #TEMP_INSPECTION";
            SqlCommand selectCommand = GetCommand(conn, selectSql);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = selectCommand;
            da.Fill(retVal);
        }
        catch (Exception ex)
        {
        }
        finally
        {
            conn.Close();
        }
        return retVal;
    }
    public void LoadMorvs(string partNumbers)
    {

        //ihc2.Service ihc2 = new ihc2.Service();
        if (HttpContext.Current.Session["morvTable"] == null)
        {
            DataTable dt = ExecuteSqlAgainstFourthShift(@"SELECT ITEM,[DATE] + [TIME] AS [DATE],BODY1 FROM ITEM_HISTORY
                        WHERE ITEM IN (" + partNumbers + ") AND TRANS_ID = 'MORV'");

            HttpContext.Current.Session["morvTable"] = dt;
        }

    }
    public void LoadImtr(string partNumbers)
    {

        //ihc2.Service ihc2 = new ihc2.Service();
        if (HttpContext.Current.Session["imtrTable"] == null)
        {
            DataTable dt = ExecuteSqlAgainstFourthShift(@"SELECT ITEM,COUNT(*) FROM ITEM_HISTORY
                        WHERE ITEM IN (" + partNumbers + ") AND TRANS_ID = 'IMTR'");

            HttpContext.Current.Session["imtrTable"] = dt;
        }

    }
    public int GetMorvs2(string partNumber, DateTime date, string moNo)
    {
        int retVal = 0;

        //ihc2.Service ihc2 = new ihc2.Service();
//        DataTable dt = ExecuteSqlAgainstFourthShift(@"SELECT BODY1 FROM ITEM_HISTORY
//                        WHERE ITEM = '" + partNumber + "'"
//                        + " AND TRANS_ID = 'MORV' "
//                        + " AND ([DATE] + TIME) > '" + date.ToShortDateString() + " " + date.ToLongTimeString() + "'");
       DataTable dt = (DataTable)HttpContext.Current.Session["morvTable"];
       DataRow[] rows = dt.Select("ITEM = '" + partNumber + "' AND [DATE] > '" + date.ToShortDateString() + " " + date.ToLongTimeString() + "'");
        string[] body = null;
        int morvs = 0;
        int neg_morvs = 0;

        foreach (DataRow row in rows)
        {
            body = row[0].ToString().Split('~');

            //iterate through string items

            int i = 0;
            string mo = "";
            string type = "";

            foreach (string item in body)
            {
                if (i == 2)
                {
                    mo = item;
                }
                if (i == 4 && mo == moNo)
                {
                    type = item;
                }
                if (i == 5 && mo == moNo)
                {
                    if (type == "R" || type == "E")
                    {
                        morvs += Int32.Parse(item);
                    }
                    if (type == "X" || type == "Y")
                    {
                        neg_morvs += Int32.Parse(item);
                    }
                }
                ++i;
            }
        }
        return morvs - neg_morvs;

    }
    public bool UseNewFourthShift()
    {
        bool newFS = false;

        if (WebConfigurationManager.AppSettings["UseNewFourthShift"].ToString() == "true")
        {
            newFS = true;
        }
        else 
        {
            newFS = false; 
        }
        return newFS;
    }
    public void LoadEmbQtys(string partNumbers)
    {
        //ihc2.Service ihc2 = new ihc2.Service();
        if (HttpContext.Current.Session["embqtyTable"] == null)
        {
            OdbcConnection conn = new OdbcConnection("DSN=FAC10;DATABASE=inven;SERVER=fac10;PORT=5432;SSLMODE=prefer;TABLE=public.cross_ref");
            OdbcCommand cmd = new OdbcCommand();
            cmd.CommandText = @"SELECT SUM(CURRQUAN),PARTNUM
                FROM SERTRK
                WHERE PARTNUM IN (" + partNumbers + ")"
                + " AND LOCATION IN ('4N27','4Q16','4Q17','4Q21','4Q22','4Q11','4Q12')";
            cmd.Connection = conn;
            OdbcDataAdapter da = new OdbcDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            HttpContext.Current.Session["embqtyTable"] = dt;
        }
    }
    public string GetEmbQty(string partno)
    {
        OdbcConnection conn = new OdbcConnection("DSN=FAC10;DATABASE=inven;SERVER=fac10;PORT=5432;SSLMODE=prefer;TABLE=public.cross_ref");
        string part = "";

        OdbcCommand cmd = new OdbcCommand();
        cmd.CommandText = @"SELECT SUM(CURRQUAN) 
                FROM SERTRK
                WHERE PARTNUM = '" + partno + "'"
                + " AND LOCATION IN ('4N27','4Q16','4Q17','4Q21','4Q22','4Q11','4Q12')";
        cmd.Connection = conn;

        try
        {
            conn.Open();

            part = cmd.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conn.Close();
        }
        return part;
    }
    public DataTable ExecuteSqlAgainstFourthShift(string sql)
    {
        DataTable dt = null;
        SqlCommand cmd = null;
        try
        {
            if (UseNewFourthShift() == true)
            {
                cmd = GetCommand(GetNewFourthShiftConnection(), sql);
                dt = new DataTable();
                //if (HttpContext.Current.Cache[sql] != null)
                //{
                //    dt = (DataTable)HttpContext.Current.Cache[sql];
                //}
                //else
                //{
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    cmd.CommandTimeout = 1000000;
                    cmd.Connection.Open();
                    da.Fill(dt);
                    //HttpContext.Current.Cache[sql] = dt;
                //}
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (cmd != null)
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
        }
        return dt;
    }
    public OdbcConnection GetFsConnection()
    {
        return new OdbcConnection(WebConfigurationManager.ConnectionStrings["FSConnectionString"].ConnectionString);
    }
    public SqlConnection GetNewFourthShiftConnection()
    {
        return new SqlConnection(WebConfigurationManager.ConnectionStrings["NewFSConnectionString"].ConnectionString);
    }
    public OdbcCommand GetFsCommand(OdbcConnection conn, string sqlStatement)
    {
        return new OdbcCommand(sqlStatement, conn);
    }
    public SqlConnection GetConnection()
    {
        return new SqlConnection(WebConfigurationManager.ConnectionStrings["IHCConnectionString"].ConnectionString);
    }
    public SqlCommand GetCommand(SqlConnection conn, string sqlStatement)
    {
        return new SqlCommand(sqlStatement, conn);
    }
    public decimal GetPrice(string partNumber)
    {
        decimal retVal = 0.00M;

        //ihc2.Service ihc2 = new ihc2.Service();
        DataTable dt = ExecuteSqlAgainstFourthShift(@"Select
                                                          MATL
                                                        From
                                                          ITEM_ITEMCOST
                                                        Where
                                                          ITEM = '" + partNumber + "' And COST_TYPE = '2'");
        if (dt.Rows.Count > 0)
        {
           Decimal.TryParse(dt.Rows[0][0].ToString(), out retVal);
        }
        return retVal;
    }
    public int GetInspQty(string partNumber)
    {
        int retVal = 0;

        //ihc2.Service ihc2 = new ihc2.Service();
        DataTable dt = ExecuteSqlAgainstFourthShift(@"SELECT
                                                    INSP_QTY
                                                    FROM ITEM_ITEMLIST
                                                    WHERE ITEM = '" + partNumber + "'");
        if (dt.Rows.Count > 0)
        {
            Int32.TryParse(dt.Rows[0][0].ToString(), out retVal);
        }
        return retVal;
    }
    public int GetITMR(string partNumber,DateTime dte)
    {
        int retVal = 0;
        try
        {

            //ihc2.Service ihc2 = new ihc2.Service();
            DataTable dt = ExecuteSqlAgainstFourthShift(@"
                SELECT COUNT(*) FROM ITEM_HISTORY
                WHERE ITEM = '" + partNumber + "'"
                + " AND TRANS_ID = 'IMTR' "
                + " AND ([DATE] + TIME) > '" + dte.ToShortDateString() + " " + dte.ToLongTimeString() + "'");
            if (dt.Rows.Count > 0)
            {
                Int32.TryParse(dt.Rows[0][0].ToString(), out retVal);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return retVal;
    }
    public DataTable GetInspStuff()
    {
        DataTable dt = ExecuteSqlAgainstFourthShift(@"SELECT DISTINCT
                MO_NUMBER,ORDER_MANUFACTDETAIL.ORDER_QTY,ITEM_ITEMLIST.ITEM,ITEM_ITEMLIST.INSP_QTY
                ,CUST_ID,REQD_DATE,'C/O' AS 'DEMANDTYPE'
                FROM ORDER_MANUFACTDETAIL,ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL
                WHERE 
                ORDER_MANUFACTDETAIL.ITEM = ITEM_ITEMLIST.ITEM
                AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
                AND ORDER_MANUFACTDETAIL.LN_STA = '4'  
                AND ITEM_ITEMLIST.INSP_QTY > 0
                ORDER  
                BY ITEM_ITEMLIST.ITEM,MO_NUMBER");
        
        return dt;
    }
    public bool YStatus(string pn)
    {
        bool retval = false;
        DataTable dt = ExecuteSqlAgainstFourthShift(@"SELECT ITEM_CLAS5,ITEM
            FROM ITEM_INFO 
            WHERE ITEM = '" + pn + "'");

        string y = dt.Rows[0][0].ToString();
        if (y.Trim().ToUpper() == "Y")
        {
            retval = true;
        }
        return retval;
    }
    public DataTable GetAllocStuff()
    {

        DataTable dt = ExecuteSqlAgainstFourthShift(@"INSERT INTO TEMP_INSP_TEST
	
						SELECT DISTINCT
                        ITEM_ITEMLIST.ITEM,ITEM_ITEMLIST.INSP_QTY
                        ,ORDER_CUSTOMERDETAIL.CUST_ID,REQD_DATE,'C/O' AS 'DEMANDTYPE',ORDER_CUSTOMERDETAIL.ORDER_QTY - ISSUED_QTY AS 'ORDER_QTY',ITEM_ITEMLIST.ITEM_REF1
						,CASE
                        WHEN CUST_CLAS2 <> '' THEN PKG_TYPE
                        ELSE 'CTN'
                        END AS PKG_TYPE
                        ,CASE
                        WHEN CUST_CLAS2 <> '' THEN PCS_PER
                        ELSE NULL
                        END AS PCS_PER,CO_NUMBER
                        ,ISNULL(CUST_CLAS2,'') AS 'CUST_CLAS2',' ' AS 'ALLOC'
                        ,CASE WHEN STK_ROOM <> 'FI' THEN 'Stock Item' END AS STK_ROOM, ITEM_INFO.ITEM_CLAS5 ,ORDER_CUSTOMERDETAIL.CO_LN_NO
                        FROM ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL,ITEM_INFO,CUSTOMER_DATA
                        WHERE  ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
                        AND ORDER_CUSTOMERDETAIL.CUST_ID = CUSTOMER_DATA.CUST_ID
                        AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
                        AND ITEM_ITEMLIST.INSP_QTY > 0
                        AND ORDER_CUSTOMERDETAIL.LN_STA = '4'
                        --irs forecast
                        UNION

                        SELECT ITEM_NO AS 'ITEM',ITEM_ITEMLIST.INSP_QTY,'IRS' AS 'CUST_ID',DTE AS 'REQD_DATE','FCST' AS 'DEMANDTYPE',QTY AS 'ORDER_QTY'
                        ,'' AS 'ITEM_REF1'
                        ,PKG_TYPE
                        ,CASE
                        WHEN CUST_CLAS2 <> '' THEN PCS_PER
                        ELSE NULL
                        END AS PCS_PER,'' AS 'CO_NUMBER'
                        ,ISNULL(CUST_CLAS2,'') AS 'CUST_CLAS2','' AS 'ALLOC'
	                    ,CASE WHEN STK_ROOM <> 'FI' THEN 'Stock Item' END AS STK_ROOM, ITEM_INFO.ITEM_CLAS5, '' AS CO_LN_NO
                        FROM IHC_IRS_FORECAST,ITEM_INFO,CUSTOMER_DATA,ITEM_ITEMLIST
						
                        WHERE ITEM_NO = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ITEM_NO
                        AND CUSTOMER_DATA.CUST_ID = 'IRS'
                        AND ITEM_NO IN 
                        (SELECT DISTINCT
                        ITEM_ITEMLIST.ITEM
                        FROM ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL,ITEM_INFO,CUSTOMER_DATA
                        WHERE  ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
                        AND ITEM_ITEMLIST.INSP_QTY > 0
                        AND ORDER_CUSTOMERDETAIL.LN_STA = '4')

                        --grove forecast
                        UNION

                        SELECT ITEM_NO AS 'ITEM',ITEM_ITEMLIST.INSP_QTY,'GRO' AS 'CUST_ID',DTE AS 'REQD_DATE','FCST' AS 'DEMANDTYPE',QTY AS 'ORDER_QTY'
                        ,'' AS 'ITEM_REF1'
                        ,PKG_TYPE
                        ,CASE
                        WHEN CUST_CLAS2 <> '' THEN PCS_PER
                        ELSE NULL
                        END AS PCS_PER,'' AS 'CO_NUMBER'
                        ,ISNULL(CUST_CLAS2,'') AS 'CUST_CLAS2','' AS 'ALLOC'
                        ,CASE WHEN STK_ROOM <> 'FI' THEN 'Stock Item' END AS STK_ROOM, ITEM_INFO.ITEM_CLAS5, '' AS CO_LN_NO
                        FROM IHC_GRO_FORECAST,ITEM_INFO,CUSTOMER_DATA,ITEM_ITEMLIST

                        WHERE ITEM_NO = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ITEM_NO
                        AND CUSTOMER_DATA.CUST_ID = 'GRO'
                        AND ITEM_NO IN 
                        (SELECT DISTINCT
                        ITEM_ITEMLIST.ITEM
                        FROM ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL,ITEM_INFO
                        WHERE  ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
                        AND ITEM_ITEMLIST.INSP_QTY > 0
                        AND ORDER_CUSTOMERDETAIL.LN_STA = '4')

                        --GENERIC FORECAST
                        UNION

                        SELECT ITEM_NO AS 'ITEM',ITEM_ITEMLIST.INSP_QTY,'GEN' AS 'CUST_ID',DTE AS 'REQD_DATE','FCST' AS 'DEMANDTYPE',QTY AS 'ORDER_QTY'
                        ,'' AS 'ITEM_REF1'
                        ,PKG_TYPE,
                        CASE
                        WHEN CUST_CLAS2 <> '' THEN PCS_PER
                        ELSE NULL
                        END AS PCS_PER,'' AS 'CO_NUMBER'
                        ,ISNULL(CUST_CLAS2,'') AS 'CUST_CLAS2','' AS 'ALLOC'
	                    ,CASE WHEN STK_ROOM <> 'FI' THEN 'Stock Item' END AS STK_ROOM, ITEM_INFO.ITEM_CLAS5, '' AS CO_LN_NO
                        FROM IHC_GEN_FORECAST,ITEM_INFO,CUSTOMER_DATA,ITEM_ITEMLIST

                        WHERE ITEM_NO = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ITEM_NO
                        AND ITEM_NO IN 
                        (SELECT DISTINCT
                        ITEM_ITEMLIST.ITEM
                        FROM ITEM_ITEMLIST,ORDER_CUSTOMERDETAIL,ITEM_INFO
                        WHERE  ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
                        AND ITEM_ITEMLIST.ITEM = ORDER_CUSTOMERDETAIL.ITEM
                        AND ITEM_ITEMLIST.INSP_QTY > 0
                        AND ORDER_CUSTOMERDETAIL.LN_STA = '4')

                        UNION
                        --PLANNED
                        SELECT DISTINCT ITEM_DEMANDS.ITEM AS 'ITEM'
                        ,ITEM_ITEMLIST.INSP_QTY
                        ,'' AS 'CUST_ID'
                        ,ITEM_DEMANDS.REQD_DATE AS 'REQD_DATE'
                        ,'Planned Rqmt' AS 'DEMANDTYPE'
                        ,ORDER_QTY - ISSUED_QTY AS 'ORDER_QTY'
                        ,'' AS 'ITEM_REF1'
                        ,PKG_TYPE AS 'PKG_TYPE'
                        ,PCS_PER AS 'PCS_PER','' AS 'CO_NUMBER'
                        ,'' AS 'CUST_CLAS2','' AS 'ALLOC'
			            ,CASE WHEN STK_ROOM <> 'FI' THEN 'Stock Item' END AS STK_ROOM, ITEM_INFO.ITEM_CLAS5, '' AS CO_LN_NO
                        FROM ITEM_DEMANDS,ITEM_ITEMLIST,ITEM_INFO
        
                        WHERE ITEM_DEMANDS.ITEM = ITEM_ITEMLIST.ITEM
                        AND ITEM_ITEMLIST.ITEM = ITEM_INFO.ITEM
                        AND COMP_STA IN (4,1)
                        AND INSP_QTY > 0
                        AND (ORDER_QTY - ISSUED_QTY) > 0
                        AND ITEM_ITEMLIST.MB = 'M'
                        AND ITEM_DEMANDS.ITEM NOT IN (SELECT ITEM FROM ORDER_CUSTOMERDETAIL WHERE LN_STA = '4')

SELECT ITEM, MIN(REQD_DATE) AS DATE#1 INTO #TEMP_INSPECTION
FROM TEMP_INSP_TEST
GROUP BY ITEM
ORDER BY ITEM, MIN(REQD_DATE)

TRUNCATE TABLE TEMP_INSP2_TEST
DECLARE @ITEM VARCHAR(50)
DECLARE EACH_ITEM CURSOR FOR
	SELECT DISTINCT ITEM FROM TEMP_INSP_TEST

OPEN EACH_ITEM

FETCH NEXT FROM EACH_ITEM INTO @ITEM
	INSERT INTO TEMP_INSP2_TEST
		    SELECT TOP 4 * FROM TEMP_INSP_TEST
			WHERE LTRIM(RTRIM(ITEM)) = LTRIM(RTRIM(@ITEM))
			ORDER BY ITEM, REQD_DATE
	WHILE (@@fetch_status <> -1)
	BEGIN
	FETCH NEXT FROM EACH_ITEM INTO @ITEM

			INSERT INTO TEMP_INSP2_TEST
		    SELECT TOP 4 * FROM TEMP_INSP_TEST
			WHERE LTRIM(RTRIM(ITEM)) = LTRIM(RTRIM(@ITEM))
			ORDER BY ITEM, REQD_DATE
	END
CLOSE EACH_ITEM
DEALLOCATE EACH_ITEM

SELECT 
#TEMP_INSPECTION.ITEM,
TEMP_INSP2_TEST.INSP_QTY,
TEMP_INSP2_TEST.CUST_ID,
TEMP_INSP2_TEST.REQD_DATE,
TEMP_INSP2_TEST.DEMANDTYPE,
TEMP_INSP2_TEST.ORDER_QTY,
TEMP_INSP2_TEST.PKG_TYPE,
TEMP_INSP2_TEST.ITEM_REF1,
TEMP_INSP2_TEST.PCS_PER,
TEMP_INSP2_TEST.CO_NUMBER,
TEMP_INSP2_TEST.CUST_CLAS2,
TEMP_INSP2_TEST.ALLOC, 
TEMP_INSP2_TEST.STK_ROOM,
TEMP_INSP2_TEST.ITEM_CLAS5,
TEMP_INSP2_TEST.CO_LN_NO,
#TEMP_INSPECTION.DATE#1
FROM #TEMP_INSPECTION LEFT JOIN TEMP_INSP2_TEST ON #TEMP_INSPECTION.ITEM = TEMP_INSP2_TEST.ITEM
ORDER BY #TEMP_INSPECTION.DATE#1, #TEMP_INSPECTION.ITEM, TEMP_INSP2_TEST.REQD_DATE

DROP TABLE #TEMP_INSPECTION
TRUNCATE TABLE TEMP_INSP_TEST
");

        return dt;
    }
    public void TruncateEmbShipments()
    {
        SqlCommand cmd = GetCommand(GetConnection(), @"TRUNCATE TABLE EMB_SHIPMENT2");

        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
      
    }
    public void TruncateEmbInspection()
    {
        SqlCommand cmd = GetCommand(GetConnection(), @"TRUNCATE TABLE EMB_INSP_PRIORITY");

        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }

    }
    public string GetInspector(string partNumber)
    {
        string inspector = "";
        SqlCommand cmd = GetCommand(GetConnection(), @"SELECT INSP_NAME FROM
                                                        EMB_INSP_PRIORITY
                                                        WHERE PART_NO = '" + partNumber + "'");
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            inspector = cmd.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            inspector = "";
        }
        finally
        {
            cmd.Connection.Close();
        }
        return inspector;
    }
    public DataTable RaLookup(string partNumber, string customerCode)
    {
        SqlCommand cmd = GetCommand(GetConnection(), @"SELECT" +
@"        RA_RA_NBR AS [RA #]," +
@"        RA_CUST_PART_NBR AS [Part #]," +
@"        RA_QTY AS [Quantity]" +
@"        FROM RA_LOG WHERE RA_CUST_PART_NBR LIKE @IHC_PART_NBR");

        if (customerCode != "select")
        {
            cmd.CommandText += " AND RA_CUST_CDE = @RA_CUST_CDE";
            cmd.Parameters.AddWithValue("@RA_CUST_CDE", customerCode);
        }

            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@IHC_PART_NBR", partNumber);
                cmd.Parameters["@IHC_PART_NBR"].Value += "%";
                cmd.Connection.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
}
    public DataTable RroLookup(string partNumber, string customerCode)
    {
        SqlCommand cmd = GetCommand(GetConnection(), @" SELECT DISTINCT " +
                                                    @" RR_QTY," +
                                                    @" RR_IHC_PART_NBR," +
                                                    @" RR_REC_DTE," +
                                                    @" RR_RA_NBR + '-' + REPLICATE('0',2 - LEN(RTRIM(LTRIM(STR(RR_RO_NBR))))) + " +
                                                    @" RTRIM(LTRIM(STR(RR_RO_NBR))) AS RRO" +
                                                    @" FROM RRO_DATA,RA_LOG" +
                                                    @" WHERE RR_IHC_PART_NBR LIKE @IHC_PART_NBR");
        if (customerCode != "select")
        {
            cmd.CommandText += " AND RR_RA_NBR IN (SELECT RA_RA_NBR FROM RA_LOG WHERE RA_CUST_CDE = @RA_CUST_CDE)";
            cmd.Parameters.AddWithValue("@RA_CUST_CDE", customerCode);
        }

        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Parameters.AddWithValue("@IHC_PART_NBR", partNumber);
            cmd.Parameters["@IHC_PART_NBR"].Value += "%";
            cmd.Connection.Open();
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }
    public DataTable InternalRroLookup(string partNumber, string customerCode)
    {
        SqlCommand cmd = GetCommand(GetConnection(), @" SELECT DISTINCT " +
                                                    @" IHC_IR_QTY," +
                                                    @" IHC_IR_IHC_PART_NBR," +
                                                    @" IHC_IR_NBR + '-' + REPLICATE('0',2 - LEN(RTRIM(LTRIM(STR(IHC_IR_RO_NBR))))) + " +
                                                    @" RTRIM(LTRIM(STR(IHC_IR_RO_NBR))) AS RRO" +
                                                    @" FROM IHC_RRO_DATA,IR_LOG" +
                                                    @" WHERE IHC_IR_IHC_PART_NBR LIKE @IHC_PART_NBR");
        if (customerCode != "select")
        {
            cmd.CommandText += " AND IHC_IR_NBR IN (SELECT IR_IR_NBR FROM IR_LOG WHERE IR_CUST_CDE = @IR_CUST_CDE)";
            cmd.Parameters.AddWithValue("@IR_CUST_CDE", customerCode);
        }

        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Parameters.AddWithValue("@IHC_PART_NBR", partNumber);
            cmd.Parameters["@IHC_PART_NBR"].Value += "%";
            cmd.Connection.Open();
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }
    public string[] GetFsPartNumbers(string prefixText)
    {
        //SqlCommand cmd = GetCommand(GetConnection(), "SELECT   '''' + ITEM + '''' AS ITEM FROM ITEM_ITEMLIST WHERE ITEM LIKE '" + prefixText + "%'");
        List<string> items = new List<string>();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            DataTable dt = ExecuteSqlAgainstFourthShift("SELECT   '''' + ITEM + '''' AS ITEM FROM ITEM_ITEMLIST WHERE ITEM LIKE '" + prefixText + "%'");
            foreach (DataRow row in dt.Rows)
            {
                if (items.Count < 10)
                {
                    items.Add(row[0].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {  
        }
        return (string[])items.ToArray();
    }
    public string[] GetFsLotNumbers(string prefixText)
    {
        List<string> items = new List<string>();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            DataTable dt = ExecuteSqlAgainstFourthShift("SELECT MO_NUMBER FROM ORDER_MANUFACTDETAIL WHERE MO_NUMBER LIKE '" + prefixText + "%' AND ITEM IS NOT NULL");
            foreach (DataRow row in dt.Rows)
            {
                if (items.Count < 10)
                {
                    items.Add(row[0].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return (string[])items.ToArray();
    }
    public DataTable GetFsCustomers()
    {
        DataTable retVal = new DataTable();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift("SELECT CUST_ID, CUST_NAME FROM CUSTOMER_DATA ORDER BY CUST_NAME");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return retVal;
    }
    public DataTable GetFsManufactDetailByLotNo(string lotNo)
    {
        DataTable retVal = new DataTable();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift(@"SELECT * FROM ORDER_MANUFACTDETAIL
                                                    WHERE MO_NUMBER = '" + lotNo + "'");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return retVal;
    }
    public decimal GetOhQty(string partNo)
    {
        DataTable retVal = new DataTable();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift(@"SELECT OHND_QTY FROM ITEM_ITEMLIST WHERE ITEM = '" + partNo + "'");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        string qty = retVal.Rows[0][0].ToString();
        return Decimal.Parse(qty);
    }
    public bool PartialMo(string item)
    {
        bool partial = false;

        DataTable retVal = new DataTable();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift(@"SELECT 
                            SUM(ORDER_QTY - QTY_RCVD) AS 'OPEN_QTY'
                            ,SUM(ORDER_QTY) AS ORDER_QTY
                            FROM ORDER_MANUFACTDETAIL
                            WHERE 
                            LN_STA = '4'
                            and
                            ITEM = '" + item + "'");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        if (retVal.Rows.Count > 0)
        {
            string open = Utility.GetIntFromString(retVal.Rows[0]["OPEN_QTY"].ToString()).ToString();
            string order = Utility.GetIntFromString(retVal.Rows[0]["ORDER_QTY"].ToString()).ToString();
            if (Int32.Parse(open) < Int32.Parse(order))
            {
                partial = true;
            }
        }
        return partial;
    }
    public DataTable PartialMo2(string item)
    {
        DataTable partial = new DataTable();

        DataTable retVal = new DataTable();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift(@"SELECT 
                            ORDER_QTY - QTY_RCVD AS 'OPEN_QTY'
                            ,ORDER_QTY AS ORDER_QTY
                            FROM ORDER_MANUFACTDETAIL
                            WHERE 
							QTY_RCVD < ORDER_QTY
							AND
                            LN_STA = '4'
                            and
                            ITEM = '" + item + "'");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        if (retVal.Rows.Count > 0)
        {
            partial = retVal;
        }
        return partial;
    }
    public DataTable GetPromiseToShip(string partNo, string coNumber)
    {
        DataTable retVal = new DataTable();
        SqlCommand cmd = new SqlCommand();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            cmd = GetCommand(GetConnection(), @"SELECT
                            HC_PART_NO
                            ,HC_PROD_QTY
                            ,HC_PROD_DTE
                            ,HC_PROD_QTY2
                            ,HC_PROD_DTE2
                            ,HC_PROD_QTY3
                            ,HC_PROD_DTE3
                            ,HC_CO_NO
                            FROM HOTLIST_TBL
                            WHERE HC_PART_NO = '" + partNo + "'"
                            + " AND HC_CO_NO = '" + coNumber + "'");

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            cmd.Connection.Open();
            da.Fill(retVal);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return retVal;
    }
    public DataTable GetEmbJunkShipments()
    {
        DataTable retVal = new DataTable();
        SqlCommand cmd = new SqlCommand() ;
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            cmd = GetCommand(GetConnection(), @"SELECT
            EMB_SHIP_ID
            ,EMB_ITEM 
            ,EMB_SHIP_QTY 
            ,EMB_PCS_PER 
            ,EMB_PKG_TYPE 
            ,EMB_BIN_QTY 
            FROM EMB_SHIPMENTS
            WHERE EMB_DATE = GETDATE()
            AND CAST(EMB_SHIP_QTY AS INT) % CAST(EMB_PCS_PER AS INT) <> 0 
            AND CAST(EMB_SHIP_QTY AS INT) % CAST(EMB_PCS_PER AS INT) > 0
            AND EMB_PCS_PER < EMB_SHIP_QTY
            ");

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
        
            cmd.Connection.Open();
            da.Fill(retVal);
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return retVal;
    }
    public DataTable GetEmbShipments()
    {
        DataTable retVal = new DataTable();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift(@"SELECT * FROM
                (
                SELECT 
                SHIPPING_SHIPMENTS.ITEM
				
                ,SHIPPING_SHIPMENTS.SHIP_TO_ID
                ,SHIP_TO_NM
                ,SHPMT_DATE
                ,SHIP_QTY
                ,PKG_TYPE
                ,PCS_PER
                ,CASE
                WHEN CAST(SHIP_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN SHIP_QTY / PCS_PER
                WHEN CAST(SHIP_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((SHIP_QTY / PCS_PER) + 1,0,1)
                END AS BINS
                FROM 
                CUSTOMER_DATA,SHIPPING_SHIPMENTS
                JOIN
                ITEM_INFO
                ON ITEM_INFO.ITEM = SHIPPING_SHIPMENTS.ITEM
                WHERE 
                CAST(SHPMT_DATE AS VARCHAR(11)) = CAST(GETDATE() AS VARCHAR(11))
                --SHPMT_DATE BETWEEN '4/28/2011 12:00 AM' AND '4/28/2011 9:00 PM'
                AND PCS_PER > 0
				AND CUSTOMER_DATA.CUST_ID = SHIPPING_SHIPMENTS.CUST_ID
				AND CUST_CLAS2 = 'E') AS A");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return retVal;
    }
    public string GetFsPartDescription(string partNumber)
    {
        DataTable retVal = new DataTable();
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift("SELECT ITEM_DESC FROM ITEM_ITEMLIST WHERE ITEM = '" + partNumber + "'");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        string desc = "";
        if (retVal.Rows.Count > 0)
        {
            desc = retVal.Rows[0]["ITEM_DESC"].ToString();
        }
        return desc;
    }
    public DataTable GetFsVendors()
    {
        DataTable retVal = new DataTable("Vendors");
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift("SELECT VENDOR_ID, VEND_NAME FROM VENDOR_DATA ORDER BY VEND_NAME");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return retVal;
    }
    public DataTable GetEmballageDemand(string binno)
    {
        DataTable retVal = new DataTable("Emballage");
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift(@"
                                SELECT 
                                * FROM 
                                (

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE, 1 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ORDER_CUSTOMERDETAIL.ITEM,SUM(ORDER_QTY - TOT_SHPPED) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM ORDER_CUSTOMERDETAIL,CUSTOMER_DATA,ITEM_INFO
                                WHERE 
                                ORDER_CUSTOMERDETAIL.CUST_ID = CUSTOMER_DATA.CUST_ID
                                AND PKG_TYPE IN ('840','780','750','500')
                                AND ORDER_CUSTOMERDETAIL.ITEM = ITEM_INFO.ITEM
                                AND ORDER_CUSTOMERDETAIL.ITEM <> 'VPNHOLDSALES'
                                AND LN_STA= '4'
                                AND CUST_CLAS2 = 'E'
                                AND PCS_PER > 0
                                AND RQST_SHIP BETWEEN '1/1/1900' AND DATEADD(WEEK,1,GETDATE())
                                GROUP BY ORDER_CUSTOMERDETAIL.ITEM,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,2 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ORDER_CUSTOMERDETAIL.ITEM,SUM(ORDER_QTY - TOT_SHPPED) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM ORDER_CUSTOMERDETAIL,CUSTOMER_DATA,ITEM_INFO
                                WHERE 
                                ORDER_CUSTOMERDETAIL.CUST_ID = CUSTOMER_DATA.CUST_ID
                                AND PKG_TYPE IN ('840','780','750','500')
                                AND ORDER_CUSTOMERDETAIL.ITEM = ITEM_INFO.ITEM
                                AND ORDER_CUSTOMERDETAIL.ITEM <> 'VPNHOLDSALES'
                                AND LN_STA= '4'
                                AND CUST_CLAS2 = 'E'
                                AND PCS_PER > 0
                                AND RQST_SHIP BETWEEN DATEADD(WEEK,1,GETDATE()) AND DATEADD(WEEK,2,GETDATE())
                                GROUP BY ORDER_CUSTOMERDETAIL.ITEM,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,3 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ORDER_CUSTOMERDETAIL.ITEM,SUM(ORDER_QTY - TOT_SHPPED) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM ORDER_CUSTOMERDETAIL,CUSTOMER_DATA,ITEM_INFO
                                WHERE 
                                ORDER_CUSTOMERDETAIL.CUST_ID = CUSTOMER_DATA.CUST_ID
                                AND PKG_TYPE IN ('840','780','750','500')
                                AND ORDER_CUSTOMERDETAIL.ITEM = ITEM_INFO.ITEM
                                AND ORDER_CUSTOMERDETAIL.ITEM <> 'VPNHOLDSALES'
                                AND LN_STA= '4'
                                AND CUST_CLAS2 = 'E'
                                AND PCS_PER > 0
                                AND RQST_SHIP BETWEEN DATEADD(WEEK,2,GETDATE()) AND DATEADD(WEEK,3,GETDATE())
                                GROUP BY ORDER_CUSTOMERDETAIL.ITEM,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,4 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ORDER_CUSTOMERDETAIL.ITEM,SUM(ORDER_QTY - TOT_SHPPED) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM ORDER_CUSTOMERDETAIL,CUSTOMER_DATA,ITEM_INFO
                                WHERE 
                                ORDER_CUSTOMERDETAIL.CUST_ID = CUSTOMER_DATA.CUST_ID
                                AND PKG_TYPE IN ('840','780','750','500')
                                AND ORDER_CUSTOMERDETAIL.ITEM = ITEM_INFO.ITEM
                                AND ORDER_CUSTOMERDETAIL.ITEM <> 'VPNHOLDSALES'
                                AND LN_STA= '4'
                                AND CUST_CLAS2 = 'E'
                                AND PCS_PER > 0
                                AND RQST_SHIP BETWEEN DATEADD(WEEK,3,GETDATE()) AND DATEADD(WEEK,4,GETDATE())
                                GROUP BY ORDER_CUSTOMERDETAIL.ITEM,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,5 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ORDER_CUSTOMERDETAIL.ITEM,SUM(ORDER_QTY - TOT_SHPPED) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM ORDER_CUSTOMERDETAIL,CUSTOMER_DATA,ITEM_INFO
                                WHERE 
                                ORDER_CUSTOMERDETAIL.CUST_ID = CUSTOMER_DATA.CUST_ID
                                AND PKG_TYPE IN ('840','780','750','500')
                                AND ORDER_CUSTOMERDETAIL.ITEM = ITEM_INFO.ITEM
                                AND ORDER_CUSTOMERDETAIL.ITEM <> 'VPNHOLDSALES'
                                AND LN_STA= '4'
                                AND CUST_CLAS2 = 'E'
                                AND PCS_PER > 0
                                AND RQST_SHIP BETWEEN DATEADD(WEEK,4,GETDATE()) AND DATEADD(WEEK,5,GETDATE())
                                GROUP BY ORDER_CUSTOMERDETAIL.ITEM,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,6 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ORDER_CUSTOMERDETAIL.ITEM,SUM(ORDER_QTY - TOT_SHPPED) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM ORDER_CUSTOMERDETAIL,CUSTOMER_DATA,ITEM_INFO
                                WHERE 
                                ORDER_CUSTOMERDETAIL.CUST_ID = CUSTOMER_DATA.CUST_ID
                                AND PKG_TYPE IN ('840','780','750','500')
                                AND ORDER_CUSTOMERDETAIL.ITEM = ITEM_INFO.ITEM
                                AND ORDER_CUSTOMERDETAIL.ITEM <> 'VPNHOLDSALES'
                                AND LN_STA= '4'
                                AND CUST_CLAS2 = 'E'
                                AND PCS_PER > 0
                                AND RQST_SHIP BETWEEN DATEADD(WEEK,5,GETDATE()) AND DATEADD(WEEK,6,GETDATE())
                                GROUP BY ORDER_CUSTOMERDETAIL.ITEM,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE
                                )AS C

                                WHERE PKG_TYPE = '" + binno + "'");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return retVal;
    }
    public DataTable GetEmballageForecast(string binno)
    {
        DataTable retVal = new DataTable("Emballage");
        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            retVal = ExecuteSqlAgainstFourthShift(@"
                                SELECT 
                                * FROM 
                                (

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE, 1 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ITEM_NO,SUM(QTY) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM IHC_IRS_FORECAST,ITEM_INFO
                                WHERE 
								PKG_TYPE IN ('840','780','750','500')
                                AND IHC_IRS_FORECAST.ITEM_NO = ITEM_INFO.ITEM
                                AND PCS_PER > 0
                                AND DTE BETWEEN '1/1/1900' AND DATEADD(WEEK,1,GETDATE())
                                GROUP BY ITEM_NO,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,2 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ITEM_NO,SUM(QTY) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM IHC_IRS_FORECAST,ITEM_INFO
                                WHERE 
								PKG_TYPE IN ('840','780','750','500')
                                AND IHC_IRS_FORECAST.ITEM_NO = ITEM_INFO.ITEM
                                AND PCS_PER > 0
                                AND DTE BETWEEN DATEADD(WEEK,1,GETDATE()) AND DATEADD(WEEK,2,GETDATE())
                                GROUP BY ITEM_NO,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,3 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ITEM_NO,SUM(QTY) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM IHC_IRS_FORECAST,ITEM_INFO
                                WHERE 
								PKG_TYPE IN ('840','780','750','500')
                                AND IHC_IRS_FORECAST.ITEM_NO = ITEM_INFO.ITEM
                                AND PCS_PER > 0
                                AND DTE BETWEEN DATEADD(WEEK,2,GETDATE()) AND DATEADD(WEEK,3,GETDATE())
                                GROUP BY ITEM_NO,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,4 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ITEM_NO,SUM(QTY) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM IHC_IRS_FORECAST,ITEM_INFO
                                WHERE 
								PKG_TYPE IN ('840','780','750','500')
                                AND IHC_IRS_FORECAST.ITEM_NO = ITEM_INFO.ITEM
                                AND PCS_PER > 0
                                AND DTE BETWEEN DATEADD(WEEK,3,GETDATE()) AND DATEADD(WEEK,4,GETDATE())
                                GROUP BY ITEM_NO,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,5 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ITEM_NO,SUM(QTY) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM IHC_IRS_FORECAST,ITEM_INFO
                                WHERE 
								PKG_TYPE IN ('840','780','750','500')
                                AND IHC_IRS_FORECAST.ITEM_NO = ITEM_INFO.ITEM
                                AND PCS_PER > 0
                                AND DTE BETWEEN DATEADD(WEEK,4,GETDATE()) AND DATEADD(WEEK,5,GETDATE())
                                GROUP BY ITEM_NO,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE

                                UNION

                                SELECT 
                                SUM(BINS) AS BINS,PKG_TYPE,6 AS WEEK
                                FROM 
                                (
                                SELECT 
                                *,
                                 CASE
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) = 0 THEN ORDER_QTY / PCS_PER
                                                WHEN CAST(ORDER_QTY AS INT) % CAST(PCS_PER AS INT) > 0 THEN ROUND((ORDER_QTY / PCS_PER) + 1,0,1)
                                                END AS BINS

                                FROM
                                (
                                SELECT ITEM_NO,SUM(QTY) AS ORDER_QTY,PKG_TYPE,PCS_PER
                                FROM IHC_IRS_FORECAST,ITEM_INFO
                                WHERE 
								PKG_TYPE IN ('840','780','750','500')
                                AND IHC_IRS_FORECAST.ITEM_NO = ITEM_INFO.ITEM
                                AND PCS_PER > 0
                                AND DTE BETWEEN DATEADD(WEEK,5,GETDATE()) AND DATEADD(WEEK,6,GETDATE())
                                GROUP BY ITEM_NO,PKG_TYPE,PCS_PER
                                ) AS A
                                ) AS B
                                GROUP BY PKG_TYPE
                                )AS C

                                WHERE PKG_TYPE = '" + binno + "'");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return retVal;
    }
    public string GetSequence(string partNumber)
     {
        string seq = "";

        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            DataTable dt = ExecuteSqlAgainstFourthShift("SELECT ITEM_REF4 AS SEQ FROM ITEM_ITEMLIST WHERE ITEM = '" + partNumber.Trim() + "'");
            string returned = dt.Rows[0][0].ToString();
            seq = returned;// (cmd.ExecuteScalar() == null ? "" : cmd.ExecuteScalar().ToString());
        }
        catch (OdbcException odbcEx)
        {
            throw odbcEx;
        }
        catch (Exception ex)
        {
            seq = "";
        }
        finally
        {
        }
        return seq;
    }

    public bool PartNumberExists(string partNumber)
    {
        bool exists = false;

        try
        {
            //ihc2.Service fsService = new ihc2.Service();
            DataTable dt = ExecuteSqlAgainstFourthShift("SELECT COUNT(ITEM) FROM ITEM_ITEMLIST WHERE ITEM = '" + partNumber + "'");
            int count = Int32.Parse(dt.Rows[0][0].ToString());
            if (count > 0) { exists = true; } else { exists = false; }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return exists;
    }
    public bool LotNumberExists(string lotNumber)
    {
        bool exists = false;

        try
        {
           // ihc2.Service fsService = new ihc2.Service();
            DataTable dt = ExecuteSqlAgainstFourthShift("SELECT COUNT(MO_NUMBER) FROM  ORDER_MANUFACTDETAIL WHERE MO_NUMBER = '" + lotNumber + "' AND ITEM IS NOT NULL");
            int count = Int32.Parse(dt.Rows[0][0].ToString());
            if (count > 0) { exists = true; } else { exists = false; }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
        return exists;
    }
    public bool PartAlreadyEnteredForRro(string pn, string raNumber, string qty, string date)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), @" SELECT COUNT(*) FROM RRO_DATA WHERE RR_RA_NBR = @RA_NBR AND RR_IHC_PART_NBR = @IHC_PN AND " +
                                                    @" RR_REC_DTE = @REC_DTE AND RR_QTY = @QTY");
        try
        {
            cmd.Connection.Open();
            cmd.Parameters.AddWithValue("@RA_NBR", raNumber);
            cmd.Parameters.AddWithValue("@IHC_PN", pn);
            cmd.Parameters.AddWithValue("@REC_DTE", date);
            cmd.Parameters.AddWithValue("@QTY", qty);

            int retval = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (retval > 0) { exists = true; }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }
    public bool PartAlreadyEnteredForInternalRro(string pn, string irNumber, string qty)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), @" SELECT COUNT(*) FROM IHC_RRO_DATA WHERE IHC_IR_NBR = @IR_NBR AND IHC_IR_IHC_PART_NBR = @IHC_PN AND " +
                                                    @"IHC_IR_QTY = @QTY");
        try
        {
            cmd.Connection.Open();
            cmd.Parameters.AddWithValue("@IR_NBR", irNumber);
            cmd.Parameters.AddWithValue("@IHC_PN", pn);
            cmd.Parameters.AddWithValue("@QTY", qty);

            int retval = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (retval > 0) { exists = true; }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return false;
    }

    public void UpdateOhQty()
    {
        
        SqlCommand cmd = GetCommand(GetConnection(), @"UPDATE EMB_PACKAGE_TYPES

SET EMB_OH_QTY = EMB_OH_QTY - (SELECT ISNULL(SUM(EMB_BIN_QTY),0)  FROM EMB_SHIPMENTS WHERE  CAST(EMB_DATE AS VARCHAR(11)) = CAST(GETDATE() AS VARCHAR(11)) AND EMB_PKG_TYPE 
= EMB_PKG_NO OR EMB_PKG_TYPE = EMB_PKG_NO2)");
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        
    }

    public string GetCustomerNameById(string customerId)
    {
        string name = string.Empty;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT CUST_NAME FROM CUSTOMER_DATA WHERE CUST_ID ='" + customerId + "'");
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            name = cmd.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return name;
    }
    public bool IsCompleted(string rrKey)
    {
        StringBuilder sql = new StringBuilder(783);
        sql.Append(@" DECLARE @TASKSLEFT INT");
        sql.Append(@" DECLARE @INSPECTED INT");
        sql.Append(@" DECLARE @IS_COMPLETED BIT");
        sql.Append(@" DECLARE @IS_INSPECTED BIT");
        sql.Append(@" SET @IS_INSPECTED = 'FALSE'");
        sql.Append(@" SET @IS_COMPLETED = 'FALSE'");
        sql.Append(@"");
        sql.Append(@" SET @INSPECTED = (SELECT COUNT(*) FROM TIME_LOG ");
        sql.Append(@"   WHERE TME_ACTIVITY_CDE = 13");
        sql.Append(@"   AND TME_RR_KEY = " + rrKey);
        sql.Append(@"   AND(");
        sql.Append(@" (TME_SET_UP_TME <> 0 AND TME_SET_UP_TME IS NOT NULL)");
        sql.Append(@"   OR");
        sql.Append(@"    (TME_QTY <> 0 AND TME_QTY IS NOT NULL)))");
        sql.Append(@" ");
        sql.Append(@" SET @TASKSLEFT =  (SELECT COUNT(*) FROM TIME_LOG");
        sql.Append(@" WHERE ");
        sql.Append(@"(TME_SET_UP_TME = 0 OR TME_SET_UP_TME IS NULL)");
        sql.Append(@" AND ");
        sql.Append(@"(TME_QTY = 0 OR TME_QTY IS NULL)");
        sql.Append(@" AND");
        sql.Append(@" TME_RR_KEY = " + rrKey);
        sql.Append(@" AND TME_SHOW_FIELD = 'TRUE' AND TME_ACTIVITY_CDE <> 14)");
        sql.Append(@" IF(@INSPECTED > 0)");
        sql.Append(@" BEGIN SET @IS_INSPECTED = 'TRUE'");
        sql.Append(@" END");
        sql.Append(@" ");
        sql.Append(@" IF(@TASKSLEFT = 0 AND @IS_INSPECTED = 'TRUE')");
        sql.Append(@" BEGIN SET @IS_COMPLETED = 'TRUE'");
        sql.Append(@" END");
        sql.Append(@" ");
        sql.Append(@" SELECT @IS_COMPLETED");
        //HttpContext.Current.Response.Write(sql.ToString());
        bool retVal = false;

        SqlCommand cmd = GetCommand(GetConnection(), sql.ToString());
        try
        {
            cmd.Connection.Open();
            retVal = Boolean.Parse(cmd.ExecuteScalar().ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return retVal;
    }
    public string GetVendorNameById(string vendorId)
    {
        string name = string.Empty;
       // ihc2.Service fsService = new ihc2.Service();
        DataTable retVal = ExecuteSqlAgainstFourthShift("SELECT VEND_NAME FROM VENDOR_DATA WHERE VENDOR_ID ='" + vendorId + "'");
        try
        {
            name = retVal.Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return name;
    }

    public bool NcmNumberExists(string ncmNumber)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT COUNT(*) FROM NCM_LOG WHERE NCM_NCM_NBR = @NCM_NCM_NBR");
        try
        {
            cmd.Parameters.AddWithValue("@NCM_NCM_NBR", ncmNumber);
            cmd.Connection.Open();
            int count = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (count > 0) exists = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }

    public bool IrNumberExists(string irNumber)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT COUNT(*) FROM IR_LOG WHERE IR_IR_NBR = @IR_IR_NBR");
        try
        {
            cmd.Parameters.AddWithValue("@IR_IR_NBR", irNumber);
            cmd.Connection.Open();
            int count = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (count > 0) exists = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }
    public bool RroNumberExists(string rroNumber, String raNumber)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT COUNT(*) FROM RRO_DATA WHERE RR_RO_NBR = @RR_RO_NBR AND RR_RA_NBR = @RR_RA_NBR ");
        try
        {
            cmd.Parameters.AddWithValue("@RR_RO_NBR", rroNumber);
            cmd.Parameters.AddWithValue("@RR_RA_NBR", raNumber);
            cmd.Connection.Open();
            int count = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (count > 0) exists = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }
    public bool InternalRroNumberExists(string rroNumber, String raNumber)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT COUNT(*) FROM IHC_RRO_DATA WHERE IHC_IR_RO_NBR = @IR_RO_NBR AND IHC_IR_NBR = @IHC_IR_NBR ");
        try
        {
            cmd.Parameters.AddWithValue("@IR_RO_NBR", rroNumber);
            cmd.Parameters.AddWithValue("@IHC_IR_NBR", raNumber);
            cmd.Connection.Open();
            int count = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (count > 0) exists = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }
    public bool CaNumberExists(string caNumber)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT COUNT(*) FROM CA_GENERAL WHERE GEN_CA_NBR = @GEN_CA_NBR");
        try
        {
            cmd.Parameters.AddWithValue("@GEN_CA_NBR", caNumber);
            cmd.Connection.Open();
            int count = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (count > 0) exists = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }
    public bool RaNumberExists(string caNumber)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT COUNT(*) FROM RA_LOG WHERE RA_RA_NBR = @RA_RA_NBR");
        try
        {
            cmd.Parameters.AddWithValue("@RA_RA_NBR", caNumber);
            cmd.Connection.Open();
            int count = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (count > 0) exists = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }
    public bool NcmAssociatedWithRa(string ncmNumber)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT COUNT(*) FROM RA_LOG WHERE RA_NCM_NBR = @RA_NCM_NBR");
        try
        {
            cmd.Parameters.AddWithValue("@RA_NCM_NBR", ncmNumber);
            cmd.Connection.Open();
            int count = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (count > 0) exists = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }
    public bool NcmAssociatedWithRaEdit(string ncmNumber, string raNumber)
    {
        bool exists = false;
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT COUNT(*) FROM RA_LOG WHERE RA_NCM_NBR = @RA_NCM_NBR AND RA_RA_NBR <> @RA_RA_NBR");
        try
        {
            cmd.Parameters.AddWithValue("@RA_NCM_NBR", ncmNumber);
            cmd.Parameters.AddWithValue("@RA_RA_NBR", raNumber);
            cmd.Connection.Open();
            int count = Int32.Parse(cmd.ExecuteScalar().ToString());
            if (count > 0) exists = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return exists;
    }
    public string GetGeneratedRaNumber(string ncmNumber, string custCode)
    {
        SqlCommand cmd = GetCommand(GetConnection(),"SELECT RA_RA_NBR FROM RA_LOG"
                           + " WHERE RA_NCM_NBR = @RA_NCM_NBR" 
                           + " AND RA_CUST_CDE = @RA_CUST_CDE");
        try
        {
            cmd.Parameters.AddWithValue("@RA_NCM_NBR", ncmNumber);
            cmd.Parameters.AddWithValue("@RA_CUST_CDE", custCode);
            cmd.Connection.Open();
            return cmd.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
    }
    public string GetRaKeyByRaNumber(string raNumber)
    {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT RA_KEY FROM RA_LOG WHERE RA_RA_NBR = @RA_RA_NBR");
            try
            {
                cmd.Parameters.AddWithValue("@RA_RA_NBR",raNumber);
                cmd.Connection.Open();
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
    }

    public string GetDebitAmt(string raNumber)
    {
        SqlCommand cmd = GetCommand(GetConnection(), "SELECT RA_CUST_DEBIT FROM RA_LOG WHERE RA_RA_NBR = @RA_RA_NBR");
        try
        {
            cmd.Parameters.AddWithValue("@RA_RA_NBR", raNumber);
            cmd.Connection.Open();
            return cmd.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
    }
    #region " AutoComplete Methods "
   
    public string[] GetCaNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 GEN_CA_NBR FROM CA_GENERAL WHERE GEN_CA_NBR LIKE @CA_CA_NBR");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@CA_CA_NBR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetRmoNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 RMO_RMO_NBR2 FROM RMO_LOG WHERE RMO_RMO_NBR2 LIKE @RMO_NBR2 ");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@RMO_NBR2", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetINcmNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 INCM_NCM_NBR FROM IHC_NCM_LOG WHERE INCM_NCM_NBR LIKE @INCM_NCM_NBR ");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@INCM_NCM_NBR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetTaskNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 PM_TASK_NBR FROM PM_TABLE WHERE PM_TASK_NBR LIKE @TASK_NBR AND PM_INPROCESS = 'TRUE' ");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@TASK_NBR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetNcmNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 NCM_NCM_NBR FROM NCM_LOG WHERE NCM_NCM_NBR LIKE @NCM_NCM_NBR ");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@NCM_NCM_NBR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetContactNames(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT DISTINCT TOP 10 CUST_CONTACT_NME FROM CUSTOMER_CONTACTS WHERE CUST_CONTACT_NME LIKE @CUST_CONTACT_NME ORDER BY CUST_CONTACT_NME");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@CUST_CONTACT_NME", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetVendorEmails(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT DISTINCT TOP 10 VEND_EMAIL_ADDR FROM VENDOR_EMAIL WHERE VEND_EMAIL_ADDR LIKE @EMAIL_ADDR ORDER BY VEND_EMAIL_ADDR");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@EMAIL_ADDR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetContactEmails(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT DISTINCT TOP 10 CUST_CONTACT_EMAIL FROM CUSTOMER_CONTACTS WHERE CUST_CONTACT_EMAIL LIKE @CUST_CONTACT_EMAIL ORDER BY CUST_CONTACT_EMAIL");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@CUST_CONTACT_EMAIL", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetRaNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 RA_RA_NBR FROM RA_LOG WHERE RA_RA_NBR LIKE @RA_RA_NBR");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@RA_RA_NBR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }

    public string[] GetIrNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 IR_IR_NBR FROM IR_LOG WHERE IR_IR_NBR LIKE @IR_IR_NBR");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@IR_IR_NBR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }


    public string[] GetRroNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 RR_RO_NBR,RR_RA_NBR FROM RRO_DATA WHERE RR_RA_NBR LIKE @RA_NBR");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@RA_NBR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row["RR_RA_NBR"].ToString().Trim() + "-" + Utility.Pad(row["RR_RO_NBR"].ToString().Trim()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public string[] GetInternalRroNumbers(string prefixText)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT TOP 10 IHC_IR_RO_NBR,IHC_IR_NBR FROM IHC_RRO_DATA WHERE IHC_IR_NBR LIKE @IR_NBR");
            List<string> items = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@IR_NBR", prefixText);
                cmd.Parameters[0].Value = prefixText + "%";
                cmd.Connection.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(row["IHC_IR_NBR"].ToString().Trim() + "-" + Utility.Pad(row["IHC_IR_RO_NBR"].ToString().Trim()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return (string[])items.ToArray();
        }
    }
    public void InsertPartsData(string key,string rroNumber, string partNumber, int qty,string raNumber)
    {
        if (!new DAL().PartNumberExists(partNumber))
        {
            throw new Exception("Part number <b>" + partNumber + "</b> does not exist in Fourth Shift.");
        }
        {
            SqlCommand cmd;
            if (key == String.Empty)
            {
                 cmd = GetCommand(GetConnection(), "INSERT INTO RRO_PARTS_DATA(RWK_RR_NBR,RWK_IHC_PART_NBR,RWK_QTY,RWK_RA_NBR)"
                    + "VALUES(@RWK_RR_NBR,@RWK_IHC_PART_NBR,@RWK_QTY,@RWK_RA_NBR)");
            }
            else
            {
                 cmd = GetCommand(GetConnection(), "UPDATE RRO_PARTS_DATA "
                    + "SET RWK_IHC_PART_NBR = @RWK_IHC_PART_NBR, RWK_QTY = @RWK_QTY WHERE RWK_KEY = @RWK_KEY");
            }
            try
            {
                cmd.Parameters.AddWithValue("@RWK_RR_NBR",rroNumber);
                cmd.Parameters.AddWithValue("@RWK_IHC_PART_NBR",partNumber);
                cmd.Parameters.AddWithValue("@RWK_QTY", qty);
                cmd.Parameters.AddWithValue("@RWK_RA_NBR", raNumber);
                cmd.Parameters.AddWithValue("@RWK_KEY", key);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public void InsertInternalPartsData(string key, string rroNumber, string partNumber, int qty, string irNumber)
    {
        {
            SqlCommand cmd;
            if (key == String.Empty)
            {
                cmd = GetCommand(GetConnection(), "INSERT INTO IHC_RRO_PARTS_DATA(RWK_RR_NBR,RWK_IHC_PART_NBR,RWK_QTY,RWK_IR_NBR)"
                   + "VALUES(@RWK_RR_NBR,@RWK_IHC_PART_NBR,@RWK_QTY,@RWK_IR_NBR)");
            }
            else
            {
                cmd = GetCommand(GetConnection(), "UPDATE IHC_RRO_PARTS_DATA "
                   + "SET RWK_IHC_PART_NBR = @RWK_IHC_PART_NBR, RWK_QTY = @RWK_QTY WHERE RWK_IR_KEY = @RWK_KEY");
            }
            try
            {
                cmd.Parameters.AddWithValue("@RWK_RR_NBR", rroNumber);
                cmd.Parameters.AddWithValue("@RWK_IHC_PART_NBR", partNumber);
                cmd.Parameters.AddWithValue("@RWK_QTY", qty);
                cmd.Parameters.AddWithValue("@RWK_IR_NBR", irNumber);
                cmd.Parameters.AddWithValue("@RWK_KEY", key);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public void DeletePartsData(string key)
    {
        {

            SqlCommand cmd = GetCommand(GetConnection(), "DELETE FROM RRO_PARTS_DATA WHERE RWK_KEY = @RWK_KEY");
            try
            {
                cmd.Parameters.AddWithValue("@RWK_KEY", key);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public void DeleteEmbShipments()
    {
        {

            SqlCommand cmd = GetCommand(GetConnection(), "TRUNCATE TABLE EMB_SHIPMENT2");
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public void UpdateEmbShipments()
    {
        {

            SqlCommand cmd = GetCommand(GetConnection(), @"
            INSERT INTO EMB_SHIPMENTS(EMB_DATE,EMB_CUST_NME,EMB_SHIP_TO_ID,EMB_PKG_TYPE,EMB_BIN_QTY,EMB_SHIP_QTY,EMB_PCS_PER,EMB_ITEM)
            SELECT EMB_DATE,EMB_CUST_NME,EMB_SHIP_TO_ID,EMB_PKG_TYPE,EMB_BIN_QTY,EMB_SHIP_QTY,EMB_PCS_PER,EMB_ITEM FROM EMB_SHIPMENT2");
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public void DeleteMIData(string equipmentID)
    {
        {

            SqlCommand cmd = GetCommand(GetConnection(), "DELETE FROM EQUIPMENT_MAINTENANCE WHERE EQUIPMENT_ID = @EID");
            try
            {
                cmd.Parameters.AddWithValue("@EID", equipmentID);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public void DeleteInternalPartsData(string key)
    {
        {

            SqlCommand cmd = GetCommand(GetConnection(), "DELETE FROM IHC_RRO_PARTS_DATA WHERE RWK_IR_KEY = @RWK_KEY");
            try
            {
                cmd.Parameters.AddWithValue("@RWK_KEY", key);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public void UpdateRro(bool requiresWire, string roNbr, string raNumber)
    {
        {

            SqlCommand cmd = GetCommand(GetConnection(), "UPDATE RRO_DATA SET " +
                        "RR_REQ_WIRE = @RR_REQ_WIRE " +
                        "WHERE RR_RO_NBR = @RR_RO_NBR AND RR_RA_NBR = @RR_RA_NBR");
            try
            {
                cmd.Parameters.AddWithValue("@RR_REQ_WIRE", requiresWire);
                cmd.Parameters.AddWithValue("@RR_RO_NBR", roNbr);
                cmd.Parameters.AddWithValue("@RR_RA_NBR", raNumber);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public void UpdateInternalRro(bool requiresWire, string roNbr, string irNumber)
    {
        {

            SqlCommand cmd = GetCommand(GetConnection(), "UPDATE IHC_RRO_DATA SET " +
                        "IHC_IR_REQ_WIRE = @RR_REQ_WIRE " +
                        "WHERE IHC_IR_RO_NBR = @RR_RO_NBR AND IHC_IR_NBR = @RR_IR_NBR");
            try
            {
                cmd.Parameters.AddWithValue("@RR_REQ_WIRE", requiresWire);
                cmd.Parameters.AddWithValue("@RR_RO_NBR", roNbr);
                cmd.Parameters.AddWithValue("@RR_IR_NBR", irNumber);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
    public DataTable GetRroEmailInfo(string raNumber)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT * FROM RRO_DATA,RA_LOG WHERE RR_RA_NBR = @RA_NBR AND RA_RA_NBR = @RA_NBR");
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@RA_NBR", raNumber);
                cmd.Connection.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
    }
    public DataTable GetIrRroEmailInfo(string irNumber)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), "SELECT * FROM IHC_RRO_DATA,IR_LOG WHERE IHC_IR_NBR = @IR_NBR AND IR_IR_NBR = @IR_NBR");
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@IR_NBR", irNumber);
                cmd.Connection.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
    }
    public DataTable GetIrEmailInfo(string irNumber)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), @"SELECT * FROM IR_LOG WHERE IR_IR_NBR= @IR_NBR");
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@IR_NBR", irNumber);
                cmd.Connection.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
    }   

    public DataTable GetRaEmailInfo(string raNumber)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), @"SELECT " +
                                                        @"RA_RA_NBR" +
                                                        @",RA_CUST_CDE" +
                                                        @",RA_ISSUE_DTE" +
                                                        @",RA_QTY" +
                                                        @",RA_CUST_PART_NBR" +
                                                        @",DEFECT_DESC" +
                                                        @",RA_CUST_TRACK_NBR" +
                                                        @",RA_CUST_DEBIT" +
                                                        @",RA_RETURN_PO_NBR" +
                                                        @",RA_RETURN_DESC" +
                                                        @",RA_RECEIPT_DTE1" +
                                                        @",RA_RECEIPT_DTE2" +
                                                        @",RA_RECEIPT_DTE3" +
                                                        @",RA_IHC_PART_NBR1" +
                                                        @",RA_IHC_PART_NBR2" +
                                                        @",RA_IHC_PART_NBR3" +
                                                        @",RA_RECIEPT_QTY1" +
                                                        @",RA_RECIEPT_QTY2" +
                                                        @",RA_RECIEPT_QTY3" +
                                                        @",RA_NCM_NBR" +
                                                        @",RA_CUST_CONTACT" +
                                                        @",RA_CUST_EMAIL" +
                                                        @",NCM_NOTES" + 
                                                        @",NCM_INIT_INVESTIGATION" + 
                                                        @"" +
                                                        @" FROM RA_LOG,IHC_DEFECT_CODES,NCM_LOG " +
                                                        @"WHERE RA_RA_NBR = @RA_NBR " +
                                                        @"AND RA_RETURN_CDE = DEFECT_ID " + 
                                                        @"AND RA_NCM_NBR = NCM_NCM_NBR");
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@RA_NBR",raNumber);
                cmd.Connection.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
    }
 public DataTable GetRaIhcPartsDataTable(string raNumber)
 {
     SqlCommand cmd = GetCommand(GetConnection(), "SELECT RA_IHC_PART_NBR1,RA_IHC_PART_NBR2,RA_IHC_PART_NBR3,RA_RECIEPT_QTY1,RA_RECIEPT_QTY2,RA_RECIEPT_QTY3,RA_RECEIPT_DTE1,RA_RECEIPT_DTE2,RA_RECEIPT_DTE3 FROM RA_LOG WHERE RA_RA_NBR = @RA_RA_NBR");
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@RA_RA_NBR",raNumber);
                cmd.Connection.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
    public DataTable RmoLookup(string partNumber, string vendorCode)
    {
        SqlCommand cmd = GetCommand(GetConnection(), @"SELECT
        RMO_RMO_NBR2,
        RMO_VENDOR_ID,
        RMO_VENDOR_NME,
        INCM_PART_NBR AS 'PART #',
        INCM_QTY AS 'Quantity'    
        FROM RMO_LOG,IHC_NCM_LOG WHERE INCM_PART_NBR LIKE @IHC_PART_NBR
        AND RMO_NCM_NBR = INCM_NCM_NBR");

        if (vendorCode != "select")
        {
            cmd.CommandText += " AND RMO_VENDOR_ID = @VENDOR_CDE";
            cmd.Parameters.AddWithValue("@VENDOR_CDE", vendorCode);
        }

        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Parameters.AddWithValue("@IHC_PART_NBR", partNumber);
            cmd.Parameters["@IHC_PART_NBR"].Value += "%";
            cmd.Connection.Open();
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }
    public DataTable INcmLookup(string partNumber, string customerCode)
    {
        SqlCommand cmd = GetCommand(GetConnection(), @"SELECT" +
@"        INCM_NCM_NBR AS [NCM #]," +
@"        INCM_PART_NBR AS [Part #]," +
@"        INCM_QTY AS [Quantity]" +
@"        FROM IHC_NCM_LOG WHERE INCM_PART_NBR LIKE @IHC_PART_NBR");

        if (customerCode != "select")
        {
            cmd.CommandText += " AND NCM_CUST_CDE = @NCM_CUST_CDE";
            cmd.Parameters.AddWithValue("@NCM_CUST_CDE", customerCode);
        }

        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Parameters.AddWithValue("@IHC_PART_NBR", partNumber);
            cmd.Parameters["@IHC_PART_NBR"].Value += "%";
            cmd.Connection.Open();
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }
    public DataTable NcmLookup(string partNumber, string customerCode)
    {
        SqlCommand cmd = GetCommand(GetConnection(), @"SELECT" +
@"        NCM_NCM_NBR AS [NCM #]," +
@"        NCM_IHC_PART_NBR AS [Part #]," +
@"        NCM_QTY AS [Quantity]" +
@"        FROM NCM_LOG WHERE NCM_IHC_PART_NBR LIKE @IHC_PART_NBR");

        if (customerCode != "select")
        {
            cmd.CommandText += " AND NCM_CUST_CDE = @NCM_CUST_CDE";
            cmd.Parameters.AddWithValue("@NCM_CUST_CDE", customerCode);
        }

        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Parameters.AddWithValue("@IHC_PART_NBR", partNumber);
            cmd.Parameters["@IHC_PART_NBR"].Value += "%";
            cmd.Connection.Open();
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }
    public DataTable FlagLookup(string partNumber, string status)
    {
        string sql = @"SELECT FLAG_NO2,PART_NO,CASE WHEN STATUS = 0 THEN 'OPEN' WHEN STATUS = 1 THEN 'CLOSED' END AS STATUS
                FROM INSP_FLAG
                WHERE PART_NO LIKE '" + partNumber + "%'";
                if(status != "(select)")
                {
                    sql += " AND STATUS = " + status;
                }
        SqlCommand cmd = GetCommand(GetConnection(),sql );


        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }
    public DataTable FlagLookupRecv(string partNumber, string status)
    {
        string sql = @"SELECT FLAG_NO2,PART_NO,CASE WHEN STATUS = 0 THEN 'OPEN' WHEN STATUS = 1 THEN 'CLOSED' END AS STATUS
                FROM INSP_FLAG_RECV
                WHERE PART_NO LIKE '" + partNumber + "%'";
        if (status != "(select)")
        {
            sql += " AND STATUS = " + status;
        }
        SqlCommand cmd = GetCommand(GetConnection(), sql);


        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }

    public DataTable GetEmailsByPosition(string position)
    {
        string[] splitPositions = position.Split(',');
        string inString = "(";
        foreach (String s in splitPositions)
        {
            inString += "'" + s + "'" + ",";
        }
        inString = inString.Remove(inString.Length - 1, 1);
        inString += ")";

        SqlCommand cmd = GetCommand(GetConnection(), @"SELECT [EMAIL_POSITION]" +
                                            @"      ,[EMAIL_FIRST_NAME]" +
                                            @"      ,[EMAIL_LAST_NAME]" +
                                            @"      ,[EMAIL_ADDRESS]" +
                                            @"  FROM [dbo].[IHC_EMAIL_POSITIONS]"
                                            +" WHERE EMAIL_POSITION IN " + inString);

        DataTable dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }
    #endregion
    public string InsertRA(string RA_CUST_CONTACT, string RA_CUST_EMAIL, string RA_CUST_CDE, string RA_NCM_NBR,
            string RA_QTY, string RA_CUST_PART_NBR, string RA_RETURN_CDE, string RA_CUST_TRACK_NBR, string RA_RETURN_DESC)
    {
        string raKey = string.Empty;

        StringBuilder sbSQL = new StringBuilder();
        sbSQL.Append("        INSERT INTO RA_LOG([RA_CUST_CONTACT],[RA_CUST_EMAIL],[RA_CUST_CDE], [RA_NCM_NBR], [RA_QTY], ");
        sbSQL.Append("        [RA_CUST_PART_NBR], [RA_RETURN_CDE], [RA_CUST_TRACK_NBR], ");
        sbSQL.Append("        [RA_RETURN_DESC])");
        sbSQL.Append("        VALUES(@RA_CUST_CONTACT,@RA_CUST_EMAIL,@RA_CUST_CDE,@RA_NCM_NBR,@RA_QTY,@RA_CUST_PART_NBR,");
        sbSQL.Append("@RA_RETURN_CDE,@RA_CUST_TRACK_NBR,@RA_RETURN_DESC)");
        sbSQL.Append(" SELECT RA_RA_NBR FROM RA_LOG WHERE RA_KEY = SCOPE_IDENTITY()");

        SqlCommand cmd = GetCommand(GetConnection(), sbSQL.ToString());
        
        cmd.Parameters.AddWithValue("@RA_CUST_CONTACT",RA_CUST_CONTACT);
        cmd.Parameters.AddWithValue("@RA_CUST_EMAIL",RA_CUST_EMAIL);
        cmd.Parameters.AddWithValue("@RA_CUST_CDE",RA_CUST_CDE);
        cmd.Parameters.AddWithValue("@RA_NCM_NBR", RA_NCM_NBR);
        cmd.Parameters.AddWithValue("@RA_QTY",RA_QTY);
        cmd.Parameters.AddWithValue("@RA_CUST_PART_NBR",RA_CUST_PART_NBR);
        cmd.Parameters.AddWithValue("@RA_RETURN_CDE",RA_RETURN_CDE);
        cmd.Parameters.AddWithValue("@RA_CUST_TRACK_NBR",RA_CUST_TRACK_NBR);
        cmd.Parameters.AddWithValue("@RA_RETURN_DESC",RA_RETURN_DESC);
        try
        {
            cmd.Connection.Open();
            raKey = cmd.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
        return raKey;
    }


    public void InsertNCM(
            String NCM_CUST_CDE, String NCM_NCM_NBR, DateTime NCM_ISSUE_DTE,
            int NCM_CUST_DISPOSITION_CDE, String NCM_NOTES,
            String NCM_IHC_PART_NBR, int NCM_QTY, string NCM_CUST_FAULT_CLAIM)
    {

        StringBuilder sbSQL = new StringBuilder();
        sbSQL.AppendFormat("INSERT ");
        sbSQL.AppendFormat("INTO [dbo].[NCM_LOG]");
        sbSQL.AppendFormat("        (");
        sbSQL.AppendFormat("                [NCM_CUST_CDE]            ,");
        sbSQL.AppendFormat("                [NCM_NCM_NBR]             ,");
        sbSQL.AppendFormat("                [NCM_ISSUE_DTE]           ,");
        sbSQL.AppendFormat("                [NCM_IHC_PART_NBR]        ,");
        sbSQL.AppendFormat("                [NCM_QTY]                 ,");
        sbSQL.AppendFormat("                [NCM_CUST_DISPOSITION_CDE],");
        sbSQL.AppendFormat("                [NCM_NOTES],               ");
        sbSQL.AppendFormat("                [NCM_CUST_FAULT_CLAIM]     ");
        sbSQL.AppendFormat("        ) ");
        sbSQL.AppendFormat("        VALUES");
        sbSQL.AppendFormat("        (");
        sbSQL.AppendFormat("@NCM_CUST_CDE,");
        sbSQL.AppendFormat("@NCM_NCM_NBR,");
        sbSQL.AppendFormat("@NCM_ISSUE_DTE,");
        sbSQL.AppendFormat("@NCM_IHC_PART_NBR,");
        sbSQL.AppendFormat("@NCM_QTY,");
        sbSQL.AppendFormat("@NCM_CUST_DISPOSITION_CDE,");
        sbSQL.AppendFormat("@NCM_NOTES,");
        sbSQL.AppendFormat("@NCM_CUST_FAULT_CLAIM");
        sbSQL.AppendFormat("        )");

        
        SqlCommand cmd = GetCommand(GetConnection(), sbSQL.ToString());


        cmd.Parameters.AddWithValue("@NCM_CUST_CDE",NCM_CUST_CDE);
        cmd.Parameters.AddWithValue("@NCM_NCM_NBR", NCM_NCM_NBR);
        cmd.Parameters.AddWithValue("@NCM_ISSUE_DTE", NCM_ISSUE_DTE);
        cmd.Parameters.AddWithValue("@NCM_IHC_PART_NBR", NCM_IHC_PART_NBR);
        cmd.Parameters.AddWithValue("@NCM_QTY", NCM_QTY);
        cmd.Parameters.AddWithValue("@NCM_CUST_DISPOSITION_CDE", NCM_CUST_DISPOSITION_CDE);
        cmd.Parameters.AddWithValue("@NCM_NOTES", NCM_NOTES);
        cmd.Parameters.AddWithValue("@NCM_CUST_FAULT_CLAIM", NCM_CUST_FAULT_CLAIM);

        try
        {
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Connection.Close();
        }
    }
 
    public DataTable GetFsInspectionInfo(string lotNumber)
    {
        string sql = @"SELECT ORDER_MANUFACTDETAIL.ITEM, ITEM_ITEMLIST.ITEM_CLAS2 AS ASSY_CATG, ITEM_ITEMLIST.BUYR AS BOARD
                       FROM ORDER_MANUFACTDETAIL, ITEM_ITEMLIST
                       WHERE ORDER_MANUFACTDETAIL.ITEM = ITEM_ITEMLIST.ITEM AND (ORDER_MANUFACTDETAIL.MO_NUMBER = '" 
                       + lotNumber + "')";
        //ihc2.Service ihc2 = new ihc2.Service();
        return ExecuteSqlAgainstFourthShift(sql);
    }
    public DataTable GetFsMoInfo(string lotNumber)
    {
        string sql = @"SELECT ORDER_MANUFACTDETAIL.ITEM, ORDER_QTY
                       FROM ORDER_MANUFACTDETAIL, ITEM_ITEMLIST
                       WHERE ORDER_MANUFACTDETAIL.ITEM = ITEM_ITEMLIST.ITEM AND (ORDER_MANUFACTDETAIL.MO_NUMBER = '"
                       + lotNumber + "')";
        //ihc2.Service ihc2 = new ihc2.Service();
        return ExecuteSqlAgainstFourthShift(sql);
    }


    public DataTable WireLookup(string cktNo, string lotNo)
    {
        {
            SqlCommand cmd = GetCommand(GetConnection(), @"SELECT " +
                                                            @"LEFT(LOT_NO,5) AS LOT_NO, LOT_NO AS WIRE" +
                                                            @",REPLACE(" +
                                                            @" REPLACE(" +
                                                            @" REPLACE(" +
                                                            @" REPLACE(" +
                                                            @" REPLACE(CKT_NO,'#',''),'%',''),'@',''),'?',''),'(','') AS CKT_NO" +
                                                            @" FROM WIRELIST" +
                                                            @" WHERE " +
                                                            @" REPLACE(" +
                                                            @" REPLACE(" +
                                                            @" REPLACE(" +
                                                            @" REPLACE(" +
                                                            @" REPLACE(CKT_NO,'#',''),'%',''),'@',''),'?',''),'(','') = @CKT_NO" +
                                                            @" AND" +
                                                            @" LEFT(LOT_NO,5) = @LOT_NO");
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.Parameters.AddWithValue("@CKT_NO",cktNo);
                cmd.Parameters.AddWithValue("@LOT_NO", lotNo);
                cmd.Connection.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
    }
}

                                           
                                            
                                            
                                            
                                            
                                            
                                            
                                            
                                            
                                            
                                            
                                            