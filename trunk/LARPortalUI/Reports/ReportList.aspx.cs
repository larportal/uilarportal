using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class ReportList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (!IsPostBack)
            {
                SortedList sParams = new SortedList();
                sParams.Add("@UserID", Master.UserID);
                sParams.Add("@CampaignID", Master.CampaignID);
                DataTable dtReports = Classes.cUtilities.LoadDataTable("uspGetReports", sParams, "LARPortal", Master.UserName, lsRoutineName);
                DataView dvReports = new DataView(dtReports, "", "", DataViewRowState.CurrentRows);
                gvReportsList.DataSource = dvReports;
                gvReportsList.DataBind();
            }

            lblCampaignName.Text = Master.CampaignName;
        }

        protected void gvReportsList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int iSelectedRow;
            if (int.TryParse(e.CommandArgument.ToString(), out iSelectedRow))
            {
                string URL = gvReportsList.DataKeys[iSelectedRow].Values[0].ToString();
                Response.Redirect(URL);
            }
        }

        protected void gvReportsList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow dt = e.Row.DataItem as DataRow;
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Click to run the report.";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.gvReportsList, "Select$" + e.Row.RowIndex);
            }
        }
    }
}
