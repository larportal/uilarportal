using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character
{
	public partial class CharCardCustomization : System.Web.UI.Page
	{
		private DataTable _dtPlaces = new DataTable();

		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
			if (!IsPostBack)
			{
				Session.Remove("SkillList");
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			oCharSelect.LoadInfo();
			ShowGrid();
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			oCharSelect.LoadInfo();
			if (oCharSelect.CharacterID.HasValue)
			{
				if (Session["SkillList"] != null)
				{
					DataTable dtSkills = Session["SkillList"] as DataTable;
					bool bTemp;
					foreach (DataRow dRow in dtSkills.Rows)
					{
						SortedList sParams = new SortedList();
						sParams.Add("@CharacterSkillID", dRow["CharacterSkillID"].ToString());
						if (bool.TryParse(dRow["CardDisplayDescription"].ToString(), out bTemp))
							sParams.Add("@CardDisplayDescription", bTemp);
						else
							sParams.Add("@CardDisplayDescription", true);

						if (bool.TryParse(dRow["CardDisplayIncant"].ToString(), out bTemp))
							sParams.Add("@CardDisplayIncant", bTemp);
						else
							sParams.Add("@CardDisplayIncant", true);

						sParams.Add("@PlayerDescription", dRow["PlayerDescription"].ToString());
						sParams.Add("@PlayerIncant", dRow["PlayerIncant"].ToString());
						sParams.Add("@UserID", Master.UserID);

						Classes.cUtilities.PerformNonQuery("uspInsUpdCHCharacterSkills", sParams, "LARPortal", Master.UserName);
					}

					lblmodalMessage.Text = "Character " + oCharSelect.CharacterInfo.AKA + " has been saved.";
					btnCloseMessage.Attributes.Add("data-dismiss", "modal");
					ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
				}
			}
		}


		protected void gvSkills_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
		{
			gvSkills.EditIndex = -1;
			ShowGrid();
		}

		protected void gvSkills_RowEditing(object sender, GridViewEditEventArgs e)
		{
			gvSkills.EditIndex = e.NewEditIndex;
			ShowGrid();
		}

		protected void gvSkills_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{
			try
			{
				CheckBox cbDisplayDesc = (CheckBox) gvSkills.Rows[e.RowIndex].FindControl("cbDisplayDesc");
				CheckBox cbDisplayIncant = (CheckBox) gvSkills.Rows[e.RowIndex].FindControl("cbDisplayIncant");
				TextBox tbPlayDesc = (TextBox) gvSkills.Rows[e.RowIndex].FindControl("tbPlayDesc");
				TextBox tbPlayIncant = (TextBox) gvSkills.Rows[e.RowIndex].FindControl("tbPlayIncant");
				HiddenField hidSkillID = (HiddenField) gvSkills.Rows[e.RowIndex].FindControl("hidSkillID");

				if (Session["SkillList"] != null)
				{
					DataTable dtSkills = Session["SkillList"] as DataTable;
					DataView dvRow = new DataView(dtSkills, "CharacterSkillID = " + hidSkillID.Value, "", DataViewRowState.CurrentRows);
					foreach (DataRowView dRow in dvRow)
					{
						if (cbDisplayDesc.Checked)
							dRow["CardDisplayDescription"] = true;
						else
							dRow["CardDisplayDescription"] = false;
						if (cbDisplayIncant.Checked)
							dRow["CardDisplayIncant"] = true;
						else
							dRow["CardDisplayIncant"] = false;
						dRow["PlayerDescription"] = tbPlayDesc.Text;
						dRow["PlayerIncant"] = tbPlayIncant.Text;
					}
					Session["SkillList"] = dtSkills;
				}
			}
			catch (Exception ex)
			{
				string l = ex.Message;
			}

			gvSkills.EditIndex = -1;
			ShowGrid();
		}

		protected void gvSkills_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				if ((e.Row.RowState & DataControlRowState.Edit) > 0)
				{
					DataRowView dRow = e.Row.DataItem as DataRowView;

					string t = dRow["CardDisplayIncant"].ToString();
				}
			}
		}

		private void ShowGrid()
		{
			oCharSelect.LoadInfo();

			if (Session["SkillList"] == null)
			{
				if (oCharSelect.CharacterID.HasValue)
				{
					DataTable dtCharactersForCampaign = new DataTable();
					SortedList sParam = new SortedList();
					sParam.Add("@CharacterID", oCharSelect.CharacterID.Value);
					dtCharactersForCampaign = Classes.cUtilities.LoadDataTable("uspGetCharacterSkillSet", sParam, "LARPortal", Master.UserName, "");

					bool bTemp = false;

					foreach (DataRow dRow in dtCharactersForCampaign.Rows)
					{
						if (!bool.TryParse(dRow["CardDisplayDescription"].ToString(), out bTemp))
							dRow["CardDisplayDescription"] = true;
						if (!bool.TryParse(dRow["CardDisplayIncant"].ToString(), out bTemp))
							dRow["CardDisplayIncant"] = true;
					}

					Session["SkillList"] = dtCharactersForCampaign;
				}
			}
			DataTable dtSkills = Session["SkillList"] as DataTable;

			DataView dvSkills = new DataView(dtSkills, "", "DisplayOrder", DataViewRowState.CurrentRows);
			gvSkills.DataSource = dvSkills;
			gvSkills.DataBind();
		}

		protected void oCharSelect_CharacterChanged(object sender, EventArgs e)
		{
			oCharSelect.LoadInfo();

			if (oCharSelect.CharacterInfo != null)
			{
				if (oCharSelect.CharacterID.HasValue)
				{
					Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
					UserInfo.LastLoggedInCampaign = oCharSelect.CharacterInfo.CampaignID;
					UserInfo.LastLoggedInCharacter = oCharSelect.CharacterID.Value;
					UserInfo.LastLoggedInMyCharOrCamp = (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters ? "M" : "C");
					UserInfo.Save();
					Master.ChangeSelectedCampaign();
				}
				Session.Remove("SkillList");
			}
		}

		protected void gvSkills_DataBound(object sender, EventArgs e)
		{
			oCharSelect.LoadInfo();

			if ((oCharSelect.CharacterInfo.CharacterType != 1) && (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters))
			{
				btnSave.Enabled = false;
				int NumColumns = gvSkills.Columns.Count;
				if (NumColumns > 0)
					gvSkills.Columns[NumColumns - 1].Visible = false;
			}
			else
			{
				btnSave.Enabled = true;
				int NumColumns = gvSkills.Columns.Count;
				if (NumColumns > 0)
					gvSkills.Columns[NumColumns - 1].Visible = true;
			}
		}

		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			string t = sender.GetType().ToString();
			oCharSelect.Reset();
		}
	}
}