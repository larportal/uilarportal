using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    public class cCharacterType
    {
        public cCharacterType()
        {
            CharacterTypeID = -1;
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return "TypeID: " + CharacterTypeID.ToString() + " " + Description + " " + Comments + " " + RecordStatus;
        }
        public int CharacterTypeID { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        /// <summary>
        /// Save a character type record to the database. Use this if you don't already have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        public void Save(string sUserUpdating)
        {
            using (SqlConnection connPortal = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
            {
                connPortal.Open();
                Save(sUserUpdating, connPortal);
            }
        }

        /// <summary>
        /// Save a character type record to the database. Use this if you have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        /// <param name="connPortal">Already OPEN connection to the database.</param>
        public void Save(string sUserUpdating, SqlConnection connPortal)
        {
            if (RecordStatus == RecordStatuses.Delete)
            {
                SqlCommand CmdDelCHCharacterType = new SqlCommand("uspDelCHCharacterType", connPortal);
                CmdDelCHCharacterType.CommandType = CommandType.StoredProcedure;
                CmdDelCHCharacterType.Parameters.AddWithValue("@RecordID", CharacterTypeID);
                CmdDelCHCharacterType.Parameters.AddWithValue("@UserID", sUserUpdating);

                CmdDelCHCharacterType.ExecuteNonQuery();
            }
            else
            {
                SqlCommand CmdInsUpdCHCharacterType = new SqlCommand("uspInsUpdCHCharacterType", connPortal);
                CmdInsUpdCHCharacterType.CommandType = CommandType.StoredProcedure;
                CmdInsUpdCHCharacterType.Parameters.AddWithValue("@CharacterTypeID", CharacterTypeID);
                CmdInsUpdCHCharacterType.Parameters.AddWithValue("@Description", Description);
                CmdInsUpdCHCharacterType.Parameters.AddWithValue("@Comments", Comments);

                CmdInsUpdCHCharacterType.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Load a character type record. Make sure to set CharacterTypeID to the record to load.  Use this if you don't have a connection open.
        /// </summary>
        public void Load()
        {
            using (SqlConnection connPortal = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
            {
                connPortal.Open();
                Load(connPortal);
            }
        }

        /// <summary>
        /// Load a character type record. Make sure to set CharacterTypeID to the record to load.  Use this if you have a connection open.
        /// </summary>
        /// <param name="connPortal">SQL Connection to the portal.</param>
        public void Load(SqlConnection connPortal)
        {
            if (CharacterTypeID > 0)
            {
                SqlCommand CmdGetCampaignPlaceRecord = new SqlCommand("select * from CHCharacterType where CharacterTypeID = @CharacterTypeID", connPortal);
                CmdGetCampaignPlaceRecord.Parameters.AddWithValue("@v", CharacterTypeID);

                SqlDataAdapter SDAGetCampaignPlaceRecord = new SqlDataAdapter(CmdGetCampaignPlaceRecord);
                DataTable dtCampaignPlaceRecord = new DataTable();

                SDAGetCampaignPlaceRecord.Fill(dtCampaignPlaceRecord);

                foreach (DataRow dRow in dtCampaignPlaceRecord.Rows)
                {
                    Description = dRow["Description"].ToString();
                    Comments = dRow["Comments"].ToString();
                    RecordStatus = RecordStatuses.Active;
                }
            }
        }
    }
}