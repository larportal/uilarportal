using LarpPortal.Classes;
using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Donations
{
    public partial class SetupDonations : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            btnClose.Attributes.Add("data-dismiss", "modal");
            lblmodalMessage.Text = "";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);

            DataTable dtEvents = Classes.cUtilities.LoadDataTable("uspGetEventsDonationListsForCampaign", sParams, "LARPortal", Master.UserName, lsRoutineName);

            if (dtEvents.Rows.Count > 0)
            {
                mvEventList.SetActiveView(vwEventList);

                DataView dvEvents = new DataView(dtEvents, "", "StartDateTime desc", DataViewRowState.CurrentRows);
                gvEventList.DataSource = dvEvents;
                gvEventList.DataBind();
            }
            else
            {
                mvEventList.SetActiveView(vwNoEvents);
                lblCampaignName.Text = Master.CampaignName; // Session["CampaignName"].ToString();
            }

            if (Session["UpdateEventMessage"] != null)
            {
                lblmodalMessage.Text = Session["UpdateEventMessage"].ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);

                Session.Remove("UpdateEventMessage");
            }
        }

        protected void gvEventList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            string sEventID = e.CommandArgument.ToString();
            Int32 CopyFromEventID = Int32.Parse(sEventID);
            Int32 CopyToEventID = 0;
            string sCommandName = e.CommandName.ToUpper();

            GridViewRow currentRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

            switch (sCommandName)
            {
                case "EDIT":
                    Response.Redirect("DonationEditList.aspx.aspx?EventID=" + sEventID, true);
                    break;

                case "CANCELLED":
                    // Confirm box "Are you sure you want to do something stupid and irreversible like cancelling the open balance of all donation items for this event?"
                    // Stored proc to cancel all open, unclaimed donations. If a donation is partially claimed lower the qty needed to match the claimed quantity.
                    CancelEventDonations(CopyFromEventID);
                    break;

                case "CLONE":
                    // Show drop down list of events with no donations and copy selected row's event's donations to the event selected from the ddl (ddlEventsNoDonations)
                    DropDownList ddlEventsNoDonations = (DropDownList)currentRow.FindControl("ddlEventsNoDonations") as DropDownList;
                    CopyToEventID = Int32.Parse(ddlEventsNoDonations.SelectedValue);
                    if (CopyToEventID == 0)
                    {
                        //  ****  NEED ERROR MESSAGE ABOUT SELECTING AN EVENT BEFORE CLONING *****

                    }
                    else
                    if (CopyToEventID == -1)
                    {
                        //  ****  NEED ERROR MESSAGE ABOUT NO EVENTS AVAILABLE TO CLONE TO *****
                    }
                    else
                    {
                        CloneEventDonations(CopyFromEventID, CopyToEventID);
                        Response.Redirect("DonationEditList.aspx?EventID=" + CopyToEventID.ToString(), true);
                    }

                    break;
            }
        }

        protected void CancelEventDonations(Int32 EventID)
        {

            // Call stored procedure uspCancelDonationList (EventID) then refresh current page
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList slParameters = new SortedList();
            slParameters.Add("@EventID", EventID);
            slParameters.Add("@UserID", Master.UserID);
            cUtilities.PerformNonQuery("uspCancelDonationList", slParameters, "LARPortal", Master.UserName);

            Page.Response.Redirect(Page.Request.Url.ToString(), true);

        }

        protected void CloneEventDonations(Int32 CopyFromEventID, Int32 CopyToEventID)
        {

            // Call stored procedure uspCloneDonationList (FromEvent, ToEvent), then redirect to Edit page for "To Event"
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList slParameters = new SortedList();
            slParameters.Add("@FromEventID", CopyFromEventID);
            slParameters.Add("@ToEventID", CopyToEventID);
            cUtilities.PerformNonQuery("uspCloneDonationList", slParameters, "LARPortal", Master.UserName);

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("DonationEventList.aspx", true);
        }

        protected void gvEventList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlEventsNoDonations = (e.Row.FindControl("ddlEventsNoDonations") as DropDownList);
                string stCallingMethod = "SetupDonations.gvEventList_RowDataBound";
                string stStoredProc = "uspGetEventsWODonationListsForCampaign";

                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
                SortedList sParams = new SortedList();
                sParams.Add("@CampaignID", Master.CampaignID);
                DataTable dtTableName = new DataTable();
                DataTable dt = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, lsRoutineName + "." + stStoredProc);
                ddlEventsNoDonations.DataSource = dt;
                ddlEventsNoDonations.DataTextField = "EventDate";
                ddlEventsNoDonations.DataValueField = "EventID";
                ddlEventsNoDonations.DataBind();
                if (dt.Rows.Count == 0)
                {
                    ddlEventsNoDonations.Items.Insert(-1, new ListItem("No events available to clone to"));
                }
                else
                {
                    ddlEventsNoDonations.Items.Insert(0, new ListItem("Select event to clone to"));
                }



                DataRowView dRow = (DataRowView)e.Row.DataItem;
                string sEventStatus = dRow["EventStatus"].ToString().ToUpper();

            }
        }

    }
}
