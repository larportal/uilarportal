using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

namespace LarpPortal.Donations
{
    public partial class DonationClaim : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetCampaignPlayerID();
                CampaignDonationSettings();
                gvDonationList.DataSource = GetData("uspGetDonations", Master.CampaignID, "", "", "", Master.UserName, Master.RoleString, Master.UserID);
                gvDonationList.DataBind();
            }
        }

        private static DataTable GetData(string query, Int32 CampaignID, string ddlStatus, string ddlEvent, string DonationID, string UserName, string RoleString, int uID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            SortedList sParams = new SortedList();
            // Common parameters
            if (CampaignID != 0)
                sParams.Add("@CampaignID", CampaignID);
            if (ddlStatus != "")
                sParams.Add("@StatusID", ddlStatus);
            if (ddlEvent != "")
                sParams.Add("@EventID", ddlEvent);
            if (DonationID != "")
                sParams.Add("DonationID", DonationID);
            // Query specific parameters
            switch (query)
            {
                case "uspGetDonations":
                    sParams.Add("@Roles", RoleString);
                    break;
                case "uspGetDonationClaims":
                    sParams.Add("@Roles", RoleString);
                    sParams.Add("@UserID", uID);
                    break;
                default:
                    break;
            }

            DataTable dt = Classes.cUtilities.LoadDataTable(query, sParams, "LARPortal", UserName, lsRoutineName + "." + query);
            return dt;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            BindData();
            if (!IsPostBack)
            {
                ddlPlayerLoad(Master.UserID, Master.CampaignID);
            }
        }

        protected void BindData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@EventID", ddlEvent.SelectedValue);
            sParams.Add("@StatusID", ddlDonationStatus.SelectedValue);
            sParams.Add("@Roles", Master.RoleString);
            sParams.Add("@UserID", Master.UserID);

            DataTable dtDonations = Classes.cUtilities.LoadDataTable("uspGetDonations", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetDonations");

            // While creating the filter am also saving the selected values so we can go back and have the drop down list use them.
            string sRowFilter = "(1 = 1)";      // This is so it's easier to build the filter string. Now can always say 'and ....'

            // Default setting
            DataView dvDonations = new DataView(dtDonations, sRowFilter, "EventID desc, StatusID", DataViewRowState.CurrentRows);
            gvDonationList.DataSource = dvDonations;
            gvDonationList.DataBind();

            if (!IsPostBack)
            {
                Session["DonationList_dvDonations"] = dvDonations;

                DataView view = new DataView(dtDonations);
                DataTable dtDistinctDonationStatus = view.ToTable(true, "StatusID", "StatusName");

                ddlDonationStatus.DataSource = dtDistinctDonationStatus;
                ddlDonationStatus.DataTextField = "StatusName";
                ddlDonationStatus.DataValueField = "StatusID";
                ddlDonationStatus.DataBind();
                ddlDonationStatus.Items.Insert(0, new ListItem("No Filter", ""));
                ddlDonationStatus.SelectedIndex = -1;

                if (ddlDonationStatus.SelectedIndex == -1)     // Didn't find what was selected.
                    ddlDonationStatus.SelectedIndex = 0;

                view = new DataView(dtDonations);   //, sRowFilter, "EventID", DataViewRowState.CurrentRows);
                DataTable dtEvent = view.ToTable(true, "EventID", "EventDate");

                ddlEvent.DataSource = dtEvent;
                ddlEvent.DataTextField = "EventDate";
                ddlEvent.DataTextFormatString = "{0:MM/dd/yyyy}";
                ddlEvent.DataValueField = "EventID";
                ddlEvent.DataBind();
                ddlEvent.Items.Insert(0, new ListItem("No Filter", ""));
                ddlEvent.SelectedIndex = -1;

                if (ddlEvent.SelectedIndex == -1)     // Didn't find what was selected.
                    ddlEvent.SelectedIndex = 0;
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
                string currentDonationID = gvDonationList.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView gvDetails = e.Row.FindControl("gvDetails") as GridView;
                gvDetails.DataSource = GetData("uspGetDonationClaims", 0, "", "", currentDonationID, Master.UserName, Master.RoleString, Master.UserID);
                gvDetails.DataBind();
            }
        }

        protected void ddlDonationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlDonationStatus";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlDonationStatus.SelectedValue;
            Option.SaveOptionValue();
            ViewState["DonationStatus"] = ddlDonationStatus.SelectedValue;
            BindData();
        }

        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlEvent";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlEvent.SelectedValue;
            Option.SaveOptionValue();
            ViewState["Event"] = ddlEvent.SelectedValue;
            BindData();
        }

        private void ddlPlayerLoad(int intUserID, int intCampaignID)
        {
            bool IsPC = Master.RoleString.Contains("/8/");
            bool Role4 = Master.RoleString.Contains("/4/");
            bool Role14 = Master.RoleString.Contains("/14/");
            bool Role16 = Master.RoleString.Contains("/16/");
            bool Role28 = Master.RoleString.Contains("/28/");
            bool Role34 = Master.RoleString.Contains("/34/");
            bool Role40 = Master.RoleString.Contains("/40/");

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlPlayer.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", intCampaignID);
            sParams.Add("@UserID", Master.UserID);
            DataTable dtPlayers = Classes.cUtilities.LoadDataTable("uspGetCampaignPlayersByRole", sParams, "LARPortal", Master.UserName, lsRoutineName + "uspGetCampaignPlayersByRole");
            ddlPlayer.DataTextField = "PlayerName";
            ddlPlayer.DataValueField = "UserID";
            ddlPlayer.DataSource = dtPlayers;
            ddlPlayer.DataBind();
            ddlPlayer.Items.Insert(0, new ListItem("Select Player", "0"));
            ddlPlayer.SelectedIndex = 0;
            if (Role4 || Role16 || Role14 || Role28 || Role34 || Role40)
            {
                pnlPlayerDropdown.Visible = true;
            }

        }

        protected void ddlPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvDonationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow clickedRow = gvDonationList.Rows[index];
            HiddenField hdfield = (HiddenField)clickedRow.Cells[2].FindControl("hidDonationID");
            HiddenField hdworth = (HiddenField)clickedRow.Cells[2].FindControl("hidWorth");
            HiddenField hdcampaignskillpool = (HiddenField)clickedRow.Cells[2].FindControl("hidCampaignSkillPoolID");
            HiddenField hddefaultpool = (HiddenField)clickedRow.Cells[2].FindControl("hidDefaultPool");
            string sDonationID = hdfield.Value;
            string sWorth = hdworth.Value;
            string sCampaignSkillPool = hdcampaignskillpool.Value;
            string sDefaultPool = hddefaultpool.Value;

            if (e.CommandName.CompareTo("DonateItem") == 0)
            {
                // The Donate button has been pressed. Open a form to donate. If you already claimed part of this item, preload it with the current quantity claimed.
                // Don't allow it to back out. Include a cancellation request button to send a cancellation request.
                Session["DonationID"] = sDonationID;
                Session["AllowPlayerToPlayerPoints"] = hidAllowPlayerToPlayerPoints.Value;
                Session["AwardWhen"] = hidDefaultAwardWhen.Value;
                Session["CPOpportunityID"] = hidCPOpportunityID.Value;
                Session["Worth"] = sWorth;
                Session["CampaignSkillPoolID"] = sCampaignSkillPool;
                Session["DefaultPool"] = sDefaultPool;
                Response.Redirect("ClaimDonation.aspx", true);

            }
            if (e.CommandName.CompareTo("ShowDetails") == 0)
            {

            }
        }

        protected void SetCampaignPlayerID()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Classes.cUserCampaign cuID = new Classes.cUserCampaign();
            cuID.Load(Master.UserID, Master.CampaignID);
            Session["CampaignPlayerID"] = cuID.CampaignPlayerID;
        }

        protected void CampaignDonationSettings()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Classes.cDonation DonationSettings = new Classes.cDonation();
            DonationSettings.GetDonationCampaignSettings(Master.CampaignID);
            hidAllowPlayerToPlayerPoints.Value = DonationSettings.AllowPlayerToPlayerPoints.ToString();
            hidShowDonationClaims.Value = DonationSettings.ShowDonationClaims.ToString();
            hidCountTransfersAgainstMax.Value = DonationSettings.CountTransfersAgainstMax.ToString();
            hidDefaultAwardWhen.Value = DonationSettings.DefaultAwardWhen.ToString();
            hidMaxItemsPerEvent.Value = DonationSettings.MaxItemsPerEvent.ToString();
            hidMaxPointsPerEvent.Value = DonationSettings.MaxPointsPerEvent.ToString();
            hidCPOpportunityID.Value = DonationSettings.CampaignCPOpportunityID.ToString();
            hidCampaignCPOpportunityDefaultID.Value = DonationSettings.CampaignCPOpportunityDefaultID.ToString();
            Session["CampaignCPOpportunityDefaultID"] = DonationSettings.CampaignCPOpportunityDefaultID.ToString();
        }

    }
}