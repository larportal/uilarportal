using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LarpPortal.Character
{
	public partial class CharInfo : System.Web.UI.Page
	{
		private bool _Reload = false;

		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			//			Classes.LogWriter oWriter = new Classes.LogWriter();
			//			oWriter.AddLogMessage("Starting up CharInfo.aspx", lsRoutineName, "", Session.SessionID);

			if (!IsPostBack)
			{
				tbRebuildToDate.Style["display"] = "none";
				tbRebuildToDate.Attributes.Add("placeholder", "Expires On Date");
				//lblExpiresOn.Style["display"] = "none";
				tbFirstName.Attributes.Add("Placeholder", "First Name");
				tbLastName.Attributes.Add("Placeholder", "Last Name");
				//lblMessage.Text = "";

				ViewState["NewRecCounter"] = (int) -1;

				oCharSelect.WhichSelected = LarpPortal.Controls.CharacterSelect.Selected.MyCharacters;
			}
			oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
			btnCancelActor.Attributes.Add("data-dismiss", "modal");
			btnCancelCharDeath.Attributes.Add("data-dismiss", "modal");
			ddlAllowRebuild.Attributes.Add("onchange", "ddlRebuildSetVisible();");

			//			oWriter.AddLogMessage("Done with page_load", lsRoutineName, "", Session.SessionID);
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			if (!IsPostBack)
			{
				SortedList sParams = new SortedList();
				sParams.Add("@StatusType", "Character");
				Classes.cUtilities.LoadDropDownList(ddlStatus, "uspGetStatus", sParams, "StatusName", "StatusID", "LARPortal", Master.UserName, lsRoutineName);
				lblStatus.Visible = false;
				ddlStatus.Visible = true;
			}

			//			Classes.LogWriter oWriter = new Classes.LogWriter();
			//			oWriter.AddLogMessage("About to start CharacterSelect.LoadInfo", lsRoutineName, "", Session.SessionID);

			oCharSelect.LoadInfo();

			//			oWriter.AddLogMessage("Done CharacterSelect.LoadInfo", lsRoutineName, "", Session.SessionID);

			if (hidActorDateProblems.Value.Length > 0)
				lblDateProblem.Visible = true;
			else
				lblDateProblem.Visible = false;

			if ((oCharSelect.SkillSetID.HasValue) && (oCharSelect.CharacterInfo != null))
			{
//				int iSelectedCharID = oCharSelect.SkillSetID.Value;
				if ((!IsPostBack) || (_Reload))
				{
					DisplayCharacter(oCharSelect.CharacterInfo);
				}
			}
			//			oWriter.AddLogMessage("Done PreRender", lsRoutineName, "", Session.SessionID);
		}

		protected void ddlDescriptor_SelectedIndexChanged(object sender, EventArgs e)
		{
			oCharSelect.LoadInfo();

			int iCampaignAttributeStandardID;

			List<Classes.cDescriptor> Desc = new List<Classes.cDescriptor>();
			Desc = oCharSelect.CharacterInfo.Descriptors;

			if (int.TryParse(ddlDescriptor.SelectedValue, out iCampaignAttributeStandardID))
			{
				SortedList sParams = new SortedList();
				sParams.Add("@CampaignAttributeID", iCampaignAttributeStandardID);
				Classes.cUtilities.LoadDropDownList(ddlName, "uspGetCampaignAttributeDescriptors", sParams, "DescriptorValue", "CampaignAttributeDescriptorID", "LARPortal", "JLB", "");
				if (ddlName.Items.Count > 0)
					ddlName.SelectedIndex = 0;
				foreach (Classes.cDescriptor dDesc in Desc)
				{
					if (dDesc.CharacterDescriptor == ddlDescriptor.SelectedItem.Text)
					{
						ddlName.SelectedIndex = -1;
						foreach (ListItem Trait in ddlName.Items)
							if (Trait.Text == dDesc.DescriptorValue)
								Trait.Selected = true;
							else
								Trait.Selected = false;
					}
				}
			}
		}

		protected void btnAddDesc_Click(object sender, EventArgs e)
		{
			oCharSelect.LoadInfo();

			Classes.cDescriptor NewDesc = new Classes.cDescriptor();
			NewDesc.DescriptorValue = ddlName.SelectedItem.Text;
			int CampaignAttStandardID;
			int CampaignAttDescriptorID;
			NewDesc.CharacterDescriptor = ddlDescriptor.SelectedItem.Text;

			if (int.TryParse(ddlDescriptor.SelectedValue, out CampaignAttStandardID))
				NewDesc.CampaignAttributeStandardID = CampaignAttStandardID;
			if (int.TryParse(ddlName.SelectedValue, out CampaignAttDescriptorID))
				NewDesc.CampaignAttributeDescriptorID = CampaignAttDescriptorID;
			NewDesc.CharacterAttributesBasicID = -1;
			NewDesc.RecordStatus = Classes.RecordStatuses.Active;
			NewDesc.CharacterSkillSetID = oCharSelect.CharacterInfo.SkillSetID;

			NewDesc.Save(Master.UserName, Master.UserID);
			BindDescriptors();
		}

		protected void BindDescriptors()
		{
			oCharSelect.LoadInfo();

			upDescriptors.Visible = false;
			gvDescriptors.DataSource = null;
			gvDescriptors.DataBind();

			if (oCharSelect.CharacterInfo.Descriptors != null)
			{
				if (oCharSelect.CharacterInfo.Descriptors.Count > 0)
				{
					upDescriptors.Visible = true;
					gvDescriptors.DataSource = null;
					gvDescriptors.DataSource = oCharSelect.CharacterInfo.Descriptors;
					gvDescriptors.DataBind();
				}
			}
		}

		protected void btnDeleteDesc_Click(object sender, EventArgs e)
		{
			int iDescID;
			if (int.TryParse(hidDescID.Value, out iDescID))
			{
				Classes.cDescriptor cDesc = new Classes.cDescriptor();
				cDesc.CharacterAttributesBasicID = iDescID;
				cDesc.RecordStatus = Classes.RecordStatuses.Delete;
				cDesc.Delete(Master.UserName, Master.UserID);
				_Reload = true;
			}
		}

		#region Actors

		protected void btnSaveActor_Click(object sender, EventArgs e)
		{
			oCharSelect.LoadInfo();

			List<Classes.cActor> cActors = oCharSelect.CharacterInfo.Actors;
			int iCharacterActorID;
			if (int.TryParse(hidActorID.Value, out iCharacterActorID))
			{
				Classes.cActor NewActor = new Classes.cActor();
				NewActor.CharacterActorID = iCharacterActorID;
				NewActor.CharacterID = Convert.ToInt32(hidCharacterID.Value);
				NewActor.Comments = tbActorComments.Text;
				NewActor.StaffComments = tbActorStaffComments.Text;
				DateTime dtActorStartDate;
				if (DateTime.TryParse(tbActorStartDate.Text, out dtActorStartDate))
					NewActor.StartDate = dtActorStartDate;
				DateTime dtActorEndDate;
				if (DateTime.TryParse(tbActorEndDate.Text, out dtActorEndDate))
					NewActor.EndDate = dtActorEndDate;
				NewActor.RecordStatus = Classes.RecordStatuses.Active;
				if (ddlActorName.SelectedIndex >= 0)
				{
					int iActorID;
					if (int.TryParse(ddlActorName.SelectedValue, out iActorID))
					{
						NewActor.loginUserName = ddlActorName.SelectedItem.Text;
						NewActor.UserID = iActorID;
					}
				}

				// If the record is already in the list, remove it so we don't have duplicates.
				cActors.RemoveAll(x => x.CharacterActorID == iCharacterActorID);

				cActors.Add(NewActor);

				// Now we go through and check all of the records for overlaps.
				var ActorList = cActors.OrderBy(x => x.StartDate).ToList();
				hidActorDateProblems.Value = "";

				if (ActorList.Count > 1)
				{
					// Now we get convoluted. Go through and check each record against the next record.
					// If the end date of the first record is null, make it 1 day less than the start
					// of the next record.
					// if Rec2.StartDate < Rec1.EndDate = raise problem.
					// Screen will already not allow the end date to be before the start date.
					for (int i = 0; i < (ActorList.Count - 1); i++)
					{
						// If the current record has no end date, fill it in with the date before the start of the next record.
						if (!ActorList[i].EndDate.HasValue)
							ActorList[i].EndDate = ActorList[i + 1].StartDate.Value.AddDays(-1);

						if (ActorList[i + 1].StartDate < ActorList[i].EndDate)
						{
							hidActorDateProblems.Value = "Y";
							lblmodalError.Text = "With this change there are actors with overlapping dates.<br>Please reenter the corrected dates.";
							ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openError();", true);
						}
					}
				}

				if (String.IsNullOrEmpty(hidActorDateProblems.Value))
					NewActor.Save(Master.UserID);

				_Reload = true;
			}
		}

		protected void btnDeleteActor_Click(object sender, EventArgs e)
		{
			int iCharacterActorID;
			if (int.TryParse(hidDeleteActorID.Value, out iCharacterActorID))
			{
				Classes.cActor cActor = new Classes.cActor();
				cActor.CharacterActorID = iCharacterActorID;
				cActor.RecordStatus = Classes.RecordStatuses.Delete;
				cActor.Save(Master.UserID);
				_Reload = true;
			}
		}

		protected void BindActors(List<Classes.cActor> Actors)
		{
			var ActorList = Actors.FindAll(x => x.RecordStatus == Classes.RecordStatuses.Active).OrderBy(x => x.StartDate).ToList();
			gvActors.DataSource = ActorList;
			gvActors.DataBind();
		}

		protected void gvActors_DataBound(object sender, EventArgs e)
		{
			if (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters)
			{
				gvActors.Columns[4].Visible = false;
				gvActors.Columns[5].Visible = false;
			}
			else
			{
				gvActors.Columns[4].Visible = true;
				gvActors.Columns[5].Visible = true;
			}
		}

		#endregion

		#region Deaths

		protected void btnSaveCharDeath_Click(object sender, EventArgs e)
		{
			oCharSelect.LoadInfo();

			if (oCharSelect.SkillSetID.HasValue)
			{
				int iDeathID;
				if (int.TryParse(hidDeathID.Value, out iDeathID))
				{
					Classes.cCharacterDeath cDeath = new Classes.cCharacterDeath();
					cDeath.CharacterDeathID = iDeathID;
					cDeath.CharacterID = oCharSelect.CharacterID.Value;
					cDeath.Comments = tbDeathComments.Text;
					DateTime dtDeathDate;
					if (DateTime.TryParse(tbDeathDate.Text, out dtDeathDate))
						cDeath.DeathDate = dtDeathDate;
					cDeath.DeathPermanent = cbxDeathPerm.Checked;
					cDeath.RecordStatus = Classes.RecordStatuses.Active;
					cDeath.StaffComments = tbDeathStaffComments.Text;

					cDeath.Save(Master.UserID);
					_Reload = true;
				}
			}
		}

		protected void btnDeleteDeath_Click(object sender, EventArgs e)
		{
			int iDeathID;
			if (int.TryParse(hidDeleteDeathID.Value, out iDeathID))
			{
				Classes.cCharacterDeath cDeath = new Classes.cCharacterDeath();
				cDeath.CharacterDeathID = iDeathID;
				cDeath.RecordStatus = Classes.RecordStatuses.Delete;
				cDeath.Save(Master.UserID);
				_Reload = true;
			}
		}

		protected void BindDeaths(List<Classes.cCharacterDeath> Deaths)
		{
			var DeathList = Deaths.FindAll(x => x.RecordStatus == Classes.RecordStatuses.Active).OrderBy(x => x.DeathDate).ToList();

			gvDeaths.DataSource = DeathList;
			gvDeaths.DataBind();
		}

		protected void gvDeaths_DataBound(object sender, EventArgs e)
		{
			if (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters)
			{
				gvDeaths.Columns[3].Visible = false;
				gvDeaths.Columns[4].Visible = false;
			}
			else
			{
				gvDeaths.Columns[3].Visible = true;
				gvDeaths.Columns[4].Visible = true;
			}
		}

		#endregion

		protected void oCharSelect_CharacterChanged(object sender, EventArgs e)
		{
			oCharSelect.LoadInfo();

			if (oCharSelect.CharacterInfo != null)
			{
				if (oCharSelect.SkillSetID.HasValue)
				{
					Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
					UserInfo.LastLoggedInCampaign = oCharSelect.CampaignID.Value;
					UserInfo.LastLoggedInCharacter = oCharSelect.CharacterID.Value;
					UserInfo.LastLoggedInSkillSetID = oCharSelect.SkillSetID.Value;
					UserInfo.LastLoggedInMyCharOrCamp = (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters ? "M" : "C");
					UserInfo.Save();
					Session["ReloadCampaigns"] = "Y";
					if (oCharSelect.CampaignID.Value != Master.CampaignID)
						Master.ChangeSelectedCampaign();
				}
				_Reload = true;
			}
		}


		private void DisplayCharacter(Classes.cCharacter CharInfo)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			//			oCharSelect.LoadInfo();		JLB Hidden Skills.

			//			Classes.LogWriter oWriter = new Classes.LogWriter();
			//			oWriter.AddLogMessage("Starting Display Character.", lsRoutineName, "", Session.SessionID);

			hidSkillSetID.Value = CharInfo.SkillSetID.ToString();
			hidCharacterID.Value = CharInfo.CharacterID.ToString();
			ViewState["ProfilePictureID"] = CharInfo.ProfilePictureID;

			tbFirstName.Text = CharInfo.FirstName;
			tbMiddleName.Text = CharInfo.MiddleName;
			tbLastName.Text = CharInfo.LastName;

			tbBirthPlace.Text = CharInfo.CurrentHome;
			tbAKA.Text = CharInfo.AKA;
			tbHome.Text = CharInfo.CurrentHome;
			if (CharInfo.LatestEventDate.HasValue)
				tbLastEvent.Text = CharInfo.LatestEventDate.Value.ToString("MM/dd/yyyy");
			else
				tbLastEvent.Text = "";

			tbCharType.Text = CharInfo.CharType.Description;

			divHiddenSkills.Attributes["class"] = "hide";

			// Hidden Skills.
			if (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.CampaignCharacters)
			{
				SortedList sParams = new SortedList();
				sParams.Add("@SkillSetID", CharInfo.SkillSetID);
				DataSet dsSkillAccess = Classes.cUtilities.LoadDataSet("uspGetCharacterHiddenSkills", sParams, "LARPortal", Master.UserName, lsRoutineName);
				if (dsSkillAccess.Tables[0].Rows.Count > 0)
				{
					gvHiddenSkillAccess.DataSource = dsSkillAccess.Tables[0];
					gvHiddenSkillAccess.DataBind();
					divHiddenSkills.Attributes["class"] = "show";
				}
			}

			if ((oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.CampaignCharacters) || (oCharSelect.CharacterInfo.CharacterType == 1))
			{
				//ddlVisible.Visible = true;
				//ddlAllowRebuild.Visible = true;
				//lblAllowSkillRebuild.Visible = true;
				//lblVisibleRelationship.Visible = true;
				//lblExpiresOn.Visible = true;
				tbCharType.Visible = false;
				ddlCharType.SelectedValue = CharInfo.CharType.CharacterTypeID.ToString();
				ddlCharType.Visible = true;

				divAddDeath.Attributes["class"] = "hide";
				divAddActor.Attributes["class"] = "show text-right";

				//lblStatus.Visible = false;
				//ddlStatus.Visible = true;
				//ddlAllowRebuild.Visible = true;
				//lblAllowSkillRebuild.Visible = true;

				divAllowRebuild.Visible = false;

				if ((oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.CampaignCharacters) && (oCharSelect.CharacterInfo.CharacterType == 1))
				{
					divAddDeath.Attributes["class"] = "show text-right";
					divAllowRebuild.Visible = true;

					if (CharInfo.AllowCharacterRebuild)
					{
						ddlAllowRebuild.SelectedValue = "Y";
						tbRebuildToDate.Text = CharInfo.AllowCharacterRebuildToDate.Value.ToShortDateString();
						tbRebuildToDate.Style["display"] = "inline";
					}
					else
					{
						ddlAllowRebuild.SelectedValue = "N";
						tbRebuildToDate.Text = "";
						tbRebuildToDate.Style["display"] = "none";
					}
				}

				// This is me being overly cautious. If the status is not found I'm not sure what will happen.
				try
				{
					ddlStatus.SelectedValue = CharInfo.Status.StatusID.ToString();
				}
				catch
				{
					ddlStatus.ClearSelection();
					foreach (ListItem litem in ddlStatus.Items)
					{
						if (litem.Text.ToUpper() == "ACTIVE")
							litem.Selected = true;
					}
				}
			}
			else
			{
				divAllowRebuild.Visible = false;
				ddlStatus.Visible = false;
				lblStatus.Visible = true;
				tbCharType.Visible = true;
				ddlCharType.Visible = false;
				lblStatus.Text = CharInfo.Status.StatusName;
				//ddlVisible.Visible = false;
				//lblVisibleRelationship.Visible = false;
				//ddlAllowRebuild.Visible = false;
				divAddDeath.Attributes["class"] = "hide";
				divAddActor.Attributes["class"] = "hide";
			}

			divVisibleRelationships.Visible = false;

			if (oCharSelect.CharacterInfo.CharacterType == 1)         // 1 = PC.
			{
				if (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters)
				{
					divNonCost.Attributes["class"] = "col-sm-6";
					divDeaths.Attributes["class"] = "col-sm-6";
					divPlayer.Attributes["class"] = "hide";
					divActors.Attributes["class"] = "hide";
				}
				else
				{
					divNonCost.Attributes["class"] = "col-sm-4";
					divDeaths.Attributes["class"] = "col-sm-4";
					divActors.Attributes["class"] = "hide";
					divPlayer.Attributes["class"] = "col-sm-4";
				}
			}
			else
			{
				if (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.CampaignCharacters)
					divVisibleRelationships.Visible = true;

				divNonCost.Attributes["class"] = "col-sm-4";
				divDeaths.Attributes["class"] = "col-sm-4";
				divActors.Attributes["class"] = "col-sm-4";
				divPlayer.Attributes["class"] = "hide";

				SortedList sParams = new SortedList();
				sParams.Add("@CampaignID", oCharSelect.CampaignID.Value);
				DataTable dtPlayers = new DataTable();
				dtPlayers = Classes.cUtilities.LoadDataTable("uspGetCampaignPlayers", sParams, "LARPortal", Master.UserName, lsRoutineName);

				DataView dvPlayers = new DataView(dtPlayers, "", "PlayerFirstLastName", DataViewRowState.CurrentRows);
				ddlActorName.DataSource = dvPlayers;
				ddlActorName.DataTextField = "PlayerFirstLastName";
				ddlActorName.DataValueField = "UserID";
				ddlActorName.DataBind();
				ddlActorName.Items.Insert(0, new ListItem("", "-1"));
			}

			if (CharInfo.VisibleToPCs)
				ddlVisible.SelectedValue = "1";
			else
				ddlVisible.SelectedValue = "0";

			if (CharInfo.Teams.Count == 0)
			{
				ddlTeamList.Visible = false;
				tbTeam.Visible = true;
				tbTeam.Text = "No Teams";
			}
			else if (CharInfo.Teams.Count == 1)
			{
				ddlTeamList.Visible = false;
				tbTeam.Visible = true;
				tbTeam.Text = CharInfo.Teams[0].TeamName;
			}
			else
			{
				ddlTeamList.Visible = true;
				tbTeam.Visible = false;
				ddlTeamList.DataSource = CharInfo.Teams;
				ddlTeamList.DataTextField = "TeamName";
				ddlTeamList.DataValueField = "TeamID";
				ddlTeamList.DataBind();

				ddlTeamList.ClearSelection();

				foreach (ListItem litem in ddlTeamList.Items)
				{
					if (litem.Value == CharInfo.TeamID.ToString())
					{
						ddlTeamList.ClearSelection();
						litem.Selected = true;
					}
				}
				if (ddlTeamList.SelectedIndex < 0)
					ddlTeamList.SelectedIndex = 0;
			}

			tbDOB.Text = CharInfo.DateOfBirth;

			tbAKA.Text = CharInfo.AKA;
			tbDOB.Text = CharInfo.DateOfBirth;
			tbHome.Text = CharInfo.CurrentHome;
			tbNumOfDeaths.Text = CharInfo.Deaths.Count().ToString();
			tbBirthPlace.Text = CharInfo.WhereFrom;

			DataTable dtCharDescriptors = new DataTable();
			dtCharDescriptors = Classes.cUtilities.CreateDataTable(CharInfo.Descriptors);
			BindDescriptors();

			if (CharInfo.ProfilePicture != null)
			{
				ViewState["UserIDPicture"] = CharInfo.ProfilePicture;
				imgCharacterPicture.ImageUrl = CharInfo.ProfilePicture.PictureURL;
				//imgCharacterPicture.Visible = true;
				//                btnClearPicture.Visible = true;
			}
			else
			{
				imgCharacterPicture.ImageUrl = "~/images/profile-photo.png";
				//imgCharacterPicture.Visible = false;
				//                btnClearPicture.Visible = false;
			}

			//			oWriter.AddLogMessage("About to load races.", lsRoutineName, "", Session.SessionID);

			Classes.cCampaignRaces Races = new Classes.cCampaignRaces();
			Races.CampaignID = CharInfo.CampaignID;
			Races.Load(Master.UserName);

			//			oWriter.AddLogMessage("Done loading races.", lsRoutineName, "", Session.SessionID);

			DataTable dtRaces = Classes.cUtilities.CreateDataTable(Races.RaceList);
			ddlRace.DataSource = dtRaces;
			ddlRace.DataTextField = "FullRaceName";
			ddlRace.DataValueField = "CampaignRaceID";
			ddlRace.DataBind();
			if (ddlRace.Items.Count > 0)
			{
				ddlRace.SelectedIndex = 0;
				foreach (ListItem dItems in ddlRace.Items)
				{
					if (dItems.Value == CharInfo.Race.CampaignRaceID.ToString())
					{
						dItems.Selected = true;
						tbRace.Text = dItems.Text;
					}
					else
						dItems.Selected = false;
				}
			}

			//			oWriter.AddLogMessage("About to get Campaign Attributes Standard.", lsRoutineName, "", Session.SessionID);

			SortedList sParam = new SortedList();
			sParam.Add("@CampaignID", CharInfo.CampaignID);
			sParam.Add("@Roles", Master.RoleString);
			DataTable dtDescriptors = Classes.cUtilities.LoadDataTable("uspGetCampaignAttributesStandard",
				sParam, "LARPortal", Master.UserName, lsRoutineName + ".GetCampaignAttributesStandard");

			//			oWriter.AddLogMessage("Done getting Campaign Attributes Standard.", lsRoutineName, "", Session.SessionID);

			DataView dvDescriptors = new DataView(dtDescriptors, "", "CharacterDescriptor", DataViewRowState.CurrentRows);
			ddlDescriptor.DataTextField = "CharacterDescriptor";
			ddlDescriptor.DataValueField = "CampaignAttributeStandardID";
			ddlDescriptor.DataSource = dtDescriptors;
			ddlDescriptor.DataBind();

			//			oWriter.AddLogMessage("About to do Descriptor SelectedIndexChanged.", lsRoutineName, "", Session.SessionID);

			if (dtDescriptors.Rows.Count > 0)
			{
				upDescriptors.Visible = true;
				gvDescriptors.Visible = true;
				divCharDev.Visible = true;
				ddlDescriptor.SelectedIndex = 0;
				ddlDescriptor_SelectedIndexChanged(null, null);
			}

			//			oWriter.AddLogMessage("Done Descriptor SelectedIndexChanged.", lsRoutineName, "", Session.SessionID);

			tbStaffComments.Text = CharInfo.StaffComments;
			if (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters)
			{
				divStaffComments.Visible = false;
				divHiddenSkills.Visible = false;
			}
			else
			{
				divStaffComments.Visible = true;
				divHiddenSkills.Visible = true;
			}

			BindActors(CharInfo.Actors);

			if (CharInfo.Actors.Count > 0)
			{
				var LatestActor = CharInfo.Actors.OrderByDescending(x => x.StartDate).ToList();
				lblPlayer.Text = "Currently played by " + LatestActor[0].loginUserName + " - ";
				if (LatestActor[0].ActorNickName.Length > 0)
					lblPlayer.Text += LatestActor[0].ActorNickName;
				else
					lblPlayer.Text += LatestActor[0].ActorFirstName;
				lblPlayer.Text += " " + LatestActor[0].ActorLastName;
			}

			BindDeaths(CharInfo.Deaths);

			ReadOnlyFields();

			//			oWriter.AddLogMessage("Done DisplayCharacter.", lsRoutineName, "", Session.SessionID);
		}

		protected void ReadOnlyFields()
		{
			if ((oCharSelect.CharacterInfo.CharacterType != 1) && (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters))
			{
				btnSave.Enabled = false;
				divCharDev.Visible = false;
				tbFirstName.Enabled = false;
				tbMiddleName.Enabled = false;
				tbLastName.Enabled = false;
				tbDOB.Enabled = false;
				tbHome.Enabled = false;
				tbBirthPlace.Enabled = false;
				tbAKA.Enabled = false;
				ddlRace.Visible = false;
				tbRace.Visible = true;
				ddlCharType.Visible = false;
				tbCharType.Visible = true;
				divAddDeath.Visible = false;
				//    ulFile.Visible = false;
				//    btnUpload.Visible = false;
				//    btnClearPicture.Visible = false;
				//    lblProfilePictureText.Visible = false;
			}
			else
			{
				btnSave.Enabled = true;
				divCharDev.Visible = true;
				tbFirstName.Enabled = true;
				tbMiddleName.Enabled = true;
				tbLastName.Enabled = true;
				tbBirthPlace.Enabled = true;
				tbDOB.Enabled = true;
				tbHome.Enabled = true;
				tbAKA.Enabled = true;
				ddlRace.Visible = true;
				tbRace.Visible = false;
				ddlCharType.Visible = true;
				tbCharType.Visible = false;
				divAddDeath.Visible = true;
				//    ulFile.Visible = true;
				//    btnUpload.Visible = true;
				//    lblProfilePictureText.Visible = true;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			int iTemp;

			oCharSelect.LoadInfo();

			if ((oCharSelect.SkillSetID.HasValue) && (oCharSelect.CharacterInfo != null))
			{
				int i = 0;
				string sHiddenSkillList = "";

				foreach (GridViewRow row in gvHiddenSkillAccess.Rows)
				{
					if (row.RowType == DataControlRowType.DataRow)
					{
						CheckBox cbxHasSkill = (row.Cells[1].FindControl("cbxHasSkill") as CheckBox);
						HiddenField hidHadOriginally = (row.Cells[2].FindControl("hidHadOriginally") as HiddenField);
						HiddenField hidCampaignSkillNodeID = (row.Cells[2].FindControl("hidCampaignSkillNodeID") as HiddenField);
						HiddenField hidSkillName = (row.Cells[2].FindControl("hidSkillName") as HiddenField);

						if ((cbxHasSkill != null) &&
							(hidHadOriginally != null) &&
							(hidCampaignSkillNodeID != null))
						{
							bool bHasOriginally = false;
							if (hidHadOriginally.Value == "1")
								bHasOriginally = true;
							if (bHasOriginally != cbxHasSkill.Checked)
							{
								if (cbxHasSkill.Checked)
								{
									// Since the value has changed, and it's checked, it means we need to add the value;
									int iCampaignSkillNodeID;
									if (int.TryParse(hidCampaignSkillNodeID.Value, out iCampaignSkillNodeID))
									{
										SortedList sParams = new SortedList();
										sParams.Add("@UserID", Master.UserID);
										sParams.Add("@CampaignSkillsStandardID", iCampaignSkillNodeID);
										sParams.Add("@SkillSetID", oCharSelect.SkillSetID);
										sParams.Add("@Comments", "Skill added by " + Master.UserName);
										sParams.Add("@CharacterID", oCharSelect.CharacterID);
										Classes.cUtilities.PerformNonQuery("uspInsUpdCHCampaignSkillAccess", sParams, "LARPortal", Master.UserName);

										if (hidSkillName != null)
										{
											sHiddenSkillList += hidSkillName.Value + "<br>";
										}
									}
								}
								else
								{
									// Since the value has changed, and it's not checked, it means we need to delete the value;
									HiddenField hidCampaignSkillAccessID = (row.Cells[2].FindControl("hidCampaignSkillAccessID") as HiddenField);
									if (hidCampaignSkillAccessID != null)
									{
										int iCampaignSkillAccessID;
										if (int.TryParse(hidCampaignSkillAccessID.Value, out iCampaignSkillAccessID))
										{
											SortedList sParams = new SortedList();
											sParams.Add("@CampaignSkillAccessID", iCampaignSkillAccessID);
											sParams.Add("@UserName", Master.UserName);
											sParams.Add("@UserID", Master.UserID);
											Classes.cUtilities.PerformNonQuery("uspDelCHCampaignSkillAccess", sParams, "LARPortal", Master.UserName);
										}
									}
								}
							}
						}
					}
					i++;
				}

				// If there were any hidden skills given to the character, send them an email.
				if ((sHiddenSkillList.Length > 0) &&
					(oCharSelect.CharacterInfo.CharacterEmail.ToString().Length > 0))
				{
					string sSubject = "LARP Portal Notification - " + Master.CampaignName + " staff has given you access to a hidden skill.";
					string sBody = "The staff of " + Master.CampaignName + " has given your character " + oCharSelect.CharacterInfo.AKA + " access to the following skill(s):<br>" +
						"<br>" +
						sHiddenSkillList;

					Classes.cUser User = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
					Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
						cEMS.SendMail(sSubject, sBody, oCharSelect.CharacterInfo.CharacterEmail, "", User.PrimaryEmailAddress.EmailAddress, "CharInfo - Hidden Skills", Master.UserName);
				}

				if ((tbAKA.Text.Length == 0) &&
					(tbFirstName.Text.Length == 0))
				{
					// JBradshaw  7/11/2016    Request #1286     Must have at least first name or last name.
					lblmodalError.Text = "You must fill in at least the first name or the character AKA.";
					ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openError();", true);
					return;
				}

				Classes.cCharacter cChar = new Classes.cCharacter();
				cChar.LoadCharacterBySkillSetID(oCharSelect.SkillSetID.Value);

				if (ulFile.HasFile)
				{
					try
					{
						//                        string sUser = Session["LoginName"].ToString();
						Classes.cPicture NewPicture = new Classes.cPicture();
						NewPicture.PictureType = Classes.cPicture.PictureTypes.Profile;
						NewPicture.CreateNewPictureRecord(Master.UserName);
						string sExtension = Path.GetExtension(ulFile.FileName);
						NewPicture.PictureFileName = "CP" + NewPicture.PictureID.ToString("D10") + sExtension;

						NewPicture.CharacterID = oCharSelect.CharacterID.Value;

						string LocalName = NewPicture.PictureLocalName;

						if (!Directory.Exists(Path.GetDirectoryName(NewPicture.PictureLocalName)))
							Directory.CreateDirectory(Path.GetDirectoryName(NewPicture.PictureLocalName));

						ulFile.SaveAs(NewPicture.PictureLocalName);
						NewPicture.Save(Master.UserName);

						cChar.ProfilePicture = NewPicture;
						//                        ViewState["UserIDPicture"] = NewPicture;
						ViewState.Remove("PictureDeleted");

						imgCharacterPicture.ImageUrl = NewPicture.PictureURL;
						imgCharacterPicture.Visible = true;
						//                    btnClearPicture.Visible = true;
					}
					catch (Exception ex)
					{
						Classes.ErrorAtServer lobjError = new Classes.ErrorAtServer();
						lobjError.ProcessError(ex, "CharInfo.savepicture", "", Master.UserName);
					}
				}

				cChar.FirstName = tbFirstName.Text;
				cChar.MiddleName = tbMiddleName.Text;
				cChar.LastName = tbLastName.Text;

				cChar.CurrentHome = tbBirthPlace.Text;

				cChar.AllowCharacterRebuildToDate = null;

				if (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.CampaignCharacters)
				{
					cChar.CharacterStatusID = Convert.ToInt32(ddlStatus.SelectedValue);
					cChar.CharacterType = Convert.ToInt32(ddlCharType.SelectedValue);

					//if (ddlAllowRebuild.SelectedValue == "Y")
					//{
					//    tbRebuildToDate.Style["display"] = "inline";
					//    lblExpiresOn.Style["display"] = "inline";
					//    DateTime dtTemp;
					//    if (DateTime.TryParse(tbRebuildToDate.Text, out dtTemp))
					//        cChar.AllowCharacterRebuildToDate = dtTemp;
					//}
					//else
					//{
					//    tbRebuildToDate.Style["display"] = "none";
					//    lblExpiresOn.Style["display"] = "none";
					//}
				}

				if (ddlVisible.SelectedValue == "1")
					cChar.VisibleToPCs = true;
				else
					cChar.VisibleToPCs = false;

				cChar.AKA = tbAKA.Text;
				cChar.CurrentHome = tbHome.Text;
				cChar.WhereFrom = tbBirthPlace.Text;

				// If the drop down list is visible, it means they belong to multiple teams so we need to check it.
				if (ddlTeamList.Visible)
				{
					if (int.TryParse(ddlTeamList.SelectedValue, out iTemp))
						cChar.TeamID = iTemp;
				}

				cChar.DateOfBirth = tbDOB.Text;
				if (ViewState["UserIDPicture"] != null)
				{
					cChar.ProfilePicture = ViewState["UserIDPicture"] as Classes.cPicture;
					if (ViewState["PictureDeleted"] != null)
						cChar.ProfilePicture.RecordStatus = Classes.RecordStatuses.Delete;
				}
				else
					cChar.ProfilePicture = null;

				if (ddlRace.SelectedIndex > -1)
				{
					cChar.Race = new Classes.cRace();
					int.TryParse(ddlRace.SelectedValue, out iTemp);
					cChar.Race.CampaignRaceID = iTemp;
				}

				if (ddlAllowRebuild.SelectedValue == "Y")
				{
					DateTime dtTemp;
					if (DateTime.TryParse(tbRebuildToDate.Text, out dtTemp))
						cChar.AllowCharacterRebuildToDate = dtTemp;
				}
				else
					cChar.AllowCharacterRebuildToDate = new DateTime(1900, 1, 1);

				if (cChar.CharacterType != 1)       // Means it's not a PC so we need to check who is the current actor.
				{
					var LastActor = cChar.Actors.OrderByDescending(x => x.StartDate).ToList();
					if (LastActor == null)
						cChar.CurrentUserID = Master.UserID;
					else
					{
						if (LastActor.Count > 0)
						{
							cChar.CurrentUserID = LastActor[0].UserID;
						}
					}
				}

				cChar.StaffComments = tbStaffComments.Text;

				cChar.SaveCharacter(Master.UserName, Master.UserID);
				// JBradshaw  7/11/2016    Request #1286     Changed over to bootstrap popup.
				lblmodalMessage.Text = "Character " + cChar.AKA + " has been saved.";
				ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
			}
		}

		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			string t = sender.GetType().ToString();
			oCharSelect.Reset();
			_Reload = true;
			Classes.cUser user = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
			if (user.LastLoggedInCharacter == -1)
				Response.Redirect("/default.aspx");
		}
	}
}