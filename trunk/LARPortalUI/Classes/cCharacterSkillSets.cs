using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    public class cCharacterSkillSet
    {
        public cCharacterSkillSet()
        {
            CharacterSkillSetID = -1;
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return "ID: " + CharacterSkillSetID.ToString() +
                "   UserID: " + SkillSetName.ToString();
        }

        public int CharacterSkillSetID { get; set; }
        public int CharacterID { get; set; }
        public string SkillSetName { get; set; }
        public int CharacterSkillSetStatusID { get; set; }
        public int CharacterSkillSetTypeID { get; set; }
        public string PlayerComments { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        /// <summary>
        /// Save an character skill set record to the database. Use this if you don't already have a connection open.
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
        /// Save an character skill set record to the database. Use this if you have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        /// <param name="connPortal">Already OPEN connection to the database.</param>
        public void Save(string sUserUpdating, SqlConnection connPortal)
        {
            if (RecordStatus == RecordStatuses.Delete)
            {
                SqlCommand CmdDelCHCharacterActors = new SqlCommand("uspDelCHCharacterSkillSets", connPortal);
                CmdDelCHCharacterActors.CommandType = CommandType.StoredProcedure;
                CmdDelCHCharacterActors.Parameters.AddWithValue("@RecordID", CharacterSkillSetID);
                CmdDelCHCharacterActors.Parameters.AddWithValue("@UserID", sUserUpdating);

                CmdDelCHCharacterActors.ExecuteNonQuery();
            }
            else
            {
                SqlCommand CmdInsUpdCHCharacterActors = new SqlCommand("uspInsUpdCHCharacterSkillSets", connPortal);
                CmdInsUpdCHCharacterActors.CommandType = CommandType.StoredProcedure;

                CmdInsUpdCHCharacterActors.Parameters.AddWithValue("@CharacterSkillSetID", CharacterSkillSetID);
                CmdInsUpdCHCharacterActors.Parameters.AddWithValue("@CharacterID", CharacterID);
                CmdInsUpdCHCharacterActors.Parameters.AddWithValue("@SkillSetName", SkillSetName);
                CmdInsUpdCHCharacterActors.Parameters.AddWithValue("@CharacterSkillSetStatusID", CharacterSkillSetStatusID);
                CmdInsUpdCHCharacterActors.Parameters.AddWithValue("@CharacterSkillSetTypeID", CharacterSkillSetTypeID);
                CmdInsUpdCHCharacterActors.Parameters.AddWithValue("@PlayerComments", PlayerComments);
                CmdInsUpdCHCharacterActors.Parameters.AddWithValue("@Comments", Comments);

                CmdInsUpdCHCharacterActors.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Load a character skill set record. Make sure to set CharacterSkillSetID to the record to load. Use this when you have a connection.
        /// </summary>
        /// <param name="connPortal">SQL Connection to the portal.</param>
        public void Load(SqlConnection connPortal)
        {
            if (CharacterSkillSetID > 0)
            {
                SqlCommand CmdGetActorRecord = new SqlCommand("select * from CHCharacterSkillSets where CharacterSkillSetID = @CharacterSkillSetID", connPortal);
                CmdGetActorRecord.Parameters.AddWithValue("@CharacterSkillSetID", CharacterSkillSetID);

                SqlDataAdapter SDAGetActorRecord = new SqlDataAdapter(CmdGetActorRecord);
                DataTable dtActorRecord = new DataTable();

                SDAGetActorRecord.Fill(dtActorRecord);

                foreach (DataRow dRow in dtActorRecord.Rows)
                {
                    int iTemp;

                    if (int.TryParse(dRow["CharacterSkillSetID"].ToString(), out iTemp))
                        CharacterSkillSetID = iTemp;

                    if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                        CharacterID = iTemp;

                    if (int.TryParse(dRow["CharacterSkillSetStatusID"].ToString(), out iTemp))
                        CharacterSkillSetStatusID = iTemp;

                    if (int.TryParse(dRow["CharacterSkillSetTypeID"].ToString(), out iTemp))
                        CharacterSkillSetTypeID = iTemp;

                    PlayerComments = dRow["PlayerComments"].ToString();
                    Comments = dRow["Comments"].ToString();
                    SkillSetName = dRow["SkillSetName"].ToString();
                }
            }
        }

        /// <summary>
        /// Load a character skill set record. Make sure to set CharacterSkillSetID to the record to load. Use this when you don't have a connection.
        /// </summary>
        public void Load()
        {
            using (SqlConnection connPortal = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
            {
                connPortal.Open();
                Load(connPortal);
            }
        }
    }
}