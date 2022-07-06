using LarpPortal.Classes;
using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LarpPortal.Points
{
    public partial class MemberPointsView : System.Web.UI.Page
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

            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Session["ActiveLeftNav"] = "PointsView";
            if (!IsPostBack || ForceReload)
            {
                ShowCPTransfer(Master.CampaignID);
                ddlPointTypeLoad(Master.CampaignID);
                BuildCPAuditTable(Master.UserID);
            }
        }
        private void ShowCPTransfer(int intCampaignID)
        {

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Classes.cCampaignBase campaign = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);
            if (campaign.AllowCPDonation)
            {
                Session["AllowCPTransfer"] = "true";
                chkApplyTo.Visible = true;
            }
            else
            {
                Session["AllowCPTransfer"] = "false";
                chkApplyTo.Checked = false;
                chkApplyTo.Visible = false;
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

        private void BuildCPAuditTable(int UserID)
        {
            if (chkApplyTo.Checked)
            {
                //gvPointsList.Columns[12].Visible = false;
                //gvPointsList.Columns[13].Visible = false;
                //gvPointsList.Columns[14].Visible = true;
                //gvPointsList.Columns[15].Visible = true;
            }
            else
            {
                //gvPointsList.Columns[12].Visible = true;
                //gvPointsList.Columns[13].Visible = true;
                //gvPointsList.Columns[14].Visible = false;
                //gvPointsList.Columns[15].Visible = false;
            }

            int CampaignID = 0;
            int CharacterID = 0;
            CampaignID = Master.CampaignID;
            bool AllowCPDonation = false;
            string CampaignDDL = "";

            Classes.cCampaignBase Campaign = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);
            AllowCPDonation = Campaign.AllowCPDonation;

            CampaignDDL = Master.CampaignName;
            Classes.cTransactions CPAudit = new Classes.cTransactions();
            DataTable dtCPAudit = new DataTable();
            dtCPAudit = CPAudit.GetCPAuditList(UserID, CampaignID, CharacterID);
            DataView dvPoints = new DataView(dtCPAudit, "", "", DataViewRowState.CurrentRows);
            gvPointsList.DataSource = dvPoints;
            gvPointsList.DataBind();
            if (dvPoints.Count == 0)
            {
                mvPointList.SetActiveView(vwPointlessList);
            }
            else
            {
                mvPointList.SetActiveView(vwPointList);
            }
        }

        protected void gvPointsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find the status (fifth column 0 based).  If 'Banked' make the ddl and button visible
                string stat = e.Row.Cells[4].Text;
                string spentat = e.Row.Cells[8].Text;
                string currentcampaign = "";
                if (Session["CampaignName"] != null)
                    currentcampaign = Session["CampaignName"].ToString();
                string currentcampaignid = Session["CampaignID"].ToString();
                if (stat != "Banked" || spentat != currentcampaign)
                {
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false; 
                }
                else
                {
                    int iTemp = 0;
                    int CampaignID = 0;
                    int CampaignPlayerID = 0;
                    int UserID = 0;
                    int index = gvPointsList.EditIndex;
                    if (chkApplyTo.Checked)
                    {
                        DropDownList ddlDropDownList = (DropDownList)e.Row.FindControl("ddlCharacters");
                        //Button btnApply = (Button)e.Row.FindControl("btnApplyBanked");
                        HiddenField hidCmpPlyrID = (HiddenField)e.Row.FindControl("hidSentToCampaignPlayerID");
                        //btnApply.Text = "Transfer";
                        //ddlDropDownList.Text = "Transfer To";
                        int.TryParse(Session["CampaignID"].ToString(), out CampaignID);
                        int.TryParse(e.Row.Cells[14].Text.ToString(), out CampaignPlayerID);
                        if (int.TryParse(hidCmpPlyrID.Value.ToString(), out iTemp))
                            CampaignPlayerID = iTemp;
                        int.TryParse(Session["UserID"].ToString(), out UserID);
                        string stStoredProc = "uspGetCampaignPlayersBySinglePlayer";     // Returns PlayerName and CampaignPlayerID ORDER BY LastName, FirstName
                        string stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowDataBound";
                        DataTable dtPlayers = new DataTable();
                        SortedList sParams = new SortedList();
                        sParams.Add("@CampaignPlayerID", CampaignPlayerID);
                        dtPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", UserID.ToString(), stCallingMethod);
                        ddlDropDownList.DataTextField = "PlayerName";
                        ddlDropDownList.DataValueField = "CampaignPlayerID";
                        ddlDropDownList.DataSource = dtPlayers;
                        ddlDropDownList.DataBind();
                        if (dtPlayers.Rows.Count > 0)
                        {
                            //ddlDropDownList.Items.Insert(0, new ListItem("Select Character", "0"));
                            //ddlDropDownList.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                            //ddlDropDownList.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlDropDownList.Items.Insert(0, new ListItem("No players to transfer to", "0"));
                            ddlDropDownList.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        DropDownList ddlDropDownList = (DropDownList)e.Row.FindControl("ddlCharacters");
                        //Button btnApply = (Button)e.Row.FindControl("btnApplyBanked");
                        HiddenField hidCmpPlyrID = (HiddenField)e.Row.FindControl("hidSentToCampaignPlayerID");
                        //btnApply.Text = "Transfer";
                        //ddlDropDownList.Text = "Apply To";
                        int.TryParse(Session["CampaignID"].ToString(), out CampaignID);
                        int.TryParse(e.Row.Cells[14].Text.ToString(), out CampaignPlayerID);
                        if (int.TryParse(hidCmpPlyrID.Value.ToString(), out iTemp))
                            CampaignPlayerID = iTemp;
                        int.TryParse(Session["UserID"].ToString(), out UserID);
                        string stStoredProc = "uspGetCampaignCharacters";
                        string stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowDataBound";
                        DataTable dtCharacters = new DataTable();
                        SortedList sParams = new SortedList();
                        sParams.Add("@CampaignID", CampaignID);
                        sParams.Add("@CampaignPlayerID", CampaignPlayerID);
                        dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", UserID.ToString(), stCallingMethod);
                        ddlDropDownList.DataTextField = "PointAppChooser";
                        ddlDropDownList.DataValueField = "CharacterID";
                        ddlDropDownList.DataSource = dtCharacters;
                        ddlDropDownList.DataBind();
                        if (dtCharacters.Rows.Count > 0)
                        {
                            //ddlDropDownList.Items.Insert(0, new ListItem("Select Character", "0"));
                            //ddlDropDownList.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                            //ddlDropDownList.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlDropDownList.Items.Insert(0, new ListItem("No characters to apply", "0"));
                            ddlDropDownList.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        protected void gvPointsList_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ApplyBanked")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPointsList.Rows[index];
                DropDownList ddl = (DropDownList)gvPointsList.Rows[index].FindControl("ddlCharacters");
                string stStoredProc = "";
                string stCallingMethod = "";
                SortedList sParams = new SortedList();
                int iTemp = 0;
                double dTemp = 0;
                int CharacterID = 0;
                double TotalCP = 0;
                double TotalCharacterCap = 0;
                double RemainingCP = 0;
                double BankedCP = 0;
                double ToBankCP = 0;
                int CampaignID = 0;
                int CampaignPlayerID = 0;
                int ToCampaignPlayerID = 0;
                int UserID = 0;
                int RowCharacterID = 0;
                int PlayerCPAuditID = 0;
                double RowTotalCP = 0;

                int.TryParse(Session["UserID"].ToString(), out UserID);

                if (chkApplyTo.Checked)
                {
                    // Transfer CP to another player

                    //Apply and display applied message
                    stStoredProc = "uspTransferBankedPoints";
                    stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowCommand1";
                    sParams.Clear();
                    HiddenField hidCPAuditID = (HiddenField)row.FindControl("hidPlayerCPAuditID");
                    if (int.TryParse(hidCPAuditID.Value.ToString(), out iTemp))
                        PlayerCPAuditID = iTemp;
                    HiddenField hidCmpPlyrID = (HiddenField)row.FindControl("hidSentToCampaignPlayerID");
                    if (int.TryParse(hidCmpPlyrID.Value.ToString(), out iTemp))
                        CampaignPlayerID = iTemp;
                    if (int.TryParse(ddl.SelectedValue.ToString(), out iTemp))
                        ToCampaignPlayerID = iTemp;
                    if (double.TryParse(row.Cells[3].Text, out dTemp))
                        BankedCP = dTemp;
                    sParams.Add("@UserID", UserID);
                    sParams.Add("@CampaignPlayerID", CampaignPlayerID);
                    sParams.Add("@CampaignID", Master.CampaignID);
                    sParams.Add("@PlayerCPAuditID", PlayerCPAuditID);
                    sParams.Add("@CPToCampaignPlayerID", ToCampaignPlayerID);
                    sParams.Add("@CPValue", BankedCP);
                    cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", UserID.ToString());
                    string jsString = "alert('Banked points have been transferred.');";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                            "MyApplication",
                            jsString,
                            true);
                    // Refresh page
                    BuildCPAuditTable(UserID);
                }
                else
                {

                    if (int.TryParse(ddl.SelectedValue.ToString(), out iTemp))
                        CharacterID = iTemp;
                    HiddenField hidTotalCharacterCapForCampaign = (HiddenField)row.FindControl("hidTotalCharacterCap");
                    HiddenField hidCmpPlyrID = (HiddenField)row.FindControl("hidSentToCampaignPlayerID");
                    HiddenField hidCPAuditID = (HiddenField)row.FindControl("hidPlayerCPAuditID");
                    int.TryParse(Session["CampaignID"].ToString(), out CampaignID);
                    if (int.TryParse(hidCmpPlyrID.Value.ToString(), out iTemp))
                        CampaignPlayerID = iTemp;
                    if (int.TryParse(hidCPAuditID.Value.ToString(), out iTemp))
                        PlayerCPAuditID = iTemp;

                    stStoredProc = "uspGetCampaignCharacters";
                    stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowCommand2";
                    DataTable dtCharacters = new DataTable();
                    sParams.Add("@CampaignID", CampaignID);
                    sParams.Add("@CampaignPlayerID", CampaignPlayerID);
                    dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", UserID.ToString(), stCallingMethod);
                    foreach (DataRow dRow in dtCharacters.Rows)
                    {
                        if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                            RowCharacterID = iTemp;
                        if (double.TryParse(dRow["TotalCP"].ToString(), out dTemp))
                            RowTotalCP = dTemp;
                        if (RowCharacterID == CharacterID)
                            TotalCP = RowTotalCP;
                    }
                    if (double.TryParse(hidTotalCharacterCapForCampaign.Value.ToString(), out dTemp))
                        TotalCharacterCap = dTemp;
                    RemainingCP = TotalCharacterCap - TotalCP;
                    if (double.TryParse(row.Cells[3].Text, out dTemp))
                        BankedCP = dTemp;
                    ToBankCP = BankedCP - RemainingCP;
                    if (RemainingCP - BankedCP >= 0)
                    {
                        //Apply and display applied message
                        stStoredProc = "uspApplyBankedPoints";
                        stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowCommand3";
                        sParams.Clear();
                        sParams.Add("@UserID", UserID);
                        sParams.Add("@CharacterID", CharacterID);
                        sParams.Add("@PlayerCPAuditID", PlayerCPAuditID);
                        cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", UserID.ToString());
                        string jsString = "alert('Banked points have been applied.');";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                                "MyApplication",
                                jsString,
                                true);
                        // Refresh page
                        BuildCPAuditTable(UserID);
                    }
                    else
                    {
                        if (RemainingCP > 0)
                        {
                            // This is where we need to split the line into balance you can apply and the leftover and then apply the line.
                            stStoredProc = "uspApplyBankedPointsSplit";
                            stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowCommand4";
                            sParams.Clear();
                            sParams.Add("@UserID", UserID);
                            sParams.Add("@CharacterID", CharacterID);
                            sParams.Add("@PlayerCPAuditID", PlayerCPAuditID);
                            sParams.Add("@PointsToApply", RemainingCP);
                            sParams.Add("@PointsToBank", ToBankCP);
                            cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", UserID.ToString());
                            string jsString = "alert('Banked points have been applied up to the cap. The balance has been banked.');";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                                    "MyApplication",
                                    jsString,
                                    true);
                            // Refresh page
                            BuildCPAuditTable(UserID);
                        }
                        else
                        {
                            //Leave banked and display at max message
                            string jsString = "alert('This character is already at maximum allowed points.');";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                                    "MyApplication",
                                    jsString,
                                    true);
                        }

                    }
                }
            }
        }

        protected void btnApplyBanked_Click(object sender, EventArgs e)
        {

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
                BuildCPAuditTable(Master.UserID);
                if (Session["AllowCPTransfer"].ToString() == "true")
                {
                    chkApplyTo.Visible = true;
                }
                else
                {
                    chkApplyTo.Checked = false;
                    chkApplyTo.Visible = false;
                }

            }
            else
            {
                //populate vwNonCPList
                BuildNonCPAuditTable(intPoolID);
                chkApplyTo.Checked = false;
                chkApplyTo.Visible = false;
            }

        }

        private void BuildNonCPAuditTable(int PoolID)
        {
            int CampaignID = 0;
            int CharacterID = 0;
            CampaignID = Master.CampaignID;

            string CampaignDDL = "";
            CampaignDDL = Master.CampaignName;
            Classes.cTransactions CPAudit = new Classes.cTransactions();
            DataTable dtCPAudit = new DataTable();
            dtCPAudit = CPAudit.GetNonCPAuditList(Master.UserID, PoolID, CharacterID);
            DataView dvPoints = new DataView(dtCPAudit, "", "", DataViewRowState.CurrentRows);
            gvNonCPList.DataSource = dvPoints;
            gvNonCPList.DataBind();
            if (dvPoints.Count == 0)
            {
                mvPointList.SetActiveView(vwPointlessList);
            }
            else
            {
                mvPointList.SetActiveView(vwNonCPList);
            }
        }


        protected void gvNonCPList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvNonCPList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void chkApplyTo_CheckedChanged(object sender, EventArgs e)
        {
            BuildCPAuditTable(Master.UserID);
        }
    }
}