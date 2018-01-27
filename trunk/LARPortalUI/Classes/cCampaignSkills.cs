using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LarpPortal.Classes
{
    [Serializable]
    class cCampaignSkills
    {
        public cCampaignSkills()
        {
        }

        public int CampaignID { get; set; }
        public List<cCampaignHeader> Headers = new List<cCampaignHeader>();

        public int Load(int CampaignID, string UserID)
        {
            this.CampaignID = CampaignID;

            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            sParam.Add("@CampaignID", CampaignID);

            DataSet dsCampaignSkills = new DataSet();
            dsCampaignSkills = cUtilities.LoadDataSet("uspGetCampaignSkills", sParam, "LARPortal", UserID, lsRoutineName);

            dsCampaignSkills.Tables[0].TableName = "CHSkillTypes";
            dsCampaignSkills.Tables[1].TableName = "CHCampaignSkillsStandard";

            foreach (DataRow dHeaderRow in dsCampaignSkills.Tables["CHSkillTypes"].Rows)
            {
                int iTemp;
                cCampaignHeader cNewHeader = new cCampaignHeader { HeaderDescription = dHeaderRow["SkillTypeDescription"].ToString() };
                if (int.TryParse(dHeaderRow["SkillTypeID"].ToString(), out iTemp))
                {
                    DataView dvSkills = new DataView(dsCampaignSkills.Tables["CHCampaignSkillsStandard"], "SkillTypeID = " + iTemp.ToString(), "", DataViewRowState.CurrentRows);
                    foreach (DataRowView dSkillRow in dvSkills)
                    {
                        cCampaignSkill cNewSkill = new cCampaignSkill
                        {
                            SkillName = dSkillRow["SkillName"].ToString(),
                            SkillShortDescription = dSkillRow["SkillShortDescription"].ToString(),
                            SkillLongDescription = dSkillRow["SkillLongDescription"].ToString(),
                            Comments = dSkillRow["Comments"].ToString(),
                            RecordStatus = RecordStatuses.Active
                        };

                        cNewSkill.CampaignID = CampaignID;
                        if (int.TryParse(dSkillRow["CampaignSkillsStandardID"].ToString(), out iTemp))
                            cNewSkill.CampaignSkillsStandardID = iTemp;
                        if (dSkillRow["HeaderAssociation"] != DBNull.Value)
                            if (int.TryParse(dSkillRow["HeaderAssociation"].ToString(), out iTemp))
                                cNewSkill.HeaderAssociation = iTemp;
                        cNewHeader.Skills.Add(cNewSkill);
                    }
                }
            }
            return 0;
        }
    }


    class cCampaignHeader
    {
        public cCampaignHeader()
        {
            HeaderID = -1;
        }

        public override string ToString()
        {
            return "Don't use this. Use something else.";
        }

        public int HeaderID { get; set; }
        public string HeaderDescription { get; set; }
        public List<cCampaignSkill> Skills = new List<cCampaignSkill>();

        public int Load(int CharacterIDToLoad)
        {
            int iNumCharacterRecords = 0;

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            sParam.Add("@CharacterID", CharacterIDToLoad);

            DataSet dsCharacterInfo = new DataSet();
            dsCharacterInfo = cUtilities.LoadDataSet("uspGetCharacterInfo", sParam, "LARPortal", "GeneralUser", lsRoutineName);

            return iNumCharacterRecords;
        }

        public int Save(string sUserUpdating)
        {
            return 0;
        }
    }


    class cCampaignSkill
    {
        public cCampaignSkill()
        {
        }

        public int CampaignSkillsStandardID { get; set; }
        public int GameSystemID { get; set; }
        public int CampaignID { get; set; }
	    public string SkillName { get; set; }
        public int SkillTypeID { get; set; }
        public int SkillHeaderTypeID { get; set; }
        public bool CanBeUsedPassively { get; set; }
        public int? HeaderAssociation { get; set; }
        public bool SkillCostFixed { get; set; }
        public double SkillCPCost { get; set; }
        public string SkillShortDescription { get; set; }
        public string SkillLongDescription { get; set; }
	    public int XrefNumber { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public override string ToString()
        {
            return "Don't use this. Use something else.";
        }

        public int Load(int CharacterIDToLoad)
        {
            int iNumCharacterRecords = 0;

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            sParam.Add("@CharacterID", CharacterIDToLoad);

            DataSet dsCharacterInfo = new DataSet();
            dsCharacterInfo = cUtilities.LoadDataSet("uspGetCharacterInfo", sParam, "LARPortal", "GeneralUser", lsRoutineName);

            return iNumCharacterRecords;
        }

        public int SaveCharacter(string sUserUpdating)
        {
            return 0;
        }
    }
}


