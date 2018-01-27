using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace LarpPortal.Calendar
{
    public partial class CalReport : System.Web.UI.Page
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

            //switch (ddlCampaignChoice.SelectedValue)
            //{
            //    case "1":
            //        CampaignID = 0; // All MY campaigns
            //        break;

            //    case "2":
            //        if (Session["CampaignID"].ToString() != null)
            //        {
            //            int.TryParse((Session["CampaignID"].ToString()), out CampaignID);   // Selected campaign
            //        }
            //        break;

            //    case "3":
            //        int.TryParse((Session["CampaignID"].ToString()), out CampaignID); ; // Campaigns in the selected game system
            //        break;

            //    case "4":
            //        CampaignID = -1; // ALL LARP Portal campaigns
            //        break;

            //    default:
            //        CampaignID = 0;
            //        break;
            //}

            if (Session["CalendarCampaignID"] == null)
                Session["CalendarCampaignID"] = "-1";

            int CalCampID;
            if (!int.TryParse(Session["CalendarCampaignID"].ToString(), out CalCampID))
                CalCampID = -1;

            //if (CalCampID == -1)
            //    CampaignID = 0;
            //else if (CalCampID == -2)
            //    CampaignID = -1;
            //else
            //    CampaignID = CalCampID;

            //switch (ddlOrderBy.SelectedValue)
            //{
            //    case "1":

            //        break;

            //    case "2":

            //        break;

            //    case "3":

            //        break;

            //    default:

            //        break;
            //}

            //int.TryParse((ddlOrderBy.SelectedValue.ToString()), out CampaignSorter);
            //sParams.Add("@UserID", UserID);
            //sParams.Add("@OrderBy", ddlOrderBy.SelectedValue);
            //sParams.Add("@CampaignID", CalCampID); // Which campaign is picked in the drop down list
            //sParams.Add("@CampaignChoice", ddlCampaignChoice.SelectedValue);    // 1 All my campaigns / 2 Selected campaign / 3 Selected game system / 4 All
            //sParams.Add("@StartDate", StartDate);
            //sParams.Add("@EndDate", EndDate);
            //sParams.Add("@CampaignSorter", CampaignSorter);

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
            HtmlForm form = new HtmlForm();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "LARPCalendar.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvCalendar.AllowPaging = false;
            //BindGridDetails(gvCalendar);
            form.Attributes["runat"] = "server";
            form.Controls.Add(gvCalendar);
            this.Controls.Add(form);
            form.RenderControl(hw);
            string style = @"<!--mce:2-->";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}
