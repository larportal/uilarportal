using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LarpPortal.Classes;

namespace LarpPortal.Controls
{
    public partial class CharacterSelect : System.Web.UI.UserControl
    {
        /// <summary>
        /// Public event that should be the routine you want to call when a character selection changes.
        /// </summary>
        public event EventHandler CharacterChanged;
        private int? _CharacterID = null;
		private int? _CampaignID = null;
        private int _UserID = 0;
        private string _UserName = "";
		private int? _SkillSetID = null;

        /// <summary>
        /// Public enum used to tell whether doing user's character or character for a campaign.
        /// </summary>
        public enum Selected
        {
            NotSpecified,
            MyCharacters,
            CampaignCharacters
        };

        /// <summary>
        /// Are we doing user's character or character for a campaign.
        /// </summary>
        public Selected WhichSelected = Selected.NotSpecified;
        private Classes.cCharacter _CharacterInfo = new cCharacter();
        private Classes.cUser _UserInfo = null;


        /// <summary>
        /// Character ID of selected character. If there are no available characters this will be null.
        /// </summary>
        public int? CharacterID
        {
            get { return _CharacterID; }
            set
            {
                _CharacterID = value;
                ListItem listItem = ddlCharacterSelector.Items.FindByValue(value.ToString());

                if (listItem != null)
                {
                    ddlCharacterSelector.ClearSelection();
                    listItem.Selected = true;
                }
            }
        }

        /// <summary>
		/// Returns campaign ID for currently selected character.
		/// </summary>
		public int? CampaignID
		{
			get { return _CampaignID; }
			set { _CampaignID = value; }
		}

		/// <summary>
        /// Character info for the currently selected character.
        /// </summary>
        public Classes.cCharacter CharacterInfo
        {
            get { return _CharacterInfo; }
            set { _CharacterInfo = value; }
        }

        public Classes.cUser UserInfo
        {
            get { return _UserInfo; }
            set { _UserInfo = value; }
        }

		public int? SkillSetID
		{
			get { return _SkillSetID; }
			set { _SkillSetID = value; }
		}

//        private Classes.LogWriter oLogWriter = new LogWriter();

