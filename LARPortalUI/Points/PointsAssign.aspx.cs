using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Points
{
    public partial class PointsAssign : System.Web.UI.Page
    {
        public Boolean ForceReload = false;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
            Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
        }

        protected void MasterPage_CampaignChanged(object sender, EventArgs e)
        {
            ForceReload = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlPointTypeLoad(Master.CampaignID);
                if (ViewState["SortField"] == null)
                    ViewState["SortField"] = "PlayerLN";
                if (ViewState["SortDir"] == null)
                    ViewState["SortDir"] = "";
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack || ForceReload)
            {

                Classes.cUserOptions OptionsLoader = new Classes.cUserOptions();
                OptionsLoader.LoadUserOptions(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
                foreach (Classes.cUserOption Option in OptionsLoader.UserOptionList)
                {

                    {
                        if (Option.ObjectOption.ToUpper() == "SORTDIR")
                            ViewState["SortDir"] = Option.OptionValue;
                        else if (Option.ObjectOption.ToUpper() == "SORTFIELD")
                            ViewState["SortField"] = Option.OptionValue;
                    }
                }

                Session["EditMode"] = "Assign";
                ddlAttendanceLoad(Master.UserName, Master.CampaignID);
                ddlCharacterLoad(Master.UserName, Master.CampaignID);
                //ddlCharacterLoadNonCP(Master.UserName, Master.CampaignID);
                ddlEarnReasonLoad(Master.UserName, Master.CampaignID);
                ddlEarnTypeLoad(Master.UserName, Master.CampaignID);
                ddlPlayerLoad(Master.UserName, Master.CampaignID);
                FillGrid();
                ddlCampaignPlayerLoad(Master.UserName, Master.CampaignID);
                ddlCampaignPlayerLoadNonCP(Master.UserName, Master.CampaignID);
                ddlAddCharacterLoad(Master.UserName, Master.CampaignID);
                DropdownListDefaultColors();
            }
        }
        private void ddlPointTypeLoad(int intCampaignID)
        {

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlPointType.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", intCampaignID);
            DataTable dtPools = Classes.cUtilities.LoadDataTable("uspGetCampaignPools", sParams, "LARPortal", Master.UserName, lsRoutineName + "uspGetCampaignPools");
            ddlPointType.DataTextField = "PoolDescription";
            ddlPointType.DataValueField = "CampaignSkillPoolID";
            ddlPointType.DataSource = dtPools;
            ddlPointType.DataBind();
            //ddlPointType.Items.Insert(0, new ListItem("Select Player", "0"));
            ddlPointType.SelectedIndex = 0;

        }
        private void FillGrid()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@StatusID", 19);   // 19 = Claimed
            sParams.Add("@EventID", ddlAttendance.SelectedValue);
            sParams.Add("@CampaignCPOpportunityDefaultID", ddlEarnReason.SelectedValue);
            sParams.Add("@UserID", ddlPlayer.SelectedValue);
            sParams.Add("@CharacterID", ddlCharacters.SelectedValue);
            DataTable dtOpportunities = Classes.cUtilities.LoadDataTable("uspGetCampaignPointOpportunities", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignPointOpportunities");
            DataView dvOpportunities = new DataView(dtOpportunities, "", ViewState["SortField"].ToString() + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows);
            gvPoints.DataSource = dvOpportunities;
            gvPoints.DataBind();
        }

        private void ddlAttendanceLoad(string strUserName, int intCampaignID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlAttendance.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", intCampaignID);
            sParams.Add("@StatusID", 51); // 51 = Completed
            sParams.Add("@EventLength", 60); // How many characters of Event Name/date to return with ellipsis
            DataTable dtAttendance = Classes.cUtilities.LoadDataTable("uspGetCampaignEvents", sParams, "LARPortal", strUserName, lsRoutineName + ".uspGetCampaignEvents");
            ddlAttendance.DataTextField = "EventNameDate";
            ddlAttendance.DataValueField = "EventID";
            ddlAttendance.DataSource = dtAttendance;
            ddlAttendance.DataBind();
            ddlAttendance.Items.Insert(0, new ListItem("Select Event - Date", "0"));
            ddlAttendance.SelectedIndex = 0;
        }

        private void ddlCharacterLoad(string strUserName, int intCampaignID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlCharacters.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", intCampaignID);
            sParams.Add("@CharacterDisplayNameLength", 25); // How much of the character name do you want to return
            DataTable dtCharacters = Classes.cUtilities.LoadDataTable("uspGetCampaignCharacters", sParams, "LARPortal", strUserName, lsRoutineName + ".uspGetCampaignCharacters");
            ddlCharacters.DataTextField = "CharacterName";
            ddlCharacters.DataValueField = "CharacterID";
            ddlCharacters.DataSource = dtCharacters;
            ddlCharacters.DataBind();
            ddlCharacters.Items.Insert(0, new ListItem("Select Character", "0"));
            ddlCharacters.SelectedIndex = 0;
        }

        private void ddlCharacterLoadNonCP(int intCampaignPlayerID, int intCampaignID)
        {
            {
                //private void ddlCharacterLoadNonCP(string strUserName, int intCampaignID)
                //Convert CampaignPlayerID using uspConvertCampaignPlayerID
                string stStoredProc = "uspConvertCampaignPlayerID";
                string stCallingMethod = "PointsAssign.aspx.ddlCharacterLoadNonCP";
                int iTemp = 0;
                int CampaignPlayerID = 0;

                DataTable dtCampaignPlayers = new DataTable();
                SortedList sParams = new SortedList();
                sParams.Add("@OriginalCampaignPlayerID", intCampaignPlayerID);
                sParams.Add("@NewCampaignID", Master.CampaignID);
                dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
                foreach (DataRow dRow in dtCampaignPlayers.Rows)
                {
                    if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                        CampaignPlayerID = iTemp;
                }
                ddlSelectCharacterNonCP.Items.Clear();
                stStoredProc = "uspGetCampaignCharacters";
                DataTable dtCharacters = new DataTable();
                SortedList sParams1 = new SortedList();
                sParams1.Add("@CampaignID", Master.CampaignID);
                sParams1.Add("@CampaignPlayerID", CampaignPlayerID);
                sParams1.Add("@NonCP", 1);
                dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams1, "LARPortal", Master.UserName, stCallingMethod);
                ddlSelectCharacterNonCP.DataTextField = "CharacterName";
                ddlSelectCharacterNonCP.DataValueField = "CharacterID";
                ddlSelectCharacterNonCP.DataSource = dtCharacters;
                ddlSelectCharacterNonCP.DataBind();
                if (dtCharacters.Rows.Count > 0)
                {
                    ddlSelectCharacterNonCP.Items.Insert(0, new ListItem("Select Character", "0"));
                    ddlSelectCharacterNonCP.SelectedIndex = 0;
                }
                else
                {
                    //ddlSelectCharacterNonCP.Items.Insert(0, new ListItem("No characters, banking", "0"));
                    //ddlSelectCharacterNonCP.SelectedIndex = 0;
                }
            }
        }

        private void ddlEarnReasonLoad(string strUserName, int intCampaignID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlEarnReason.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", intCampaignID);
            DataTable dtEarnReasons = Classes.cUtilities.LoadDataTable("uspGetCampaignCPOpportunities", sParams, "LARPortal", strUserName, lsRoutineName + "uspGetCampaignCPOpportunities");
            ddlEarnReason.DataTextField = "Description";
            ddlEarnReason.DataValueField = "CampaignCPOpportunityDefaultID";
            ddlEarnReason.DataSource = dtEarnReasons;
            ddlEarnReason.DataBind();
            ddlEarnReason.Items.Insert(0, new ListItem("Select Earn Reason", "0"));
            ddlEarnReason.SelectedIndex = 0;
        }

        private void ddlEarnTypeLoad(string strUserName, int intCampaignID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlEarnType.Items.Clear();
            SortedList sParams = new SortedList();
            DataTable dtEarnTypes = Classes.cUtilities.LoadDataTable("uspGetCPOpportunityTypes", sParams, "LARPortal", strUserName, lsRoutineName + "uspGetCPOpportunityTypes");
            ddlEarnType.DataTextField = "OpportunityTypeName";
            ddlEarnType.DataValueField = "CampaignCPOpportunityTypeID";
            ddlEarnType.DataSource = dtEarnTypes;
            ddlEarnType.DataBind();
            ddlEarnType.Items.Insert(0, new ListItem("Select Earn Type", "0"));
            ddlEarnType.SelectedIndex = 0;
        }

        private void ddlPlayerLoad(string strUserName, int intCampaignID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlPlayer.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", intCampaignID);
            DataTable dtPlayers = Classes.cUtilities.LoadDataTable("uspGetCampaignPlayers", sParams, "LARPortal", strUserName, lsRoutineName + "uspGetCampaignPlayers");
            ddlPlayer.DataTextField = "PlayerName";
            ddlPlayer.DataValueField = "UserID";
            ddlPlayer.DataSource = dtPlayers;
            ddlPlayer.DataBind();
            ddlPlayer.Items.Insert(0, new ListItem("Select Player", "0"));
            ddlPlayer.SelectedIndex = 0;
        }

        protected void gvPoints_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPoints.EditIndex = -1;
            FillGrid();
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
            FillGrid();
        }

        protected void gvPoints_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int index = gvPoints.EditIndex;
                int iTemp;
                double dblTemp = 0;
                double CP = 0;
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
                //Label lblEarnDesc = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblEarnDescription");
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
                string strOppNotes = hidOppNotes.Value.ToString();
                string strExURL = hidExURL.Value.ToString();
                //string strDesc = lblEarnDesc.Text;
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
                if (int.TryParse(hidRole.Value.ToString(), out iTemp))
                    intRole = iTemp;
                if (int.TryParse(hidNPCCampaignID.Value.ToString(), out iTemp))
                    intNPCCampaignID = iTemp;
                if (int.TryParse(hidRegistrationID.Value.ToString(), out iTemp))
                    intRegistrationID = iTemp;
                string strNPCCampaignName = "";
                string strComments = "";
                string strDesc = "";
                if (Session["EditMode"].ToString() == "Edit")
                {
                    GridViewRow row = gvPoints.Rows[index];
                    //TextBox txtComments = row.FindControl("tbNPCCampaignName") as TextBox;
                    TextBox txtDesc = row.FindControl("tbEarnDescription") as TextBox;
                    //strComments = txtComments.Text;
                    strDesc = txtDesc.Text;
                    TextBox txtCP = row.FindControl("txtCPValue") as TextBox;
                    if (double.TryParse(txtCP.Text.ToString(), out dblTemp))
                        CP = dblTemp;
                    Session["EditMode"] = "Assign";
                }
                else
                {
                    Label lblCPValue = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblCPValue");
                    Label lblNPCCampaignName = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblNPCCampaignName");
                    Label lblEarnDescription = (Label)gvPoints.Rows[e.RowIndex].FindControl("lblEarnDescription");
                    if (double.TryParse(lblCPValue.Text, out dblTemp))
                        CP = dblTemp;
                    strNPCCampaignName = lblNPCCampaignName.Text;
                    strDesc = lblEarnDescription.Text;

                }
                Classes.cPoints Point = new Classes.cPoints();
                //  JB  10/2/2021  Added points from parameter to try and help with debugging.
                Point.UpdateCPOpportunity(Master.UserID, intCPOpp, intCmpPlyrID, intCharID, intOppDefID, intEvntID,
                    strDesc, strOppNotes, strExURL, intRsnID, intAddID, CP, Master.UserID,
                    DateTime.Now, Master.UserID, strComments, intRole, intNPCCampaignID, Master.CampaignID, "PointsAssign.aspx.cs.gvPoints_RowUpdating");
            }
            catch (Exception ex)
            {
                string l = ex.Message;
            }
            gvPoints.EditIndex = -1;
            FillGrid();
        }

        protected void gvPoints_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            foreach (DictionaryEntry entry in e.NewValues)
            {
                e.NewValues[entry.Key] = Server.HtmlEncode(entry.Value.ToString());
            }
        }

        protected void ddl_ReloadGrid(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void gvPoints_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidCPOpp = (HiddenField)gvPoints.Rows[e.RowIndex].FindControl("hidPointID");
            int intCPOpp = 0;
            if (int.TryParse(hidCPOpp.Value.ToString(), out intCPOpp))
            {
                Classes.cPoints Points = new Classes.cPoints();
                Points.DeleteCPOpportunity(Master.UserID, intCPOpp);
            }
            gvPoints.EditIndex = -1;
            FillGrid();
        }

        protected void btnAddNewOpportunity_Click(object sender, EventArgs e)
        {
            pnlAddNewCP.Visible = true;
            pnlCharacterPointDisplay.Visible = true;
            pnlAssignExisting.Visible = false;
            pnlAddHeader.Visible = true;
            pnlAssignHeader.Visible = false;
            pnlAddNewNonCP.Visible = false;
            pnlCharacterNonCPDisplay.Visible = false;

        }

        protected void btnAssignExisting_Click(object sender, EventArgs e)
        {
            pnlAddNewCP.Visible = false;
            pnlCharacterPointDisplay.Visible = false;
            pnlAssignExisting.Visible = true;
            pnlAddHeader.Visible = false;
            pnlAssignHeader.Visible = true;
            pnlAddNewNonCP.Visible = false;
            pnlCharacterNonCPDisplay.Visible = false;
        }

        protected void ddlSelectCharacterNonCP_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCampaignPlayerLoadNonCP(Master.UserName, Master.CampaignID);
        }

        private void BuildNonCPAuditTable(int PoolID, int UserID)
        {
            int CampaignID = 0;
            int CharacterID = 0;
            CampaignID = Master.CampaignID;

            string CampaignDDL = "";
            CampaignDDL = Master.CampaignName;
            Classes.cTransactions CPAudit = new Classes.cTransactions();
            DataTable dtCPAudit = new DataTable();
            dtCPAudit = CPAudit.GetNonCPAuditList(UserID, PoolID, CharacterID);

            DataView dvPoints = new DataView(dtCPAudit, "", "", DataViewRowState.CurrentRows);
            gvPointsNonCPList.DataSource = dvPoints;
            gvPointsNonCPList.DataBind();
            //mvPointList.SetActiveView(vwNonCPList);

        }

        protected void btnSaveNewNonCP_Click(object sender, EventArgs e)
        {
            if(ddlSelectCharacterNonCP.SelectedValue == "0")
            {
                //Display choose a character option
                lblModalMessage.Text = "You must select a character to assign the points to.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
            }
            else
            {
                if(string.IsNullOrEmpty(tbNonCPAmount.Text))
                {
                    lblModalMessage.Text = "You must assign a non-zero number of points.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
                }
                else
                {
                    if(string.IsNullOrEmpty(tbNonCPReceiptDate.Text))
                    {
                        lblModalMessage.Text = "You must assign an award date.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
                    }
                    else
                    {
                        Classes.cPoints PointsNonCPAdd = new Classes.cPoints();
                        int addCharacterID = 0;
                        int.TryParse(ddlSelectCharacterNonCP.SelectedValue.ToString(), out addCharacterID);
                        string addOpportunityNotes;
                        addOpportunityNotes = tbNonCPVisibleReason.Text.Trim();
                        int AddedByID = Master.UserID;
                        double addCPValue = 0;            
                        double.TryParse(tbNonCPAmount.Text.Trim(), out addCPValue);
                        int addPoolID = 0;
                        int.TryParse(ddlPointType.SelectedValue.ToString(), out addPoolID);
                        DateTime addReceiptDate = DateTime.Now;
                        DateTime.TryParse(tbNonCPReceiptDate.Text.Trim(), out addReceiptDate);
                        string addStaffComments;
                        addStaffComments = tbNonCPHiddenReason.Text.Trim();
                        PointsNonCPAdd.AddNonCPPoints(addCharacterID, addOpportunityNotes, AddedByID, addCPValue, addPoolID, addReceiptDate, addStaffComments);
                        tbNonCPVisibleReason.Text = string.Empty;
                        tbNonCPAmount.Text = string.Empty;
                        tbNonCPReceiptDate.Text = string.Empty;
                        tbNonCPHiddenReason.Text = string.Empty;
                        ddlSelectCharacterNonCP.Visible = false;
                        pnlCharacterNonCPDisplay.Visible = false;
                    }
                }
            }
        }

        protected void ddlCampaignPlayerNonCP_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            ResetHiddenValues();
            string stStoredProc = "uspGetCampaignPlayers";
            string stCallingMethod = "PointsAssign.aspx.ddlCampaignPlayerNonCP_SelectedIndexChanged";
            int PoolID = 0;
            int.TryParse(ddlPointType.SelectedValue.ToString(), out PoolID);
            int intCampaignPlayerID = 0;
            int.TryParse(ddlCampaignPlayerNonCP.SelectedValue.ToString(), out intCampaignPlayerID);
            int UserID = 0;
            DataTable dtUsers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@CampaignPlayerID", intCampaignPlayerID);
            dtUsers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtUsers.Rows)
            {
                int.TryParse(dRow["UserID"].ToString(), out UserID);
                hidCampaignPlayerUserID.Value = UserID.ToString();
                BuildNonCPAuditTable(PoolID, UserID);
            }
            ddlCharacterLoadNonCP(intCampaignPlayerID, Master.CampaignID);
            ddlSelectCharacterNonCP.Visible = true;
            pnlCharacterNonCPDisplay.Visible = true;
        }

        protected void btnSaveNewOpportunity_Click(object sender, EventArgs e)
        {
            Classes.cPoints PointAdd = new Classes.cPoints();
            int addCampaignPlayerID = 0;
            int.TryParse(hidInsertCampaignPlayerID.Value.ToString(), out addCampaignPlayerID);
            int addCharacterID = 0;
            int.TryParse(hidInsertCharacterID.Value.ToString(), out addCharacterID);
            int addCampaignCPOpportunityDefaultID = 0;
            int.TryParse(hidInsertCampaignCPOpportunityDefaultID.Value.ToString(), out addCampaignCPOpportunityDefaultID);
            int addEventID = 0;
            int.TryParse(hidInsertEventID.Value.ToString(), out addEventID);
            int addCampaignID = 0;
            int.TryParse(ddlAddSourceCampaign.SelectedValue.ToString(), out addCampaignID);
            string addDescription;
            addDescription = hidInsertDescription.Value.Trim();
            string addOpportunityNotes;
            addOpportunityNotes = hidInsertOpportunityNotes.Value.Trim();
            string addExampleURL;
            addExampleURL = hidInsertExampleURL.Value.Trim();
            int addReasonID = 0;
            int.TryParse(hidInsertReasonID.Value.ToString(), out addReasonID);
            int addStatusID = 21;
            int addAddedByID = 0;
            int.TryParse(hidInsertAddedByID.Value.ToString(), out addAddedByID);
            double addCPValue = 0;
            double.TryParse(hidInsertCPValue.Value.ToString(), out addCPValue);
            int addApprovedByID = 0;
            int.TryParse(hidInsertApprovedByID.Value.ToString(), out addApprovedByID);
            DateTime addReceiptDate = DateTime.Now;
            DateTime.TryParse(hidInsertReceiptDate.Value.ToString(), out addReceiptDate);
            int addReceivedByID = 0;
            int.TryParse(hidInsertReceivedByID.Value.ToString(), out addReceivedByID);
            DateTime addCPAssignmentDate = DateTime.Now;
            DateTime.TryParse(hidInsertCPAssignmentDate.Value.ToString(), out addCPAssignmentDate);
            string addStaffComments;
            addStaffComments = txtStaffComments.Text.Trim();
            switch (hidLastAddCPStep.Value)
            {
                case "F3":
                    // NPC Processing local to LARP Portal campaign
                    for (int i = 1; i < 4; i++)
                    {
                        // Set values for each of the NPC options and call PointAdd
                        switch (i)
                        {
                            case 1:
                                // NPC event
                                if (chkNPCEvent.Checked == true)
                                {
                                    double.TryParse(txtNPCEvent.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCEvent.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCEvent.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCEvent.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            case 2:
                                // NPC setup/cleanup
                                if (chkSetupCleanup.Checked == true)
                                {
                                    double.TryParse(txtSetupCleanup.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCSetup.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCSetup.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCSetup.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            case 3:
                                // NPC PEL
                                if (chkPEL.Checked == true)
                                {
                                    double.TryParse(txtPEL.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCPEL.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCPEL.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCPEL.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case "F4":
                    // NPC Processing non-local to LARP Portal campaign
                    // Convert local CampaignPlayerID to CampaignPlayerID of transfer campaign
                    // Convert CampaignPlayerID using uspConvertCampaignPlayerID
                    string stStoredProc = "uspConvertCampaignPlayerID";
                    string stCallingMethod = "PointsAssign.aspx.btnSaveNewOpportunityClick";
                    int iTemp = 0;
                    int CampaignPlayerID = 0;
                    int NewCampaignID = 0;
                    if (int.TryParse(addCampaignPlayerID.ToString(), out iTemp))
                        CampaignPlayerID = iTemp;
                    int.TryParse(ddlDestinationCampaign.SelectedValue.ToString(), out NewCampaignID);
                    DataTable dtCampaignPlayers = new DataTable();
                    SortedList sParams = new SortedList();
                    sParams.Add("@OriginalCampaignPlayerID", CampaignPlayerID);
                    sParams.Add("@NewCampaignID", NewCampaignID);
                    dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
                    foreach (DataRow dRow in dtCampaignPlayers.Rows)
                    {
                        if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                            addCampaignPlayerID = iTemp;
                    }
                    //
                    addOpportunityNotes = ddlAddSourceCampaign.SelectedItem + "-" + addOpportunityNotes;
                    for (int i = 1; i < 4; i++)
                    {
                        // Set values for each of the NPC options and call PointAdd
                        switch (i)
                        {
                            case 1:
                                // NPC event
                                if (chkNPCEvent.Checked == true)
                                {
                                    double.TryParse(txtNPCEvent.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCEvent.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCEvent.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCEvent.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            case 2:
                                // NPC setup/cleanup
                                if (chkSetupCleanup.Checked == true)
                                {
                                    double.TryParse(txtSetupCleanup.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCSetup.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCSetup.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCSetup.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            case 3:
                                // NPC PEL
                                if (chkPEL.Checked == true)
                                {
                                    double.TryParse(txtPEL.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCPEL.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCPEL.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCPEL.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case "F5":
                    // TODO - Rick - NPC processing non-local to non-LARP Portal campaign via email
                    addOpportunityNotes = ddlAddSourceCampaign.SelectedItem + "-" + addOpportunityNotes;
                    addStatusID = 68;   // set for sending email
                    addEventID = 0;     // set for sending email
                    for (int i = 1; i < 4; i++)
                    {
                        // Set values for each of the NPC options and call PointAdd
                        switch (i)
                        {
                            case 1:
                                // NPC event
                                if (chkNPCEvent.Checked == true)
                                {
                                    double.TryParse(txtNPCEvent.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCEvent.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCEvent.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCEvent.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            case 2:
                                // NPC setup/cleanup
                                if (chkSetupCleanup.Checked == true)
                                {
                                    double.TryParse(txtSetupCleanup.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCSetup.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCSetup.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCSetup.Value.ToString(), out addReasonID);
                                    //PointAdd.AddManualCPEntry(addUserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                    //    addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                    //    addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            case 3:
                                // NPC PEL
                                if (chkPEL.Checked == true)
                                {
                                    double.TryParse(txtPEL.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCPEL.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCPEL.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCPEL.Value.ToString(), out addReasonID);
                                    //PointAdd.AddManualCPEntry(addUserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                    //    addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                    //    addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                case "E6":
                    // NPC processing from other campaign
                    addOpportunityNotes = ddlAddSourceCampaign.SelectedItem + "-" + addOpportunityNotes;
                    for (int i = 1; i < 4; i++)
                    {
                        // Set values for each of the NPC options and call PointAdd
                        switch (i)
                        {
                            case 1:
                                // NPC event
                                if (chkNPCEvent.Checked == true)
                                {
                                    double.TryParse(txtNPCEvent.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCEvent.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCEvent.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCEvent.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            case 2:
                                // NPC setup/cleanup
                                if (chkSetupCleanup.Checked == true)
                                {
                                    double.TryParse(txtSetupCleanup.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCSetup.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCSetup.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCSetup.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            case 3:
                                // NPC PEL
                                if (chkPEL.Checked == true)
                                {
                                    double.TryParse(txtPEL.Text.ToString(), out addCPValue);
                                    int.TryParse(hidInsertCampaignCPOpportunityDefaultIDNPCPEL.Value, out addCampaignCPOpportunityDefaultID);
                                    addDescription = hidInsertDescriptionNPCPEL.Value.Trim();
                                    int.TryParse(hidInsertReasonIDNPCPEL.Value.ToString(), out addReasonID);
                                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID,
                                        addDescription, addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate,
                                        addReceivedByID, addCPAssignmentDate, addStaffComments);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;

                default:
                    PointAdd.AddManualCPEntry(Master.UserID, addCampaignPlayerID, addCharacterID, addCampaignCPOpportunityDefaultID, addEventID, addCampaignID, addDescription,
                        addOpportunityNotes, addExampleURL, addReasonID, addStatusID, addAddedByID, addCPValue, addApprovedByID, addReceiptDate, addReceivedByID, addCPAssignmentDate,
                        addStaffComments);

                    break;
            }
            lblAddMessage.Text = "Points added";
            int UserID = 0;
            int.TryParse(hidCampaignPlayerUserID.Value, out UserID);
            BuildCPAuditTable(UserID);
            Session["AddCPStep"] = "A";
            ResetHiddenValues();
            ResetDropDownListChoices();
        }

        protected void ddlAddOpportunityDefaultIDLoad(int CampaignID)
        {
            string stStoredProc = "uspGetCampaignCPOpportunityDefaults";
            string stCallingMethod = "PointsAssign.aspx.ddlAddOpportunityDefaultIDLoad";
            DataTable dtCPOpportunityDefaults = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", CampaignID);
            dtCPOpportunityDefaults = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            ddlAddOpportunityDefaultID.DataTextField = "Description";
            ddlAddOpportunityDefaultID.DataValueField = "CampaignCPOpportunityDefaultID";
            ddlAddOpportunityDefaultID.DataSource = dtCPOpportunityDefaults;
            ddlAddOpportunityDefaultID.DataBind();
            ddlAddOpportunityDefaultID.Items.Insert(0, new ListItem("Select Description", "0"));
            ddlAddOpportunityDefaultID.SelectedIndex = 0;
        }

        protected void ddlAddOpportunityDefaultID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            Session["AddCPStep"] = "C1";
            hidLastAddCPStep.Value = "C1";
            // If Donation --> F0 / Non-Event --> F1 / If PC --> F2 / If NPC/Staff --> D3
            AddPanelVisibility();
            string stStoredProc = "uspGetCampaignCPOpportunityDefaults";
            string stCallingMethod = "PointsAssign.aspx.ddlAddOpportunityDefaultID_SelectedIndexChanged";
            int DefaultOpportunityID = 0;
            int OpportunityType = 0;
            //int CampaignID = 0;
            int CampaignCPOpportunityDefaultID = 0;
            string EarnType = "";
            int.TryParse(ddlAddOpportunityDefaultID.SelectedValue, out DefaultOpportunityID);
            DataTable dtEarnType = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignCPOpportunityID", DefaultOpportunityID);
            sParams.Add("@CampaignID", Master.CampaignID);
            dtEarnType = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            if (String.IsNullOrEmpty(EarnType))
                EarnType = "NPC";
            foreach (DataRow dRow in dtEarnType.Rows)
            {
                hidInsertDescription.Value = dRow["Description"].ToString();
                int.TryParse(dRow["OpportunityTypeID"].ToString(), out OpportunityType);
                if (OpportunityType == 1 || OpportunityType == 2 || OpportunityType == 4)
                {
                    int.TryParse(dRow["CampaignCPOpportunityDefaultID"].ToString(), out CampaignCPOpportunityDefaultID);
                    if (CampaignCPOpportunityDefaultID == 1)
                    {
                        EarnType = "Donation";  // F0
                        Session["AddCPStep"] = "F0";
                        hidLastAddCPStep.Value = "F0";
                        txtCPF0.Text = dRow["CPValue"].ToString();
                        ddlSelectCharacterOrBankF0Load();
                    }
                    else
                    {
                        EarnType = "NonEvent";  // F1
                        Session["AddCPStep"] = "F1";
                        hidLastAddCPStep.Value = "F1";
                        txtCPF1.Text = dRow["CPValue"].ToString();
                        hidInsertCPValue.Value = txtCPF1.Text;
                        ddlSelectCharacterOrBankF1Load();
                    }
                }
                else
                {
                    if (OpportunityType == 3 && (dRow["Description"].ToString().Contains("NPC") || dRow["Description"].ToString().Contains("Staff")))    // NPC or staff
                    {
                        EarnType = "NPC";   // D3
                        Session["AddCPStep"] = "D3";
                        hidLastAddCPStep.Value = "D3";
                        ddlDestinationCampaignLoad(Master.CampaignID);
                    }
                    else
                    {
                        // PC (we hope)     // F2
                        EarnType = "PC";
                        Session["AddCPStep"] = "F2";
                        hidLastAddCPStep.Value = "F2";
                        ddlSourceEventPCLoad();
                        txtCPF2.Text = dRow["CPValue"].ToString();
                        hidInsertCPValue.Value = txtCPF2.Text;
                        ddlSelectCharacterOrBankF2Load();
                    }
                }
                hidInsertCampaignCPOpportunityDefaultID.Value = ddlAddOpportunityDefaultID.SelectedValue;
                int OppID = 0;
                int.TryParse(ddlAddOpportunityDefaultID.SelectedValue, out OppID);
                LookupReasonID(OppID);
                AddPanelVisibility();
            }
        }

        protected void LookupReasonID(int OpportunityDefaultID)
        {
            string stStoredProc = "uspGetCampaignCPOpportunityDefaults";
            string stCallingMethod = "PointsAssign.aspx.LookupReasonID";
            int ReasonID = 0;
            DataTable dtOpportunityDefault = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignCPOpportunityID", OpportunityDefaultID);
            sParams.Add("@CampaignID", Master.CampaignID);
            dtOpportunityDefault = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtOpportunityDefault.Rows)
            {
                int.TryParse(dRow["ReasonID"].ToString(), out ReasonID);
                hidInsertReasonID.Value = ReasonID.ToString();
            }
        }

        protected void ddlDonationTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidInsertDescription.Value = ddlDonationTypes.SelectedItem.Text;
        }

        protected void ddlAddOpportunityDefaultIDC6_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
        }

        protected void ddlSourceEventLoad()
        {
            string stStoredProc = "uspGetCampaignEvents";
            string stCallingMethod = "PointsAssign.aspx.ddlSourceEventPCLoad";
            DataTable dtAttendance = new DataTable();
            SortedList sParams = new SortedList();
            int CampaignID = 0;
            int.TryParse(ddlAddSourceCampaign.SelectedValue, out CampaignID);
            sParams.Add("@CampaignID", CampaignID);
            sParams.Add("@StatusID", 51); // 51 = Completed
            sParams.Add("@EventLength", 60); // How many characters of Event Name/date to return with ellipsis
            dtAttendance = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            ddlSourceEvent.DataTextField = "EventNameDate";
            ddlSourceEvent.DataValueField = "EventID";
            ddlSourceEvent.DataSource = dtAttendance;
            ddlSourceEvent.DataBind();
            ddlSourceEvent.Items.Insert(0, new ListItem("Select Event - Date", "0"));
            ddlSourceEvent.SelectedIndex = 0;
        }

        protected void ddlSourceEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            hidInsertEventID.Value = ddlSourceEvent.SelectedValue;
            hidInsertOpportunityNotes.Value = ddlSourceEvent.SelectedItem.Text;
        }

        protected void ddlSourceEventPCLoad()
        {
            string stStoredProc = "uspGetCampaignEvents";
            string stCallingMethod = "PointsAssign.aspx.ddlSourceEventPCLoad";
            DataTable dtAttendance = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@StatusID", 51); // 51 = Completed
            sParams.Add("@EventLength", 60); // How many characters of Event Name/date to return with ellipsis
            dtAttendance = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            ddlSourceEventPC.DataTextField = "EventNameDate";
            ddlSourceEventPC.DataValueField = "EventID";
            ddlSourceEventPC.DataSource = dtAttendance;
            ddlSourceEventPC.DataBind();
            ddlSourceEventPC.Items.Insert(0, new ListItem("Select Event - Date", "0"));
            ddlSourceEventPC.SelectedIndex = 0;
        }

        protected void ddlDestinationCampaignLoad(int CurrentCampaignID)
        {
            // Should this only include PCs campaigns working on the assumption they'll assign to their own campaigns when registering?
            // Yes, I think we should because it puts the onus on them to register correctly
            // No, you're wrong.  Show the list of ALL campaigns that are on the exchange list.  Send it there if the NPC is already a campaign player there.
            // TODO - RP - Add them as a campaign player to that campaign if they're not with a warning to that effect.

            string stStoredProc = "uspGetSourceCampaigns";
            string stCallingMethod = "PointsAssign.aspx.ddlDestinationCampaignLoad";
            DataTable dtDestinationCampaigns = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", CurrentCampaignID);
            dtDestinationCampaigns = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            ddlDestinationCampaign.DataTextField = "CampaignName";
            ddlDestinationCampaign.DataValueField = "CampaignID";
            ddlDestinationCampaign.DataSource = dtDestinationCampaigns;
            ddlDestinationCampaign.DataBind();
            ddlDestinationCampaign.Items.Insert(0, new ListItem("Select Campaign", "0"));
            ddlDestinationCampaign.SelectedIndex = 0;
        }

        protected void ddlDestinationCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            string CampaignLPType;
            // If current campaign is picked then E3 otherwise E4
            if (ddlDestinationCampaign.SelectedValue == Master.CampaignID.ToString())
            {
                CampaignLPType = "S";
                Session["AddCPStep"] = "E3";
                hidLastAddCPStep.Value = "E3";
                //Load Event ddl
                ddlSourceEventLoad();
                AddPanelVisibility();
                Session["AddCPStep"] = "F3";
                hidLastAddCPStep.Value = "F3";
                ddlSelectCharacterOrBankF3Load();
            }
            else
            {
                Session["AddCPStep"] = "E4";
                hidLastAddCPStep.Value = "E4";
                hidInsertDestinationCampaign.Value = ddlDestinationCampaign.SelectedValue;
                //Load Event ddl
                ddlSourceEventLoad();
                AddPanelVisibility();
                //Need to check whether it's a LARP Portal campaign or not before deciding between F4 and F5
                int iTemp = 0;
                Int32.TryParse(hidInsertDestinationCampaign.Value.ToString(), out iTemp);
                if (iTemp != 0)
                {
                    Classes.cCampaignBase cCampaign = new Classes.cCampaignBase(iTemp, Master.UserName, 0);
                    CampaignLPType = cCampaign.PortalAccessType;
                }
                else
                {
                    CampaignLPType = "B";
                }
                if (CampaignLPType == "S")
                {
                    Session["AddCPStep"] = "F4";
                    hidLastAddCPStep.Value = "F4";
                }
                else
                {
                    Session["AddCPStep"] = "F5";
                    hidLastAddCPStep.Value = "F5";
                }
                ddlSelectCharacterOrBankF4Load();
            }
            hidInsertDestinationCampaign.Value = ddlDestinationCampaign.SelectedValue;
            hidInsertDestinationCampaignLPType.Value = CampaignLPType;
            AddPanelVisibility();
        }

        protected void ddlSourceEventPC_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            hidInsertEventID.Value = ddlSourceEventPC.SelectedValue;
            hidInsertOpportunityNotes.Value = ddlSourceEventPC.SelectedItem.Text;
        }

        //protected void chkNPCEvent_CheckedChanged(object sender, EventArgs e)
        //{

        //}

        //protected void chkSetupCleanup_CheckedChanged(object sender, EventArgs e)
        //{

        //}

        //protected void chkPEL_CheckedChanged(object sender, EventArgs e)
        //{

        //}

        protected void ddlPickCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Old? Can be deleted?
        }

        protected void ddlSelectCharacterOrBankF0Load()
        {
            //Convert CampaignPlayerID using uspConvertCampaignPlayerID
            string stStoredProc = "uspConvertCampaignPlayerID";
            string stCallingMethod = "PointsAssign.aspx.ddlSelectCharacterOrBankF0Load";
            int iTemp = 0;
            int CampaignPlayerID = 0;
            if (int.TryParse(ddlCampaignPlayer.SelectedValue.ToString(), out iTemp))
                CampaignPlayerID = iTemp;
            DataTable dtCampaignPlayers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@OriginalCampaignPlayerID", CampaignPlayerID);
            sParams.Add("@NewCampaignID", Master.CampaignID);
            dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtCampaignPlayers.Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
            }
            ddlSelectCharacterOrBankF0.Items.Clear();
            stStoredProc = "uspGetCampaignCharacters";
            DataTable dtCharacters = new DataTable();
            SortedList sParams1 = new SortedList();
            sParams1.Add("@CampaignID", Master.CampaignID);
            sParams1.Add("@CampaignPlayerID", CampaignPlayerID);
            dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams1, "LARPortal", Master.UserName, stCallingMethod);
            ddlSelectCharacterOrBankF0.DataTextField = "CharacterName";
            ddlSelectCharacterOrBankF0.DataValueField = "CharacterID";
            ddlSelectCharacterOrBankF0.DataSource = dtCharacters;
            ddlSelectCharacterOrBankF0.DataBind();
            if (dtCharacters.Rows.Count > 0)
            {
                ddlSelectCharacterOrBankF0.Items.Insert(0, new ListItem("Select Character", "0"));
                ddlSelectCharacterOrBankF0.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                ddlSelectCharacterOrBankF0.SelectedIndex = 0;
            }
            else
            {
                ddlSelectCharacterOrBankF0.Items.Insert(0, new ListItem("No characters, banking", "0"));
                ddlSelectCharacterOrBankF0.SelectedIndex = 0;
            }
        }

        protected void ddlSelectCharacterOrBankF0_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            if (Convert.ToInt32(ddlSelectCharacterOrBankF0.SelectedValue) > 1)
            {
                hidInsertCharacterID.Value = ddlSelectCharacterOrBankF0.SelectedValue;
            }
            else
            {
                hidInsertCharacterID.Value = "0";
            }
            txtStaffComments.Focus();
        }

        protected void ddlSelectCharacterOrBankF1Load()
        {
            //Convert CampaignPlayerID using uspConvertCampaignPlayerID
            string stStoredProc = "uspConvertCampaignPlayerID";
            string stCallingMethod = "PointsAssign.aspx.ddlSelectCharacterOrBankF1Load";
            int iTemp = 0;
            int CampaignPlayerID = 0;
            if (int.TryParse(ddlCampaignPlayer.SelectedValue.ToString(), out iTemp))
                CampaignPlayerID = iTemp;
            DataTable dtCampaignPlayers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@OriginalCampaignPlayerID", CampaignPlayerID);
            sParams.Add("@NewCampaignID", Master.CampaignID);
            dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtCampaignPlayers.Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
            }
            ddlSelectCharacterOrBankF1.Items.Clear();
            stStoredProc = "uspGetCampaignCharacters";
            DataTable dtCharacters = new DataTable();
            SortedList sParams1 = new SortedList();
            sParams1.Add("@CampaignID", Master.CampaignID);
            sParams1.Add("@CampaignPlayerID", CampaignPlayerID);
            dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams1, "LARPortal", Master.UserName, stCallingMethod);
            ddlSelectCharacterOrBankF1.DataTextField = "CharacterName";
            ddlSelectCharacterOrBankF1.DataValueField = "CharacterID";
            ddlSelectCharacterOrBankF1.DataSource = dtCharacters;
            ddlSelectCharacterOrBankF1.DataBind();
            if (dtCharacters.Rows.Count > 0)
            {
                ddlSelectCharacterOrBankF1.Items.Insert(0, new ListItem("Select Character", "0"));
                ddlSelectCharacterOrBankF1.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                ddlSelectCharacterOrBankF1.SelectedIndex = 0;
            }
            else
            {
                ddlSelectCharacterOrBankF1.Items.Insert(0, new ListItem("No characters, banking", "0"));
                ddlSelectCharacterOrBankF1.SelectedIndex = 0;
            }
        }

        protected void ddlSelectCharacterOrBankF1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            if (Convert.ToInt32(ddlSelectCharacterOrBankF1.SelectedValue) > 1)
            {
                hidInsertCharacterID.Value = ddlSelectCharacterOrBankF1.SelectedValue;
            }
            else
            {
                hidInsertCharacterID.Value = "0";
            }
            txtStaffComments.Focus();
        }

        protected void ddlSelectCharacterOrBankF2Load()
        {
            //Convert CampaignPlayerID using uspConvertCampaignPlayerID
            string stStoredProc = "uspConvertCampaignPlayerID";
            string stCallingMethod = "PointsAssign.aspx.ddlSelectCharacterOrBankF2Load";
            int iTemp = 0;
            int CampaignPlayerID = 0;
            if (int.TryParse(ddlCampaignPlayer.SelectedValue.ToString(), out iTemp))
                CampaignPlayerID = iTemp;
            DataTable dtCampaignPlayers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@OriginalCampaignPlayerID", CampaignPlayerID);
            sParams.Add("@NewCampaignID", Master.CampaignID);
            dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtCampaignPlayers.Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
            }
            ddlSelectCharacterOrBankF2.Items.Clear();
            stStoredProc = "uspGetCampaignCharacters";
            DataTable dtCharacters = new DataTable();
            SortedList sParams1 = new SortedList();
            sParams1.Add("@CampaignID", Master.CampaignID);
            sParams1.Add("@CampaignPlayerID", CampaignPlayerID);
            dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams1, "LARPortal", Master.UserName, stCallingMethod);
            ddlSelectCharacterOrBankF2.DataTextField = "CharacterName";
            ddlSelectCharacterOrBankF2.DataValueField = "CharacterID";
            ddlSelectCharacterOrBankF2.DataSource = dtCharacters;
            ddlSelectCharacterOrBankF2.DataBind();
            if (dtCharacters.Rows.Count > 0)
            {
                ddlSelectCharacterOrBankF2.Items.Insert(0, new ListItem("Select Character", "0"));
                ddlSelectCharacterOrBankF2.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                ddlSelectCharacterOrBankF2.SelectedIndex = 0;
            }
            else
            {
                ddlSelectCharacterOrBankF2.Items.Insert(0, new ListItem("No characters, banking", "0"));
                ddlSelectCharacterOrBankF2.SelectedIndex = 0;
            }
        }

        protected void ddlSelectCharacterOrBankF2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            if (Convert.ToInt32(ddlSelectCharacterOrBankF2.SelectedValue) > 1)
            {
                hidInsertCharacterID.Value = ddlSelectCharacterOrBankF2.SelectedValue;
            }
            else
            {
                hidInsertCharacterID.Value = "0";
            }
            txtStaffComments.Focus();
        }

        protected void ddlSelectCharacterOrBankF3Load()
        {
            //Convert CampaignPlayerID using uspConvertCampaignPlayerID
            string stStoredProc = "uspConvertCampaignPlayerID";
            string stCallingMethod = "PointsAssign.aspx.ddlSelectCharacterOrBankF3Load";
            int iTemp = 0;
            int CampaignPlayerID = 0;
            if (int.TryParse(ddlCampaignPlayer.SelectedValue.ToString(), out iTemp))
                CampaignPlayerID = iTemp;
            DataTable dtCampaignPlayers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@OriginalCampaignPlayerID", CampaignPlayerID);
            sParams.Add("@NewCampaignID", Master.CampaignID);
            dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtCampaignPlayers.Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
            }
            ddlSelectCharacterOrBankF3.Items.Clear();
            stStoredProc = "uspGetCampaignCharacters";
            DataTable dtCharacters = new DataTable();
            SortedList sParams1 = new SortedList();
            sParams1.Add("@CampaignID", Master.CampaignID);
            sParams1.Add("@CampaignPlayerID", CampaignPlayerID);
            dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams1, "LARPortal", Master.UserName, stCallingMethod);
            ddlSelectCharacterOrBankF3.DataTextField = "CharacterName";
            ddlSelectCharacterOrBankF3.DataValueField = "CharacterID";
            ddlSelectCharacterOrBankF3.DataSource = dtCharacters;
            ddlSelectCharacterOrBankF3.DataBind();
            if (dtCharacters.Rows.Count > 0)
            {
                ddlSelectCharacterOrBankF3.Items.Insert(0, new ListItem("Select Character", "0"));
                ddlSelectCharacterOrBankF3.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                ddlSelectCharacterOrBankF3.SelectedIndex = 0;
            }
            else
            {
                ddlSelectCharacterOrBankF3.Items.Insert(0, new ListItem("No characters, banking", "0"));
                ddlSelectCharacterOrBankF3.SelectedIndex = 0;
            }
        }

        protected void ddlSelectCharacterOrBankF3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            if (Convert.ToInt32(ddlSelectCharacterOrBankF3.SelectedValue) > 1)
            {
                hidInsertCharacterID.Value = ddlSelectCharacterOrBankF3.SelectedValue;
            }
            else
            {
                hidInsertCharacterID.Value = "0";
            }
            txtStaffComments.Focus();
        }

        protected void ddlSelectCharacterOrBankF4Load()
        {
            //Convert CampaignPlayerID using uspConvertCampaignPlayerID
            string stStoredProc = "uspConvertCampaignPlayerID";
            string stCallingMethod = "PointsAssign.aspx.ddlSelectCharacterOrBankF4Load";
            int iTemp = 0;
            int CampaignPlayerID = 0;
            int CampaignID = 0;
            if (int.TryParse(ddlCampaignPlayer.SelectedValue.ToString(), out iTemp))
                CampaignPlayerID = iTemp;
            int.TryParse(hidInsertDestinationCampaign.Value.ToString(), out CampaignID);
            if (CampaignID == 0)
            {
                if (int.TryParse(ddlSelectCharacterOrBankF4.SelectedValue.ToString(), out iTemp))
                    CampaignID = iTemp;
            }
            DataTable dtCampaignPlayers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@OriginalCampaignPlayerID", CampaignPlayerID);
            sParams.Add("@NewCampaignID", CampaignID);
            dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtCampaignPlayers.Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
            }
            ddlSelectCharacterOrBankF4.Items.Clear();
            stStoredProc = "uspGetCampaignCharacters";
            DataTable dtCharacters = new DataTable();
            SortedList sParams1 = new SortedList();
            sParams1.Add("@CampaignID", CampaignID);
            sParams1.Add("@CampaignPlayerID", CampaignPlayerID);
            dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams1, "LARPortal", Master.UserName, stCallingMethod);
            ddlSelectCharacterOrBankF4.DataTextField = "CharacterName";
            ddlSelectCharacterOrBankF4.DataValueField = "CharacterID";
            ddlSelectCharacterOrBankF4.DataSource = dtCharacters;
            ddlSelectCharacterOrBankF4.DataBind();
            if (dtCharacters.Rows.Count > 0)
            {
                ddlSelectCharacterOrBankF4.Items.Insert(0, new ListItem("Select Character", "0"));
                ddlSelectCharacterOrBankF4.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                ddlSelectCharacterOrBankF4.SelectedIndex = 0;
            }
            else
            {
                ddlSelectCharacterOrBankF4.Items.Insert(0, new ListItem("No characters, banking", "0"));
                ddlSelectCharacterOrBankF4.SelectedIndex = 0;
            }
        }

        protected void ddlSelectCharacterOrBankF4_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            if (Convert.ToInt32(ddlSelectCharacterOrBankF4.SelectedValue) > 1)
            {
                hidInsertCharacterID.Value = ddlSelectCharacterOrBankF4.SelectedValue;
            }
            else
            {
                hidInsertCharacterID.Value = "0";
            }
            txtStaffComments.Focus();
        }

        protected void ddlSelectCharacterOrBankF6Load()
        {
            //Convert CampaignPlayerID using uspConvertCampaignPlayerID
            string stStoredProc = "uspConvertCampaignPlayerID";
            string stCallingMethod = "PointsAssign.aspx.ddlSelectCharacterOrBankF6Load";
            int iTemp = 0;
            int CampaignPlayerID = 0;
            if (int.TryParse(ddlCampaignPlayer.SelectedValue.ToString(), out iTemp))
                CampaignPlayerID = iTemp;
            DataTable dtCampaignPlayers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@OriginalCampaignPlayerID", CampaignPlayerID);
            sParams.Add("@NewCampaignID", Master.CampaignID);
            dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtCampaignPlayers.Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
            }
            ddlSelectCharacterOrBankF6.Items.Clear();
            stStoredProc = "uspGetCampaignCharacters";
            DataTable dtCharacters = new DataTable();
            SortedList sParams1 = new SortedList();
            sParams1.Add("@CampaignID", Master.CampaignID);
            sParams1.Add("@CampaignPlayerID", CampaignPlayerID);
            dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams1, "LARPortal", Master.UserName, stCallingMethod);
            ddlSelectCharacterOrBankF6.DataTextField = "CharacterName";
            ddlSelectCharacterOrBankF6.DataValueField = "CharacterID";
            ddlSelectCharacterOrBankF6.DataSource = dtCharacters;
            ddlSelectCharacterOrBankF6.DataBind();
            if (dtCharacters.Rows.Count > 0)
            {
                ddlSelectCharacterOrBankF6.Items.Insert(0, new ListItem("Select Character", "0"));
                ddlSelectCharacterOrBankF6.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                ddlSelectCharacterOrBankF6.SelectedIndex = 0;
            }
            else
            {
                ddlSelectCharacterOrBankF6.Items.Insert(0, new ListItem("No characters, banking", "0"));
                ddlSelectCharacterOrBankF6.SelectedIndex = 0;
            }
        }

        protected void ddlSelectCharacterOrBankF6_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            if (Convert.ToInt32(ddlSelectCharacterOrBankF6.SelectedValue) > 1)
            {
                hidInsertCharacterID.Value = ddlSelectCharacterOrBankF6.SelectedValue;
            }
            else
            {
                hidInsertCharacterID.Value = "0";
            }
            txtStaffComments.Focus();
        }

        protected void AddPanelVisibility()
        {
            lblAddMessage.Text = "";
            string FlowStep = "A";
            if (Session["AddCPStep"] != null)
            {
                FlowStep = Session["AddCPStep"].ToString();
            }
            switch (FlowStep)
            {
                case "A":
                    pnlAddSourceCampaign.Visible = false;
                    pnlAddOpportunityDefault.Visible = false;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "B":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = false;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "C1":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "C6":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = false;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "D3":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = true;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "D6":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "E1":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = true;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "E2":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = true;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "E3":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = true;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "E4":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = true;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "E6":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = false;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = true;
                    break;

                case "F0":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = true;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "F1":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = true;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "F2":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = true;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "F3":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = true;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = true;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "F4":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = true;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = true;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "F5":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = true;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = true;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = true;
                    pnlAddNPCIncoming.Visible = false;
                    break;

                case "F6":
                    pnlAddSourceCampaign.Visible = true;
                    pnlAddOpportunityDefault.Visible = false;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = true;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = true;
                    break;

                default:
                    pnlAddSourceCampaign.Visible = false;
                    pnlAddOpportunityDefault.Visible = false;
                    pnlAddOpportunityDefaultC6.Visible = false;
                    pnlCPDestinationD3.Visible = false;
                    pnlCPDestinationD6.Visible = false;
                    pnlNPCCheckboxes.Visible = false;
                    pnlAddDonationCP.Visible = false;
                    pnlAddNonEventCP.Visible = false;
                    pnlAddPCLocalCP.Visible = false;
                    pnlAddNPCLocalCPStaying.Visible = false;
                    pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
                    pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
                    pnlAddNPCIncoming.Visible = false;
                    hidInsertCharacterID.Value = "0";
                    hidInsertCampaignCPOpportunityDefaultID.Value = "0";
                    hidInsertEventID.Value = "0";
                    hidInsertCampaignID.Value = "0";
                    hidInsertDescription.Value = "";
                    hidInsertOpportunityNotes.Value = "";
                    hidInsertExampleURL.Value = "";
                    hidInsertReasonID.Value = "0";
                    hidInsertStatusID.Value = "19";
                    hidInsertAddedByID.Value = "0";
                    hidInsertCPValue.Value = "0";
                    hidInsertApprovedByID.Value = "0";
                    hidInsertReceiptDate.Value = "";
                    hidInsertReceivedByID.Value = "0";
                    hidInsertCPAssignmentDate.Value = "Today";
                    hidInsertStaffComments.Value = "";
                    break;
            }
            DropdownListDefaultColors();
        }

        private void DropdownListDefaultColors()
        {
            if (ddlCampaignPlayer.SelectedIndex == 0)
                ddlCampaignPlayer.BackColor = System.Drawing.Color.Yellow;
            else
                ddlCampaignPlayer.BackColor = System.Drawing.Color.White;

            if (ddlAddSourceCampaign.SelectedIndex == 0)
                ddlAddSourceCampaign.BackColor = System.Drawing.Color.Yellow;
            else
                ddlAddSourceCampaign.BackColor = System.Drawing.Color.White;

            if (ddlAddOpportunityDefaultID.SelectedIndex == 0)
                ddlAddOpportunityDefaultID.BackColor = System.Drawing.Color.Yellow;
            else
                ddlAddOpportunityDefaultID.BackColor = System.Drawing.Color.White;

            if (ddlAddOpportunityDefaultIDC6.SelectedIndex == 0)
                ddlAddOpportunityDefaultIDC6.BackColor = System.Drawing.Color.Yellow;
            else
                ddlAddOpportunityDefaultIDC6.BackColor = System.Drawing.Color.White;

            if (ddlSourceEvent.SelectedIndex == 0)
                ddlSourceEvent.BackColor = System.Drawing.Color.Yellow;
            else
                ddlSourceEvent.BackColor = System.Drawing.Color.White;

            if (ddlDestinationCampaign.SelectedIndex == 0)
                ddlDestinationCampaign.BackColor = System.Drawing.Color.Yellow;
            else
                ddlDestinationCampaign.BackColor = System.Drawing.Color.White;

            if (ddlSelectCharacterOrBankF0.SelectedIndex == 0)
                ddlSelectCharacterOrBankF0.BackColor = System.Drawing.Color.Yellow;
            else
                ddlSelectCharacterOrBankF0.BackColor = System.Drawing.Color.White;

            if (ddlSelectCharacterOrBankF1.SelectedIndex == 0)
                ddlSelectCharacterOrBankF1.BackColor = System.Drawing.Color.Yellow;
            else
                ddlSelectCharacterOrBankF1.BackColor = System.Drawing.Color.White;

            if (ddlSelectCharacterOrBankF2.SelectedIndex == 0)
                ddlSelectCharacterOrBankF2.BackColor = System.Drawing.Color.Yellow;
            else
                ddlSelectCharacterOrBankF2.BackColor = System.Drawing.Color.White;

            if (ddlSelectCharacterOrBankF3.SelectedIndex == 0)
                ddlSelectCharacterOrBankF3.BackColor = System.Drawing.Color.Yellow;
            else
                ddlSelectCharacterOrBankF3.BackColor = System.Drawing.Color.White;

            if (ddlSelectCharacterOrBankF4.SelectedIndex == 0)
                ddlSelectCharacterOrBankF4.BackColor = System.Drawing.Color.Yellow;
            else
                ddlSelectCharacterOrBankF4.BackColor = System.Drawing.Color.White;

            if (ddlSelectCharacterOrBankF6.SelectedIndex == 0)
                ddlSelectCharacterOrBankF6.BackColor = System.Drawing.Color.Yellow;
            else
                ddlSelectCharacterOrBankF6.BackColor = System.Drawing.Color.White;

            if (ddlSourceEventPC.SelectedIndex == 0)
                ddlSourceEventPC.BackColor = System.Drawing.Color.Yellow;
            else
                ddlSourceEventPC.BackColor = System.Drawing.Color.White;
        }

        private void BuildCPAuditTable(int UserID)
        {
            // Stolen blatantly from MemberPointsView.aspx
            int CharacterID = 0;
            //string CampaignDDL = "";
            //if (Session["CampaignName"] != null)
            //    CampaignDDL = Session["CampaignName"].ToString();
            Classes.cTransactions CPAudit = new Classes.cTransactions();
            DataTable dtCPAudit = new DataTable();
            dtCPAudit = CPAudit.GetCPAuditList(UserID, Master.CampaignID, CharacterID);
            DataView dvPoints = new DataView(dtCPAudit, "", "", DataViewRowState.CurrentRows);
            gvPointsList.DataSource = dvPoints;
            gvPointsList.DataBind();
        }

        private void ddlAddCharacterLoad(string strUserName, int intCampaignID)
        {
            // Old - Can be deleted?
        }

        protected void txtOpportunityNotes_TextChanged(object sender, EventArgs e)
        {
            hidInsertOpportunityNotes.Value = txtOpportunityNotes.Text;
            txtReceiptDate.Focus();
        }

        protected void txtCPF0_TextChanged(object sender, EventArgs e)
        {
            hidInsertCPValue.Value = txtCPF0.Text;
            txtStaffComments.Focus();
        }

        protected void txtCPF1_TextChanged(object sender, EventArgs e)
        {
            hidInsertCPValue.Value = txtCPF1.Text;
            txtStaffComments.Focus();
        }

        protected void txtCPF2_TextChanged(object sender, EventArgs e)
        {
            hidInsertCPValue.Value = txtCPF2.Text;
            txtStaffComments.Focus();
        }

        protected void txtReceiptDate_TextChanged(object sender, EventArgs e)
        {
            hidInsertReceiptDate.Value = txtReceiptDate.Text;
        }





        protected void ResetHiddenValues()
        {
            hidInsertCharacterID.Value = "0";
            hidInsertCampaignCPOpportunityDefaultID.Value = "0";
            hidInsertEventID.Value = "0";
            hidInsertCampaignID.Value = "0";
            hidInsertDescription.Value = "";
            hidInsertOpportunityNotes.Value = "";
            hidInsertExampleURL.Value = "";
            hidInsertReasonID.Value = "0";
            hidInsertStatusID.Value = "21";
            hidInsertAddedByID.Value = Master.UserID.ToString();
            hidInsertCPValue.Value = "0";
            hidInsertApprovedByID.Value = Master.UserID.ToString();
            hidInsertReceiptDate.Value = DateTime.Now.ToString();
            hidInsertReceivedByID.Value = Master.UserID.ToString();
            hidInsertCPAssignmentDate.Value = DateTime.Now.ToString();
            hidInsertStaffComments.Value = "";
            // RPierce - 9/17/2016 - add to resets
            hidInsertCampaignCPOpportunityDefaultIDNPCEvent.Value = "";
            hidInsertCampaignCPOpportunityDefaultIDNPCPEL.Value = "";
            hidInsertCampaignCPOpportunityDefaultIDNPCSetup.Value = "";
            //hidInsertCampaignPlayerID.Value = "";
            hidInsertDescriptionNPCEvent.Value = "";
            hidInsertDescriptionNPCPEL.Value = "";
            hidInsertDescriptionNPCSetup.Value = "";
            hidInsertDestinationCampaign.Value = "";
            hidInsertDestinationCampaignLPType.Value = "";
            hidInsertReasonIDNPCEvent.Value = "";
            hidInsertReasonIDNPCPEL.Value = "";
            hidInsertReasonIDNPCSetup.Value = "";
            hidLastAddCPStep.Value = "";
            // End of add to resets
            txtStaffComments.Text = "";

        }

        protected void ResetDropDownListChoices()
        {
            try
            {
                ddlAddSourceCampaign.Items.Insert(0, new ListItem("Select Campaign", "0"));
                ddlAddSourceCampaign.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlAddOpportunityDefaultID.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlAddOpportunityDefaultIDC6.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlSourceEvent.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlDestinationCampaign.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlDonationTypes.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlSelectCharacterOrBankF0.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlSelectCharacterOrBankF1.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlSelectCharacterOrBankF2.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlSelectCharacterOrBankF3.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlSelectCharacterOrBankF4.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlSelectCharacterOrBankF6.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            try
            {
                ddlSourceEventPC.SelectedIndex = 0;
            }
            catch
            {
                // If I haven't set up this particular ddl yet I don't care, get on with it.
            }
            chkNPCEvent.Checked = true;
            txtNPCEvent.Text = "";
            chkPEL.Checked = true;
            txtPEL.Text = "";
            chkSetupCleanup.Checked = true;
            txtSetupCleanup.Text = "";
            txtOpportunityNotes.Text = "";
            txtReceiptDate.Text = "";
            txtCPF0.Text = "0";
            txtCPF1.Text = "0";
            txtCPF2.Text = "0";
            // Make all the appopriate panels invisible again
            //pnlAddSourceCampaign.Visible = false;
            pnlAddOpportunityDefault.Visible = false;
            pnlAddOpportunityDefaultC6.Visible = false;
            pnlCPDestinationD6.Visible = false;
            pnlNPCCheckboxes.Visible = false;
            pnlCPDestinationD3.Visible = false;
            pnlAddDonationCP.Visible = false;
            pnlAddNonEventCP.Visible = false;
            pnlAddPCLocalCP.Visible = false;
            pnlAddNPCLocalCPStaying.Visible = false;
            pnlAddNPCLocalCPGoingToLARPPortalCampaign.Visible = false;
            pnlAddNPCLocalCPGoingToNonLARPPortalCampaign.Visible = false;
            pnlAddNPCIncoming.Visible = false;
            //pnlAddStaffComments.Visible = false;
        }

        protected void ddlCampaignPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdownListDefaultColors();
            ResetHiddenValues();
            Session["AddCPStep"] = "A";
            hidLastAddCPStep.Value = "A";
            ddlAddSourceCampaignLoad(Master.UserName, Master.CampaignID, ddlCampaignPlayer.SelectedValue);
            Session["AddCPStep"] = "B";
            hidLastAddCPStep.Value = "B";
            AddPanelVisibility();
            FillHiddenCampaignPlayerUserID(Master.CampaignID, ddlCampaignPlayer.SelectedValue);
            hidInsertCampaignPlayerID.Value = ddlCampaignPlayer.SelectedValue;
        }

        protected void FillHiddenCampaignPlayerUserID(int CampaignID, string strCampaignPlayerID)
        {
            string stStoredProc = "uspGetCampaignPlayers";
            string stCallingMethod = "PointsAssign.aspx.FillHiddenCampaignPlayerUserID";
            int CampaignPlayerID = 0;
            int UserID = 0;
            int.TryParse(strCampaignPlayerID.ToString(), out CampaignPlayerID);
            DataTable dtUsers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", CampaignID);
            sParams.Add("@CampaignPlayerID", CampaignPlayerID);
            dtUsers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtUsers.Rows)
            {
                int.TryParse(dRow["UserID"].ToString(), out UserID);
                hidCampaignPlayerUserID.Value = UserID.ToString();
                BuildCPAuditTable(UserID);
            }
        }

        protected void FillHiddenCampaignPlayerUserIDNonCP(int CampaignID, int strCampaignPlayerID)
        {
            string stStoredProc = "uspGetCampaignPlayers";
            string stCallingMethod = "PointsAssign.aspx.FillHiddenCampaignPlayerUserIDNonCP";
            int PoolID = 0;
            int.TryParse(ddlPointType.SelectedValue.ToString(), out PoolID);
            int CampaignPlayerID = 0;
            int UserID = 0;
            int.TryParse(strCampaignPlayerID.ToString(), out CampaignPlayerID);
            DataTable dtUsers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", CampaignID);
            sParams.Add("@CampaignPlayerID", CampaignPlayerID);
            dtUsers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtUsers.Rows)
            {
                int.TryParse(dRow["UserID"].ToString(), out UserID);
                hidCampaignPlayerUserID.Value = UserID.ToString();
                BuildNonCPAuditTable(PoolID, UserID);
            }
        }

        protected void ddlAddSourceCampaignLoad(string strUserName, int CurrentCampaignID, string strCampaignPlayerID)
        {
            ddlAddSourceCampaign.Items.Clear();
            string stStoredProc = "uspGetSourceCampaigns";
            string stCallingMethod = "PointsAssign.aspx.ddlAddSourceCampaignLoad";
            int CampaignPlayerID = 0;
            int iTemp;
            if (int.TryParse(strCampaignPlayerID.ToString(), out iTemp))
            {
                CampaignPlayerID = iTemp;
            }
            DataTable dtSourceCampaigns = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", CurrentCampaignID);
            sParams.Add("@CampaignPlayerID", CampaignPlayerID);
            dtSourceCampaigns = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", strUserName, stCallingMethod);
            ddlAddSourceCampaign.DataTextField = "CampaignName";
            ddlAddSourceCampaign.DataValueField = "CampaignID";
            ddlAddSourceCampaign.DataSource = dtSourceCampaigns;
            ddlAddSourceCampaign.DataBind();
            ddlAddSourceCampaign.Items.Insert(0, new ListItem("Select Campaign", "0"));
            ddlAddSourceCampaign.SelectedIndex = 0;
        }

        protected void ddlAddSourceCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAddSourceCampaign.Items.RemoveAt(0);
            DropdownListDefaultColors();
            Session["AddCPStep"] = "B";
            hidLastAddCPStep.Value = "B";
            AddPanelVisibility();
            // If pick current campaign follow step c1, else step E6 - For now assume incoming CP is from NPCing another campaign
            int FromCampaignID = 0;
            //int CurrentCampaignID = 0;
            //int.TryParse(hidCampaignID.Value.ToString(), out CurrentCampaignID);
            int Reason = 0;
            double CPValue = 0;
            double EventCP = 0;
            double PELCP = 0;
            double SetupCleanupCP = 0;
            double ExchangeMultiplier = 1;
            hidInsertCampaignID.Value = ddlAddSourceCampaign.SelectedValue;
            // Determine current campaign's point values for NPCing tasks
            string stStoredProc = "uspGetCampaignCPOpportunityDefaults";
            string stCallingMethod = "PointsAssign.aspx.ddlAddSourceCampaign_SelectedIndexChanged";
            DataTable dtLocalNPCPoints = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@NPCOnly", 1);
            // Rick - 11/27/2021 - Change to campaign getting the points, not necessarily the current campaign.
            sParams.Add("@CampaignID", ddlAddSourceCampaign.SelectedValue);
            //sParams.Add("@CampaignID", Master.CampaignID);
            dtLocalNPCPoints = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
            foreach (DataRow dRow in dtLocalNPCPoints.Rows)
            {
                int.TryParse(dRow["ReasonID"].ToString(), out Reason);
                double.TryParse(dRow["CPValue"].ToString(), out CPValue);
                if (Reason == 1)
                {
                    txtNPCEvent.Text = CPValue.ToString();
                    EventCP = CPValue;
                    hidInsertDescriptionNPCEvent.Value = dRow["Description"].ToString();
                    hidInsertReasonIDNPCEvent.Value = Reason.ToString();
                    hidInsertCampaignCPOpportunityDefaultIDNPCEvent.Value = dRow["CampaignCPOpportunityDefaultID"].ToString();
                }
                if (Reason == 13)
                {
                    txtPEL.Text = CPValue.ToString();
                    PELCP = CPValue;
                    hidInsertDescriptionNPCPEL.Value = dRow["Description"].ToString();
                    hidInsertReasonIDNPCPEL.Value = Reason.ToString();
                    hidInsertCampaignCPOpportunityDefaultIDNPCPEL.Value = dRow["CampaignCPOpportunityDefaultID"].ToString();
                }
                if (Reason == 17)
                {
                    txtSetupCleanup.Text = CPValue.ToString();
                    SetupCleanupCP = CPValue;
                    hidInsertDescriptionNPCSetup.Value = dRow["Description"].ToString();
                    hidInsertReasonIDNPCSetup.Value = Reason.ToString();
                    hidInsertCampaignCPOpportunityDefaultIDNPCSetup.Value = dRow["CampaignCPOpportunityDefaultID"].ToString();
                }

            }
            FromCampaignID = Convert.ToInt32(ddlAddSourceCampaign.SelectedValue);
            if (FromCampaignID == Master.CampaignID)
            {
                Session["AddCPStep"] = "C1";
                hidLastAddCPStep.Value = "C1";
                ddlAddOpportunityDefaultIDLoad(Master.CampaignID);
            }
            else
            {
                Session["AddCPStep"] = "E6";
                hidLastAddCPStep.Value = "E6";
                // Determine NPC check box values - Lookup values in campaign exchange table
                stStoredProc = "uspGetCampaignExchangeValues";
                DataTable dtPointsExchange = new DataTable();
                sParams.Clear();
                sParams.Add("@NPCOnly", 1);
                sParams.Add("@ToCampaign", Master.CampaignID);
                sParams.Add("@FromCampaign", FromCampaignID);
                dtPointsExchange = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", Master.UserName, stCallingMethod);
                // Now go through the returned values and modify the check box values
                foreach (DataRow dRow2 in dtPointsExchange.Rows)
                {
                    int.TryParse(dRow2["ReasonID"].ToString(), out Reason);
                    double.TryParse(dRow2["ExchangeMultiplier"].ToString(), out ExchangeMultiplier);
                    if (Reason == 1)
                    {
                        txtNPCEvent.Text = (EventCP * ExchangeMultiplier).ToString();
                        hidInsertReasonIDNPCEvent.Value = Reason.ToString();
                    }
                    if (Reason == 13)
                    {
                        txtPEL.Text = (PELCP * ExchangeMultiplier).ToString();
                        hidInsertReasonIDNPCPEL.Value = Reason.ToString();
                    }
                    if (Reason == 17)
                    {
                        txtSetupCleanup.Text = (SetupCleanupCP * ExchangeMultiplier).ToString();
                        hidInsertReasonIDNPCSetup.Value = Reason.ToString();
                    }
                }
                ddlSourceEventLoad();
                ddlSelectCharacterOrBankF6Load();
            }
            //ddlSelectCharacterOrBankF6Load();
            AddPanelVisibility();
        }


        protected void btnAssignAll_Click(object sender, EventArgs e)
        {
            // Rick - 12/10/2016 - Hide button, show processing label, when done unhide button and rehide processing label
            btnAssignAll.Visible = false;
            lblAssignAll.Visible = true;

            foreach (GridViewRow gvrow in gvPoints.Rows)
            {
                try
                {

                    int index = gvPoints.EditIndex;
                    int iTemp;
                    double dblTemp = 0;
                    double CP = 0;
                    HiddenField hidCPOpp = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidPointID");
                    HiddenField hidCmpPlyrID = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidCampaignPlayer");
                    HiddenField hidCharID = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidCharacterID");
                    HiddenField hidEvntID = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidEventID");
                    HiddenField hidOppDefID = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidCPOpportunityDefaultID");
                    HiddenField hidRsnID = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidReasonID");
                    HiddenField hidAddID = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidAddedByID");
                    HiddenField hidOppNotes = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidOpportunityNotes");
                    HiddenField hidExURL = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidExampleURL");
                    HiddenField hidRole = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidRole");
                    HiddenField hidNPCCampaignID = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidNPCCampaignID");
                    HiddenField hidRegistrationID = (HiddenField)gvPoints.Rows[gvrow.RowIndex].FindControl("hidRegistrationID");
                    Label lblEarnDesc = (Label)gvPoints.Rows[gvrow.RowIndex].FindControl("lblEarnDescription");
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
                    string strOppNotes = hidOppNotes.Value.ToString();
                    string strExURL = hidExURL.Value.ToString();
                    string strDesc = lblEarnDesc.Text;
                    if (int.TryParse(hidCmpPlyrID.Value.ToString(), out iTemp))
                        intCmpPlyrID = iTemp;
                    //if (int.TryParse(hidCharID.Value.ToString(), out iTemp))
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
                    if (int.TryParse(hidRole.Value.ToString(), out iTemp))
                        intRole = iTemp;
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
                        Label lblCPValue = (Label)gvPoints.Rows[gvrow.RowIndex].FindControl("lblCPValue");
                        Label lblStaffComents = (Label)gvPoints.Rows[gvrow.RowIndex].FindControl("lblStaffComments");
                        if (double.TryParse(lblCPValue.Text, out dblTemp))
                            CP = dblTemp;
                        if(lblStaffComents == null)
                        {
                            strComments = "";
                        }
                        else
                        {
                            strComments = lblStaffComents.Text;
                        }
                        

                    }
                    Classes.cPoints Point = new Classes.cPoints();
                    //  JB  10/2/2021  Added points from parameter to try and help with debugging.
                    Point.UpdateCPOpportunity(Master.UserID, intCPOpp, intCmpPlyrID, intCharID, intOppDefID, intEvntID,
                        strDesc, strOppNotes, strExURL, intRsnID, intAddID, CP, Master.UserID,
                        DateTime.Now, Master.UserID, strComments, intRole, intNPCCampaignID, Master.CampaignID, "PointsAssign.aspx.cs.btnAssignAll_Click");
                }
                catch (Exception ex)
                {
                    string l = ex.Message;
                }
            }
            gvPoints.EditIndex = -1;

            // Rick - 12/10/2016 Show processing complete pop-up and restore assign all button and label to default state
            string jsString = "alert('All points applied.');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                    "MyApplication",
                    jsString,
                    true);
            btnAssignAll.Visible = true;
            lblAssignAll.Visible = false;
            FillGrid();
        }

        protected void ddlCampaignPlayerLoad(string strUserName, int intCampaignID)
        {
            ddlCampaignPlayer.Items.Clear();
            string stStoredProc = "uspGetCampaignPlayers";
            string stCallingMethod = "PointsAssign.aspx.ddlCampaignPlayerLoad";
            DataTable dtPlayers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", intCampaignID);
            dtPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", strUserName, stCallingMethod);
            ddlCampaignPlayer.DataTextField = "PlayerName";
            ddlCampaignPlayer.DataValueField = "CampaignPlayerID";
            ddlCampaignPlayer.DataSource = dtPlayers;
            ddlCampaignPlayer.DataBind();
            ddlCampaignPlayer.Items.Insert(0, new ListItem("Select Player", "0"));
        }

        protected void ddlCampaignPlayerLoadNonCP(string strUserName, int intCampaignID)
        {
            ddlCampaignPlayerNonCP.Items.Clear();
            string stStoredProc = "uspGetCampaignPlayers";
            string stCallingMethod = "PointsAssign.aspx.ddlCampaignPlayerLoadNonCP";
            DataTable dtPlayers = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", intCampaignID);
            dtPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", strUserName, stCallingMethod);
            ddlCampaignPlayerNonCP.DataTextField = "PlayerName";
            ddlCampaignPlayerNonCP.DataValueField = "CampaignPlayerID";
            ddlCampaignPlayerNonCP.DataSource = dtPlayers;
            ddlCampaignPlayerNonCP.DataBind();
            ddlCampaignPlayerNonCP.Items.Insert(0, new ListItem("Select Player", "0"));
        }

        protected void ddlPointType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool blUniversalPoint = true;
            bool bTemp;
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            int intPoolID;
            int.TryParse(ddlPointType.SelectedValue.ToString(), out intPoolID);
            SortedList sParams = new SortedList();
            sParams.Add("@PoolID", intPoolID);
            DataTable dtPools = Classes.cUtilities.LoadDataTable("uspGetCampaignPoolByID", sParams, "LARPortal", Master.UserName, lsRoutineName + "uspGetCampaignPoolByID");
            foreach (DataRow drow in dtPools.Rows)
            {
                if (bool.TryParse(drow["DefaultPool"].ToString(), out bTemp))
                {
                    blUniversalPoint = bTemp;
                }
            }
            if (blUniversalPoint == true)
            {
                //populate vwPointList
                pnlAddNewCP.Visible = true;
                pnlCharacterPointDisplay.Visible = true;
                pnlAssignExisting.Visible = false;
                pnlAddHeader.Visible = true;
                pnlAssignHeader.Visible = false;
                pnlAddNewNonCP.Visible = false;
                pnlCharacterNonCPDisplay.Visible = false;
            }
            else
            {
                //populate vwNonCPList
                pnlAddNewCP.Visible = false;
                pnlCharacterPointDisplay.Visible = false;
                pnlAssignExisting.Visible = false;
                pnlAddHeader.Visible = true;
                pnlAssignHeader.Visible = false;
                pnlAddNewNonCP.Visible = true;
                pnlCharacterNonCPDisplay.Visible = true;
                lblTotalNonCP.Text = "Total Points - " + ddlPointType.SelectedItem.Text;
                ddlCampaignPlayerLoadNonCP(Master.UserName, Master.CampaignID);
            }
        }

        protected void gvPoints_Sorting(object sender, GridViewSortEventArgs e)
        {

            string sField = ViewState["SortField"].ToString();
            string sDir = ViewState["SortDir"].ToString();

            if (e.SortExpression == sField)
            {
                if (String.IsNullOrEmpty(sDir))
                    ViewState["SortDir"] = "DESC";
                else
                    ViewState["SortDir"] = "";
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDir"] = "";
            }

            FillGrid();
        }

    }
}