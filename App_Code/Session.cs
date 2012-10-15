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
/// Summary description for Session
/// </summary>
public class SessionManager
{
  

	public SessionManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private static IHC.IrLog _irEntry;

    public static IHC.IrLog IrEntry
    {
        get { return _irEntry; }
        set { _irEntry = value; }
    }
}

