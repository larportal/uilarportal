using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    public class cCharacterStatus
    {
        public cCharacterStatus()
        {
            StatusID = -1;
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int StatusID { get; set; }
        public string StatusType { get; set; }
        public string StatusName { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        /// <summary>
        /// Save a status record to the database. Use this if you don't already have a connection open.
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
        /// Save a status record to the database. Use this if you already have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        /// <param name="connPortal">Already OPEN connection to the database.</param>
        public void Save(string sUserUpdating, SqlConnection connPortal)
        {
            if (RecordStatus == RecordStatuses.Delete)
            {
                SqlCommand CmdDelCHCharacterStatus = new SqlCommand("uspDelCHCharacterStatus", connPortal);
                CmdDelCHCharacterStatus.CommandType = CommandType.StoredProcedure;
                CmdDelCHCharacterStatus.Parameters.AddWithValue("@RecordID", StatusID);
                CmdDelCHCharacterStatus.Parameters.AddWithValue("@UserID", sUserUpdating);

                CmdDelCHCharacterStatus.ExecuteNonQuery();
            }
            else
            {
                SqlCommand CmdInsUpdCHCharacterStatus = new SqlCommand("uspInsUpdCHCharacterStatus", connPortal);
                CmdInsUpdCHCharacterStatus.CommandType = CommandType.StoredProcedure;

                CmdInsUpdCHCharacterStatus.Parameters.AddWithValue("@StatusID", StatusID);
                CmdInsUpdCHCharacterStatus.Parameters.AddWithValue("@StatusType", StatusType );
                CmdInsUpdCHCharacterStatus.Parameters.AddWithValue("@StatusName", StatusName);
                CmdInsUpdCHCharacterStatus.Parameters.AddWithValue("@Comments", Comments);

                CmdInsUpdCHCharacterStatus.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Load a character status record. Make sure to set StatusID to the record to load.
        /// </summary>
        /// <param name="connPortal">SQL Connection to the portal.</param>
        public void Load(SqlConnection connPortal)
        {
            if ( StatusID > 0 )
            {
                SqlCommand CmdGetCharacterStatusRecord = new SqlCommand("select * from CHCharacterStatus where StatusID = @StatusID", connPortal);
                CmdGetCharacterStatusRecord.Parameters.AddWithValue("@StatusID", StatusID);

                SqlDataAdapter SDAGetCharacterStatusRecord = new SqlDataAdapter(CmdGetCharacterStatusRecord);
                DataTable dtCharacterStatusRecord = new DataTable();

                SDAGetCharacterStatusRecord.Fill(dtCharacterStatusRecord);

                foreach (DataRow dRow in dtCharacterStatusRecord.Rows)
                {
                    StatusType = dRow["StatusType"].ToString();
                    StatusName = dRow["StatusName"].ToString();
                    Comments = dRow["Comments"].ToString();
                    RecordStatus = RecordStatuses.Active;
                }
            }
        }

        /// <summary>
        /// Load a character status record when you do NOT has an open connection. Make sure to set StatusID to the record to load.
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