using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class DonationEventSummary : System.Web.UI.Page
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
            DataTable dtCheckList = Classes.cUtilities.LoadDataTable("uspRptDonationEventSummary", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspRptDonationEventSummary");

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
            //string CellText = "";
            //for (int k = 0; k < gvCheckList.Columns.Count; k++)
            //{
            //    columnbind.Append(gvCheckList.Columns[k].HeaderText + ',');
            //}
            //columnbind.Append("\r\n");
            //for (int i = 0; i < gvCheckList.Rows.Count; i++)
            //{
            //    for (int k = 0; k < gvCheckList.Columns.Count; k++)
            //    {
            //        CellText = gvCheckList.Rows[i].Cells[k].Text;
            //        // Take out commas because they screw up the comma delimited csv string
            //        CellText = CellText.Replace(",", "");
            //        // Replace HTML characters with real counterparts; &nbsp -> space / &#39; -> apostrophe / &amp; -> & / &quot; -> " / &lt; -> < / &gt; -> >
            //        CellText = CellText.Replace("&nbsp;", "");
            //        CellText = CellText.Replace("&#39;", "'");
            //        CellText = CellText.Replace("&amp;", " and ");
            //        CellText = CellText.Replace("&quot;", "\"");
            //        CellText = CellText.Replace("&lt;", "<");
            //        CellText = CellText.Replace("&gt;", ">");
            //        CellText = CellText + ",";
            //        columnbind.Append(CellText);
            //    }
            //    columnbind.Append("\r\n");
            //}
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