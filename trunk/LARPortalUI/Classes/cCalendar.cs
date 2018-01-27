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
    public class cCalendar
    {
       public cCalendar()
        {
            OccasionID = -1;
            RecordStatus = RecordStatuses.Active;
        }
        private int _addedBy;
        private int _addedByID;
        private int _addedByPlayerID;
        private Boolean _auditAdd;
        private Boolean _auditChange;
        private Boolean _auditDelete;
        private int _auditRequirementsID;
        private int _campaignID;
        private string _campaignName;
        private int _campaignOccasionExceptionID;
        private DateTime _dateApproved;
        private string _endTime;
        private string _eventDescription;
        private DateTime _eventEndDate;
        private string _eventEndTime;
        private string _eventName;
        private int _eventNumber;
        private DateTime _eventStartDate;
        private string _eventStartTime;
        private int _exceptionLevel;
        private int _gameSystemID;
        private DateTime _moonDate;
        private string _moonPhase;
        private int _moonPhaseID;
        private int _occasionCampaignShareID;
        private DateTime _occasionDate;
        private int _occasionID;
        private string _occasionName;
        private string _occasionNotes;
        private string _occasionPostalCode;
        private int _occasionType;
        private int _occasionTypeID;
        private string _ownerComments;
        private Boolean _recurring;
        private int _recurringDay;
        private Boolean _recurringFixed;
        private int _recurringMonth;
        private Boolean _showPublicly;
        private int _siteID;
        private string _siteMapURL;
        private string _siteName;
        private string _startTime;
        private int _status;
        private string _statusName;
        private string _statusType;
        public RecordStatuses RecordStatus { get; set; }
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
        public DateTime DateApproved       // 
        {
            get { return _dateApproved; }
            set { _dateApproved = value; }
        }
        public string EndTime       // 
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        public string EventDescription       // 
        {
            get { return _eventDescription; }
            set { _eventDescription = value; }
        }
        public DateTime EventEndDate       // 
        {
            get { return _eventEndDate; }
            set { _eventEndDate = value; }
        }
        public string EventEndTime       // 
        {
            get { return _eventEndTime; }
            set { _eventEndTime = value; }
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
        public int GameSystemID       // 
        {
            get { return _gameSystemID; }
            set { _gameSystemID = value; }
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
        public Boolean ShowPublicly       // 
        {
            get { return _showPublicly; }
            set { _showPublicly = value; }
        }
        public int SiteID       // 
        {
            get { return _siteID; }
            set { _siteID = value; }
        }
        public string SiteMapURL       // 
        {
            get { return _siteMapURL; }
            set { _siteMapURL = value; }
        }
        public string SiteName       // 
        {
            get { return _siteName; }
            set { _siteName = value; }
        }
        public string StartTime       // 
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        public int Status       // 
        {
            get { return _status; }
            set { _status = value; }
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
    }
}
