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
    public class cPlayerAffiliation
    {
        public Int32 PlayerAffiliationID { get; set; }
        public Int32 PlayerProfileID { get; set; }
        public string AffiliationName { get; set; }
        public string AffiliationRole { get; set; }
        public string PlayerComments { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cPlayerAffiliation()
        {
            PlayerAffiliationID = -1;
            PlayerProfileID = -1;
            AffiliationName = "";
            AffiliationRole = "";
            PlayerComments = "";
            Comments = "";
            RecordStatus = RecordStatuses.Active;
        }

        public cPlayerAffiliation(Int32 iPlayerAffilID, string sUserName, Int32 iUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            PlayerAffiliationID = iPlayerAffilID;

            SortedList slParams = new SortedList();
            slParams.Add("@PlayerAffiliationID", iPlayerAffilID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerAffiliationByID", slParams, "LARPortal", sUserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    AffiliationName = ldt.Rows[0]["AffiliationName"].ToString();
                    AffiliationRole = ldt.Rows[0]["AffiliationRole"].ToString();
                    PlayerComments = ldt.Rows[0]["PlayerComments"].ToString();
                    Comments = ldt.Rows[0]["Comments"].ToString();

                    int iTemp;
                    if (int.TryParse(ldt.Rows[0]["PlayerProfileID"].ToString(), out iTemp))
                        PlayerProfileID = iTemp;
 
                    RecordStatus = RecordStatuses.Active;
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, sUserName + lsRoutineName);
            }
        }
        public void Save(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (RecordStatus == RecordStatuses.Delete)
                Delete(UserName, UserID);
            else
            {
                try
                {
                    SortedList slParams = new SortedList();
                    slParams.Add("@UserID", UserID);
                    slParams.Add("@PlayerAffiliationID", PlayerAffiliationID);
                    slParams.Add("@PlayerProfileID", PlayerProfileID);
                    slParams.Add("@AffiliationName", AffiliationName);
                    slParams.Add("@AffiliationRole", AffiliationRole);
                    slParams.Add("@PlayerComments", PlayerComments);
                    slParams.Add("@Comments", Comments);
                    cUtilities.PerformNonQuery("uspInsUpdPLPlayerAffiliations", slParams, "LARPortal", UserName);
                }
                catch (Exception ex)
                {
                    ErrorAtServer lobjError = new ErrorAtServer();
                    lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
                }
            }
        }

        public void Delete(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (RecordStatus == RecordStatuses.Active)
                Save(UserName, UserID);
            else
            {
                try
                {
                    SortedList slParams = new SortedList();
                    slParams.Add("@UserID", UserID);
                    slParams.Add("@RecordID", PlayerAffiliationID);
                    cUtilities.PerformNonQuery("uspDelPLPlayerAffiliations", slParams, "LARPortal", UserName);
                }
                catch (Exception ex)
                {
                    ErrorAtServer lobjError = new ErrorAtServer();
                    lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
                }
            }
        }
    }
}