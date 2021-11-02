using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Reflection;

namespace LarpPortal.Donations
{
    public partial class DonationAdd : System.Web.UI.Page
    {
		protected void Page_Load(object sender, EventArgs e)
		{
            if(!IsPostBack)
            {
                CampaignDonationSettings();
            }

		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
                SortedList sParams = new SortedList();
                sParams.Add("@CampaignID", Master.CampaignID);
                DataSet dtEventInfo = Classes.cUtilities.LoadDataSet("uspGetEventInfo", sParams, "LARPortal", Master.UserName, lsRoutineName);

                dtEventInfo.Tables[0].TableName = "EventInfo";

                // Eventually the row filter needs to be     StatusName = 'Scheduled' and RegistrationOpenDateTime <= GetDate() and RegistrationCloseDateTime >= GetDate()
                DataView dvEventInfo = new DataView(dtEventInfo.Tables["EventInfo"], "PELDeadlineDate > '" + System.DateTime.Today.ToShortDateString() + "'",    // "StatusName = 'Scheduled'",    // and RegistrationOpenDateTime > '" + System.DateTime.Today + "'",
                    "", DataViewRowState.CurrentRows);

                DataTable dtEventDates = dvEventInfo.ToTable(true, "StartDate", "EventID", "EventName");

                // Could do this as a computed column - but I want to specify the format.
                dtEventDates.Columns.Add("DisplayStartDate", typeof(string));
                DateTime dtTemp;

                foreach (DataRow dRow in dtEventDates.Rows)
                    if (DateTime.TryParse(dRow["StartDate"].ToString(), out dtTemp))
                        dRow["DisplayStartDate"] = dtTemp.ToString("MM/dd/yyyy") + " - " + dRow["EventName"].ToString();

                DataView dvEventDate = new DataView(dtEventDates, "", "StartDate", DataViewRowState.CurrentRows);

                ddlEventDate.DataSource = dvEventDate;
                ddlEventDate.DataTextField = "DisplayStartDate";
                ddlEventDate.DataValueField = "EventID";
                ddlEventDate.DataBind();

                ddlEventDate_SelectedIndexChanged(null, null);


            }
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{


		}

        protected void ddlEventDate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAddRecurring_Click(object sender, EventArgs e)
        {
            btnAddNew.Visible = true;
            btnAddRecurring.Visible = false;
            mvDonations.SetActiveView(vwDonationsRecurring);
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            btnAddRecurring.Visible = true;
            btnAddNew.Visible = false;
            mvDonations.SetActiveView(vwDonationSingle);
        }

        protected void CampaignDonationSettings()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Classes.cDonation DonationSettings = new Classes.cDonation();
            DonationSettings.GetDonationCampaignSettings(Master.CampaignID);
            //hidStatus.Value = DonationSettings.StatusID.ToString();
            hidShip1.Value = DonationSettings.DefaultShipToAdd1.ToString();
            hidShip2.Value = DonationSettings.DefaultShipToAdd2.ToString();
            hidCity.Value = DonationSettings.DefaultShipToCity.ToString();
            hidState.Value = DonationSettings.DefaultShipToState.ToString();
            hidZip.Value = DonationSettings.DefaultShipToPostalCode.ToString();
            hidPhone.Value = DonationSettings.DefaultShipToPhone.ToString();
        }

    }
}