        protected void Page_Load(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (!IsPostBack)
            {
                _UserInfo = null;
            }

//            oLogWriter.AddLogMessage("Starting up CharacterSelect", lsRoutineName, "", Session.SessionID);

            if (Session["UserID"] != null)
                int.TryParse(Session["UserID"].ToString(), out _UserID);
            if (Session["UserName"] != null)
                _UserName = Session["UserName"].ToString();

            if (_UserInfo == null)
            {
//                oLogWriter.AddLogMessage("About to load user in Page Load routine.", lsRoutineName, "", Session.SessionID);
                _UserInfo = new cUser(_UserName, "PasswordNotNeeded", Session.SessionID);
//                oLogWriter.AddLogMessage("Done  loading user in Page_Load routine.", lsRoutineName, "", Session.SessionID);
            }

            if ((Session["CampaignsToEdit"] == null) ||
                (Session["MyCharacters"] == null))
            {
				_CharacterID = UserInfo.LastLoggedInCharacter;
				_SkillSetID = UserInfo.LastLoggedInSkillSetID;
				_CampaignID = UserInfo.LastLoggedInCampaign;

				if (UserInfo.LastLoggedInMyCharOrCamp == "C")
                {
                    WhichSelected = Selected.CampaignCharacters;
					Session["CharCampaignCampaignID"] = UserInfo.LastLoggedInCampaign;
					Session["CharCampaignCharID"] = UserInfo.LastLoggedInCharacter;
					Session["CharCampaignSkillSetID"] = UserInfo.LastLoggedInSkillSetID;
                    Session["CharacterSelectGroup"] = "Campaigns";
                }
                else
                {
                    WhichSelected = Selected.MyCharacters;
					Session["CharSkillSetID"] = UserInfo.LastLoggedInSkillSetID;
					Session["CharCharacterID"] = UserInfo.LastLoggedInCharacter;
					Session["CharCampaignID"] = UserInfo.LastLoggedInCampaign;
					Session["CharacterSelectGroup"] = "Characters";
                }
            }
//            oLogWriter.AddLogMessage("Done loading CharacterSelect", lsRoutineName, "", Session.SessionID);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

//            oLogWriter.AddLogMessage("Starting up CharacterSelect.PreRender", lsRoutineName, "", Session.SessionID);

            SortedList slParameters = new SortedList();
            DataTable dtCampaignsToEdit = new DataTable();
            DataTable dtMyCharacters = new DataTable();

            WhichSelected = Selected.NotSpecified;

            if ((Session["CampaignsToEdit"] == null) ||
                (Session["MyCharacters"] == null))
            {
                if ( _UserInfo == null )
                    _UserInfo = new cUser(_UserName, "PasswordNotNeeded", Session.SessionID);

//                oLogWriter.AddLogMessage("Done loading user.", lsRoutineName, "", Session.SessionID);
				//                Session["CharCharacterID"] = UserInfo.LastLoggedInCharacter;
                if (UserInfo.LastLoggedInMyCharOrCamp == "C")
                {
                    WhichSelected = Selected.CampaignCharacters;
					Session["CharCampaignCampaignID"] = UserInfo.LastLoggedInCampaign;
					Session["CharCampaignCharID"] = UserInfo.LastLoggedInCharacter;
					Session["CharCampaignSkillSetID"] = UserInfo.LastLoggedInSkillSetID;
                    Session["CharacterSelectGroup"] = "Campaigns";
                }
                else
                {
                    WhichSelected = Selected.MyCharacters;
					Session["CharSkillSetID"] = UserInfo.LastLoggedInSkillSetID;
					Session["CharCharacterID"] = UserInfo.LastLoggedInCharacter;
					Session["CharCampaignID"] = UserInfo.LastLoggedInCampaign;
					Session["CharacterSelectGroup"] = "Characters";
                }
            }

            if (Session["CampaignsToEdit"] == null)
            {
                slParameters.Add("@UserID", _UserID);
//                oLogWriter.AddLogMessage("About to run uspGetPrivCampaignCharacterEdit.", lsRoutineName, "", Session.SessionID);
                DataTable dtFullCampaignsToEdit = LarpPortal.Classes.cUtilities.LoadDataTable("uspGetPrivCampaignCharacterEdit", slParameters,
                    "LARPortal", _UserName, lsRoutineName + ".uspGetPrivCampaignCharacterEdit");
//                oLogWriter.AddLogMessage("Done running uspGetPrivCampaignCharacterEdit.", lsRoutineName, "", Session.SessionID);
                dtCampaignsToEdit = new DataView(dtFullCampaignsToEdit, "", "CampaignName", DataViewRowState.CurrentRows).ToTable(true, "CampaignID", "CampaignName");
                Session["CampaignsToEdit"] = dtCampaignsToEdit;
                if (dtCampaignsToEdit.Rows.Count == 0)
                {
                    WhichSelected = Selected.MyCharacters;
                }
                //else
                //{
                //    Session["CampaignsToEdit"] = dtCampaignsToEdit;
                //}
            }
            else
                dtCampaignsToEdit = Session["CampaignsToEdit"] as DataTable;

            if (Session["MyCharacters"] == null)
            {
                slParameters = new SortedList();
                slParameters.Add("@intUserID", Session["UserID"].ToString());
//                oLogWriter.AddLogMessage("About to run uspGetCharacterIDsByUserID", lsRoutineName, "", Session.SessionID);
                DataTable dtCharacters = LarpPortal.Classes.cUtilities.LoadDataTable("uspGetCharacterIDsByUserID", slParameters,
                    "LARPortal", _UserName, lsRoutineName + ".uspGetCharacterIDsByUserID");
//                oLogWriter.AddLogMessage("Done running uspGetCharacterIDsByUserID", lsRoutineName, "", Session.SessionID);

                // If the person has no characters force it to charadd.
                if (dtCharacters.Rows.Count == 0)
				{
					if (!HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("CHARADD.ASPX"))
					{
                    Response.Redirect("~/Character/CharAdd.aspx", true);
					}
					else
						return;
				}
				dtMyCharacters = new DataView(dtCharacters, "", "DisplayName", DataViewRowState.CurrentRows).ToTable(true, "DisplayName", "CharacterID", "CharacterSkillSetID");
                Session["MyCharacters"] = dtMyCharacters;
            }
            else
                dtMyCharacters = Session["MyCharacters"] as DataTable;

            if (dtCampaignsToEdit.Rows.Count == 0)
            {
                ddlCampaigns.Visible = false;
                rbCampaignCharacters.Visible = false;
                rbMyCharacters.Visible = false;
                lblNoCharacters.Visible = false;
            }
            else
            {
                DataView dvCampaigns = new DataView(dtCampaignsToEdit, "", "CampaignName", DataViewRowState.CurrentRows);
                ddlCampaigns.DataSource = dvCampaigns;
                ddlCampaigns.DataTextField = "CampaignName";
                ddlCampaigns.DataValueField = "CampaignID";
                ddlCampaigns.DataBind();
            }

            if (WhichSelected == Selected.NotSpecified)
                if (Session["CharacterSelectGroup"] != null)
                    if (Session["CharacterSelectGroup"].ToString() == "Campaigns")
                        WhichSelected = Selected.CampaignCharacters;
                    else if (Session["CharacterSelectGroup"].ToString() == "Characters")
                        WhichSelected = Selected.MyCharacters;

            if ((dtMyCharacters.Rows.Count >= 1) &&
                (dtCampaignsToEdit.Rows.Count >= 1))
            {
                rbMyCharacters.Visible = true;
                rbCampaignCharacters.Visible = true;
            }
            else
            {
                rbMyCharacters.Visible = false;
                rbCampaignCharacters.Visible = false;
            }

            _CharacterID = null;

//            oLogWriter.AddLogMessage("About to run which selected.", lsRoutineName, "", Session.SessionID);

            if (WhichSelected == Selected.MyCharacters)
            {
                rbMyCharacters.Checked = true;
				DataView dvCharacters = new DataView(dtMyCharacters, "", "DisplayName", DataViewRowState.CurrentRows);
                ddlCharacterSelector.DataSource = dvCharacters;
				ddlCharacterSelector.DataTextField = "DisplayName";
				ddlCharacterSelector.DataValueField = "CharacterSkillSetID";
                ddlCharacterSelector.DataBind();
                ddlCharacterSelector.Visible = true;
                ddlCampCharSelector.Visible = false;
                lblSelectedCampaign.Visible = false;
                rbMyCharacters.Checked = true;

                ddlCharacterSelector.ClearSelection();

				if (Session["CharSkillSetID"] != null)
                {
					int iCharSkillSetID;
					if (int.TryParse(Session["CharSkillSetID"].ToString(), out iCharSkillSetID))
                    {
                        foreach (ListItem liItem in ddlCharacterSelector.Items)
                        {
							if (liItem.Value == iCharSkillSetID.ToString())
							{
								ddlCharacterSelector.ClearSelection();
								liItem.Selected = true;
							}
                        }
                    }
                }

                if (ddlCharacterSelector.SelectedIndex < 0)
                    ddlCharacterSelector.Items[0].Selected = true;

				int iSkillSetID;
				if (int.TryParse(ddlCharacterSelector.SelectedValue, out iSkillSetID))
					if (iSkillSetID == 0)
                    {
                        _CharacterID = null;
                        _CharacterInfo = null;
						_SkillSetID = null;
						Session.Remove("CharSkillSetID");
						Session.Remove("CharCharacterID");
						Session.Remove("CharCampaignID");
                    }
                    else
                    {
						_SkillSetID = iSkillSetID;
						_CharacterInfo.LoadCharacterBySkillSetID(iSkillSetID);
						_CharacterID = _CharacterInfo.CharacterID;
						_CampaignID = _CharacterInfo.CampaignID;
                        lblSelectedCampaign.Visible = true;
                        lblSelectedCampaign.Text = _CharacterInfo.CampaignName;
                        ddlCampaigns.Visible = false;
						Session["CharSkillSetID"] = iSkillSetID;
						Session["CharCharacterID"] = _CharacterInfo.CharacterID;
						Session["CharCampaignID"] = _CharacterInfo.CampaignID;
                    }
//                oLogWriter.AddLogMessage("Done with MyCharacters", lsRoutineName, "", Session.SessionID);
            }
            else if (WhichSelected == Selected.CampaignCharacters)
            {
//                oLogWriter.AddLogMessage("About to load campaign characters.", lsRoutineName, "", Session.SessionID);
                LoadCampaignCharacters();
//                oLogWriter.AddLogMessage("Done loading campaign characters.", lsRoutineName, "", Session.SessionID);
            }

            if (!IsPostBack)
                if (this.CharacterChanged != null)
                {
//                    oLogWriter.AddLogMessage("About to call CharacterChanged", lsRoutineName, "", Session.SessionID);
                    this.CharacterChanged(this, e);
//                    oLogWriter.AddLogMessage("Done calling CharacterChanged", lsRoutineName, "", Session.SessionID);
                }
        }


