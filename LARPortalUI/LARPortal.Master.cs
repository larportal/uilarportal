using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
    public partial class LARPortal : System.Web.UI.MasterPage
    {
        public event EventHandler CampaignChanged;

        public string UserName
        {
            get
            {
                if (Session["UserName"] != null)
                    return Session["UserName"].ToString();
                else
                {
                    Response.Redirect("/index.aspx", true);
                    return "";      // It won't get here - but the compiler complains if there is no return.
                }
            }
        }

        public int UserID
        {
            get
            {
                if (Session["Guest"] != null)
                    return -1;

                int iUserID = 0;
                if (Session["UserID"] != null)
                    if (int.TryParse(Session["UserID"].ToString(), out iUserID))
                        return iUserID;

                // If we got this far - there was problem with the ID.
                //                int i = 0;
                //                int j = 12 / i;
                Response.Redirect("/index.aspx", true);
                return -1;      // It won't get here - but the compiler complains if there is no return.
            }
        }

        public int CampaignID
        {
            get
            {
                if (Session["Guest"] != null)
                    return -102;

                int iCampID = 0;
                if (Session["CampaignID"] != null)
                    if (int.TryParse(Session["CampaignID"].ToString(), out iCampID))
                        return iCampID;

                // if we got this far, there was a problem. Load the data so the values are filled in.
                LoadData();
                if (Session["CampaignID"] != null)
                    if (int.TryParse(Session["CampaignID"].ToString(), out iCampID))
                        return iCampID;

                // Still has a problem.
                return -1;
            }
        }

        public string CampaignName
        {
            get
            {
                if (Session["Guest"] != null)
                    return "All Campaigns";

                // Assume everything is OK.
                return Session["CampaignName"].ToString();
            }
        }

        public string RoleString
        {
            get
            {
                return Session["RoleString"].ToString();
            }
        }

        public bool SuperUser
        {
            get
            {
                if (Session["SuperUser"] != null)
                    return true;
                else
                    return false;
            }
        }

        public bool DisplayAllOptions { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string l = (string) Session ["UserName"];
            //l = (string) Session ["UserID"];

            if ((Session["UserName"] == null) ||
                (Session["UserID"] == null))
            {
                Response.Redirect("/index.aspx", true);
            }

        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (Request.Url.Host.ToUpper().Contains("BETA."))
            //{
            //    lblMessage.Text = "Beta Site";
            //    SqlConnectionStringBuilder ConnPieces = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString);
            //    if (ConnPieces != null)
            //        lblMessage.Text += "  Database: " + ConnPieces.InitialCatalog;
            //}
            //else if (Request.Url.Host.ToUpper().Contains("LOCALHOST"))
            //{
            //    lblMessage.Text = "Local Host";
            //    SqlConnectionStringBuilder ConnPieces = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString);
            //    if (ConnPieces != null)
            //        lblMessage.Text += "  Database: " + ConnPieces.InitialCatalog;
            //}

            if (Session["CompileDate"] is null)
            {
                DateTime dtCompileTime = Classes.cCompileDate.GetLinkerDateTime(Assembly.GetExecutingAssembly());
                string sCompileTime = "Compiled: " + dtCompileTime.ToString("g", CultureInfo.CreateSpecificCulture("en-US"));
                Session["CompileDate"] = sCompileTime;
            }
            SqlConnectionStringBuilder ConnPieces = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString);
            if (ConnPieces != null)
                lblMessage.Text = Session["CompileDate"].ToString() + "&nbsp;&nbsp;&nbsp;Database: " + ConnPieces.InitialCatalog;

            lblMessage.ForeColor = System.Drawing.Color.Transparent;

            if ((Session["SuperUser"] != null) ||
                (Request.Url.Host.ToUpper().Contains("BETA.")) ||
                (Request.Url.Host.ToUpper().Contains("LOCALHOST")))
                lblMessage.ForeColor = System.Drawing.Color.White;

            Classes.cLogin SiteFooter = new Classes.cLogin();
            SiteFooter.SetPageFooter();
            lblFooter.Text = SiteFooter.SiteFooter;

            if ((!IsPostBack) || (Session["ReloadCampaigns"] != null))
            {
                if (Session["ReloadCampaigns"] != null)
                {
                    Session.Remove("CampaignID");
                    Session.Remove("CampaignName");
                    Session.Remove("CampaignList");
                    Session.Remove("RoleString");
                }

                Session.Remove("ReloadCampaigns");
                if (Session["Guest"] != null)
                {
                    mvMenuArea.SetActiveView(vwGuest);
                    lblUserName.Text = "Guest";
                    return;
                }
                mvMenuArea.SetActiveView(vwFullMenu);
                lblUserName.Text = Session["UserName"].ToString();
                if (!DisplayAllOptions)
                {
                    if (Session["CampaignID"] != null)
                    {
                        int iCampaignID;
                        if (int.TryParse(Session["CampaignID"].ToString(), out iCampaignID))
                            if (iCampaignID < 0)
                                Session.Remove("CampaignID");
                    }
                }
                LoadData();

            }
            Classes.cPlayerRoles Roles = new Classes.cPlayerRoles();
            Roles.Load(UserID, 0, CampaignID, DateTime.Today);
            Classes.cURLPermission permissions = new Classes.cURLPermission();
            bool PagePermission = true;
            string DefaultUnauthorizedURL = "";
            string CurrentPage = Request.RawUrl;
            if (CurrentPage.ToUpper().EndsWith(".ASPX"))
                CurrentPage = CurrentPage.Substring(0, CurrentPage.Length - 5);

            permissions.GetURLPermissions(CurrentPage, UserName, Roles.PlayerRoleString);
            PagePermission = permissions.PagePermission;
            DefaultUnauthorizedURL = permissions.DefaultUnauthorizedURL;
            if (!PagePermission)
                Response.Redirect(DefaultUnauthorizedURL);

            // Save current page to database so when person logs back in it can go to last page.
            string PageName = Request.Url.AbsolutePath;
            Classes.cLogin LastLoggedIn = new Classes.cLogin();
            LastLoggedIn.LogLastPage(UserID, PageName);
        }


        protected void ddlCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (ddlCampaigns.SelectedValue == "-1")
            {
                Response.Redirect("~/Campaigns/JoinACampaign.aspx");
            }

            //			Classes.LogWriter oLogWriter = new Classes.LogWriter();

            // If the campaign ID has changed...
            if (CampaignID.ToString() != ddlCampaigns.SelectedValue)
            {
                //				oLogWriter.AddLogMessage("Starting to change campaign from " + CampaignID.ToString() + " to " + ddlCampaigns.SelectedValue, "Master.ddlCampaigns_SelectedIndexChanged", "", Session.SessionID);
                int iCampaignID;
                if (int.TryParse(ddlCampaigns.SelectedValue, out iCampaignID))
                {
                    Session["CampaignID"] = iCampaignID;
                    Session["CampaignName"] = ddlCampaigns.SelectedItem.Text;

                    //					oLogWriter.AddLogMessage("New campaign will be " + iCampaignID.ToString(), "Master.ddlCampaigns_SelectedIndexChanged", "", Session.SessionID);

                    if (iCampaignID > 0)
                    {
                        // Since the campaign has changed, need to save the last campaign to the database.

                        Classes.cUser UserInfo = new Classes.cUser(UserName, "NOPASSWORD", Session.SessionID);
                        Classes.cUtilities utilities = new Classes.cUtilities();
                        SortedList sParams = new SortedList();
                        sParams.Add("@intUserId", UserID);
                        DataTable dtCharList = Classes.cUtilities.LoadDataTable("uspGetCharacterIDsByUserID", sParams, "LARPortal", UserName, lsRoutineName + ".GetCharList");
                        DataView dvCampChar = new DataView(dtCharList, "CampaignID = " + iCampaignID.ToString(), "CharacterAKA", DataViewRowState.CurrentRows);
                        if (dvCampChar.Count > 0)
                        {
                            int iTemp;
                            if (int.TryParse(dvCampChar[0]["CharacterID"].ToString(), out iTemp))
                            {
                                UserInfo.LastLoggedInCharacter = iTemp;
                                UserInfo.LastLoggedInMyCharOrCamp = "M";
                            }
                        }
                        else
                        {
                            UserInfo.LastLoggedInCharacter = -1;
                        }
                        UserInfo.LastLoggedInCampaign = iCampaignID;
                        UserInfo.Save();

                        Classes.cCampaignBase Campaign = new Classes.cCampaignBase(iCampaignID, UserName, UserID);






                        //oLogWriter.AddLogMessage("UserInfo was saved." + UserInfo.LastLoggedInCampaign.ToString(), "Master.ddlCampaigns_SelectedIndexChanged", "", Session.SessionID);
                        //Classes.cUser NewUserInfo = new Classes.cUser(UserName, "NOPASSWORD", Session.SessionID);
                        //oLogWriter.AddLogMessage("CampaignID after update was " + NewUserInfo.LastLoggedInCampaign.ToString(), "Master.ddlCampaigns_SelectedIndexChanged", "", Session.SessionID);

                        // Now that we have saved the campaign ID to the database, clear the session variables so it will force a reload.
                        Session.Remove("CampaignID");
                        Session.Remove("CampaignName");
                        Session.Remove("CampaignList");
                        Session.Remove("RoleString");

                        LoadData();

                        if (CampaignChanged != null)
                            CampaignChanged(this, EventArgs.Empty);
                    }
                    //					oLogWriter.AddLogMessage("Done with adding." + CampaignID.ToString(), "Master.ddlCampaigns_SelectedIndexChanged", "", Session.SessionID);
                }
            }
        }

        private void LoadData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if ((Session["UserName"] == null) ||
                (Session["UserID"] == null))
            {
                Response.Redirect("/index.aspx", true);
            }

            //			Classes.LogWriter oLogWriter = new Classes.LogWriter();

            lblUserName.Text = this.UserName;

            if ((Session["CampaignID"] == null) ||
                (Session["CampaignName"] == null) ||
                (Session["CampaignList"] == null) ||
                (Session["RoleString"] == null) ||
                (Session["AllowAdditionalInfo"] == null))
            {
                Classes.cUserCampaigns CampaignChoices = new Classes.cUserCampaigns();
                CampaignChoices.Load(UserID);

                //foreach (Classes.cUserCampaign Camp in CampaignChoices.lsUserCampaigns)
                //{
                //					oLogWriter.AddLogMessage("ID:" + Camp.CampaignID.ToString() + "/" + Camp.CampaignName + "/" + Camp.LastLoggedInCampaign.ToString(), "Master.LoadData", "", Session.SessionID);
                //}

                int iCampID = -1;
                Session["CampaignID"] = "-1";
                Session["CampaignName"] = "";

                //				oLogWriter.AddLogMessage("Starting: iCampID = " + iCampID.ToString(), "Master.LoadData", "", Session.SessionID);

                if (CampaignChoices.CountOfUserCampaigns > 0)
                {
                    // If last logged in campaign is not set, set it to the first value.
                    if (CampaignChoices.lsUserCampaigns[0].LastLoggedInCampaign == 0)
                        CampaignChoices.lsUserCampaigns[0].LastLoggedInCampaign = CampaignChoices.lsUserCampaigns[0].CampaignID;

                    Session["CampaignID"] = CampaignChoices.lsUserCampaigns[0].LastLoggedInCampaign.ToString();
                    Session["AllowAdditionalInfo"] = "FALSE";
                    foreach (Classes.cUserCampaign Camp in CampaignChoices.lsUserCampaigns)
                    {
                        if (Camp.LastLoggedInCampaign == Camp.CampaignID)
                        {
                            Session["CampaignName"] = Camp.CampaignName;
                            if (Camp.AllowAdditionalInfo.HasValue)
                                if (Camp.AllowAdditionalInfo.Value)
                                    Session["AllowAdditionalInfo"] = "T";
                        }
                    }
                }
                //					iCampID = CampaignChoices.lsUserCampaigns [0].CampaignID;

                //				oLogWriter.AddLogMessage("After setting from list: iCampID = " + Session["CampaignID"].ToString() + "/" + Session["CampaignName"].ToString(), "Master.LoadData", "", Session.SessionID);

                if (ddlCampaigns != null)
                    if (ddlCampaigns.Items != null)
                        if (ddlCampaigns.Items.Count > 0)
                        {
                            Session["CampaignID"] = ddlCampaigns.SelectedValue;
                            Session["CampaignName"] = ddlCampaigns.SelectedItem.Text;
                            //							int.TryParse(ddlCampaigns.SelectedValue, out iCampID);
                        }

                //				oLogWriter.AddLogMessage("After getting from ddlCampaigns: iCampID = " + Session["CampaignID"].ToString(), "Master.LoadData", "", Session.SessionID);

                int.TryParse(Session["CampaignID"].ToString(), out iCampID);

                if (iCampID > 0)
                {
                    Classes.cPlayerRoles Roles = new Classes.cPlayerRoles();
                    Roles.Load(UserID, 0, iCampID, DateTime.Today);
                    Session["RoleString"] = Roles.PlayerRoleString;

                    //Classes.cPlayerRoles Roles = new Classes.cPlayerRoles();
                    //Roles.Load(UserID, 0, CampaignID, DateTime.Today);
                    Session["PlayerRoleString"] = Roles.PlayerRoleString;
                }
                if (CampaignChoices.CountOfUserCampaigns == 0)
                    Response.Redirect("~/NoCurrentCampaignAssociations.aspx");

                DataTable dtCampList = ConvertCampaignListToDataTable(CampaignChoices.lsUserCampaigns);
                Session["CampaignList"] = dtCampList;
            }

            DataTable dtCampaignList = Session["CampaignList"] as DataTable;
            ddlCampaigns.DataTextField = "CampaignName";
            ddlCampaigns.DataTextField = "CampaignName";
            ddlCampaigns.DataValueField = "CampaignID";
            ddlCampaigns.DataSource = dtCampaignList;
            ddlCampaigns.DataBind();

            if (DisplayAllOptions)
            {
                ddlCampaigns.Items.Add(new ListItem("Show My Campaigns", "-101"));
                ddlCampaigns.Items.Add(new ListItem("Show All Campaigns", "-102"));
            }

            ddlCampaigns.Items.Add(new ListItem("Add a new campaign", "-1"));

            ddlCampaigns.SelectedIndex = 0;
            Session["CampaignID"] = ddlCampaigns.SelectedValue;
            Session["CampaignName"] = ddlCampaigns.SelectedItem.Text;

            string sRoleString = Session["RoleString"].ToString();
            lblRoles.Text = "Roles: " + sRoleString;
            if (Session["SuperUser"] != null)
                lblRoles.ForeColor = System.Drawing.Color.White;

            liSetupCampaign.Style.Add("display", "none");
            liSetupCustomFields.Style.Add("display", "none");
            liSetupCampaignDemographics.Style.Add("display", "none");
            liSetupPlayerReqs.Style.Add("display", "none");
            liSetupContacts.Style.Add("display", "none");
            liSetupPolicies.Style.Add("display", "none");
            liSetupAssignRoles.Style.Add("display", "none");
            liSetupDescription.Style.Add("display", "none");
            liEventRegistrationApproval.Style.Add("display", "none");
            liEventDefaults.Style.Add("display", "none");
            liEventAssignHousing.Style.Add("display", "none");
            liCharacterApproveHistory.Style.Add("display", "none");
            liPELApprovalList.Style.Add("display", "none");
            liPELsMain.Style.Add("display", "none");
            liPointsAssign.Style.Add("display", "none");
            liEMailPoints.Style.Add("display", "none");
            liCampaignSetupMenu.Style.Add("display", "none");
            liCharacterBuildPoints.Style.Add("display", "none");
            liEventSetup.Style.Add("display", "none");
            //liCampaignMenu.Style.Add("display", "none");
            liSkillQualifiers.Style.Add("display", "none");

            liClaimDonations.Style.Add("display", "none");
            liAddDonationRequests.Style.Add("display", "none");
            liReceiveDonations.Style.Add("display", "none");
            liDonations.Style.Add("display", "none");

            liModifySkills.Style.Add("display", "none");
            liIBSkills.Style.Add("display", "none");

            bool bSuperUser = false;
            if (Session["SuperUser"] != null)
                bSuperUser = true;

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_SKILL_UPDATES_5)))
                liModifySkills.Style.Add("display", "block");

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                liSetupCustomFields.Style.Add("display", "block");
                liSetupCampaignDemographics.Style.Add("display", "block");
                liSetupCampaign.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
                //liCampaignMenu.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_OWNER_3)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                liSetupPlayerReqs.Style.Add("display", "block");
                liSetupContacts.Style.Add("display", "block");
                liSetupPolicies.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
                liSetupAssignRoles.Style.Add("display", "block");
                //liCampaignMenu.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_WORLD_SETTING_UPDATES_32)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_OWNER_3)) |
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                liSetupDescription.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
                //liCampaignMenu.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_OWNER_3)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_EVENT_REGISTRATION_APPROVAL_37)))
            {
                liEventRegistrationApproval.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
                //liCampaignMenu.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_OWNER_3)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_EVENT_SCHEDULING_27)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                //ReqPage = "/events/eventlist";
                //				liEvent
                liEventDefaults.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
                //				liCampaignMenu.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_HOUSING_ASSIGNMENT_11)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                liEventAssignHousing.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
                //				liCampaignMenu.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_PLOT_4)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                liCharacterApproveHistory.Style.Add("display", "block");
                liPELApprovalList.Style.Add("display", "block");
                liPELsMain.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
            }


            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_OWNER_3)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_EVENT_SCHEDULING_27)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                liEventSetup.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_CP_ASSIGNMENT_15)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_EVENT_CHECK_OUT_35)))
            {
                liPointsAssign.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
                liPELsMain.Style.Add("display", "block");
                liCharacterBuildPoints.Style.Add("display", "block");
                liEMailPoints.Style.Add("display", "block");
                //				liCampaignMenu.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_SKILL_UPDATES_5)))
            {
                if (Session["AllowAdditionalInfo"] != null)
                    if (Session["AllowAdditionalInfo"].ToString().ToUpper().StartsWith("T"))
                    {
                        liCampaignSetupMenu.Style.Add("display", "block");
                        liSkillQualifiers.Style.Add("display", "block");
                    }
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_CP_ASSIGNMENT_15)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                liEMailPoints.Style.Add("display", "block");
                liCampaignSetupMenu.Style.Add("display", "block");
                //				liCampaignMenu.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.LARPORTAL_DATABASE_OWNER_1)) ||
                (sRoleString.Contains(Classes.cConstants.LARPORTAL_DATABASE_ADMINISTRATOR_2)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_OWNER_3)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_NPC_COORDINATOR_12)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_MONSTER_MASTER_20)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_ROLE_ASSIGNMENT_21)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)))
            {
                liCampaignSetupMenu.Style.Add("display", "block");
                liSetupAssignRoles.Style.Add("display", "block");
                liSetupCampaign.Style.Add("display", "block");
            }

            if ((bSuperUser) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_PLOT_4)) ||                           //  JB/RP  1/16/2022  Added as part of donations.
                (sRoleString.Contains(Classes.cConstants.PLAYER_PC_8)) ||
                (sRoleString.Contains(Classes.cConstants.LARPORTAL_APPROVED_USER_9)) ||
                (sRoleString.Contains(Classes.cConstants.PLAYER_NPC_10)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISTICS_EVENT_CHECK_IN_16)) ||
                (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)) ||
                (sRoleString.Contains(Classes.cConstants.LOGISITICS_DONATION_SET_UP_40)))
            {
                liDonations.Style.Add("display", "block");
                if ((sRoleString.Contains(Classes.cConstants.CAMPAIGN_PLOT_4)) ||
                    (sRoleString.Contains(Classes.cConstants.PLAYER_PC_8)) ||
                    (sRoleString.Contains(Classes.cConstants.LARPORTAL_APPROVED_USER_9)) ||
                    (sRoleString.Contains(Classes.cConstants.PLAYER_NPC_10)) ||
                    (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)) ||
                    (sRoleString.Contains(Classes.cConstants.LOGISITICS_DONATION_SET_UP_40)))
                    liClaimDonations.Style.Add("display", "block");
                if ((sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)) ||
                    (sRoleString.Contains(Classes.cConstants.LOGISITICS_DONATION_SET_UP_40)))
                    liAddDonationRequests.Style.Add("display", "block");
                if ((sRoleString.Contains(Classes.cConstants.CAMPAIGN_PLOT_4)) ||
                    (sRoleString.Contains(Classes.cConstants.LOGISTICS_EVENT_CHECK_IN_16)) ||
                    (sRoleString.Contains(Classes.cConstants.CAMPAIGN_GENERAL_MANAGER_28)) ||
                    (sRoleString.Contains(Classes.cConstants.LOGISITICS_DONATION_SET_UP_40)))
                    liReceiveDonations.Style.Add("display", "block");
            }

            if (bSuperUser)
            {
                liIBSkills.Style.Add("display", "block");
            }

            SortedList sParams = new SortedList();
            sParams.Add("@intUserID", UserID);
            DataTable dtCharacters = Classes.cUtilities.LoadDataTable("uspGetCharacterIDsByUserID", sParams, "LARPortal", UserName, lsRoutineName + ".LoadData.GetChar");
            DataView dvCampChar = new DataView(dtCharacters, "CampaignID = " + ddlCampaigns.SelectedValue, "", DataViewRowState.CurrentRows);

            // If the person has plot privileges, they get to edit/add characters.		JLB 8/12/2018
            SortedList sPrivParams = new SortedList();
            sPrivParams.Add("@UserID", UserID);
            sPrivParams.Add("@CampaignID", ddlCampaigns.SelectedValue);
            DataTable dtPrivs = Classes.cUtilities.LoadDataTable("prUserHasPlotPriv", sPrivParams, "LARPortal", UserName, lsRoutineName + ".LoadData.GetPlotPriv");

            if ((dvCampChar.Count == 0) &&
                (dtPrivs.Rows.Count == 0))
            {
                liHasNoCharacters.Style.Add("display", "block");
                liHasCharacters.Style.Add("display", "none");
            }
            else
            {
                liHasNoCharacters.Style.Add("display", "none");
                liHasCharacters.Style.Add("display", "block");
            }
        }

        private DataTable ConvertCampaignListToDataTable(List<Classes.cUserCampaign> ListOfCamp)
        {
            DataTable dtCampList = new DataTable();
            dtCampList.Columns.Add("CampaignID", typeof(int));
            dtCampList.Columns.Add("CampaignName", typeof(string));
            foreach (Classes.cUserCampaign Camp in ListOfCamp)
            {
                DataRow NewRow = dtCampList.NewRow();
                NewRow["CampaignID"] = Camp.CampaignID;
                NewRow["CampaignName"] = Camp.CampaignName;
                dtCampList.Rows.Add(NewRow);
            }

            return dtCampList;
        }

        /// <summary>
        /// When the user changes the campaign that is selected (usually by changing characters) this will force a reload and change to the selected campaign.
        /// </summary>
        public void ChangeSelectedCampaign()
        {
            if (Session["CampaignID"] != null)
                Session.Remove("CampaignID");

            if (Session["CampaignName"] != null)
                Session.Remove("CampaignName");

            if (Session["CampaignList"] != null)
                Session.Remove("CampaignList");

            if (Session["RoleString"] != null)
                Session.Remove("RoleString");

            if (Session["AllowAdditionalInfo"] != null)
                Session.Remove("AllowAdditionalInfo");

            LoadData();
        }
    }
}