using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using LarpPortal.Classes;

namespace LarpPortal.Donations
{
    public partial class ClaimDonation : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlDeliveryLoad();
                ShowDonateButton();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int intDonationID = 0;
            if (Session["DonationID"] == null)
            {

                Response.Redirect("DonationClaim.aspx", true);
            }
            else
            {
                string sDonationID = Session["DonationID"] as string;
                intDonationID = Int32.Parse(sDonationID);
            }

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;



            Classes.cDonation PlayerDonation = null;
            PlayerDonation = new Classes.cDonation();
            DataTable dtPlayerClaims = new DataTable();
            dtPlayerClaims = PlayerDonation.GetDonationClaimsForPlayer(Master.UserID, Master.CampaignID, intDonationID);
            int PlayerID = 0;
            int ddlEnable = 0;
            bool PCsOnly = false;  // By default return all roles
            decimal ATC = 0;
            foreach (DataRow dRow in dtPlayerClaims.Rows)
            {
                lbEventName.Text = dRow["EventName"].ToString();
                lblItem.Text = dRow["Item"].ToString();
                lblValue.Text = dRow["DisplayWorth"].ToString();
                PlayerID = (int)dRow["PlayerID"];
                ATC = Convert.ToDecimal(dRow["AvailableToClaim"]);
                Session["EventID"] = dRow["EventID"];
            }

            if (!IsPostBack)
            {
                if (Session["AllowPlayerToPlayerPoints"].ToString() == "True" && Session["AwardWhen"].ToString() == "1")
                {
                    ddlEnable = 1;
                    lblCampaignCPDonationPolicy.Text = Master.CampaignName + " allows players to assign donation rewards to other players .";
                }
                else
                {
                    if (Session["AllowPlayerToPlayerPoints"].ToString() == "True")
                    {
                        lblCampaignCPDonationPolicy.Text = Master.CampaignName + " does not allow players to assign donation rewards to other players until the donation is accepted.";
                    }
                    else
                    {
                        lblCampaignCPDonationPolicy.Text = Master.CampaignName + " does not allows players to assign donation rewards to other players .";
                    }
                }
                //lblCampaignCPDonationPolicy.Text = Master.CampaignName + allows + "players to assign donation rewards to other players while claiming t.";
                // Populate ddlReceivingPlayer - The player who will receive the credit for the donation. Default to logged in player.
                SortedList sParams = new SortedList();
                sParams = new SortedList();
                sParams.Add("@CampaignID", Master.CampaignID);
                sParams.Add("@UserID", Master.UserID);
                // If campaign doesn't allow staff and NPCs to donate
                // PCsOnly = true; // We have no logic for this currently, but if we need it, this is where it would go.
                // Optional parameter; the stored proc defaults to false.
                sParams.Add("@PCOnly", PCsOnly);
                DataTable dtPlayers = cUtilities.LoadDataTable("uspGetCampaignPCsForDonations", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignPCsForDonations");

                if (dtPlayers.Columns["DisplayValue"] == null)
                    dtPlayers.Columns.Add(new DataColumn("DisplayValue", typeof(string), "Convert(PlayerName, 'System.String') + ' - ' + Convert(LoginUserName, 'System.String')"));

                DataView dvPlayers = new DataView(dtPlayers, "", "DisplayValue", DataViewRowState.CurrentRows);
                ddlReceivingPlayer.DataSource = dvPlayers;
                ddlReceivingPlayer.DataTextField = "PlayerName";
                ddlReceivingPlayer.DataValueField = "CampaignPlayerID";
                ddlReceivingPlayer.DataBind();

                if (ddlReceivingPlayer.Items.Count > 0)
                {
                    ddlReceivingPlayer.SelectedValue = Session["CampaignPlayerID"].ToString();
                }
                if (ddlEnable == 1)
                    ddlReceivingPlayer.Enabled = true;
                //if (Session["AllowPlayerToPlayerPoints"].ToString() == "True")
                //    ddlReceivingPlayer.Enabled = true;

                ddlReceivingPlayer_SelectedIndexChanged(null, null);
            }
            // Populate ddlQtyClaim - Add 1 to quantity available to claim. Default to 1.
            //ddlQtyClaim.Items.AddRange(Enumerable.Range(1, 100).Select(e => new ListItem(e.ToString())).ToArray());
            if (!IsPostBack)
            {
                ddlQtyClaim.Items.Clear();
                for (int i = 1; i <= ATC; i++)
                {

                    ddlQtyClaim.Items.Add(i.ToString());
                }
            }


            Classes.cDonation PlayerDonationHistory = null;
            PlayerDonationHistory = new Classes.cDonation();
            DataTable dtPlayerDonationHistory = new DataTable();
            dtPlayerDonationHistory = PlayerDonationHistory.GetDonationClaims(Master.UserID, intDonationID, PlayerID);
            DataView dvPlayerDonationHistory = new DataView(dtPlayerDonationHistory, "", "", DataViewRowState.CurrentRows);
            gvPreviouslyClaimed.DataSource = dvPlayerDonationHistory;
            gvPreviouslyClaimed.DataBind();

        }

