using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Utility
/// </summary>
public class Utility
{
	public Utility()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private static TextBox currentBox;
    public static void Log()
    {
       
    }
    public static TextBox CurrentBox
    {
        get { return Utility.currentBox; }
        set { Utility.currentBox = value; }
    }
    public string ProperCase(string stringInput)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        bool fEmptyBefore = true;
        foreach (char ch in stringInput)
        {
            char chThis = ch;
            if (Char.IsWhiteSpace(chThis))
                fEmptyBefore = true;
            else
            {
                if (Char.IsLetter(chThis) && fEmptyBefore)
                    chThis = Char.ToUpper(chThis);
                else
                    chThis = Char.ToLower(chThis);
                fEmptyBefore = false;
            }
            sb.Append(chThis);
        }
        return sb.ToString();
    }
    public static bool IsNumeric(object ValueToCheck)
    {
    double Dummy = new double();
    string InputValue = Convert.ToString(ValueToCheck);

    bool Numeric = double.TryParse( InputValue , System.Globalization.NumberStyles.Any , null , out Dummy);

    return Numeric;
    }
    public static string Pad(string value)
    {
        string retVal;
        if (value.Trim().Length < 2)
        {
            retVal = "0" + value.ToString();
        }
        else
        {
            retVal = value;
        }
        return retVal;
    }
    public static string GetDateString(string date)
    {
        string returnDateString = "";
        try
        {
            returnDateString = DateTime.Parse(date).ToShortDateString();
        }
        catch (Exception ex)
        {
            ex = null;
            returnDateString = "";
        }
        return returnDateString;
    }
    public static int GetIntFromString(string input)
    {
        int returnInt = 0;
        try
        {
            returnInt = Int32.Parse(input);
        }
        catch (Exception ex)
        {
            ex = null;
            returnInt = 0;
        }
        return returnInt;
    }
    public static double GetDblFromString(string input)
    {
        double returnDbl = 0;
        try
        {
            returnDbl = Double.Parse(input);
        }
        catch (Exception ex)
        {
            ex = null;
            returnDbl = 0;
        }
        return returnDbl;
    }
    public System.Data.DataTable GetTable(System.Data.SqlClient.SqlDataReader _reader)
    {

        System.Data.DataTable _table = _reader.GetSchemaTable();
        System.Data.DataTable _dt = new System.Data.DataTable();
        System.Data.DataColumn _dc;
        System.Data.DataRow _row;
        System.Collections.ArrayList _al = new System.Collections.ArrayList();

        for (int i = 0; i < _table.Rows.Count; i++)
        {

            _dc = new System.Data.DataColumn();

            if (!_dt.Columns.Contains(_table.Rows[i]["ColumnName"].ToString()))
            {

                _dc.ColumnName = _table.Rows[i]["ColumnName"].ToString();
                _dc.Unique = Convert.ToBoolean(_table.Rows[i]["IsUnique"]);
                _dc.AllowDBNull = Convert.ToBoolean(_table.Rows[i]["AllowDBNull"]);
                _dc.ReadOnly = Convert.ToBoolean(_table.Rows[i]["IsReadOnly"]);
                _al.Add(_dc.ColumnName);
                _dt.Columns.Add(_dc);

            }

        }

        while (_reader.Read())
        {

            _row = _dt.NewRow();

            for (int i = 0; i < _al.Count; i++)
            {

                _row[((System.String)_al[i])] = _reader[(System.String)_al[i]];

            }

            _dt.Rows.Add(_row);

        }

        return _dt;

    }
}
