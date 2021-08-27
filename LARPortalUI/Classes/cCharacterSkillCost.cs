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
    public class cCharacterSkillCost
    {
        public cCharacterSkillCost()
        {
            CharacterSkillCostID = 0;
            CharacterSkillNodeID = 0;
            CharacterSkillSetID = 0;
            CampaignSkillPoolID = 0;
            CPCostPaid = 0;
            WhenPurchased = DateTime.MinValue;
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return "ID: " + CharacterSkillCostID.ToString() + "   Skill Node ID: " + CharacterSkillNodeID.ToString() + "   Campaign Skill Pool ID: " +
                CampaignSkillPoolID.ToString() + "   CPCostPaid: " + CPCostPaid.ToString() + "   WhenPurchased: " + WhenPurchased.ToString() +
                "   Pool Desc: " + PoolDescription + "   Display Color: " + DisplayColor;
        }

        public int CharacterSkillID { get; set; }
        public int CharacterSkillCostID { get; set; }
        public int CharacterSkillNodeID { get; set; }
        public int CampaignSkillPoolID { get; set; }
        public int CharacterSkillSetID { get; set; }
        public double CPCostPaid { get; set; }
        public DateTime WhenPurchased { get; set; }
        public string PoolDescription { get; set; }         // More for debugging.
        public string DisplayColor { get; set; }
        public RecordStatuses RecordStatus { get; set; }
        public bool isDefaultPool { get; set; }
        public void Delete(string sUserName, int iUserID)
        {
            //SortedList sParam = new SortedList();

            //sParam.Add("@CharacterSkillID", CharacterSkillID);
            //sParam.Add("@UserID", iUserID);
            //DataTable dtDelChar = new DataTable();
            //dtDelChar = cUtilities.LoadDataTable("uspDelCHCharacterSkills", sParam, "LARPortal", sUserName, "cCharacterSkills.Delete");
        }

        public void Save(string sUserName, int iUserID)
        {
            SortedList sParam = new SortedList();

            sParam.Add("@CharacterSkillCostID", CharacterSkillCostID);
            sParam.Add("@CharacterSkillID", CharacterSkillID);
            //sParam.Add("@CharacterSkillSetID", CharacterSkillSetID);
            //sParam.Add("@CampaignSkillNodeID", CharacterSkillNodeID);
            sParam.Add("@CampaignSkillPoolID", CampaignSkillPoolID);
            sParam.Add("@CPCostPaid", CPCostPaid);
            sParam.Add("@WhenPurchased", WhenPurchased);

            cUtilities.PerformNonQuery("uspInsUpdCHCharacterSkillCost", sParam, "LARPortal", sUserName);
        }













    }
}