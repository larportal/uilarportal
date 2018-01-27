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
    public class cPlayerResume
    {
        public int PlayerProfileID { get; set; }
        public string PlayerComments { get; set; }
        public List<cPlayerSkill> PlayerSkills { get; set; }
        public List<cPlayerAffiliation> PlayerAffiliations { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cPlayerResume()
        {
            PlayerProfileID = -1;
            PlayerSkills = new List<cPlayerSkill>();
            PlayerAffiliations = new List<cPlayerAffiliation>();
            RecordStatus = RecordStatuses.Active;
        }

        public void Load(string UserName, Int32 intUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@PlayerResumeID", PlayerProfileID);
            try
            {
                DataSet ldsPlayerInfo = cUtilities.LoadDataSet("uspGetPlayerResumeByID", slParams, "LARPortal", UserName, lsRoutineName);
                if (ldsPlayerInfo.Tables[0].Rows.Count > 0)
                {
                    PlayerComments = ldsPlayerInfo.Tables[0].Rows[0]["ResumeContents"].ToString();
                }

                PlayerSkills = new List<cPlayerSkill>();

                foreach (DataRow dRow in ldsPlayerInfo.Tables[1].Rows)
                {
                    cPlayerSkill NewSkill = new cPlayerSkill
                    {
                        SkillName = dRow["SkillName"].ToString(),
                        SkillLevel = dRow["SkillLevel"].ToString(),
                        PlayerComments = dRow["PlayerComments"].ToString()
                    };

                    NewSkill.PlayerProfileID = PlayerProfileID;

                    int iTemp;
                    if (int.TryParse(dRow["'PlayerSkillID"].ToString(), out iTemp))
                        NewSkill.PlayerSkillID = iTemp;

                    PlayerSkills.Add(NewSkill);
                }

                PlayerAffiliations = new List<cPlayerAffiliation>();

                foreach (DataRow dRow in ldsPlayerInfo.Tables[2].Rows)
                {
                    cPlayerAffiliation NewAffil = new cPlayerAffiliation
                    {
                        AffiliationName = dRow["AffiliationName"].ToString(),
                        AffiliationRole = dRow["AffiliationRole"].ToString(),
                        PlayerComments = dRow["PlayerComments"].ToString(),
                        Comments = dRow["Comments"].ToString()
                    };

                    NewAffil.PlayerProfileID = PlayerProfileID;

                    int iTemp;
                    if (int.TryParse(dRow["'PlayerAffiliationID"].ToString(), out iTemp))
                        NewAffil.PlayerAffiliationID = iTemp;

                    PlayerAffiliations.Add(NewAffil);
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
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (RecordStatus == RecordStatuses.Delete)
                PlayerComments = "";
            try
            {
                SortedList slParams = new SortedList();
                slParams.Add("@UserID", UserID);
                slParams.Add("@PlayerProfileID", PlayerProfileID);
                slParams.Add("@ResumeComments", PlayerComments);
                cUtilities.PerformNonQueryBoolean("uspInsUpdPLPlayerProfiles", slParams, "LARPortal", UserName);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }
    }
}