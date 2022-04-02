using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Donations
{
    public partial class DonationEventList : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataTable dtPELs = new DataTable();
            SortedList sParams = new SortedList();

            if (Session["CampaignID"] == null)
                return;

            sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DataTable dtEventInfo = Classes.cUtilities.LoadDataTable("uspGetEventsWODonationListsForCampaign", sParams, "LARPortal", Master.UserName, "DonationEventList.GetEventList");
            ddlMissedEvents.DataSource = dtEventInfo;
            ddlMissedEvents.DataTextField = "EventDate";
            ddlMissedEvents.DataValueField = "EventID";
            ddlMissedEvents.DataBind();

            if (dtEventInfo.Rows.Count == 0)
            {
                divEventList.Visible = false;
                divNoEvents.Visible = true;
                btnDonationsForEvent.Visible = false;
            }
            else
            {
                divEventList.Visible = true;
                divNoEvents.Visible = false;
                btnDonationsForEvent.Visible = true;
            }


        }

        protected void ddlMissedEvents_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void btnDonationsForEvent_Click(object sender, EventArgs e)
        {

            Response.Redirect("DonationAdd.aspx?EventID=" + ddlMissedEvents.SelectedValue.ToString(), true);
        }

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
        }


    }
}
