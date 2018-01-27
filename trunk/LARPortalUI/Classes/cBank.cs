using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Data;

namespace LarpPortal.Classes
{
    [Serializable()]
    public class cBank
    {
       public cBank()
        {
            PlayerCPAuditID = -1;
            RecordStatus = RecordStatuses.Active;
        }

        private int _addedBy;
        private int _addedByID;
        private Boolean _allowCPDonation;
        private int _annualCharacterCap;
        private int _approvedByID;
        private Boolean _auditAdd;
        private Boolean _auditChange;
        private Boolean _auditDelete;
        private int _auditRequirementsID;
        private Boolean _backApply;
        private int _campaignCPOpportunityID;
        private int _campaignExchangeID;
        private int _campaignFromID;
        private int _campaignID;
        private string _campaignName;
        private int _campaignPlayerID;
        private int _campaignPlayerRoleID;
        private int _campaignToID;
        private double _cPAmount;
        private int _cPApprovedBy;
        private DateTime _cPApprovedDate;
        private DateTime _cPAssignmentDate;
        private double _cPCostPaid;
        private Boolean _cPEarnedForRole;
        private double _cPQuantityEarnedPerEvent;
        private double _cPValue;
        private DateTime _dateApproved;
        private Boolean _disableExchange;
        private DateTime _exchangeEndDate;
        private double _exchangeMultiplier;
        private DateTime _exchangeStartDate;
        private string _opportunityNotes;
        private Boolean _overrideAnnualCap;
        private Boolean _overrideEventCap;
        private int _playerCPAuditID;
        private int _reasonID;
        private DateTime _receiptDate;
        private int _receivedByID;
        private int _receivedFromCampaignID;
        private int _roleAlignmentID;
        private int _roleID;
        private int _sentToCampaignPlayerID;
        private string _staffNotes;
        private int _status;
        private int _statusID;
        private string _statusName;
        private string _statusType;
        private int _totalCharacterCap;
        private double _totalCP;
        private DateTime _transactionDate;

