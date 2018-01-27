using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Events
{
    public partial class SetupEvent : System.Web.UI.Page
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

            DataTable dtEvents = Classes.cUtilities.LoadDataTable("uspGetEventsForCampaign", sParams, "LARPortal", Master.UserName, lsRoutineName);

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
            string sCommandName = e.CommandName.ToUpper();

            switch (sCommandName)
            {
                case "EDIT":
                    Response.Redirect("EventEdit.aspx?EventID=" + sEventID, true);
                    break;

                case "CANCELLED":
                case "COMPLETED":
                    SortedList sGetStatusParams = new SortedList();
                    sGetStatusParams.Add("@StatusType", "Event");

                    DataTable dtStatus = Classes.cUtilities.LoadDataTable("uspGetStatus", sGetStatusParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetStatus");
                    dtStatus.CaseSensitive = false;
                    DataView dvStatus = new DataView(dtStatus, "StatusName = '" + sCommandName + "'", "", DataViewRowState.CurrentRows);
                    if (dvStatus.Count > 0)
                    {
                        SortedList sUpdateEvent = new SortedList();
                        sUpdateEvent.Add("@UserID", Master.UserID);
                        sUpdateEvent.Add("@StatusID", dvStatus[0]["StatusID"].ToString());
                        sUpdateEvent.Add("@EventID", sEventID);         // Added JLB 6/7/2016
                        Classes.cUtilities.PerformNonQuery("uspInsUpdCMEvents", sUpdateEvent, "LARPortal", Master.UserName);
                    }
                    break;

                case "DELETEEVENT":         // J.Bradshaw  Request # 1290    Was Delete which defaulted to the row delete command which isn't defined.
                    SortedList sDeleteParms = new SortedList();
                    sDeleteParms.Add("@RecordID", sEventID);
                    sDeleteParms.Add("@UserID", Master.UserID);
                    Classes.cUtilities.PerformNonQuery("uspDelCMEvents", sDeleteParms, "LARPortal", Master.UserName);
                    break;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("EventEdit.aspx?EventID=-1", true);
        }

        protected void gvEventList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dRow = (DataRowView)e.Row.DataItem;
                string sEventStatus = dRow["EventStatus"].ToString().ToUpper();

                if ((sEventStatus == "COMPLETED") ||
                     (sEventStatus == "CANCELLED"))
                {
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    if (btnEdit != null)
                        btnEdit.Visible = false;
                    Button btnCancel = (Button)e.Row.FindControl("btnCancel");
                    if (btnCancel != null)
                        btnCancel.Visible = false;
                    Button btnComplete = (Button)e.Row.FindControl("btnComplete");
                    if (btnComplete != null)
                        btnComplete.Visible = false;
                }
            }
        }
    }
}
