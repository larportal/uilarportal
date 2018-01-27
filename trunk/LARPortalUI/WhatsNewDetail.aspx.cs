using System;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
	public partial class WhatsNewDetail : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				MethodBase lmth = MethodBase.GetCurrentMethod();
				string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

				int intWhatsNewID = 1;
				DateTime dtTemp;

				if (Request.QueryString["WhatsNewID"] != null)
					if (!int.TryParse(Request.QueryString ["WhatsNewID"].ToString(), out intWhatsNewID))
						intWhatsNewID = 1;      // Use the What's New? announcement (ID 1) as the default if we somehow get here with a NULL ID

				DataSet dsWhatsNewDetail = new DataSet();
				SortedList sParams = new SortedList();
				sParams.Add("@WhatsNewID", intWhatsNewID);
				dsWhatsNewDetail = Classes.cUtilities.LoadDataSet("uspGetWhatsNew", sParams, "LARPortal", Session ["UserName"].ToString(), lsRoutineName);
				dsWhatsNewDetail.Tables [0].TableName = "MDBWhatsNew";

				foreach (DataRow dRow in dsWhatsNewDetail.Tables ["MDBWhatsNew"].Rows)
				{
					if (DateTime.TryParse(dRow ["ReleaseDate"].ToString(), out dtTemp))
						lblReleaseDate.Text = dtTemp.ToShortDateString();
					lblModuleName.Text = dRow ["ModuleName"].ToString();
					lblReleaseName.Text = dRow ["BriefName"].ToString();
					lblDescription.Text = dRow ["LongDescription"].ToString();
					lblPanelHeader.Text = "What's New? - " + lblModuleName.Text + " - " + lblReleaseName.Text;
				}
			}
		}

		protected void btnClose_Click(object sender, EventArgs e)
		{
			ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
		}
	}
}