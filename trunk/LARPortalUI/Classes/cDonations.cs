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
    public class cDonations
    {
       public cDonations()
        {
            //RecordStatus = RecordStatuses.Active;
        }

        //private DateTime _acceptedDate;
        private int _addedBy;
        private int _addedByID;
        private int _approvalLevelID;
        private int _approvalLevelType;
        private int _approvalLevelTypeID;
        private int _approvedByID;
        private Boolean _auditAdd;
        private Boolean _auditChange;
        private Boolean _auditDelete;
        private int _auditRequirementsID;
        private int _campaignID;
        private int _campaignName;
        private DateTime _dateApproved;
        private string _staffNotes;
        private int _status;
        private int _statusID;
        private string _statusName;
        private string _statusType;
        private int _userID;
        public RecordStatuses RecordStatus { get; set; }
        //public DateTime AcceptedDate       // Date player accepted the waiver
        //{
        //    get { return _acceptedDate; }
        //}
        public int AddedBy       // This field will track the player who completed the transaction
        {
            get { return _addedBy; }
            set { _addedBy = value; }
        }
        public int AddedByID       // This field will identify the person who added the opportunity
        {
            get { return _addedByID; }
            set { _addedByID = value; }
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
        public int CampaignID       // 
        {
            get { return _campaignID; }
            set { _campaignID = value; }
        }
        public int CampaignName       // 
        {
            get { return _campaignName; }
            set { _campaignName = value; }
        }
        public DateTime DateApproved       // 
        {
            get { return _dateApproved; }
            set { _dateApproved = value; }
        }
        public string StaffNotes       // 
        {
            get { return _staffNotes; }
            set { _staffNotes = value; }
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
