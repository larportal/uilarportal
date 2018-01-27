using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Calendar
{
    public partial class CalGraphic : System.Web.UI.Page
    {
        const int _START_DATE = 0;
        const int _REGISTRATION_OPEN = 1;
        const int _REGISTRATION_CLOSE = 2;
        const int _PAYMENT_DUE = 3;
        const int _PREREGISTRATION_DUE = 4;
        const int _INFO_SKILLS_DUE = 5;
        const int _PEL_DUE = 6;

        const string _EVENT_BACKGROUND_COLOR = "blue";

        string[] DateLabels = new string[] { "Event", "REG Opens", "REG Closes", "$ Due", "PRE Reg", "Info Due", "PEL Due" };

        private class DateWithDescription
        {
            public DateTime DateValue { get; set; }
            public string CampaignName { get; set; }
            public string EventName { get; set; }
            public int EventID { get; set; }
            public int DateType { get; set; }

            public DateWithDescription(DateTime _DateValue, string _CampaignName, int _EventID, string _EventName, int _DateType)
            {
                DateValue = _DateValue;
                CampaignName = _CampaignName;
                EventName = _EventName;
                EventID = _EventID;
                DateType = _DateType;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.DisplayAllOptions = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@UserID", Master.UserID.ToString());

            DataSet dsCampaigns = Classes.cUtilities.LoadDataSet("uspGetEventList", sParams, "LARPortal", Master.UserName, lsRoutineName);
            DataView dvCampaigns = new DataView(dsCampaigns.Tables[0]);
            DataTable dtEventList = dvCampaigns.ToTable(true, "CampaignName", "EventID", "EventName");

            List<DateWithDescription> DateList = new List<DateWithDescription>();

            foreach (DataRow dEventInfo in dsCampaigns.Tables[0].Rows)
            {
                string sCampaignName = dEventInfo["CampaignName"].ToString();
                DateTime dtTemp;

                int iEventID = 0;
                int.TryParse(dEventInfo["EventID"].ToString(), out iEventID);
                string sEventName = dEventInfo["EventName"].ToString();
                string sEventDescription = dEventInfo["EventDescription"].ToString();

                if (DateTime.TryParse(dEventInfo["RegistrationOpenDateTime"].ToString(), out dtTemp))
                    DateList.Add(new DateWithDescription(dtTemp, sCampaignName, iEventID, sEventName, _REGISTRATION_OPEN));
                if (DateTime.TryParse(dEventInfo["RegistrationCloseDateTime"].ToString(), out dtTemp))
                    DateList.Add(new DateWithDescription(dtTemp, sCampaignName, iEventID, sEventName, _REGISTRATION_CLOSE));
                if (DateTime.TryParse(dEventInfo["PaymentDueDate"].ToString(), out dtTemp))
                    DateList.Add(new DateWithDescription(dtTemp, sCampaignName, iEventID, sEventName, _PAYMENT_DUE));
                if (DateTime.TryParse(dEventInfo["PreregistrationDeadline"].ToString(), out dtTemp))
                    DateList.Add(new DateWithDescription(dtTemp, sCampaignName, iEventID, sEventName, _PREREGISTRATION_DUE));
                if (DateTime.TryParse(dEventInfo["PELDeadlineDate"].ToString(), out dtTemp))
                    DateList.Add(new DateWithDescription(dtTemp, sCampaignName, iEventID, sEventName, _PEL_DUE));
                if (DateTime.TryParse(dEventInfo["InfoSkillDeadlineDate"].ToString(), out dtTemp))
                    DateList.Add(new DateWithDescription(dtTemp, sCampaignName, iEventID, sEventName, _INFO_SKILLS_DUE));
                if (DateTime.TryParse(dEventInfo["StartDateTime"].ToString(), out dtTemp))
                    DateList.Add(new DateWithDescription(dtTemp, sCampaignName, iEventID, sEventName, _START_DATE));
            }

            // Build the list of campaigns.
            // If doing 'My Campaigns' then build the list of campaigns.
            if (Master.CampaignID == -101)
            {
                string sLabel = "";

                // Using a for loop because we have to handle the first record and last record differently.
                for (int i = 0; i < dsCampaigns.Tables[1].Rows.Count; i++)
                {
                    if (i == 0)
                        sLabel = " - " + dsCampaigns.Tables[1].Rows[0]["CampaignName"].ToString();
                    else
                    {
                        if (i == dsCampaigns.Tables[1].Rows.Count - 1)
                            sLabel += " and " + dsCampaigns.Tables[1].Rows[i]["CampaignName"].ToString();
                        else
                            sLabel += ", " + dsCampaigns.Tables[1].Rows[i]["CampaignName"].ToString();
                    }
                }
                lblCampaignList.Text = sLabel;
            }
            else
            {
                // Not doing my campaigns so take whatever was selected in the drop down list.
                lblCampaignList.Text = " - " + Master.CampaignName;
            }

            if (DateList.Count == 0)
            {
                tabCalendar.Visible = false;
                lblCalendar.Text = "<h1>There are no upcoming events for this campaign.</h1>";
                lblCalendar.Visible = true;
                return;
            }

            tabCalendar.Visible = true;
            lblCalendar.Visible = false;
            lblCalendar.Text = "";

            var FirstDateList = DateList.OrderBy(x => x.DateValue).First();
            var LastDateList = DateList.OrderByDescending(x => x.DateValue).First();

            DateTime StartDate = FirstDateList.DateValue;
            TableRow trDate = new TableRow();
            TableRow trDesc = new TableRow();

            TableHeaderRow trColumnHeader = new TableHeaderRow();
			TableHeaderCell tcColumnHeader = new TableHeaderCell
			{
				Text = "Sunday",
				CssClass = "cellColumnHeader"
			};
			trColumnHeader.Cells.Add(tcColumnHeader);
			tcColumnHeader = new TableHeaderCell
			{
				Text = "Monday",
				CssClass = "cellColumnHeader"
			};
			trColumnHeader.Cells.Add(tcColumnHeader);
			tcColumnHeader = new TableHeaderCell
			{
				Text = "Tuesday",
				CssClass = "cellColumnHeader"
			};
			trColumnHeader.Cells.Add(tcColumnHeader);
			tcColumnHeader = new TableHeaderCell
			{
				Text = "Wednesday",
				CssClass = "cellColumnHeader"
			};
			trColumnHeader.Cells.Add(tcColumnHeader);
			tcColumnHeader = new TableHeaderCell
			{
				Text = "Thursday",
				CssClass = "cellColumnHeader"
			};
			trColumnHeader.Cells.Add(tcColumnHeader);
			tcColumnHeader = new TableHeaderCell
			{
				Text = "Friday",
				CssClass = "cellColumnHeader"
			};
			trColumnHeader.Cells.Add(tcColumnHeader);
			tcColumnHeader = new TableHeaderCell
			{
				Text = "Saturday",
				CssClass = "cellColumnHeader"
			};
			trColumnHeader.Cells.Add(tcColumnHeader);

            tabCalendar.Rows.Add(trColumnHeader);

            bool bWroteAnchor = false;

            if (StartDate.DayOfWeek != DayOfWeek.Sunday)
            {
                DateTime dtStartOfWeek = StartDate.AddDays((int)StartDate.DayOfWeek * -1);
                for (int i = 0; i < (int)StartDate.DayOfWeek; i++)
                {
                    TableCell tcHeader = new TableCell();
                    if (trDate.Cells.Count == 0)
                    {
                        tcHeader.Text = dtStartOfWeek.ToString("MMM") + " " + dtStartOfWeek.Day.ToString() + ", " + dtStartOfWeek.Year.ToString();
                        tcHeader.Font.Bold = true;
                    }
                    else
                        tcHeader.Text = dtStartOfWeek.Day.ToString();
                    tcHeader.CssClass = "cellHeader";
                    trDate.Cells.Add(tcHeader);

					TableCell tcDesc = new TableCell
					{
						Text = "",
						CssClass = "cellContents"
					};
					trDesc.Cells.Add(tcDesc);
                    dtStartOfWeek = dtStartOfWeek.AddDays(1);
                }
            }

            while (StartDate <= LastDateList.DateValue)
            {
                TableCell tcHeader = new TableCell();
                if (StartDate.Day == 1)
                {
                    tcHeader.Text = StartDate.ToString("MMM") + StartDate.Day.ToString() + ", " + StartDate.Year.ToString();
                    tcHeader.Text = StartDate.Day.ToString();
                    TableHeaderRow tcMonthRow = new TableHeaderRow();
					TableHeaderCell tcMonthCell = new TableHeaderCell
					{
						ColumnSpan = 7,
						Text = StartDate.ToString("MMMM") + " " + StartDate.Year.ToString(),
						CssClass = "CalendarMonth"
					};
					tcMonthRow.Cells.Add(tcMonthCell);
                    tabCalendar.Rows.Add(tcMonthRow);
                    trColumnHeader = new TableHeaderRow();
					tcColumnHeader = new TableHeaderCell
					{
						Text = "Sunday",
						CssClass = "cellColumnHeader"
					};
					trColumnHeader.Cells.Add(tcColumnHeader);
					tcColumnHeader = new TableHeaderCell
					{
						Text = "Monday",
						CssClass = "cellColumnHeader"
					};
					trColumnHeader.Cells.Add(tcColumnHeader);
					tcColumnHeader = new TableHeaderCell
					{
						Text = "Tuesday",
						CssClass = "cellColumnHeader"
					};
					trColumnHeader.Cells.Add(tcColumnHeader);
					tcColumnHeader = new TableHeaderCell
					{
						Text = "Wednesday",
						CssClass = "cellColumnHeader"
					};
					trColumnHeader.Cells.Add(tcColumnHeader);
					tcColumnHeader = new TableHeaderCell
					{
						Text = "Thursday",
						CssClass = "cellColumnHeader"
					};
					trColumnHeader.Cells.Add(tcColumnHeader);
					tcColumnHeader = new TableHeaderCell
					{
						Text = "Friday",
						CssClass = "cellColumnHeader"
					};
					trColumnHeader.Cells.Add(tcColumnHeader);
					tcColumnHeader = new TableHeaderCell
					{
						Text = "Saturday",
						CssClass = "cellColumnHeader"
					};
					trColumnHeader.Cells.Add(tcColumnHeader);
                    tabCalendar.Rows.Add(trColumnHeader);
                }
                else
                    tcHeader.Text = StartDate.Day.ToString();
                tcHeader.CssClass = "cellHeader";
                if ((StartDate.Date >= DateTime.Today.AddDays(-14)) &&
                    (!bWroteAnchor))
                {
                    tcHeader.Text += @"<a name=""ScrollToDate""></a>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ScrollToToday", "ScrollToDate();", true);
                    bWroteAnchor = true;
                }

                if (StartDate.Date == DateTime.Today)
                    tcHeader.BackColor = System.Drawing.Color.AntiqueWhite;

                trDate.Cells.Add(tcHeader);

                TableCell tcDesc = new TableCell();

                string sCellContent = "";

                foreach (DataRow dEvent in dtEventList.Rows)
                {
                    int iEventID;
                    if (int.TryParse(dEvent["EventID"].ToString(), out iEventID))
                    {
                        List<DateWithDescription> CampDesc = DateList.FindAll(x => ((x.DateValue.Date == StartDate.Date) && (x.EventID == iEventID)));
                        if (CampDesc.Count > 0)
                        {
                            sCellContent += @"<p class='Event" + iEventID.ToString() + "'><b>" + dEvent["CampaignName"].ToString() + " - " + dEvent["EventName"].ToString() + " :</b>";

                            foreach (DateWithDescription d in CampDesc)
                            {
                                if (d.DateType == _START_DATE)
                                    sCellContent += @" <span style=""color: white; background-color: " + _EVENT_BACKGROUND_COLOR + @""">&nbsp;" + DateLabels[d.DateType] + "&nbsp;</span>,";
                                else
                                    sCellContent += " " + DateLabels[d.DateType] + ",";
                            }
                            sCellContent = sCellContent.Substring(0, sCellContent.Length - 1) + "</p>";
                        }
                    }
                }
				tcDesc = new TableCell
				{
					Text = sCellContent,
					CssClass = "cellContents"
				};

				if (StartDate.Date == DateTime.Today)
                    tcDesc.BackColor = System.Drawing.Color.AntiqueWhite;

                trDesc.Cells.Add(tcDesc);

                if (StartDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    tabCalendar.Rows.Add(trDate);
                    trDate = new TableRow();
                    tabCalendar.Rows.Add(trDesc);
                    trDesc = new TableRow();
                }

                StartDate = StartDate.AddDays(1);

                if (trDesc.Cells.Count > 0)
                {
                    tabCalendar.Rows.Add(trDate);
                    tabCalendar.Rows.Add(trDesc);
                }
            }

            string sPopup = "";
            foreach (DataRow dRow in dtEventList.Rows)
            {
                int iEventID;
                if (int.TryParse(dRow["EventID"].ToString(), out iEventID))
                {
                    List<DateWithDescription> CampDesc = DateList.FindAll(x => x.EventID == iEventID).OrderBy(x => x.DateType).ToList();

                    sPopup += @"$(document).ready(function () {$('.Event" + iEventID.ToString() + @"').tooltip({ title: ""<table><tr><td colspan='2'>" + dRow["EventName"].ToString() +
                        "</td></tr>";

                    foreach (DateWithDescription DateDesc in CampDesc)
                    {
                        sPopup += @"<tr><td align='left'>" + DateLabels[DateDesc.DateType] + @"</td><td align='left'>&nbsp;" + DateDesc.DateValue.ToShortDateString() + "</td></tr>";
                    }
                    sPopup += @"</table>"", html: true, offset: '0 0' });});" + Environment.NewLine;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", sPopup, true);
        }
    }
}