        public RecordStatuses RecordStatus { get; set; }
        public int AddedBy      // This field will track the player who completed the transaction
        {
            get { return _addedBy; }
            set { _addedBy = value; }
        }
        public int AddedByID        //This field will identify the person who added the opportunity
        {
            get { return _addedByID; }
            set { _addedByID = value; }
        }
        public Boolean AllowCPDonation      //this field represents if a campaign allows a player to donate their CP to another player
        {
            get { return _allowCPDonation; }
            set { _allowCPDonation = value; }
        }
        public int AnnualCharacterCap       //This field identifies the Annual CP Cap
        {
            get { return _annualCharacterCap; }
            set { _annualCharacterCap = value; }
        }
        public int ApprovedByID     // This field identifies who approved a campaign CP opportunity
        {
            get { return _approvedByID; }
            set { _approvedByID = value; }
        }
        public Boolean AuditAdd     // This field represents the need to audit additions to a table
        {
            get { return _auditAdd; }
            set { _auditAdd = value; }
        }
        public Boolean AuditChange      // This field represents the need to audit change to a field
        {
            get { return _auditChange; }
            set { _auditChange = value; }
        }
        public Boolean AuditDelete      // This field represents the need to audit deletions from a table
        {
            get { return _auditDelete; }
            set { _auditDelete = value; }
        }
        public int AuditRequirementsID      // The number that identifies the unique  requirement
        {
            get { return _auditRequirementsID; }
            set { _auditRequirementsID = value; }
        }
        public Boolean BackApply        // This field identifies if the campaign allows exchange value ti be back applied to a character
        {
            get { return _backApply; }
            set { _backApply = value; }
        }
        public int CampaignCPOpportunityID      //  This field will identify the CP opportunity campaign record
        {
            get { return _campaignCPOpportunityID; }
            set { _campaignCPOpportunityID = value; }
        }
        public int CampaignExchangeID       // This field identifies the unique campaign exchange
        {
            get { return _campaignExchangeID; }
            set { _campaignExchangeID = value; }
        }
        public int CampaignFromID       // This field represents the campaign that is delivering the CP transfer
        {
            get { return _campaignFromID; }
            set { _campaignFromID = value; }
        }
        public int CampaignID       // This field identifies the campaign associated with the campaign attribute record
        {
            get { return _campaignID; }
            set { _campaignID = value; }
        }
        public string CampaignName       // This is the name which players will associate the game they are playing Campaign Name
        {
            get { return _campaignName; }
            set { _campaignName = value; }
        }
        public int CampaignPlayerID       // This field will identify is the CP was earned by a player not tied to a specific event
        {
            get { return _campaignPlayerID; }
            set { _campaignPlayerID = value; }
        }
        public int CampaignPlayerRoleID       // This field will uniquely identify the player's roles for each campaign
        {
            get { return _campaignPlayerRoleID; }
            set { _campaignPlayerRoleID = value; }
        }
        public int CampaignToID       // This field identifies the unique campaign exchange
        {
            get { return _campaignToID; }
            set { _campaignToID = value; }
        }
        public double CPAmount       // This field is the amount of CP earned or spent
        {
            get { return _cPAmount; }
            set { _cPAmount = value; }
        }
        public int CPApprovedBy       // This field will track the date the CP was approved by staff
        {
            get { return _cPApprovedBy; }
            set { _cPApprovedBy = value; }
        }
        public DateTime CPApprovedDate       // This field will track the date the CP was approved by staff
        {
            get { return _cPApprovedDate; }
            set { _cPApprovedDate = value; }
        }
        public DateTime CPAssignmentDate       // This field identifies when CP was approved and assigned for the item
        {
            get { return _cPAssignmentDate; }
            set { _cPAssignmentDate = value; }
        }
        public double CPCostPaid       // Cost paid for this skill (needs storage because of variable cost systems)
        {
            get { return _cPCostPaid; }
            set { _cPCostPaid = value; }
        }
        public Boolean CPEarnedForRole       // This field identifies the CP qty Earned per event
        {
            get { return _cPEarnedForRole; }
            set { _cPEarnedForRole = value; }
        }
        public double CPQuantityEarnedPerEvent       // CP QTY Earned/ event
        {
            get { return _cPQuantityEarnedPerEvent; }
            set { _cPQuantityEarnedPerEvent = value; }
        }
        public double CPValue       // This field identifies the amount of CP the opportunity is worth
        {
            get { return _cPValue; }
            set { _cPValue = value; }
        }
        public DateTime DateApproved       // The date staff approved the PEL
        {
            get { return _dateApproved; }
            set { _dateApproved = value; }
        }
        public Boolean DisableExchange       // This field allows the owner  to stop exchanging CP 
        {
            get { return _disableExchange; }
            set { _disableExchange = value; }
        }
        public DateTime ExchangeEndDate       // This field represents the date the campaign ended exchanging CP
        {
            get { return _exchangeEndDate; }
            set { _exchangeEndDate = value; }
        }
        public double ExchangeMultiplier       // This field indicates if the CP is multiplied  or converted between games and is used when games apply a different range for same services
        {
            get { return _exchangeMultiplier; }
            set { _exchangeMultiplier = value; }
        }
        public DateTime ExchangeStartDate       // This field represents the date the campaign starting exchanging CP\
        {
            get { return _exchangeStartDate; }
            set { _exchangeStartDate = value; }
        }
        public string OpportunityNotes       // This field will provide additional comments about the opportunity
        {
            get { return _opportunityNotes; }
            set { _opportunityNotes = value; }
        }
        public Boolean OverrideAnnualCap       // This field idetnifies if campaign applies a non standard annual CA
        {
            get { return _overrideAnnualCap; }
            set { _overrideAnnualCap = value; }
        }
        public Boolean OverrideEventCap       // This field idetnifies if campaign applies a non standard event CA
        {
            get { return _overrideEventCap; }
            set { _overrideEventCap = value; }
        }
        public int PlayerCPAuditID       // This field will uniquely identify the combination of Player CP
        {
            get { return _playerCPAuditID; }
            set { _playerCPAuditID = value; }
        }
        public int ReasonID       // This field will identify the type of CP earned for transfer process
        {
            get { return _reasonID; }
            set { _reasonID = value; }
        }
        public DateTime ReceiptDate       // This field identifies the date the CP opportunity item was received
        {
            get { return _receiptDate; }
            set { _receiptDate = value; }
        }
        public int ReceivedByID       // This field identifies the date the CP opportunity item was received
        {
            get { return _receivedByID; }
            set { _receivedByID = value; }
        }
        public int ReceivedFromCampaignID       // This field will track from which campaign the CP was earned
        {
            get { return _receivedFromCampaignID; }
            set { _receivedFromCampaignID = value; }
        }
        public int RoleAlignmentID       // This field will track the role alignment specific to the campaign
        {
            get { return _roleAlignmentID; }
            set { _roleAlignmentID = value; }
        }
        public int RoleID       // This field will identify the role associated with the player and campaign
        {
            get { return _roleID; }
            set { _roleID = value; }
        }
        public int SentToCampaignPlayerID       // This field will track from which campaign CP bank  the CP was transferred 
        {
            get { return _sentToCampaignPlayerID; }
            set { _sentToCampaignPlayerID = value; }
        }
        public string StaffNotes       // This field tracks staff comments associated with Players Notes
        {
            get { return _staffNotes; }
            set { _staffNotes = value; }
        }
        public int Status       // This field tracks the status of a info skill request
        {
            get { return _status; }
            set { _status = value; }
        }
        public int StatusID       // This field will track the status of the CP opportunity
        {
            get { return _statusID; }
            set { _statusID = value; }
        }
        public string StatusName       // This field names or describes the status
        {
            get { return _statusName; }
            set { _statusName = value; }
        }
        public string StatusType       // StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }
        /// <summary>
        /// This field identifies the Character CAP
        /// </summary>
        public int TotalCharacterCap
        {
            get { return _totalCharacterCap; }
            set { _totalCharacterCap = value; }
        }
        public double TotalCP       // This field will track the characters total CP however earned
        {
            get { return _totalCP; }
            set { _totalCP = value; }
        }
        public DateTime TransactionDate       // This field will track the date the CP was earned or spent
        {
            get { return _transactionDate; }
            set { _transactionDate = value; }
        }

