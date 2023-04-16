using System;
using System.Data;
using System.Reflection;
using System.Collections;

namespace LarpPortal.Classes
{
    public class cDonation
    {
        private int _AcceptedBy;
        public int AcceptedBy
        {
            get { return _AcceptedBy; }
            set { _AcceptedBy = value; }
        }

        private bool _AllowEventFee;
        public bool AllowEventFee
        {
            get { return _AllowEventFee; }
            set { _AllowEventFee = value; }
        }

        private bool _AllowPlayerToPlayerPoints;
        public bool AllowPlayerToPlayerPoints
        {
            get { return _AllowPlayerToPlayerPoints; }
            set { _AllowPlayerToPlayerPoints = value; }
        }

        private int _AlreadyClaimedByThisPlayer;
        public int AlreadyClaimedByThisPlayer
        {
            get { return _AlreadyClaimedByThisPlayer; }
            set { _AlreadyClaimedByThisPlayer = value; }
        }

        private int _AvailableToClaim;
        public int AvailableToClaim
        {
            get { return _AvailableToClaim; }
            set { _AvailableToClaim = value; }
        }

        private int _AwardedTo;
        public int AwardedTo
        {
            get { return _AwardedTo; }
            set { _AwardedTo = value; }
        }

        private string _AwardWhen;
        public string AwardWhen
        {
            get { return _AwardWhen; }
            set { _AwardWhen = value; }
        }

        private int _CampaignCPOpportunityID;
        public int CampaignCPOpportunityID
        {
            get { return _CampaignCPOpportunityID; }
            set { _CampaignCPOpportunityID = value; }
        }

        private int _CampaignCPOpportunityDefaultID;
        public int CampaignCPOpportunityDefaultID
        {
            get { return _CampaignCPOpportunityDefaultID; }
            set { _CampaignCPOpportunityDefaultID = value; }
        }

        private int _CampaignID;
        public int CampaignID
        {
            get { return _CampaignID; }
            set { _CampaignID = value; }
        }

        private int _CampaignPlayerID;
        public int CampaignPlayerID
        {
            get { return _CampaignPlayerID; }
            set { _CampaignPlayerID = value; }
        }

        private bool _CarryToNextEvent;
        public bool CarryToNextEvent
        {
            get { return _CarryToNextEvent; }
            set { _CarryToNextEvent = value; }
        }

        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }

        private bool _CountTransfersAgainstMax;
        public bool CountTransfersAgainstMax
        {
            get { return _CountTransfersAgainstMax; }
            set { _CountTransfersAgainstMax = value; }
        }

        private DateTime _DateAccepted;
        public DateTime DateAccepted
        {
            get { return _DateAccepted; }
            set { _DateAccepted = value; }
        }

        private int _DefaultAwardWhen;
        public int DefaultAwardWhen
        {
            get { return _DefaultAwardWhen; }
            set { _DefaultAwardWhen = value; }
        }

        private double _DefaultDollarToPointValue;
        public double DefaultDollarToPointValue
        {
            get { return _DefaultDollarToPointValue; }
            set { _DefaultDollarToPointValue = value; }
        }

        private double _DefaultHourToPointValue;
        public double DefaultHourToPointValue
        {
            get { return _DefaultHourToPointValue; }
            set { _DefaultHourToPointValue = value; }
        }

        private string _DefaultNotificationEmail;
        public string DefaultNotificationEmail
        {
            get { return _DefaultNotificationEmail; }
            set { _DefaultNotificationEmail = value; }
        }

        private string _DefaultShipToAdd1;
        public string DefaultShipToAdd1
        {
            get { return _DefaultShipToAdd1; }
            set { _DefaultShipToAdd1 = value; }
        }

        private string _DefaultShipToAdd2;
        public string DefaultShipToAdd2
        {
            get { return _DefaultShipToAdd2; }
            set { _DefaultShipToAdd2 = value; }
        }

        private string _DefaultShipToCity;
        public string DefaultShipToCity
        {
            get { return _DefaultShipToCity; }
            set { _DefaultShipToCity = value; }
        }

        private string _DefaultShipToPhone;
        public string DefaultShipToPhone
        {
            get { return _DefaultShipToPhone; }
            set { _DefaultShipToPhone = value; }
        }