        public void LoadInfo()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            WhichSelected = Selected.NotSpecified;

			if (UserInfo != null)
			{
				if (UserInfo.LastLoggedInSkillSetID == 0)
				{
					UserInfo.LastLoggedInCampaign = 0;
					UserInfo.LastLoggedInCharacter = 0;
					UserInfo.LastLoggedInMyCharOrCamp = "";
				}
			}

//            oLogWriter.AddLogMessage("About to check for session variables.", lsRoutineName, "", Session.SessionID);

            if ((Session["CampaignsToEdit"] == null) ||
                (Session["MyCharacters"] == null))
            {
                if ( _UserInfo == null )
                    _UserInfo = new cUser(_UserName, "PasswordNotNeeded", Session.SessionID);

                if (UserInfo.LastLoggedInMyCharOrCamp == "C")
                {
                    WhichSelected = Selected.CampaignCharacters;
					if (UserInfo.LastLoggedInSkillSetID != 0)
					{
						Session["CharCampaignCampaignID"] = UserInfo.LastLoggedInCampaign;
						Session["CharCampaignCharID"] = UserInfo.LastLoggedInCharacter;
						Session["CharCampaignSkillSetID"] = UserInfo.LastLoggedInSkillSetID;
					}
					else
					{
						Session.Remove("CharCampaignCampaignID");
						Session.Remove("CharCampaignCharID");
						Session.Remove("CharCampaignSkillSetID");
					}
                    Session["CharacterSelectGroup"] = "Campaigns";
                }
                else
                {
                    WhichSelected = Selected.MyCharacters;
					if (UserInfo.LastLoggedInSkillSetID != 0)
					{
						Session["CharSkillSetID"] = UserInfo.LastLoggedInSkillSetID;
						Session["CharCharacterID"] = UserInfo.LastLoggedInCharacter;
						Session["CharCampaignID"] = UserInfo.LastLoggedInCampaign;
					}
					else
					{
						Session.Remove("CharSkillSetID");
						Session.Remove("CharCharacterID");
						Session.Remove("CharCampaignID");
					}
                    Session["CharacterSelectGroup"] = "Characters";
                }
//                oLogWriter.AddLogMessage("Done loading user.", lsRoutineName, "", Session.SessionID);
            }

