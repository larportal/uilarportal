using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    public class cCharacterDeath
    {
        public cCharacterDeath()
        {
            CharacterDeathID = -1;
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return "ID: " + CharacterDeathID.ToString() + "   Name: " + DeathDate.ToString();
        }

        public int? CharacterDeathID { get; set; }
        public int CharacterID { get; set; }
        public DateTime? DeathDate { get; set; }
        public bool DeathPermanent { get; set; }
        public string StaffComments { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        /// <summary>
        /// Save a character death record to the database. Use this if you don't already have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        public void Save(int iUserID)
        {
            using (SqlConnection connPortal = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
            {
                connPortal.Open();
                Save(iUserID, connPortal);
            }
        }

        /// <summary>
        /// Save a character death record to the database. Use this if you have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        /// <param name="connPortal">Already OPEN connection to the database.</param>
        public void Save(int iUserID, SqlConnection connPortal)
        {
            if (RecordStatus == RecordStatuses.Delete)
            {
                SqlCommand CmdDelCHCharacterDeath = new SqlCommand("uspDelCHCharacterDeaths", connPortal);
                CmdDelCHCharacterDeath.CommandType = CommandType.StoredProcedure;
                CmdDelCHCharacterDeath.Parameters.AddWithValue("@RecordID", CharacterDeathID);
                CmdDelCHCharacterDeath.Parameters.AddWithValue("@UserID", iUserID);

                CmdDelCHCharacterDeath.ExecuteNonQuery();
            }
            else
            {
                if (CharacterDeathID < 0)
                    CharacterDeathID = -1;

                SqlCommand CmdInsUpdCHCharacterDeath = new SqlCommand("uspInsUpdCHCharacterDeaths", connPortal);
                CmdInsUpdCHCharacterDeath.CommandType = CommandType.StoredProcedure;
                CmdInsUpdCHCharacterDeath.Parameters.AddWithValue("@CharacterDeathID", CharacterDeathID);
                CmdInsUpdCHCharacterDeath.Parameters.AddWithValue("@CharacterID", CharacterID);
                CmdInsUpdCHCharacterDeath.Parameters.AddWithValue("@UserID", iUserID);
                CmdInsUpdCHCharacterDeath.Parameters.AddWithValue("@DeathDate", DeathDate);
                CmdInsUpdCHCharacterDeath.Parameters.AddWithValue("@DeathPermanent", DeathPermanent);
                CmdInsUpdCHCharacterDeath.Parameters.AddWithValue("@StaffComments", StaffComments);
                CmdInsUpdCHCharacterDeath.Parameters.AddWithValue("@Comments", Comments);

                CmdInsUpdCHCharacterDeath.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Load a character death record. Make sure to set CharacterDeathID to the record to load.  Use this if you don't have a connection open.
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
        /// Load a character death record. Make sure to set CharacterDeathID to the record to load.  Use this if you have a connection open.
        /// </summary>
        /// <param name="connPortal">SQL Connection to the portal.</param>
        public void Load(SqlConnection connPortal)
        {
            if (CharacterDeathID > 0)
            {
                SqlCommand CmdGetDeathRecord = new SqlCommand("select * from CHCharacterDeaths where CharacterDeathID = @CharacterDeathID", connPortal);
                CmdGetDeathRecord.Parameters.AddWithValue("@CharacterActorID", CharacterDeathID);

                SqlDataAdapter SDAGetDeathRecord = new SqlDataAdapter(CmdGetDeathRecord);
                DataTable dtDeathRecord = new DataTable();

                SDAGetDeathRecord.Fill(dtDeathRecord);

                foreach (DataRow dRow in dtDeathRecord.Rows)
                {
                    StaffComments = dRow["StaffComments"].ToString();
                    Comments = dRow["Comments"].ToString();

                    int iTemp;
                    if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                        CharacterID = iTemp;

                    DateTime dtTemp;
                    if (DateTime.TryParse(dRow["DeathDate"].ToString(), out dtTemp))
                        DeathDate = dtTemp;

                    bool bTemp;
                    if (bool.TryParse(dRow["DeathPermanent"].ToString(), out bTemp))
                        DeathPermanent = bTemp;
                }
            }
        }
    }
}