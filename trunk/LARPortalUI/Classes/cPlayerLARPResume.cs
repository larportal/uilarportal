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
    public class cPlayerLARPResume
    {
        public int PlayerLARPResumeID { get; set; }
        public int PlayerProfileID { get; set; }
        public string GameSystem { get; set; }
        public string Campaign { get; set; }
        public string AuthorGM { get; set; }
        public int StyleID { get; set; }
        public string Style { get; set; }
        public int GenreID { get; set; }
        public string Genre { get; set; }
        public int RoleID { get; set; }
        public string Role { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PlayerComments { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cPlayerLARPResume()
        {
            PlayerLARPResumeID = -1;
            RecordStatus = RecordStatuses.Active;
        }

        public void Load(string UserName, Int32 intUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList slParams = new SortedList();
            slParams.Add("@PlayerLARPResumeID", PlayerLARPResumeID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerLARPResumeByID", slParams, "LARPortal", UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    int iTemp;
                    DateTime dtTemp;
                    DataRow dRow = ldt.Rows[0];

                    if (int.TryParse(dRow["PlayerProfileID"].ToString(), out iTemp))
                        PlayerProfileID = iTemp;
                    GameSystem = dRow["GameSystem"].ToString();
                    Campaign = dRow["Campaign"].ToString();
                    AuthorGM = dRow["AuthorGM"].ToString();
                    Style = dRow["Style"].ToString();
                    if (int.TryParse(dRow["StyleID"].ToString(), out iTemp))
                        StyleID = iTemp;
                    Genre = dRow["Genre"].ToString();
                    if (int.TryParse(dRow["GenreID"].ToString(), out iTemp))
                        GenreID = iTemp;
                    Role = dRow["Role"].ToString();
                    if (int.TryParse(dRow["RoleID"].ToString(), out iTemp))
                        RoleID = iTemp;
                    if (DateTime.TryParse(dRow["StartDate"].ToString(), out dtTemp))
                        StartDate = dtTemp;
                    if (DateTime.TryParse(dRow["EndDate"].ToString(), out dtTemp))
                        EndDate = dtTemp;
                    PlayerComments = dRow["PlayerComments"].ToString();
                    Comments = dRow["Comments"].ToString();
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
                    slParams.Add("@PlayerLARPResumeID", PlayerLARPResumeID);
                    slParams.Add("@PlayerProfileID", PlayerProfileID);
                    slParams.Add("@GameSystem", GameSystem);
                    slParams.Add("@Campaign", Campaign);
                    slParams.Add("@AuthorGM", AuthorGM);
                    slParams.Add("@Style", StyleID);
                    slParams.Add("@Genre", GenreID);
                    slParams.Add("@RoleID", RoleID);
                    if (StartDate.HasValue)
                        slParams.Add("@StartDate", StartDate.Value);
                    if (EndDate.HasValue)
                        slParams.Add("@EndDate", EndDate.Value);
                    slParams.Add("@PlayerComments", PlayerComments);
                    slParams.Add("@Comments", Comments);
                    cUtilities.PerformNonQueryBoolean("uspInsUpdPLPlayerLARPResumes", slParams, "LARPortal", UserName);
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
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList();
                slParams.Add("@UserID", UserID);
                slParams.Add("@RecordID", PlayerLARPResumeID);
                cUtilities.PerformNonQueryBoolean("uspDelPLPlayerLARPResumes", slParams, "LARPortal", UserName);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }
    }
}