        private string _DefaultShipToPostalCode;
        public string DefaultShipToPostalCode
        {
            get { return _DefaultShipToPostalCode; }
            set { _DefaultShipToPostalCode = value; }
        }

        private string _DefaultShipToState;
        public string DefaultShipToState
        {
            get { return _DefaultShipToState; }
            set { _DefaultShipToState = value; }
        }

        private string _DeliveryMethod;
        public string DeliveryMethod
        {
            get { return _DeliveryMethod; }
            set { _DeliveryMethod = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private int _DisplayOrder;
        public int DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }

        private string _DisplayWorth;
        public string DisplayWorth
        {
            get { return _DisplayWorth; }
            set { _DisplayWorth = value; }
        }

        private int _DonationClaimID;
        public int DonationClaimID
        {
            get { return _DonationClaimID; }
            set { _DonationClaimID = value; }
        }

        private string _DonationComments;
        public string DonationComments
        {
            get { return _DonationComments; }
            set { _DonationComments = value; }
        }

        private int _DonationID;
        public int DonationID
        {
            get { return _DonationID; }
            set { _DonationID = value; }
        }

        private int _DonationRewardUnitID;
        public int DonationRewardUnitID
        {
            get { return _DonationRewardUnitID; }
            set { _DonationRewardUnitID = value; }
        }

        private int _DonationSettingsID;
        public int DonationSettingsID
        {
            get { return _DonationSettingsID; }
            set { _DonationSettingsID = value; }
        }

        private int _DonationTemplateID;
        public int DonationTemplateID
        {
            get { return _DonationTemplateID; }
            set { _DonationTemplateID = value; }
        }

        private string _DonationTypeDescription;
        public string DonationTypeDescription
        {
            get { return _DonationTypeDescription; }
            set { _DonationTypeDescription = value; }
        }

        private int _DonationTypeID;
        public int DonationTypeID
        {
            get { return _DonationTypeID; }
            set { _DonationTypeID = value; }
        }

        private int _EventID;
        public int EventID
        {
            get { return _EventID; }
            set { _EventID = value; }
        }

        private string _EventName;
        public string EventName
        {
            get { return _EventName; }
            set { _EventName = value; }
        }

        private bool _EveryEvent;
        public bool EveryEvent
        {
            get { return _EveryEvent; }
            set { _EveryEvent = value; }
        }

        private DateTime _ExpirationDate;
        public DateTime ExpirationDate
        {
            get { return _ExpirationDate; }
            set { _ExpirationDate = value; }
        }

        private bool _HideDonators;
        public bool HideDonators
        {
            get { return _HideDonators; }
            set { _HideDonators = value; }
        }

        private double _HoursAllowed;
        public double HoursAllowed
        {
            get { return _HoursAllowed; }
            set { _HoursAllowed = value; }
        }

        private string _Item;
        public string Item
        {
            get { return _Item; }
            set { _Item = value; }
        }

        private bool _MaterialsReimbursable;
        public bool MaterialsReimbursable
        {
            get { return _MaterialsReimbursable; }
            set { _MaterialsReimbursable = value; }
        }

        private double _MaxItemsPerEvent;
        public double MaxItemsPerEvent
        {
            get { return _MaxItemsPerEvent; }
            set { _MaxItemsPerEvent = value; }
        }

        private double _MaxPointsPerEvent;
        public double MaxPointsPerEvent
        {
            get { return _MaxPointsPerEvent; }
            set { _MaxPointsPerEvent = value; }
        }

        private string _NotificationEmail;
        public string NotificationEmail
        {
            get { return _NotificationEmail; }
            set { _NotificationEmail = value; }
        }

        private string _PlayerComments;
        public string PlayerComments
        {
            get { return _PlayerComments; }
            set { _PlayerComments = value; }
        }

        private string _PoolDescription;
        public string PoolDescription
        {
            get { return _PoolDescription; }
            set { _PoolDescription = value; }
        }

        private double _QtyNeeded;
        public double QtyNeeded
        {
            get { return _QtyNeeded; }
            set { _QtyNeeded = value; }
        }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }

        private int _RegistrationID;
        public int RegistrationID
        {
            get { return _RegistrationID; }
            set { _RegistrationID = value; }
        }

