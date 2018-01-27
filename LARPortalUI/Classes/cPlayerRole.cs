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
    public class cPlayerRole
    {
        private int _CampaignPlayerRoleID;
        public int CampaignPlayerRoleID
        {
            get { return _CampaignPlayerRoleID; }
            set { _CampaignPlayerRoleID = value; }
        }
        public int RoleID { get; set; }
        public int RoleAlignmentID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool CPEarnedForRole { get; set; }
        public double CPQuantityEarnedPerEvent { get; set; }
        // Reference fields
        public int CampaignPlayerID { get; set; }
        public string CampaignName { get; set; }
        public string RoleDescription { get; set; }
        public int CampaignID { get; set; }
        public int UserID { get; set; }
        public bool UseLoginEmail { get; set; }
        public int CampaignEmailID { get; set; }
        public int PreferredCampaignHousingTypeID { get; set; }
        public int WriteUpLeadTimeNeeded { get; set; }
        public string WriteUpLengthPreference { get; set; }
        public string BackgroundKnowledge { get; set; }
        public DateTime LastWaiverDate { get; set; }
        public string WaiverFile { get; set; }
        public double CreditAmount { get; set; }
        public bool ShowRolesToOtherCampaigns { get; set; }
        public bool AutoregisterForEvents { get; set; }
        public int AutoregisterType { get; set; }
        public int OwnerCriticalityRating { get; set; }
        public string RoleTier { get; set; }
        public int RoleLevel { get; set; }
        public bool RoleAssignmentAuthority { get; set; }
        public int RoleAssignmentLevel { get; set; }
        public bool CanRegisterForEvents { get; set; }
        public int RoleAlignment { get; set; }
        public int StatusID { get; set; }
        public string IsPC { get; set; }
        public string IsNPC { get; set; }
        
        /// <summary>
        /// This will load the details of a particular player role
        /// Must pass a UserID
        /// CampaignPlayerIDToLoad optional to return a specific role in a specific campaign, else 0
        /// CampaignIDToLoad optional to return all roles for a player in a specific campaign, else 0
        /// </summary>
        public void Load( int UserID, int CampaignPlayerRoleIDToLoad, int CampaignIDToLoad )
        {
            string stStoredProc = "uspGetPlayerRoles";
            string stCallingMethod = "cPlayerRole.Load";
            int iTemp;
            bool bTemp;
            DateTime dtTemp;
            double dTemp;
            IsPC = "false";
            IsNPC = "false";
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CampaignPlayerRoleID", CampaignPlayerRoleIDToLoad);
            slParameters.Add("@CampaignID", CampaignIDToLoad);
            DataSet dsPlayerRole = new DataSet();
            dsPlayerRole = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            dsPlayerRole.Tables[0].TableName = "CMCampaignPlayerRoles";

            foreach (DataRow dRow in dsPlayerRole.Tables["CMCampaignPlayerRoles"].Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerRoleID"].ToString(), out iTemp))
                    CampaignPlayerRoleID = iTemp;
                if (int.TryParse(dRow["RoleID"].ToString(), out iTemp))
                    RoleID = iTemp;
                switch (RoleID)
                {
                    case 6:
                        IsNPC = "true";
                        break;
                    case 7:
                        IsNPC = "true";
                        break;
                    case 8:
                        IsPC = "true";
                        break;
                    case 10:
                        IsNPC = "true";
                        break;
                    default:
                        break;
                }
                if (int.TryParse(dRow["RoleAlignmentID"].ToString(), out iTemp))
                    RoleAlignmentID = iTemp;
                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
                if (int.TryParse(dRow["CampaignID"].ToString(), out iTemp))
                    CampaignID = iTemp;
                if (int.TryParse(dRow["UserID"].ToString(), out iTemp))
                    UserID = iTemp;
                if (int.TryParse(dRow["CampaignEmailID"].ToString(), out iTemp))
                    CampaignEmailID = iTemp;
                if (int.TryParse(dRow["PreferredCampaignHousingTypeID"].ToString(), out iTemp))
                    PreferredCampaignHousingTypeID = iTemp;
                if (int.TryParse(dRow["WriteUpLeadTimeNeeded"].ToString(), out iTemp))
                    WriteUpLeadTimeNeeded = iTemp;
                if (int.TryParse(dRow["AutoregisterType"].ToString(), out iTemp))
                    AutoregisterType = iTemp;
                if (int.TryParse(dRow["OwnerCriticalityRating"].ToString(), out iTemp))
                    OwnerCriticalityRating = iTemp;
                if (int.TryParse(dRow["RoleLevel"].ToString(), out iTemp))
                    RoleLevel = iTemp;
                if (int.TryParse(dRow["RoleAssignmentLevel"].ToString(), out iTemp))
                    RoleAssignmentLevel = iTemp;
                if (int.TryParse(dRow["RoleAlignment"].ToString(), out iTemp))
                    RoleAlignment = iTemp;
                if (int.TryParse(dRow["StatusID"].ToString(), out iTemp))
                    StatusID = iTemp;
                CampaignName = dRow["CampaignName"].ToString();
                RoleDescription = dRow["RoleDescription"].ToString();
                if (StatusID == 56)
                    RoleDescription = RoleDescription + " (Pending)";
                WriteUpLengthPreference = dRow["WriteUpLengthPreference"].ToString();
                BackgroundKnowledge = dRow["BackgroundKnowledge"].ToString();
                //WaiverFile = dRow["WaiverFile"].ToString();
                RoleTier = dRow["RoleTier"].ToString();
                if (DateTime.TryParse(dRow["ExpirationDate"].ToString(), out dtTemp))
                    ExpirationDate = dtTemp;
                if (DateTime.TryParse(dRow["LastWaiverDate"].ToString(), out dtTemp))
                    LastWaiverDate = dtTemp;
                if (bool.TryParse(dRow["CPEarnedForRole"].ToString(), out bTemp))
                    CPEarnedForRole = bTemp;
                if (bool.TryParse(dRow["UseLoginEmail"].ToString(), out bTemp))
                    UseLoginEmail = bTemp;
                if (bool.TryParse(dRow["ShowRolesToOtherCampaigns"].ToString(), out bTemp))
                    ShowRolesToOtherCampaigns = bTemp;
                if (bool.TryParse(dRow["AutoregisterForEvents"].ToString(), out bTemp))
                    AutoregisterForEvents = bTemp;
                if (bool.TryParse(dRow["RoleAssignmentAuthority"].ToString(), out bTemp))
                    RoleAssignmentAuthority = bTemp;
                if (bool.TryParse(dRow["CanRegisterForEvents"].ToString(), out bTemp))
                    CanRegisterForEvents = bTemp;
                if (double.TryParse(dRow["CPQuantityEarnedPerEvent"].ToString(), out dTemp))
                    CPQuantityEarnedPerEvent = dTemp;
                if (double.TryParse(dRow["CreditAmount"].ToString(), out dTemp))
                    CreditAmount = dTemp;
            }
        }


        /// <summary>
        /// Save will handle both insert new record and update old record.
        /// Set CampaignPlayerRoleID = -1 for insert and CampaignPlayerRoleID = record number to update for existing record.
        /// </summary>
        public void Save(int UserID)
        {
            // 
            string stStoredProc = "uspInsUpdCMCampaignPlayerRoles";
            //string stCallingMethod = "cGameSystem.Save";
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CampaignPlayerRoleID", CampaignPlayerRoleID);
            //slParameters.Add("@RoleAlignmentID", RoleAlignmentID);
            slParameters.Add("@CPEarnedForRole", CPEarnedForRole);
            slParameters.Add("@CPQuantityEarnedPerEvent", CPQuantityEarnedPerEvent);
            slParameters.Add("@StatusID", StatusID);
            if (@CampaignPlayerRoleID==-1) // Set fields that can only be set on insert of new record
            {
                slParameters.Add("@CampaignPlayerID", CampaignPlayerID);
                slParameters.Add("@RoleID", RoleID);
                slParameters.Add("@RoleAlignmentID", RoleAlignmentID);
            }
            cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
        }

        public void Delete(int UserID)
        {
            string stStoredProc = "uspDelCMCampaignPlayerRoles";
            SortedList slParameters = new SortedList();
            slParameters.Add("@RecordID", CampaignPlayerRoleID);
            slParameters.Add("@UserID", UserID.ToString());
            cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
        }
    }
}