        /// <summary>
        /// This is called to retrieve a list of a players' points.
        /// </summary>
        /// <returns>
        /// Returns a datatable containing all a players' points
        /// </returns>
        public int LoadPoints(int UserIDToLoad)
        {
            int inumPointRecords = 0;
            SortedList slStoredProcParameters = new SortedList();
            slStoredProcParameters.Add("@UserID", UserIDToLoad);
            DataTable dtMyPoints = new DataTable();
            dtMyPoints = cUtilities.LoadDataTable("uspGetMyPoints", slStoredProcParameters, "LARPortal", UserIDToLoad.ToString(), "LoadPoints");
            return inumPointRecords;
        }

        /// <summary>
        /// This is called to retrieve a list of a players' campaigns.
        /// </summary>
        /// <returns>
        /// Returns a datatable containing all a players' campaigns.
        /// </returns>
        public DataTable GetMyCampaigns(int UserID)
        {
            SortedList slStoredProcParameters = new SortedList();
            slStoredProcParameters.Add("@UserID", UserID);
            DataTable dtMyCampaigns = new DataTable();
            dtMyCampaigns = cUtilities.LoadDataTable("uspGetMyCampaigns", slStoredProcParameters, "LARPortal", "rpierce", "GetMyCampaigns");
            return dtMyCampaigns;
        }

        /// <summary>
        /// This is called to retrieve a list of a players' roles for a given campaign
        /// </summary>
        /// <returns>
        /// Returns a datatable containing all a players' roles for a given campaign.
        /// </returns>
        public DataTable GetMyRoles(int UserID, int CampaignID)
        {
            SortedList slStoredProcParameters = new SortedList();
            slStoredProcParameters.Add("@UserID", UserID);
            // CampaignID = 0 for full list
            slStoredProcParameters.Add("@CampaignID", CampaignID);
            DataTable dtMyRoles = new DataTable();
            dtMyRoles = cUtilities.LoadDataTable("uspGetMyRoles", slStoredProcParameters, "LARPortal", "rpierce", "GetMyRoles");
            return dtMyRoles;
        }

        ///<sumarry>
        /// This is called to retrieve a list of all point earning opportunities for a given campaign
        /// </sumarry>
        /// <returns>
        /// Returns a datatable containing all standard point earning opportunities for a given campaign
        /// </returns>
        public DataTable GetCampaignPointOpportunities(int CampaignID)
        {
            SortedList slStoredProcParameters = new SortedList();
            slStoredProcParameters.Add("@CampaignID", CampaignID);
            DataTable dtCampaignPointOpportunities = new DataTable();
            dtCampaignPointOpportunities = cUtilities.LoadDataTable("uspGetCampaignPointOpportunities", slStoredProcParameters, "LARPortal", "rpierce", "GetCampaignPointOpportunities");
            return dtCampaignPointOpportunities;
        }

        ///<sumarry>
        /// This is called to retrieve a list of all point earning opportunities for a given game system
        /// </sumarry>
        /// <returns>
        /// Returns a datatable containing all standard point earning opportunities for a given game system
        /// </returns>
        public DataTable GetGameSystemPointOpportunities(int GameSystemID)
        {
            SortedList slStoredProcParameters = new SortedList();
            slStoredProcParameters.Add("@GameSystemID", GameSystemID);
            DataTable dtGameSystemPointOpportunities = new DataTable();
            dtGameSystemPointOpportunities = cUtilities.LoadDataTable("uspGetGameSystemPointOpportunities", slStoredProcParameters, "LARPortal", "rpierce", "GetGameSystemPointOpportunities");
            return dtGameSystemPointOpportunities;
        }

    }
}