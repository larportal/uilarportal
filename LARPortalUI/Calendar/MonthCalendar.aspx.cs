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
    public partial class MonthCalendar : System.Web.UI.Page
    {
        const int _START_DATE = 0;
        const int _REGISTRATION_OPEN = 1;
        const int _REGISTRATION_CLOSE = 2;
        const int _PAYMENT_DUE = 3;
        const int _PREREGISTRATION_DUE = 4;
        const int _INFO_SKILLS_DUE = 5;
        const int _PEL_DUE = 6;

        readonly string[] DateLabels = new string[] { "Event", "REG Opens", "REG Closes", "$ Due", "PRE Reg", "Info Due", "PEL Due" };

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

        public string eventstring = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
            Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
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

            Dictionary<int, string> ListTooltip = new Dictionary<int, string>();

            foreach (DataRow dEventInfo in dsCampaigns.Tables[0].Rows)
            {
                string sCampaignName = dEventInfo["CampaignName"].ToString();
                DateTime dtTemp;

                int iEventID = 0;
                int.TryParse(dEventInfo["EventID"].ToString(), out iEventID);
                string sEventName = dEventInfo["EventName"].ToString().Replace("'", "`");
                string sEventDescription = dEventInfo["EventDescription"].ToString().Replace("\"", "'");

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

                string sTooltip = "";
                foreach (DateWithDescription dItem in DateList.Where(x => x.EventID == iEventID).OrderBy(x => x.DateValue).ToList<DateWithDescription>())
                {
                    if (sTooltip.Length == 0)
                    {
                        sTooltip = @"<table><tr><td colspan=""2"" align=""center"">" + dItem.EventName + "</td></tr>";
                    }
                    sTooltip += @"<tr><td align=""left"">" + DateLabels[dItem.DateType] + @"</td><td align=""left"">&nbsp;" + dItem.DateValue.ToShortDateString() + "</td></tr>";
                }
                if (sTooltip.Length > 0)
                {
                    sTooltip += "</table>";
                    ListTooltip.Add(iEventID, sTooltip);
                }
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
                h2NoEvents.Visible = true;
                eventstring = "";
                calendar.Visible = false;
            }
            else
            {
                h2NoEvents.Visible = false;
                calendar.Visible = true;
                var FirstDateList = DateList.OrderBy(x => x.DateValue).First();
                var LastDateList = DateList.OrderByDescending(x => x.DateValue).First();

                eventstring = "";

                int counter = 0;
                foreach (DateWithDescription DateItem in DateList)
                {
                    if (++counter >= 160)
                        break;
                    string sTooltip = "";
                    if (ListTooltip.TryGetValue(DateItem.EventID, out sTooltip))
                    {
                        sTooltip = "', tooltip: '" + sTooltip;
                    }
                    if (eventstring.Trim().Length > 0)
                        eventstring += ",";
                    eventstring = eventstring + "{     title: '" +
                        //"<b>" + 
                        DateItem.CampaignName + " - " + DateItem.EventName + " : " +
                        //"</b> " + 
                        DateLabels[DateItem.DateType] +
                        sTooltip +
                        "', textEscape: false,    start:  '" +
                        string.Format("{0:yyyy-MM-dd}", DateItem.DateValue) + "'  } " + Environment.NewLine;
                }
            }
            Console.WriteLine(eventstring);
        }

        protected void MasterPage_CampaignChanged(object sender, EventArgs e)
        {
            string dTime = DateTime.Now.ToString();
        }
    }
}