            if (Session["CharacterSelectGroup"] != null)
                if (Session["CharacterSelectGroup"].ToString() == "Campaigns")
                    WhichSelected = Selected.CampaignCharacters;
                else if (Session["CharacterSelectGroup"].ToString() == "Characters")
                    WhichSelected = Selected.MyCharacters;

            int iCampaignID = 0;

            // This is a fake out. If it's not defined we define it as a non valid value so it will fall through to the bottom.
            if (Session["CharacterSelectGroup"] == null)
                Session["CharacterSelectGroup"] = "PlaceHolder";

			int iCampSkillSetID = 0;
			int iCharSkillSetID = 0;
            _CharacterID = null;

            if ((WhichSelected == Selected.MyCharacters) &&
				(Session["CharSkillSetID"] != null))
            {
				if (int.TryParse(Session["CharSkillSetID"].ToString(), out iCharSkillSetID))
                {
					if (iCharSkillSetID != 0)
					{
						_SkillSetID = iCharSkillSetID;
						//                    _CharacterID = iCharID;
						_CharacterInfo.LoadCharacterBySkillSetID(iCharSkillSetID);
						_CharacterID = _CharacterInfo.CharacterID;
						_CampaignID = _CharacterInfo.CampaignID;
						Session["CharSkillSetID"] = iCharSkillSetID;
						Session["CharCharacterID"] = _CharacterInfo.CharacterID;
						Session["CharCampaignID"] = _CharacterInfo.CampaignID;
                    return;
                }
            }
			}

            if ((WhichSelected == Selected.CampaignCharacters) &&
				(Session["CharCampaignSkillSetID"] != null))
            {
				if (int.TryParse(Session["CharCampaignSkillSetID"].ToString(), out iCampSkillSetID))
                {
					if (iCampSkillSetID != 0)
					{
						_CharacterInfo.LoadCharacterBySkillSetID(iCampSkillSetID);
						_CharacterID = _CharacterInfo.CharacterID;
						_CampaignID = _CharacterInfo.CampaignID;
						_SkillSetID = iCampSkillSetID;
						Session["CharCampaignSkillSetID"] = iCampSkillSetID;
						Session["CharCampaignCharacterID"] = _CharacterInfo.CharacterID;
						Session["CharCampaignCampaignID"] = _CharacterInfo.CampaignID;
                    return;
                }
            }
			}

//            oLogWriter.AddLogMessage("About to run uspGetPrivCampaignCharacterEdit", lsRoutineName, "", Session.SessionID);

            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", _UserID);
            DataTable dtCampaignsToEdit = LarpPortal.Classes.cUtilities.LoadDataTable("uspGetPrivCampaignCharacterEdit", slParameters,
                "LARPortal", _UserName, lsRoutineName + ".uspGetPrivCampaignCharacterEdit");

//            oLogWriter.AddLogMessage("Done running uspGetPrivCampaignCharacterEdit", lsRoutineName, "", Session.SessionID);

