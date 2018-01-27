using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.PELs
{
    public partial class PELList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            DataTable dtPELs = new DataTable();
            SortedList sParams = new SortedList();

            sParams.Add("@UserID", Master.UserID);
            sParams.Add("@CampaignID", Master.CampaignID);

            dtPELs = Classes.cUtilities.LoadDataTable("uspGetPELsForUser", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELsForUser");

            if (dtPELs.Rows.Count > 0)
            {
                mvPELList.SetActiveView(vwPELList);

                if (dtPELs.Columns["PELStatus"] == null)
                    dtPELs.Columns.Add(new DataColumn("PELStatus", typeof(string)));
                if (dtPELs.Columns["ButtonText"] == null)
                    dtPELs.Columns.Add(new DataColumn("ButtonText", typeof(string)));
                if (dtPELs.Columns["DisplayAddendum"] == null)
                    dtPELs.Columns.Add(new DataColumn("DisplayAddendum", typeof(Boolean)));

                foreach (DataRow dRow in dtPELs.Rows)
                {
                    dRow["DisplayAddendum"] = false;
                    if (dRow["DateApproved"] != System.DBNull.Value)
                    {
                        dRow["PELStatus"] = "Approved";
                        dRow["ButtonText"] = "View";
                        dRow["DisplayAddendum"] = true;
                    }
                    else if (dRow["DateSubmitted"] != System.DBNull.Value)
                    {
                        dRow["PELStatus"] = "Submitted";
                        dRow["ButtonText"] = "View";
                        dRow["DisplayAddendum"] = true;
                    }
                    else if (dRow["DateStarted"] != System.DBNull.Value)
                    {
                        dRow["PELStatus"] = "Started";
                        dRow["ButtonText"] = "Edit";
                        dRow["DisplayAddendum"] = false;
                    }
                    else
                    {
                        dRow["PELStatus"] = "";
                        dRow["ButtonText"] = "Create";
                        dRow["DisplayAddendum"] = false;
                        int iPELID;
                        if (int.TryParse(dRow["RegistrationID"].ToString(), out iPELID))
                            dRow["PELID"] = iPELID * -1;
                    }
                }

                DataView dvPELs = new DataView(dtPELs, "", "ActualArrivalDate desc, ActualArrivalTime desc, RegistrationID desc", DataViewRowState.CurrentRows);
                gvPELList.DataSource = dvPELs;
                gvPELList.DataBind();
            }
            else
            {
                mvPELList.SetActiveView(vwNoPELs);
                lblCampaignName.Text = Master.CampaignName;
            }

            if (Session["UpdatePELMessage"] != null)
            {
                string jsString = Session["UpdatePELMessage"].ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", jsString, true);
                Session.Remove("UpdatePELMessage");
            }
        }

        protected void gvPELList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sRegistrationID = e.CommandArgument.ToString();
            if (e.CommandName.ToUpper() == "ADDENDUM")
                Response.Redirect("PELAddAddendum.aspx?RegistrationID=" + sRegistrationID, true);
            else
                Response.Redirect("PELEdit.aspx?RegistrationID=" + sRegistrationID, true);
        }

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
        }

        //protected void InsertCPOpportunity(int RoleAlignment, int iCharacterID, int iEventID, int iRegistrationID)
        //{
        //    int iCampaignID = 0;
        //    int iUserID = 0;
        //    if (Session["UserID"] != null)
        //        int.TryParse(Session["UserID"].ToString(), out iUserID);
        //    if (Session["CampaignID"] != null)
        //        int.TryParse(Session["CampaignID"].ToString(), out iCampaignID);

        //    int iReasonID = 0;
        //    switch (ddlRoles.SelectedItem.Text)
        //    {
        //        case "PC":
        //            iReasonID = 3;
        //            break;
        //        case "NPC":
        //            iReasonID = 1;
        //            break;
        //        case "Staff":
        //            RoleAlignment = 12;
        //            break;
        //        default:
        //            break;
        //    }

        //    Classes.cPoints cPoints = new Classes.cPoints();

        //    // Currently have no need to delete. The Opportunty should be there currently. Leaving in case we need it.
        //    // cPoints.DeleteRegistrationCPOpportunity(iUserID, iRegistrationID);

        //    cPoints.CreateRegistrationCPOpportunity(iUserID, iCampaignID, RoleAlignment, iCharacterID, iReasonID, iEventID, iRegistrationID);
        //}

        protected void btnMissedEvent_Click(object sender, EventArgs e)
        {
            Response.Redirect("MissingEvent.aspx", true);
        }
    }
}