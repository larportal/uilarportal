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
    public class cPoints
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
        private int _ReceivedFromCampaignID;
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
        public int ReceivedFromCampaignID
        {
            get { return _ReceivedFromCampaignID; }
            set { _ReceivedFromCampaignID = value; }
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

        /// <summary>
        /// This will load a CP Opportunity
        /// Must pass a CPOpportunityID
        /// </summary>
        public void LoadCPOpportunity(int UserID, int OpportunityID)
        {
            string stStoredProc = "uspGetCampaignPointOpportunity";
            string stCallingMethod = "cPoints.LoadCPOpportunity";
            int iTemp = 0;
            double dblTemp = 0;
            DateTime dtTemp;
            SortedList slParameters = new SortedList();
            slParameters.Add("@CampaignCPOpportunityID", OpportunityID);
            DataSet dsOpportunity = new DataSet();
            dsOpportunity = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            dsOpportunity.Tables[0].TableName = "CMCampaignCPOpportunities";
            foreach (DataRow dRow in dsOpportunity.Tables["CMCampaignCPOpportunities"].Rows)
            {
                if (int.TryParse(dRow["CampaignCPOpportunityID"].ToString(), out iTemp))
                    CampaignCPOpportunityID = iTemp;
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
                if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                    CharacterID = iTemp;
                if (int.TryParse(dRow["CampaignCPOpportunityDefaultID"].ToString(), out iTemp))
                    CampaignCPOpportunityDefaultID = iTemp;
                if (int.TryParse(dRow["EventID"].ToString(), out iTemp))
                    EventID = iTemp;
                if (int.TryParse(dRow["ReasonID"].ToString(), out iTemp))
                    ReasonID = iTemp;
                if (int.TryParse(dRow["StatusID"].ToString(), out iTemp))
                    StatusID = iTemp;
                if (int.TryParse(dRow["AddedByID"].ToString(), out iTemp))
                    AddedByID = iTemp;
                if (int.TryParse(dRow["ApprovedByID"].ToString(), out iTemp))
                    ApprovedByID = iTemp;
                if (int.TryParse(dRow["ReceivedByID"].ToString(), out iTemp))
                    ReceivedByID = iTemp;
                if (double.TryParse(dRow["CPValue"].ToString(), out dblTemp))
                    CPValue = dblTemp;
                if (DateTime.TryParse(dRow["ReceiptDate"].ToString(), out dtTemp))
                    ReceiptDate = dtTemp;
                if (DateTime.TryParse(dRow["CPAssignmentDate"].ToString(), out dtTemp))
                    CPAssignmentDate = dtTemp;
                Description = dRow["Description"].ToString();
                OpportunityNotes = dRow["OpportunityNotes"].ToString();
                ExampleURL = dRow["ExampleURL"].ToString();
                StaffComments = dRow["StaffComments"].ToString();
                Comments = dRow["Comments"].ToString();
            }
        }

        /// <summary>
        /// This will "delete" a CP Opportunity
        /// Must pass a CPOpportunityID and UserID
        /// </summary>
        public void DeleteCPOpportunity(int UserID, int OpportunityID)
        {
            string stStoredProc = "uspDelCMCampaignCPOpportunities";
            //string stCallingMethod = "cPoints.DeleteCPOpportunity";
            SortedList slParameters = new SortedList();
            slParameters.Add("@RecordID", OpportunityID);
            slParameters.Add("@UserID", UserID);
            try
            {
                cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
            }
            catch
            {

            }
        }

        public void DeleteRegistrationCPOpportunity (int UserID, int RegistrationID)
        {
            string stStoredProc = "uspDelCMCampaignCPOpportunitiesByRegistrationID";
            //string stCallingMethod = "cPoints.DeleteRegistrationCPOpportunity";

            SortedList slParameters = new SortedList();
            slParameters.Add("@RecordID", RegistrationID);
            slParameters.Add("@UserID", UserID);
            try
            {
                cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
            }
            catch
            {

            }
        }

        /// <summary>
        /// This will create a CPOpportunity associated with a registration
        /// </summary>
        public void CreateRegistrationCPOpportunity (int UserID, int CampaignID, int RoleAlignment, int CharacterID, int ReasonID, int EventID, int RegistrationID)
        {
            string OpportunityNotes = "";
            string ExampleURL = "";
            int StatusID = 19;
            int ApprovedByID = 0;
            DateTime? NullDate = null;
            DateTime ReceiptDate;
            ReceiptDate = Convert.ToDateTime(NullDate);
            int ReceivedByID = 0;
            DateTime CPAssignmentDate;
            CPAssignmentDate = Convert.ToDateTime(NullDate);
            string StaffComments = "";

            // Get the CampaignPlayerID
            GetCampaignPlayerID(UserID, CampaignID, RoleAlignment);

            // Determine >>> CampaignCPOpportunityDefaultID, Description, CPValue <<< from the reason
            string stStoredProc = "uspGetCampaignCPOpportunityByID";
            string stCallingMethod = "cPoints.CreateRegistrationCPOpportunity";
            DataTable dtOppDefault = new DataTable();
            SortedList slParameters = new SortedList();
            slParameters.Add("@ReasonID", ReasonID);
            slParameters.Add("@CampaignID", CampaignID);
            dtOppDefault = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            string strDescription = "";
            int CampaignCPOpportunityDefault = 0;
            double CPVal = 0;
            foreach (DataRow drow in dtOppDefault.Rows)
            {
                strDescription = drow["Description"].ToString();
                Int32.TryParse(drow["CampaignCPOpportunityDefaultID"].ToString(), out CampaignCPOpportunityDefault);
                double.TryParse(drow["CPValue"].ToString(), out CPVal);
            }

            // Call the routine to add the opportunity.  Create it already assigned (last two parameters both = 1)
            InsUpdCPOpportunity(UserID, -1, _CampaignPlayerID, CharacterID, CampaignCPOpportunityDefault, EventID, strDescription, OpportunityNotes, ExampleURL, ReasonID,
                StatusID, UserID, CPVal, ApprovedByID, ReceiptDate, ReceivedByID, CPAssignmentDate, StaffComments, 1, 1, RegistrationID);
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

        /// <summary>
        /// This will post points for a PEL
        /// Must pass a CPOpportunityID and UserID
        /// </summary>
        public void AssignPELPoints(int UserID, int CampaignPlayer, int Character, int CampaignCPOpportunityDefault, int Event,
            string EventDescription, int Reason, int Campaign, double CPVal, DateTime RecptDate)
        {
            int ReceivedFromCampaignID = _ReceivedFromCampaignID;
            string stStoredProc = "uspGetCampaignCPOpportunityByID";
            string stCallingMethod = "cPoints.AssignPELPoints.GetCPOpportunity";
            DataTable dtOppDefault = new DataTable();
            SortedList slParameters = new SortedList();
            slParameters.Add("@CampaignCPOpportunityDefaultID", CampaignCPOpportunityDefault);
            dtOppDefault = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            string strDescription = "";
            foreach (DataRow drow in dtOppDefault.Rows)
            {
                strDescription = drow["Description"].ToString();
            }

            // Check to see if the PEL was submitted on time to get CP
            // Take the eventID and go get its PEL deadline date
            stStoredProc = "uspGetEventInfoByID";
            stCallingMethod = "cPoints.AssignPELPoints.GetPELDeadline";
            string strEventName = "";
            DataTable dtEvent = new DataTable();
            slParameters.Clear();
            slParameters.Add("@EventID", Event);
            dtEvent = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            DateTime dtTemp;
            DateTime PELDeadline = DateTime.Today;
            DateTime dtEventDate = DateTime.Today;
            foreach (DataRow drow2 in dtEvent.Rows)
            {
                if (DateTime.TryParse(drow2["PELDeadlineDate"].ToString(), out dtTemp))
                    PELDeadline = dtTemp;
                if (DateTime.TryParse(drow2["StartDate"].ToString(), out dtTemp))
                    dtEventDate = dtTemp;
                strEventName = String.Format("{0:MM/dd/yyyy}", dtTemp);
                strEventName = strEventName + " - " + drow2["EventName"].ToString();
                EventDescription = strEventName;
            }

            PELDeadline = PELDeadline.AddDays(2);   // For now, give a 2 day grace period.  Eventually, put the grace period as a field in CMCampaigns
            if(RecptDate > PELDeadline)
            {
                CPVal = 0;
                strDescription = strDescription.Trim() + " - Late";
            }
            int iTemp = 0;
            // If Campaign = 0, go out and get campaign, assuming there's an EventID
            if (Campaign == 0  && Event != 0)
            {
                stStoredProc = "uspGetCampaignFromEvent";
                stCallingMethod = "cPoints.AssignPELPoints.GetCampaignFromEvent";
                DataTable dtCampaign = new DataTable();
                slParameters.Clear();
                slParameters.Add("@EventID", Event);
                dtCampaign = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
                if (dtCampaign.Rows.Count > 0)
                {
                    foreach (DataRow drow4 in dtCampaign.Rows)
                    {
                        if (int.TryParse(drow4["CampaignID"].ToString(), out iTemp))
                            Campaign = iTemp;
                    }
                }         
            }

            // If Character = 0, go out and get character for campaign.  If only one, set Character = that one.  If <> one character, leave at 0 and bank.
            if (Character == 0 && Campaign !=0)
            {
                stStoredProc = "uspGetCharacters";
                stCallingMethod = "cPoints.AssignPELPoints.GetCharacters";
                DataTable dtChars = new DataTable();
                slParameters.Clear();
                slParameters.Add("@CampaignID", Campaign);
                slParameters.Add("@CampaignPlayerID", CampaignPlayer);
                dtChars = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
                if (dtChars.Rows.Count == 1)
                {
                    foreach (DataRow drow3 in dtChars.Rows)
                    {
                        if (int.TryParse(drow3["CharacterID"].ToString(), out iTemp))
                            Character = iTemp;
                    }
                }
            }

            // Using the Event and Campaign Player, go get the RegistrationID
            stStoredProc = "uspGetRegistrationByPlayer";
            stCallingMethod = "cPoints.AssignPELPoints.GetRegistration";
            int Reg = 0;
            int RoleAlignment = 1;  // Assume PC by default
            int NPCCampaignID = 0;
            int TransferCampaignPlayerID = 0;
            DataTable dtReg = new DataTable();
            slParameters.Clear();
            slParameters.Add("@EventID", Event);
            slParameters.Add("@CampaignPlayerID", CampaignPlayer);
            dtReg = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow drowreg in dtReg.Rows)
            {
                if (int.TryParse(drowreg["RegistrationID"].ToString(), out iTemp))
                    Reg = iTemp;
                if (int.TryParse(drowreg["RoleAlignmentID"].ToString(), out iTemp))
                    RoleAlignment = iTemp;
                if (int.TryParse(drowreg["NPCCampaignID"].ToString(), out iTemp))
                    NPCCampaignID = iTemp;
                if (int.TryParse(drowreg["TransferCampaignPlayerID"].ToString(), out iTemp))
                    TransferCampaignPlayerID = iTemp;
            }

            // Call the routine to add the opportunity.  Create it already assigned (last two parameters both = 1)
            // 11/17/2016 - Rick - Need branching logic to send CP to another chapter if requested for role alignments other than 1=PC
            if (RoleAlignment != 1)
            {   // Change parameters to match destination campaign
                CampaignPlayer = TransferCampaignPlayerID;
                Character = 0;
                // Convert CampaignCPOpportunityDefault to destination campaign
                stStoredProc = "uspConvertCampaignCPOpportunityDefaultID";
                stCallingMethod = "cPoints.AssignPELPoints.GetCPOpportunityDefault";
                DataTable dtOppDef = new DataTable();
                slParameters.Clear();
                slParameters.Add("@OldCampaignCPOpportunityDefaultID", CampaignCPOpportunityDefault);
                slParameters.Add("@NewCampaignID", NPCCampaignID);
                dtOppDef = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
                foreach (DataRow drowOppDef in dtOppDef.Rows)
                {
                    if (int.TryParse(drowOppDef["CampaignCPOpportunityDefaultID"].ToString(), out iTemp))
                        CampaignCPOpportunityDefault = iTemp;
                }
                _ReceivedFromCampaignID = Campaign;
            }
            InsUpdCPOpportunity(UserID, -1, CampaignPlayer, Character, CampaignCPOpportunityDefault, Event, strDescription, EventDescription, "", Reason, 21, UserID, CPVal, UserID, RecptDate, UserID, DateTime.Now, "", 1, 1, Reg);

            // Call the routine to check if CP can be assigned to character and if appropriate assign the CP
            // 11/17/2016 - Rick - Only add points if character is not 0 (NPC/Staff transfer), otherwise set variables for banking
            if (Character !=0)
            {
                AddPointsToCharacter(Character, CPVal); 
            }
            else
            {
                _PLAuditStatus = 60;    // Banked status
            }


            // Call the routine to add the CP to the player CP audit log.  If assigned to character, create it spent otherwise
            //      create it banked (_PLPlayerAuditStatus)
            CreatePlayerCPLog(UserID, _CampaignCPOpportunityID, RecptDate, CPVal, Reason, CampaignPlayer, Character);
            _ReceivedFromCampaignID = ReceivedFromCampaignID;
        }

        public void AddManualCPEntry(int UserID, int CampaignPlayerID, int CharacterID, int CampaignCPOpportunityDefaultID, int EventID, int CampaignID, string Description,
                string OpportunityNotes, string ExampleURL, int ReasonID, int StatusID, int AddedByID, double CPValue, int ApprovedByID, DateTime ReceiptDate,
                int ReceivedByID, DateTime CPAssignmentDate, string StaffComments)
        {
            // Call the routine to add the opportunity.  Create it already assigned (last two parameters both = 1)
            InsUpdCPOpportunity(UserID, -1, CampaignPlayerID, CharacterID, CampaignCPOpportunityDefaultID, EventID, Description, OpportunityNotes, ExampleURL, ReasonID,
                StatusID, AddedByID, CPValue, ApprovedByID, ReceiptDate, ReceivedByID, CPAssignmentDate, StaffComments, 1, 1, 0);
            // Call the routine to check if CP can be assigned to character and if appropriate assign the CP
            AddPointsToCharacter(CharacterID, CPValue);
            // Call the routine to add the CP to the player CP audit log.  If assigned to character, create it spent otherwise
            //      create it banked (_PLPlayerAuditStatus)
            _CampaignID = CampaignID;
            _ReceivedFromCampaignID = CampaignID;
            CreatePlayerCPLog(UserID, _CampaignCPOpportunityID, ReceiptDate, CPValue, ReasonID, CampaignPlayerID, CharacterID);
        }

        /// <summary>
        /// This will post points for a history
        /// Requires UserID, CampaignPlayer, Character and Campaign
        /// Other fields are optional for history; can pass through anything.
        /// </summary>
        public void AssignHistoryPoints(int UserID, int CampaignPlayer, int Character, int CampaignCPOpportunityDefault,
            int Campaign, double CPVal, DateTime RecptDate)
        {
            int Event = 0;
            string EventDescription = "";
            int Reason = 24;  // Character History
            string stStoredProc = "uspGetCampaignCPOpportunityByID";
            string stCallingMethod = "cPoints.AssignHistoryPoints1";
            DataTable dtOppDefault = new DataTable();
            SortedList slParameters = new SortedList();
            slParameters.Add("@ReasonID", Reason);
            slParameters.Add("@CampaignID", Campaign);
            dtOppDefault = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            string strDescription = "";
            foreach (DataRow drow in dtOppDefault.Rows)
            {
                strDescription = drow["Description"].ToString();
                Int32.TryParse(drow["CampaignCPOpportunityDefaultID"].ToString(), out CampaignCPOpportunityDefault);
                double.TryParse(drow["CPValue"].ToString(), out CPVal);
            }

            // Go get the ID of the current actor of the character
            stStoredProc = "uspGetCharacters";
            DataTable dtCharacter = new DataTable();
            slParameters.Clear();
            slParameters.Add("@CharacterID", Character);
            stCallingMethod = "cPoints.AssignHistoryPoints2";
            dtCharacter = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            int PlayerID = 0;
            foreach (DataRow drowchar in dtCharacter.Rows)
            {
                Int32.TryParse(drowchar["UserID"].ToString(), out PlayerID);
            }

            // Call the routine to add the opportunity.  Create it already assigned (last two parameters both = 1)
            InsUpdCPOpportunity(PlayerID, -1, CampaignPlayer, Character, CampaignCPOpportunityDefault, Event, strDescription, EventDescription, "", Reason, 21, UserID, CPVal, UserID, RecptDate, UserID, DateTime.Now, "", 1, 1,0);

            // Call the routine to check if CP can be assigned to character and if appropriate assign the CP
            AddPointsToCharacter(Character, CPVal);

            // Call the routine to add the CP to the player CP audit log.  If assigned to character, create it spent otherwise
            //      create it banked (_PLPlayerAuditStatus)
            CreatePlayerCPLog(PlayerID, _CampaignCPOpportunityID, RecptDate, CPVal, Reason, CampaignPlayer, Character);
        }

        /// <summary>
        /// This will create an entry in the player CP audit log
        /// Must pass a CPOpportunityID and UserID
        /// </summary>
        public void CreatePlayerCPLog(int UserID, int OpportunityID, DateTime RecptDate, double CPVal, int Reason, 
            int CampaignPlayer, int CharacterID)
        {
            int iTemp = 0;
            DateTime MinDate = new DateTime(0001, 1, 2);
            int PlayerID = 0; // This is the MDBUserID of the player getting the points
            DataTable dtPlayer = new DataTable();
            if(RecptDate < MinDate)
            {
                RecptDate = DateTime.Now;
            }
            string stStoredProc = "uspGetCampaignPlayerByID";
            string stCallingMethod = "cPoints.CreatePlayerCPLog";
            SortedList slParameters = new SortedList();
            slParameters.Add("@CampaignPlayerID", CampaignPlayer);
            dtPlayer = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow drow in dtPlayer.Rows)
            {
                if (int.TryParse(drow["UserID"].ToString(), out iTemp))
                    PlayerID = iTemp;
            }
            if (_PLAuditStatus == 60)
                CharacterID = 0;
            stStoredProc = "uspInsUpdPLPlayerCPAudit";
            DateTime datevar = DateTime.Today;
            int ThisYear = datevar.Year;
            //string stCallingMethod = "cPoints.DeleteCPOpportunity";
            slParameters.Clear();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@PlayerCPAuditID", -1);
            slParameters.Add("@PlayerID", PlayerID);
            slParameters.Add("@TransactionDate", RecptDate);
            slParameters.Add("@CPApprovedDate", DateTime.Today);
            slParameters.Add("@CPApprovedBy", UserID);
            slParameters.Add("@CPAmount", CPVal);
            // TODO - Rick - For now apply it to this year.  We'll worry about restrictions later.
            slParameters.Add("@YearAppliedTo", ThisYear);
            slParameters.Add("@ReasonID", Reason);
            slParameters.Add("@CampaignCPOpportunityID", OpportunityID);
            slParameters.Add("@AddedBy", UserID);
            slParameters.Add("@ReceivedFromCampaignID", _ReceivedFromCampaignID);
            // TODO - Rick - For now assume this isn't from one player passing to another
            slParameters.Add("@ReceivedFromPlayerCPAuditID", 0);
            slParameters.Add("@SentToCampaignPlayerID", CampaignPlayer);
            slParameters.Add("@CharacterID", CharacterID);
            slParameters.Add("@StatusID", _PLAuditStatus);

            try
            {
                cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
            }
            catch
            {

            }
        }

        /// <summary>
        /// This will update an existing CP Opportunity
        /// </summary>
        public void UpdateCPOpportunity(int UserID, int CampaignCPOpportunityID, int CampaignPlayerID, int CharacterID,
            int CampaignOpportunityDefaultID, int EventID, string DescriptionText, string OppNotes, string URL, int ReasonID,
            int AddedByID, double CPVal, int ApprovedByID, DateTime RecptDate, int ReceivedByID, string StaffComments,
            int RoleID, int NPCCampaignID, int CampaignID)
        {
            // If PC add points to character
            // If NPC/staff keeping points at this campaign, bank them here
            // If NPC/staff sending points to another campaign, bank them there if participating campaign, else flag it for email
            string stStoredProc = "uspGetCampaignByCampaignID";
            string stCallingMethod = "cPoints.UpdateCPOpportunity";
            DataTable dtNPCCampaign = new DataTable();
            string PortalAccessType = "";
            int OpportunityStatus = 21;     // Assume Complete.  We'll change as necessary.
            SortedList slParameters = new SortedList();
            slParameters.Add("@CampaignID", NPCCampaignID);
            dtNPCCampaign = Classes.cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow dRow in dtNPCCampaign.Rows)
            {
                PortalAccessType = dRow["PortalAccessType"].ToString();
            }

            if (RoleID == 1)        // PC - Add points to character
            {
                // Call the routine to update the opportunity.  Create it already assigned (last two parameters both = 1)
                InsUpdCPOpportunity(UserID, CampaignCPOpportunityID, CampaignPlayerID, CharacterID, CampaignOpportunityDefaultID, 
                                EventID, DescriptionText, OppNotes, URL, ReasonID, OpportunityStatus, UserID, CPVal, UserID, RecptDate, UserID, 
                                DateTime.Now, StaffComments, 1, 1, 0);
                // Call the routine to check if CP can be assigned to character and if appropriate assign the CP
                AddPointsToCharacter(CharacterID, CPVal);
                // Call the routine to add the CP to the player CP audit log.  If assigned to character, create it spent otherwise
                //      create it banked (_PLPlayerAuditStatus)
                int SaveReceivedFromCampaignID = _ReceivedFromCampaignID;
                _ReceivedFromCampaignID = CampaignID;
                CreatePlayerCPLog(UserID, CampaignCPOpportunityID, RecptDate, CPVal, ReasonID, CampaignPlayerID, CharacterID);
                _ReceivedFromCampaignID = SaveReceivedFromCampaignID;
            }
            else
            {                      // Non-PC
                _PLAuditStatus = 60;
                if (PortalAccessType != "S")
                {
                    OpportunityStatus = 68;     // Ready to send to a non-LP campaign via email
                    _PLAuditStatus = 79;
                }
                if(NPCCampaignID != CampaignID && NPCCampaignID != -1)      // NOT staying where earned and not Campaign "Other"
                {
                    if (OpportunityStatus == 21)    
                    {
                        // If NPC/Staff and the points are going somewhere bank them there or email them there (depending on OpportunityStatus which
                            // is based on PortalAccessType)
                        // Set parameters for AddPointToBank function
                        // Convert CampaignPlayer from this campaign to the destination = uspConvertCampaignPlayerID @OriginalCampaignPlayerID & @NewCampaignID
                        stStoredProc = "uspConvertCampaignPlayerID";
                        stCallingMethod = "PointsAssign.aspx.btnSaveNewOpportunityClick";
                        int iTemp = 0;
                        DataTable dtCampaignPlayers = new DataTable();
                        SortedList sParams = new SortedList();
                        sParams.Add("@OriginalCampaignPlayerID", CampaignPlayerID);
                        sParams.Add("@NewCampaignID", NPCCampaignID);
                        dtCampaignPlayers = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", UserID.ToString(), stCallingMethod);
                        foreach (DataRow dRow in dtCampaignPlayers.Rows)
                        {
                            if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                                CampaignPlayerID = iTemp;
                        }
                        // Assume that staff and NPC are going to a bank, at least for now
                        AddPointsToBank(UserID, CampaignCPOpportunityID, CampaignPlayerID, CampaignOpportunityDefaultID, EventID, DescriptionText, OppNotes, URL,
                            ReasonID, CPVal, ReceiptDate, StaffComments, CampaignID, RoleID, NPCCampaignID);
                    }
                    else
                    {
                        // This is going to be emailed.
                        MarkPointsForEmail(UserID, CampaignCPOpportunityID, CampaignPlayerID, CampaignOpportunityDefaultID, EventID, DescriptionText, OppNotes, URL,
                        ReasonID, CPVal, ReceiptDate, StaffComments, CampaignID, RoleID, NPCCampaignID, OpportunityStatus);
                    }
                }
                else
                {
                    // Else (If NPC/Staff and the points are staying here or going to "other campaign" then bank here)
                    // Set parameters for AddPointToBank function

                    // Assume that staff and NPC are going to a bank, at least for now
                    AddPointsToBank(UserID, CampaignCPOpportunityID, CampaignPlayerID, CampaignOpportunityDefaultID, EventID, DescriptionText, OppNotes, URL,
                        ReasonID, CPVal, ReceiptDate, StaffComments, CampaignID, RoleID, NPCCampaignID);

                }


            }
        }

        /// <summary>
        /// This will add or update a CP Opportunity
        /// Must pass a CPOpportunityID
        /// </summary>
        public void InsUpdCPOpportunity(int UserID, int OpportunityID, 
            int CampaignPlayer, int Character, int CampaignOppDefault, int Event, string DescriptionText, string OpportunityNotesText,
            string URL, int Reason, int Status, int AddedBy, double CPVal, int ApprovedBy, DateTime RecptDate, int ReceivedBy, 
            DateTime CPAssignment, string StaffCommentsText, int PLAudit, int CharacterUpdate, int Registration)
        {
            string stStoredProc = "uspInsUpdCMCampaignCPOpportunities";
            string stCallingMethod = "cPoints.InsUpdCPOpportunity";
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CPAssignmentDate", DateTime.Today);

            if(OpportunityID == -1) // Add a new opportunity
            {
                DataTable dtPoints = new DataTable();
                slParameters.Add("@CampaignCPOpportunityID", OpportunityID);
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
                slParameters.Add("@ApprovedByID", ApprovedBy);
                slParameters.Add("@ReceiptDate", RecptDate);
                slParameters.Add("@ReceivedByID", ReceivedBy);
                slParameters.Add("@StaffComments", StaffCommentsText);
                slParameters.Add("@PLAudit", PLAudit);
                slParameters.Add("@CharacterUpdate", CharacterUpdate);
                slParameters.Add("@RegistrationID", Registration);
                //cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
                dtPoints = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
                int iTemp = 0;
                foreach (DataRow drow in dtPoints.Rows)
                {
                    if (int.TryParse(drow["CampaignCPOpportunityID"].ToString(), out iTemp))
                        _CampaignCPOpportunityID = iTemp;
                }
            }
            else      // Change an existing
            {
                slParameters.Add("@CampaignCPOpportunityID", OpportunityID);
                slParameters.Add("@CampaignPlayerID", CampaignPlayer);
                slParameters.Add("@CharacterID", Character);
                slParameters.Add("@CampaignCPOpportunityDefaultID", CampaignOppDefault);
                slParameters.Add("@EventID", Event);
                slParameters.Add("@Description", DescriptionText);
                slParameters.Add("@OpportunityNotes", OpportunityNotesText);
                slParameters.Add("@ExampleURL", URL);
                slParameters.Add("@ReasonID", Reason);
                slParameters.Add("@AddedByID", AddedBy);
                slParameters.Add("@StatusID", Status);
                slParameters.Add("@CPValue", CPVal);
                slParameters.Add("@ApprovedByID", ApprovedBy);
                slParameters.Add("@ReceiptDate", RecptDate);
                slParameters.Add("@ReceivedByID", ReceivedBy);
                slParameters.Add("@StaffComments", StaffCommentsText);
                cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
            }  
        }

        /// <summary>
        /// This will mark an opportunity to set it ready to email to non-LARP Portal campaign
        /// </summary>
        public void MarkPointsForEmail(int UserID, int CampaignCPOpportunityID, int CampaignPlayerID, int CampaignOpportunityDefaultID,
             int EventID, string DescriptionText, string OppNotes, string URL, int ReasonID, double CPVal, DateTime RecptDate, string StaffComments,
             int CampaignID, int RoleID, int NPCCampaignID, int OppStatus)
        {
            InsUpdCPOpportunity(UserID, CampaignCPOpportunityID, CampaignPlayerID, 0, CampaignOpportunityDefaultID,
                                           EventID, DescriptionText, OppNotes, URL, ReasonID, OppStatus, UserID, CPVal, UserID, RecptDate, UserID,
                                           DateTime.Now, StaffComments, 1, 1, 0);
            CreatePlayerCPLog(UserID, CampaignCPOpportunityID, RecptDate, CPVal, ReasonID, CampaignPlayerID, CharacterID);
        }


        /// <summary>
        /// This will add points to a player's bank
        /// Must pass CampignID, Points
        /// </summary>
        public void AddPointsToBank(int UserID, int CampaignCPOpportunityID, int CampaignPlayerID, int CampaignOpportunityDefaultID,
            int EventID, string DescriptionText, string OppNotes, string URL, int ReasonID, double CPVal, DateTime RecptDate, string StaffComments,
            int CampaignID, int RoleID, int NPCCampaignID)
        {

            // Need
            // CampaignOpportunity - CampaignCPOpportunityID, CampaignPlayerID, CharacterID, CampaignCPOpportunityDefaultID, EventID, CampaignID, Description,
            //      OpportunityNotes, ExampleURL, ReasonID, StatusID, AddedByID, CPValue, ApprovedByID, ReceiptDate, ReceivedByID, CPAssignmentDate, StaffComments,
            //      RegistrationID, DateAdded, DateChanged
            // PlayerCPAudit - PlayerID, TransactionDate, CPApprovedDate, CPApprovedBy, CPAmount, YearAppliedTo, ReasonID, CampaignCPOpportunityID, AddedBy,
            //      ReceivedFromCampaignID, ReceivedFromPlayerCPAuditID, SentToCampaignPlayerID, CharacterID, StatusID, Comments, DateAdded, DateChanged
            InsUpdCPOpportunity(UserID, CampaignCPOpportunityID, CampaignPlayerID, 0, CampaignOpportunityDefaultID,
                                           EventID, DescriptionText, OppNotes, URL, ReasonID, 21, UserID, CPVal, UserID, RecptDate, UserID,
                                           DateTime.Now, StaffComments, 1, 1, 0);           
            _PLAuditStatus = 60;
            CreatePlayerCPLog(UserID, CampaignCPOpportunityID, RecptDate, CPVal, ReasonID, CampaignPlayerID, CharacterID);
        }

        /// <summary>
        /// This will add the points where appropriate to the character
        /// Must pass a CharacterID and CPValue
        /// </summary>
        public void AddPointsToCharacter(int CharacterID, double CPVal)
        {
            // Go get campaign character update parameters
            string stStoredProc = "uspGetCharacterUpdateParameters";
            DataTable dtUpdateParameters = new DataTable();
            string stCallingMethod = "cPoints.AddPointsToCharacter";
            SortedList slParameters = new SortedList();
            slParameters.Add("@CharacterID", CharacterID);
            //slParameters.Add("@CPValue", CPVal);
            try
            {
                dtUpdateParameters = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
                int iTemp = 0;
                bool btemp = true;
                double dbltemp = 0;
                foreach (DataRow drow in dtUpdateParameters.Rows)
                {
                    if (int.TryParse(drow["CampaignID"].ToString(), out iTemp))
                       _CampaignID = iTemp;
                    if (double.TryParse(drow["TotalCP"].ToString(), out dbltemp))
                        _TotalCP = dbltemp;
                    if (double.TryParse(drow["MaximumCPPerYear"].ToString(), out dbltemp))
                        _MaximumCPPerYear = dbltemp;
                    if (bool.TryParse(drow["AllowCPDonation"].ToString(), out btemp))
                        _AllowCPDonation = btemp;
                    if (double.TryParse(drow["EventCharacterCap"].ToString(), out dbltemp))
                        _EventCharacterCap = dbltemp;
                    if (double.TryParse(drow["AnnualCharacterCap"].ToString(), out dbltemp))
                        _AnnualCharacterCap = dbltemp;
                    if (double.TryParse(drow["TotalCharacterCap"].ToString(), out dbltemp))
                        _TotalCharacterCap = dbltemp;
                    if (int.TryParse(drow["EarliestCPApplicationYear"].ToString(), out iTemp))
                        _EarliestCPApplicationYear = iTemp;
                }
                if (_TotalCP >= _TotalCharacterCap)
                    _PLAuditStatus = 60;    // Character already at cap, bank it
                else
                {
                    // Apply it to the character not to exceed the cap
                    if (_TotalCP + CPVal <= _TotalCharacterCap)
                    {
                        _TotalCP = _TotalCP + CPVal;
                        _PLAuditStatus = 15;    // Status used and apply to character
                        stStoredProc = "uspUpdateCharacterCP";
                        slParameters.Clear();
                        slParameters.Add("@CharacterID", CharacterID);
                        slParameters.Add("@TotalCP", _TotalCP);
                        cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
                    }
                    else
                        _PLAuditStatus = 60;    // Would put character over cap, bank it
                }
            }
            catch
            {

            }
        }
    }
}

