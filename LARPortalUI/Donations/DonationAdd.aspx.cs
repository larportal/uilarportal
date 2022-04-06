using LarpPortal.Classes;
using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Donations
{

    public partial class DonationAdd : System.Web.UI.Page
    {

        private int CurrentEvent;
        private int StatusID;

        protected void Page_Load(object sender, EventArgs e)
        {
            CampaignDonationSettings();
            StatusID = int.Parse(hidStatusID.Value);
            ddlDonationTypeLoad();
            if (hidDefaultPoolDescription.Value is null)
            {
                RU.Text = "";
                RUEdit.Text = "";
            }
            else
            {
                RU.Text = hidDefaultPoolDescription.Value.ToString();
                RUEdit.Text = hidDefaultPoolDescription.Value.ToString();
            }
            if (!IsPostBack)
            {

            }

        }

        protected void ddlDonationTypeLoad()
        {
            // Load both ddlDonationTypes
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlDonationType.Items.Clear();
            SortedList sParams = new SortedList();
            //sParams.Add("@CampaignID", Master.CampaignID);
            DataTable dtDonationType = Classes.cUtilities.LoadDataTable("uspGetDonationTypes", sParams, "LARPortal", Master.UserName, lsRoutineName + "ddlDonationTypeLoad");
            ddlDonationType.DataTextField = "DonationTypeDescription";
            ddlDonationType.DataValueField = "DonationTypeID";
            ddlDonationType.DataSource = dtDonationType;
            ddlDonationType.DataBind();
            ddlDonationTypeEdit.DataTextField = "DonationTypeDescription";
            ddlDonationTypeEdit.DataValueField = "DonationTypeID";
            ddlDonationTypeEdit.DataSource = dtDonationType;
            ddlDonationTypeEdit.DataBind();
        }

        protected void CampaignDonationSettings()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Classes.cDonation DonationSettings = new Classes.cDonation();
            DonationSettings.GetDonationCampaignSettings(Master.CampaignID);
            hidAdd1.Value = DonationSettings.DefaultShipToAdd1.ToString();
            hidAdd2.Value = DonationSettings.DefaultShipToAdd2.ToString();
            hidCity.Value = DonationSettings.DefaultShipToCity.ToString();
            hidState.Value = DonationSettings.DefaultShipToState.ToString();
            hidZip.Value = DonationSettings.DefaultShipToPostalCode.ToString();
            hidPhone.Value = DonationSettings.DefaultShipToPhone.ToString();
            hidEmail.Value = DonationSettings.DefaultNotificationEmail.ToString();
            hidDefaultPoolDescription.Value = DonationSettings.PoolDescription.ToString();
            hidDefaultRewardUnitID.Value = DonationSettings.DonationRewardUnitID.ToString();
            hidStatusID.Value = DonationSettings.StatusID.ToString();
            hidStatusDescription.Value = DonationSettings.StatusDescription.ToString();
            hidDefaultAwardWhen.Value = DonationSettings.DefaultAwardWhen.ToString();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (!IsPostBack)
            {
                if (Request.QueryString["EventID"] == null)
                {
                    Response.Redirect("SetupDonations.aspx", true);
                }
                else
                {
                    CurrentEvent = int.Parse(Request.QueryString["EventID"].ToString());
                    Session["CurrentEvents"] = Request.QueryString["EventID"].ToString();
                    SetEventDate(CurrentEvent);
                }
                //int.Parse(Request.QueryString["EventID"].ToString());
                tbReqdBy.Text = Session["EventDate"].ToString();
                lblEventInfo.Text = "Event Date: " + Session["EventDate"].ToString();

            }
            FillGrid();
        }

        private void SetEventDate(int EventID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            string storedproc = "uspGetEventInfo";
            SortedList sParams = new SortedList();
            sParams = new SortedList();
            DateTime dtEventDate = DateTime.Now;

            if (EventID != -1)
            {
                sParams = new SortedList();
                sParams.Add("@EventID", EventID);
                DataSet dsEventInfo = Classes.cUtilities.LoadDataSet(storedproc, sParams, "LARPortal", Session["UserName"].ToString(), lsRoutineName + ".SetEventDate");
                foreach (DataRow dEventInfo in dsEventInfo.Tables[0].Rows)
                {
                    if (DateTime.TryParse(dEventInfo["StartDate"].ToString(), out dtEventDate))
                        Session["EventDate"] = dtEventDate.ToShortDateString();

                }
            }

        }

        private void FillGrid()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            string storedproc = "uspGetDonations";
            CurrentEvent = int.Parse(Session["CurrentEvents"].ToString());

            SortedList sparams = new SortedList();
            sparams.Add("@CampaignID", Master.CampaignID);
            sparams.Add("@EventID", CurrentEvent);
            DataTable dtDonations = Classes.cUtilities.LoadDataTable(storedproc, sparams, "LARPortal", Master.UserName, lsRoutineName + storedproc);
            //DataView dvDonations = new DataView(dtDonations, "", ViewState["SortField"].ToString() + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows);
            gvDonationList.DataSource = dtDonations;
            gvDonationList.DataBind();

        }

        protected void gvDonationList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //gvDonationList.EditIndex = -1;
            //FillGrid();
        }

        protected void gvDonationList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //Session["EditMode"] = "Edit";
            //gvDonationList.EditIndex = e.NewEditIndex;
            //FillGrid();
        }

        protected void gvDonationList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Do Stuff to be defined
            //FillGrid();
            //OnRowCancelingEdit = "gvDonationList_RowCancelingEdit"
            //                    OnRowEditing = "gvDonationList_RowEditing"
            //                    OnRowUpdating = "gvDonationList_RowUpdating"
            //                    OnRowUpdated = "gvDonationList_RowUpdated"
            //                    OnRowDeleting = "gvDonationList_RowDeleting"
        }

        protected void gvDonationList_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

        }

        protected void gvDonationList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Do Stuff to be defined
            //FillGrid();
        }

        protected void gvDonationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DataRowView dRow = e.Row.DataItem as DataRowView;
                }
            }
        }

        protected void gvDonationList_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {
            // Validate fields

            // Save values
            Classes.cDonation AddDonation = new Classes.cDonation();
            int QtyNeeded = int.Parse(tbQuantityNeeded.Text);
            double UnitReward = double.Parse(tbUnitReward.Text);
            DateTime ReqdBy = DateTime.Parse(tbReqdBy.Text);
            int DonationType = int.Parse(ddlDonationType.SelectedValue);
            CurrentEvent = int.Parse(Session["CurrentEvents"].ToString());
            int RewardUnit = int.Parse(hidDefaultRewardUnitID.Value);
            DateTime dtEventDate = DateTime.Parse(Session["EventDate"].ToString());
            int AwardWhen = int.Parse(hidDefaultAwardWhen.Value);
            bool Recur = false;
            bool MatReimb = false;
            bool HideDon = false;
            bool CarryOver = false;
            bool EventFee = false;
            int UID = Master.UserID;
            int DonationID = -1;
            AddDonation.UpdateDonation(UID, DonationID, Master.CampaignID, CurrentEvent, tbDescription.Text, DonationType,
                QtyNeeded, UnitReward, RewardUnit, tbDonationComments.Text, tbURL.Text, tbStaffComments.Text, ReqdBy, tbSTAdd1.Text,
                tbSTAdd2.Text, tbSTCity.Text, tbSTState.Text, tbSTZip.Text, tbSTPhone.Text, tbSTEmail.Text, StatusID,
                Recur, MatReimb, 1, 0, HideDon, dtEventDate, CarryOver, AwardWhen, EventFee);

            // Rebuild grid
            FillGrid();
            // Clear form
            ClearAddFields();

            pnlDonationAdd.Visible = true;
            pnlDonationEdit.Visible = false;
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            // Validate fields - btnSaveEdit_Click

            // Save values
            Classes.cDonation EditDonation = new Classes.cDonation();
            int QtyNeeded = int.Parse(tbQuantityNeededEdit.Text);
            double UnitReward = double.Parse(tbUnitRewardEdit.Text);
            DateTime ReqdBy = DateTime.Parse(tbReqdByEdit.Text);
            int DonationType = int.Parse(ddlDonationTypeEdit.SelectedValue);
            CurrentEvent = int.Parse(Session["CurrentEvents"].ToString());
            int RewardUnit = int.Parse(hidDefaultRewardUnitID.Value);
            DateTime dtEventDate = DateTime.Parse(Session["EventDate"].ToString());
            int AwardWhen = int.Parse(hidDefaultAwardWhen.Value);
            bool Recur = false;
            bool MatReimb = false;
            bool HideDon = false;
            bool CarryOver = false;
            bool EventFee = false;
            int UID = Master.UserID;
            int DonationID = int.Parse(hidDonationIDEdit2.ToString());
            EditDonation.UpdateDonation(UID, DonationID, Master.CampaignID, CurrentEvent, tbDescriptionEdit.Text, DonationType,
                QtyNeeded, UnitReward, RewardUnit, tbDonationCommentsEdit.Text, tbURLEdit.Text, tbStaffCommentsEdit.Text, ReqdBy, tbSTAdd1Edit.Text,
                tbSTAdd2Edit.Text, tbSTCityEdit.Text, tbSTStateEdit.Text, tbSTZipEdit.Text, tbSTPhoneEdit.Text, tbSTEmailEdit.Text, StatusID,
                Recur, MatReimb, 1, 0, HideDon, dtEventDate, CarryOver, AwardWhen, EventFee);

            // Rebuild grid
            FillGrid();
            // Clear form
            ClearAddFields();

            pnlDonationAdd.Visible = true;
            pnlDonationEdit.Visible = false;
        }

        protected void btnCancelAdd_Click(object sender, EventArgs e)
        {
            ClearAddFields();
        }

        protected void ClearAddFields()
        {
            // Reset form to default values
            tbDescription.Text = "";
            tbQuantityNeeded.Text = "1";
            tbUnitReward.Text = "1";
            tbReqdBy.Text = Session["EventDate"].ToString();
            tbURL.Text = "";
            tbDonationComments.Text = "";
            tbStaffComments.Text = "";
            tbSTAdd1.Text = "";
            tbSTAdd2.Text = "";
            tbSTCity.Text = "";
            tbSTState.Text = "";
            tbSTZip.Text = "";
            tbSTPhone.Text = "";
            tbSTEmail.Text = "";
        }
        protected void ClearEditFields()
        {
            // Reset form to default values
            tbDescriptionEdit.Text = "";
            tbQuantityNeededEdit.Text = "1";
            tbUnitRewardEdit.Text = "1";
            tbReqdByEdit.Text = Session["EventDate"].ToString();
            tbURLEdit.Text = "";
            tbDonationCommentsEdit.Text = "";
            tbStaffCommentsEdit.Text = "";
            tbSTAdd1Edit.Text = "";
            tbSTAdd2Edit.Text = "";
            tbSTCityEdit.Text = "";
            tbSTStateEdit.Text = "";
            tbSTZipEdit.Text = "";
            tbSTPhoneEdit.Text = "";
            tbSTEmailEdit.Text = "";
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            // Go back to Setup Donations page
            Page.Response.Redirect("SetupDonations.aspx", true);
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            // Make the add panel visible
            ClearAddFields();
            ClearEditFields();
            pnlDonationEdit.Visible = false;
            pnlDonationAdd.Visible = true;
        }

        protected void btnFillSTDefault_Click(object sender, EventArgs e)
        {
            // Fill in the ship to address fields with the default values
            tbSTAdd1.Text = hidAdd1.Value;
            tbSTAdd2.Text = hidAdd2.Value;
            tbSTCity.Text = hidCity.Value;
            tbSTState.Text = hidState.Value;
            tbSTZip.Text = hidZip.Value;
            tbSTPhone.Text = hidPhone.Value;
            tbSTEmail.Text = hidEmail.Value;
        }

        protected void btnFillSTDefaultEdit_Click(object sender, EventArgs e)
        {
            // Fill in the ship to address fields with the default values
            tbSTAdd1Edit.Text = hidAdd1.Value;
            tbSTAdd2Edit.Text = hidAdd2.Value;
            tbSTCityEdit.Text = hidCity.Value;
            tbSTStateEdit.Text = hidState.Value;
            tbSTZipEdit.Text = hidZip.Value;
            tbSTPhoneEdit.Text = hidPhone.Value;
            tbSTEmailEdit.Text = hidEmail.Value;
        }

        protected void gvDonationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            string sDonationID = e.CommandArgument.ToString();
            Int32 DonationID = Int32.Parse(sDonationID);
            Session["DonationID"] = DonationID;
            string sCommandName = e.CommandName.ToUpper();

            GridViewRow currentRow = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

            // Apparently EDIT and DELETE are invokable and need to be "handled" thus CHANGE and REMOVE
            switch (sCommandName)
            {
                case "REMOVE":
                    DeleteDonation(DonationID);
                    break;

                case "CHANGE":
                    //Response.Redirect("DonationAdd.aspx?EventID=" + sEventID, true);
                    pnlDonationAdd.Visible = false;
                    pnlDonationEdit.Visible = true;
                    EditDonation(DonationID);
                    break;
            }
        }


        protected void EditDonation(int DonationID)
        {
            // Call stored procedure uspGetDonation (@ DonationID) and populate edit panel
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList slParameters = new SortedList();
            slParameters.Add("@DonationID", DonationID);
            /*cUtilities.PerformNonQuery("uspGetDonation", slParameters, "LARPortal", Master.UserName)*/
            ;

            // Now fill the form with the values to edit

            DataSet dsDonationInfo = Classes.cUtilities.LoadDataSet("uspGetDonation", slParameters, "LARPortal", Master.UserName.ToString(), lsRoutineName + ".EditDonation");
            foreach (DataRow dDonationInfo in dsDonationInfo.Tables[0].Rows)
            {
                tbDescriptionEdit.Text = dDonationInfo["Description"].ToString().Trim();
                ddlDonationTypeEdit.SelectedValue = dDonationInfo["DonationTypeID"].ToString().Trim();
                tbQuantityNeededEdit.Text = dDonationInfo["QtyNeeded"].ToString().Trim();
                tbUnitRewardEdit.Text = dDonationInfo["Worth"].ToString().Trim();
                RUEdit.Text = dDonationInfo["PoolDescription"].ToString().Trim();
                tbReqdByEdit.Text = dDonationInfo["RequiredByDate"].ToString().Trim();
                tbURLEdit.Text = dDonationInfo["URL"].ToString().Trim();
                tbDonationCommentsEdit.Text = dDonationInfo["DonationComments"].ToString().Trim();
                tbStaffCommentsEdit.Text = dDonationInfo["StaffComments"].ToString().Trim();
                tbSTAdd1Edit.Text = dDonationInfo["ShipToAdd1"].ToString().Trim();
                tbSTAdd2Edit.Text = dDonationInfo["ShipToAdd2"].ToString().Trim();
                tbSTCityEdit.Text = dDonationInfo["ShipToCity"].ToString().Trim();
                tbSTStateEdit.Text = dDonationInfo["ShipToState"].ToString().Trim();
                tbSTZipEdit.Text = dDonationInfo["ShipToPostalCode"].ToString().Trim();
                tbSTPhoneEdit.Text = dDonationInfo["ShipToPhone"].ToString().Trim();
                tbSTEmailEdit.Text = dDonationInfo["NotificationEmail"].ToString().Trim();
                hidDonationIDEdit2.Value = DonationID.ToString();
            }
        }


        protected void DeleteDonation(int DonationID)
        {
            // Call stored procedure uspDelCMDonations (@RecordID int = DonationID, @UserID int) then refresh current page
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList slParameters = new SortedList();
            slParameters.Add("@RecordID", DonationID);
            slParameters.Add("@UserID", Master.UserID);
            cUtilities.PerformNonQuery("uspDelCMDonations", slParameters, "LARPortal", Master.UserName);
            FillGrid();
            //Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
    }
}