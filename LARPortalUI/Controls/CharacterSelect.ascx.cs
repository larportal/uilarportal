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
        private int _UserID = 0;
        private string _UserName = "";

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
                Session["CharacterSelectID"] = UserInfo.LastLoggedInCharacter;
				_CharacterID = UserInfo.LastLoggedInCharacter;
				if (UserInfo.LastLoggedInMyCharOrCamp == "C")
                {
                    WhichSelected = Selected.CampaignCharacters;
                    Session["CharacterSelectCampaign"] = UserInfo.LastLoggedInCampaign;
                    Session["CharacterCampaignCharID"] = UserInfo.LastLoggedInCharacter;
                    Session["CharacterSelectGroup"] = "Campaigns";
                }
                else
                {
                    WhichSelected = Selected.MyCharacters;
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
                Session["CharacterSelectID"] = UserInfo.LastLoggedInCharacter;
                if (UserInfo.LastLoggedInMyCharOrCamp == "C")
                {
                    WhichSelected = Selected.CampaignCharacters;
                    Session["CharacterSelectCampaign"] = UserInfo.LastLoggedInCampaign;
                    Session["CharacterCampaignCharID"] = UserInfo.LastLoggedInCharacter;
                    Session["CharacterSelectGroup"] = "Campaigns";
                }
                else
                {
                    WhichSelected = Selected.MyCharacters;
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
                    Response.Redirect("~/Character/CharAdd.aspx", true);

                dtMyCharacters = new DataView(dtCharacters, "", "CharacterAKA", DataViewRowState.CurrentRows).ToTable(true, "CharacterAKA", "CharacterID");
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
                DataView dvCharacters = new DataView(dtMyCharacters, "", "CharacterAKA", DataViewRowState.CurrentRows);
                ddlCharacterSelector.DataSource = dvCharacters;
                ddlCharacterSelector.DataTextField = "CharacterAKA";
                ddlCharacterSelector.DataValueField = "CharacterID";
                ddlCharacterSelector.DataBind();
                ddlCharacterSelector.Visible = true;
                ddlCampCharSelector.Visible = false;
                lblSelectedCampaign.Visible = false;
                rbMyCharacters.Checked = true;

                ddlCharacterSelector.ClearSelection();

                if (Session["CharacterSelectID"] != null)
                {
                    int iCharID;
                    if (int.TryParse(Session["CharacterSelectID"].ToString(), out iCharID))
                    {
                        foreach (ListItem liItem in ddlCharacterSelector.Items)
                        {
							if (liItem.Value == iCharID.ToString())
							{
								ddlCharacterSelector.ClearSelection();
								liItem.Selected = true;
							}
                        }
                    }
                }

                if (ddlCharacterSelector.SelectedIndex < 0)
                    ddlCharacterSelector.Items[0].Selected = true;

                int iTempCharID;
                if (int.TryParse(ddlCharacterSelector.SelectedValue, out iTempCharID))
                    if (iTempCharID == 0)
                    {
                        _CharacterID = null;
                        _CharacterInfo = null;
                        Session["CharacterSelectID"] = "0";
                    }
                    else
                    {
                        _CharacterID = iTempCharID;
                        _CharacterInfo.LoadCharacter(iTempCharID);
                        lblSelectedCampaign.Visible = true;
                        lblSelectedCampaign.Text = _CharacterInfo.CampaignName;
                        ddlCampaigns.Visible = false;
                        Session["CharacterSelectID"] = iTempCharID;
                        Session["CampaignID"] = _CharacterInfo.CampaignID;
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

//            oLogWriter.AddLogMessage("About to check for session variables.", lsRoutineName, "", Session.SessionID);

            if ((Session["CampaignsToEdit"] == null) ||
                (Session["MyCharacters"] == null))
            {
                if ( _UserInfo == null )
                    _UserInfo = new cUser(_UserName, "PasswordNotNeeded", Session.SessionID);

                if (UserInfo.LastLoggedInMyCharOrCamp == "C")
                {
                    WhichSelected = Selected.CampaignCharacters;
                    Session["CharacterSelectCampaign"] = UserInfo.LastLoggedInCampaign;
                    Session["CharacterCampaignCharID"] = UserInfo.LastLoggedInCharacter;
                    Session["CharacterSelectGroup"] = "Campaigns";
                }
                else
                {
                    WhichSelected = Selected.MyCharacters;
                    Session["CharacterSelectID"] = UserInfo.LastLoggedInCharacter;
                    Session["CharacterSelectGroup"] = "Characters";
                }
//                oLogWriter.AddLogMessage("Done loading user.", lsRoutineName, "", Session.SessionID);
            }

            if (Session["CharacterSelectGroup"] != null)
                if (Session["CharacterSelectGroup"].ToString() == "Campaigns")
                    WhichSelected = Selected.CampaignCharacters;
                else if (Session["CharacterSelectGroup"].ToString() == "Characters")
                    WhichSelected = Selected.MyCharacters;

            int iCharID = 0;
            int iCampaignID = 0;
            int iCampCharID = 0;

            // This is a fake out. If it's not defined we define it as a non valid value so it will fall through to the bottom.
            if (Session["CharacterSelectGroup"] == null)
                Session["CharacterSelectGroup"] = "PlaceHolder";

            _CharacterID = null;

            if ((WhichSelected == Selected.MyCharacters) &&
                (Session["CharacterSelectID"] != null))
            {
                if (int.TryParse(Session["CharacterSelectID"].ToString(), out iCharID))
                {
                    _CharacterID = iCharID;
                    _CharacterInfo.LoadCharacter(iCharID);
                    Session["CampaignID"] = _CharacterInfo.CampaignID;
                    return;
                }
            }

            if ((WhichSelected == Selected.CampaignCharacters) &&
                (Session["CharacterCampaignCharID"] != null))
            {
                if (int.TryParse(Session["CharacterCampaignCharID"].ToString(), out iCharID))
                {
                    _CharacterID = iCharID;
                    _CharacterInfo.LoadCharacter(iCharID);
                    Session["CampaignID"] = _CharacterInfo.CampaignID;
                    return;
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

                    DataView dvCampChars = new DataView(dtCampChars, "", "CharacterAKA", DataViewRowState.CurrentRows);
                    if (dtCampChars.Rows.Count > 0)
                    {
                        int.TryParse(dvCampChars[0]["CharacterID"].ToString(), out iCampCharID);
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
                DataView dvCharacters = new DataView(dtCharacters, "", "CharacterAKA", DataViewRowState.CurrentRows);
                int.TryParse(dvCharacters[0]["CharacterID"].ToString(), out iCharID);
            }

            if ((iCharID != 0) &&
                ((WhichSelected == Selected.MyCharacters) || (WhichSelected == Selected.NotSpecified)))
            {
                _CharacterID = iCharID;
//                oLogWriter.AddLogMessage("About to load character in MyCharacters", lsRoutineName, "", Session.SessionID);
                _CharacterInfo.LoadCharacter(iCharID);
//                oLogWriter.AddLogMessage("Done loading character in MyCharacters", lsRoutineName, "", Session.SessionID);
                WhichSelected = Selected.MyCharacters;
                Session["CharacterSelectGroup"] = "Characters";
                Session["CharacterSelectID"] = iCharID;
                Session["CampaignID"] = _CharacterInfo.CampaignID;
                ddlCampCharSelector.Visible = false;
                ddlCharacterSelector.Visible = true;
                return;
            }

            if (((iCampCharID != 0) && (iCampaignID != 0)) &&
                ((WhichSelected == Selected.CampaignCharacters) || (WhichSelected == Selected.NotSpecified)))
            {
                _CharacterID = iCampCharID;
//                oLogWriter.AddLogMessage("About to load character in CampaignCharacters", lsRoutineName, "", Session.SessionID);
                _CharacterInfo.LoadCharacter(iCampCharID);
//                oLogWriter.AddLogMessage("Done loading character in CampaignCharacters", lsRoutineName, "", Session.SessionID);
                WhichSelected = Selected.CampaignCharacters;
                Session["CharacterSelectGroup"] = "Campaigns";
                Session["CharacterCampaignCharID"] = iCharID;
                Session["CharacterSelectCampaign"] = _CharacterInfo.CampaignID;
                Session["CampaignID"] = _CharacterInfo.CampaignID;
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
            int iCharID;
            if (int.TryParse(ddlCampCharSelector.SelectedValue, out iCharID))
            {
                if (iCharID < 0)
                    Response.Redirect("~/Character/CharAdd.aspx", true);

                WhichSelected = Selected.CampaignCharacters;
                _CharacterID = iCharID;
                _CharacterInfo = new cCharacter();
                _CharacterInfo.LoadCharacter(iCharID);
                Session["CharacterCampaignCharID"] = iCharID;
                Session["CampaignID"] = _CharacterInfo.CampaignID;
                if (this.CharacterChanged != null)
                    CharacterChanged(this, e);
            }
            else
                _CharacterID = null;
        }

        protected void ddlCharacterSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iCharID = 0;

            if (ddlCharacterSelector.SelectedIndex >= 0)
            {
                int.TryParse(ddlCharacterSelector.SelectedValue, out iCharID);
                if (iCharID == -1)
                {
                    // Person choose to add a new character.
                    Response.Redirect("~/Character/CharAdd.aspx", true);
                }

                _CharacterID = iCharID;
                _CharacterInfo = new cCharacter();
                _CharacterInfo.LoadCharacter(iCharID);
                Session["CharacterSelectGroup"] = "Characters";
                Session["CharacterSelectID"] = iCharID;
                Session["CampaignID"] = _CharacterInfo.CampaignID;
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
                Session["CharacterSelectCampaign"] = ddlCampaigns.SelectedValue;
                Session["CharacterSelectGroup"] = "Campaigns";

                int iCampaignID = 0;
                if (int.TryParse(ddlCampaigns.SelectedValue, out iCampaignID))
                {
                    LoadCampaignCharacters();

                    int iCharID;
                    if (int.TryParse(ddlCampCharSelector.SelectedValue, out iCharID))
                    {
                        if (iCharID < 0)
                            Response.Redirect("~/Character/CharAdd.aspx", true);

                        WhichSelected = Selected.CampaignCharacters;
                        _CharacterID = iCharID;
                        _CharacterInfo = new cCharacter();
                        _CharacterInfo.LoadCharacter(iCharID);
                        Session["CharacterCampaignCharID"] = iCharID;
                        Session["CampaignID"] = _CharacterInfo.CampaignID;
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

            if (Session["CharacterSelectCampaign"] != null)
                int.TryParse(Session["CharacterSelectCampaign"].ToString(), out iCampaignID);

            if (dtCampaignsToEdit.Rows.Count == 1)
            {
                ddlCampaigns.Visible = false;
                lblSelectedCampaign.Visible = true;
                lblSelectedCampaign.Text = dtCampaignsToEdit.Rows[0]["CampaignName"].ToString();
                Session["CharacterSelectCampaign"] = dtCampaignsToEdit.Rows[0]["CampaignID"].ToString();
                int.TryParse(dtCampaignsToEdit.Rows[0]["CampaignID"].ToString(), out iCampaignID);
                Session["CharacterID"] = iCampaignID;
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
            DataView dvChars = new DataView(dtChars, "", "CharacterType, CharacterAKA", DataViewRowState.CurrentRows);

            ddlCampCharSelector.DataSource = dvChars;
            ddlCampCharSelector.DataTextField = "CharacterName";
            ddlCampCharSelector.DataValueField = "CharacterID";
            ddlCampCharSelector.DataBind();
            ddlCampCharSelector.Visible = true;
            ddlCharacterSelector.Visible = false;

            //if (dvChars.Count == 0)
            //    ddlCampCharSelector.Items.Add(new ListItem("", "0"));

            //ddlCampCharSelector.Items.Add(new ListItem("Add A New Character", "-1"));

            ddlCampCharSelector.ClearSelection();

            if (Session["CharacterCampaignCharID"] != null)
            {
                int iChar;
                if (int.TryParse(Session["CharacterCampaignCharID"].ToString(), out iChar))
                {
                    foreach (ListItem liItem in ddlCampCharSelector.Items)
                        if (liItem.Value == iChar.ToString())
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
                Session["CharacterCampaignCharID"] = "0";
            }
            else
            {
                int iTempCharID;
                if (int.TryParse(ddlCampCharSelector.SelectedValue, out iTempCharID))
                {
                    _CharacterID = iTempCharID;
                    _CharacterInfo.LoadCharacter(iTempCharID);
                    //                    lblSelectedCampaign.Visible = true;
                    //                    lblSelectedCampaign.Text = _CharacterInfo.CampaignName;
                    //                    ddlCampaigns.Visible = false;
                    Session["CampaignID"] = _CharacterInfo.CampaignID;
                    Session["CharacterCampaignCharID"] = iTempCharID;
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

            DataView dvCharacters = new DataView(dtCharacters, "", "CharacterAKA", DataViewRowState.CurrentRows);
            ddlCharacterSelector.DataSource = dvCharacters;
            ddlCharacterSelector.DataTextField = "CharacterAKA";
            ddlCharacterSelector.DataValueField = "CharacterID";
            ddlCharacterSelector.DataBind();
            ddlCharacterSelector.Visible = true;
            ddlCampCharSelector.Visible = false;
            lblSelectedCampaign.Visible = false;
            rbMyCharacters.Checked = true;

            ddlCharacterSelector.ClearSelection();

            if (Session["CharacterSelectID"] != null)
            {
                int iCharID;
                if (int.TryParse(Session["CharacterSelectID"].ToString(), out iCharID))
                {
                    foreach (ListItem liItem in ddlCharacterSelector.Items)
                    {
						ddlCharacterSelector.ClearSelection();
                        if (liItem.Value == iCharID.ToString())
                            liItem.Selected = true;
                    }
                }
            }

            if (ddlCharacterSelector.SelectedIndex < 0)
                ddlCharacterSelector.Items[0].Selected = true;

            int iTempCharID;
            if (int.TryParse(ddlCharacterSelector.SelectedValue, out iTempCharID))
                if (iTempCharID == 0)
                {
                    _CharacterID = null;
                    _CharacterInfo = null;
                    Session["CharacterSelectID"] = "0";
                }
                else
                {
                    _CharacterID = iTempCharID;
                    _CharacterInfo.LoadCharacter(iTempCharID);
                    lblSelectedCampaign.Visible = true;
                    lblSelectedCampaign.Text = _CharacterInfo.CampaignName;
                    ddlCampaigns.Visible = false;
                    Session["CharacterSelectID"] = iTempCharID;
                    Session["CampaignID"] = _CharacterInfo.CampaignID;
                }
        }


        public void Reset()
        {
            if (Session["CharacterCampaignCharID"] != null)
                Session.Remove("CharacterCampaignCharID");

            if (Session["CharacterSelectCampaign"] != null)
                Session.Remove("CharacterSelectCampaign");

            if (Session["CharacterSelectGroup"] != null)
                Session.Remove("CharacterSelectGroup");

            if (Session["CharacterSelectID"] != null)
                Session.Remove("CharacterSelectID");

            if (Session["CampaignsToEdit"] != null)
                Session.Remove("CampaignsToEdit");

            if (Session["MyCharacters"] != null)
                Session.Remove("MyCharacters");

			_UserInfo = new Classes.cUser(_UserName, "PasswordNotNeeded", Session.SessionID);
//			Session ["CharacterSelectID"] = _UserInfo.LastLoggedInCharacter;

//			LoadAndSetCharacter();
			//_UserInfo.LastLoggedInCharacter = -1;
			//_UserInfo.Save();
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
				Session ["CharacterSelectID"] = UserInfo.LastLoggedInCharacter;
				if (UserInfo.LastLoggedInMyCharOrCamp == "C")
				{
					WhichSelected = Selected.CampaignCharacters;
					Session ["CharacterSelectCampaign"] = UserInfo.LastLoggedInCampaign;
					Session ["CharacterCampaignCharID"] = UserInfo.LastLoggedInCharacter;
					Session ["CharacterSelectGroup"] = "Campaigns";
				}
				else
				{
					WhichSelected = Selected.MyCharacters;
				}
			}
		}
	}
}