using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;

namespace LarpPortal.Classes
{
    [Serializable()]
    public class cSchedule
    {
       public cSchedule()
        {
            RecordStatus = RecordStatuses.Active;
        }
        private DateTime _acceptedDate;
        private int _addedBy;
        private int _addedByID;
        private int _addedByPlayerID;
        private int _approvalLevelID;
        private int _approvalLevelType;
        private int _approvalLevelTypeID;
        private int _approvedByID;
        private Boolean _auditAdd;
        private Boolean _auditChange;
        private Boolean _auditDelete;
        private int _auditRequirementsID;
        private int _baseNPCCount;
        private int _campaignID;
        private string _campaignName;
        private int _campaignOccasionExceptionID;
        private int _campaignPlayerID;
        private int _capacity;
        private Boolean _createEvent;
        private DateTime _dateApproved;
        private DateTime _dateSubmitted;
        private DateTime _decisionByDate;
        private int _eventDatePriorityID;
        private string _eventDescription;
        private string _eventName;
        private int _eventNumber;
        private DateTime _eventStartDate;
        private string _eventStartTime;
        private int _exceptionLevel;
        private int _maximumPCCount;
        private DateTime _moonDate;
        private string _moonPhase;
        private int _moonPhaseID;
        private DateTime _notificationDate;
        private int _occasionCampaignShareID;
        private DateTime _occasionDate;
        private int _occasionID;
        private string _occasionName;
        private string _occasionNotes;
        private string _occasionPostalCode;
        private int _occasionType;
        private int _occasionTypeID;
        private string _ownerComments;
        private int _ownerCriticalityRating;
        private string _plannerComments;
        private int _playerOccasionExceptionID;
        private int _preferredSite;
        private int _primarySiteID;
        private int _priorityNumber;
        private Boolean _recurring;
        private int _recurringDay;
        private Boolean _recurringFixed;
        private int _recurringMonth;
        private int _schedulingID;
        private int _secondarySite;
        private int _secondarySiteID;
        private Boolean _shareEvent;
        private Boolean _sharePlanningInfo;
        private Boolean _showPublicly;
        private int _siteAvailabilityDateID;
        private int _siteID;
        private string _siteName;
        private int _status;
        private int _statusID;
        private string _statusName;
        private string _statusType;
        private int _userID;

