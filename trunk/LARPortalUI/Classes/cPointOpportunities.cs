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
    public class cPointOpportunities
    {

        private int _UserID;
        private int _CampaignCPOpportunityID;
        private int _CampaignPlayerID;
        private int _CharacterID;
        private int _CampaignCPOpportunityDefaultID;
        private int _EventID;
        private string _Description;
        private string _OpportunityNotes;
        private string _ExampleURL;
        private int _ReasonID;
        private int _StatusID;
        private int _AddedByID;
        private double _CPValue;
        private int _ApprovedByID;
        private DateTime _ReceiptDate;
        private int _ReceivedByID;
        private DateTime _CPAssignmentDate;
        private string _StaffComments;
        private string _Comments;
        private double _MaximumCPPerYear;
        private bool _AllowCPDonation;
        private double _EventCharacterCap;
        private double _AnnualCharacterCap;
        private double _TotalCharacterCap;
        private int _EarliestCPApplicationYear;
        private int _CampaignID;
        private double _TotalCP;
        private int _PLAuditStatus;

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public int CampaignCPOpportunityID
        {
            get { return _CampaignCPOpportunityID; }
            set { _CampaignCPOpportunityID = value; }
        }
        public int CampaignPlayerID
        {
            get { return _CampaignPlayerID; }
            set { _CampaignPlayerID = value; }
        }
        public int CharacterID
        {
            get { return _CharacterID; }
            set { _CharacterID = value; }
        }
        public int CampaignCPOpportunityDefaultID
        {
            get { return _CampaignCPOpportunityDefaultID; }
            set { _CampaignCPOpportunityDefaultID = value; }
        }
        public int EventID
        {
            get { return _EventID; }
            set { _EventID = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string OpportunityNotes
        {
            get { return _OpportunityNotes; }
            set { _OpportunityNotes = value; }
        }
        public string ExampleURL
        {
            get { return _ExampleURL; }
            set { _ExampleURL = value; }
        }
        public int ReasonID
        {
            get { return _ReasonID; }
            set { _ReasonID = value; }
        }
        public int StatusID
        {
            get { return _StatusID; }
            set { _StatusID = value; }
        }
        public int AddedByID
        {
            get { return _AddedByID; }
            set { _AddedByID = value; }
        }
        public double CPValue
        {
            get { return _CPValue; }
            set { _CPValue = value; }
        }
        public int ApprovedByID
        {
            get { return _ApprovedByID; }
            set { _ApprovedByID = value; }
        }
        public DateTime ReceiptDate
        {
            get { return _ReceiptDate; }
            set { _ReceiptDate = value; }
        }
        public int ReceivedByID
        {
            get { return _ReceivedByID; }
            set { _ReceivedByID = value; }
        }
        public DateTime CPAssignmentDate
        {
            get { return _CPAssignmentDate; }
            set { _CPAssignmentDate = value; }
        }
        public string StaffComments
        {
            get { return _StaffComments; }
            set { _StaffComments = value; }
        }
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        public double MaximumCPPerYear
        {
            get { return _MaximumCPPerYear; }
            set { _MaximumCPPerYear = value; }
        }
        public bool AllowCPDonation
        {
            get { return _AllowCPDonation; }
            set { _AllowCPDonation = value; }
        }
        public double EventCharacterCap
        {
            get { return _EventCharacterCap; }
            set { _EventCharacterCap = value; }
        }
        public double AnnualCharacterCap
        {
            get { return _AnnualCharacterCap; }
            set { _AnnualCharacterCap = value; }
        }
        public double TotalCharacterCap
        {
            get { return _TotalCharacterCap; }
            set { _TotalCharacterCap = value; }
        }
        public int EarliestCPApplicationYear
        {
            get { return _EarliestCPApplicationYear; }
            set { _EarliestCPApplicationYear = value; }
        }
        public int CampaignID
        {
            get { return _CampaignID; }
            set { _CampaignID = value; }
        }
        public double TotalCP
        {
            get { return _TotalCP; }
            set { _TotalCP = value; }
        }
        public int PLAuditStatus
        {
            get { return _PLAuditStatus; }
            set { _PLAuditStatus = value; }
        }

        public void GetCampaignPlayerID(int UserID, int CampaignID, int RoleAlignmentID)
        {
            int iTemp = 0;
            string stStoredProc = "uspGetPlayerRoles";
            string stCallingMethod = "cPointOpportunities.GetCampaignPlayerID";
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CampaignID", CampaignID);
            slParameters.Add("@RoleAlignmentID", RoleAlignmentID);
            DataTable dtCampaignPlayer = new DataTable();
            dtCampaignPlayer = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow drowPl in dtCampaignPlayer.Rows)
            {
                if (int.TryParse(drowPl["CampaignPlayerID"].ToString(), out iTemp))
                    _CampaignPlayerID = iTemp;
            }
        }

        public void CreateAttendanceOpportunity(int RoleAlignment, int UserID, int CharacterID, int EventID, int CampaignID, string EventName, string EventDescription, DateTime EventStartDate)
        {
            bool blCreateOpportunity;   // By default, no opportunity.  Set true if one is needed.
            // Go get the CampaignPlayerID
            int iTemp = 0;
            SortedList slParameters = new SortedList();
            ReasonID = 0;
            switch (RoleAlignment)
            {
                case 1: // PC
                    ReasonID = 3;
                    //blCreateOpportunity = true;
                    break;

                case 2: // NPC
                    ReasonID = 1;
                    //blCreateOpportunity = true;
                    break;

                case 3: // Staff
                    ReasonID = 12;
                    //blCreateOpportunity = true;
                    break;

                default:
                    // No match of any kind; skip adding an opportunity.
                    break;
            }
            if (ReasonID != 0)
            {
                blCreateOpportunity = true;
                string stStoredProc = "uspGetCampaignCPOpportunityDefaultByReason";
                string stCallingMethod = "cPointOpportunities.CreateAttendanceOpportunity.GetDefaultByReason";
                slParameters.Clear();
                slParameters.Add("@CampaignID", CampaignID);
                slParameters.Add("@ReasonID", ReasonID);
                // Go get the matching CPOpportunityDefault.  If none, return, otherwise go on.
                DataTable dtOppDef = new DataTable();
                dtOppDef = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
                double dTemp = 0;
                string OppDescription = "";
                string OppNotes = "";
                double CPValue = 0;
                string OppURL = "";
                string StaffComm = "";
                StatusID = 19; // Open
                foreach (DataRow drow in dtOppDef.Rows)
                {
                    if (int.TryParse(drow["CampaignCPOpportunityDefaultID"].ToString(), out iTemp))
                        CampaignCPOpportunityDefaultID = iTemp;
                    OppDescription = drow["Description"].ToString();
                    OppNotes = EventStartDate.Year + "/" + EventStartDate.Month + "/" + EventStartDate.Day + " " + EventName;
                    if (double.TryParse(drow["CPValue"].ToString(), out dTemp))
                        CPValue = dTemp;
                    blCreateOpportunity = true;
                }
                if (blCreateOpportunity == true)
                    InsUpdCPOpportunity(UserID, _CampaignPlayerID, CharacterID, CampaignCPOpportunityDefaultID, EventID,
                        OppDescription, OppNotes, OppURL, ReasonID, StatusID, UserID, CPValue, StaffComm);
            }
            else
                blCreateOpportunity = false;
        }

        public void CreateCleanupOpportunity()
        {

        }

        public void CreatePreregOpportunity()
        {

        }

        public void CreateDonationOpportunity()
        {

        }

        /// <summary>
        /// This will add CP Opportunity
        /// </summary>
        public void InsUpdCPOpportunity(int UserID, int CampaignPlayer, int Character, int CampaignOppDefault, int Event, string DescriptionText, 
            string OpportunityNotesText, string URL, int Reason, int Status, int AddedBy, double CPVal, string StaffCommentsText)
        {
            string stStoredProc = "uspInsUpdCMCampaignCPOpportunities";
            string stCallingMethod = "cPoints.InsUpdCPOpportunity";
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CPAssignmentDate", DateTime.Today);

            DataTable dtPoints = new DataTable();
            slParameters.Add("@CampaignCPOpportunityID", -1);
            slParameters.Add("@CampaignPlayerID", CampaignPlayer);
            slParameters.Add("@CharacterID", Character);
            slParameters.Add("@CampaignCPOpportunityDefaultID", CampaignOppDefault);
            slParameters.Add("@EventID", Event);
            slParameters.Add("@Description", DescriptionText);
            slParameters.Add("@OpportunityNotes", OpportunityNotesText);
            slParameters.Add("@ExampleURL", URL);
            slParameters.Add("@ReasonID", Reason);
            slParameters.Add("@StatusID", Status);
            slParameters.Add("@AddedByID", AddedBy);
            slParameters.Add("@CPValue", CPVal);
            slParameters.Add("@StaffComments", StaffCommentsText);
            //cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
            dtPoints = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            int iTemp = 0;
            foreach (DataRow drow in dtPoints.Rows)
            {
                if (int.TryParse(drow["CampaignCPOpportunityID"].ToString(), out iTemp))
                    _CampaignCPOpportunityID = iTemp;
            }
        }
    }
}