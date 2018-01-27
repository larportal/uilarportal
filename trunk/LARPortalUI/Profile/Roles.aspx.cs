using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Profile
{
	public partial class Roles : System.Web.UI.Page
	{
		DataTable _dtRoles = new DataTable();

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			LoadData();
		}

		protected void LoadData()
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@UserID", Master.UserID);
			sParams.Add("@CampaignID", Master.CampaignID);
			DataSet dsRoles = Classes.cUtilities.LoadDataSet("uspGetPlayerRoles", sParams, "LARPortal", Master.UserName, lsRoutineName);
			_dtRoles = dsRoles.Tables [0];

			DataView dv = new DataView(dsRoles.Tables [0]);
			dv.Sort = "DisplayGroup";
			DataTable distinctValues = dv.ToTable(true, "DisplayGroup");

			rptRoles.DataSource = distinctValues;
			rptRoles.DataBind();

			lblCampName.Text = Master.CampaignName;
		}

		//protected void gvFullRoleList_RowDataBound(object sender, GridViewRowEventArgs e)
		//{
		//    if (e.Row.RowType == DataControlRowType.Header)
		//        e.Row.Cells[7].Text = "<div style='text-align: center;'>Give role to<br>" + hidDisplayName.Value + "</div>";
		//}

		protected void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			DataRowView RptrRow = (DataRowView) e.Item.DataItem;
			DataView dv = new DataView(_dtRoles, "DisplayGroup = '" + RptrRow [0].ToString() + "'", "DisplayDescription", DataViewRowState.CurrentRows);
			string sContents = "";

			foreach (DataRowView dRow in dv)
			{
				sContents += "<li>" + dRow ["RoleDescription"].ToString() + " - " + dRow ["DisplayDescription"].ToString() + "</li>";
			}
			Label lblDesc = (Label) e.Item.FindControl("lblDesc");
			lblDesc.Text = sContents;
		}
	}
}