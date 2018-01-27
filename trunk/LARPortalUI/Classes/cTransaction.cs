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
    public class cTransaction
    {
        private int _PlayerCPAuditID;
        public int AuditID
        {
            get { return _PlayerCPAuditID; }
            set { _PlayerCPAuditID = value; }
        }
        private int _PlayerID;
        public int OwningPlayerID
        {
            get { return _PlayerID; }
            set { _PlayerID = value; }
        }
        private int _CPApprovedBy;
        public int ApprovedByUserID
        {
            get { return _CPApprovedBy; }
            set { _CPApprovedBy = value; }
        }
        private int _ReasonID;
        public int EarnDescriptionID
        {
            get { return _ReasonID; }
            set { _ReasonID = value; }
        }
        private int _CampaignCPOpportunityID;
        public int OpportunityID
        {
            get { return _CampaignCPOpportunityID; }
            set { _CampaignCPOpportunityID = value; }
        }
        private int _AddedBy;
        public int AddedByID
        {
            get { return _AddedBy; }
            set { _AddedBy = value; }
        }
        private int _ReceivedFromCampaignID;
        public int EarnAtCampaignID
        {
            get { return _ReceivedFromCampaignID; }
            set { _ReceivedFromCampaignID = value; }
        }
        private int _ReceivedFromPlayerCPAuditID;
        public int GotFromAuditID
        {
            get { return _ReceivedFromPlayerCPAuditID; }
            set { _ReceivedFromPlayerCPAuditID = value; }
        }
        private int _SentToCampaignPlayerID;
        public int SpentOnPlayerID
        {
            get { return _SentToCampaignPlayerID; }
            set { _SentToCampaignPlayerID = value; }
        }
        private int _CharacterID;
        public int SpentOnCharacterID
        {
            get { return _CharacterID; }
            set { _CharacterID = value; }
        }
        private DateTime _TransactionDate;
        public DateTime DateEarned
        {
            get { return _TransactionDate; }
            set { _TransactionDate = value; }
        }
        private DateTime _CPApprovedDate;
        public DateTime ApprovedDate
        {
            get { return _CPApprovedDate; }
            set { _CPApprovedDate = value; }
        }
        private double _CPAmount;
        public double Points
        {
            get { return _CPAmount; }
            set { _CPAmount = value; }
        }

        // Reference fields
        public string OwningPlayer { get; set; }
        public string ApprovedBy { get; set; }
        public string EarnDescription { get; set; }
        public string AddedBy { get; set; }
        public string EarnAtCampaign { get; set; }
        public string SpentAtCampaignID { get; set; }
        public string SpentOnPlayer { get; set; }
        public string SpendAtCampaign { get; set; }
        public string SpentOnCharacter { get; set; }
 
        /// <summary>
        /// This will load the details of a particular player audit transaction
        /// OwningPlayerID option to return earnings for a specific player, else 0
        /// AuditID optional to return a specific earning, else 0
        /// EarnedAtCampaignID optional to return earnings at a speicific campaign, else 0
        /// SpentAtCampaignID optional to return earnings sent to a speicific campaign, else 0
        /// CharacterID optional to return earnings to a speicific character, else 0
        /// Status optional to return earnings at a speicific status, else 0
        /// ReasonID optional to return earnings for a particular reason, else 0
        /// StartDate optional to return earnings after a speicific start date (usually in conjuntion with end date for range), else 0 - Future Functionality
        /// EndDate optional to return earnings before a speicific end date (usually in conjunction with start date for range, else 0 - Future Functionality
        /// </summary>
        public void Load( int OwningPlayerID, int AuditID, int EarnAtCampaignID, int SpentAtCampaignID, int SpentOnCharacterID, string Status, int  EarnDescriptionID)
        {
            string stStoredProc = "uspGetAuditRecords";
            string stCallingMethod = "cTransaction.Load";
            int iTemp;
            DateTime dtTemp;
            DateTime StartDate;
            StartDate = DateTime.Parse("1/1/1900");
            DateTime EndDate;
            EndDate = DateTime.Now;
            double dTemp;
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", OwningPlayerID);
            slParameters.Add("@EarnedAtCampaignID", EarnAtCampaignID);
            slParameters.Add("@SpentAtCampaignID", SpentAtCampaignID);
            slParameters.Add("@CharacterID", SpentOnCharacterID);
            slParameters.Add("@StartDate", StartDate);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@Status", Status);
            slParameters.Add("@ReasonID", EarnDescriptionID);
            DataSet dsTransaction = new DataSet();
            dsTransaction = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", OwningPlayerID.ToString(), stCallingMethod);
            dsTransaction.Tables[0].TableName = "PLPlayerCPAudit";

            foreach (DataRow dRow in dsTransaction.Tables["PLPlayerCPAudit"].Rows)
            {
                if (int.TryParse(dRow["AuditID"].ToString(), out iTemp))
                    AuditID = iTemp;
                if (int.TryParse(dRow["OwningPlayerID"].ToString(), out iTemp))
                    OwningPlayerID = iTemp;
                if (int.TryParse(dRow["ApprovedByUserID"].ToString(), out iTemp))
                    ApprovedByUserID = iTemp;
                if (int.TryParse(dRow["EarnDescriptionID"].ToString(), out iTemp))
                    EarnDescriptionID = iTemp;
                if (int.TryParse(dRow["OpportunityID"].ToString(), out iTemp))
                    OpportunityID = iTemp;
                if (int.TryParse(dRow["AddedByID"].ToString(), out iTemp))
                    AddedByID = iTemp;
                if (int.TryParse(dRow["EarnAtCampaignID"].ToString(), out iTemp))
                    EarnAtCampaignID = iTemp;
                if (int.TryParse(dRow["GotFromAuditID"].ToString(), out iTemp))
                    GotFromAuditID = iTemp;
                if (int.TryParse(dRow["SpentOnPlayerID"].ToString(), out iTemp))
                    SpentOnPlayerID = iTemp;
                if (int.TryParse(dRow["SpentOnCharacterID"].ToString(), out iTemp))
                    SpentOnCharacterID = iTemp;
                OwningPlayer = dRow["OwningPlayer"].ToString();
                ApprovedBy = dRow["ApprovedBy"].ToString();
                EarnDescription = dRow["EarnDescription"].ToString();
                AddedBy = dRow["AddedBy"].ToString();
                EarnAtCampaign = dRow["EarnAtCampaign"].ToString();
                SpentOnPlayer = dRow["SpentOnPlayer"].ToString();
                SpendAtCampaign = dRow["SpendAtCampaign"].ToString();
                SpentOnCharacter = dRow["SpentOnCharacter"].ToString();
                if (DateTime.TryParse(dRow["DateEarned"].ToString(), out dtTemp))
                    DateEarned = dtTemp;
                if (DateTime.TryParse(dRow["ApprovedDate"].ToString(), out dtTemp))
                    ApprovedDate = dtTemp;
                if (double.TryParse(dRow["Points"].ToString(), out dTemp))
                    Points = dTemp;
            }
        }

        /// <summary>
        /// Save will handle both insert new record and update old record.
        /// Set CampaignPlayerRoleID = -1 for insert and CampaignPlayerRoleID = record number to update for existing record.
        /// </summary>
        public void Save(int UserID)
        {
            // 
            string stStoredProc = "uspInsUpdPLPlayerCPAudit";
            //string stCallingMethod = "cTransaction.Save";
            SortedList slParameters = new SortedList();
            slParameters.Add("@PlayerCPAuditID", AuditID);
            slParameters.Add("@CPApprovedDate", ApprovedDate);
            slParameters.Add("@CPApprovedBy", ApprovedByUserID);
            slParameters.Add("@Comments", "");
            if (AuditID==-1) // Set fields that can only be set on insert of new record
            {
                slParameters.Add("@UserID", UserID);
                slParameters.Add("@PlayerID", OwningPlayerID);
                slParameters.Add("@TransactionDate", DateEarned);
                slParameters.Add("@CPAmount", Points);
                slParameters.Add("@ReasonID", EarnDescriptionID);
                slParameters.Add("@CampaignCPOpportunityID", OpportunityID);
                slParameters.Add("@AddedBy", AddedByID);
                slParameters.Add("@ReceivedFromCampaignID", EarnAtCampaignID);
                slParameters.Add("@ReceivedFromPlayerCPAuditID", GotFromAuditID);
                slParameters.Add("@SentToCampaignPlayerID", SpentOnPlayerID);
                slParameters.Add("@CharacterID", SpentOnCharacterID);
            }
            cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
        }

        public void Delete(int UserID)
        {
            string stStoredProc = "uspDelPLPlayerCPAudit";
            SortedList slParameters = new SortedList();
            slParameters.Add("@RecordID", AuditID);
            slParameters.Add("@UserID", UserID.ToString());
            cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
        }
    }
}