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
    public class cPlayerSkill
    {
        public Int32 PlayerSkillID { get; set; }
        public Int32 PlayerProfileID { get; set; }
        public string SkillName { get; set; }
        public string SkillLevel { get; set; }
        public string PlayerComments { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cPlayerSkill()
        {
            PlayerSkillID = -1;
            PlayerProfileID = -1;
            SkillName = "";
            SkillLevel = "";
            PlayerComments = "";
            Comments = "";
            RecordStatus = RecordStatuses.Active;
        }

        public cPlayerSkill(Int32 intPlayerSkillID, string strUserName, Int32 intUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            PlayerSkillID = intPlayerSkillID;

            SortedList slParams = new SortedList();
            slParams.Add("@PlayerSkillID", intPlayerSkillID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerSkillsByID", slParams, "LARPortal", strUserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    SkillName = ldt.Rows[0]["SkillName"].ToString();
                    SkillLevel = ldt.Rows[0]["SkillLevel"].ToString();
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
                lobjError.ProcessError(ex, lsRoutineName, strUserName + lsRoutineName);
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
                    slParams.Add("@PlayerSkillID", PlayerSkillID);
                    slParams.Add("@PlayerProfileID", PlayerProfileID);
                    slParams.Add("@SkillName", SkillName);
                    slParams.Add("@SkillLevel", SkillLevel);
                    slParams.Add("@PlayerComments", PlayerComments);
                    slParams.Add("@Comments", Comments);
                    cUtilities.PerformNonQuery("uspInsUpdPLPlayerSkills", slParams, "LARPortal", UserName);
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
                    slParams.Add("@RecordID", PlayerSkillID);
                    cUtilities.PerformNonQuery("uspDelPLPlayerSkills", slParams, "LARPortal", UserName);
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