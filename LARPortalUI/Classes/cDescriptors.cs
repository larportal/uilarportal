using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    [Serializable]
    public class cDescriptor
    {
        public cDescriptor()
        {
            CharacterSkillSetID = -1;
            SkillSetName = "";
            PlayerComments = "";
            CharacterAttributesBasicID = -1;
            CampaignAttributeStandardID = -1;
            CampaignAttributeDescriptorID = -1;
            CharacterDescriptor = "";
            DescriptorValue = "";
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return "Not Yet Defined.";
        }

        public int CharacterSkillSetID { get; set; }
        public string SkillSetName { get; set; }
        public string PlayerComments { get; set; }
        public int CharacterAttributesBasicID { get; set; }
        public int CampaignAttributeStandardID { get; set; }
        public int CampaignAttributeDescriptorID { get; set; }
        public string CharacterDescriptor { get; set; }
        public string DescriptorValue { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public void Delete(string sUserUpdating, int iUserID)
        {
            SortedList sParam = new SortedList();
            //sParam.Add("@CharacterSkillSetID", CharacterSkillSetID);
            //sParam.Add("@CampaignAttributeStandardID", CampaignAttributeStandardID);
            sParam.Add("@RecordID", CharacterAttributesBasicID);
            //sParam.Add("@CampaignAttributeDescriptorID", CampaignAttributeDescriptorID);
            sParam.Add("@UserID", iUserID);

            Classes.cUtilities.PerformNonQuery("uspDelCHCharacterAttributesStandard", sParam, "LARPortal", "");
        }

        public void Save(string sUserUpdating, int iUserID)
        {
            SortedList sParam = new SortedList();
            sParam.Add("@CharacterSkillSetID", CharacterSkillSetID);
            sParam.Add("@CampaignAttributeStandardID", CampaignAttributeStandardID);
            sParam.Add("@CharacterAttributesBasicID", CharacterAttributesBasicID);
            sParam.Add("@CampaignAttributeDescriptorID", CampaignAttributeDescriptorID);
            sParam.Add("@UserID", iUserID);

            Classes.cUtilities.PerformNonQuery("uspInsUpdCHCharacterAttributesStandard", sParam, "LARPortal", "");
        }

        ///// <summary>
        ///// Load a actor record. Make sure to set CharacterActorID to the record to load. Use this when you have a connection.
        ///// </summary>
        ///// <param name="connPortal">SQL Connection to the portal.</param>
        //public void Load(SqlConnection connPortal)
        //{
        //    if (CharacterActorID > 0)
        //    {
        //        SqlCommand CmdGetActorRecord = new SqlCommand("select * from CHCharacterActors where CharacterActorID = @CharacterActorID", connPortal);
        //        CmdGetActorRecord.Parameters.AddWithValue("@CharacterActorID", CharacterActorID);

        //        SqlDataAdapter SDAGetActorRecord = new SqlDataAdapter(CmdGetActorRecord);
        //        DataTable dtActorRecord = new DataTable();

        //        SDAGetActorRecord.Fill(dtActorRecord);

        //        foreach (DataRow dRow in dtActorRecord.Rows)
        //        {
        //            StaffComments = dRow["StaffComments"].ToString();
        //            Comments = dRow["Comments"].ToString();

        //            int iTemp;
        //            if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
        //                CharacterID = iTemp;

        //            DateTime dtTemp;
        //            if (DateTime.TryParse(dRow["StartDate"].ToString(), out dtTemp))
        //                StartDate = dtTemp;

        //            if (DateTime.TryParse(dRow["EndDate"].ToString(), out dtTemp))
        //                EndDate = dtTemp;
        //        }
        //    }
        //}

        /// <summary>
        /// Load a actor record. Make sure to set CharacterActorID to the record to load. Use this when you don't have a connection.
        /// </summary>
        public void Load()
        {
            //using (SqlConnection connPortal = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
            //{
            //    connPortal.Open();
            //    Load(connPortal);
            //}
        }
    }
}