        private string _ReimbuirseType;
        public string ReimbuirseType
        {
            get { return _ReimbuirseType; }
            set { _ReimbuirseType = value; }
        }

        private DateTime _RequiredByDate;
        public DateTime RequiredByDate
        {
            get { return _RequiredByDate; }
            set { _RequiredByDate = value; }
        }

        private double _RewardQty;
        public double RewardQty
        {
            get { return _RewardQty; }
            set { _RewardQty = value; }
        }

        private string _RewardUnit;
        public string RewardUnit
        {
            get { return _RewardUnit; }
            set { _RewardUnit = value; }
        }

        private string _RewardUnitDescription;
        public string RewardUnitDescription
        {
            get { return _RewardUnitDescription; }
            set { _RewardUnitDescription = value; }
        }

        private string _ShipToAdd1;
        public string ShipToAdd1
        {
            get { return _ShipToAdd1; }
            set { _ShipToAdd1 = value; }
        }

        private string _ShipToAdd2;
        public string ShipToAdd2
        {
            get { return _ShipToAdd2; }
            set { _ShipToAdd2 = value; }
        }

        private string _ShipToCity;
        public string ShipToCity
        {
            get { return _ShipToCity; }
            set { _ShipToCity = value; }
        }

        private string _ShipToPhone;
        public string ShipToPhone
        {
            get { return _ShipToPhone; }
            set { _ShipToPhone = value; }
        }

        private string _ShipToPostalCode;
        public string ShipToPostalCode
        {
            get { return _ShipToPostalCode; }
            set { _ShipToPostalCode = value; }
        }

        private string _ShipToState;
        public string ShipToState
        {
            get { return _ShipToState; }
            set { _ShipToState = value; }
        }

        private bool _ShowDonationClaims;
        public bool ShowDonationClaims
        {
            get { return _ShowDonationClaims; }
            set { _ShowDonationClaims = value; }
        }

        private string _StaffComments;
        public string StaffComments
        {
            get { return _StaffComments; }
            set { _StaffComments = value; }
        }

        private string _StatusDescription;
        public string StatusDescription
        {
            get { return _StatusDescription; }
            set { _StatusDescription = value; }
        }
        private int _StatusID;
        public int StatusID
        {
            get { return _StatusID; }
            set { _StatusID = value; }
        }

        private double _TimeAllowed;
        public double TimeAllowed
        {
            get { return _TimeAllowed; }
            set { _TimeAllowed = value; }
        }

        private double _UserID;
        public double UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _Worth;
        public string Worth
        {
            get { return _Worth; }
            set { _Worth = value; }
        }

