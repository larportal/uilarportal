using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace LarpPortal.Reports
{
    public partial class CalendarReport : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGrid();
            }
        }

        protected void FillGrid()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;

            DataTable dtCalendar = new DataTable();
            SortedList sParams = new SortedList();


            StartDate = DateTime.Today;
            EndDate = DateTime.Today;

            switch (ddlEventDateRange.SelectedValue)
            {
                case "1":
                    EndDate = EndDate.AddYears(10); // Look out 10 years
                    break;

                case "2":
                    EndDate = EndDate.AddMonths(6); // Look out 6 months
                    break;

                case "3":
                    StartDate = StartDate.AddMonths(-3); // Look back 3 months
                    break;

                case "4":
                    StartDate = StartDate.AddMonths(-6); // Look back 6 months
                    break;

                case "5":
                    StartDate = StartDate.AddYears(-1); // Look back 12 months
                    break;

                case "6":
                    StartDate = StartDate.AddYears(-100); // All historical
                    break;

                default:
                    EndDate = EndDate.AddYears(10); // Look out 10 years
                    break;
            }

            if (Session["CalendarCampaignID"] == null)
                Session["CalendarCampaignID"] = "-1";

            int CalCampID;
            if (!int.TryParse(Session["CalendarCampaignID"].ToString(), out CalCampID))
                CalCampID = -1;

            sParams = new SortedList();
            sParams.Add("@UserID", Master.UserID);
            sParams.Add("@CampaignID", CalCampID);
            sParams.Add("@StartDate", StartDate);
            sParams.Add("@EndDate", EndDate);

            dtCalendar = Classes.cUtilities.LoadDataTable("uspGetEventCalendarDates", sParams, "LARPortal", Master.UserName, lsRoutineName);
            DataView dvCalendar = new DataView(dtCalendar, "", ddlOrderBy.SelectedValue, DataViewRowState.CurrentRows);
            gvCalendar.DataSource = dvCalendar;
            gvCalendar.DataBind();
        }

        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            btnExportExcel.Visible = true;
            FillGrid();
            pnlReportOutput.Visible = true;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            FillGrid();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=LARPCalendar.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            gvCalendar.AllowPaging = false;
            gvCalendar.DataBind();
            StringBuilder columnbind = new StringBuilder();
            string CellText = "";
            for (int k = 0; k < gvCalendar.Columns.Count; k++)
            {
                columnbind.Append(gvCalendar.Columns[k].HeaderText + ',');
            }
            columnbind.Append("\r\n");
            for (int i = 0; i < gvCalendar.Rows.Count; i++)
            {
                for (int k = 0; k < gvCalendar.Columns.Count; k++)
                {
                    CellText = gvCalendar.Rows[i].Cells[k].Text;
                    // Take out commas because they screw up the comma delimited csv string
                    CellText = CellText.Replace(",", "");
                    // Replace HTML characters with real counterparts; &nbsp -> space / &#39; -> apostrophe / &amp; -> & / &quot; -> " / &lt; -> < / &gt; -> >
                    CellText = CellText.Replace("&nbsp;", "");
                    CellText = CellText.Replace("&#39;", "'");
                    CellText = CellText.Replace("&amp;", " and ");
                    CellText = CellText.Replace("&quot;", "\"");
                    CellText = CellText.Replace("&lt;", "<");
                    CellText = CellText.Replace("&gt;", ">");
                    CellText = CellText + ",";
                    columnbind.Append(CellText);
                }
                columnbind.Append("\r\n");
            }
            Response.Output.Write(columnbind.ToString());
            Response.Flush();
            Response.End();
            //HtmlForm form = new HtmlForm();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "LARPCalendar.xls"));
            //Response.ContentType = "application/ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //gvCalendar.AllowPaging = false;
            ////BindGridDetails(gvCalendar);
            //form.Attributes["runat"] = "server";
            //form.Controls.Add(gvCalendar);
            //this.Controls.Add(form);
            //form.RenderControl(hw);
            //string style = @"<!--mce:2-->";
            //Response.Write(style);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
        }

    }
}
