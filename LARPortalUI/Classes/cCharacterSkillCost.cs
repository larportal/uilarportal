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
            CharacterSkillID = 0;
            CampaignSkillPoolID = 0;
            CPCostPaid = 0;
            WhenPurchased = DateTime.MinValue;
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return "ID: " + CharacterSkillCostID.ToString() + "   Skill Node ID: " + CharacterSkillID.ToString() + "   Campaign Skill Pool ID: " +
                CampaignSkillPoolID.ToString() + "   CPCostPaid: " + CPCostPaid.ToString() + "   WhenPurchased: " + WhenPurchased.ToString() +
                "   Pool Desc: " + PoolDescription + "   Display Color: " + DisplayColor;
        }

        public int CharacterSkillCostID { get; set; }
        public int CharacterSkillID { get; set; }
        public int CampaignSkillPoolID { get; set; }
        public double CPCostPaid { get; set; }
        public DateTime WhenPurchased { get; set; }
        public string PoolDescription { get; set; }         // More for debugging.
        public string DisplayColor { get; set; }
        public RecordStatuses RecordStatus { get; set; }

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
            //SortedList sParam = new SortedList();

            //sParam.Add("@CharacterSkillID", CharacterSkillID);
            //sParam.Add("@CharacterSkillSetID", CharacterSkillSetID);
            //sParam.Add("@CampaignSkillNodeID", CampaignSkillNodeID);
            //sParam.Add("@DisplayOnCard", 1);
            //sParam.Add("@CPCostPaid", CPCostPaid);
            //sParam.Add("@CardDisplayDescription", CardDisplayDescription);
            //sParam.Add("@CardDisplayIncant", CardDisplayIncant);
            //if (PlayerDescription != null)
            //    sParam.Add("@PlayerDescription", PlayerDescription);
            //if (PlayerIncant != null)
            //    sParam.Add("@PlayerIncant", PlayerIncant);
            //if (SkillCardDescription != null)
            //    sParam.Add("@CardDescription", SkillCardDescription);
            //if (SkillCardIncant != null)
            //    sParam.Add("@CardIncant", SkillCardIncant);
            //sParam.Add("@SuppressCampaignDescription", SuppressCampaignDescription);
            //sParam.Add("@SuppressCampaignIncant", SuppressCampaignIncant);
            //sParam.Add("@AddInfoValue", AddInfoValue);
            //sParam.Add("@UserID", iUserID);

            //cUtilities.PerformNonQuery("uspInsUpdCHCharacterSkills", sParam, "LARPortal", sUserName);
        }













    }
}