using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Reflection;

namespace LarpPortal.Donations
{
    public partial class DonationReceipt : System.Web.UI.Page
    {
        public bool _Reload = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ViewState["SortField"] == null)
                    ViewState["SortField"] = "Player";
                if (ViewState["SortDir"] == null)
                    ViewState["SortDir"] = "";
                SetCampaignPlayerID();
                CampaignDonationSettings();
                gvRegistrations.DataSource = GetData("uspGetDonationClaimsForReceipt", Master.CampaignID, "", "", "", Master.UserName, Master.RoleString, Master.UserID);
                gvRegistrations.DataBind();

            }
        }

        protected void BindData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@EventID", ddlEvent.SelectedValue);
            sParams.Add("@StatusID", ddlDonationStatus.SelectedValue);
            sParams.Add("@PlayerID", ddlPlayer.SelectedValue);
            sParams.Add("@Roles", Master.RoleString);
            sParams.Add("@UserID", Master.UserID);

            DataTable dtDonations = Classes.cUtilities.LoadDataTable("uspGetDonationClaimsForReceipt", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetDonations");

            gvRegistrations.DataSource = dtDonations;
            gvRegistrations.DataBind();

        }

        private static DataTable GetData(string query, Int32 CampaignID, string ddlStatus, string ddlEvent, string DonationID, string UserName, string RoleString, int uID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            SortedList sParams = new SortedList();
            // Common parameters
            if (CampaignID != 0)
                sParams.Add("@CampaignID", CampaignID);
            if (ddlStatus != "")
                sParams.Add("@StatusID", ddlStatus);
            if (ddlEvent != "")
                sParams.Add("@EventID", ddlEvent);
            if (DonationID != "")
                sParams.Add("DonationID", DonationID);
            // Query specific parameters
            switch (query)
            {
                case "uspGetDonations":
                    sParams.Add("@Roles", RoleString);
                    break;
                case "uspGetDonationClaims":
                    sParams.Add("@Roles", RoleString);
                    sParams.Add("@UserID", uID);
                    break;
                case "uspGetDonationClaimsForReceipt":
                    sParams.Add("@Roles", RoleString);
                    sParams.Add("@UserID", uID);
                    //sParams.Add("@PlayerID", );
                    break;
                default:
                    break;
            }

            DataTable dt = Classes.cUtilities.LoadDataTable(query, sParams, "LARPortal", UserName, lsRoutineName + "." + query);
            return dt;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Session["ActiveLeftNav"] = "PointsView";
            LoadStatusddl();
            LoadEventddl();
            LoadPlayerddl();

        }

        private void PopulateGrid()
        {

        }

        protected void gvRegistrations_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRegistrations.EditIndex = -1;

            BindData();
        }

        protected void gvRegistrations_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRegistrations.EditIndex = e.NewEditIndex;

            BindData();
        }

        protected void gvRegistrations_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gvRegistrations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DataRowView dRow = e.Row.DataItem as DataRowView;

                    System.Web.UI.WebControls.Calendar calPaymentDate = (System.Web.UI.WebControls.Calendar)e.Row.FindControl("calPaymentDate");
                    if (calPaymentDate != null)
                    {
                        calPaymentDate.SelectedDate = DateTime.Today;
                        HiddenField hidPaymentDate = (HiddenField)e.Row.FindControl("hidPaymentDate");
                        if (hidPaymentDate != null)
                        {
                            DateTime dtPaymentDate;
                            if (DateTime.TryParse(hidPaymentDate.Value, out dtPaymentDate))
                            {
                                calPaymentDate.SelectedDate = dtPaymentDate;
                            }
                        }
                    }

                }
            }

        }

        protected void ddlPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlPlayer";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlPlayer.SelectedValue;
            Option.SaveOptionValue();
            ViewState["Player"] = ddlPlayer.SelectedValue;
            BindData();
        }

        protected void ddlDonationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlDonationStatus";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlDonationStatus.SelectedValue;
            Option.SaveOptionValue();
            ViewState["DonationStatus"] = ddlDonationStatus.SelectedValue;
            BindData();
        }

        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlEvent";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlEvent.SelectedValue;
            Option.SaveOptionValue();
            ViewState["Event"] = ddlEvent.SelectedValue;
            BindData();
        }

        protected void gvRegistrations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "RECEIVE")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow clickedRow = gvRegistrations.Rows[rowIndex];

                HiddenField hdDonationClaimID = (HiddenField)clickedRow.FindControl("hidDonationClaimID");
                HiddenField hdCampaignCPOpportunityID = (HiddenField)clickedRow.FindControl("hidCampaignCPOpportunityID");
                HiddenField hdPlayerCPAuditID = (HiddenField)clickedRow.FindControl("hidPlayerCPAuditID");
                Int32 DonationClaimID = Int32.Parse(hdDonationClaimID.Value);
                Int32 CampaignCPOpportunityID = Int32.Parse(hdCampaignCPOpportunityID.Value);
                Int32 PlayerCPAuditID = Int32.Parse(hdPlayerCPAuditID.Value);
                TextBox tbStaffComments = (TextBox)clickedRow.FindControl("tbStaffComments");
                string StaffComments = tbStaffComments.Text;
                Label lbClaimed = (Label)clickedRow.FindControl("lblQtyClaimed");
                double Claimed = double.Parse(lbClaimed.Text);

                SortedList sParams = new SortedList();
                sParams.Add("@DonationClaimID", DonationClaimID);
                sParams.Add("@QuantityReceived", Claimed);
                sParams.Add("@StaffComments", StaffComments);
                sParams.Add("@UserID", Master.UserID);
                sParams.Add("@CampaignCPOpportunityID", CampaignCPOpportunityID);
                sParams.Add("@PlayerCPAuditID", PlayerCPAuditID);
                Classes.cUtilities.PerformNonQuery("uspReceiveDonationClaims", sParams, "LARPortal", Master.UserName);
                _Reload = true;
                BindData();
            }
        }

        protected void btnApproveAll_Click(object sender, EventArgs e)
        {
            try
            {
                SortedList sParams = new SortedList();
                sParams.Add("@EventID", ddlEvent.SelectedValue);
                Classes.cUtilities.PerformNonQuery("uspEventApproveAllReg", sParams, "LARPortal", Master.UserName);
                _Reload = true;
            }
            catch
            {
            }
        }

        protected void gvRegistrations_Sorting(object sender, GridViewSortEventArgs e)
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

            //PopulateGrid();
            BindData();

            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoginUsername = Session["UserName"].ToString();
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "gvRegistrations";
            Option.ObjectOption = "SortField";
            Option.OptionValue = ViewState["SortField"].ToString();
            Option.SaveOptionValue();

            Option = new Classes.cUserOption();
            Option.LoginUsername = Session["UserName"].ToString();
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "gvRegistrations";
            Option.ObjectOption = "SortDir";
            Option.OptionValue = ViewState["SortDir"].ToString();
            Option.SaveOptionValue();
        }

        private void DisplaySorting()
        {
            if (gvRegistrations.HeaderRow != null)
            {
                int iSortedColumn = 0;
                foreach (DataControlFieldHeaderCell cHeaderCell in gvRegistrations.HeaderRow.Cells)
                {
                    if (cHeaderCell.ContainingField.SortExpression == ViewState["SortField"].ToString())
                    {
                        iSortedColumn = gvRegistrations.HeaderRow.Cells.GetCellIndex(cHeaderCell);
                    }
                }
                if (iSortedColumn != 0)
                {
                    Label lblArrowLabel = new Label();
                    if (ViewState["SortDir"].ToString().Length == 0)
                        lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-up'> </span></a>";
                    else
                        lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-down'> </span></a>";
                    gvRegistrations.HeaderRow.Cells[iSortedColumn].Controls.Add(lblArrowLabel);
                }
            }
        }

        protected void SetCampaignPlayerID()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Classes.cUserCampaign cuID = new Classes.cUserCampaign();
            cuID.Load(Master.UserID, Master.CampaignID);
            Session["CampaignPlayerID"] = cuID.CampaignPlayerID;
        }

        protected void CampaignDonationSettings()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Classes.cDonation DonationSettings = new Classes.cDonation();
            DonationSettings.GetDonationCampaignSettings(Master.CampaignID);

            Session["CampaignCPOpportunityDefaultID"] = DonationSettings.CampaignCPOpportunityDefaultID.ToString();
        }

        protected void LoadStatusddl()
        {
            // uspGetDonationClaimsForReceiptStatusDDL
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlDonationStatus.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DataTable dtStatus = Classes.cUtilities.LoadDataTable("uspGetDonationClaimsForReceiptStatusDDL", sParams, "LARPortal", Master.UserName, lsRoutineName + "uspGetCampaignPools");
            ddlDonationStatus.DataTextField = "StatusDescription";
            ddlDonationStatus.DataValueField = "CampaignDonationStatusID";
            ddlDonationStatus.DataSource = dtStatus;
            ddlDonationStatus.DataBind();
            ddlDonationStatus.Items.Insert(0, new ListItem("All Statuses", "0"));
            if (ViewState["DonationStatus"] != null)
            {
                ddlDonationStatus.SelectedValue = ViewState["DonationStatus"].ToString();
            }
            else
            {
                ddlDonationStatus.SelectedIndex = 0;
            }
        }

        protected void LoadPlayerddl()
        {
            //uspGetDonationClaimsForReceiptPlayerDDL
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlPlayer.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DataTable dtPlayer = Classes.cUtilities.LoadDataTable("uspGetDonationClaimsForReceiptPlayerDDL", sParams, "LARPortal", Master.UserName, lsRoutineName + "uspGetCampaignPools");
            ddlPlayer.DataTextField = "Player";
            ddlPlayer.DataValueField = "CampaignPlayerID";
            ddlPlayer.DataSource = dtPlayer;
            ddlPlayer.DataBind();
            ddlPlayer.Items.Insert(0, new ListItem("All Players", "0"));
            if (ViewState["Player"] != null)
            {
                ddlPlayer.SelectedValue = ViewState["Player"].ToString();
            }
            else
            {
                ddlPlayer.SelectedIndex = 0;
            }
        }

        protected void LoadEventddl()
        {
            //uspGetDonationClaimsForReceiptEventDDL
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlEvent.Items.Clear();
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DataTable dtEvent = Classes.cUtilities.LoadDataTable("uspGetDonationClaimsForReceiptEventDDL", sParams, "LARPortal", Master.UserName, lsRoutineName + "uspGetCampaignPools");
            ddlEvent.DataTextField = "StartDate";
            ddlEvent.DataValueField = "EventID";
            ddlEvent.DataSource = dtEvent;
            ddlEvent.DataBind();
            ddlEvent.Items.Insert(0, new ListItem("All Events", "0"));
            if (ViewState["Event"] != null)
            {
                ddlEvent.SelectedValue = ViewState["Event"].ToString();
            }
            else
            {
                ddlEvent.SelectedIndex = 0;
            }

        }

        protected Boolean AlreadyReceived(Decimal QuantityAccepted)
        {
            return QuantityAccepted == 0;
        }

    }
}