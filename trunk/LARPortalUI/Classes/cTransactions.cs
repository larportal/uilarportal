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
    public class cTransactions
    {
        private int _PlayerCPAuditID;
        public int AuditID
        {
            get { return _PlayerCPAuditID; }
            set { _PlayerCPAuditID = value; }
        }
        public List<cTransaction> lsTransactions = new List<cTransaction>();

        public int PlayerCPAuditID { get; set; }
        public int PlayerID { get; set; }
        public int CPApprovedBy { get; set; }
        public int ReasonID { get; set; }
        public int CampaignCPOpportunityID { get; set; }
        public int AddedBy { get; set; }
        public int ReceivedFromCampaignID { get; set; }
        public int ReceivedFromPlayerCPAuditID { get; set; }
        public int SentToCampaignPlayerID { get; set; }
        public int CharacterID { get; set; }
        public decimal CPAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CPApprovedDate { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string OwningPlayer { get; set; }
        public string ApprovingStaffer { get; set; }
        public string ReasonDescription { get; set; }
        public string FullDescription { get; set; }
        public string AdditionalNotes { get; set; }
        public string AddingStaffer { get; set; }
        public string RecvFromCampaign { get; set; }
        public string ReceivingPlayer { get; set; }
        public string ReceivingCampaign { get; set; }
        public string Character { get; set; }
        public int CPAuditCount { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public decimal TotalCP { get; set; }
        public decimal TotalCharacterCap { get; set; }
        //public List<cPageTab> lsPageTabs = new List<cPageTab>();

        /// <summary>
        /// This will load a data table of CP based on the player's requested parameters (I hope)
        /// </summary>
        public DataTable GetCPAuditList (int UserID, int CampaignID, int CharacterID)
        {
            string stStoredProc = "uspGetPlayerCPAudit";
            string stCallingMethod = "cTransaction.GetCPAuditList";
            CPAuditCount = 0;
            int iTemp;
            DateTime dtTemp;
            decimal dTemp;
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);    
            slParameters.Add("@CampaignID", CampaignID);
            slParameters.Add("@CharacterID", CharacterID);
            DataTable dtCPAuditList = new DataTable();
            DataSet dsCPAuditList = new DataSet();
            dtCPAuditList = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", "Usename", stCallingMethod);
            foreach (DataRow dRow in dtCPAuditList.Rows)
            {
                CPAuditCount++;
                if (int.TryParse(dRow["PlayerCPAuditID"].ToString(), out iTemp))
                    PlayerCPAuditID = iTemp;
                if (int.TryParse(dRow["PlayerID"].ToString(), out iTemp))
                    PlayerID = iTemp;
                if (int.TryParse(dRow["CPApprovedBy"].ToString(), out iTemp))
                    CPApprovedBy = iTemp;
                if (int.TryParse(dRow["ReasonID"].ToString(), out iTemp))
                    ReasonID = iTemp;
                if (int.TryParse(dRow["CampaignCPOpportunityID"].ToString(), out iTemp))
                    CampaignCPOpportunityID = iTemp;
                if (int.TryParse(dRow["AddedBy"].ToString(), out iTemp))
                    AddedBy = iTemp;
                if (int.TryParse(dRow["ReceivedFromCampaignID"].ToString(), out iTemp))
                    ReceivedFromCampaignID = iTemp;
                if (int.TryParse(dRow["ReceivedFromPlayerCPAuditID"].ToString(), out iTemp))
                    ReceivedFromPlayerCPAuditID = iTemp;
                if (int.TryParse(dRow["SentToCampaignPlayerID"].ToString(), out iTemp))
                    SentToCampaignPlayerID = iTemp;
                if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                    CharacterID = iTemp;
                if (int.TryParse(dRow["StatusID"].ToString(), out iTemp))
                    StatusID = iTemp;
                if (decimal.TryParse(dRow["CPAmount"].ToString(), out dTemp))
                    CPAmount = dTemp;
                if (decimal.TryParse(dRow["TotalCharacterCap"].ToString(), out dTemp))
                    TotalCharacterCap = dTemp;
                if (DateTime.TryParse(dRow["TransactionDate"].ToString(), out dtTemp))
                    TransactionDate = dtTemp;
                if (DateTime.TryParse(dRow["CPApprovedDate"].ToString(), out dtTemp))
                    CPApprovedDate = dtTemp;
                if (DateTime.TryParse(dRow["ReceiptDate"].ToString(), out dtTemp))
                    ReceiptDate = dtTemp;
                OwningPlayer = dRow["OwningPlayer"].ToString();
                ApprovingStaffer = dRow["ApprovingStaffer"].ToString();
                ReasonDescription = dRow["ReasonDescription"].ToString();
                FullDescription = dRow["FullDescription"].ToString();
                AdditionalNotes = dRow["AdditionalNotes"].ToString();
                AddingStaffer = dRow["AddingStaffer"].ToString();
                RecvFromCampaign = dRow["RecvFromCampaign"].ToString();
                ReceivingPlayer = dRow["ReceivingPlayer"].ToString();
                ReceivingCampaign = dRow["ReceivingCampaign"].ToString();
                Character = dRow["Character"].ToString();
                StatusName = dRow["StatusName"].ToString();
            }
            return dtCPAuditList;
        }

        /// <summary>
        /// This will load the details of a particular player audit transaction
        /// Must pass a UserID
        /// EarnedAtCampaignID optional to return earnings at a speicific campaign, else 0
        /// SpentAtCampaignID optional to return earnings sent to a speicific campaign, else 0
        /// CharacterID optional to return earnings to a speicific character, else 0
        /// Status optional to return earnings at a speicific status, else 0
        /// ReasonID optional to return earnings for a particular reason, else 0
        /// StartDate optional to return earnings after a speicific start date (usually in conjuntion with end date for range), else 0 - Future Functionality
        /// EndDate optional to return earnings before a speicific end date (usually in conjunction with start date for range, else 0 - Future Functionality
        /// </summary>
        public void Load(int OwningPlayerID, int AuditID, int EarnAtCampaignID, int SpentAtCampaignID, int SpentOnCharacterID, string Status, int EarnDescriptionID)
        {
            string stStoredProc = "uspGetAuditRecords";
            string stCallingMethod = "cTransaction.Load";
            int iTemp;
            DateTime StartDate;
            StartDate = DateTime.Parse("1/1/1900");
            DateTime EndDate;
            EndDate = DateTime.Now;
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", OwningPlayerID);
            slParameters.Add("@EarnedAtCampaignID", EarnAtCampaignID);
            slParameters.Add("@SpentAtCampaignID", SpentAtCampaignID);
            slParameters.Add("@CharacterID", SpentOnCharacterID);
            slParameters.Add("@StartDate", StartDate);
            slParameters.Add("@EndDate", EndDate);
            slParameters.Add("@Status", Status);
            slParameters.Add("@ReasonID", EarnDescriptionID);
            DataSet dsTransactions = new DataSet();
            dsTransactions = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", OwningPlayerID.ToString(), stCallingMethod);
            foreach (DataRow dRow in dsTransactions.Tables[0].Rows)
            {
                if (int.TryParse(dRow["AuditID"].ToString(), out iTemp))
                {
                    cTransaction Transaction = new cTransaction();
                    Transaction.Load(OwningPlayerID, AuditID, EarnAtCampaignID, SpentAtCampaignID, SpentAtCampaignID, Status, EarnDescriptionID);
                    lsTransactions.Add(Transaction);
                }
            }
        }
    }
}