using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

namespace LarpPortal.Classes
{
    [Serializable()]
    public class cRole
    {
        public cRole()
        {
            roleID = -1;
            campaignRoleID = -1;
            RecordStatus = RecordStatuses.Active;
        }
        public RecordStatuses RecordStatus { get; set; }
        private int _addedBy;
        /// <summary>
        /// Comment
        /// </summary>
        public int addedBy
        {
            get { return _addedBy; }
            set { _addedBy = value; }
        }
        private int _addedByID;
        /// <summary>
        /// Comment
        /// </summary>
        public int addedByID
        {
            get { return _addedByID; }
            set { _addedByID = value; }
        }
        private int _applicableToRoleAlignment;
        /// <summary>
        /// Comment
        /// </summary>
        public int applicableToRoleAlignment
        {
            get { return _applicableToRoleAlignment; }
            set { _applicableToRoleAlignment = value; }
        }
        private int _approvalLevelID;
        /// <summary>
        /// Comment
        /// </summary>
        public int approvalLevelID
        {
            get { return _approvalLevelID; }
            set { _approvalLevelID = value; }
        }
        private int _approvalLevelType;
        /// <summary>
        /// Comment
        /// </summary>
        public int approvalLevelType
        {
            get { return _approvalLevelType; }
            set { _approvalLevelType = value; }
        }
        private int _approvalLevelTypeID;
        /// <summary>
        /// Comment
        /// </summary>
        public int approvalLevelTypeID
        {
            get { return _approvalLevelTypeID; }
            set { _approvalLevelTypeID = value; }
        }
        private int _approvedByID;
        /// <summary>
        /// Comment
        /// </summary>
        public int approvedByID
        {
            get { return _approvedByID; }
            set { _approvedByID = value; }
        }
        private Boolean _auditAdd;
        /// <summary>
        /// Comment
        /// </summary>
        public Boolean auditAdd
        {
            get { return _auditAdd; }
            set { _auditAdd = value; }
        }
        private Boolean _auditChange;
        /// <summary>
        /// Comment
        /// </summary>
        public Boolean auditChange
        {
            get { return _auditChange; }
            set { _auditChange = value; }
        }
        private Boolean _auditDelete;
        /// <summary>
        /// Comment
        /// </summary>
        public Boolean auditDelete
        {
            get { return _auditDelete; }
            set { _auditDelete = value; }
        }
        private int _auditRequirementsID;
        /// <summary>
        /// Comment
        /// </summary>
        public int auditRequirementsID
        {
            get { return _auditRequirementsID; }
            set { _auditRequirementsID = value; }
        }
        private Boolean _autoApprove;
        /// <summary>
        /// Comment
        /// </summary>
        public Boolean autoApprove
        {
            get { return _autoApprove; }
            set { _autoApprove = value; }
        }
        private int _campaignID;
        /// <summary>
        /// Comment
        /// </summary>
        public int campaignID
        {
            get { return _campaignID; }
            set { _campaignID = value; }
        }
        private string _campaignName;
        /// <summary>
        /// Comment
        /// </summary>
        public string campaignName
        {
            get { return _campaignName; }
            set { _campaignName = value; }
        }
        private int _campaignRoleID;
        /// <summary>
        /// Comment
        /// </summary>
        public int campaignRoleID
        {
            get { return _campaignRoleID; }
            set { _campaignRoleID = value; }
        }
        private Boolean _canRegisterForEvents;
        /// <summary>
        /// Comment
        /// </summary>
        public Boolean canRegisterForEvents
        {
            get { return _canRegisterForEvents; }
            set { _canRegisterForEvents = value; }
        }
        private DateTime _dateAdded;
        /// <summary>
        /// Comment
        /// </summary>
        public DateTime dateAdded
        {
            get { return _dateAdded; }
            set { _dateAdded = value; }
        }
        private DateTime _dateApproved;
        /// <summary>
        /// Comment
        /// </summary>
        public DateTime dateApproved
        {
            get { return _dateApproved; }
            set { _dateApproved = value; }
        }
        private DateTime _dateChanged;
        /// <summary>
        /// Comment
        /// </summary>
        public DateTime dateChanged
        {
            get { return _dateChanged; }
            set { _dateChanged = value; }
        }
        private DateTime _dateDeleted;
        /// <summary>
        /// Comment
        /// </summary>
        public DateTime dateDeleted
        {
            get { return _dateDeleted; }
            set { _dateDeleted = value; }
        }
        private DateTime _expirationDate;
        /// <summary>
        /// Comment
        /// </summary>
        public DateTime expirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }
        private Boolean _requestable;
        /// <summary>
        /// Comment
        /// </summary>
        public Boolean requestable
        {
            get { return _requestable; }
            set { _requestable = value; }
        }
        private int _roleAlignment;
        /// <summary>
        /// Comment
        /// </summary>
        public int roleAlignment
        {
            get { return _roleAlignment; }
            set { _roleAlignment = value; }
        }
        private int _roleAlignmentID;
        /// <summary>
        /// Comment
        /// </summary>        
        public int roleAlignmentID
        {
            get { return _roleAlignmentID; }
            set { _roleAlignmentID = value; }
        }
        private Boolean _roleAssignmentAuthority;
        /// <summary>
        /// Comment
        /// </summary>
        public Boolean roleAssignmentAuthority
        {
            get { return _roleAssignmentAuthority; }
            set { _roleAssignmentAuthority = value; }
        }
        private int _roleAssignmentLevel;
        /// <summary>
        /// Comment
        /// </summary>
        public int roleAssignmentLevel
        {
            get { return _roleAssignmentLevel; }
            set { _roleAssignmentLevel = value; }
        }
        private string _roleDescription;
        /// <summary>
        /// Comment
        /// </summary>
        public string roleDescription
        {
            get { return _roleDescription; }
            set { _roleDescription = value; }
        }
        private int _roleID;
        /// <summary>
        /// Comment
        /// </summary>
        public int roleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }
        private int _roleLevel;
        /// <summary>
        /// Comment
        /// </summary>
        public int roleLevel
        {
            get { return _roleLevel; }
            set { _roleLevel = value; }
        }
        private string _roleTier;
        /// <summary>
        /// Comment
        /// </summary>
        public string roleTier
        {
            get { return _roleTier; }
            set { _roleTier = value; }
        }
        private int _status;
        /// <summary>
        /// Comment
        /// </summary>
        public int status
        {
            get { return _status; }
            set { _status = value; }
        }
        private int _statusID;
        /// <summary>
        /// Comment
        /// </summary>
        public int statusID
        {
            get { return _statusID; }
            set { _statusID = value; }
        }
        private string _statusName;
        /// <summary>
        /// Comment
        /// </summary>
        public string statusName
        {
            get { return _statusName; }
            set { _statusName = value; }
        }
        private string _statusType;
        /// <summary>
        /// Comment
        /// </summary>
        public string statusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }
        private int _userID;
        /// <summary>
        /// Comment
        /// </summary>
        public int userID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        /// <summary>
        /// This is called to retrieve a list of a player's roles.  Campaign 0 returns all roles for the user for all campaigns in which they have roles.
        /// </summary>
        /// <returns>
        /// Returns a datatable containing all of a player's roles (optional CampaignID narrows it to one campaign).
        /// </returns>
        public DataTable GetRoles(int UserID, int CampaignID)
        {
            SortedList slStoredProcParameters = new SortedList();
            slStoredProcParameters.Add("@UserID", UserID);
            // CampaignID = 0 for full list
            slStoredProcParameters.Add("@CampaignID", CampaignID);
            DataTable dtMyRoles = new DataTable();
            dtMyRoles = cUtilities.LoadDataTable("uspGetRoles", slStoredProcParameters, "LARPortal", UserID.ToString(), "GetMyRoles");

            return dtMyRoles;
        }

        /// <summary>
        /// This is called to retrieve a list of the roles being used by a campaign.
        /// </summary>
        /// <returns>
        /// Returns a datatable containing all of the roles being used by a campaign.
        /// </returns>
        public DataTable GetCampaignRoles(int CampaignID, int UserID)
        {
            SortedList slStoredProcParameters = new SortedList();
            slStoredProcParameters.Add("@CampaignID", CampaignID);
            DataTable dtCampaignRoles = new DataTable();
            dtCampaignRoles = cUtilities.LoadDataTable("uspGetCampaignRoles", slStoredProcParameters, "LARPortal", UserID.ToString(), "GetCampaignRoles");
            return dtCampaignRoles;
        }

        /// <summary>
        /// This will write the association of a new role with a campaign.
        /// </summary>
        /// <returns>
        /// Writes to the Campaign Role association table. Returns true of false.
        /// </returns>
        public Boolean SaveCampaignRole(int CampaignRoleID, int UserID, int CampaignID, int RoleID, bool AutoApprove, bool Requestable, string Comments, bool DeleteRole = false)
        {
            Boolean bInsertComplete = false;
            string stStoredProc;
            SortedList slStoredProcParameters = new SortedList();
            if (DeleteRole)     //Delete role is true
            {
                slStoredProcParameters.Add("@UserID", UserID);
                slStoredProcParameters.Add("@CampaignRoleID", CampaignRoleID);
                stStoredProc = "uspDelCMCampaignRoles";
            }
            else                //Delete role is false so it's add or change
            {
                slStoredProcParameters.Add("@UserID", UserID);
                slStoredProcParameters.Add("@CampaignRoleID", CampaignRoleID);  // This will be -1 for additions
                slStoredProcParameters.Add("@CampaignID", CampaignID);
                slStoredProcParameters.Add("@RoleID", RoleID);
                slStoredProcParameters.Add("@AutoApprove", AutoApprove);
                slStoredProcParameters.Add("@Requestable", Requestable);
                slStoredProcParameters.Add("@Comments", Comments);
                stStoredProc = "uspInsUpdCMCampaignRoles";
              }
            bInsertComplete = cUtilities.PerformNonQueryBoolean(stStoredProc, slStoredProcParameters, "LARPortal", UserID.ToString());
            return bInsertComplete;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public Boolean AssignRolesToUsers()
            // Need to pass in paired lists of users and roles, add and delete and process accordingly (see page 79 of 91)
            // Or do we just call the above routine once for every addition or deletion?
        {
            Boolean bAssignComplete = false;



            return bAssignComplete;
        }



    }
}
