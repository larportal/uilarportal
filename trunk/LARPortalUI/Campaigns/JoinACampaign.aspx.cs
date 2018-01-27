using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace LarpPortal.Campaigns
{
    public partial class JoinACampaign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["DefaultCampaignLogoPath"] = "img/logo/";
                Session["DefaultCampaignLogoImage"] = "http://placehold.it/820x130";
                pnlOverview.Visible = false;
                pnlSelectors.Visible = false;
                pnlImageURL.Visible = false;
                hplLinkToSite.Visible = false;
                pnlSignUpForCampaign.Visible = false;

                ReloadGameSystemTreeView();
            }
        }

        protected void ddlOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make all tree views invisible and show the one that's poplulating
            tvGameSystem.Visible = false;
            tvCampaign.Visible = false;
            tvGenre.Visible = false;
            tvStyle.Visible = false;
            tvTechLevel.Visible = false;
            tvSize.Visible = false;

            pnlImageURL.Visible = false;
            pnlOverview.Visible = false;
            pnlSelectors.Visible = false;
            pnlSignUpForCampaign.Visible = false;

            switch (ddlOrderBy.Text)
            {
                case "Game System":
                    lblCampaignSearchBy.Text = " by Game System";
                    tvGameSystem.Visible = true;
                    break;
                case "Campaign":
                    lblCampaignSearchBy.Text = " by Campaign";
                    tvCampaign.Visible = true;
                    break;
                case "Genre":
                    lblCampaignSearchBy.Text = " by Genre";
                    tvGenre.Visible = true;
                    break;
                case "Style":
                    lblCampaignSearchBy.Text = " by Style";
                    tvStyle.Visible = true;
                    break;
                case "Tech Level":
                    lblCampaignSearchBy.Text = " by Tech Level";
                    tvTechLevel.Visible = true;
                    break;
                case "Size":
                    lblCampaignSearchBy.Text = " by Size";
                    tvSize.Visible = true;
                    break;
                default:
                    break;

            }
            ReloadActiveTreeView(Master.UserID);
        }

        protected void MakeDetailsVisible(int URLVisible, int ImageVisible, int imgHeight, int imgWidth, int OverviewVisible, int SelectorsVisible, int CampaignID)
        {
            if (URLVisible == 1)
                hplLinkToSite.Visible = true;
            else
                hplLinkToSite.Visible = false;
            if (ImageVisible == 1)
            {
                // Max dimensions are 820 x 130
                if (imgHeight == 0)
                    imgHeight = 130;
                if (imgWidth == 0)
                    imgWidth = 820;
                decimal imgRatio;
                imgRatio = decimal.Divide(imgWidth, imgHeight);
                if (imgRatio == 0)
                    imgRatio = 1;

                int CalculatedHeight = Convert.ToInt32(Math.Round(820 / imgRatio, 0));
                int CalculatedWidth = Convert.ToInt32(Math.Round(130 * imgRatio, 0));

                if (imgRatio == 6.308m)
                {
                    imgCampaignImage.Height = 130;
                    imgCampaignImage.Width = 820;
                }
                if (imgRatio > 6.308m)
                {
                    imgCampaignImage.Height = CalculatedHeight;
                    imgCampaignImage.Width = 820;
                }
                if (imgRatio < 6.308m)
                {
                    imgCampaignImage.Height = 130;
                    imgCampaignImage.Width = CalculatedWidth;
                }
                pnlImageURL.Visible = true;
                imgCampaignImage.Visible = true;
            }
            else
                //pnlImageURL.Visible = false;
                imgCampaignImage.Visible = false;
            if (OverviewVisible == 1)
                pnlOverview.Visible = true;
            else
                pnlOverview.Visible = false;
            if (SelectorsVisible == 1)
            {
                pnlSelectors.Visible = true;
                if (((int)Session["SecurityRole"]) == 10) 
                    MakePanelSignUpVisible(CampaignID);
                else
                    pnlSignUpForCampaign.Visible = false;
            }
            else
            {
                pnlSelectors.Visible = false;
                pnlSignUpForCampaign.Visible = false;
            }
        }

        protected void MakePanelSignUpVisible(int CampaignID)
        {
            // Determine current roles for this campaign.
            int iTemp = 0;
            int RoleID = 0;
            Boolean bTemp = false;
            Boolean AutoApprove = false;
            string RoleDescription = "";
            string IsPC = "false";
            string IsNPC = "false";
            lblSignUpMessage.Visible = false;
            lblCurrentCampaign.Text = CampaignID.ToString();
            Classes.cPlayerRole Role = new Classes.cPlayerRole();
            Role.Load(Master.UserID, 0, CampaignID);
            //Possible options:
            int RoleID6 = 0;    //6 Permanent NPC
            int RoleID7 = 0;    //7 Event NPC
            int RoleID8 = 0;    //8 PC
            int RoleID10 = 0;   //10 NPC
            int RoleCounter = 0; //Count the number of available roles for choosing "Both or All"
            string RoleDescription6 = "";
            string RoleDescription7 = "";
            string RoleDescription8 = "";
            string RoleDescription10 = "";
            Boolean AutoApprove6 = false;
            Boolean AutoApprove7 = false;
            Boolean AutoApprove8 = false;
            Boolean AutoApprove10 = false;
            Classes.cCampaignBase CampaignRoles = new Classes.cCampaignBase(CampaignID, "UserName", Master.UserID);
            DataTable dtAvailRoles = new DataTable();
            dtAvailRoles = CampaignRoles.GetCampaignRequestableRoles(CampaignID, Master.UserID);
            foreach (DataRow dRow in dtAvailRoles.Rows)
            {
                RoleCounter = 0;
                if (int.TryParse(dRow["RoleID"].ToString(), out iTemp))
                    RoleID = iTemp;
                if (Boolean.TryParse(dRow["AutoApprove"].ToString(), out bTemp))
                    AutoApprove = bTemp;
                RoleDescription = dRow["RoleDescription"].ToString();
                switch (RoleID)
                {
                    case 6:
                        RoleID6 = 6;
                        RoleDescription6 = "Permanent NPC";
                        AutoApprove6 = AutoApprove;
                        break;
                    case 7:
                        RoleID7 = 7;
                        RoleDescription7 = "Event NPC";
                        AutoApprove7 = AutoApprove;
                        break;
                    case 8:
                        RoleID8 = 8;
                        RoleDescription8 = RoleDescription;
                        AutoApprove8 = AutoApprove;
                        break;
                    case 10:
                        RoleID10 = 10;
                        RoleDescription10 = RoleDescription;
                        AutoApprove10 = AutoApprove;
                        break;
                    default:
                        //TODO - Get rid of these two lines once 6 and 7 are really defined.  This will shut up the warnings
                        RoleDescription6 = RoleDescription7;
                        RoleDescription7 = RoleDescription6;
                        break;
                }
            }
            IsPC = Role.IsPC;
            IsNPC = Role.IsNPC;
            //TODO-Rick-9-Clean-up crap code - all instances of btnSignup; changed to chkSignup
            //btnSignUp.Items.Clear();
            chkSignUp.Items.Clear();
            // None - Show all three choices if applicable
            if (IsPC == "false" && IsNPC == "false")
            {
                if (RoleID8 != 0)
                {
                    //btnSignUp.Items.Add(new ListItem("PC", "8"));
                    chkSignUp.Items.Add(new ListItem("PC", "8" + AutoApprove8.ToString()));
                    RoleCounter++;
                }
                if (RoleID10 != 0)
                {
                    //btnSignUp.Items.Add(new ListItem("NPC", "10"));
                    chkSignUp.Items.Add(new ListItem("NPC", "10" + AutoApprove10.ToString()));
                    RoleCounter++;
                }
                if (RoleID7 != 0)
                {
                    //btnSignUp.Items.Add(new ListItem("Event NPC", "7"));
                    chkSignUp.Items.Add(new ListItem("Event NPC", "7" + AutoApprove7.ToString()));
                    RoleCounter++;
                }
                if (RoleID6 != 0)
                {
                    //btnSignUp.Items.Add(new ListItem("Permanent NPC", "6"));
                    chkSignUp.Items.Add(new ListItem("Permanent NPC", "6" + AutoApprove6.ToString()));
                    RoleCounter++;
                }
            }
            // PC Only - Show NPC
            if (IsPC == "true" && IsNPC == "false")
            {
                if (RoleID10 != 0)
                {
                    //btnSignUp.Items.Add(new ListItem("NPC", "10"));
                    chkSignUp.Items.Add(new ListItem("NPC", "10" + AutoApprove10.ToString()));
                    RoleCounter++;
                }
                if (RoleID7 != 0)
                {
                    //btnSignUp.Items.Add(new ListItem("Event NPC", "7"));
                    chkSignUp.Items.Add(new ListItem("Event NPC", "7" + AutoApprove7.ToString()));
                    RoleCounter++;
                }
                if (RoleID6 != 0)
                {
                    //btnSignUp.Items.Add(new ListItem("Permanent NPC", "6"));
                    chkSignUp.Items.Add(new ListItem("Permanent NPC", "6" + AutoApprove6.ToString()));
                    RoleCounter++;
                }
            }
            // NPC Only - Show PC
            if (IsPC == "false" && IsNPC == "true")
            {
                if (RoleID8 != 0)
                {
                    //btnSignUp.Items.Add(new ListItem("PC", "8"));
                    chkSignUp.Items.Add(new ListItem("PC", "8" + AutoApprove8.ToString()));
                    RoleCounter++;
                }
            }
            // Both - Don't show panel
            if (IsPC == "true" && IsNPC == "true")
            {
                pnlSignUpForCampaign.Visible = true;
                btnSignUpForCampaign.Visible = false;
            }
            else
            {
                pnlSignUpForCampaign.Visible = true;
                btnSignUpForCampaign.Visible = true;
            }
            // Now get current roles.  If there are any start the label with "Current Roles:<br>"
            Classes.cPlayerRoles Roles = new Classes.cPlayerRoles();
            Roles.Load(Master.UserID, 0, CampaignID, DateTime.Now); // Last parameter = 1 for Yes, Current roles only
            //Convert list to datatable
            listCurrentRoles.DataSource = Classes.cUtilities.CreateDataTable(Roles.lsPlayerRoles);
            listCurrentRoles.DataBind();
        }

        protected void btnSignUpForCampaign_Click(object sender, EventArgs e)
        {
            int intCampaignID = lblCurrentCampaign.Text.ToString().ToInt32();
            if (Session["CampaignPlayerRoleID"] == null)
                Session["CampaignPlayerRoleID"] = 0;
            int CampaignPlayerRoleID = Session["CampaignPlayerRoleID"].ToString().ToInt32();
            string RequestEmail;
            Classes.cCampaignBase Campaign = new Classes.cCampaignBase(intCampaignID, Master.UserName, Master.UserID);
            if (String.IsNullOrEmpty(Campaign.JoinRequestEmail))
                RequestEmail = Campaign.InfoRequestEmail;
            else
                RequestEmail = Campaign.JoinRequestEmail;
            if (RequestEmail.Contains("@"))
            {
                // It has a "@".  Assume the email format is close enough, go on.
            }
            else
            {
                RequestEmail = "";
            }
            foreach (ListItem item in chkSignUp.Items)
            {
                if (item.Selected)
                {
                    switch (item.Value)
                    {
                        case "6True":
                            // Permanent NPC needs approval
                            if (String.IsNullOrEmpty(RequestEmail))
                                SignUpForSelectedRole(6, Master.UserID, intCampaignID, 55);
                            else
                            {
                                SendApprovalEmail(intCampaignID, Master.UserID, 6, RequestEmail);
                                SignUpForSelectedRole(10, Master.UserID, intCampaignID, 56);
                            }
                            break;
                        case "6False":
                            // Permanent NPC no approval
                            SignUpForSelectedRole(6, Master.UserID, intCampaignID, 55);
                            break;
                        case "7True":
                            // Event NPC needs approval
                            if (String.IsNullOrEmpty(RequestEmail))
                                SignUpForSelectedRole(7, Master.UserID, intCampaignID, 55);
                            else
                            {
                                SendApprovalEmail(intCampaignID, Master.UserID, 7, RequestEmail);
                                SignUpForSelectedRole(10, Master.UserID, intCampaignID, 56);
                            }
                            break;
                        case "7False":
                            // Event NPC no approval
                            SignUpForSelectedRole(7, Master.UserID, intCampaignID, 55);
                            break;
                        case "8True":
                            // PC needs approval
                            if (String.IsNullOrEmpty(RequestEmail))
                                SignUpForSelectedRole(8, Master.UserID, intCampaignID, 55);
                            else
                            {
                                SendApprovalEmail(intCampaignID, Master.UserID, 8, RequestEmail);
                                SignUpForSelectedRole(8, Master.UserID, intCampaignID, 56);
                            }
                            break;
                        case "8False":
                            // PC no approval
                            SignUpForSelectedRole(8, Master.UserID, intCampaignID, 55);
                            break;
                        case "10True":
                            // NPC needs approval
                            if (String.IsNullOrEmpty(RequestEmail))
                                SignUpForSelectedRole(10, Master.UserID, intCampaignID, 55);
                            else
                            {
                                SendApprovalEmail(intCampaignID, Master.UserID, 10, RequestEmail);
                                SignUpForSelectedRole(10, Master.UserID, intCampaignID, 56);
                            }
                            break;
                        case "10False":
                            // NPC no approval
                            SignUpForSelectedRole(10, Master.UserID, intCampaignID, 55);
                            break;
                        default:
                            // Technically we shouldn't be able to get here so do nothing
                            break;
                    }
                }
            }
        }

        protected void SignUpForSelectedRole(int RoleToSignUp, int UserID, int CampaignID, int StatusID)
        {
            int CampaignPlayerID = 0;
            Classes.cUserCampaign CampaignPlayer = new Classes.cUserCampaign();
            CampaignPlayer.Load(UserID, CampaignID);
            CampaignPlayerID = CampaignPlayer.CampaignPlayerID; // if this comes back empty (-1) make one
            if (CampaignPlayerID == -1)
            {
                CreatePlayerInCampaign(UserID, CampaignID);
                CampaignPlayer.Load(UserID, CampaignID);
                CampaignPlayerID = CampaignPlayer.CampaignPlayerID;
            }
            int RoleAlignment = 2;
            if (RoleToSignUp == 8)
                RoleAlignment = 1;
            Classes.cPlayerRole PlayerRole = new Classes.cPlayerRole();
            PlayerRole.CampaignPlayerRoleID = -1;
            PlayerRole.CampaignPlayerID = CampaignPlayerID;
            PlayerRole.RoleID = RoleToSignUp;
            PlayerRole.RoleAlignmentID = RoleAlignment;
            PlayerRole.Save(UserID);
            Classes.cUser LastLogged = new Classes.cUser(Master.UserName, "Password", Session.SessionID);
            string LastCampaign = LastLogged.LastLoggedInCampaign.ToString();
            if (LastCampaign == null || LastCampaign == "0")
            {
                LastLogged.LastLoggedInCampaign = CampaignID;
                LastLogged.Save();
                Session["CampaignID"] = CampaignID;
            }

            btnSignUpForCampaign.Visible = false;
            lblSignUpMessage.Text = "Request submitted. Choose more campaigns or <a id=\"" + "lnkReturnToMember " + "\"href=\"" + "CampaignInfo.aspx\"" + ">return to the member section.</a>";
            lblSignUpMessage.Visible = true;
        }

        protected void CreatePlayerInCampaign(int UserID, int CampaignID)
        {
            Classes.cUserCampaign UserCampaign = new Classes.cUserCampaign();
            UserCampaign.CampaignPlayerID = -1;
            UserCampaign.CampaignID = CampaignID;
            UserCampaign.Save(UserID);
        }

        protected void SendApprovalEmail(int CampaignID, int UserID, int Role, string RequestEmail)
        {
            string strFromUser = "playerservices";
            string strFromDomain = "larportal.com";
            string strFrom = strFromUser + "@" + strFromDomain;
            string strTo = RequestEmail;
            string strSMTPPassword = "Piccolo1";
            string strSubject = "";
            string strBody = "";
            string CampaignName = "";
            string PlayerFirstName = "";  // Needs defining - Look up based on UserID
            string PlayerLastName = "";  // Needs defining
            string PlayerEmailAddress = "";  // Needs defining
            Classes.cCampaignBase Campaign = new Classes.cCampaignBase(CampaignID, "Username", UserID);
            CampaignName = Campaign.CampaignName;
            Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "Password", Session.SessionID);
            PlayerFirstName = UserInfo.FirstName;
            PlayerLastName = UserInfo.LastName;
            PlayerEmailAddress = Session["MemberEmailAddress"].ToString();
            switch (Role)
            {
                case 6:
                    strSubject = "Request to be a Permanent NPC for " + CampaignName;
                    strBody = "A request to be a permanent NPC " + CampaignName + " has been received through LARP Portal.  The player information is below.<p></p><p></p>";
                    strBody = strBody + PlayerFirstName + " " + PlayerLastName + "<p></p>" + PlayerEmailAddress + "<p></p><p></p>";
                    strBody = strBody + "Please reply to the player's request within 48 hours.<p></p><p></p>";
                    strBody = strBody + "Thank you<p></p><p></p>LARP Portal staff";
                    break;
                case 7:
                    strSubject = "Request to be an Event NPC for " + CampaignName;
                    strBody = "A request to be an Evnet NPC " + CampaignName + " has been received through LARP Portal.  The player information is below.<p></p><p></p>";
                    strBody = strBody + PlayerFirstName + " " + PlayerLastName + "<p></p>" + PlayerEmailAddress + "<p></p><p></p>";
                    strBody = strBody + "Please reply to the player's request within 48 hours.<p></p><p></p>";
                    strBody = strBody + "Thank you<p></p><p></p>LARP Portal staff";
                    break;
                case 8:
                    strSubject = "Request to PC " + CampaignName;
                    strBody = "A request to PC " + CampaignName + " has been received through LARP Portal.  The player information is below.<p></p><p></p>";
                    strBody = strBody + PlayerFirstName + " " + PlayerLastName + "<p></p>" + PlayerEmailAddress + "<p></p><p></p>";
                    strBody = strBody + "Please reply to the player's request within 48 hours.<p></p><p></p>";
                    strBody = strBody + "Thank you<p></p><p></p>LARP Portal staff";
                    break;
                case 10:
                    strSubject = "Request to NPC " + CampaignName;
                    strBody = "A request to NPC " + CampaignName + " has been received through LARP Portal.  The player information is below.<p></p><p></p>";
                    strBody = strBody + PlayerFirstName + " " + PlayerLastName + "<p></p>" + PlayerEmailAddress + "<p></p><p></p>";
                    strBody = strBody + "Please reply to the player's request within 48 hours.<p></p><p></p>";
                    strBody = strBody + "Thank you<p></p><p></p>LARP Portal staff";
                    break;
                default:
                    break;
            }
            MailMessage mail = new MailMessage(strFrom, strTo);
            SmtpClient client = new SmtpClient("smtpout.secureserver.net", 80);
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(strFrom, strSMTPPassword);
            client.Timeout = 10000;
            mail.Subject = strSubject;
            mail.Body = strBody;
            mail.IsBodyHtml = true;
            if (String.IsNullOrEmpty(strSubject))
            {
                try
                {
                    client.Send(mail);
                }
                catch (Exception)
                {

                }
            }
        }

        protected void SetSiteLink(string strURL, string strGameName)
        {
            hplLinkToSite.NavigateUrl = strURL;
            hplLinkToSite.Text = "Visit " + strGameName + " home page.";

        }

        protected void SetSiteImage(string strImage)
        {
            string DefaultPath = "";
            string DefaultImage = "";
            if (Session["DefaultCampaignLogoImage"] == null)
                DefaultImage = "";
            else
                DefaultImage = Session["DefaultCampaignLogoImage"].ToString();
            if (Session["DefaultCampaignLogoPath"] == null)
                DefaultPath = "";
            else
                DefaultPath = Session["DefaultCampaignLogoPath"].ToString();
            if (String.IsNullOrEmpty(strImage))
                imgCampaignImage.ImageUrl = DefaultImage;
            else
                imgCampaignImage.ImageUrl = DefaultPath + strImage;
        }

        protected void ReloadGameSystemTreeView()
        {
            tvGameSystem.Nodes.Clear();

            Classes.cCampaignSelection GameSystems = new Classes.cCampaignSelection();
            DataSet dsCampaigns = ReloadCampaigns();

            // Order the records by game system name then make a unique list of them.
            DataView dvOrderedCampaigns = new DataView(dsCampaigns.Tables["Campaigns"], "", "GameSystemName", DataViewRowState.CurrentRows);
            DataTable dtGameSystems = dvOrderedCampaigns.ToTable(true, "GameSystemID", "GameSystemName");   // Unique list of game systems.

            // Go through each game system getting the campaigns for each.
            foreach (DataRow dRow in dtGameSystems.Rows)
            {
                TreeNode GameSystemNode = new TreeNode();
                int GameSystemID = 0;
                string GameSystemName = "";
                int.TryParse(dRow["GameSystemID"].ToString(), out GameSystemID);
                GameSystemName = dRow["GameSystemName"].ToString();
                GameSystemNode = new TreeNode(GameSystemName, "G" + GameSystemID.ToString());
                GameSystemNode.Selected = false;
                GameSystemNode.NavigateUrl = "";

                // Build the list of campaigns that belong to this game system.
                DataTable dtCampaigns = new DataTable();
                DataView dvCamps = new DataView(dsCampaigns.Tables["Campaigns"], "GameSystemID = " + GameSystemID.ToString(), "CampaignName", DataViewRowState.CurrentRows);

                // Add each campaign to the node.
                foreach (DataRowView cRow in dvCamps)
                {
                    TreeNode CampaignNode;
                    int StatusID = 0;
                    int CampaignID = 0;
                    string CampaignName = cRow["CampaignName"].ToString();
                    int.TryParse(cRow["CampaignID"].ToString(), out CampaignID);
                    int.TryParse(cRow["StatusID"].ToString(), out StatusID);

                    if (StatusID == 4)   // Past/Ended
                    {
                        if (chkEndedCampaigns.Checked == true)
                        {
                            CampaignName = CampaignName + " (Ended)";
                            CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                            CampaignNode.Selected = false;
                            CampaignNode.NavigateUrl = "";
                            GameSystemNode.ChildNodes.Add(CampaignNode);
                        }
                    }
                    else
                    {
                        CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                        CampaignNode.Selected = false;
                        CampaignNode.NavigateUrl = "";
                        GameSystemNode.ChildNodes.Add(CampaignNode);
                    }
                }

                // After having gone through all the campaigns, if there are any nodes added, add the game system node to the tree.
                if (GameSystemNode.ChildNodes.Count > 0)
                    tvGameSystem.Nodes.Add(GameSystemNode);
            }
            RebuildDropDownList(dsCampaigns);
        }

        protected void tvGameSystem_SelectedNodeChanged(object sender, EventArgs e)
        {
            string strURL = "";
            string strImage = "";
            int intImageHeight = 0;
            int intImageWidth = 0;
            string strGameOrCampaignName = "";
            tvGameSystem.SelectedNode.Selected = true;
            string SelectedGorC = tvGameSystem.SelectedValue + "";
            string GorC = SelectedGorC.Substring(0, 1);
            string stGameSystemID = SelectedGorC.Substring(1, SelectedGorC.Length - 1);
            int GameSystemID;
            int CampaignID;
            int intURLVisible = 1;
            int intImageVisible = 1;
            int intOverviewVisible = 1;
            int intSelectorsVisible = 1;
            int.TryParse(stGameSystemID, out GameSystemID);
            int.TryParse(tvGameSystem.SelectedValue, out CampaignID);
            if (GorC == "G") // Game System
            {
                intSelectorsVisible = 0;
                lblGorC1.Text = "Game System";
                lblGorC2.Text = "Game System Description";
                Classes.cGameSystem GS = new Classes.cGameSystem();
                GS.Load(GameSystemID, 0);
                strURL = GS.GameSystemURL;
                strImage = GS.GameSystemLogo;
                intImageHeight = GS.GameSystemLogoHeight;
                intImageWidth = GS.GameSystemLogoWidth;
                strGameOrCampaignName = GS.GameSystemName;
                lblCampaignOverview.Text = GS.GameSystemWebPageDescription.Replace("\n", "<br>");
            }
            else  // Campaign
            {
                if (CampaignID < 1) // Placeholder for Game Systems with no campaigns
                {
                    intURLVisible = 0;
                    intImageVisible = 0;
                    intOverviewVisible = 0;
                    intSelectorsVisible = 0;
                }
                else
                {
                    lblGorC1.Text = "Campaign";
                    lblGorC2.Text = "Campaign Description";
                    Classes.cCampaignBase Cam = new Classes.cCampaignBase(CampaignID, "public", 0);
                    strURL = Cam.URL;
                    strImage = Cam.Logo;
                    intImageHeight = Cam.LogoHeight;
                    intImageWidth = Cam.LogoWidth;
                    strGameOrCampaignName = Cam.CampaignName;
                    lblCampaignOverview.Text = Cam.WebPageDescription.Replace("\n", "<br>");
                    lblGameSystem1.Text = "Game System: ";
                    lblGameSystem2.Text = " " + Cam.GameSystemName;
                    lblGenre1.Text = "Genre: ";
                    lblGenre2.Text = " " + Cam.GenreList; //TODO-Rick-00 Fix this
                    lblStyle1.Text = "Style: ";
                    lblStyle2.Text = " " + Cam.StyleDescription;
                    lblTechLevel1.Text = "Tech Level: ";
                    lblTechLevel2.Text = " " + Cam.TechLevelName + Cam.TechLevelList;  //TODO-Rick-00 Fix this too
                    lblSize1.Text = "Size:";
                    lblSize2.Text = " " + Cam.CampaignSizeRange;
                    lblLocation2.Text = Cam.MarketingLocation;
                    lblEvent2.Text = string.Format("{0:MMM d, yyyy}", Cam.NextEventDate);
                    lblLastUpdated2.Text = string.Format("{0:MMM d, yyyy}", Cam.DateChanged);
                };
            }
            SetSiteImage(strImage);
            if (strURL != null)
                SetSiteLink(strURL, strGameOrCampaignName);
            MakeDetailsVisible(intURLVisible, intImageVisible, intImageHeight, intImageWidth, intOverviewVisible, intSelectorsVisible, CampaignID);
        }

        protected void chkGameSystem_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGameSystem.Checked == true)
            {
                ddlGameSystem.Visible = true;
            }
            else
            {
                Session.Remove("GameSystemFilter");
                ddlGameSystem.Visible = false;
                ReloadActiveTreeView(Master.UserID);
            }
        }

        protected void ddlGameSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkGameSystem.Checked == true)
                Session["GameSystemFilter"] = ddlGameSystem.SelectedValue;
            else
                Session.Remove("GameSystemFilter");
            ReloadActiveTreeView(Master.UserID);
        }



        protected void ReloadCampaignTreeView()
        {
            tvCampaign.Nodes.Clear();
            DataSet dsCampaigns = ReloadCampaigns();

            DataView dvCampaigns = new DataView(dsCampaigns.Tables["Campaigns"], "", "CampaignName", DataViewRowState.CurrentRows);
            foreach (DataRowView dRow in dvCampaigns)
            {
                TreeNode CampaignNode;
                int CampaignID = 0;
                int StatusID = 0;
                string CampaignName = dRow["CampaignName"].ToString();
                int.TryParse(dRow["CampaignID"].ToString(), out CampaignID);
                int.TryParse(dRow["StatusID"].ToString(), out StatusID);

                if (StatusID == 4)  // Past/Ended
                {
                    if (chkEndedCampaigns.Checked == true)
                    {
                        CampaignName = CampaignName + " (Ended)";
                        CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                        CampaignNode.Selected = false;
                        CampaignNode.NavigateUrl = "";
                        tvCampaign.Nodes.Add(CampaignNode);
                    }
                }
                else
                {
                    CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                    CampaignNode.Selected = false;
                    CampaignNode.NavigateUrl = "";
                    tvCampaign.Nodes.Add(CampaignNode);
                }
            }
        }

        protected void tvCampaign_SelectedNodeChanged(object sender, EventArgs e)
        {
            string strURL = "";
            string strImage = "";
            int intImageHeight = 130;
            int intImageWidth = 820;
            string strGameOrCampaignName = "";
            tvCampaign.SelectedNode.Selected = true;
            string SelectedGorC = tvCampaign.SelectedValue + "";
            string GorC = SelectedGorC.Substring(0, 1);
            string stGameSystemID = SelectedGorC.Substring(1, SelectedGorC.Length - 1);
            //            int GameSystemID;
            int CampaignID = tvCampaign.SelectedValue.ToString().ToInt32();
            int intURLVisible = 1;
            int intImageVisible = 1;
            int intOverviewVisible = 1;
            int intSelectorsVisible = 1;
            //            int.TryParse(stGameSystemID, out GameSystemID);

            lblGorC1.Text = "Campaign";
            lblGorC2.Text = "Campaign Description";

            if (GorC == "G")
            {
                intSelectorsVisible = 0;
                //Classes.cGameSystem GS = new Classes.cGameSystem();
                //GS.Load(GameSystemID, 0);
                //strURL = GS.GameSystemURL;
                //strImage = "";
                //strGameOrCampaignName = GS.GameSystemName;
                //lblCampaignOverview.Text = GS.GameSystemWebPageDescription.Replace("\n", "<br>");
            }
            else  // Campaign
            {
                if (CampaignID < 1) // Placeholder for Game Systems with no campaigns
                {
                    intURLVisible = 0;
                    intImageVisible = 0;
                    intOverviewVisible = 0;
                    intSelectorsVisible = 0;
                }
                else   // for this treeview we really only want this line of logic
                {
                    Classes.cCampaignBase Cam = new cCampaignBase();
                    Cam = DisplayCampaignInfo(CampaignID);     //, ref strURL, ref strImage, ref intImageHeight, ref intImageWidth, ref strGameOrCampaignName);
                    strURL = Cam.URL;
                    strImage = Cam.Logo;
                    intImageHeight = Cam.LogoHeight;
                    intImageWidth = Cam.LogoWidth;
                    strGameOrCampaignName = Cam.CampaignName;
                }
            }
            SetSiteImage(strImage);
            if (strURL != null)
                SetSiteLink(strURL, strGameOrCampaignName);
            MakeDetailsVisible(intURLVisible, intImageVisible, intImageHeight, intImageWidth, intOverviewVisible, intSelectorsVisible, CampaignID);
        }

        protected void chkCampaign_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCampaign.Checked == true)
            {
                ddlCampaign.Visible = true;
            }
            else
            {
                Session.Remove("CampaignFilter");
                ddlCampaign.Visible = false;
                ReloadActiveTreeView(Master.UserID);
            }
        }

        protected void ddlCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkCampaign.Checked == true)
                Session["CampaignFilter"] = ddlGameSystem.SelectedValue;
            else
                Session.Remove("CampaignFilter");
            ReloadActiveTreeView(Master.UserID);
        }



        protected void ReloadGenreTreeView()
        {
            tvGenre.Nodes.Clear();
            DataSet dsCampaigns = ReloadCampaigns();

            // Genres works slightly differently because a game can belong to multiple genres. Theres a table that lists 
            // all the campaigns/genres. So we build a list from that, then get the unique list, and go through.
            // Order the records by genre then make a unique list of them.
            DataView dvOrderedCampaigns = new DataView(dsCampaigns.Tables["Genres"], "", "GenreName", DataViewRowState.CurrentRows);
            DataTable dtGenres = dvOrderedCampaigns.ToTable(true, "GenreID", "GenreName");   // Unique list of genres.
            DataView dvGenres = new DataView(dtGenres, "", "GenreName", DataViewRowState.CurrentRows);
            foreach (DataRowView dRow in dvGenres)
            {
                int GenreID;
                string GenreName = dRow["GenreName"].ToString();
                int.TryParse(dRow["GenreID"].ToString(), out GenreID);
                TreeNode GenreNode = new TreeNode(GenreName, "G" + GenreID.ToString());     // G will be asssigned to all genre categories.
                GenreNode.Selected = false;
                GenreNode.NavigateUrl = "";

                // Build the list of campaigns that belong to this genre. This is slightly different than other areas. 
                // Take the genreID, look at the list of campaigns/genres, then get the actual campaign record.
                DataView dvGenreToCampRow = new DataView(dsCampaigns.Tables["Genres"], "GenreID = " + GenreID.ToString(), "CampaignName", DataViewRowState.CurrentRows);
                foreach (DataRowView dGenreToCampRow in dvGenreToCampRow)
                {
                    DataView dvCamp = new DataView(dsCampaigns.Tables["Campaigns"], "CampaignID = " + dGenreToCampRow["CampaignID"].ToString(), "CampaignName", DataViewRowState.CurrentRows);
                    foreach (DataRowView dCamp in dvCamp)
                    {
                        TreeNode CampaignNode;
                        int StatusID = 0;
                        int CampaignID = 0;
                        string CampaignName = dCamp["CampaignName"].ToString();
                        int.TryParse(dCamp["CampaignID"].ToString(), out CampaignID);
                        int.TryParse(dCamp["StatusID"].ToString(), out StatusID);

                        if (StatusID == 4)      // Past/Ended
                        {
                            if (chkEndedCampaigns.Checked)
                            {
                                CampaignName = CampaignName + " (Ended)";
                                CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                                GenreNode.ChildNodes.Add(CampaignNode);
                                CampaignNode.Selected = false;
                                CampaignNode.NavigateUrl = "";
                            }
                        }
                        else
                        {
                            CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                            GenreNode.ChildNodes.Add(CampaignNode);
                            CampaignNode.Selected = false;
                            CampaignNode.NavigateUrl = "";
                        }
                    }
                }

                // After having gone through all the campaigns, if there are any nodes added, add the genre node to the tree.
                if (GenreNode.ChildNodes.Count > 0)
                    tvGenre.Nodes.Add(GenreNode);
            }
            RebuildDropDownList(dsCampaigns);
        }

        protected void tvGenre_SelectedNodeChanged(object sender, EventArgs e)
        {
            {
                string strURL = "";
                string strImage = "";
                int intImageHeight = 130;
                int intImageWidth = 820;
                string strGameOrCampaignName = "";
                tvGenre.SelectedNode.Selected = true;
                string SelectedGorC = tvGenre.SelectedValue + "";
                string GorC = SelectedGorC.Substring(0, 1);
                string stGameSystemID = SelectedGorC.Substring(1, SelectedGorC.Length - 1);
                int GameSystemID;
                int CampaignID;
                int intURLVisible = 1;
                int intImageVisible = 1;
                int intOverviewVisible = 1;
                int intSelectorsVisible = 1;
                int.TryParse(stGameSystemID, out GameSystemID);
                int.TryParse(tvGenre.SelectedValue, out CampaignID);
                if (GorC == "G") // Game System
                {
                    intSelectorsVisible = 0;
                    intURLVisible = 0;
                    intImageVisible = 0;
                    lblGorC1.Text = "Genre";
                    lblGorC2.Text = "Genre Description";
                    Classes.cGameSystem GS = new Classes.cGameSystem();
                    GS.Load(GameSystemID, 0);
                    strURL = GS.GameSystemURL;
                    strImage = "";
                    strGameOrCampaignName = GS.GameSystemName;
                    lblCampaignOverview.Text = GS.GameSystemWebPageDescription.Replace("\n", "<br>");
                }
                else  // Campaign
                {
                    if (CampaignID < 1) // Placeholder for Game Systems with no campaigns
                    {
                        intURLVisible = 0;
                        intImageVisible = 0;
                        intOverviewVisible = 0;
                        intSelectorsVisible = 0;
                    }
                    else
                    {
                        lblGorC1.Text = "Campaign";
                        lblGorC2.Text = "Campaign Description";
                        Classes.cCampaignBase Cam = new Classes.cCampaignBase(CampaignID, "public", 0);
                        strURL = Cam.URL;
                        strImage = Cam.Logo;
                        intImageHeight = Cam.LogoHeight;
                        intImageWidth = Cam.LogoWidth;
                        strGameOrCampaignName = Cam.CampaignName;
                        lblCampaignOverview.Text = Cam.WebPageDescription.Replace("\n", "<br>");
                        lblGameSystem1.Text = "Game System: ";
                        lblGameSystem2.Text = " " + Cam.GameSystemName;
                        lblGenre1.Text = "Genre: ";
                        lblGenre2.Text = " " + Cam.GenreList; //TODO-Rick-00 Fix this
                        lblStyle1.Text = "Style: ";
                        lblStyle2.Text = " " + Cam.StyleDescription;
                        lblTechLevel1.Text = "Tech Level: ";
                        lblTechLevel2.Text = " " + Cam.TechLevelName + Cam.TechLevelList;  //TODO-Rick-00 Fix this too
                        lblSize1.Text = "Size:";
                        lblSize2.Text = " " + Cam.CampaignSizeRange;
                        lblLocation2.Text = Cam.MarketingLocation;
                        lblEvent2.Text = string.Format("{0:MMM d, yyyy}", Cam.NextEventDate);
                    }
                }
                SetSiteImage(strImage);
                if (strURL != null)
                    SetSiteLink(strURL, strGameOrCampaignName);
                MakeDetailsVisible(intURLVisible, intImageVisible, intImageHeight, intImageWidth, intOverviewVisible, intSelectorsVisible, CampaignID);
            }
        }

        protected void chkGenre_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGenre.Checked == true)
            {
                ddlGenre.Visible = true;
            }
            else
            {
                Session.Remove("GenreFilter");
                ddlGenre.Visible = false;
                ReloadActiveTreeView(Master.UserID);
            }
        }

        protected void ddlGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkGenre.Checked)
                Session["GenreFilter"] = ddlGenre.SelectedValue;
            else
                Session.Remove("GenreFilter");
            ReloadActiveTreeView(Master.UserID);
        }



        protected void ReloadStyleTreeView()
        {
            tvStyle.Nodes.Clear();

            Classes.cCampaignSelection GameSystems = new Classes.cCampaignSelection();
            DataSet dsCampaigns = ReloadCampaigns();

            // Order the records by styles then make a unique list of them.
            DataView dvOrderedCampaigns = new DataView(dsCampaigns.Tables["Campaigns"], "", "StyleName", DataViewRowState.CurrentRows);
            DataTable dtStyles = dvOrderedCampaigns.ToTable(true, "StyleID", "StyleName");   // Unique list of game systems.

            // Go through each style getting the campaigns for each.
            foreach (DataRow dRow in dtStyles.Rows)
            {
                TreeNode StyleNode = new TreeNode();
                int StyleID = 0;
                string StyleName = "";
                int.TryParse(dRow["StyleID"].ToString(), out StyleID);
                StyleName = dRow["StyleName"].ToString();
                StyleNode = new TreeNode(StyleName, "G" + StyleID.ToString());
                StyleNode.Selected = false;
                StyleNode.NavigateUrl = "";

                // Build the list of campaigns that belong to this style.
                DataTable dtCampaigns = new DataTable();
                DataView dvCamps = new DataView(dsCampaigns.Tables["Campaigns"], "StyleID = " + StyleID.ToString(), "CampaignName", DataViewRowState.CurrentRows);

                // Add each campaign to the node.
                foreach (DataRowView cRow in dvCamps)
                {
                    TreeNode CampaignNode;
                    int StatusID = 0;
                    int CampaignID = 0;
                    string CampaignName = cRow["CampaignName"].ToString();
                    int.TryParse(cRow["CampaignID"].ToString(), out CampaignID);
                    int.TryParse(cRow["StatusID"].ToString(), out StatusID);

                    if (StatusID == 4)   // Past/Ended
                    {
                        if (chkEndedCampaigns.Checked == true)
                        {
                            CampaignName = CampaignName + " (Ended)";
                            CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                            CampaignNode.Selected = false;
                            CampaignNode.NavigateUrl = "";
                            StyleNode.ChildNodes.Add(CampaignNode);
                        }
                    }
                    else
                    {
                        CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                        CampaignNode.Selected = false;
                        CampaignNode.NavigateUrl = "";
                        StyleNode.ChildNodes.Add(CampaignNode);
                    }
                }

                // After having gone through all the campaigns, if there are any nodes added, add the style node to the tree.
                if (StyleNode.ChildNodes.Count > 0)
                    tvStyle.Nodes.Add(StyleNode);
            }
            RebuildDropDownList(dsCampaigns);
        }

        protected void tvStyle_SelectedNodeChanged(object sender, EventArgs e)
        {
            {
                string strURL = "";
                string strImage = "";
                int intImageHeight = 130;
                int intImageWidth = 820;
                string strGameOrCampaignName = "";
                tvStyle.SelectedNode.Selected = true;
                string SelectedGorC = tvStyle.SelectedValue + "";
                string GorC = SelectedGorC.Substring(0, 1);
                string stGameSystemID = SelectedGorC.Substring(1, SelectedGorC.Length - 1);
                int GameSystemID;
                int CampaignID;
                int intURLVisible = 1;
                int intImageVisible = 1;
                int intOverviewVisible = 1;
                int intSelectorsVisible = 1;
                int.TryParse(stGameSystemID, out GameSystemID);
                int.TryParse(tvStyle.SelectedValue, out CampaignID);
                if (GorC == "G") // Game System
                {
                    intSelectorsVisible = 0;
                    intURLVisible = 0;
                    intImageVisible = 0;
                    lblGorC1.Text = "Style";
                    lblGorC2.Text = "Style Description";
                    Classes.cGameSystem GS = new Classes.cGameSystem();
                    GS.Load(GameSystemID, 0);
                    strURL = GS.GameSystemURL;
                    strImage = "";
                    strGameOrCampaignName = GS.GameSystemName;
                    lblCampaignOverview.Text = GS.GameSystemWebPageDescription.Replace("\n", "<br>");
                }
                else  // Campaign
                {
                    if (CampaignID < 1) // Placeholder for Game Systems with no campaigns
                    {
                        intURLVisible = 0;
                        intImageVisible = 0;
                        intOverviewVisible = 0;
                        intSelectorsVisible = 0;
                    }
                    else
                    {
                        lblGorC1.Text = "Campaign";
                        lblGorC2.Text = "Campaign Description";
                        Classes.cCampaignBase Cam = new Classes.cCampaignBase(CampaignID, "public", 0);
                        strURL = Cam.URL;
                        strImage = Cam.Logo;
                        intImageHeight = Cam.LogoHeight;
                        intImageWidth = Cam.LogoWidth;
                        strGameOrCampaignName = Cam.CampaignName;
                        lblCampaignOverview.Text = Cam.WebPageDescription.Replace("\n", "<br>");
                        lblGameSystem1.Text = "Game System: ";
                        lblGameSystem2.Text = " " + Cam.GameSystemName;
                        lblGenre1.Text = "Genre: ";
                        lblGenre2.Text = " " + Cam.GenreList; //TODO-Rick-00 Fix this
                        lblStyle1.Text = "Style: ";
                        lblStyle2.Text = " " + Cam.StyleDescription;
                        lblTechLevel1.Text = "Tech Level: ";
                        lblTechLevel2.Text = " " + Cam.TechLevelName + Cam.TechLevelList;  //TODO-Rick-00 Fix this too
                        lblSize1.Text = "Size:";
                        lblSize2.Text = " " + Cam.CampaignSizeRange;
                        lblLocation2.Text = Cam.MarketingLocation;
                        lblEvent2.Text = string.Format("{0:MMM d, yyyy}", Cam.NextEventDate);
                    }
                }
                SetSiteImage(strImage);
                if (strURL != null)
                    SetSiteLink(strURL, strGameOrCampaignName);
                MakeDetailsVisible(intURLVisible, intImageVisible, intImageHeight, intImageWidth, intOverviewVisible, intSelectorsVisible, CampaignID);
            }
        }

        protected void chkStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStyle.Checked == true)
            {
                ddlStyle.Visible = true;
            }
            else
            {
                ddlStyle.Visible = false;
                Session.Remove("StyleFilter");
                ReloadActiveTreeView(Master.UserID);
            }
        }

        protected void ddlStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkStyle.Checked)
                Session["StyleFilter"] = ddlStyle.SelectedValue;
            else
                Session.Remove("StyleFilter");
            ReloadActiveTreeView(Master.UserID);
        }


        protected void ReloadTechLevelTreeView()
        {
            tvStyle.Nodes.Clear();
            DataSet dsCampaigns = ReloadCampaigns();

            // Tech Levels works slightly differently because a game can belong to multiple tech levels. Theres a table that lists 
            // all the campaigns/tech levels. So we build a list from that, then get the unique list, and go through.
            // Order the records by genre then make a unique list of them.
            DataView dvOrderedCampaigns = new DataView(dsCampaigns.Tables["TechLevel"], "", "SortOrder", DataViewRowState.CurrentRows);
            DataTable dtTechLevels = dvOrderedCampaigns.ToTable(true, "TechLevelID", "TechLevelName", "SortOrder");   // Unique list of tech levels.
            DataView dvTechLevels = new DataView(dtTechLevels, "", "SortOrder", DataViewRowState.CurrentRows);
            foreach (DataRowView dRow in dvTechLevels)
            {
                int TechLevelID;
                string TechLevelName = dRow["TechLevelName"].ToString();
                int.TryParse(dRow["TechLevelID"].ToString(), out TechLevelID);
                TreeNode TechLevelNode = new TreeNode(TechLevelName, "G" + TechLevelID.ToString());     // G will be asssigned to all tech level categories.
                TechLevelNode.Selected = false;
                TechLevelNode.NavigateUrl = "";

                // Build the list of campaigns that belong to this tech level. This is slightly different than other areas. 
                // Take the techlevelID, look at the list of campaigns/tech levels, then get the actual campaign record.
                DataView dvTechLevelToCampRow = new DataView(dsCampaigns.Tables["TechLevel"], "TechLevelID = " + TechLevelID.ToString(), "CampaignName", DataViewRowState.CurrentRows);
                foreach (DataRowView dTechLevelToCampRow in dvTechLevelToCampRow)
                {
                    DataView dvCamp = new DataView(dsCampaigns.Tables["Campaigns"], "CampaignID = " + dTechLevelToCampRow["CampaignID"].ToString(), "CampaignName", DataViewRowState.CurrentRows);
                    foreach (DataRowView dCamp in dvCamp)
                    {
                        TreeNode CampaignNode;
                        int StatusID = 0;
                        int CampaignID = 0;
                        string CampaignName = dCamp["CampaignName"].ToString();
                        int.TryParse(dCamp["CampaignID"].ToString(), out CampaignID);
                        int.TryParse(dCamp["StatusID"].ToString(), out StatusID);

                        if (StatusID == 4)      // Past/Ended
                        {
                            if (chkEndedCampaigns.Checked)
                            {
                                CampaignName = CampaignName + " (Ended)";
                                CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                                TechLevelNode.ChildNodes.Add(CampaignNode);
                                CampaignNode.Selected = false;
                                CampaignNode.NavigateUrl = "";
                            }
                        }
                        else
                        {
                            CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                            TechLevelNode.ChildNodes.Add(CampaignNode);
                            CampaignNode.Selected = false;
                            CampaignNode.NavigateUrl = "";
                        }
                    }
                }

                // After having gone through all the campaigns, if there are any nodes added, add the tech level node to the tree.
                if (TechLevelNode.ChildNodes.Count > 0)
                    tvTechLevel.Nodes.Add(TechLevelNode);
            }
            RebuildDropDownList(dsCampaigns);
        }

        protected void tvTechLevel_SelectedNodeChanged(object sender, EventArgs e)
        {
            string strURL = "";
            string strImage = "";
            int intImageHeight = 130;
            int intImageWidth = 820;
            string strGameOrCampaignName = "";

            string SelectedGorC = tvTechLevel.SelectedValue + "";
            string GorC = SelectedGorC.Substring(0, 1);
            //            string stGameSystemID = SelectedGorC.Substring(1, SelectedGorC.Length - 1);
            //            int GameSystemID;
            int CampaignID = 0;
            int intURLVisible = 0;
            int intImageVisible = 0;
            int intOverviewVisible = 0;
            int intSelectorsVisible = 0;

            tvTechLevel.SelectedNode.Selected = true;

            //            int.TryParse(stGameSystemID, out GameSystemID);
            if (GorC == "G")
            {
                lblGorC1.Text = "Genre";
                lblGorC2.Text = "Genre Description";
                //Classes.cGameSystem GS = new Classes.cGameSystem();
                //GS.Load(GameSystemID, 0);
                //strURL = GS.GameSystemURL;
                //strImage = "";
                //strGameOrCampaignName = GS.GameSystemName;
                //lblCampaignOverview.Text = GS.GameSystemWebPageDescription.Replace("\n", "<br>");
            }
            else
            {
                if (int.TryParse(tvTechLevel.SelectedValue, out CampaignID))
                {
                    Classes.cCampaignBase Cam = new cCampaignBase();
                    Cam = DisplayCampaignInfo(CampaignID);
                    strURL = Cam.URL;
                    strImage = Cam.Logo;
                    intImageHeight = Cam.LogoHeight;
                    intImageWidth = Cam.LogoWidth;
                    strGameOrCampaignName = Cam.CampaignName;
                    intURLVisible = 1;
                    intImageVisible = 1;
                    intOverviewVisible = 1;
                    intSelectorsVisible = 1;
                }
            }
            SetSiteImage(strImage);
            if (strURL != null)
                SetSiteLink(strURL, strGameOrCampaignName);
            MakeDetailsVisible(intURLVisible, intImageVisible, intImageHeight, intImageWidth, intOverviewVisible, intSelectorsVisible, CampaignID);
        }

        protected void chkTechLevel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTechLevel.Checked == true)
            {
                ddlTechLevel.Visible = true;
            }
            else
            {
                Session.Remove("TechLevelFilter");
                ddlTechLevel.Visible = false;
                ReloadActiveTreeView(Master.UserID);
            }
        }

        protected void ddlTechLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkTechLevel.Checked)
                Session["TechLevelFilter"] = ddlTechLevel.SelectedValue;
            else
                Session.Remove("TechLevelFilter");
            ReloadActiveTreeView(Master.UserID);
        }


        protected void ReloadSizeTreeView()
        {
            tvSize.Nodes.Clear();
            DataSet dsCampaigns = ReloadCampaigns();

            // Order the records by game size then make a unique list of them.
            DataView dvOrderedCampaigns = new DataView(dsCampaigns.Tables["Campaigns"], "", "SizeSortOrder", DataViewRowState.CurrentRows);
            DataTable dtSizes = dvOrderedCampaigns.ToTable(true, "CampaignSizeID", "CampaignSizeRange", "SizeSortOrder");   // Unique list of game sizes.

            // Go through the list of sizes and get the campaign info.
            DataView dvSortedSizes = new DataView(dtSizes, "", "SizeSortOrder", DataViewRowState.CurrentRows);
            foreach (DataRowView dRow in dvSortedSizes)
            {
                int CampaignSizeID = 0;
                string SizeName = dRow["CampaignSizeRange"].ToString();
                int.TryParse(dRow["CampaignSizeID"].ToString(), out CampaignSizeID);

                TreeNode SizeNode = new TreeNode(SizeName, "G" + CampaignSizeID.ToString()); // G will be assigned to all campaign sizes.
                SizeNode.Selected = false;
                SizeNode.NavigateUrl = "";

                // Build the list of campaigns that belong to this campaign size.
                DataView dvCamps = new DataView(dsCampaigns.Tables["Campaigns"], "CampaignSizeID = " + CampaignSizeID.ToString(), "CampaignName", DataViewRowState.CurrentRows);

                // Add each campaign to the node.
                foreach (DataRowView cRow in dvCamps)
                {
                    TreeNode CampaignNode;
                    int StatusID = 0;
                    int CampaignID = 0;
                    string CampaignName = cRow["CampaignName"].ToString();
                    int.TryParse(cRow["CampaignID"].ToString(), out CampaignID);
                    int.TryParse(cRow["StatusID"].ToString(), out StatusID);

                    if (StatusID == 4)   // Past/Ended
                    {
                        if (chkEndedCampaigns.Checked == true)
                        {
                            CampaignName = CampaignName + " (Ended)";
                            CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                            SizeNode.ChildNodes.Add(CampaignNode);
                            CampaignNode.Selected = false;
                            CampaignNode.NavigateUrl = "";
                        }
                    }
                    else
                    {
                        CampaignNode = new TreeNode(CampaignName, CampaignID.ToString());
                        SizeNode.ChildNodes.Add(CampaignNode);
                        CampaignNode.Selected = false;
                        CampaignNode.NavigateUrl = "";
                    }
                }

                // After having gone through all the campaigns, if there are any nodes added add the size node to the tree.
                if (SizeNode.ChildNodes.Count > 0)
                    tvSize.Nodes.Add(SizeNode);
            }
            RebuildDropDownList(dsCampaigns);
        }

        protected void tvSize_SelectedNodeChanged(object sender, EventArgs e)
        {
            string strURL = "";
            string strImage = "";
            int intImageHeight = 130;
            int intImageWidth = 820;
            string strGameOrCampaignName = "";
            tvSize.SelectedNode.Selected = true;
            string SelectedGorC = tvSize.SelectedValue + "";
            string GorC = SelectedGorC.Substring(0, 1);
            string stGameSystemID = SelectedGorC.Substring(1, SelectedGorC.Length - 1);
            //int GameSystemID;
            //int CampaignID;
            int intURLVisible = 1;
            int intImageVisible = 1;
            int intOverviewVisible = 1;
            int intSelectorsVisible = 1;
            int CampaignID = 0;

            lblGorC1.Text = "Size";
            lblGorC2.Text = "Size Description";

            if (GorC == "G") // Game System
            {
                intSelectorsVisible = 0;
                intURLVisible = 0;
                intImageVisible = 0;
                //Classes.cGameSystem GS = new Classes.cGameSystem();
                //GS.Load(GameSystemID, 0);
                //strURL = GS.GameSystemURL;
                strImage = "";
                strGameOrCampaignName = "Size";
                lblCampaignOverview.Text = tvSize.SelectedNode.Text;
            }
            else  // Campaign
            {
                int.TryParse(tvSize.SelectedValue, out CampaignID);
                if (CampaignID < 1)
                {
                    intURLVisible = 0;
                    intImageVisible = 0;
                    intOverviewVisible = 0;
                    intSelectorsVisible = 0;
                }
                else
                {
                    Classes.cCampaignBase Cam = new cCampaignBase();
                    Cam = DisplayCampaignInfo(CampaignID);
                    strURL = Cam.URL;
                    strImage = Cam.Logo;
                    intImageHeight = Cam.LogoHeight;
                    intImageWidth = Cam.LogoWidth;
                    strGameOrCampaignName = Cam.CampaignName;
                }
            }
            SetSiteImage(strImage);
            if (strURL != null)
                SetSiteLink(strURL, strGameOrCampaignName);
            MakeDetailsVisible(intURLVisible, intImageVisible, intImageHeight, intImageWidth, intOverviewVisible, intSelectorsVisible, CampaignID);
        }

        protected void chkSize_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSize.Checked == true)
            {
                ddlSize.Visible = true;
            }
            else
            {
                Session.Remove("SizeFilter");
                ddlSize.Visible = false;
                ReloadActiveTreeView(Master.UserID);
            }
        }

        protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkSize.Checked)
                Session["SizeFilter"] = ddlSize.SelectedValue;
            else
                Session.Remove("SizeFilter");
            ReloadActiveTreeView(Master.UserID);
        }


        protected void chkZipCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkZipCode.Checked == true)
            {
                txtZipCode.Text = "";
                txtZipCode.Attributes.Add("Placeholder", "Enter your zip code");
                txtZipCode.Visible = true;
                ddlMileRadius.Visible = true;
                LoadddlMileRadius();
            }
            else
            {
                txtZipCode.Text = "";
                txtZipCode.Visible = false;
                ddlMileRadius.Visible = false;
                Session.Remove("ZipCodeFilter");
                Session.Remove("RadiusFilter");
                ReloadActiveTreeView(Master.UserID);
            }
        }

        protected void txtZipCode_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtZipCode.Text))
            {
                txtZipCode.Focus();
            }
            else
            {
                if (txtZipCode.Text.Trim().Length == 5)
                {
                    Session["ZipCodeFilter"] = txtZipCode.Text;
                    if (!String.IsNullOrEmpty(ddlMileRadius.SelectedValue.ToString()))
                        ReloadActiveTreeView(Master.UserID);
                    else
                        ddlMileRadius.Focus();
                }
                else
                    txtZipCode.Focus();
            }
        }

        protected void ddlMileRadius_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMileRadius.SelectedIndex != 0)
            {
                Session["RadiusFilter"] = ddlMileRadius.SelectedValue;
                if (!String.IsNullOrEmpty(txtZipCode.Text))
                    ReloadActiveTreeView(Master.UserID);
                else
                    txtZipCode.Focus();
            }
            else
                ddlMileRadius.Focus();
        }

        protected void chkEndedCampaigns_CheckedChanged(object sender, EventArgs e)
        {
            ReloadActiveTreeView(Master.UserID);
        }

        protected void ReloadActiveTreeView(int UserID)
        {
            if (tvGameSystem.Visible)
                ReloadGameSystemTreeView();
            if (tvCampaign.Visible)
                ReloadCampaignTreeView();
            if (tvGenre.Visible)
                ReloadGenreTreeView();
            if (tvStyle.Visible)
                ReloadStyleTreeView();
            if (tvTechLevel.Visible)
                ReloadTechLevelTreeView();
            if (tvSize.Visible)
                ReloadSizeTreeView();
        }

        private void GetSessionFilters(ref int? GameSystemFilter, ref int? CampaignFilter, ref int? GenreFilter,
                            ref int? StyleFilter, ref int? TechLevelFilter, ref int? SizeFilter, ref string ZipCodeFilter, ref int? RadiusFilter)
        {
            int iTemp = 0;
            GameSystemFilter = null;
            if (Session["GameSystemFilter"] != null)
                if (int.TryParse(Session["GameSystemFilter"].ToString(), out iTemp))
                    GameSystemFilter = iTemp;

            CampaignFilter = null;
            if (Session["CampaignFilter"] != null)
                if (int.TryParse(Session["CampaignFilter"].ToString(), out iTemp))
                    CampaignFilter = iTemp;

            GenreFilter = null;
            if (Session["GenreFilter"] != null)
                if (int.TryParse(Session["GenreFilter"].ToString(), out iTemp))
                    GenreFilter = iTemp;

            StyleFilter = null;
            if (Session["StyleFilter"] != null)
                if (int.TryParse(Session["StyleFilter"].ToString(), out iTemp))
                    StyleFilter = iTemp;

            TechLevelFilter = null;
            if (Session["TechLevelFilter"] != null)
                if (int.TryParse(Session["TechLevelFilter"].ToString(), out iTemp))
                    TechLevelFilter = iTemp;

            SizeFilter = null;
            if (Session["SizeFilter"] != null)
                if (int.TryParse(Session["SizeFilter"].ToString(), out iTemp))
                    SizeFilter = iTemp;

            ZipCodeFilter = "";
            if (Session["ZipCodeFilter"] != null)
                ZipCodeFilter = Session["ZipCodeFilter"].ToString();

            RadiusFilter = null;
            if (Session["RadiusFilter"] != null)
                if (int.TryParse(Session["RadiusFilter"].ToString(), out iTemp))
                    RadiusFilter = iTemp;
        }

        /// <summary>
        /// Given a campaign ID, it will load the campaign and fill in the appropriate information about it on the screen. Also will get 
        /// </summary>
        /// <param name="CampaignID"></param>
        /// <returns>Campaign class with the load campaign in it.</returns>
        private Classes.cCampaignBase DisplayCampaignInfo(int CampaignID)
        {
            lblGorC1.Text = "Campaign";
            lblGorC2.Text = "Campaign Description";
            Classes.cCampaignBase Cam = new Classes.cCampaignBase(CampaignID, "public", 0);
            lblCampaignOverview.Text = Cam.WebPageDescription.Replace("\n", "<br>");
            lblGameSystem1.Text = "Game System: ";
            lblGameSystem2.Text = " " + Cam.GameSystemName;
            lblGenre1.Text = "Genre: ";
            lblGenre2.Text = " " + Cam.GenreList; //TODO-Rick-00 Fix this
            lblStyle1.Text = "Style: ";
            lblStyle2.Text = " " + Cam.StyleDescription;
            lblTechLevel1.Text = "Tech Level: ";
            lblTechLevel2.Text = " " + Cam.TechLevelName + Cam.TechLevelList;
            lblSize1.Text = "Size:";
            lblSize2.Text = " " + Cam.CampaignSizeRange;
            lblLocation2.Text = Cam.MarketingLocation;
            lblEvent2.Text = string.Format("{0:MMM d, yyyy}", Cam.NextEventDate);

            return Cam;
        }

        /// <summary>
        /// Get the list of campaigns for the user applying the appropriate filters. Will return campaigns, genres and techlevel.
        /// Genre and techlevels are separate because a campaign can be part of multiple genres and techlevels.
        /// </summary>
        /// <returns>Dataset of filtered campaigns with genres and techlevels in separate tables.</returns>
        public DataSet ReloadCampaigns()
        {
            int? GameSystemFilter = null;
            int? CampaignFilter = null;
            int? GenreFilter = null;
            int? StyleFilter = null;
            int? TechLevelFilter = null;
            int? SizeFilter = null;
            int? RadiusFilter = null;
            string ZipCodeFilter = "";
            GetSessionFilters(ref GameSystemFilter, ref CampaignFilter, ref GenreFilter, ref StyleFilter, ref TechLevelFilter, ref SizeFilter, ref ZipCodeFilter, ref RadiusFilter);
            cCampaignSelection cCampSel = new cCampaignSelection();
            DataSet dsCamp = cCampSel.LoadFilteredCampaigns(Master.UserName, GameSystemFilter, CampaignFilter, GenreFilter, StyleFilter, TechLevelFilter, SizeFilter, ZipCodeFilter, RadiusFilter);
            dsCamp.Tables[0].TableName = "Campaigns";
            dsCamp.Tables[1].TableName = "Genres";
            dsCamp.Tables[2].TableName = "TechLevel";
            return dsCamp;
        }

        /// <summary>
        /// Given a dataset of campaigns, reload the filter drop down lists.
        /// </summary>
        /// <param name="dsCampaigns">The campaigns. Use ReloadCampaigns routine - it gets the information needed.</param>
        public void RebuildDropDownList(DataSet dsCampaigns)
        {
            int? GameSystemFilter = null;
            int? CampaignFilter = null;
            int? GenreFilter = null;
            int? StyleFilter = null;
            int? TechLevelFilter = null;
            int? SizeFilter = null;
            string ZipCodeFilter = "";
            int? RadiusFilter = null;

            GetSessionFilters(ref GameSystemFilter, ref CampaignFilter, ref GenreFilter, ref StyleFilter, ref TechLevelFilter, ref SizeFilter, ref ZipCodeFilter, ref RadiusFilter);

            DataTable dtGameSystems = new DataView(dsCampaigns.Tables["Campaigns"], "", "GameSystemName", DataViewRowState.CurrentRows).ToTable(true, "GameSystemID", "GameSystemName");
            ddlGameSystem.Items.Clear();
            ddlGameSystem.DataSource = dtGameSystems;
            ddlGameSystem.DataTextField = "GameSystemName";
            ddlGameSystem.DataValueField = "GameSystemID";
            ddlGameSystem.DataBind();
            ddlGameSystem.Items.Insert(0, new ListItem("Select a Game System", ""));
            ddlGameSystem.SelectedIndex = 0;
            if (GameSystemFilter.HasValue)
                SetDropDown(ref ddlGameSystem, GameSystemFilter.Value.ToString());

            //gvCampaigns.DataSource = dsCampaigns.Tables["Campaigns"];
            //gvCampaigns.DataBind();

            DataTable dtCampaigns = new DataView(dsCampaigns.Tables["Campaigns"], "", "CampaignName", DataViewRowState.CurrentRows).ToTable(true, "CampaignID", "CampaignName");
            ddlCampaign.Items.Clear();
            ddlCampaign.DataSource = dtCampaigns;
            ddlCampaign.DataTextField = "CampaignName";
            ddlCampaign.DataValueField = "CampaignID";
            ddlCampaign.DataBind();
            ddlCampaign.Items.Insert(0, new ListItem("Select a Campaign", ""));
            ddlCampaign.SelectedIndex = 0;
            if (GameSystemFilter.HasValue)
                SetDropDown(ref ddlCampaign, CampaignFilter.Value.ToString());


            DataTable dtTechOrder = new DataView(dsCampaigns.Tables["TechLevel"], "", "SortOrder", DataViewRowState.CurrentRows).ToTable(true, "TechLevelID", "TechLevelName");
            ddlTechLevel.Items.Clear();
            ddlTechLevel.DataSource = dtTechOrder;
            ddlTechLevel.DataTextField = "TechLevelName";
            ddlTechLevel.DataValueField = "TechLevelID";
            ddlTechLevel.DataBind();
            ddlTechLevel.Items.Insert(0, new ListItem("Select a Tech Level", ""));
            ddlTechLevel.SelectedIndex = 0;
            if (TechLevelFilter.HasValue)
                SetDropDown(ref ddlTechLevel, TechLevelFilter.Value.ToString());

            DataTable dtGenre = new DataView(dsCampaigns.Tables["Genres"], "", "GenreName", DataViewRowState.CurrentRows).ToTable(true, "GenreID", "GenreName");
            ddlGenre.Items.Clear();
            ddlGenre.DataSource = dtGenre;
            ddlGenre.DataTextField = "GenreName";
            ddlGenre.DataValueField = "GenreID";
            ddlGenre.DataBind();
            ddlGenre.Items.Insert(0, new ListItem("Select a Genre", ""));
            if (GenreFilter.HasValue)
                SetDropDown(ref ddlGenre, GenreFilter.Value.ToString());

            DataTable dtStyles = new DataView(dsCampaigns.Tables["Campaigns"], "", "StyleName", DataViewRowState.CurrentRows).ToTable(true, "StyleID", "StyleName");
            ddlStyle.Items.Clear();
            ddlStyle.DataSource = dtStyles;
            ddlStyle.DataTextField = "StyleName";
            ddlStyle.DataValueField = "StyleID";
            ddlStyle.DataBind();
            ddlStyle.Items.Insert(0, new ListItem("Select a Style", ""));
            if (StyleFilter.HasValue)
                SetDropDown(ref ddlStyle, StyleFilter.Value.ToString());

            DataTable dtSize = new DataView(dsCampaigns.Tables["Campaigns"], "", "SizeSortOrder", DataViewRowState.CurrentRows).ToTable(true, "CampaignSizeID", "CampaignSizeRange");
            ddlSize.Items.Clear();
            ddlSize.DataSource = dtSize;
            ddlSize.DataTextField = "CampaignSizeRange";
            ddlSize.DataValueField = "CampaignSizeID";
            ddlSize.DataBind();
            ddlSize.Items.Insert(0, new ListItem("Select a Size", ""));
            if (SizeFilter.HasValue)
                SetDropDown(ref ddlSize, SizeFilter.Value.ToString());
        }

        /// <summary>
        /// Given a drop down list and value it will set the drop down list to value. If value doesn't exist it will pick the first item.
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="value"></param>
        public void SetDropDown(ref DropDownList ddl, string value)
        {
            ddl.ClearSelection();
            if (ddl.Items.Count > 0)
                ddl.Items[0].Selected = true;

            foreach (ListItem lItem in ddl.Items)
            {
                if (lItem.Value == value)
                {
                    ddl.ClearSelection();
                    lItem.Selected = true;
                }
            }
        }

        protected void LoadddlMileRadius()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@RadiusFilter", "0");
            DataTable dtRadii = cUtilities.LoadDataTable("uspGetRadii", sParams, "LARPortal", Master.UserName, lsRoutineName);
            DataView dvRadii = new DataView(dtRadii, "", "MaximumDistance", DataViewRowState.CurrentRows);

            ddlMileRadius.DataSource = dvRadii;
            ddlMileRadius.DataTextField = "DistanceDescription";
            ddlMileRadius.DataValueField = "DistanceID";
            ddlMileRadius.DataBind();
            ddlMileRadius.Items.Insert(0, new ListItem("Select a Maximum Distance", ""));

            int iRadiusFilter = 0;
            if (Session["RadiusFilter"] != null)
                int.TryParse(Session["RadiusFilter"].ToString(), out iRadiusFilter);
            SetDropDown(ref ddlMileRadius, iRadiusFilter.ToString());
        }
    }
}






    
        //                       <button type="button" id="sidebarCollapse" class="btn btn-info navbar-btn">
        //                        <i class="glyphicon glyphicon-align-left"></i>
        //                        <span>Toggle Sidebar</span>
        //                    </button>
   
    
    
    
    
        //    <nav id="sidebar">
        //    <div class="sidebar-header">
        //        <h3>Campaign Filters</h3>
        //    </div>

        //    <ul class="list-unstyled components">
        //        <li class="active">
        //            <a href="#homeSubmenu" data-toggle="collapse" aria-expanded="false">Home</a>
        //            <ul class="collapse list-unstyled" id="homeSubmenu">
        //                <li>Item i</li>
        //                <li>Item 2</li>
        //            </ul>
        //        </li>
        //    </ul>
        //</nav>





