using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LarpPortal.Classes;

namespace LarpPortal.Campaigns
{
	public partial class AssignRoles : System.Web.UI.Page
	{
		private DataTable _dtRoles = new DataTable();
		private bool _ReloadUser = false;
		private bool _SuperUser = false;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
			btnCloseMessage.Attributes.Add("data-dismiss", "modal");
			if (Session["SuperUser"] != null)
				_SuperUser = true;
			else
				_SuperUser = false;
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				MethodBase lmth = MethodBase.GetCurrentMethod();
				string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

				MasterPage_CampaignChanged(null, null);
			}
		}


		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@UserID", Master.UserID);
			sParams.Add("@CampaignID", Master.CampaignID);
			DataSet dsRoles = cUtilities.LoadDataSet("uspGetFullListPlayerRoles", sParams, "LARPortal", Master.UserName, lsRoutineName);

			int iMaxRoleAssign = 0;
			int iMaxRole = 0;

			if (dsRoles.Tables[0].Rows.Count > 0)
			{
				if (!int.TryParse(dsRoles.Tables[0].Rows[0][0].ToString(), out iMaxRoleAssign))
					iMaxRoleAssign = -1;
				int.TryParse(dsRoles.Tables[0].Rows[0][1].ToString(), out iMaxRole);
			}

			if (_SuperUser)
			{
				// If user is a superuser we are going to include everything.
				iMaxRole = int.MaxValue;
				iMaxRoleAssign = int.MaxValue;
			}

			DataTable dtRoles = new DataTable();
			dtRoles.Columns.Add(new DataColumn("RoleID", typeof(int)));
			dtRoles.Columns.Add(new DataColumn("RoleDesc", typeof(string)));
			dtRoles.Columns.Add(new DataColumn("RoleLevel", typeof(int)));
			dtRoles.Columns.Add(new DataColumn("DisplayGroup", typeof(string)));
			dtRoles.Columns.Add(new DataColumn("DisplayDescription", typeof(string)));
			dtRoles.Columns.Add(new DataColumn("UserHasRole", typeof(bool)));
			dtRoles.Columns.Add(new DataColumn("CampaignPlayerRoleID", typeof(int)));

			// For next two columns need to set up a column variable so we can make it not null and have a default value. Need not null because the front end needs it to display correctly.
			DataColumn dCanAssign = new DataColumn("CanAssign", typeof(Boolean));
			dCanAssign.AllowDBNull = false;
			dCanAssign.DefaultValue = false;
			dtRoles.Columns.Add(dCanAssign);

			DataColumn dPlayerHasRole = new DataColumn("PlayerHasRole", typeof(Boolean));
			dPlayerHasRole.AllowDBNull = false;
			dPlayerHasRole.DefaultValue = false;
			dtRoles.Columns.Add(dPlayerHasRole);

			DataView dvHasRoles = new DataView(dsRoles.Tables[1], "RoleLevel <= " + iMaxRole, "RoleLevel desc", DataViewRowState.CurrentRows);

			foreach (DataRowView dRole in dvHasRoles)
			{
				DataRow dRow = dtRoles.NewRow();
				dRow["RoleID"] = dRole["RoleID"] as int?;
				dRow["RoleDesc"] = dRole["RoleDescription"].ToString();
				dRow["RoleLevel"] = dRole["RoleLevel"] as int?;
				if ((dRole["HasRole"].ToString().StartsWith("1")) ||
					(_SuperUser))
					dRow["UserHasRole"] = true;
				else
					dRow["UserHasRole"] = false;
				if ((dRole["CanAssign"].ToString().StartsWith("1")) ||
					(_SuperUser))
					dRow["CanAssign"] = true;
				else
					dRow["CanAssign"] = false;
				dRow["DisplayGroup"] = dRole["DisplayGroup"].ToString();
				dRow["DisplayDescription"] = dRole["DisplayDescription"].ToString();
				dtRoles.Rows.Add(dRow);
			}

			Session["UserRoles"] = dtRoles;

			if ((iMaxRoleAssign > 0) ||
				(_SuperUser))
			{
				sParams = new SortedList();
				sParams.Add("@CampaignID", Master.CampaignID);
				DataTable dtPlayers = cUtilities.LoadDataTable("uspGetCampaignPlayers", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignPlayers");

				if (dtPlayers.Columns["DisplayValue"] == null)
					dtPlayers.Columns.Add(new DataColumn("DisplayValue", typeof(string), "Convert(PlayerName, 'System.String') + ' - ' + Convert(LoginUserName, 'System.String')"));

				DataView dvPlayers = new DataView(dtPlayers, "", "DisplayValue", DataViewRowState.CurrentRows);
				ddlPlayerSelector.DataSource = dvPlayers;
				ddlPlayerSelector.DataTextField = "DisplayValue";
				ddlPlayerSelector.DataValueField = "CampaignPlayerID";
				ddlPlayerSelector.DataBind();

				if (ddlPlayerSelector.Items.Count > 0)
					ddlPlayerSelector.Items[0].Selected = true;
//				spnPlayer.Visible = true;
				ddlPlayerSelector_SelectedIndexChanged(null, null);
//				gvFullRoleList.Visible = true;
//				gvRoleList.Visible = false;
			}
			else
			{
				//spnPlayer.Visible = false;
				//gvRoleList.DataSource = dtRoles;
				//gvRoleList.DataBind();
				//gvFullRoleList.Visible = false;
				//gvRoleList.Visible = true;
			}
		}

		protected void ddlPlayerSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@CampaignPlayerID", ddlPlayerSelector.SelectedValue);
			sParams.Add("@CampaignID", Master.CampaignID);
			DataSet dsRoles = cUtilities.LoadDataSet("uspGetFullListPlayerRoles", sParams, "LARPortal", Master.UserName, lsRoutineName);

			_dtRoles = Session["UserRoles"] as DataTable;
			foreach (DataRow dRow in _dtRoles.Rows)
			{
				dRow["PlayerHasRole"] = false;
				dRow["CampaignPlayerRoleID"] = DBNull.Value;
			}

			//if (dsRoles.Tables[1].Rows.Count > 0)
			//	hidDisplayName.Value = dsRoles.Tables[1].Rows[0]["PlayerDisplayName"].ToString();
			//else
			//	hidDisplayName.Value = "";

			DataView dvPlayerRoles = new DataView(dsRoles.Tables[1]);

			// If user is not a superuser, limit the roles to only the ones they have.
			if (!_SuperUser)
				dvPlayerRoles.RowFilter = "UserHasRole = 1";
			foreach (DataRowView dRow in dvPlayerRoles)
			{
				DataView dvExisting = new DataView(_dtRoles, "RoleID = " + dRow["RoleID"].ToString(), "", DataViewRowState.CurrentRows);
				if (dvExisting.Count > 0)
				{
					dvExisting[0]["PlayerHasRole"] = true;
					dvExisting[0]["CampaignPlayerRoleID"] = dRow["CampaignPlayerRoleID"].ToString();
				}
			}

			gvFullRoleList.DataSource = _dtRoles;
			gvFullRoleList.DataBind();

			DataView dv = new DataView(_dtRoles);
			// If current user is not a super user, limit the roles.
			if (!_SuperUser)
				dv.RowFilter = "UserHasRole = 1";
			DataTable distinctValues = dv.ToTable(true, "DisplayGroup");

			rptRoles.DataSource = distinctValues;
			rptRoles.DataBind();
		}

		protected void gvFullRoleList_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			//if (e.Row.RowType == DataControlRowType.Header)
			//	e.Row.Cells[7].Text = "<div style='text-align: center;'>Give role to<br>" + hidDisplayName.Value + "</div>";
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				System.Web.UI.HtmlControls.HtmlInputCheckBox swHasRole = (System.Web.UI.HtmlControls.HtmlInputCheckBox) e.Row.FindControl("swHasRole");
				HiddenField hidRoleID = (HiddenField) e.Row.FindControl("hidRoleID");
				HiddenField hidPlayerHasRole = (HiddenField) e.Row.FindControl("hidPlayerHasRole");
				if ((swHasRole != null) &&
					(hidRoleID != null) &&
					(hidPlayerHasRole != null))
				{
					bool bHasrole;
					if (Boolean.TryParse(hidPlayerHasRole.Value, out bHasrole))
						if (bHasrole)
							swHasRole.Checked = true;
						else
							swHasRole.Checked = false;
				}
			}
		}

		protected void btnGiveUserRole_Command(object sender, CommandEventArgs e)
		{
			string[] sValues = e.CommandArgument.ToString().Split(";".ToCharArray());

			SortedList sParams = new SortedList();
			sParams.Add("@UserID", Master.UserID);

			if (sValues[0].Length > 0)
				sParams.Add("@CampaignPlayerRoleID", sValues[0]);
			else
			{
				sParams.Add("@CampaignPlayerRoleID", "-1");
				sParams.Add("@CampaignPlayerID", ddlPlayerSelector.SelectedValue);
			}
			sParams.Add("@RoleID", sValues[1]);
			cUtilities.PerformNonQuery("uspInsUpdCMCampaignPlayerRoles", sParams, "LARPortal", Master.UserName);
			_ReloadUser = true;
		}

		protected void btnRevokeUserRole_Command(object sender, CommandEventArgs e)
		{
			string[] sValues = e.CommandArgument.ToString().Split(";".ToCharArray());

			SortedList sParams = new SortedList();
			sParams.Add("@UserID", Master.UserID);

			sParams.Add("@RecordID", sValues[0]);
			cUtilities.PerformNonQuery("uspDelCMCampaignPlayerRoles", sParams, "LARPortal", Master.UserName);

			_ReloadUser = true;
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{

		}
	}
}