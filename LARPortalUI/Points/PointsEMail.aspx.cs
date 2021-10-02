using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LarpPortal.Classes;

namespace LarpPortal.Points
{
    public partial class PointsEMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtAdditionalPlayers.Attributes.Add("Placeholder", "e.g. Name:John Doe Event: Event 7 Points: 1.5: Name:Jane Smith Event: May 23, 2017-One Day Points:0.5:");
                int CurrentCampaignID = Convert.ToInt32(Session["CampaignID"]);
                // ********** When we send, calling job is going to be PointsEmail which is entry in MDBSMTP table **********
                ddlCampaignLoad(CurrentCampaignID);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Session["ActiveLeftNav"] = "Points";
            if (!IsPostBack)
            {
                Session["EditMode"] = "Assign";
                int iTemp = 0;
                int intCampaignID = 0;
                if (Session["UserName"] != null)
                    hidUserName.Value = Session["UserFullName"].ToString();
                if (int.TryParse(Session["CampaignID"].ToString(), out iTemp))
                {
                    hidCampaignID.Value = iTemp.ToString();
                    intCampaignID = iTemp;
                }
                if (Session["CampaignName"] != null)
                {
                    hidCampaignName.Value = Session["CampaignName"].ToString();
                }
                if (Session["MemberEmailAddress"] != null)
                {
                    hidBcc.Value = Session["MemberEmailAddress"].ToString();
                }
                FillGrid(hidUserName.Value, hidCampaignID.Value);
            }
        }

        // ************************ Populate Send-To Dropdown list, Main Points grid, Edit Email button
        protected void ddlCampaignLoad(int CurrentCampaignID)
        {
            string stStoredProc = "uspGetCampaignsToSendPointsTo";
            string stCallingMethod = "PointsEmail.aspx.ddlCampaignLoad";
            DataTable dtDestinationCampaigns = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@FromCampaignID", CurrentCampaignID);
            dtDestinationCampaigns = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", hidUserName.Value, stCallingMethod);
            ddlCampaign.DataTextField = "CampaignName";
            ddlCampaign.DataValueField = "CampaignToID";
            ddlCampaign.DataSource = dtDestinationCampaigns;
            ddlCampaign.DataBind();
            ddlCampaign.Items.Insert(0, new ListItem("Select Campaign", "0"));
            ddlCampaign.SelectedIndex = 0;
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            pnlAwaitingSending.Visible = false;
            pnlAddMissingRegistration.Visible = false;
            pnlPreviewEmail.Visible = true;
            btnPreview.Visible = false;
            btnPreviewUpdate.Visible = true;
            btnSendEmail.Visible = true;
            btnCancel.Visible = true;
            ddlCampaign.Enabled = false;
            // Build the email preview in lblEmail
            BuildPreview();
        }

        protected void ddlCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCampaign.SelectedIndex == 0)
            {
                btnPreview.Visible = false;
                pnlAddMissingRegistration.Visible = false;
            }
            else
            {
                hidRecevingCampaignID.Value = ddlCampaign.SelectedValue;
                btnPreview.Visible = true;
                ddlPlayerLoad();
                int CampaignID = 0;
                Int32.TryParse(hidCampaignID.Value, out CampaignID);
                ddlEventLoad(CampaignID);
                ddlDescriptionLoad(CampaignID);
                pnlAddMissingRegistration.Visible = true;
                lblpnlAddMissingREgistrationHeader.Text = " to send points for " + ddlCampaign.SelectedItem;
            }
            FillGrid(hidUserName.Value, hidCampaignID.Value);
            string CampaignLPType;
            if (ddlCampaign.SelectedValue == hidCampaignID.Value)
            {
                CampaignLPType = "S";
            }
            else
            {
                int iTemp = 0;
                if (iTemp != 0)
                {
                    Classes.cCampaignBase cCampaign = new Classes.cCampaignBase(iTemp, hidUserName.Value, 0);
                    CampaignLPType = cCampaign.PortalAccessType;
                    hidTo.Value = cCampaign.CPNotificationEmail;
                }
                else
                {
                    CampaignLPType = "B";
                }
                // Get values to put in email
                //hidTo.Value = "";
                //hidSubject.Value = "";
                //hidBcc.Value = This gets set on page load since it will never change.
            }
            //hidInsertDestinationCampaignLPType.Value = CampaignLPType;
        }

        private void FillGrid(string strUserName, string strCampaignID)
        {
            int iTemp = 0;
            int intFromCampaignID = 0;
            int intToCampaignID = 0;
            if (int.TryParse(strCampaignID, out iTemp))
                intFromCampaignID = iTemp;
            if (int.TryParse(ddlCampaign.SelectedValue.ToString(), out iTemp))
                intToCampaignID = iTemp;
            string stStoredProc = "uspGetCampaignPointOpportunitiesToSendOut";
            string stCallingMethod = "PointsEmail.aspx.FillGrid";
            DataTable dtOpportunities = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@FromCampaignID", intFromCampaignID);
            sParams.Add("@ToCampaignID", intToCampaignID);
            dtOpportunities = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", strUserName, stCallingMethod);
            gvPoints.DataSource = dtOpportunities;
            gvPoints.DataBind();
        }

        protected void gvPoints_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPoints.EditIndex = -1;
            FillGrid(hidUserName.Value, hidCampaignID.Value);
        }

        protected void gvPoints_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DataRowView dRow = e.Row.DataItem as DataRowView;
                }
            }
        }

        protected void gvPoints_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Session["EditMode"] = "Edit";
            gvPoints.EditIndex = e.NewEditIndex;
            FillGrid(hidUserName.Value, hidCampaignID.Value);
        }

        protected void gvPoints_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int index = gvPoints.EditIndex;
                int iTemp;
                int UserID = 0;
                double dblTemp = 0;
                double CP = 0;
                if (int.TryParse(Session["UserID"].ToString(), out iTemp))
                    UserID = iTemp;
                HiddenField hidCPOpp = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidPointID");
                HiddenField hidCmpPlyrID = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidCampaignPlayer");
                HiddenField hidCharID = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidCharacterID");
                HiddenField hidEvntID = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidEventID");
                HiddenField hidOppDefID = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidCPOpportunityDefaultID");
                HiddenField hidRsnID = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidReasonID");
                HiddenField hidAddID = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidAddedByID");
                HiddenField hidOppNotes = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidOpportunityNotes");
                HiddenField hidExURL = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidExampleURL");
                HiddenField hidRole = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidRole");
                HiddenField hidNPCCampaignID = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidNPCCampaignID");
                HiddenField hidRegistrationID = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidRegistrationID");
                Label lblEarnDesc = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblEarnDescription");
                int intCmpPlyrID = 0;
                int intCharID = 0;
                int intEvntID = 0;
                int intCPOpp = 0;
                int intOppDefID = 0;
                int intRsnID = 0;
                int intAddID = 0;
                int intRole = 0;
                int intNPCCampaignID = 0;
                int intRegistrationID = 0;
                int intCampaignID = 0;
                int.TryParse(Session["CampaignID"].ToString(), out intCampaignID);
                string strOppNotes = hidOppNotes.Value.ToString();
                string strExURL = hidExURL.Value.ToString();
                string strDesc = lblEarnDesc.Text;
                if (int.TryParse(hidCmpPlyrID.Value.ToString(), out iTemp))
                    intCmpPlyrID = iTemp;
                if (int.TryParse(hidCharID.Value.ToString(), out iTemp))
                    intCharID = iTemp;
                if (int.TryParse(hidEvntID.Value.ToString(), out iTemp))
                    intEvntID = iTemp;
                if (int.TryParse(hidCPOpp.Value.ToString(), out iTemp))
                    intCPOpp = iTemp;
                if (int.TryParse(hidOppDefID.Value.ToString(), out iTemp))
                    intOppDefID = iTemp;
                if (int.TryParse(hidRsnID.Value.ToString(), out iTemp))
                    intRsnID = iTemp;
                if (int.TryParse(hidAddID.Value.ToString(), out iTemp))
                    intAddID = iTemp;
                if (int.TryParse(hidNPCCampaignID.Value.ToString(), out iTemp))
                    intNPCCampaignID = iTemp;
                if (int.TryParse(hidRegistrationID.Value.ToString(), out iTemp))
                    intRegistrationID = iTemp;
                string strComments = "";
                if (Session["EditMode"].ToString() == "Edit")
                {
                    GridViewRow row = gvPoints.Rows[index];
                    TextBox txtComments = row.FindControl("tbStaffComments") as TextBox;
                    strComments = txtComments.Text;
                    TextBox txtCP = row.FindControl("txtCPValue") as TextBox;
                    if (double.TryParse(txtCP.Text.ToString(), out dblTemp))
                        CP = dblTemp;
                    Session["EditMode"] = "Assign";
                }
                else
                {
                    Label lblCPValue = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblCPValue");
                    Label lblStaffComents = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblStaffComments");
                    if (double.TryParse(lblCPValue.Text, out dblTemp))
                        CP = dblTemp;
                    strComments = lblStaffComents.Text;

                }
                Classes.cPoints Point = new Classes.cPoints();
                Point.UpdateCPOpportunity(UserID, intCPOpp, intCmpPlyrID, intCharID, intOppDefID, intEvntID,
                    strDesc, strOppNotes, strExURL, intRsnID, intAddID, CP, UserID,
                    DateTime.Now, UserID, strComments, intRole, intNPCCampaignID, intCampaignID, "PointsEMail.aspx.cs.gvPoints_RowUpdating");
            }
            catch (Exception ex)
            {
                string l = ex.Message;
            }
            gvPoints.EditIndex = -1;
            FillGrid(hidUserName.Value, hidCampaignID.Value);
        }

        protected void gvPoints_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            foreach (DictionaryEntry entry in e.NewValues)
            {
                e.NewValues[entry.Key] = Server.HtmlEncode(entry.Value.ToString());
            }
        }

        protected void gvPoints_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidCPOpp = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidPointID");
            int iTemp = 0;
            int intCPOpp = 0;
            int UserID = 0;
            if (int.TryParse(Session["UserID"].ToString(), out iTemp))
                UserID = iTemp;
            if (int.TryParse(hidCPOpp.Value.ToString(), out iTemp))
                intCPOpp = iTemp;
            Classes.cPoints Points = new Classes.cPoints();
            Points.DeleteCPOpportunity(UserID, intCPOpp);
            gvPoints.EditIndex = -1;
            FillGrid(hidUserName.Value, hidCampaignID.Value);
        }

        // ************************ Populate add player ddlPlayer, ddlEvent, ddlDescription, Next button, NewPoints GridView, Add button
        private void ddlPlayerLoad()
        {
            string stStoredProc = "uspGetAllPlayers";
            string stCallingMethod = "PointsEmail.aspx.ddlPlayerLoad";
            DataTable dtPlayers = new DataTable();
            SortedList sParams = new SortedList();
            dtPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", hidUserName.Value, stCallingMethod);
            ddlRegistrant.DataTextField = "PlayerName";
            ddlRegistrant.DataValueField = "UserID";
            ddlRegistrant.DataSource = dtPlayers;
            ddlRegistrant.DataBind();
            ddlRegistrant.Items.Insert(0, new ListItem("Select Player", "0"));
            ddlRegistrant.SelectedIndex = 0;
        }

        protected void ddlRegistrant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegistrant.SelectedIndex == 0)
            {
                btnPreview.Visible = true;
            }
            else
            {
                btnPreview.Visible = false;
                btnAddNewRegCancel.Visible = true;
            }
        }


        private void GetOrCreateCampaignPlayer()
        {
            // Get or create campaign player for campaign where points where earned - "From" campaign
            string stStoredProc = "uspGetOrInsertCampaignPlayerFromUserID";
            string stCallingMethod = "PointsEmail.aspx.GetOrCreateCampaignPlayer";
            DataTable dtCampaignPlayer = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@UserID", ddlRegistrant.SelectedValue);
            sParams.Add("@CampaignID", hidCampaignID.Value);
            sParams.Add("@CallingProgramComments", "Added by PointsEmail.aspx.cs");
            dtCampaignPlayer = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", hidUserName.Value, stCallingMethod);
            foreach (DataRow drow in dtCampaignPlayer.Rows)
            {
                hidCampaignPlayerID.Value = drow["CampaignPlayerID"].ToString();
            }
        }

        private void ddlEventLoad(int CampaignID)
        {
            string stStoredProc = "uspGetCampaignEvents";
            string stCallingMethod = "PointsEmail.aspx.ddlEventLoad";
            DataTable dtEvents = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", CampaignID);
            sParams.Add("@StatusID", 51);   // 51 Closed events
            dtEvents = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", hidUserName.Value, stCallingMethod);
            ddlEvent.DataTextField = "EventNameDate";
            ddlEvent.DataValueField = "CampaignID";
            ddlEvent.DataSource = dtEvents;
            ddlEvent.DataBind();
            // Let it default to whatever is on top, which should be the most recent event, which is the most likely choice.
            //ddlEvent.Items.Insert(0, new ListItem("Select Event", "0"));
            //ddlEvent.SelectedIndex = 0;
        }

        private void ddlDescriptionLoad(int CampaignID)
        {
            string stStoredProc = "uspGetCampaignComboOpportunities";
            string stCallingMethod = "PointsEmail.aspx.ddlDescriptionLoad";
            DataTable dtCombos = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", CampaignID);
            dtCombos = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", hidUserName.Value, stCallingMethod);
            ddlDescription.DataTextField = "ComboDescription";
            ddlDescription.DataValueField = "CampaignCPOpportunityDefaultComboID";
            ddlDescription.DataSource = dtCombos;
            ddlDescription.DataBind();
            // Let it default to whatever is on top.
            //ddlDescription.Items.Insert(0, new ListItem("Select Description", "0"));
            //ddlDescription.SelectedIndex = 0;
        }

        protected void btnEditNewPoints_Click(object sender, EventArgs e)
        {
            // Add new player "Next" button
            // Check for a selected player
            if (ddlRegistrant.SelectedIndex == 0)
            {
                string jsString = "alert('Select a player first.');";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                        "MyApplication",
                        jsString,
                        true);
            }
            else
            {
                btnPreview.Visible = false;
                btnEditNewPoints.Visible = false;
                btnAddNewReg.Visible = true;
                btnAddNewRegCancel.Visible = true;
                GetOrCreateCampaignPlayer();
                AddNewPointsTentative(ddlDescription.SelectedValue);
                FillGridNewPoints();
                gvNewPoints.Visible = true;
                ddlRegistrant.Enabled = false;
                ddlEvent.Enabled = false;
                ddlDescription.Enabled = false;
                //FillGrid(hidUserName.Value, hidCampaignID.Value);
            }

        }

        protected void btnAddNewRegCancel_Click(object sender, EventArgs e)
        {
            // Undo the addition of the tentative points
            // Delete records from CMCPOpportunity table with status of 80 (tentative) for this CampaignPlayerID (clean up uncommitted records).
            int CampaignPlayerID = 0;
            int UserID = 0;
            int iTemp = 0;
            if (Int32.TryParse(hidCampaignPlayerID.Value, out iTemp))
                CampaignPlayerID = iTemp;
            if (Int32.TryParse(Session["UserID"].ToString(), out iTemp))
                UserID = iTemp;
            string stStoredProc = "uspDelCMCampaignOpportunitiesTentative";
            //string stCallingMethod = "PointsEmail.aspx.AddNewRegCancel";
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignPlayerID", CampaignPlayerID);
            try
            {
                cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", hidUserName.Value);
            }
            catch
            {

            }
            // Reset the player ddl to 0
            ddlRegistrant.SelectedIndex = 0;
            btnPreview.Visible = true;
            gvNewPoints.Visible = false;
            btnAddNewRegCancel.Visible = false;
            btnAddNewReg.Visible = false;
            btnPreview.Visible = true;
            btnEditNewPoints.Visible = true;
            ddlRegistrant.Enabled = true;
            ddlEvent.Enabled = true;
            ddlDescription.Enabled = true;
        }

        protected void btnAddNewReg_Click(object sender, EventArgs e)
        {
            // Add new player "Add" button to change from tentative to ready to send
            btnPreview.Visible = true;
            ddlRegistrant.Enabled = true;
            ddlEvent.Enabled = true;
            ddlDescription.Enabled = true;
            ddlRegistrant.SelectedIndex = 0;
            btnAddNewRegCancel.Visible = false;
            btnAddNewReg.Visible = false;
            gvNewPoints.Visible = false;
            btnEditNewPoints.Visible = true;
            // Update the opportunities as ready to send
            int UserID = 0;
            if (Session["UserID"] != null)
                int.TryParse(Session["UserID"].ToString(), out UserID);
            Classes.cPoints Point = new Classes.cPoints();
            foreach (GridViewRow gvrow in gvNewPoints.Rows)
            {
                try
                {
                    int index = gvNewPoints.EditIndex;
                    int iTemp;
                    double dblTemp = 0;
                    double CP = 0;
                    HiddenField hidCPOpp = (HiddenField)gvNewPoints.Rows[gvrow.RowIndex].FindControl("hidPointID");
                    int intCPOpp = 0;
                    if (int.TryParse(hidCPOpp.Value.ToString(), out iTemp))
                        intCPOpp = iTemp;
                    string strComments = "";
                    GridViewRow row = gvNewPoints.Rows[gvrow.RowIndex];
                    Label lblCPValue = (Label)gvNewPoints.Rows[gvrow.RowIndex].FindControl("lblNewCPValue");
                    Label lblStaffComments = (Label)gvNewPoints.Rows[gvrow.RowIndex].FindControl("lblStaffComments");
                    strComments = lblStaffComments.Text;
                    if (double.TryParse(lblCPValue.Text.ToString(), out dblTemp))
                    {
                        CP = dblTemp;
                    }
                    else
                    {
                        string jsString = "alert('Point value must be a number.');";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                                "MyApplication",
                                jsString,
                                true);
                        return;
                    }
                    Point.UpdateTentativeOpportunity(UserID, intCPOpp, CP, strComments, 68);
                }
                catch (Exception ex)
                {
                    string l = ex.Message;
                }
            }
            gvPoints.EditIndex = -1;
            FillGrid(hidUserName.Value, hidCampaignID.Value);
        }

        protected void AddNewPointsTentative(string strComboID)
        {
            // Add records to CMCPOpportunity table with status of 80 (tentative) until committed.  Either commit when done or clean-up after yourself.
            int CampaignPlayerID = 0;
            int UserID = 0;
            int ComboID = 0;
            int iTemp = 0;
            if (Int32.TryParse(hidCampaignPlayerID.Value, out iTemp))
                CampaignPlayerID = iTemp;
            if (Int32.TryParse(strComboID, out iTemp))
                ComboID = iTemp;
            if (Int32.TryParse(Session["UserID"].ToString(), out iTemp))
                UserID = iTemp;
            string stStoredProc = "uspInsOpportunityFromComboID";
            //string stCallingMethod = "PointsEmail.aspx.AddNewPointsTentative";
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignPlayerID", CampaignPlayerID);
            sParams.Add("@PlayerID", ddlRegistrant.SelectedValue);
            sParams.Add("@UserID", UserID);
            sParams.Add("@ComboID", ComboID);
            sParams.Add("@EventID", ddlEvent.SelectedValue);
            sParams.Add("@EventName", ddlEvent.SelectedItem);
            sParams.Add("@ReceivingCampaignName", ddlCampaign.SelectedItem);
            sParams.Add("@ReceivingCampaignID", ddlCampaign.SelectedValue);
            try
            {
                cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", hidUserName.Value);
            }
            catch
            {

            }
        }

        protected void FillGridNewPoints()
        {
            int iTemp = 0;
            int intUserID = 0;
            int intFromCampaignID = 0;
            int intToCampaignID = 0;
            if (int.TryParse(ddlRegistrant.SelectedValue.ToString(), out iTemp))
                intUserID = iTemp;
            if (int.TryParse(hidCampaignID.Value, out iTemp))
                intFromCampaignID = iTemp;
            if (int.TryParse(ddlCampaign.SelectedValue.ToString(), out iTemp))
                intToCampaignID = iTemp;
            string stStoredProc = "uspGetCampaignPointOpportunitiesToSendOutTentative";
            string stCallingMethod = "PointsEmail.aspx.FillGridNewPoints";
            DataTable dtOpportunities = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@UserID", intUserID);
            sParams.Add("@FromCampaignID", intFromCampaignID);
            sParams.Add("@ToCampaignID", intToCampaignID);
            dtOpportunities = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", hidUserName.Value, stCallingMethod);
            gvNewPoints.DataSource = dtOpportunities;
            gvNewPoints.DataBind();
        }

        protected void gvNewPoints_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvNewPoints.EditIndex = -1;
            FillGridNewPoints();
        }

        protected void gvNewPoints_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Session["EditMode"] = "Edit";
            gvNewPoints.EditIndex = e.NewEditIndex;
            FillGridNewPoints();
        }

        protected void gvNewPoints_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int index = gvNewPoints.EditIndex;
                int iTemp;
                int UserID = 0;
                int OppID = 0;
                double dblTemp = 0;
                double CP = 0;
                if (int.TryParse(Session["UserID"].ToString(), out iTemp))
                    UserID = iTemp;
                HiddenField hidPointID = (HiddenField)gvNewPoints.Rows[e.RowIndex].FindControl("hidPointID");
                if (int.TryParse(hidPointID.Value.ToString(), out iTemp))
                    OppID = iTemp;
                string strComments = "";
                if (Session["EditMode"].ToString() == "Edit")
                {
                    GridViewRow row = gvNewPoints.Rows[index];
                    TextBox txtComments = row.FindControl("tbStaffComments") as TextBox;
                    strComments = txtComments.Text;
                    TextBox txtCP = row.FindControl("txtNewCPValue") as TextBox;
                    if (double.TryParse(txtCP.Text.ToString(), out dblTemp))
                        CP = dblTemp;
                    Session["EditMode"] = "Assign";
                }
                else
                {
                    Label lblCPValue = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblCPValue");
                    Label lblStaffComents = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblStaffComments");
                    if (double.TryParse(lblCPValue.Text, out dblTemp))
                        CP = dblTemp;
                    strComments = lblStaffComents.Text;

                }
                Classes.cPoints Point = new Classes.cPoints();
                Point.UpdateTentativeOpportunity(UserID, OppID, CP, strComments, 80);   // 80 - Tentative --> 68 - Ready to send --> 69 Sent to campaign
                // Audit status 78 - Emailed
            }
            catch (Exception ex)
            {
                string l = ex.Message;
            }
            gvNewPoints.EditIndex = -1;
            FillGridNewPoints();
        }

        protected void gvNewPoints_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            foreach (DictionaryEntry entry in e.NewValues)
            {
                e.NewValues[entry.Key] = Server.HtmlEncode(entry.Value.ToString());
            }
        }

        protected void gvNewPoints_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Rick - 3/26/2017 - Review
            HiddenField hidCPOpp = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidPointID");
            int iTemp = 0;
            int intCPOpp = 0;
            int UserID = 0;
            if (int.TryParse(Session["UserID"].ToString(), out iTemp))
                UserID = iTemp;
            if (int.TryParse(hidCPOpp.Value.ToString(), out iTemp))
                intCPOpp = iTemp;
            Classes.cPoints Points = new Classes.cPoints();
            Points.DeleteCPOpportunity(UserID, intCPOpp);
            gvPoints.EditIndex = -1;
            FillGridNewPoints();
        }

        protected void gvNewPoints_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DataRowView dRow = e.Row.DataItem as DataRowView;
                }
            }
        }

        // ************************ Email screen stuff here

        protected void ResetHiddenValues()
        {
            int UserID = 0;
            if (Session["UserID"] != null)
                int.TryParse(Session["UserID"].ToString(), out UserID);
        }


        protected void BuildPreview()
        {
            int UserID = 0;
            if (Session["UserID"] != null)
                int.TryParse(Session["UserID"].ToString(), out UserID);
            string stStoredProc = "uspCreateEmailToSendPoints";
            string stCallingMethod = "PointsEmail.aspx.BuildPreview";
            DataTable dtEmail = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@ToCampaignID", ddlCampaign.SelectedValue);
            sParams.Add("@FromCampaignID", hidCampaignID.Value);
            if (txtAlternateFromEmail.Text != "")
            {

                sParams.Add("@FromEmailAddress", txtAlternateFromEmail.Text);
            }
            if (hidAlternateTo.Value != "")
            {
                sParams.Add("@AlternateToEmail", hidAlternateTo.Value);
            }
            sParams.Add("@FromUserID", UserID);
            sParams.Add("@CCEmail", txtAdditionalCcs.Text);
            if (txtNewSubject.Text != "")
            {
                sParams.Add("@NewSubject", txtNewSubject.Text);
            }
            sParams.Add("@AdditionalPlayers", hidAdditionalPlayers.Value);
            sParams.Add("@AdditionalText", hidBodyAdditionalText.Value);
            dtEmail = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", hidUserName.Value, stCallingMethod);
            foreach (DataRow drow in dtEmail.Rows)
            {
                lblEmail.Text = drow["Email"].ToString();
                // Set up the individual fields for sending the email
                hidEmailTo.Value = drow["EmailTo"].ToString();
                hidEmailCC.Value = drow["EmailCC"].ToString();
                hidEmailBCC.Value = drow["EmailBCC"].ToString();
                hidEmailSubject.Value = drow["EmailSubject"].ToString();
                hidEmailBody.Value = drow["EmailBody"].ToString();
            }

        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            // Rick 2/5/2017 - Delete function
        }

        protected void btnPreviewUpdate_Click(object sender, EventArgs e)
        {
            lblEmailFailed.Visible = false;
            if (txtAdditionalPlayers.Text != "")
            {
                hidAdditionalPlayers.Value = txtAdditionalPlayers.Text;
                //txtAdditionalPlayers.Text = "";
            }
            if (txtAdditionalCcs.Text != "")
            {
                hidCc.Value = txtAdditionalCcs.Text;
                //txtAdditionalCcs.Text = "";
                if (hidEmailTo.Value == "")
                {
                    hidEmailTo.Value = txtAdditionalCcs.Text;
                    hidAlternateTo.Value = txtAdditionalCcs.Text;
                    txtAdditionalCcs.Text = "";
                    hidCc.Value = "";
                    //hidAlternateTo.Value = txtAdditionalCcs.Text;
                }
            }
            if (hidEmailTo.Value == "")
            {
                string jsString = "alert('There is no To: address.  Please enter email for additional recipients.');";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                        "MyApplication",
                        jsString,
                        true);
                return;
            }
            if (txtNewSubject.Text != "")
            {
                hidSubject.Value = txtNewSubject.Text;
                //txtNewSubject.Text = "";
            }
            if (txtBodyAdditionalText.Text != "")
            {
                hidBodyAdditionalText.Value = txtBodyAdditionalText.Text;
                //txtBodyAdditionalText.Text = "";
            }
            BuildPreview();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlCampaign.Enabled = true;
            btnPreview.Visible = true;
            btnCancel.Visible = false;
            btnSendEmail.Visible = false;
            btnPreviewUpdate.Visible = false;
            pnlPreviewEmail.Visible = false;
            pnlAwaitingSending.Visible = true;
            txtAdditionalCcs.Text = "";
            txtAdditionalPlayers.Text = "";
            txtAlternateFromEmail.Text = "";
            txtBodyAdditionalText.Text = "";
            txtNewSubject.Text = "";
            hidCc.Value = "";
            hidSubject.Value = hidSubjectOriginal.Value;
            hidBodyAdditionalText.Value = "";
            hidEmailTo.Value = "";
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            // Send the email
            GenerateEmail();
            if (lblEmailFailed.Text == "")
            {
                ProcessRecords();
                // Turn back on things that should be on.  Turn off things that should be off.
                txtAdditionalCcs.Text = "";
                txtAdditionalPlayers.Text = "";
                txtAlternateFromEmail.Text = "";
                txtBodyAdditionalText.Text = "";
                txtNewSubject.Text = "";
                pnlAddMissingRegistration.Visible = false;
                pnlAwaitingSending.Visible = true;
                pnlPreviewEmail.Visible = false;
                ddlCampaign.SelectedIndex = 0;
                ddlCampaign.Enabled = true;
                btnCancel.Visible = false;
                btnSendEmail.Visible = false;
                btnPreviewUpdate.Visible = false;
                hidEmailTo.Value = "";
                FillGrid(hidUserName.Value, hidCampaignID.Value);
            }
            else
            {
                lblEmailFailed.Visible = true;
            }

        }

        protected void GenerateEmail()
        {
            lblEmailFailed.Text = "";
            string strTo = hidEmailTo.Value;
            if (strTo == "")
            {
                string jsString = "alert('There is no To: address.  Please enter email for additional recipients.');";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                        "MyApplication",
                        jsString,
                        true);
                lblEmailFailed.Text = "There is no To: address.  Please enter email for additional recipients.";
                return;
            }
            string strCC = hidEmailCC.Value;
            string strBCC = hidEmailBCC.Value;
            string strSubject = hidEmailSubject.Value;
            string strBody = hidEmailBody.Value;
            Classes.cEmailMessageService NotifyStaff = new Classes.cEmailMessageService();
            try
            {
                NotifyStaff.SendMail(strSubject, strBody, strTo, strCC, strBCC, "PointsEmail", hidUserName.Value);
            }
            catch (Exception)
            {
                lblEmailFailed.Text = "There was an issue. Please contact us at support@larportal.com for assistance.";
            }
        }

        protected void ProcessRecords()
        {
            // Create the PLPlayerAudit & update the Opportunity status
            int UserID = 0;
            int iTemp = 0;
            if (Int32.TryParse(Session["UserID"].ToString(), out iTemp))
                UserID = iTemp;
            string stStoredProc = "uspProcessEmailedPoints";
            //string stCallingMethod = "PointsEmail.aspx.AddNewRegCancel";
            SortedList sParams = new SortedList();
            sParams.Add("@ToCampaignID", ddlCampaign.SelectedValue);
            sParams.Add("@FromCampaignID", hidCampaignID.Value);
            try
            {
                cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", hidUserName.Value);
            }
            catch
            {

            }

        }

    }
}