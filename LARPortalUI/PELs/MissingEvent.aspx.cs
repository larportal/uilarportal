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
    public partial class MissingEvent : System.Web.UI.Page
    {
        //private //string _UserName = "";
        //private int _UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataTable dtPELs = new DataTable();
            SortedList sParams = new SortedList();

            tbSendToCPOther.Attributes.Add("PlaceHolder", "Enter any comments for staff.");

            if (Session["CampaignID"] == null)
                return;

            ddlRoles.Attributes.Add("OnChange", "DisplayRoles(this);");
            sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@UserID", Master.UserID);
            DataSet dsEventInfo = Classes.cUtilities.LoadDataSet("uspGetMissedEvents", sParams, "LARPortal", Master.UserName, "PELList.GetMissedRegistrations");

            dsEventInfo.Tables[0].TableName = "Events";
            dsEventInfo.Tables[1].TableName = "CharactersForUser";
            dsEventInfo.Tables[2].TableName = "Roles";
            dsEventInfo.Tables[3].TableName = "Campaigns";
            dsEventInfo.Tables[4].TableName = "AllCharactersForUser";

            if (dsEventInfo.Tables["Events"].Rows.Count == 0)
            {
                divEventList.Visible = false;
                divNoEvents.Visible = true;
                btnRegisterForEvent.Visible = false;
                return;
            }

            divEventList.Visible = true;
            divNoEvents.Visible = false;
            btnRegisterForEvent.Visible = true;

            ddlMissedEvents.DataTextField = "EventName";
            ddlMissedEvents.DataValueField = "EventID";
            ddlMissedEvents.DataSource = dsEventInfo.Tables["Events"];
            ddlMissedEvents.DataBind();

            if (dsEventInfo.Tables["AllCharactersForUser"] != null)
            {
                if (dsEventInfo.Tables["AllCharactersForUser"].Rows.Count == 1)
                {
                    ddlCharacterList.Visible = false;
                    lblCharacter.Text = dsEventInfo.Tables["AllCharactersForUser"].Rows[0]["CharacterAKA"].ToString();
                    lblCharacter.Visible = true;
                    hidSkillSetID.Value = dsEventInfo.Tables["AllCharactersForUser"].Rows[0]["CharacterSkillSetID"].ToString();
                }
                else
                {
                    hidSkillSetID.Value = "";
                    ddlCharacterList.Visible = true;
                    ddlCharacterList.ClearSelection();
                    ddlCharacterList.DataTextField = "CharacterAKA";
                    ddlCharacterList.DataValueField = "CharacterID";
                    ddlCharacterList.DataSource = dsEventInfo.Tables["AllCharactersForUser"];
                    ddlCharacterList.DataBind();
                    lblCharacter.Visible = false;
                    if (ddlCharacterList.Items.Count > 0)
                    {
                        ddlCharacterList.ClearSelection();
                        ddlCharacterList.Items[0].Selected = true;
                    }
                }
                if (dsEventInfo.Tables["Roles"] != null)
                {
                    if (dsEventInfo.Tables["Roles"].Rows.Count == 1)
                    {
                        ddlRoles.Visible = false;
                        lblRole.Text = dsEventInfo.Tables["Roles"].Rows[0]["Description"].ToString();
                        lblRole.Visible = true;
                        if ((lblRole.Text.ToUpper() == "PC") || (lblRole.Text.ToUpper() == "STAFF"))
                        {
                            divPCStaff.Visible = true;
                            divNPC.Visible = false;
                            divSendOther.Visible = false;
                        }
                        else
                        {
                            divPCStaff.Visible = false;
                            divNPC.Visible = true;
                            divSendOther.Visible = true;
                        }
                    }
                    else
                    {
                        ddlRoles.Visible = true;
                        ddlRoles.DataTextField = "Description";
                        ddlRoles.DataValueField = "RoleAlignmentID";
                        ddlRoles.DataSource = dsEventInfo.Tables["Roles"];
                        ddlRoles.DataBind();
                        lblRole.Visible = false;
                        if (ddlRoles.Items.Count > 0)
                        {
                            ddlRoles.ClearSelection();
                            ddlRoles.Items[0].Selected = true;

                            if ((ddlRoles.SelectedItem.Text.ToUpper() == "PC") || (ddlRoles.SelectedItem.Text.ToUpper() == "STAFF"))
                            {
                                divPCStaff.Style.Add("display", "block");
                                divNPC.Style.Add("display", "none");
                                divSendOther.Style.Add("display", "none");
                            }
                            else
                            {
                                divPCStaff.Style.Add("display", "none");
                                divNPC.Style.Add("display", "block");
                                divSendOther.Style.Add("display", "block");
                            }
                        }
                    }

                    if (dsEventInfo.Tables["Campaigns"] != null)
                    {
                        ddlSendToCampaign.DataTextField = "CampaignName";
                        ddlSendToCampaign.DataValueField = "CampaignID";
                        ddlSendToCampaign.DataSource = dsEventInfo.Tables["Campaigns"];
                        ddlSendToCampaign.DataBind();
                    }
                }
                ddlMissedEvents_SelectedIndexChanged(null, null);
            }
        }

        //protected void gvPELList_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string sRegistrationID = e.CommandArgument.ToString();
        //    if (e.CommandName.ToUpper() == "ADDENDUM")
        //        Response.Redirect("PELAddAddendum.aspx?RegistrationID=" + sRegistrationID, true);
        //    else
        //        Response.Redirect("PELEdit.aspx?RegistrationID=" + sRegistrationID, true);
        //}

        protected void ddlMissedEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortedList sParams = new SortedList();
            sParams.Add("@EventID", ddlMissedEvents.SelectedValue);
            sParams.Add("@UserID", Master.UserID);
            DataSet dsEventInfo = Classes.cUtilities.LoadDataSet("uspGetEventInfo", sParams, "LARPortal", Master.UserName, "EventRegistration.gvEvents_RowCommand");

            dsEventInfo.Tables[0].TableName = "EventInfo";
            dsEventInfo.Tables[1].TableName = "Housing";
            dsEventInfo.Tables[2].TableName = "PaymentType";

            if (dsEventInfo.Tables.Count >= 5)
            {
                dsEventInfo.Tables[3].TableName = "Character";
                dsEventInfo.Tables[4].TableName = "Teams";
                dsEventInfo.Tables[5].TableName = "Registration";
                dsEventInfo.Tables[6].TableName = "RolesForEvent";
                dsEventInfo.Tables[7].TableName = "RegistrationStatuses";
                dsEventInfo.Tables[8].TableName = "Meals";
                dsEventInfo.Tables[9].TableName = "PlayerInfo";
                dsEventInfo.Tables[10].TableName = "CampaignPELs";
                dsEventInfo.Tables[11].TableName = "EventPELs";
                dsEventInfo.Tables[12].TableName = "AllCharactersForUser";
            }

            foreach (DataRow dRow in dsEventInfo.Tables["EventInfo"].Rows)
            {
                DateTime dtStartDate;
                DateTime dtEndDate;

                if ((DateTime.TryParse(dRow["StartDate"].ToString(), out dtStartDate)) &&
                    (DateTime.TryParse(dRow["EndDate"].ToString(), out dtEndDate)))
                {
                    ViewState["EventStartDate"] = dtStartDate.ToShortDateString();
                    ViewState["EventEndDate"] = dtEndDate.ToShortDateString();
                    ViewState["EventStartTime"] = dRow["StartTime"].ToString();
                    ViewState["EventEndTime"] = dRow["EndTime"].ToString();
                }
                else
                {
                    ViewState.Remove("EventStartDate");
                    ViewState.Remove("EventEndDate");
                    ViewState.Remove("EventStartTime");
                    ViewState.Remove("EventEndTime");
                }
            }

            if (dsEventInfo.Tables["AllCharactersForUser"].Rows.Count > 0)
            //            if (dsEventInfo.Tables["Character"].Rows.Count > 0)
            {
                ddlCharacterList.DataSource = dsEventInfo.Tables["AllCharactersForUser"];            //["Character"];
                ddlCharacterList.DataTextField = "CharacterAKA";
                ddlCharacterList.DataValueField = "CharacterID";
                ddlCharacterList.DataBind();
                ddlCharacterList.SelectedIndex = 0;
                ddlCharacterList.Visible = true;
                lblCharacter.Visible = false;
                hidSkillSetID.Value = "";
            }

            if (dsEventInfo.Tables["AllCharactersForUser"].Rows.Count == 1)
            //            if (dsEventInfo.Tables["Character"].Rows.Count == 1)
            {
                ddlCharacterList.Visible = false;
                lblCharacter.Visible = true;
                lblCharacter.Text = ddlCharacterList.Items[0].Text;
                hidSkillSetID.Value = dsEventInfo.Tables["AllCharactersForUser"].Rows[0]["CharacterSkillSetID"].ToString();
            }

            DataView dvJustRoleNames = new DataView(dsEventInfo.Tables["RolesForEvent"], "", "", DataViewRowState.CurrentRows);
            DataTable dtJustRoleNames = dvJustRoleNames.ToTable(true, "RoleAlignmentID", "Description");

            ddlRoles.DataSource = dtJustRoleNames;
            ddlRoles.DataTextField = "Description";
            ddlRoles.DataValueField = "RoleAlignmentID";
            ddlRoles.DataBind();

            if (dtJustRoleNames.Rows.Count == 1)
            {
                ddlRoles.Visible = false;
                lblRole.Text = ddlRoles.Items[0].Text;
                lblRole.Visible = true;
            }
            else
            {
                if (dtJustRoleNames.Rows.Count > 1)
                {
                    lblRole.Visible = false;
                    ddlRoles.Visible = true;
                    ddlRoles.Items[0].Selected = true;
                }
            }

            ddlSendToCampaign.ClearSelection();

            foreach (DataRow dCharInfo in dsEventInfo.Tables["AllCharactersForUser"].Rows)
            {
                lblCharacter.Text = dCharInfo["CharacterAKA"].ToString().Trim();
            }

            if (IsPostBack)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openRegistration();", true);
        }

        protected void btnRegisterForEvent_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            int iRegStatus = -1;

            string sSQL = "select StatusID, StatusName from MDBStatus " +
                "where StatusType like 'Registration' " +
                    "and StatusName = 'Added Post Event'" +
                    "and DateDeleted is null";
            DataTable dtRegStatus = Classes.cUtilities.LoadDataTable(sSQL, sParam, "LARPortal", Master.UserName, lsRoutineName + ".select for statuses",
                Classes.cUtilities.LoadDataTableCommandType.Text);

            if (dtRegStatus.Rows.Count > 0)
                int.TryParse(dtRegStatus.Rows[0]["StatusID"].ToString(), out iRegStatus);

            sParam = new SortedList();

            int iRegistrationID = -1;
            int iRoleAlignment = 0;
            int iEventID = 0;
            int.TryParse(ddlRoles.SelectedValue, out iRoleAlignment);
            int.TryParse(ddlMissedEvents.SelectedValue, out iEventID);

            sParam.Add("@RegistrationID", iRegistrationID);
            sParam.Add("@UserID", Master.UserID);
            sParam.Add("@EventID", iEventID);
            sParam.Add("@RoleAlignmentID", ddlRoles.SelectedValue);

            //  JB  10/2/2021  Fixed for when there is one character - need to pull from hidden skill set id.
            int iCharacterID = 0;
            if ((ddlCharacterList.Visible) &&
                (hidSkillSetID.Value.Length > 0))
                int.TryParse(hidSkillSetID.Value, out iCharacterID);
            else
                int.TryParse(ddlCharacterList.SelectedValue, out iCharacterID);

            sParam.Add("@CharacterID", iCharacterID);

            sParam.Add("@DateRegistered", DateTime.Now);
            if (ddlRoles.SelectedItem.Text != "PC")
                sParam.Add("@NPCCampaignID", ddlSendToCampaign.SelectedValue);

            sParam.Add("@RegistrationStatus", iRegStatus);
            sParam.Add("@PartialEvent", false);

            if ((ViewState["EventStartDate"] != null) &&
                (ViewState["EventStartTime"] != null) &&
                (ViewState["EventEndDate"] != null) &&
                (ViewState["EventEndTime"] != null))
            {
                sParam.Add("@ExpectedArrivalDate", ViewState["EventStartDate"].ToString());
                sParam.Add("@ExpectedArrivalTime", ViewState["EventStartTime"].ToString());
                sParam.Add("@ExpectedDepartureDate", ViewState["EventEndDate"].ToString());
                sParam.Add("@ExpectedDepartureTime", ViewState["EventEndTime"].ToString());

                sParam.Add("@ActualArrivalDate", ViewState["EventStartDate"].ToString());
                sParam.Add("@ActualArrivalTime", ViewState["EventStartTime"].ToString());
                sParam.Add("@ActualDepartureDate", ViewState["EventEndDate"].ToString());
                sParam.Add("@ActualDepartureTime", ViewState["EventEndTime"].ToString());
            }

            try
            {
                DataTable dtUser = Classes.cUtilities.LoadDataTable("uspRegisterForEvent", sParam, "LARPortal", Master.UserName, lsRoutineName);

                foreach (DataRow dRegRecord in dtUser.Rows)
                {
                    iRegistrationID = 0;
                    int.TryParse(dRegRecord["RegistrationID"].ToString(), out iRegistrationID);
                    InsertCPOpportunity(iRoleAlignment, iCharacterID, iEventID, iRegistrationID, "MissingEvents");
                }

                Session["UpdatePELMessage"] = "alert('You have been registered for the event.');";
                Response.Redirect("PELList.aspx", true);
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                lblMessage.Text = "There was a problem registering for the event. LARPortal support has been notified.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
            }
        }

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
        }

        protected void InsertCPOpportunity(int RoleAlignment, int iCharacterID, int iEventID, int iRegistrationID, string PointsFrom)
        {
            int iReasonID = 0;
            switch (ddlRoles.SelectedItem.Text)
            {
                case "PC":
                    iReasonID = 3;
                    break;
                case "NPC":
                    iReasonID = 1;
                    break;
                case "Staff":
                    RoleAlignment = 12;
                    break;
                default:
                    break;
            }

            Classes.cPoints cPoints = new Classes.cPoints();
            cPoints.CreateRegistrationCPOpportunity(Master.UserID, Master.CampaignID, RoleAlignment, iCharacterID, iReasonID, iEventID, iRegistrationID, PointsFrom);
        }

        //protected void oCampSelect_CampaignChanged(object sender, EventArgs e)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

        //    SortedList sParams = new SortedList();
        //    sParams.Add("@CampaignID", Master.CampaignID);
        //    DataSet dtEventInfo = Classes.cUtilities.LoadDataSet("uspGetEventInfo", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetEventInfo");

        //    dtEventInfo.Tables[0].TableName = "EventInfo";
        //    dtEventInfo.Tables[1].TableName = "Housing";
        //    dtEventInfo.Tables[2].TableName = "PaymentType";

        //    // Eventually the row filter needs to be     StatusName = 'Scheduled' and RegistrationOpenDateTime <= GetDate() and RegistrationCloseDateTime >= GetDate()
        //    DataView dvEventInfo = new DataView(dtEventInfo.Tables["EventInfo"], "PELDeadlineDate > '" + System.DateTime.Today.ToShortDateString() + "'",    // "StatusName = 'Scheduled'",    // and RegistrationOpenDateTime > '" + System.DateTime.Today + "'",
        //        "", DataViewRowState.CurrentRows);

        //    DataTable dtEventDates = dvEventInfo.ToTable(true, "StartDate", "EventID", "EventName");

        //    // Could do this as a computed column - but I want to specify the format.
        //    dtEventDates.Columns.Add("DisplayStartDate", typeof(string));
        //    DateTime dtTemp;

        //    foreach (DataRow dRow in dtEventDates.Rows)
        //        if (DateTime.TryParse(dRow["StartDate"].ToString(), out dtTemp))
        //            dRow["DisplayStartDate"] = dtTemp.ToString("MM/dd/yyyy") + " - " + dRow["EventName"].ToString();

        //    DataView dvEventDate = new DataView(dtEventDates, "", "StartDate", DataViewRowState.CurrentRows);

        //    ddlMissedEvents.DataSource = dvEventDate;
        //    ddlMissedEvents.DataTextField = "DisplayStartDate";
        //    ddlMissedEvents.DataValueField = "EventID";
        //    ddlMissedEvents.DataBind();

        //    ddlMissedEvents_SelectedIndexChanged(null, null);
        //}
    }
}
