using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character
{
	public partial class SkillSets : System.Web.UI.Page
	{
		private bool _Reload = false;

		private DataTable _dtSkillSetTypes = new DataTable();

		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			//			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			_dtSkillSetTypes = Classes.cUtilities.LoadDataTable("uspGetSkillSetTypes", sParams,
				"LARPortal", Master.UserName, lsRoutineName + ".uspGetSkillSetTypes");
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			SortedList slParameters = new SortedList();
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			if ((!IsPostBack) ||
				(_Reload))
			{
				slParameters.Add("@intUserID", Session["UserID"].ToString());

				DataTable dtCharacters = LarpPortal.Classes.cUtilities.LoadDataTable("uspGetCharacterIDsByUserID", slParameters,
					"LARPortal", "Character", lsRoutineName + ".uspGetCharacterIDsByUserID");

				gvSkills.DataSource = dtCharacters;
				gvSkills.DataBind();
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			HiddenField hidSkillID = new HiddenField();
			HiddenField hidCharacterID = new HiddenField();
			HiddenField hidCampaignID = new HiddenField();
			DropDownList ddlSkillSetType = new DropDownList();
			HiddenField hidSkillSetTypeID = new HiddenField();
			HiddenField hidSkillSetName = new HiddenField();

			TextBox tbSkillSetName = new TextBox();
			Label lblDuplicate = new Label();
			Label lblMultiplePrimary = new Label();

			DataTable dtValues = new DataTable();
			dtValues.Columns.Add("RowID", typeof(int));
			dtValues.Columns.Add("SkillSetID", typeof(string));
			dtValues.Columns.Add("SkillSetName", typeof(string));
			dtValues.Columns.Add("OrigSkillSetName", typeof(string));
			dtValues.Columns.Add("CharacterID", typeof(string));
			dtValues.Columns.Add("CampaignID", typeof(string));
			dtValues.Columns.Add("SkillSetType", typeof(string));
			dtValues.Columns.Add("OrigSkillSetTypeID", typeof(string));
			dtValues.Columns.Add("Duplicate", typeof(string));
			dtValues.Columns.Add("TooManyPrimary", typeof(string));

			oCharSelect.Reset();

			int iRowCounter = 0;
			foreach (GridViewRow gRow in gvSkills.Rows)
			{
				hidSkillID = (HiddenField) gRow.FindControl("hidSkillID");
				hidCharacterID = (HiddenField) gRow.FindControl("hidCharacterID");
				hidCampaignID = (HiddenField) gRow.FindControl("hidCampaignID");
				ddlSkillSetType = (DropDownList) gRow.FindControl("ddlSkillSetType");
				hidSkillSetTypeID = (HiddenField) gRow.FindControl("hidSkillSetTypeID");
				tbSkillSetName = (TextBox) gRow.FindControl("tbSkillSetName");
				hidSkillSetName = (HiddenField) gRow.FindControl("hidSkillSetName");
				lblDuplicate = (Label) gRow.FindControl("lblDuplicate");
				if (lblDuplicate != null)
					lblDuplicate.Visible = false;
				lblMultiplePrimary = (Label) gRow.FindControl("lblMultiplePrimary");

				if ((hidSkillID != null) &&
					(tbSkillSetName != null) &&
					(hidSkillSetName != null) &&
					(hidCharacterID != null) &&
					(hidCampaignID != null) &&
					(ddlSkillSetType != null) &&
					(hidSkillSetTypeID != null))
				{
					DataRow dNewRow = dtValues.NewRow();
					dNewRow["RowID"] = iRowCounter++;
					dNewRow["SkillSetID"] = hidSkillID.Value;
					dNewRow["SkillSetName"] = tbSkillSetName.Text;
					dNewRow["OrigSkillSetName"] = hidSkillSetName.Value;
					dNewRow["CharacterID"] = hidCharacterID.Value;
					dNewRow["CampaignID"] = hidCampaignID.Value;
					dNewRow["SkillSetType"] = ddlSkillSetType.SelectedItem.Value;
					dNewRow["OrigSkillSetTypeID"] = hidSkillSetTypeID.Value;
					dNewRow["Duplicate"] = "N";
					dNewRow["TooManyPrimary"] = "N";
					dtValues.Rows.Add(dNewRow);
				}
			}

			string sRowID = "";
			string sSkillSetID = "";
			string sSkillSetName = "";
			string sCharacterID = "";
			string sCampaignID = "";
			string sSkillSetType = "";
			List<string> DupSkillNames = new List<string>();
			List<string> PrimaryDups = new List<string>();

			if (dtValues.Rows.Count > 0)
			{
				bool bDupSkillSetName = false;
				bool bTooManyPrimary = false;

				for (int iCount = 0; iCount < dtValues.Rows.Count; iCount++)
				{
					sRowID = iCount.ToString();
					sSkillSetID = dtValues.Rows[iCount]["SkillSetID"].ToString();
					sSkillSetName = dtValues.Rows[iCount]["SkillSetName"].ToString();
					sCharacterID = dtValues.Rows[iCount]["CharacterID"].ToString();
					sCampaignID = dtValues.Rows[iCount]["CampaignID"].ToString();
					sSkillSetType = dtValues.Rows[iCount]["SkillSetType"].ToString();

					// First check to make sure that character/campaignID/SkillSetID are unique;
					string sRowFilter = string.Format("RowID <> '{0}' and CharacterID = '{1}' and CampaignID = '{2}' and SkillSetName = '{3}'",
						sRowID, sCharacterID, sCampaignID, sSkillSetName.Replace("'", "''"));
					DataView dvRows = new DataView(dtValues, sRowFilter, "", DataViewRowState.CurrentRows);
					if (dvRows.Count > 0)
					{
						dtValues.Rows[iCount]["Duplicate"] = "Y";
						lblDuplicate = (Label) gvSkills.Rows[iCount].FindControl("lblDuplicate");
						if (lblDuplicate != null)
						{
							lblDuplicate.Visible = true;
						}

						foreach (DataRowView dvRow in dvRows)
						{
							dvRow["Duplicate"] = "Y";
							DupSkillNames.Add(sSkillSetID);
						}
						bDupSkillSetName = true;
					}
					if (sSkillSetType == "4")
					{
						sRowFilter = string.Format("CharacterID = '{0}' and CampaignID = '{1}' and SkillSetType = '4'", sCharacterID, sCampaignID);
						dvRows = new DataView(dtValues, sRowFilter, "", DataViewRowState.CurrentRows);
						if (dvRows.Count > 1)
						{
							foreach (DataRowView dvRow in dvRows)
							{
								dvRow["TooManyPrimary"] = true;
								PrimaryDups.Add(sSkillSetID);
							}
							bTooManyPrimary = true;
						}
					}
				}

				// If we got this far, it means everything is OK.
				foreach (GridViewRow dRow in gvSkills.Rows)
				{
					dRow.BackColor = System.Drawing.Color.White;
					sRowID = "";
					sSkillSetID = "";
					sSkillSetName = "";
					sCharacterID = "";
					sCampaignID = "";
					sSkillSetType = "";
					lblDuplicate = (Label) dRow.FindControl("lblDuplicate");
					if (lblDuplicate != null)
						lblDuplicate.Visible = false;

					lblMultiplePrimary = (Label) dRow.FindControl("lblMultiplePrimary");
					if (lblMultiplePrimary != null)
						lblMultiplePrimary.Visible = false;

					hidSkillID = (HiddenField) dRow.FindControl("hidSkillID");
					if (hidSkillID != null)
					{
						if (DupSkillNames.Contains(hidSkillID.Value))
						{
							dRow.BackColor = System.Drawing.Color.Pink;
							lblDuplicate.Visible = true;
						}

						if (PrimaryDups.Contains(hidSkillID.Value))
						{
							dRow.BackColor = System.Drawing.Color.Pink;
							lblMultiplePrimary.Visible = true;
						}

						//DataView dvRows = new DataView(dtValues, "CharacterSkillSetID = '" + hidSkillID.Value + "'", "", DataViewRowState.CurrentRows);
						//if (dvRows.Count > 0)
						//{
						//	if ((dvRows[0]["Duplicate"].ToString() == "Y") ||
						//		(dvRows[0]["TooManyPrimary"].ToString() == "Y"))
						//	{
						//		dRow.BackColor = System.Drawing.Color.Pink;
						//	}

						//}
					}
				}

				lblProblem.Text = "";

				lblProblem.Visible = true;
				if ((bDupSkillSetName) ||
					 (bTooManyPrimary))
				{
					if (bDupSkillSetName)
					{
						if (bTooManyPrimary)
						{
							lblProblem.Text = "There are problems - duplicate skills and too many primary characters.";
							return;
						}
						else
						{
							lblProblem.Text = "There are problems - there are duplicate skills names.";
							return;
						}
					}
					else
					{
						if (bTooManyPrimary)
						{
							lblProblem.Text = "There are problems - there are too many primary characters.";
							return;
						}
					}
				}
				else
				{
					string sRowFilter = "SkillSetName <> OrigSkillSetName or SkillSetType <> OrigSkillSetTypeID";
					DataView dvChangedRow = new DataView(dtValues, sRowFilter, "", DataViewRowState.CurrentRows);
					foreach (DataRowView dRow in dvChangedRow)
					//foreach (DataRow dRow in dtValues.Rows)
					{
						SortedList sParams = new SortedList();
						sParams.Add("@CharacterSkillSetID", dRow["SkillSetID"].ToString());
						sParams.Add("@SkillSetName", dRow["SkillSetName"].ToString());
						sParams.Add("@CharacterSkillSetTypeID", dRow["SkillSetType"].ToString());
						sParams.Add("@UserID", Master.UserID);
						Classes.cUtilities.PerformNonQuery("uspInsUpdCHCharacterSkillSets", sParams, "LARPortal", Master.UserName);
					}
					lblmodalMessage.Text = "The skill sets have been saved.";
					lblmodalMessage.Visible = true;
					btnCloseMessage.Attributes.Add("data-dismiss", "modal");
					ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
				}
			}
		}

		protected void btnDeleteSkillSet_Command(object sender, CommandEventArgs e)
		{
			lblDeleteSkillSetMessage.Text = "Are you sure you want to delete the skill set " + e.CommandName + " ?";
			lblDeleteSkillSetMessage.Visible = true;
			hidDeleteSkillSetID.Value = e.CommandArgument.ToString();
			btnDeleteCancel.Attributes.Add("data-dismiss", "modal");
			oCharSelect.Reset();
			ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openDeleteSkillSet();", true);
		}

		protected void gvSkills_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				TextBox tbSkillSetName = (TextBox) e.Row.FindControl("tbSkillSetName");
				if (tbSkillSetName != null)
					tbSkillSetName.Attributes.Add("onChange", "changeBackColor(this);");
				DropDownList ddlSkillSetType = (DropDownList) e.Row.FindControl("ddlSkillSetType");
				HiddenField hidSkillSetTypeID = (HiddenField) e.Row.FindControl("hidSkillSetTypeID");
				HiddenField hidCampaignID = (HiddenField) e.Row.FindControl("hidCampaignID");
				if ((ddlSkillSetType != null) &&
					(hidSkillSetTypeID != null) &&
					(hidCampaignID != null))
				{
					DataView dvSkills = new DataView(_dtSkillSetTypes, "CampaignID = " + hidCampaignID.Value, "SkillSetSortOrder", DataViewRowState.CurrentRows);
					ddlSkillSetType.DataTextField = "SkillSetTypeDescription";
					ddlSkillSetType.DataValueField = "CampaignSkillSetTypeID";
					ddlSkillSetType.DataSource = dvSkills;
					ddlSkillSetType.DataBind();
					if (!string.IsNullOrEmpty(hidSkillSetTypeID.Value))
					{
						foreach (ListItem lItem in ddlSkillSetType.Items)
						{
							if (hidSkillSetTypeID.Value == lItem.Value)
							{
								ddlSkillSetType.ClearSelection();
								lItem.Selected = true;
							}
						}
					}
				}
			}
		}

		protected void btnDeleteSkillSet_Click(object sender, EventArgs e)
		{
			SortedList sParams = new SortedList();
			sParams.Add("@RecordID", hidDeleteSkillSetID.Value);
			sParams.Add("@UserID", Master.UserID);
			Classes.cUtilities.PerformNonQuery("uspDelCHCharacterSkillSets", sParams, "LARPortal", Master.UserName);
			lblmodalMessage.Text = "The skill sets have been deleted.";
			lblmodalMessage.Visible = true;
			btnCloseMessage.Attributes.Add("data-dismiss", "modal");
			oCharSelect.Reset();
			ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
			_Reload = true;
			oCharSelect.Reset();
		}
	}
}