        public void UpdateDonationStatus(int DonationID, int CampaignID, string StatusDescription)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            string stStoredProc = "uspUpdateDonationStatus";
            SortedList sParams = new SortedList();
            sParams.Add("@DonationID", DonationID);
            sParams.Add("@CampaignID", CampaignID);
            sParams.Add("@StatusDescription", StatusDescription);
            try
            {
                cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", UserID.ToString());
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }

        }

        public Boolean SaveDonationClaims(int UserID, int DonationClaimID, int DonationID, int CampaignPlayerID, int Qty, int RegID, int CPOppID,
            string PlayerComments, string StaffComments, string DeliveryMethod, int AwardedTo, DateTime dtAccepted, int AcceptedBy, string Comments)
        {

            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Boolean blnReturn = false;
            DataTable dtDonationClaim = new DataTable();

            try
            {
                SortedList slParams = new SortedList();
                // slParams.Add("@Parmeter1", strParameter1)
                slParams.Add("@UserID", UserID);
                slParams.Add("@DonationClaimID", DonationClaimID);
                slParams.Add("@DonationID", DonationID);
                slParams.Add("@CampaignPlayerID", CampaignPlayerID);
                slParams.Add("@Quantity", Qty);
                slParams.Add("@RegistrationID", RegID);
                slParams.Add("@CampaignCPOpportunityID", CPOppID);
                slParams.Add("@PlayerComments", PlayerComments);
                slParams.Add("@StaffComments", StaffComments);
                slParams.Add("@DeliveryMethod", DeliveryMethod);
                slParams.Add("@AwardedTo", AwardedTo);
                //slParams.Add("@DateAccepted", dtAccepted);
                slParams.Add("@AcceptedBy", AcceptedBy);
                slParams.Add("@Comments", Comments);
                dtDonationClaim = cUtilities.LoadDataTable("uspInsUpdCMDonationClaims", slParams, "LARPortal", UserID.ToString(), lsRoutineName);
                int iTemp = 0;
                foreach (DataRow drow in dtDonationClaim.Rows)
                {
                    if (int.TryParse(drow["DonationClaimID"].ToString(), out iTemp))
                        DonationClaimID = iTemp;
                }
                UpdateOpportunityWithClaim(CPOppID, DonationClaimID, UserID);
                blnReturn = true;
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
                blnReturn = false;
            }
            return blnReturn;

        }

        public void UpdateOpportunityWithClaim(int OpportunityID, int ClaimID, int UserID)
        {

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            string stStoredProc = "uspUpdateOpportunityWithDonationClaim";
            SortedList sParams = new SortedList();
            sParams.Add("@CPOpportunityID", OpportunityID);
            sParams.Add("@DonationClaimID", ClaimID);
            try
            {
                cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", UserID.ToString());
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }

        public DataTable GetDonationClaims(int UserID, int DonationID, int? PlayerID = null)
        {
            string stStoredProc = "uspGetDonationClaims";
            string stCallingMethod = "cDonation.GetDonationClaims";
            int DonationCount;
            DonationCount = 0;
            int iTemp;
            SortedList slParameters = new SortedList();
            slParameters.Add("@DonationID", DonationID);
            if (PlayerID != null)
                slParameters.Add("@PlayerID", PlayerID);

            DataTable dtDonationList = new DataTable();
            DataSet dsDonationList = new DataSet();
            dtDonationList = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow dRow in dtDonationList.Rows)
            {
                DonationCount++;
                if (int.TryParse(dRow["AlreadyClaimedByThisPlayer"].ToString(), out iTemp))
                    AlreadyClaimedByThisPlayer = iTemp;
                EventName = dRow["EventName"].ToString();
                DisplayWorth = dRow["DisplayWorth"].ToString();
                Item = dRow["Item"].ToString();
            }
            return dtDonationList;
        }

        public DataTable GetDonationClaimsForPlayer(int UserID, int CampaignID, int DonationID)
        {
            string stStoredProc = "uspGetDonationClaimsForPlayer";
            string stCallingMethod = "cDonation.GetDonationClaimsForPlayer";
            int DonationCount;
            DonationCount = 0;
            int iTemp;
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CampaignID", CampaignID);
            slParameters.Add("@DonationID", DonationID);
            DataTable dtDonationList = new DataTable();
            DataSet dsDonationList = new DataSet();
            dtDonationList = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow dRow in dtDonationList.Rows)
            {
                DonationCount++;
                if (int.TryParse(dRow["AlreadyClaimedByThisPlayer"].ToString(), out iTemp))
                    AlreadyClaimedByThisPlayer = iTemp;
                if (int.TryParse(dRow["AvailableToClaim"].ToString(), out iTemp))
                    AvailableToClaim = iTemp;
                EventName = dRow["EventName"].ToString();
                DisplayWorth = dRow["DisplayWorth"].ToString();
                Item = dRow["Item"].ToString();
            }
            return dtDonationList;
        }

        public void GetDonationCampaignSettings(int CampaignID)
        {
            bool bTemp;
            int iTemp;
            double dTemp;
            string stStoredProc = "uspGetDonationCampaignSettings";
            string stCallingMethod = "cDonation.GetDonationCampaignSettings";
            SortedList slParams = new SortedList();
            DataTable dtDonationSettings = new DataTable();
            slParams.Add("@CampaignID", CampaignID);
            dtDonationSettings = cUtilities.LoadDataTable(stStoredProc, slParams, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow drow in dtDonationSettings.Rows)
            {
                // DefaultAwardWhen 1-Immediately / 2-On Delivery / 3-On Approval
                DefaultShipToAdd1 = drow["DefaultShipToAdd1"].ToString();
                DefaultShipToAdd2 = drow["DefaultShipToAdd2"].ToString();
                DefaultShipToCity = drow["DefaultShipToCity"].ToString();
                DefaultShipToState = drow["DefaultShipToState"].ToString();
                DefaultShipToPostalCode = drow["DefaultShipToPostalCode"].ToString();
                DefaultShipToPhone = drow["DefaultShipToPhone"].ToString();
                DefaultNotificationEmail = drow["DefaultNotificationEmail"].ToString();
                DonationRewardUnitID = int.Parse(drow["DonationRewardUnitID"].ToString());
                PoolDescription = drow["PoolDescription"].ToString();
                StatusID = int.Parse(drow["CampaignDonationStatusID"].ToString());
                StatusDescription = drow["StatusDescription"].ToString();

                if (int.TryParse(drow["DefaultAwardWhen"].ToString(), out iTemp))
                {
                    DefaultAwardWhen = iTemp;
                }
                if (int.TryParse(drow["CampaignCPOpportunityDefaultID"].ToString(), out iTemp))
                {
                    CampaignCPOpportunityDefaultID = iTemp;
                }
                if (double.TryParse(drow["DefaultHourToPointValue"].ToString(), out dTemp))
                {
                    DefaultHourToPointValue = dTemp;
                }
                if (double.TryParse(drow["DefaultDollarToPointValue"].ToString(), out dTemp))
                {
                    DefaultDollarToPointValue = dTemp;
                }
                if (bool.TryParse(drow["ShowDonationClaims"].ToString(), out bTemp))
                {
                    ShowDonationClaims = bTemp;
                }
                if (bool.TryParse(drow["AllowPlayerToPlayerPoints"].ToString(), out bTemp))
                {
                    AllowPlayerToPlayerPoints = bTemp;
                }

            }
        }

        public void UpdateDonation(int UserID, int DonationID, int CampaignID, int EventID, string Description, int DonationTypeID, int QuantityNeeded,
            double RewardQty, int RewardUnit, string DonationComments, string URL, string StaffComments, DateTime RequiredByDate, string STAdd1, string STAdd2,
            string STCity, string STState, string STZip, string STPhone, string STEmail, int StatusID,
            bool Recurring, bool MaterialsReimbursable, int ReimburseType, double HoursAllowed, bool HideDonators, DateTime ExpirationDate,
            bool CarryOver, int AwardWhen, bool AllowEventFee)

        {

            string stStoredProc = "uspInsUpdCMDonations";
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@DonationID", DonationID);
            slParameters.Add("@Description", Description);
            slParameters.Add("@DonationTypeID", DonationTypeID);
            slParameters.Add("@RewardQty", RewardQty);
            slParameters.Add("@Worth", RewardQty.ToString());
            slParameters.Add("@RewardUnit", RewardUnit);
            slParameters.Add("@QtyNeeded", QuantityNeeded);
            slParameters.Add("@DonationComments", DonationComments);
            slParameters.Add("@URL", URL);
            slParameters.Add("@StaffComments", StaffComments);
            slParameters.Add("@RequiredByDate", RequiredByDate);
            slParameters.Add("@ShipToAdd1", STAdd1);
            slParameters.Add("@ShipToAdd2", STAdd2);
            slParameters.Add("@ShipToCity", STCity);
            slParameters.Add("@ShipToState", STState);
            slParameters.Add("@ShipToPostalCode", STZip);
            slParameters.Add("@ShipToPhone", STPhone);
            slParameters.Add("@NotificationEmail", STEmail);
            slParameters.Add("@StatusID", StatusID);
            slParameters.Add("@Recurring", Recurring);
            slParameters.Add("@MaterialsReimbursable", MaterialsReimbursable);
            slParameters.Add("@ReimbuirseType", ReimburseType);
            slParameters.Add("@HoursAllowed", HoursAllowed);
            slParameters.Add("@HideDonators", HideDonators);
            slParameters.Add("@ExpirationDate", ExpirationDate);
            slParameters.Add("@CarryToNextEvent", CarryOver);
            slParameters.Add("@AwardWhen", AwardWhen);
            slParameters.Add("@AllowEventFee", AllowEventFee);

            if (DonationID == -1)
            {
                slParameters.Add("@CampaignID", CampaignID);
                slParameters.Add("@EventID", EventID);



            }
            try
            {
                cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
            }
            catch (Exception ex)
            {
                string l = ex.Message;
            }


        }
    }
}