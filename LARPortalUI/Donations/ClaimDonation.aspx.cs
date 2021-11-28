using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LarpPortal.Classes;

namespace LarpPortal.Donations
{
    public partial class ClaimDonation : System.Web.UI.Page
    {

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
            decimal ATC = 0;
            foreach(DataRow dRow in dtPlayerClaims.Rows)
            {
                lbEventName.Text = dRow["EventName"].ToString();
                lblItem.Text = dRow["Item"].ToString();
                lblValue.Text = dRow["DisplayWorth"].ToString();
                PlayerID = (int)dRow["PlayerID"];
                ATC = Convert.ToDecimal(dRow["AvailableToClaim"]);
            }

            if (!IsPostBack)
            {

                // Populate ddlReceivingPlayer - The player who will receive the credit for the donation. Default to logged in player.
                SortedList sParams = new SortedList();
                sParams = new SortedList();
                sParams.Add("@CampaignID", Master.CampaignID);
                DataTable dtPlayers = cUtilities.LoadDataTable("uspGetCampaignPCs", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignPCs");

                if (dtPlayers.Columns["DisplayValue"] == null)
                    dtPlayers.Columns.Add(new DataColumn("DisplayValue", typeof(string), "Convert(PlayerName, 'System.String') + ' - ' + Convert(LoginUserName, 'System.String')"));

                DataView dvPlayers = new DataView(dtPlayers, "", "DisplayValue", DataViewRowState.CurrentRows);
                ddlReceivingPlayer.DataSource = dvPlayers;
                ddlReceivingPlayer.DataTextField = "PlayerName";
                ddlReceivingPlayer.DataValueField = "CampaignPlayerID";
                ddlReceivingPlayer.DataBind();

                if (ddlReceivingPlayer.Items.Count > 0)
                {
                    ddlReceivingPlayer.SelectedValue = PlayerID.ToString();
                }
                if (Session["AllowPlayerToPlayerPoints"].ToString() == "True")
                    ddlReceivingPlayer.Enabled = true;

                ddlReceivingPlayer_SelectedIndexChanged(null, null);
            }
            // Populate ddlQtyClaim - Add 1 to quantity available to claim. Default to 1.
            //ddlQtyClaim.Items.AddRange(Enumerable.Range(1, 100).Select(e => new ListItem(e.ToString())).ToArray());
            if (!IsPostBack)
            {
                ddlQtyClaim.Items.Clear();
                for(int i = 1; i <= ATC; i++)
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

        protected void gvPreviouslyClaimed_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnRegisterForDonation_Click(object sender, EventArgs e)
        {

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;


            // Does campaign allow for immediate award?
            //    No - add opportunity only
 

            //   Yes - add opportunity and audit







            Classes.cPoints PlayerAudit = null;
            PlayerAudit = new Classes.cPoints();


            try
            {
 
                string jsString = "alert('You have been registered for the donation.');";
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

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            
        }

        protected void ddlReceivingPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlQtyClaim_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