        private void ddlDeliveryLoad()
        {

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            SortedList sParams = new SortedList();

            DataTable dtDelivery = Classes.cUtilities.LoadDataTable("uspGetDonationDeliveryMethods", sParams, "LARPortal", Master.UserName, lsRoutineName + "uspGetDonationDeliveryMethods");
            ddlDelivery.DataTextField = "DeliveryDescription";
            ddlDelivery.DataValueField = "DeliveryMethodID";
            ddlDelivery.DataSource = dtDelivery;
            ddlDelivery.DataBind();
            ddlDelivery.Items.Insert(0, new ListItem("Select Delivery Method", "0"));
            ddlDelivery.SelectedIndex = 0;

        }


        protected void gvPreviouslyClaimed_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnRegisterForDonation_Click(object sender, EventArgs e)
        {

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            string jsString = "alert('You have been registered for the donation.');";

            // AwardWhen - 1 (Award Immediately) - 2 (Award on delivery) - 3 (Award on approval)
            if (Session["AwardWhen"].ToString() == "1")
            {
                // Does campaign allow for immediate award?
                //    Yes - add opportunity and audit
                AddOpportunityAndAudit(1);
                jsString = "alert('You have been registered for the donation and the points have been assigned.');";

            }
            else
            {
                AddOpportunityAndAudit(0);
            }

            try
            {

                //string jsString = "alert('You have been registered for the donation.');";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                        "MyApplication",
                        jsString,
                        true);
            }
            catch (Exception ex)
            {
                string t = ex.Message;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
            }

            // After donation accepted go back to donation page
            Response.Redirect("DonationClaim.aspx", true);
        }

        protected void AddOpportunityAndAudit(int AddAudit)
        {
            // Add Opportunity record

            Classes.cPoints Points = new Classes.cPoints();
            int Qty = Int32.Parse(ddlQtyClaim.SelectedValue);
            int CharacterID = 0;
            int RegistrationID = 0;
            int CPOppID = 0;
            int CPOpportunityID = Convert.ToInt32(Session["CampaignCPOpportunityDefaultID"]);
            int PlayerReceivingCP = Int32.Parse(ddlReceivingPlayer.SelectedValue);
            string DeliveryMethod = ddlDelivery.SelectedItem.ToString();
            int PlayerEarningCP = Int32.Parse(Session["CampaignPlayerID"].ToString());
            int EventID = Convert.ToInt32(Session["EventID"].ToString());
            int DonationID = Convert.ToInt32(Session["DonationID"].ToString());
            string URL = "";
            int ReasonID = 2;
            int ApprovedByID = 0;
            int StatusID;
            int ReceivedByID = 0;
            int AcceptedBy = 0;
            string PlayerComments = tbCommentsToStaff.Text;
            string StaffComments = "";
            string Comments = "";

            // DonationClaimID -1 = Claiming player getting points / -2 = Points going to another player
            int DonationClaimID = -1;

            if (PlayerReceivingCP != PlayerEarningCP)
            {
                DonationClaimID = -2;
            }
            DateTime CPAssignmentDate = DateTime.Today;
            DateTime ReceiptDate = DateTime.Parse("0001-01-01");
            int AddedByID = Master.UserID;
            double CPValue = double.Parse(Session["Worth"].ToString()) * double.Parse(ddlQtyClaim.SelectedValue);

            // If AddAudit is 1 then add Opportunity AND Audit record. If AddAudit = 0 then only add Opportunity.
            if (AddAudit == 1)  // Opp and audit
            {
                StatusID = 21;

            }
            else               // Opportunity only
            {
                StatusID = 19;

            }

            Points.AddDonationOpportunity(Master.UserID, PlayerReceivingCP, CharacterID, CPOpportunityID, EventID, Master.CampaignID,
                lblItem.Text, PlayerComments, URL, ReasonID, StatusID, AddedByID, CPValue, ApprovedByID, ReceiptDate, ReceivedByID,
                CPAssignmentDate, "", "Donation", AddAudit, DonationClaimID, PlayerEarningCP);

            DonationClaimID = -1;
            CPOppID = Convert.ToInt32(Session["CampaignCPOpportunityID"].ToString());

            Classes.cDonation Claim = new Classes.cDonation();
            Claim.SaveDonationClaims(Master.UserID, DonationClaimID, DonationID, PlayerEarningCP, Qty, RegistrationID, CPOppID, PlayerComments,
               StaffComments, DeliveryMethod, PlayerEarningCP, ReceiptDate, AcceptedBy, Comments);

        }

        protected void ShowDonateButton()
        {

            int Qty;
            int iCheck;
            if (Int32.TryParse(ddlQtyClaim.SelectedValue, out iCheck))
            {
                Qty = iCheck;
            }
            else
            {
                Qty = 0;
            }

            int Delivery = ddlDelivery.SelectedIndex;

            if (Qty > 0 && Delivery > 0)
            {
                btnRegisterForDonation.Visible = true;
            }
        }

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {

        }

        protected void ddlReceivingPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowDonateButton();
        }

        protected void ddlQtyClaim_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowDonateButton();
        }

        protected void ddlDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowDonateButton();
        }
    }
}
