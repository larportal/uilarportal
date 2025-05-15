using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;

//  JB  5/13/2025  Added UserDisplayMyCampaigns so we can show only the campaigns the user wants.

namespace LarpPortal.Classes
{
    public class cUserCampaign
    {
        private Int32 _CampaignPlayerID = -1;
        private Int32 _UserID = -1;
        private string _FirstName = "";
        private string _LastName = "";
        private Int32 _CampaignID = -1;
        private string _CampaignName = "";
        private string _EmailAddress = "";
        private Int32 _LastLoggedInCampaign;
        private string _Sort = "";
        private Boolean _UseLoginEmail = true;
        private int _CampaignEmailID;
        private int _PreferredCampaignHousingTypeID = 0;
        private int _WriteUpLeadTimeNeeded = 0;
        private string _WriteUpLengthPreference = "";
        private string _BackgroundKnowledge = "";
        private DateTime _LastWaiverDate;
        private int _PlayerWaiverID = 0;
        private decimal _CreditAmount = 0;
        private Boolean _ShowRolesToOtherCampaigns = false;
        private Boolean _AutoregisterForEvents = false;
        private int _AutoregisterType = 0;
        private int _OwnerCriticalityRating = 3;
        private int _StatusID = 0;
        private string _Comments = "";
		private bool? _AllowAdditionalInfo = false;
        private bool _HasInbetweenSkills = false;
        private bool _UserDisplayMyCampaigns = false;
        private DateTime? _StaffHideUntil = null;

