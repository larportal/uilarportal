using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using LarpPortal.Classes;

namespace LARPortal.Classes
{
    /// <summary>
    /// Class to hold event information.
    /// </summary>
    public class cEvent
    {
        /// <summary>
        /// Currently nothing to pass in to contructor. Use the Load function.
        /// </summary>
        public cEvent()
        {
            EventID = -1;
            CampaignID = -1;
            RecStatus = RecordStatuses.Active;
        }

        public int EventID { get; set; }
        public int CampaignID { get; set; }
        public string EventName { get; set; }
        public string IGEventLocation { get; set; }
        public DateTime? DecisionByDate { get; set; }
        public DateTime? NotificationDate { get; set; }
        public Boolean? SharePlanningInfo { get; set; }
        public int? StatusID { get; set; }
        public string StatusType { get; set; }
        public string StatusName { get; set; }
        public string StatusComments { get; set; }
        public string EventDescription { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int SiteID { get; set; }
        public int? MaximumPCCount { get; set; }
        public int? BaseNPCCount { get; set; }
        public int? NPCOverrideRatio { get; set; }
        public int? CapThresholdNotification { get; set; }
        public Boolean? CapMetNotification { get; set; }
        public int? MaximumNPCCount { get; set; }
        public int? AutoApproveWaitListOpenings { get; set; }
        public DateTime? RegistrationOpenDateTime { get; set; }
        public DateTime? PreregistrationDeadline { get; set; }
        public double? PreregistrationPrice { get; set; }
        public double? LateRegistrationPrice { get; set; }
        public double? CheckinPrice { get; set; }
        public int? DaysToAutoCancelOtherPlayerRegistration { get; set; }
        public Boolean? PCFoodService { get; set; }
        public Boolean? NPCFoodService { get; set; }
        public Boolean? CookingFacilitiesAvailable { get; set; }
        public Boolean? RefrigeratorAvailable { get; set; }
        public DateTime? PELDeadlineDate { get; set; }
        public DateTime? InfoSkillDeadlineDate { get; set; }
        public string Comments { get; set; }
        public cSite SiteInfo { get; set; }
        public RecordStatuses RecStatus { get; set; }

        /// <summary>
        /// Load an event.
        /// </summary>
        /// <param name="EventID">The Event ID to load.</param>
        /// <param name="UserName">The user name of the person loading it.</param>
        /// <returns></returns>
        public int Load(int EventID, string UserName)
        {
            int iNumEventRecords = 0;

            SortedList Params = new SortedList();
            Params.Add("@EventID", EventID.ToString());

            // Go get the dataset from GetEvents.
            DataSet dtEvent = new DataSet();
            dtEvent = cUtilities.LoadDataSet("uspGetEvent", Params, "LARPortal", UserName, "cEvents.Load");

            int iTemp;
            bool bTemp;
            DateTime dtTemp;
            double dTemp;

            // Name the tables so that we can use them by name later. Makes it easier to reference them.
            dtEvent.Tables[0].TableName = "CMEvents";
            dtEvent.Tables[1].TableName = "CMSites";
            dtEvent.Tables[2].TableName = "MDBAddresses";
            dtEvent.Tables[3].TableName = "MDBPhoneNumbers";

            foreach (DataRow dRow in dtEvent.Tables["CMEvents"].Rows)
            {
                if (int.TryParse(dRow["EventID"].ToString(), out iTemp))
                    EventID = iTemp;
                if (int.TryParse(dRow["CampaignID"].ToString(), out iTemp))
                    CampaignID = iTemp;
                EventName = dRow["EventName"].ToString();
                if (dRow["IGEventLocation"] != DBNull.Value)
                    IGEventLocation = dRow["IGEventLocation"].ToString();
                if (DateTime.TryParse(dRow["DecisionByDate"].ToString(), out dtTemp))
                    DecisionByDate = dtTemp;
                if (DateTime.TryParse(dRow["NotificationDate"].ToString(), out dtTemp))
                    NotificationDate = dtTemp;
                if (Boolean.TryParse(dRow["SharePlanningInfo"].ToString(), out bTemp))
                    SharePlanningInfo = bTemp;
                if (Int32.TryParse(dRow["StatusID"].ToString(), out iTemp))
                    StatusID = iTemp;
                if (dRow["StatusName"] != DBNull.Value)
                    StatusName = dRow["StatusName"].ToString();
                if (dRow["StatusType"] != DBNull.Value)
                    StatusType = dRow["StatusType"].ToString();
                if (dRow["StatusComments"] != DBNull.Value)
                    StatusComments = dRow["StatusComments"].ToString();
                if (dRow["EventDescription"] != DBNull.Value)
                    EventDescription = dRow["EventDescription"].ToString();
                if (DateTime.TryParse(dRow["StartDateTime"].ToString(), out dtTemp))
                    StartDateTime = dtTemp;
                if (DateTime.TryParse(dRow["EndDateTime"].ToString(), out dtTemp))
                    EndDateTime = dtTemp;
                if (int.TryParse(dRow["MaximumPCCount"].ToString(), out iTemp))
                    MaximumPCCount = iTemp;
                if (int.TryParse(dRow["BaseNPCCount"].ToString(), out iTemp))
                    BaseNPCCount = iTemp;
                if (int.TryParse(dRow["NPCOverrideRatio"].ToString(), out iTemp))
                    NPCOverrideRatio = iTemp;
                if (int.TryParse(dRow["CapThresholdNotification"].ToString(), out iTemp))
                    CapThresholdNotification = iTemp;
                if (bool.TryParse(dRow["CapMetNotification"].ToString(), out bTemp))
                    CapMetNotification = bTemp;
                if (int.TryParse(dRow["MaximumNPCCount"].ToString(), out iTemp))
                    MaximumNPCCount = iTemp;
                if (int.TryParse(dRow["AutoApproveWaitListOpenings"].ToString(), out iTemp))
                    AutoApproveWaitListOpenings = iTemp;
                if (DateTime.TryParse(dRow["RegistrationOpenDateTime"].ToString(), out dtTemp))
                    RegistrationOpenDateTime = dtTemp;
                if (DateTime.TryParse(dRow["PreregistrationDeadline"].ToString(), out dtTemp))
                    PreregistrationDeadline = dtTemp;
                if (Double.TryParse(dRow["PreregistrationPrice"].ToString(), out dTemp))
                    PreregistrationPrice = dTemp;
                if (Double.TryParse(dRow["LateRegistrationPrice"].ToString(), out dTemp))
                    LateRegistrationPrice = dTemp;
                if (Double.TryParse(dRow["CheckinPrice"].ToString(), out dTemp))
                    CheckinPrice = dTemp;
                if (int.TryParse(dRow["DaysToAutoCancelOtherPlayerRegistration"].ToString(), out iTemp))
                    DaysToAutoCancelOtherPlayerRegistration = iTemp;
                if (Boolean.TryParse(dRow["PCFoodService"].ToString(), out bTemp))
                    PCFoodService = bTemp;
                if (Boolean.TryParse(dRow["NPCFoodService"].ToString(), out bTemp))
                    NPCFoodService = bTemp;
                if (Boolean.TryParse(dRow["CookingFacilitiesAvailable"].ToString(), out bTemp))
                    CookingFacilitiesAvailable = bTemp;
                if (Boolean.TryParse(dRow["RefrigeratorAvailable"].ToString(), out bTemp))
                    RefrigeratorAvailable = bTemp;
                if (DateTime.TryParse(dRow["PELDeadlineDate"].ToString(), out dtTemp))
                    PELDeadlineDate = dtTemp;
                if (DateTime.TryParse(dRow["InfoSkillDeadlineDate"].ToString(), out dtTemp))
                    InfoSkillDeadlineDate = dtTemp;
                if (dRow["Comments"] != DBNull.Value)
                    Comments = dRow["Comments"].ToString();
            }

            foreach (DataRow dRow in dtEvent.Tables["CMSites"].Rows)
            {
                SiteInfo = new cSite();

                if (int.TryParse(dRow["SiteID"].ToString(), out iTemp))
                    SiteInfo.SiteID = iTemp;

                SiteInfo.SiteName = dRow["SiteName"].ToString();
                if (int.TryParse(dRow["AddressID"].ToString(), out iTemp))
                    SiteInfo.AddressID = iTemp;
                SiteInfo.URL = dRow["URL"].ToString();
                SiteInfo.SiteMapURL = dRow["SiteMapURL"].ToString();

                if (Boolean.TryParse(dRow["YearRound"].ToString(), out bTemp))
                    SiteInfo.YearRound = bTemp;

                SiteInfo.TimeRestrictions = dRow["TimeRestrictions"].ToString();
                if (Boolean.TryParse(dRow["EMTCertificationRequired"].ToString(), out bTemp))
                    SiteInfo.EMTCertificationRequired = bTemp;

                if (Boolean.TryParse(dRow["CookingCertificationRequired"].ToString(), out bTemp))
                    SiteInfo.CookingCertificationRequired = bTemp;

                if (Boolean.TryParse(dRow["AdditionalWaiversRequired"].ToString(), out bTemp))
                    SiteInfo.AdditionalWaiversRequired = bTemp;

                SiteInfo.SiteNotes = dRow["SiteNotes"].ToString();
                SiteInfo.Comments = dRow["Comments"].ToString();

                foreach (DataRow dAddress in dtEvent.Tables["MDBAddresses"].Rows)
                {
                    cAddress NewAdd = new cAddress();
                    if (int.TryParse(dAddress["AddressID"].ToString(), out iTemp))
                        NewAdd.IntAddressID = iTemp;
                    NewAdd.StrAddress1 = dAddress["Address1"].ToString();
                    NewAdd.StrAddress2 = dAddress["Address2"].ToString();
                    NewAdd.StrCity = dAddress["City"].ToString();
                    NewAdd.StrStateID = dAddress["StateID"].ToString();
                    NewAdd.StrPostalCode = dAddress["PostalCode"].ToString();
                    NewAdd.StrCountry = dAddress["Country"].ToString();
                    NewAdd.StrComments = dAddress["Comments"].ToString();
                    SiteInfo.SiteAddress = NewAdd;
                }

                foreach (DataRow dPhone in dtEvent.Tables["MDBPhoneNumbers"].Rows)
                {
                    cPhone NewPhone = new cPhone();
                    if (int.TryParse(dPhone["PhoneNumberID"].ToString(), out iTemp))
                        NewPhone.PhoneNumberID = iTemp;
                    if (int.TryParse(dPhone["PhoneTypeID"].ToString(), out iTemp))
                        NewPhone.PhoneTypeID = iTemp;
                    NewPhone.IDD = dPhone["IDD"].ToString();
                    NewPhone.CountryCode = dPhone["CountryCode"].ToString();
                    NewPhone.AreaCode = dPhone["AreaCode"].ToString();
                    NewPhone.PhoneNumber = dPhone["PhoneNumber"].ToString();
                    NewPhone.Extension = dPhone["Extension"].ToString();
                    NewPhone.Comments = dPhone["Comments"].ToString();
                    SiteInfo.SitePhone = NewPhone;
                }
            }

            return iNumEventRecords;
        }


        /// <summary>
        /// Saves an Event. If the record status == Delete, then the record is deleted.
        /// </summary>
        /// <param name="sUserUpdating">User name of who is doing it.</param>
        /// <returns></returns>
        public int Save(string sUserUpdating)
        {
            if (RecStatus == RecordStatuses.Delete)
            {
                SortedList Params = new SortedList();
                Params.Add("@UserID", "1");
                Params.Add("@RecordID", EventID.ToString());

                cUtilities.PerformNonQuery("uspDelCMEvents", Params, "LARPortal", sUserUpdating);
            }
            else
            {
                SortedList Params = new SortedList();
                Params.Add("@UserID", "1");
                Params.Add("@EventID", EventID.ToString());
                Params.Add("@CampaignID", CampaignID.ToString());
                if (EventName != null)
                    Params.Add("@EventName", EventName);
                if (IGEventLocation != null)
                    Params.Add("@IGEventLocation", IGEventLocation);
                if (DecisionByDate.HasValue)
                    Params.Add("@DecisionByDate", DecisionByDate.ToString());
                if (NotificationDate.HasValue)
                    Params.Add("@NotificationDate", NotificationDate.Value.ToShortDateString());
                if (SharePlanningInfo.HasValue)
                    Params.Add("@SharePlanningInfo", SharePlanningInfo.ToString());
                if (StatusID.HasValue)
                    Params.Add("@StatusID", StatusID.ToString());
                if (EventDescription != null)
                    Params.Add("@EventDescription", EventDescription);
                if (StartDateTime.HasValue)
                {
                    Params.Add("@StartDate", StartDateTime.Value.ToShortDateString());
                    Params.Add("@StartTime", StartDateTime.Value.ToShortTimeString());
                }
                if (EndDateTime.HasValue)
                {
                    Params.Add("@EndDate", EndDateTime.Value.ToShortDateString());
                    Params.Add("@EndTime", EndDateTime.Value.ToShortTimeString());
                }
                Params.Add("@SiteID", SiteID.ToString());
                if (MaximumPCCount.HasValue)
                    Params.Add("@MaximumPCCount", MaximumPCCount.ToString());
                if (BaseNPCCount.HasValue)
                    Params.Add("@BaseNPCCount", BaseNPCCount.ToString());
                if (NPCOverrideRatio.HasValue)
                    Params.Add("@NPCOverrideRatio", NPCOverrideRatio.ToString());
                if (CapThresholdNotification.HasValue)
                    Params.Add("@CapThresholdNotification", CapThresholdNotification.ToString());
                if (CapMetNotification.HasValue)
                    Params.Add("@CapMetNotification", CapMetNotification.ToString());
                if (MaximumPCCount.HasValue)
                    Params.Add("@MaximumNPCCount", MaximumNPCCount.ToString());
                if (AutoApproveWaitListOpenings.HasValue)
                    Params.Add("@AutoApproveWaitListOpenings", AutoApproveWaitListOpenings.ToString());
                if (RegistrationOpenDateTime.HasValue)
                {
                    Params.Add("@RegistrationOpenDate", RegistrationOpenDateTime.Value.ToShortDateString());
                    Params.Add("@RegistrationOpenTime", RegistrationOpenDateTime.Value.ToShortTimeString());
                }
                if (PreregistrationDeadline.HasValue)
                    Params.Add("@PreregistrationDeadline", PreregistrationDeadline.Value.ToShortDateString());
                if (PreregistrationPrice.HasValue)
                    Params.Add("@PreregistrationPrice", PreregistrationPrice.ToString());
                if (LateRegistrationPrice.HasValue)
                    Params.Add("@LateRegistrationPrice", LateRegistrationPrice.ToString());
                if (CheckinPrice.HasValue)
                    Params.Add("@CheckinPrice", CheckinPrice.ToString());
                if (DaysToAutoCancelOtherPlayerRegistration.HasValue)
                    Params.Add("@DaysToAutoCancelOtherPlayerRegistration", DaysToAutoCancelOtherPlayerRegistration.ToString());
                if (PCFoodService.HasValue)
                    Params.Add("@PCFoodService", PCFoodService.ToString());
                if (NPCFoodService.HasValue)
                    Params.Add("@NPCFoodService", NPCFoodService.ToString());
                if (CookingFacilitiesAvailable.HasValue)
                    Params.Add("@CookingFacilitiesAvailable", CookingFacilitiesAvailable.ToString());
                if (RefrigeratorAvailable.HasValue)
                    Params.Add("@RefrigeratorAvailable", RefrigeratorAvailable.ToString());
                if (PELDeadlineDate.HasValue)
                    Params.Add("@PELDeadlineDate", PELDeadlineDate.Value.ToShortDateString());
                if (InfoSkillDeadlineDate.HasValue)
                    Params.Add("@InfoSkillDeadlineDate", InfoSkillDeadlineDate.Value.ToShortDateString());
                if (Comments != null)
                    Params.Add("@Comments", Comments);

                DataTable UpdateResults = cUtilities.LoadDataTable("uspInsUpdCMEvents", Params, "LARPortal", sUserUpdating, "cEvent.Save");
                if (UpdateResults.Rows.Count > 0)
                {
                    int iEventID;
                    if (int.TryParse(UpdateResults.Rows[0]["EventID"].ToString(), out iEventID))
                        EventID = iEventID;
                }
            }

            return 0;
        }
    }
}