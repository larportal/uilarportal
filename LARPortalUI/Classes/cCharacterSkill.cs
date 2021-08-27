using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

// JBradhaw 11/3/2019  Added addition information fields and saving to database.
namespace LarpPortal.Classes
{
	public class cCharacterSkillAddValues
	{
		public string Value { get; set; }
		public int SortOrder { get; set; }
	}

	public enum enumAddInfoType
	{
		None = 0,
		FreeText = 1,
		DropDown = 2
	};

    public class cCharacterSkill
    {
        public cCharacterSkill()
        {
            CharacterSkillID = -1;
            RecordStatus = RecordStatuses.Active;
            SuppressCampaignDescription = false;
            SuppressCampaignIncant = false;
            CardDisplayDescription = true;
            CardDisplayIncant = true;
			AddInfoValue = "";
			AddAvailableValues = new List<cCharacterSkillAddValues>();
            SkillCost = new List<cCharacterSkillCost>();
			AddInfoType = enumAddInfoType.None;
        }

        public override string ToString()
        {
            return "ID: " + CharacterSkillID.ToString() + "   UserID: " + SkillName.ToString();
        }

        public string CostString()
        {
            string Cost = "";
            cCharacterSkillCost[] cDefaultArray = SkillCost.Where(x => x.isDefaultPool).ToArray();
            foreach (cCharacterSkillCost cDefault in cDefaultArray)
            {
                Cost = cDefault.CPCostPaid.ToString();
                if (SkillCost.Count > 1)
                    Cost = cDefault.PoolDescription + ":" + Cost;
            }

            cCharacterSkillCost[] cOtherArray = SkillCost.Where(x => !x.isDefaultPool).ToArray();
            foreach (cCharacterSkillCost cOther in cOtherArray)
            {
                if (Cost.Length > 0)
                    Cost += "; ";
                Cost += cOther.PoolDescription + ":" + cOther.CPCostPaid;
            }
            return Cost;
        }
        public int CharacterSkillID { get; set; }
        public int CharacterSkillSetID { get; set; }
        public int CharacterID { get; set; }
        public string SkillSetName { get; set; }
        public int CharacterSkillSetStatusID { get; set; }
        public int StatusType { get; set; }
        public string StatusName { get; set; }
        public string SkillSetTypeDescription { get; set; }
 //       public double CPCostPaid { get; set; }
        public string SkillName { get; set; }
        public int CampaignSkillNodeID { get; set; }
        public int SkillTypeID { get; set; }
        public bool CanBeUsedPassively { get; set; }
        public double SkillCPCost { get; set; }
        public string SkillShortDescription { get; set; }
        public string PlayerDescription { get; set; }
        public string PlayerIncant { get; set; }
        public string SkillCardDescription { get; set; }
        public string SkillCardIncant { get; set; }
        public bool SuppressCampaignDescription { get; set; }
        public bool SuppressCampaignIncant { get; set; }
        public bool CardDisplayDescription { get; set; }
        public bool CardDisplayIncant { get; set; }
        public string CampaignSkillsStandardComments { get; set; }
        public bool AllowPassiveUse { get; set; }
        public bool OpenToAll { get; set; }
        public string SkillTypeComments { get; set; }
        public int DisplayOrder { get; set; }
		public enumAddInfoType AddInfoType { get; set; }
		public string AddInfoValue { get; set; }
		public List<cCharacterSkillAddValues> AddAvailableValues { get; set; }
        public List<cCharacterSkillCost> SkillCost { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        ///// <summary>
        ///// Delete the picture. Set the ID before doing this.
        ///// </summary>
        ///// <param name="sUserID">User ID of person deleting it.</param>
        public void Delete(string sUserName, int iUserID)
        {
            SortedList sParam = new SortedList();

            sParam.Add("@CharacterSkillID", CharacterSkillID);
            sParam.Add("@UserID", iUserID);
            DataTable dtDelChar = new DataTable();
            dtDelChar = cUtilities.LoadDataTable("uspDelCHCharacterSkills", sParam, "LARPortal", sUserName, "cCharacterSkills.Delete");
        }

        public void Save(string sUserName, int iUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();

            sParam.Add("@CharacterSkillID", CharacterSkillID);
            sParam.Add("@CharacterSkillSetID", CharacterSkillSetID);
            sParam.Add("@CampaignSkillNodeID", CampaignSkillNodeID);
            sParam.Add("@DisplayOnCard", 1);
//            sParam.Add("@CPCostPaid", CPCostPaid);
            sParam.Add("@CardDisplayDescription", CardDisplayDescription);
            sParam.Add("@CardDisplayIncant", CardDisplayIncant);
            if (PlayerDescription != null)
                sParam.Add("@PlayerDescription", PlayerDescription);
            if (PlayerIncant != null)
                sParam.Add("@PlayerIncant", PlayerIncant);
            if (SkillCardDescription != null)
                sParam.Add("@CardDescription", SkillCardDescription);
            if (SkillCardIncant != null)
                sParam.Add("@CardIncant", SkillCardIncant);
            sParam.Add("@SuppressCampaignDescription", SuppressCampaignDescription);
            sParam.Add("@SuppressCampaignIncant", SuppressCampaignIncant);
			sParam.Add("@AddInfoValue", AddInfoValue);
            sParam.Add("@UserID", iUserID);

            DataTable dSkills = new DataTable();
            dSkills = cUtilities.LoadDataTable("uspInsUpdCHCharacterSkills", sParam, "LARPortal", sUserName, lsRoutineName);

            //string t = "No";
            //if (SkillCost.Count > 1)
            //    t = "Yes";

            if (dSkills.Rows.Count > 0)
            {
                int itemp;
                if (int.TryParse(dSkills.Rows[0]["CharacterSkillID"].ToString(), out itemp))
                {
                    CharacterSkillID = itemp;
                    foreach (cCharacterSkillCost cCost in SkillCost)
                    {
                        //SortedList sParamCost = new SortedList();
                        //sParamCost.Add(
                        //sParamCost.Add("@CharacterSkillSetID", CharacterSkillSetID);
                        //sParamCost.Add("@CampaignSkillNodeID", CampaignSkillNodeID);
                        //sParamCost.Add("@CampaignSkillPoolID", cCost.CampaignSkillPoolID);
                        //sParamCost.Add("@CPCostPaid", cCost.CPCostPaid);
                        //sParamCost.Add("@WhenPurchased", cCost.WhenPurchased);
                        cCost.CharacterSkillID = itemp;
                        cCost.Save(sUserName, iUserID);
                    }
                }
            }
        }
    }
}