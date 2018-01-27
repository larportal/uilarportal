using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
	public partial class WhatsNew : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			DataSet dsWhatsNew = LarpPortal.Classes.cUtilities.LoadDataSet("uspGetWhatsNew", sParams, "LARPortal", Session ["UserName"].ToString(), lsRoutineName);
			gvWhatsNewList.DataSource = dsWhatsNew.Tables [0];
			gvWhatsNewList.DataBind();
		}
	}
}