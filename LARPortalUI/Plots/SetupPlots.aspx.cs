using LarpPortal.Classes;
using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Plots
{
    public partial class SetupPlots : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["cbOpenOnly"] != null)
                {
                    cbDisplayOnlyOpenPlots.Checked = (bool)Session["cbOpenOnly"];
                }
            }
            btnClose.Attributes.Add("data-dismiss", "modal");
            lblmodalMessage.Text = "";

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);

            // Need to incorporate a parameter for cbDisplayOnlyOpenEvents
            if (cbDisplayOnlyOpenPlots.Checked)
            {
                sParams.Add("@OpenOnly", 1);
            }
            else
            {
                sParams.Add("@OpenOnly", 0);
            }

            DataTable dtPlots = Classes.cUtilities.LoadDataTable("uspGetPlotsForCampaign", sParams, "LARPortal", Master.UserName, lsRoutineName);

            if (dtPlots.Rows.Count > 0)
            {
                mvPlotList.SetActiveView(vwPlotList);

                DataView dvPlots = new DataView(dtPlots, "", "PlotCode asc", DataViewRowState.CurrentRows);
                gvPlotList.DataSource = dvPlots;
                gvPlotList.DataBind();
            }
            else
            {
                mvPlotList.SetActiveView(vwNoPlots);
                lblCampaignName.Text = Master.CampaignName; // Session["CampaignName"].ToString();
            }

            if (Session["UpdateEventMessage"] != null)
            {
                lblmodalMessage.Text = Session["UpdateEventMessage"].ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);

                Session.Remove("UpdateEventMessage");
            }
        }

        protected void gvPlotList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            string sPlotID = e.CommandArgument.ToString();
            Int32 CopyFromEventID = Int32.Parse(sPlotID);
            Int32 CopyToEventID = 0;
            string sCommandName = e.CommandName.ToUpper();

            GridViewRow currentRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

            switch (sCommandName)
            {
                case "EDIT":
                    Response.Redirect("PlotAdd.aspx?PlotID=" + sPlotID, true);
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
                        //CloneEventDonations(CopyFromEventID, CopyToEventID);
                        //Response.Redirect("PlotAdd.aspx?PlotID=" + CopyToPlotID.ToString(), true);
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
            slParameters.Add("@PlotID", EventID);
            slParameters.Add("@UserID", Master.UserID);
            cUtilities.PerformNonQuery("uspCancelDonationList", slParameters, "LARPortal", Master.UserName);

            Page.Response.Redirect(Page.Request.Url.ToString(), true);

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlotList.aspx", true);
        }

        protected void gvPlotList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlPlotsNoDonations = (e.Row.FindControl("ddlPlotsNoDonations") as DropDownList);
                string stCallingMethod = "SetupPlots.gvPlotList_RowDataBound";
                string stStoredProc = "uspGetPlotsForCampaign";

                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
                SortedList sParams = new SortedList();
                sParams.Add("@PlotID", Master.CampaignID);
                sParams.Add("@OpenOnly", Session["cbOpenOnly"]);
                DataTable dtTableName = new DataTable();
                DataTable dt = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, lsRoutineName + "." + stStoredProc);
                //ddlEventsNoDonations.DataSource = dt;
                //ddlEventsNoDonations.DataTextField = "EventDate";
                //ddlEventsNoDonations.DataValueField = "EventID";
                //ddlEventsNoDonations.DataBind();
                //if (dt.Rows.Count == 0)
                //{
                //    ddlEventsNoDonations.Items.Insert(0, new ListItem("No events available to clone to"));
                //}
                //else
                //{
                //    ddlEventsNoDonations.Items.Insert(0, new ListItem("Select event to clone to"));
                //}

                DataRowView dRow = (DataRowView)e.Row.DataItem;
                string sEventStatus = dRow["EventStatus"].ToString().ToUpper();

            }
        }

        protected void cbDisplayOnlyOpenPlots_CheckedChanged(object sender, EventArgs e)
        {
            Session["cbOpenOnly"] = cbDisplayOnlyOpenPlots.Checked;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);

        }



    }
}
