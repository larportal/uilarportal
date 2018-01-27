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
    public class cPlayerMedical
    {
        public Int32 PlayerMedicalID { get; set; }
        public Int32 PlayerProfileID { get; set; }
        public Int32 MedicalTypeID { get; set; }
        public string MedicalTypeDescription { get; set; }
        public string Description { get; set; }
        public string Medication { get; set; }
        public Boolean ShareInfo { get; set; }
        public Boolean PrintOnCard { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cPlayerMedical()
        {
            PlayerMedicalID = -1;
            PlayerProfileID = -1;
            MedicalTypeID = -1;
            MedicalTypeDescription = "";
            Description = "";
            Medication = "";
            ShareInfo = false;
            PrintOnCard = false;
            StartDate = null;
            EndDate = null;
            Comments = "";
            RecordStatus = RecordStatuses.Active;
        }

        public cPlayerMedical(Int32 intPlayerMedicalID, string UserName)
        {
            Load(intPlayerMedicalID, UserName);
        }

        public void Load(Int32 intPlayerMedicalID, string UserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            PlayerMedicalID = intPlayerMedicalID;

            SortedList slParams = new SortedList();
            slParams.Add("@PlayerMedicalID", intPlayerMedicalID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerMedicalByID", slParams, "LARPortal", UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    int iTemp;
                    bool bTemp;
                    DateTime dtTemp;
                    DataRow dRow = ldt.Rows[0];

                    if (int.TryParse(dRow["PlayerMedicalID"].ToString(), out iTemp))
                        PlayerMedicalID = iTemp;
                    if (int.TryParse(dRow["PlayerProfileID"].ToString(), out iTemp))
                        PlayerProfileID = iTemp;
                    if (int.TryParse(dRow["MedicalTypeID"].ToString(), out iTemp))
                        MedicalTypeID = iTemp;

//                    MedicalTypeDescription = dRow["MedicalTypeDescription"].ToString();
                    Description = dRow["Description"].ToString();
                    Medication = dRow["Medication"].ToString();
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
                    slParams.Add("@PlayerMedicalID", PlayerMedicalID);
                    slParams.Add("@PlayerProfileID", PlayerProfileID);
                    slParams.Add("@MedicalTypeID", MedicalTypeID);
                    slParams.Add("@Description", Description);
                    slParams.Add("@Medication", Medication);
                    slParams.Add("@ShareInfo", ShareInfo);
                    slParams.Add("@PrintOnCard", PrintOnCard);

                    if (StartDate.HasValue)
                        slParams.Add("@StartDate", StartDate.Value);
                    else
                        slParams.Add("@ClearStartDate", "1");
                    if (EndDate.HasValue)
                        slParams.Add("@EndDate", EndDate.Value);
                    else
                        slParams.Add("@ClearEndDate", "1");
                    slParams.Add("@Comments", Comments);

                    cUtilities.PerformNonQueryBoolean("uspInsUpdPLPlayerMedical", slParams, "LARPortal", UserName);
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
                slParams.Add("@RecordID", PlayerMedicalID);
                cUtilities.PerformNonQueryBoolean("uspDelPLPlayerMedical", slParams, "LARPortal", UserName);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }
    }
}