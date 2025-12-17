using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
    public partial class Dashboard : System.Web.UI.Page
    {
        bool _ReloadData = false;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            Classes.cPlayer player = new Classes.cPlayer(Master.UserID, Master.UserName);
            if (player.HomeScreen.ToUpper().StartsWith("H"))
                Response.Redirect("/Default.aspx", true);

            if ((!IsPostBack) ||
                (_ReloadData))
            {
                DataSet dsItems = new DataSet();
                SortedList sParams = new SortedList();
                sParams.Add("@UserID", Master.UserID);
                dsItems = Classes.cUtilities.LoadDataSet("uspGetDashboard", sParams, "LARPortal", "Admin", lsRoutineName);

                DataView dvCharacters = new DataView(dsItems.Tables[0], "", "CharacterAKA", DataViewRowState.CurrentRows);
                gvCharacters.DataSource = dvCharacters;
                gvCharacters.DataBind();

                DataView dvDonations = new DataView(dsItems.Tables[1], "", "CampaignName", DataViewRowState.CurrentRows);
                gvDonations.DataSource = dvDonations;
                gvDonations.DataBind();

                ddlCampaigns.DataSource = dvDonations;
                ddlCampaigns.DataTextField = "CampaignName";
                ddlCampaigns.DataValueField = "CampaignID";
                ddlCampaigns.DataBind();
                ddlCampaigns.Items.Insert(0, new ListItem("myCampaigns", "0"));
                ddlCampaigns.Items[0].Selected = true;

                DataView dvUpcomingEvents = new DataView(dsItems.Tables[2], "", "StartDate", DataViewRowState.CurrentRows);
                gvUpcomingEvents.DataSource = dvUpcomingEvents;
                gvUpcomingEvents.DataBind();

                DataView dvRecentEvents = new DataView(dsItems.Tables[3], "", "StartDate", DataViewRowState.CurrentRows);
                gvRecentEvents.DataSource = dvRecentEvents;
                gvRecentEvents.DataBind();

                Session["DashboardData"] = dsItems;
            }
        }

        protected void gvCharacters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowID;
            int.TryParse(e.CommandArgument.ToString(), out RowID);

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
            Session.Remove("CurrentChar");
            int iSkillSetID = Convert.ToInt32(gvCharacters.DataKeys[RowID].Values[0]);
            int iCharID = Convert.ToInt32(gvCharacters.DataKeys[RowID].Values[1]);

            int iCampaignID = Convert.ToInt32(gvCharacters.DataKeys[RowID].Values[3]);

            Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
            UserInfo.LastLoggedInCampaign = iCampaignID;
            UserInfo.LastLoggedInCharacter = iCharID;
            UserInfo.LastLoggedInSkillSetID = iSkillSetID;
            UserInfo.LastLoggedInMyCharOrCamp = "M";
            UserInfo.Save();

            Session.Remove("MyCharacters");

            switch (e.CommandName.ToString())
            {
                case "TotalCP":
                case "AvailCP":
                    Response.Redirect("/Character/CharSkills.aspx", true);
                    break;

                case "History":
                    Response.Redirect("/Character/History/Edit.aspx", true);
                    break;

                case "NumPeople":
                    Response.Redirect("/Character/CharRelationships.aspx", true);
                    break;

                case "NumPlaces":
                    Response.Redirect("/Character/CharPlaces.aspx", true);
                    break;

                default:
                    Response.Redirect("/Character/CharInfo.aspx", true);
                    break;
            }
        }

        protected void ddlCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsItems = (DataSet)Session["DashboardData"];
            string RowFilter = "";
            if (ddlCampaigns.SelectedValue != "0")
                RowFilter = "CampaignID = " + ddlCampaigns.SelectedValue;
            DataView dvDonations = new DataView(dsItems.Tables[1], RowFilter, "CampaignName", DataViewRowState.CurrentRows);
            gvDonations.DataSource = dvDonations;
            gvDonations.DataBind();

            DataView dvCharacters = new DataView(dsItems.Tables[0], RowFilter, "CharacterAKA", DataViewRowState.CurrentRows);
            gvCharacters.DataSource = dvCharacters;
            gvCharacters.DataBind();

            DataView dvUpcomingEvents = new DataView(dsItems.Tables[2], RowFilter, "StartDate", DataViewRowState.CurrentRows);
            gvUpcomingEvents.DataSource = dvUpcomingEvents;
            gvUpcomingEvents.DataBind();

            DataView dvRecentEvents = new DataView(dsItems.Tables[3], RowFilter, "StartDate", DataViewRowState.CurrentRows);
            gvRecentEvents.DataSource = dvRecentEvents;
            gvRecentEvents.DataBind();
        }

        protected void gvDonations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowID;
            int.TryParse(e.CommandArgument.ToString(), out RowID);
            int iCampaignID = Convert.ToInt32(gvDonations.DataKeys[RowID].Values[0]);

            Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
            UserInfo.LastLoggedInCampaign = iCampaignID;
            UserInfo.LastLoggedInCharacter = -1;
            UserInfo.LastLoggedInSkillSetID = -1;
            UserInfo.LastLoggedInMyCharOrCamp = "M";
            UserInfo.Save();

            Session["ReloadCampaigns"] = "Reset";

            switch (e.CommandName.ToString())
            {
                case "Banked":
                    Session["DashboardCampaignID"] = iCampaignID;
                    Response.Redirect("/Points/MemberPointsView.aspx", true);
                    break;

                case "Donations":
                    Session["DashboardCampaignID"] = iCampaignID;
                    Session["DashboardLatestEvent"] = 1;
                    Response.Redirect("/Donations/DonationClaim.aspx", true);
                    break;

                default:
                    Response.Redirect("/Character/CharInfo.aspx", true);
                    break;
            }
        }

        protected void gvUpcomingEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowID;
            int.TryParse(e.CommandArgument.ToString(), out RowID);

            int iCharID = Convert.ToInt32(gvUpcomingEvents.DataKeys[RowID].Values[0]);
            int iSkillSetID = Convert.ToInt32(gvUpcomingEvents.DataKeys[RowID].Values[1]);
            int iCampaignID = Convert.ToInt32(gvUpcomingEvents.DataKeys[RowID].Values[2]);
            int iEventID = Convert.ToInt32(gvUpcomingEvents.DataKeys[RowID].Values[3]);

            Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
            Session["ReloadCampaigns"] = "Reset";

            switch (e.CommandName.ToString())
            {
                case "Event":
                case "Status":
                    UserInfo.LastLoggedInCampaign = iCampaignID;
                    UserInfo.LastLoggedInCharacter = -1;
                    UserInfo.LastLoggedInSkillSetID = -1;
                    UserInfo.LastLoggedInMyCharOrCamp = "M";
                    UserInfo.Save();

                    Session["DashboardCampaignID"] = iCampaignID;
                    Session["DashboardEventID"] = iEventID;
                    Response.Redirect("/Events/EventRegistration.aspx", true);
                    break;

                case "Role":
                case "ISAvail":
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
                    Session.Remove("CurrentChar");

                    UserInfo.LastLoggedInCampaign = iCampaignID;
                    UserInfo.LastLoggedInCharacter = iCharID;
                    UserInfo.LastLoggedInSkillSetID = iSkillSetID;
                    UserInfo.LastLoggedInMyCharOrCamp = "M";
                    UserInfo.Save();

                    Session.Remove("MyCharacters");

                    if (e.CommandName.ToString() == "Role")
                        Response.Redirect("/Character/CharInfo.aspx", true);
                    else
                    {
                        Session["DashboardSkillSetID"] = iSkillSetID;
                        Response.Redirect("/Character/ISkills/Requests.aspx", true);
                    }
                    break;

                default:
                    Response.Redirect("/Character/CharInfo.aspx", true);
                    break;
            }
        }

        protected void gvRecentEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowID;
            int.TryParse(e.CommandArgument.ToString(), out RowID);

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
            Session.Remove("CurrentChar");
            int iCharID = Convert.ToInt32(gvRecentEvents.DataKeys[RowID].Values[0]);
            int iSkillSetID = Convert.ToInt32(gvRecentEvents.DataKeys[RowID].Values[1]);
            int iCampaignID = Convert.ToInt32(gvRecentEvents.DataKeys[RowID].Values[2]);
            DateTime dtTemp;
            DateTime dtEventDate = DateTime.Today;
            if (DateTime.TryParse(gvRecentEvents.DataKeys[RowID].Values[3].ToString(), out dtTemp))
                dtEventDate = dtTemp;

            Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
            Session["ReloadCampaigns"] = "Reset";
            Session["DashboardCampaignID"] = iCampaignID;

            UserInfo.LastLoggedInCampaign = iCampaignID;
            UserInfo.LastLoggedInCharacter = iCharID;
            UserInfo.LastLoggedInSkillSetID = iSkillSetID;
            UserInfo.LastLoggedInMyCharOrCamp = "M";
            UserInfo.Save();

            Session.Remove("MyCharacters");

            switch (e.CommandName.ToString())
            {
                case "PEL":
                    Response.Redirect("/PELs/PELList.aspx");
                    break;

                case "RoleChar":
                    Response.Redirect("/Character/CharInfo.aspx", true);
                    break;

                case "ISReq":
                    Session["DashboardSkillSetID"] = iSkillSetID;
                    Session["DashboardEventDate"] = dtEventDate;
                    Classes.cUserOption Option = new Classes.cUserOption();
                    Option.LoginUsername = Master.UserName;
                    Option.PageName = "/Character/ISkills/Requests.aspx";
                    Option.ObjectName = "ddlEventDate";
                    Option.ObjectOption = "SelectedValue";
                    Option.OptionValue = dtEventDate.ToString("MM/dd/yyyy");
                    Option.SaveOptionValue();
                    Response.Redirect("/Character/ISkills/Requests.aspx", true);
                    break;

                default:
                    Response.Redirect("/Character/CharInfo.aspx", true);
                    break;
            }
        }

        protected void gvRecentEvents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hidRoleCharacter = (HiddenField)(e.Row.FindControl("hidRoleChar"));
                if (hidRoleCharacter != null)
                {
                    LinkButton lbRoleChar = (LinkButton)(e.Row.FindControl("lbRoleChar"));
                    Label lblRolwChar = (Label)(e.Row.FindControl("lblRoleChar"));
                    if (hidRoleCharacter.Value.StartsWith("P"))
                    {
                        if (lbRoleChar != null)
                            lbRoleChar.Visible = true;
                        if (lblRolwChar != null)
                            lblRolwChar.Visible = false;
                    }
                    else
                    {
                        if (lbRoleChar != null)
                            lbRoleChar.Visible = false;
                        if (lblRolwChar != null)
                            lblRolwChar.Visible = true;

                    }
                }
                LinkButton lbISReq = (LinkButton)(e.Row.FindControl("lbIsReq"));
                Label lblISReq = (Label)(e.Row.FindControl("lblISReq"));
                if (lblISReq.Text.StartsWith("n"))
                {
                    if (lbISReq != null)
                        lbISReq.Visible = false;
                    if (lblISReq != null)
                        lblISReq.Visible = true;
                }
                else
                {
                    if (lbISReq != null)
                        lbISReq.Visible = true;
                    if (lblISReq != null)
                        lblISReq.Visible = false;
                }
            }
        }

        protected void gvUpcomingEvents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hidRoleCharacter = (HiddenField)(e.Row.FindControl("hidRoleChar"));
                if (hidRoleCharacter != null)
                {
                    LinkButton lbRoleChar = (LinkButton)(e.Row.FindControl("lbRoleChar"));
                    Label lblRolwChar = (Label)(e.Row.FindControl("lblRoleChar"));
                    if (hidRoleCharacter.Value.StartsWith("P"))
                    {
                        if (lbRoleChar != null)
                            lbRoleChar.Visible = true;
                        if (lblRolwChar != null)
                            lblRolwChar.Visible = false;
                    }
                    else
                    {
                        if (lbRoleChar != null)
                            lbRoleChar.Visible = false;
                        if (lblRolwChar != null)
                            lblRolwChar.Visible = true;

                    }
                }
                LinkButton lbISAvail = (LinkButton)(e.Row.FindControl("lbISAvail"));
                Label lblISAvail = (Label)(e.Row.FindControl("lblISAvail"));
                if (lblISAvail.Text.StartsWith("n"))
                {
                    if (lbISAvail != null)
                        lbISAvail.Visible = false;
                    if (lblISAvail != null)
                        lblISAvail.Visible = true;
                }
                else
                {
                    if (lbISAvail != null)
                        lbISAvail.Visible = true;
                    if (lblISAvail != null)
                        lblISAvail.Visible = false;
                }
            }
        }

        protected void btnHomeView_Click(object sender, EventArgs e)
        {
            Classes.cPlayer player = new Classes.cPlayer(Master.UserID, Master.UserName);
            player.HomeScreen = "Home";
            player.Save();
            Response.Redirect("/Default.aspx", true);
        }
    }
}