        public RecordStatuses RecordStatus { get; set; }
        public DateTime AcceptedDate       // 
        {
            get { return _acceptedDate; }
            set { _acceptedDate = value; }
        }
        public int AddedBy       // 
        {
            get { return _addedBy; }
            set { _addedBy = value; }
        }
        public int AddedByID       // 
        {
            get { return _addedByID; }
            set { _addedByID = value; }
        }
        public int AddedByPlayerID       // 
        {
            get { return _addedByPlayerID; }
            set { _addedByPlayerID = value; }
        }
        public int ApprovalLevelID       // 
        {
            get { return _approvalLevelID; }
            set { _approvalLevelID = value; }
        }
        public int ApprovalLevelType       // 
        {
            get { return _approvalLevelType; }
            set { _approvalLevelType = value; }
        }
        public int ApprovalLevelTypeID       // 
        {
            get { return _approvalLevelTypeID; }
            set { _approvalLevelTypeID = value; }
        }
        public int ApprovedByID       // 
        {
            get { return _approvedByID; }
            set { _approvedByID = value; }
        }
        public Boolean AuditAdd       // 
        {
            get { return _auditAdd; }
            set { _auditAdd = value; }
        }
        public Boolean AuditChange       // 
        {
            get { return _auditChange; }
            set { _auditChange = value; }
        }
        public Boolean AuditDelete       // 
        {
            get { return _auditDelete; }
            set { _auditDelete = value; }
        }
        public int AuditRequirementsID       // 
        {
            get { return _auditRequirementsID; }
            set { _auditRequirementsID = value; }
        }
        public int BaseNPCCount       // 
        {
            get { return _baseNPCCount; }
            set { _baseNPCCount = value; }
        }
        public int CampaignID       // 
        {
            get { return _campaignID; }
            set { _campaignID = value; }
        }
        public string CampaignName       // 
        {
            get { return _campaignName; }
            set { _campaignName = value; }
        }
        public int CampaignOccasionExceptionID       // 
        {
            get { return _campaignOccasionExceptionID; }
            set { _campaignOccasionExceptionID = value; }
        }
        public int CampaignPlayerID       // 
        {
            get { return _campaignPlayerID; }
            set { _campaignPlayerID = value; }
        }
        public int Capacity       // 
        {
            get { return _capacity; }
            set { _capacity = value; }
        }
        public Boolean CreateEvent       // 
        {
            get { return _createEvent; }
            set { _createEvent = value; }
        }
        public DateTime DateApproved       // 
        {
            get { return _dateApproved; }
            set { _dateApproved = value; }
        }
        public DateTime DateSubmitted       // 
        {
            get { return _dateSubmitted; }
            set { _dateSubmitted = value; }
        }
        public DateTime DecisionByDate       // 
        {
            get { return _decisionByDate; }
            set { _decisionByDate = value; }
        }
        public int EventDatePriorityID       // 
        {
            get { return _eventDatePriorityID; }
            set { _eventDatePriorityID = value; }
        }
        public string EventDescription       // 
        {
            get { return _eventDescription; }
            set { _eventDescription = value; }
        }
        public string EventName       // 
        {
            get { return _eventName; }
            set { _eventName = value; }
        }
        public int EventNumber       // 
        {
            get { return _eventNumber; }
            set { _eventNumber = value; }
        }
        public DateTime EventStartDate       // 
        {
            get { return _eventStartDate; }
            set { _eventStartDate = value; }
        }
        public string EventStartTime       // 
        {
            get { return _eventStartTime; }
            set { _eventStartTime = value; }
        }
        public int ExceptionLevel       // 
        {
            get { return _exceptionLevel; }
            set { _exceptionLevel = value; }
        }
        public int MaximumPCCount       // 
        {
            get { return _maximumPCCount; }
            set { _maximumPCCount = value; }
        }
        public DateTime MoonDate       // 
        {
            get { return _moonDate; }
            set { _moonDate = value; }
        }
        public string MoonPhase       // 
        {
            get { return _moonPhase; }
            set { _moonPhase = value; }
        }
        public int MoonPhaseID       // 
        {
            get { return _moonPhaseID; }
            set { _moonPhaseID = value; }
        }
        public DateTime NotificationDate       // 
        {
            get { return _notificationDate; }
            set { _notificationDate = value; }
        }
        public int OccasionCampaignShareID       // 
        {
            get { return _occasionCampaignShareID; }
            set { _occasionCampaignShareID = value; }
        }
        public DateTime OccasionDate       // 
        {
            get { return _occasionDate; }
            set { _occasionDate = value; }
        }
        public int OccasionID       // 
        {
            get { return _occasionID; }
            set { _occasionID = value; }
        }
        public string OccasionName       // 
        {
            get { return _occasionName; }
            set { _occasionName = value; }
        }
        public string OccasionNotes       // 
        {
            get { return _occasionNotes; }
            set { _occasionNotes = value; }
        }
        public string OccasionPostalCode       // 
        {
            get { return _occasionPostalCode; }
            set { _occasionPostalCode = value; }
        }
        public int OccasionType       // 
        {
            get { return _occasionType; }
            set { _occasionType = value; }
        }
        public int OccasionTypeID       // 
        {
            get { return _occasionTypeID; }
            set { _occasionTypeID = value; }
        }
        public string OwnerComments       // 
        {
            get { return _ownerComments; }
            set { _ownerComments = value; }
        }
        public int OwnerCriticalityRating       // 
        {
            get { return _ownerCriticalityRating; }
            set { _ownerCriticalityRating = value; }
        }
        public string PlannerComments       // 
        {
            get { return _plannerComments; }
            set { _plannerComments = value; }
        }
        public int PlayerOccasionExceptionID       // 
        {
            get { return _playerOccasionExceptionID; }
            set { _playerOccasionExceptionID = value; }
        }
        public int PreferredSite       // 
        {
            get { return _preferredSite; }
            set { _preferredSite = value; }
        }
        public int PrimarySiteID       // 
        {
            get { return _primarySiteID; }
            set { _primarySiteID = value; }
        }
        public int PriorityNumber       // 
        {
            get { return _priorityNumber; }
            set { _priorityNumber = value; }
        }
        public Boolean Recurring       // 
        {
            get { return _recurring; }
            set { _recurring = value; }
        }
        public int RecurringDay       // 
        {
            get { return _recurringDay; }
            set { _recurringDay = value; }
        }
        public Boolean RecurringFixed       // 
        {
            get { return _recurringFixed; }
            set { _recurringFixed = value; }
        }
        public int RecurringMonth       // 
        {
            get { return _recurringMonth; }
            set { _recurringMonth = value; }
        }
        public int SchedulingID       // 
        {
            get { return _schedulingID; }
            set { _schedulingID = value; }
        }
        public int SecondarySite       // 
        {
            get { return _secondarySite; }
            set { _secondarySite = value; }
        }
        public int SecondarySiteID       // 
        {
            get { return _secondarySiteID; }
            set { _secondarySiteID = value; }
        }
        public Boolean ShareEvent       // 
        {
            get { return _shareEvent; }
            set { _shareEvent = value; }
        }
        public Boolean SharePlanningInfo       // 
        {
            get { return _sharePlanningInfo; }
            set { _sharePlanningInfo = value; }
        }
        public Boolean ShowPublicly       // 
        {
            get { return _showPublicly; }
            set { _showPublicly = value; }
        }
        public int SiteAvailabilityDateID       // 
        {
            get { return _siteAvailabilityDateID; }
            set { _siteAvailabilityDateID = value; }
        }
        public int SiteID       // 
        {
            get { return _siteID; }
            set { _siteID = value; }
        }
        public string SiteName       // 
        {
            get { return _siteName; }
            set { _siteName = value; }
        }
        public int Status       // 
        {
            get { return _status; }
            set { _status = value; }
        }
        public int StatusID       // 
        {
            get { return _statusID; }
            set { _statusID = value; }
        }
        public string StatusName       // 
        {
            get { return _statusName; }
            set { _statusName = value; }
        }
        public string StatusType       // 
        {
            get { return _statusType; }
            set { _statusType = value; }
        }
        public int UserID       // 
        {
            get { return _userID; }
            set { _userID = value; }
        }
    }
}
