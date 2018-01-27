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
    public class cPlayerLimitation
    {
        public Int32 PlayerLimitationID { get; set; }
        public Int32 PlayerProfileID { get; set; }
        public string Description { get; set; }
        public Boolean ShareInfo { get; set; }
        public Boolean PrintOnCard { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cPlayerLimitation()
        {
            PlayerLimitationID = -1;
            PlayerProfileID = -1;
            Description = "";
            ShareInfo = false;
            PrintOnCard = false;
            StartDate = null;
            EndDate = null;
            Comments = "";
            RecordStatus = RecordStatuses.Active;
        }

        public cPlayerLimitation(Int32 intPlayerLimitationID, string UserName)
        {
            Load(intPlayerLimitationID, UserName);
        }

        public void Load(Int32 intPlayerLimitationID, string UserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            PlayerLimitationID = intPlayerLimitationID;

            SortedList slParams = new SortedList();
            slParams.Add("@PlayerLimitationID", intPlayerLimitationID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerLimitationsByID", slParams, "LARPortal", UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    int iTemp;
                    bool bTemp;
                    DateTime dtTemp;
                    DataRow dRow = ldt.Rows[0];

                    if (int.TryParse(dRow["PlayerLimitationID"].ToString(), out iTemp))
                        PlayerLimitationID = iTemp;
                    if (int.TryParse(dRow["PlayerProfileID"].ToString(), out iTemp))
                        PlayerProfileID = iTemp;

                    Description = dRow["Description"].ToString();
                    if (bool.TryParse(dRow["ShareInfo"].ToString(), out bTemp))
                        ShareInfo = bTemp;
                    if (bool.TryParse(dRow["PrintOnCard"].ToString(), out bTemp))
                        PrintOnCard = bTemp;
                    if (DateTime.TryParse(dRow["StartDate"].ToString(), out dtTemp))
                        StartDate = dtTemp;
                    if (DateTime.TryParse(dRow["EndDate"].ToString(), out dtTemp))
                        EndDate = dtTemp;
                    Comments = dRow["Comments"].ToString();
                    RecordStatus = RecordStatuses.Active;
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        public void Save(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (RecordStatus == RecordStatuses.Delete)
            {
                Delete(UserName, UserID);
            }
            else
            {
                try
                {
                    SortedList slParams = new SortedList();
                    slParams.Add("@UserID", UserID);
                    slParams.Add("@PlayerLimitationID", PlayerLimitationID);
                    slParams.Add("@PlayerProfileID", PlayerProfileID);
                    slParams.Add("@Description", Description);
                    slParams.Add("@ShareInfo", ShareInfo);
                    slParams.Add("@PrintOnCard", PrintOnCard);

                    if (StartDate.HasValue)
                        slParams.Add("@StartDate", StartDate.Value);
                    if (EndDate.HasValue)
                        slParams.Add("@EndDate", EndDate.Value);
                    slParams.Add("@Comments", Comments);

                    cUtilities.PerformNonQueryBoolean("uspInsUpdPLPlayerLimitations", slParams, "LARPortal", UserName);
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

            try
            {
                SortedList slParams = new SortedList();
                slParams.Add("@UserID", UserID);
                slParams.Add("@RecordID", PlayerLimitationID);
                cUtilities.PerformNonQueryBoolean("uspDelPLPlayerLimitations", slParams, "LARPortal", UserName);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }
    }
}