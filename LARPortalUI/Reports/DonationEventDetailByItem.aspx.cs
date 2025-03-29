using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class DonationEventDetailByItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlEvent.Attributes.Add("OnChange", "EventChange(); return false;");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnRunReport.Style["Display"] = "none";
                btnExportExcel.Style["Display"] = "none";
                LoadData();
            }
        }

        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            Bindgrid();
        }

        private void Bindgrid()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            btnExportExcel.Style["Display"] = "inline";
            btnRunReport.Style["Display"] = "inline";
            pnlReport.Visible = true;

            SortedList sParams = new SortedList();
            int EventID = 0;
            int.TryParse(ddlEvent.SelectedValue.ToString(), out EventID);
            sParams.Add("@EventID", EventID);
            DataTable dtCheckList = Classes.cUtilities.LoadDataTable("uspRptDonationEventDetailByItem", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspRptDonationDetailByItem");

            gvCheckList.DataSource = dtCheckList;
            gvCheckList.DataBind();
        }


        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            Bindgrid();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DonationEventSummary.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            gvCheckList.AllowPaging = false;
            gvCheckList.DataBind();
            StringBuilder columnbind = new StringBuilder();
            Response.Output.Write(columnbind.ToString());
            Response.Flush();
            Response.End();
        }

        public void LoadData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DataTable dtEvent = Classes.cUtilities.LoadDataTable("uspGetCampaignEvents", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignEvents");
            ddlEvent.ClearSelection();
            ddlEvent.DataTextField = "EventNameDate";
            ddlEvent.DataValueField = "EventID";
            ddlEvent.DataSource = dtEvent;
            ddlEvent.DataBind();
            ddlEvent.Items.Insert(0, new ListItem("Select Event - Date", ""));
            ddlEvent.SelectedIndex = 0;

            btnRunReport.Style["Display"] = "none";
            btnExportExcel.Style["Display"] = "none";
            pnlReport.Visible = false;
        }
    }
}