            if (dtCampaignsToEdit.Rows.Count > 0)
            {
                DataView dvCampaignsToEdit = new DataView(dtCampaignsToEdit, "", "CampaignName", DataViewRowState.CurrentRows);
                if (int.TryParse(dvCampaignsToEdit[0]["CampaignID"].ToString(), out iCampaignID))
                {
//                    oLogWriter.AddLogMessage("About to run uspGetCampaignCharactersAll", lsRoutineName, "", Session.SessionID);

                    slParameters = new SortedList();
                    slParameters.Add("@CampaignID", iCampaignID);
                    DataTable dtCampChars = Classes.cUtilities.LoadDataTable("uspGetCampaignCharactersAll", slParameters, "LARPortal", _UserName, lsRoutineName + ".uspGetCampaignCharactersAll");

//                    oLogWriter.AddLogMessage("Done running uspGetCampaignCharactersAll", lsRoutineName, "", Session.SessionID);

					DataView dvCampChars = new DataView(dtCampChars, "", "DisplayName", DataViewRowState.CurrentRows);
                    if (dtCampChars.Rows.Count > 0)
                    {
						int.TryParse(dvCampChars[0]["CharacterSkillSetID"].ToString(), out iCampSkillSetID);
                    }
                }
            }

            // Now get  all characters for user.
            slParameters = new SortedList();
            slParameters.Add("@intUserID", _UserID);

//            oLogWriter.AddLogMessage("About to run uspGetCharacterIDsByUserID", lsRoutineName, "", Session.SessionID);

            DataTable dtCharacters = LarpPortal.Classes.cUtilities.LoadDataTable("uspGetCharacterIDsByUserID", slParameters, "LARPortal", _UserName, lsRoutineName + ".uspGetCharacterIDsByUserID");

//            oLogWriter.AddLogMessage("Done running uspGetCharacterIDsByUserID", lsRoutineName, "", Session.SessionID);

            if (dtCharacters.Rows.Count > 0)
            {
				DataView dvCharacters = new DataView(dtCharacters, "", "DisplayName", DataViewRowState.CurrentRows);
				int.TryParse(dvCharacters[0]["CharacterSkillSetID"].ToString(), out iCharSkillSetID);
            }

			if ((iCharSkillSetID != 0) &&
                ((WhichSelected == Selected.MyCharacters) || (WhichSelected == Selected.NotSpecified)))
            {
//                oLogWriter.AddLogMessage("About to load character in MyCharacters", lsRoutineName, "", Session.SessionID);
				_CharacterInfo.LoadCharacterBySkillSetID(iCharSkillSetID);
//                oLogWriter.AddLogMessage("Done loading character in MyCharacters", lsRoutineName, "", Session.SessionID);
				_CharacterID = _CharacterInfo.CharacterID;
				_CampaignID = _CharacterInfo.CampaignID;
				_SkillSetID = iCharSkillSetID;
                WhichSelected = Selected.MyCharacters;
                Session["CharacterSelectGroup"] = "Characters";
				Session["CharCharacterID"] = _CharacterInfo.CharacterID;
				Session["CharSkillSetID"] = iCharSkillSetID;
				Session["CharCampaignID"] = _CharacterInfo.CampaignID;
                ddlCampCharSelector.Visible = false;
                ddlCharacterSelector.Visible = true;
                return;
            }

			if (((iCampSkillSetID != 0) && (iCampaignID != 0)) &&
                ((WhichSelected == Selected.CampaignCharacters) || (WhichSelected == Selected.NotSpecified)))
            {
//                oLogWriter.AddLogMessage("About to load character in CampaignCharacters", lsRoutineName, "", Session.SessionID);
				_CharacterInfo.LoadCharacterBySkillSetID(iCampSkillSetID);
//                oLogWriter.AddLogMessage("Done loading character in CampaignCharacters", lsRoutineName, "", Session.SessionID);
                WhichSelected = Selected.CampaignCharacters;
                Session["CharacterSelectGroup"] = "Campaigns";
				Session["CharCampaignSkillSetID"] = iCampSkillSetID;
				Session["CharCampaignCharacterID"] = _CharacterInfo.CharacterID;
				Session["CharCampaignCampaignID"] = _CharacterInfo.CampaignID;
				_SkillSetID = iCampSkillSetID;
				_CharacterID = _CharacterInfo.CharacterID;
				_CampaignID = _CharacterInfo.CampaignID;

                ddlCampCharSelector.Visible = true;
                ddlCharacterSelector.Visible = false;
                return;
            }

            // If we got this far - there are nocharacters to select.
            ddlCharacterSelector.Visible = false;
            ddlCampCharSelector.Visible = false;
        }