        public Int32 CampaignPlayerID
        {
            get { return _CampaignPlayerID; }
            set { _CampaignPlayerID = value; }
        }
        public Int32 UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public Int32 CampaignID
        {
            get { return _CampaignID; }
            set { _CampaignID = value; }
        }
        public string CampaignName
        {
            get { return _CampaignName; }
            set { _CampaignName = value; }
        }
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = value; }
        }
        public Int32 LastLoggedInCampaign
        {
            get { return _LastLoggedInCampaign; }
            set { _LastLoggedInCampaign = value; }
        }
        public string Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }
        public Boolean UseLoginEmail
        {
            get { return _UseLoginEmail; }
            set { _UseLoginEmail = value; }
        }
        private int CampaignEmailID
        {
            get { return _CampaignEmailID; }
            set { _CampaignEmailID = value; }
        }
        public int PreferredCampaignHousingTypeID
        {
            get { return _PreferredCampaignHousingTypeID; }
            set { _PreferredCampaignHousingTypeID = value; }
        }
        public int WriteUpLeadTimeNeeded
        {
            get { return _WriteUpLeadTimeNeeded; }
            set { _WriteUpLeadTimeNeeded = value; }
        }
        public string WriteUpLengthPreference
        {
            get { return _WriteUpLengthPreference; }
            set { _WriteUpLengthPreference = value; }
        }
        public string BackgroundKnowledge
        {
            get { return _BackgroundKnowledge; }
            set { _BackgroundKnowledge = value; }
        }
        public DateTime LastWaiverDate
        {
            get { return _LastWaiverDate; }
            set { _LastWaiverDate = value; }
        }
        public int PlayerWaiverID
        {
            get { return _PlayerWaiverID; }
            set { _PlayerWaiverID = value; }
        }
        public decimal CreditAmount
        {
            get { return _CreditAmount; }
            set { _CreditAmount = value; }
        }
        public Boolean ShowRolesToOtherCampaigns
        {
            get { return _ShowRolesToOtherCampaigns; }
            set { _ShowRolesToOtherCampaigns = value; }
        }
        public Boolean AutoregisterForEvents
        {
            get { return _AutoregisterForEvents; }
            set { _AutoregisterForEvents = value; }
        }
        public int AutoregisterType
        {
            get { return _AutoregisterType; }
            set { _AutoregisterType = value; }
        }
        public int OwnerCriticalityRating
        {
            get { return _OwnerCriticalityRating; }
            set { _OwnerCriticalityRating = value; }
        }
        public int StatusID
        {
            get { return _StatusID; }
            set { _StatusID = value; }
        }
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
		public bool? AllowAdditionalInfo
		{
			get { return _AllowAdditionalInfo; }
			set { _AllowAdditionalInfo = value; }
		}
        public bool HasInbetweenSkills
        {
            get { return _HasInbetweenSkills; }
            set { _HasInbetweenSkills = value; }
        }

        public bool UserDisplayMyCampaigns
        {
            get { return _UserDisplayMyCampaigns; }
            set { _UserDisplayMyCampaigns = value; }
        }

        public DateTime? StaffHideUntil
        {
            get { return _StaffHideUntil; }
            set { _StaffHideUntil = value; }
        }
        /// <summary>
        /// This will load the details of a particular user campaign
        /// Must pass a UserID
        /// CampaignIDToLoad to return a specific campaign
        /// </summary>
        public void Load(int UserID, int CampaignIDToLoad)
        {
            string stStoredProc = "uspGetMyCampaigns";
            string stCallingMethod = "cUserCampaign.Load";
            int iTemp;
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CampaignID", CampaignIDToLoad);
            DataSet dsUserCampaign = new DataSet();
            dsUserCampaign = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            dsUserCampaign.Tables[0].TableName = "CMCampaignPlayers";

            foreach (DataRow dRow in dsUserCampaign.Tables["CMCampaignPlayers"].Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
                if (int.TryParse(dRow["UserID"].ToString(), out iTemp))
                    UserID = iTemp;
                if (int.TryParse(dRow["CampaignID"].ToString(), out iTemp))
                    CampaignID = iTemp;
                if (int.TryParse(dRow["LastLoggedInCampaign"].ToString(), out iTemp))
                    LastLoggedInCampaign = iTemp;
                FirstName = dRow["FirstName"].ToString();
                LastName = dRow["LastName"].ToString();
                CampaignName = dRow["CampaignName"].ToString();
                EmailAddress = dRow["EmailAddress"].ToString();
                Sort = dRow["Sort"].ToString();
                bool AllowAdd = false;
                AllowAdditionalInfo = false;
                if (bool.TryParse(dRow["AllowAdditionalInfo"].ToString(), out AllowAdd))
                {
                    AllowAdditionalInfo = AllowAdd;
                }
                bool bDisplay = false;
                if (bool.TryParse(dRow["UserDisplayMyCampaigns"].ToString(), out bDisplay))     //  JB  5/13/2025  Display all campaigns or only ones the user wants.
                {
                    UserDisplayMyCampaigns = bDisplay;
                }
                if (dRow["NumInfoSkills"].ToString() == "0")
                    _HasInbetweenSkills = false;
                else
                    _HasInbetweenSkills = true;
            }
        }


        /// <summary>
        /// Save will handle both insert new record and update old record.
        /// Set CampaignPlayerID = -1 for insert and CampaignPlayerID = record number to update for existing record.
        /// </summary>
        public void Save(int UserID)
        {
            // I need to finish defining this one with the commented out fields that aren't necessary for adding.
            string stStoredProc = "uspInsUpdCMCampaignPlayers";
            //string stCallingMethod = "cUserCampaign.Save";
            SortedList slParameters = new SortedList();
            slParameters.Add("@CampaignPlayerID", CampaignPlayerID);
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@UseLoginEmail", UseLoginEmail);
            slParameters.Add("@CampaignEmailID", CampaignEmailID);
            slParameters.Add("@PreferredCampaignHousingTypeID", PreferredCampaignHousingTypeID);
            slParameters.Add("@WriteUpLeadTimeNeeded", WriteUpLeadTimeNeeded);
            slParameters.Add("@WriteUpLengthPreference", WriteUpLengthPreference);
            slParameters.Add("@BackgroundKnowledge", BackgroundKnowledge);
            slParameters.Add("@LastWaiverDate", LastWaiverDate);
            slParameters.Add("@PlayerWaiverID", PlayerWaiverID);
            slParameters.Add("@CreditAmount", CreditAmount);
            slParameters.Add("@ShowRolesToOtherCampaigns", ShowRolesToOtherCampaigns);
            slParameters.Add("@AutoregisterForEvents", AutoregisterForEvents);
            slParameters.Add("@AutoregisterType", AutoregisterType);
            slParameters.Add("@OwnerCriticalityRating", OwnerCriticalityRating);
            slParameters.Add("@Comments", Comments);
            slParameters.Add("@UserDisplayMYCampaigns", UserDisplayMyCampaigns);
            slParameters.Add("@StaffHideUntil", StaffHideUntil);
            if (@CampaignPlayerID == -1) // Set fields that can only be set on insert of new record
            {
                slParameters.Add("@CampaignID", CampaignID);
                //slParameters.Add("@UserID", UserID);
            }
            cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
        }

        public void Delete(int RecordID, int UserID)
        {
            string stStoredProc = "uspDelCMCampaignPlayers";
            SortedList slParameters = new SortedList();
            slParameters.Add("@RecordID", CampaignPlayerID);
            slParameters.Add("@UserID", UserID.ToString());
            cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
        }
    }
}