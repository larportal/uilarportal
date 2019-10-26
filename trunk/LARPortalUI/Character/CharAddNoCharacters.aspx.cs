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
	public partial class CharAddNoCharacters : System.Web.UI.Page
	{
		bool _Reload = false;
//		bool _Redirect = false;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				tbCharacterName.Attributes.Add("PlaceHolder", "The nickname for the character.");
				//cbNewCharacter.Attributes.Add("onclick", "RadioButtonChanged();");
				//cbNewCampaign.Attributes.Add("onclick", "RadioButtonChanged();");
			}

			btnCloseMessage.Attributes.Add("data-dismiss", "modal");
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			if ((!IsPostBack) ||
				(_Reload))
			{
				SortedList slParameters = new SortedList();
				slParameters.Add("@intUserID", Session["UserID"].ToString());
				//                oLogWriter.AddLogMessage("About to run uspGetCharacterIDsByUserID", lsRoutineName, "", Session.SessionID);
				DataTable dtCharacters = LarpPortal.Classes.cUtilities.LoadDataTable("uspGetCharacterIDsByUserID", slParameters,
					"LARPortal", Master.UserName, lsRoutineName + ".uspGetCharacterIDsByUserID");


				ddlUserCampaigns.SelectedIndex = 0;
				ddlUserCampaigns.Items.Clear();
				Classes.cUserCampaigns CampaignChoices = new Classes.cUserCampaigns();
				CampaignChoices.Load(Master.UserID);

				if (CampaignChoices.CountOfUserCampaigns == 0)
				{
					mvCharacterCreate.SetActiveView(vwNoCampaigns);
				}
				else
				{
					DataView dvCampaigns = new DataView(Classes.cUtilities.CreateDataTable(CampaignChoices.lsUserCampaigns), "", "CampaignName", DataViewRowState.CurrentRows);
					ddlUserCampaigns.DataTextField = "CampaignName";
					ddlUserCampaigns.DataValueField = "CampaignID";
					ddlUserCampaigns.DataSource = dvCampaigns;
					ddlUserCampaigns.DataBind();
					foreach (ListItem liItem in ddlUserCampaigns.Items)
					{
						if (liItem.Value == Master.CampaignID.ToString())
						{
							ddlUserCampaigns.ClearSelection();
							liItem.Selected = true;
						}
					}

					ddlUserCampaigns_SelectedIndexChanged(null, null);

					//SortedList sParams = new SortedList();
					//sParams.Add("@UserID", Master.UserID);
					//DataSet dsCharList = Classes.cUtilities.LoadDataSet("uspGetUniqueCharacterIDsByUserID", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetCharacterList");

					//DataView dvCharList = new DataView(dsCharList.Tables[0]);
					//dvCharList.Sort = "CharacterAKA, CharacterID";
					//DataTable distinctValues = dvCharList.ToTable(true, "CharacterID", "CharacterAKA");
					//ddlCampaignCharacter.DataSource = distinctValues;
					//ddlCampaignCharacter.DataTextField = "CharacterAKA";
					//ddlCampaignCharacter.DataValueField = "CharacterID";
					//ddlCampaignCharacter.DataBind();


					//sParams = new SortedList();
					//sParams.Add("@intUserID", Master.UserID);
					//DataSet dsFullCharList = Classes.cUtilities.LoadDataSet("uspGetCharacterIDsByUserID", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetCharacterList");
					//DataView dvSkillList = new DataView(dsFullCharList.Tables[0], "", "DisplayName", DataViewRowState.CurrentRows);
					//ddlSkillSetCharacter.DataSource = dvSkillList;
					//ddlSkillSetCharacter.DataTextField = "DisplayName";
					//ddlSkillSetCharacter.DataValueField = "CharacterSkillSetID";
					//ddlSkillSetCharacter.DataBind();

					//sParams = new SortedList();
					//sParams.Add("@UserID", Master.UserID);
					//DataSet dsCharacterListWithShares = Classes.cUtilities.LoadDataSet("uspGetCharactersByUserIDWithShares", sParams,
					//	"LARPortal", Master.UserName, lsRoutineName + ".uspGetCharactersByUserIDWithShares");
					//DataView dvUniqueCharList = new DataView(dsCharacterListWithShares.Tables[0], "", "CharacterAKA", DataViewRowState.CurrentRows);

					//ddlJoinCampaign.DataSource = dvUniqueCharList;
					//ddlJoinCampaign.DataTextField = "CharacterAKA";
					//ddlJoinCampaign.DataValueField = "CharacterID";
					//ddlJoinCampaign.DataBind();
				}
			}
			//if (_Redirect)
			//	Response.Redirect("CharAdd.aspx", true);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			if (tbCharacterName.Text.Trim().Length > 0)
			{
				// Reset the user control so it will load the characters the next time it's used.
				//oCharSelect.Reset();

				SortedList sParam = new SortedList();
				sParam.Add("@CampaignID", ddlUserCampaigns.SelectedValue);
				sParam.Add("@CharacterAKA", tbCharacterName.Text.Trim());
				sParam.Add("@RoleAlignmentID", ddlCharacterType.SelectedValue);
				sParam.Add("@UserID", Master.UserID);
				if (ddlCharacterType.SelectedValue != "1")      // Non PC Character.
					sParam.Add("@CurrentUserID", ddlPlayer.SelectedValue);
				sParam.Add("@SkillSetTypeID", ddlNewCharSkillSetType.SelectedValue);
				sParam.Add("@SkillSetName", tbNewCharSkillSetName.Text);
				DataTable dtCharInfo = new DataTable();
				dtCharInfo = Classes.cUtilities.LoadDataTable("uspInsCreateNewCharacter", sParam, "LARPortal", Master.UserName, lsRoutineName);

				if (dtCharInfo.Rows.Count > 0)
				{
					if (ddlCharacterType.SelectedValue == "1")
					{
						string sCharId = dtCharInfo.Rows[0]["CharacterID"].ToString();
						int iCharID;
						int iCampaignID;
						if ((int.TryParse(sCharId, out iCharID)) &&
							(int.TryParse(ddlUserCampaigns.SelectedValue, out iCampaignID)))
						{
							Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "Password", Session.SessionID);
							UserInfo.LastLoggedInCharacter = iCharID;
							UserInfo.LastLoggedInCampaign = iCampaignID;
							UserInfo.Save();
						}
						//oCharSelect.Reset();
						//Response.Redirect("CharInfo.aspx", true);
						//_Reload = true;
						lblCharAdded.Text = "The character has been created.";
						ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "openCharAdd();", true);
						return;
						//lblMessage.Text = "The character has been created.";
						//lblmodalMessage.Text = "The character has been created.";
						//ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
						//ResetScreen();
					}
					else
					{
						//Response.Redirect("CharInfo.aspx", true);
						//tbCharacterName.Text = "";
						//ddlPlayer.SelectedIndex = 0;
						//lblMessage.Text = "The character has been created.";
						//lblmodalMessage.Text = "The character has been created.";
						//ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
						lblCharAdded.Text = "The character has been created.";
						//oCharSelect.Reset();
						ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "openCharAdd();", true);
						//_Reload = true;
						ResetScreen();
						return;
					}
				}
				else
				{
					lblmodalMessage.Text = "There a problem saving the character. The technical staff has been contacted.";
					ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
				}
			}
		}

		protected void ddlUserCampaigns_SelectedIndexChanged(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@UserID", Master.UserID);
			sParams.Add("@CampaignID", ddlUserCampaigns.SelectedValue);

			DataTable dtEditCampaigns = Classes.cUtilities.LoadDataTable("uspGetPrivCampaignCharacterEdit", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetPrivs");
			DataView dvEditCampaigns = new DataView(dtEditCampaigns, "CampaignID = " + ddlUserCampaigns.SelectedValue, "", DataViewRowState.CurrentRows);

			sParams = new SortedList();
			sParams.Add("@UserID", Master.UserID);
			sParams.Add("@CampaignID", ddlUserCampaigns.SelectedValue);
			DataTable dtCampaignRoles = Classes.cUtilities.LoadDataTable("uspGetUserCampaignRoleInfo", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetRoleInfo");

			sParams = new SortedList();
			sParams.Add("@CampaignID", ddlUserCampaigns.SelectedValue);
			DataSet dsCampaignSkillSetTypes = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillSetTypes", sParams, "LARPortal",
				Master.UserName, lsRoutineName + ".uspGetCharactersByUserIDWithShares");
			DataView dlSkillSetType = new DataView(dsCampaignSkillSetTypes.Tables[0], "", "SortOrder", DataViewRowState.CurrentRows);
			ddlNewCharSkillSetType.DataSource = dlSkillSetType;
			ddlNewCharSkillSetType.DataTextField = "SkillSetTypeDescription";
			ddlNewCharSkillSetType.DataValueField = "CampaignSkillSetTypeID";
			ddlNewCharSkillSetType.DataBind();

			int PCCount = dtCampaignRoles.Select("RoleDescription LIKE '%PC%' and RoleAlignmentDesc = 'PC'").Length;

			ddlCharacterType.ClearSelection();

			if (dvEditCampaigns.Count == 0)
			{
				divPlayer.Visible = false;
				if (PCCount > 0)
				{
					ddlCharacterType.SelectedValue = "1";
					divPlayerType.Visible = false;
				}
			}
			else
			{
				// Only go get the players if the user can choose type. Do this here because we only need to do it once.
				divPlayerType.Visible = true;
				divPlayer.Visible = true;

				sParams = new SortedList();
				sParams.Add("@CampaignID", ddlUserCampaigns.SelectedValue);
				DataTable dtNPCPlayers = Classes.cUtilities.LoadDataTable("uspGetCampaignNPCPlayers", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetPlayers");

				if (dtNPCPlayers.Columns["DisplayValue"] == null)
					dtNPCPlayers.Columns.Add(new DataColumn("DisplayValue", typeof(string)));

				foreach (DataRow dRow in dtNPCPlayers.Rows)
				{
					if (dRow["NickName"].ToString().Length > 0)
						dRow["DisplayValue"] = dRow["NickName"].ToString();
					else
						dRow["DisplayValue"] = dRow["FirstName"].ToString();
					dRow["DisplayValue"] += " " + dRow["LastName"].ToString() + " - " + dRow["loginUserName"].ToString();
				}
				DataView dvNPCPlayers = new DataView(dtNPCPlayers, "", "LastName, FirstName, LoginUserName", DataViewRowState.CurrentRows);
				ddlPlayer.DataSource = dvNPCPlayers;
				ddlPlayer.DataTextField = "DisplayValue";
				ddlPlayer.DataValueField = "UserID";
				ddlPlayer.DataBind();
				ddlPlayer.Items.Insert(0, new ListItem("Select Player...", ""));
				ddlPlayer.ClearSelection();
				ddlPlayer.Items[0].Selected = true;
				ddlCharacterType_SelectedIndexChanged(null, null);
			}
		}

		protected void ddlCharacterType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlCharacterType.SelectedValue == "2")
				divPlayer.Visible = true;
			else
				divPlayer.Visible = false;
		}

		//protected void btnSkillSetSave_Click(object sender, EventArgs e)
		//{
		//	MethodBase lmth = MethodBase.GetCurrentMethod();
		//	string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

		//	oCharSelect.Reset();

		//	SortedList sParams = new SortedList();
		//	sParams.Add("@CharacterSkillSetID", ddlSkillSetCharacter.SelectedValue);
		//	DataTable dtSkillSets = Classes.cUtilities.LoadDataTable("uspGetCharacterSkillSets", sParams, "LARPortal", Master.UserName,
		//		lsRoutineName + ".uspGetCharacterSkillSets");

		//	string sFilter = "CharacterSkillSetTypeID = " + ddlNewSkillSetType.SelectedValue + " and AllowMultiples = 0";
		//	DataView dvSkillSets = new DataView(dtSkillSets, sFilter, "", DataViewRowState.CurrentRows);
		//	if (dvSkillSets.Count > 0)
		//	{
		//		//Response.Redirect("CharInfo.aspx", true);
		//		string sCampaign = dvSkillSets[0]["CampaignName"].ToString();
		//		lblmodalMessage.Text = "The campaign " + sCampaign + " does not allow multiple skill sets of type " +
		//			dvSkillSets[0]["SkillSetTypeDescription"].ToString() + ". Please change the type and try to save again.";
		//		lblMessage.Text = lblmodalMessage.Text;
		//		oCharSelect.Reset();
		//		ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
		//		//ResetScreen();
		//		return;
		//	}

		//	sFilter = string.Format("SkillSetName = '{0}'", tbNewSkillSetName.Text);
		//	dvSkillSets = new DataView(dtSkillSets, sFilter, "", DataViewRowState.CurrentRows);
		//	if (dvSkillSets.Count > 0)
		//	{
		//		lblmodalMessage.Text = "There is already a skill set with the name " + tbNewSkillSetName.Text + ". Please change the name and try to save again.";
		//		lblMessage.Text = lblmodalMessage.Text;
		//		oCharSelect.Reset();
		//		ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
		//		ResetScreen();
		//		return;
		//	}

		//	sParams = new SortedList();

		//	sParams.Add("@SkillSetName", tbNewSkillSetName.Text);
		//	sParams.Add("@OriginalSkillSetID", ddlSkillSetCharacter.SelectedValue);
		//	sParams.Add("@SkillSetTypeID", ddlNewSkillSetType.SelectedValue);
		//	DataSet dsResults = new DataSet();
		//	dsResults = Classes.cUtilities.LoadDataSet("uspAddSkillSet", sParams, "LARPortal", Master.UserName, lsRoutineName);

		//	if (dsResults.Tables.Count > 0)
		//	{
		//		if (dsResults.Tables[0].Rows.Count > 0)
		//		{
		//			DataRow dNewRow = dsResults.Tables[0].Rows[0];
		//			int iCampaignID;
		//			int iCharacterID;
		//			int iSkillSetID;

		//			Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "Password", Session.SessionID);

		//			if ((int.TryParse(dNewRow["CampaignID"].ToString(), out iCampaignID)) &&
		//				(int.TryParse(dNewRow["CharacterID"].ToString(), out iCharacterID)) &&
		//				(int.TryParse(dNewRow["CharacterSkillSetID"].ToString(), out iSkillSetID)))
		//			{
		//				UserInfo.LastLoggedInCampaign = iCampaignID;
		//				UserInfo.LastLoggedInCharacter = iCharacterID;
		//				UserInfo.LastLoggedInSkillSetID = iSkillSetID;
		//			}

		//			UserInfo.Save();
		//			//Response.Redirect("CharInfo.aspx", true);
		//			//tbNewSkillSetName.Text = "";
		//			oCharSelect.Reset();
		//			lblCharAdded.Text = "The character skill set has been created.";
		//			oCharSelect.Reset();
		//			ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "openCharAdd();", true);
		//			return;
		//			//lblMessage.Text = "The character skill set has been created.";
		//			//lblmodalMessage.Visible = true;
		//			//lblmodalMessage.Text = "The character skill set has been created.";
		//			//ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
		//			//ResetScreen();
		//			//_Reload = true;
		//		}
		//	}
		//}

		//protected void ddlCampaignCharacter_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//	MethodBase lmth = MethodBase.GetCurrentMethod();
		//	string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

		//	// Going and getting the list of characters but more importantly getting the list of shares.
		//	SortedList sParams = new SortedList();
		//	sParams.Add("@UserID", Master.UserID);
		//	DataSet dsCharacterListWithShares = Classes.cUtilities.LoadDataSet("uspGetCharactersByUserIDWithShares", sParams,
		//		"LARPortal", Master.UserName, lsRoutineName + ".uspGetCharactersByUserIDWithShares");

		//	int iCharacterID = 0;
		//	int iOrigCampaignID = 0;

		//	if (int.TryParse(ddlCampaignCharacter.SelectedValue, out iCharacterID))
		//	{
		//		DataView dvOrigCampaign = new DataView(dsCharacterListWithShares.Tables[0], "CharacterID = " + iCharacterID.ToString(),
		//			"", DataViewRowState.CurrentRows);
		//		if (dvOrigCampaign.Count > 0)
		//		{
		//			if (int.TryParse(dvOrigCampaign[0]["OriginalCampaignID"].ToString(), out iOrigCampaignID))
		//			{
		//				// Got the number of the original campaign the character was created in. Now need to go see what campaigns that has shares with.
		//				DataView dvShareList = new DataView(dsCharacterListWithShares.Tables[1], "FromCampID = " + iOrigCampaignID.ToString(),
		//					"ToCampName", DataViewRowState.CurrentRows);
		//				if (dvShareList.Count > 0)
		//				{
		//					ddlJoinCampaign.DataTextField = "ToCampName";
		//					ddlJoinCampaign.DataValueField = "ToCampID";
		//					ddlJoinCampaign.DataSource = dvShareList;
		//					ddlJoinCampaign.DataBind();
		//					ddlJoinCampaign.Visible = true;
		//					ddlJoinCampaign.Enabled = true;
		//					divJoinCampaign.Visible = true;
		//					divNoCampaigns.Visible = false;
		//					divJoinSkillSetName.Visible = true;
		//					divJoinSkillSetType.Visible = true;
		//					ddlJoinCampaign_SelectedIndexChanged(null, null);
		//				}
		//				else
		//				{
		//					divJoinCampaign.Visible = false;
		//					divNoCampaigns.Visible = true;
		//					divJoinSkillSetName.Visible = false;
		//					divJoinSkillSetType.Visible = false;
		//					//ddlJoinCampaign.Visible = false;
		//				}
		//			}
		//		}
		//	}
		//	sParams = new SortedList();
		//	sParams.Add("@CampaignID", ddlUserCampaigns.SelectedValue);
		//	DataSet dsCampaignSkillSetTypes = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillSetTypes", sParams, "LARPortal",
		//		Master.UserName, lsRoutineName + ".uspGetCharactersByUserIDWithShares");
		//	ddlNewCharSkillSetType.DataSource = dsCampaignSkillSetTypes.Tables[0];
		//	ddlNewCharSkillSetType.DataTextField = "SkillSetTypeDescription";
		//	ddlNewCharSkillSetType.DataValueField = "CampaignSkillSetTypeID";
		//	ddlNewCharSkillSetType.DataBind();
		//}

		//protected void RadioButtonCheckedChanged(object sender, EventArgs e)
		//{
		//	pnlAddCampaign.Visible = false;
		//	pnlAddCharacter.Visible = false;
		//	pnlAddSkillSet.Visible = false;

		//	if (rbNewCharacter.Checked)
		//	{
		//		pnlAddCharacter.Visible = true;
		//		ddlCampaignCharacter_SelectedIndexChanged(null, null);
		//	}
		//	else if (rbNewSkillSet.Checked)
		//	{
		//		pnlAddSkillSet.Visible = true;
		//		ddlSkillSetCharacter_SelectedIndexChanged(null, null);
		//	}
		//	else if (rbNewCampaign.Checked)
		//	{
		//		pnlAddCampaign.Visible = true;
		//		ddlCampaignCharacter_SelectedIndexChanged(null, null);
		//	}
		//}

		//protected void ddlSkillSetCharacter_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//	MethodBase lmth = MethodBase.GetCurrentMethod();
		//	string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

		//	SortedList sParams = new SortedList();
		//	sParams.Add("@SkillSetID", ddlSkillSetCharacter.SelectedValue);
		//	DataTable dtCampaign = Classes.cUtilities.LoadDataTable("uspGetCampaignSkillSetTypesFromSkillSetID", sParams, "LARPortal", Master.UserName,
		//		lsRoutineName + ".uspGetCampaignSkillSetTypesFromSkillSetID");
		//	ddlNewSkillSetType.DataSource = dtCampaign;
		//	ddlNewSkillSetType.DataTextField = "SkillSetTypeDescription";
		//	ddlNewSkillSetType.DataValueField = "CampaignSkillSetTypeID";
		//	ddlNewSkillSetType.DataBind();
		//}





		//protected void btnJoinCampaign_Click(object sender, EventArgs e)
		//{
		//	SortedList sParams = new SortedList();
		//	sParams.Add("@CampaignID", ddlJoinCampaign.SelectedValue);
		//	sParams.Add("@CharacterID", ddlCampaignCharacter.SelectedValue);
		//	sParams.Add("@UserID", Master.UserID);
		//	sParams.Add("@SkillSetTypeID", ddlJoinSkillSetType.SelectedValue);
		//	sParams.Add("@SkillSetName", tbJoinSkillSetName.Text);

		//	Classes.cUtilities.PerformNonQuery("uspCharacterJoinCampaign", sParams, "LarPortal", Master.UserName);
		//	//Response.Redirect("CharInfo.aspx", true);
		//	lblCharAdded.Text = "The character skill set has been created.";
		//	oCharSelect.Reset();
		//	ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "openCharAdd();", true);
		//	return;
		//	//lblMessage.Text = "The character skill set has been created.";
		//	//lblmodalMessage.Text = "The character skill set has been created.";
		//	//ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
		//	//ResetScreen();
		//}

		//protected void ddlJoinCampaign_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//	MethodBase lmth = MethodBase.GetCurrentMethod();
		//	string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

		//	SortedList sParams = new SortedList();
		//	sParams.Add("@CampaignID", ddlJoinCampaign.SelectedValue);
		//	DataTable dtSkillSetTypes = Classes.cUtilities.LoadDataTable("uspGetCampaignSkillSetTypes", sParams, "LARPortal", Master.UserName, lsRoutineName);
		//	ddlJoinSkillSetType.DataSource = dtSkillSetTypes;
		//	ddlJoinSkillSetType.DataTextField = "SkillSetTypeDescription";
		//	ddlJoinSkillSetType.DataValueField = "CampaignSkillSetTypeID";
		//	ddlJoinSkillSetType.DataBind();
		//}

		protected void ResetScreen()
		{
			//rbNewCharacter.Checked = false;
			pnlAddCharacter.Visible = false;
			//rbNewSkillSet.Checked = false;
			//pnlAddSkillSet.Visible = false;
			//pnlAddCampaign.Visible = false;
			//rbNewCampaign.Checked = false;
		}

		protected void btnCloseAdded_Click(object sender, EventArgs e)
		{
			Response.Redirect("CharInfo.aspx", true);
//			_Redirect = true;
//			Response.Redirect("CharAdd.aspx", false);
		}
	}
}