        protected void rbMyCharacters_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMyCharacters.Checked)
            {
                Session["CharacterSelectGroup"] = "Characters";
                LoadMyCharacters();
            }
            else
            {
                Session["CharacterSelectGroup"] = "Campaigns";
                LoadCampaignCharacters();
            }

            LoadInfo();
            if (this.CharacterChanged != null)
                CharacterChanged(this, e);
        }

        protected void ddlCampCharSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CharacterSelectGroup"] = "Campaigns";
			int iSkillSetID;
			if (int.TryParse(ddlCampCharSelector.SelectedValue, out iSkillSetID))
            {
				if (iSkillSetID < 0)
                    Response.Redirect("~/Character/CharAdd.aspx", true);

                WhichSelected = Selected.CampaignCharacters;
                _CharacterInfo = new cCharacter();
				_CharacterInfo.LoadCharacterBySkillSetID(iSkillSetID);
				_SkillSetID = iSkillSetID;
				_CharacterID = _CharacterInfo.CharacterID;
				_CampaignID = _CharacterInfo.CampaignID;

				Session["CharCampaignSkillSetID"] = iSkillSetID;
				Session["CharCampaignCharID"] = _CharacterInfo.CharacterID;
				Session["CharCampaignCampaignID"] = _CharacterInfo.CampaignID;

				Classes.cUser UserInfo = new Classes.cUser(_UserName, "PasswordNotNeeded", Session.SessionID);
				UserInfo.LastLoggedInCampaign = _CampaignID.Value;
				UserInfo.LastLoggedInCharacter = _CharacterID.Value;
				UserInfo.LastLoggedInSkillSetID = _SkillSetID.Value;
				UserInfo.LastLoggedInMyCharOrCamp = "C";
				UserInfo.Save();

//				_CharacterInfo.LoadCharacterBySkillSetID(_SkillSetID.Value);
                if (this.CharacterChanged != null)
                    CharacterChanged(this, e);
            }
            else
                _CharacterID = null;
        }

        protected void ddlCharacterSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
			int iSkillSetID = 0;

            if (ddlCharacterSelector.SelectedIndex >= 0)
            {
				int.TryParse(ddlCharacterSelector.SelectedValue, out iSkillSetID);
				if (iSkillSetID == -1)
                {
                    // Person choose to add a new character.
                    Response.Redirect("~/Character/CharAdd.aspx", true);
                }

                _CharacterInfo = new cCharacter();
				_CharacterInfo.LoadCharacterBySkillSetID(iSkillSetID);

				_SkillSetID = iSkillSetID;
				_CharacterID = _CharacterInfo.CharacterID;
				_CampaignID = _CharacterInfo.CampaignID;

                Session["CharacterSelectGroup"] = "Characters";
				Session["CharSkillSetID"] = iSkillSetID;
				Session["CharCharacterID"] = _CharacterInfo.CharacterID;
				Session["CharCampaignID"] = _CharacterInfo.CampaignID;
                if (this.CharacterChanged != null)
                    CharacterChanged(this, e);
            }
            else
            {
                _CharacterID = null;
            }
        }

        protected void ddlCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCampaigns.SelectedIndex >= 0)
            {
				//                Session["CharacterSelectCampaign"] = ddlCampaigns.SelectedValue;
                Session["CharacterSelectGroup"] = "Campaigns";

                int iCampaignID = 0;
                if (int.TryParse(ddlCampaigns.SelectedValue, out iCampaignID))
                {
					Session["CharCampaignCampaignID"] = iCampaignID;
                    LoadCampaignCharacters();

					int iSkillSetID;
					if (int.TryParse(ddlCampCharSelector.SelectedValue, out iSkillSetID))
                    {
						if (iSkillSetID < 0)
                            Response.Redirect("~/Character/CharAdd.aspx", true);

                        WhichSelected = Selected.CampaignCharacters;

                        _CharacterInfo = new cCharacter();
						_CharacterInfo.LoadCharacterBySkillSetID(iSkillSetID);
						_SkillSetID = iSkillSetID;
						_CharacterID = _CharacterInfo.CharacterID;
						_CampaignID = _CharacterInfo.CampaignID;

						Session["CharCampaignSkillSetID"] = iSkillSetID;
						Session["CharCampaignCharID"] = _CharacterInfo.CharacterID;
						Session["CharCampaignCampaignID"] = _CharacterInfo.CampaignID;
						Session["CharacterSelectGroup"] = "Campaigns";
                        if (this.CharacterChanged != null)
                            CharacterChanged(this, e);
                    }
                }
            }
        }

        protected void LoadCampaignCharacters()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            DataTable dtCampaignsToEdit = Session["CampaignsToEdit"] as DataTable;

            ddlCharacterSelector.Visible = false;
            rbCampaignCharacters.Checked = true;
            int iCampaignID = 0;

			if (Session["CharCampaignCampaignID"] != null)
				int.TryParse(Session["CharCampaignCampaignID"].ToString(), out iCampaignID);

            if (dtCampaignsToEdit.Rows.Count == 1)
            {
                ddlCampaigns.Visible = false;
                lblSelectedCampaign.Visible = true;
                lblSelectedCampaign.Text = dtCampaignsToEdit.Rows[0]["CampaignName"].ToString();
				Session["CharCampaignCampaignID"] = dtCampaignsToEdit.Rows[0]["CampaignID"].ToString();
				//--Session.Remove("CharCampaignSkillSetID");
				//--Session.Remove("CharCampaignCharID");

				//int.TryParse(dtCampaignsToEdit.Rows[0]["CampaignID"].ToString(), out iCampaignID);
				//            Session["CharacterID"] = iCampaignID;

				//_CharacterInfo = new cCharacter();
				//_CharacterInfo.LoadCharacterBySkillSetID(iSkillSetID);
				//_SkillSetID = iSkillSetID;
				//_CharacterID = _CharacterInfo.CharacterID;
				//_CampaignID = _CharacterInfo.CamaignID;

				//Session["CampSkillSetID"] = iSkillSetID;
				//Session["CampCharacterID"] = _CharacterInfo.CharacterID;
				//Session["CampCampaignID"] = _CharacterInfo.CampaignID;
            }
            else
            {
                ddlCampaigns.Visible = true;
                lblSelectedCampaign.Visible = false;

                ddlCampaigns.ClearSelection();

                if (iCampaignID != 0)
                {
                    foreach (ListItem liCamp in ddlCampaigns.Items)
                        if (liCamp.Value == iCampaignID.ToString())
                            liCamp.Selected = true;
                    //}
                    //if (iCampaignID != 0)       // Campaign is selected.
                    //{
                    //            Session["CharacterSelectCampaign"] = iCampaignID.ToString();
                    //int iCampaignID;
                    //int.TryParse(Session["CharacterSelectCampaign"].ToString(), out iCampaignID);
                }
            }
            SortedList slParameters = new SortedList();
            slParameters.Add("@CampaignID", iCampaignID);
            DataTable dtChars = Classes.cUtilities.LoadDataTable("uspGetCampaignCharactersAll", slParameters, "LARPortal", Session["UserID"].ToString(), lsRoutineName);
			DataView dvChars = new DataView(dtChars, "", "CharacterType, DisplayName", DataViewRowState.CurrentRows);

            ddlCampCharSelector.DataSource = dvChars;
			ddlCampCharSelector.DataTextField = "DisplayName";
			ddlCampCharSelector.DataValueField = "CharacterSkillSetID";
            ddlCampCharSelector.DataBind();
            ddlCampCharSelector.Visible = true;
            ddlCharacterSelector.Visible = false;

            //if (dvChars.Count == 0)
            //    ddlCampCharSelector.Items.Add(new ListItem("", "0"));

            //ddlCampCharSelector.Items.Add(new ListItem("Add A New Character", "-1"));

            ddlCampCharSelector.ClearSelection();

			if (Session["CharCampaignSkillSetID"] != null)
            {
				int iSkillSetID;
				if (int.TryParse(Session["CharCampaignSkillSetID"].ToString(), out iSkillSetID))
                {
                    foreach (ListItem liItem in ddlCampCharSelector.Items)
						if (liItem.Value == iSkillSetID.ToString())
                            liItem.Selected = true;
                }
            }
            if (ddlCampCharSelector.SelectedIndex < 0)
                if (ddlCampCharSelector.Items.Count > 0)
                    ddlCampCharSelector.Items[0].Selected = true;

            if (ddlCampCharSelector.SelectedValue == "0")       // There were no values.
            {
                _CharacterID = null;
                _CharacterInfo = null;
				Session.Remove("CharCampaignCharID");
				Session.Remove("CharCampaignSkillSetID");
            }
            else
            {
				int iSkillSetID;
				if (int.TryParse(ddlCampCharSelector.SelectedValue, out iSkillSetID))
                {
					//                    _CharacterID = iTempCharID;
					_CharacterInfo.LoadCharacterBySkillSetID(iSkillSetID);
					Session["CharCampaignSkillSetID"] = iSkillSetID;
					Session["CharCampaignCharID"] = _CharacterInfo.CharacterID;
					Session["CharCampaignCampaignID"] = _CharacterInfo.CampaignID;
                }
            }
            //  }
            //}
        }

        protected void LoadMyCharacters()
        {
            SortedList slParameters = new SortedList();
            slParameters.Add("@intUserID", Session["UserID"].ToString());

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            DataTable dtCharacters = LarpPortal.Classes.cUtilities.LoadDataTable("uspGetCharacterIDsByUserID", slParameters,
                "LARPortal", "Character", lsRoutineName);

			DataView dvCharacters = new DataView(dtCharacters, "", "DisplayName", DataViewRowState.CurrentRows);
            ddlCharacterSelector.DataSource = dvCharacters;
			ddlCharacterSelector.DataTextField = "DisplayName";
			ddlCharacterSelector.DataValueField = "CharacterSkillSetID";
            ddlCharacterSelector.DataBind();
            ddlCharacterSelector.Visible = true;
            ddlCampCharSelector.Visible = false;
            lblSelectedCampaign.Visible = false;
            rbMyCharacters.Checked = true;

            ddlCharacterSelector.ClearSelection();

			int iSkillSetID;
			if (Session["CharSkillSetID"] != null)
            {
				if (int.TryParse(Session["CharSkillSetID"].ToString(), out iSkillSetID))
                {
                    foreach (ListItem liItem in ddlCharacterSelector.Items)
                    {
						ddlCharacterSelector.ClearSelection();
						if (liItem.Value == iSkillSetID.ToString())
                            liItem.Selected = true;
                    }
                }
            }

            if (ddlCharacterSelector.SelectedIndex < 0)
                ddlCharacterSelector.Items[0].Selected = true;

			if (int.TryParse(ddlCharacterSelector.SelectedValue, out iSkillSetID))
				if (iSkillSetID == 0)
                {
                    _CharacterID = null;
                    _CharacterInfo = null;
					Session.Remove("CharSkillSetID");
                }
                else
                {
					_SkillSetID = iSkillSetID;
					_CharacterInfo = new cCharacter();
					_CharacterInfo.LoadCharacterBySkillSetID(iSkillSetID);
                    lblSelectedCampaign.Visible = true;
                    lblSelectedCampaign.Text = _CharacterInfo.CampaignName;
                    ddlCampaigns.Visible = false;

					_SkillSetID = iSkillSetID;
					_CharacterID = _CharacterInfo.CharacterID;
					_CampaignID = _CharacterInfo.CampaignID;

					Session["CharSkillSetID"] = iSkillSetID;
					Session["CharCharacterID"] = _CharacterInfo.CharacterID;
					Session["CharCampaignID"] = _CharacterInfo.CampaignID;
                }
        }


        public void Reset()
        {
			if (Session["CharCampaignCampaignID"] != null)
				Session.Remove("CharCampaignCampaignID");
			if (Session["CharCampaignCharID"] != null)
				Session.Remove("CharCampaignCharID");
			if (Session["CharCampaignSkillSetID"] != null)
				Session.Remove("CharCampaignSkillSetID");
            if (Session["CharacterSelectGroup"] != null)
                Session.Remove("CharacterSelectGroup");
			if (Session["CharSkillSetID"] != null)
				Session.Remove("CharSkillSetID");
			if (Session["CharCharID"] != null)
				Session.Remove("CharCharID");
			if (Session["CharCampaignID"] != null)
				Session.Remove("CharCampaignID");
			if (Session["MyCharacters"] != null)
				Session.Remove("MyCharacters");
            if (Session["CampaignsToEdit"] != null)
                Session.Remove("CampaignsToEdit");
			_UserInfo = null;
		}

		public void LoadAndSetCharacter()
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			WhichSelected = Selected.NotSpecified;

			if ((Session ["CampaignsToEdit"] == null) ||
				(Session ["MyCharacters"] == null))
			{
				if (_UserInfo == null)
					_UserInfo = new cUser(_UserName, "PasswordNotNeeded", Session.SessionID);

//				oLogWriter.AddLogMessage("Done loading user.", lsRoutineName, "", Session.SessionID);
				_CharacterInfo = new cCharacter();
				_CharacterInfo.LoadCharacterBySkillSetID(UserInfo.LastLoggedInSkillSetID);

				_SkillSetID = UserInfo.LastLoggedInSkillSetID;
				_CharacterID = _CharacterInfo.CharacterID;
				_CampaignID = _CharacterInfo.CampaignID;

				Session["CharSelectSetID"] = UserInfo.LastLoggedInCharacter;
				if (UserInfo.LastLoggedInMyCharOrCamp == "C")
				{
					WhichSelected = Selected.CampaignCharacters;
					Session["CharCampaignSkillSetID"] = _UserInfo.LastLoggedInSkillSetID;
					Session["CharCampaignCharID"] = _CharacterInfo.CharacterID;
					Session["CharCampaignCampaignID"] = _CharacterInfo.CampaignID;
					Session ["CharacterSelectGroup"] = "Campaigns";
				}
				else
				{
					WhichSelected = Selected.MyCharacters;
					Session["CharSkillSetID"] = UserInfo.LastLoggedInSkillSetID;
					Session["CharCharacterID"] = _CharacterInfo.CharacterID;
					Session["CharCampaignID"] = _CharacterInfo.CampaignID;
				}
			}
		}
	}
}