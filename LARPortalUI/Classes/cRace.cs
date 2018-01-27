using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    public class cRace
    {
        public cRace()
        {
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int CampaignRaceID { get; set; }
        public int GameSystemID { get; set; }
        public int CampaignID { get; set; }
        public string RaceName { get; set; }
        public string SubRace { get; set; }
        public string FullRaceName
        {
            get
            {
                if ((SubRace != null) && (SubRace.Length > 0))
                    return RaceName + " - " + SubRace;
                else
                    return RaceName;
            }
        }
        public string Description { get; set; }
        public string MakeupRequirements { get; set; }
        public string Photo { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        /// <summary>
        /// Save a character death record to the database. Use this if you don't already have a connection open.
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
        /// Save a campaign race record to the database. Use this if you have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        /// <param name="connPortal">Already OPEN connection to the database.</param>
        public void Save(string sUserUpdating, SqlConnection connPortal)
        {
            if (RecordStatus == RecordStatuses.Delete)
            {
                SqlCommand CmdDelCHCampaignRaces = new SqlCommand("uspDelCHCampaignRaces", connPortal);
                CmdDelCHCampaignRaces.CommandType = CommandType.StoredProcedure;
                CmdDelCHCampaignRaces.Parameters.AddWithValue("@RecordID", CampaignRaceID);
                CmdDelCHCampaignRaces.Parameters.AddWithValue("@UserID", sUserUpdating);

                CmdDelCHCampaignRaces.ExecuteNonQuery();
            }
            else
            {
                SqlCommand CmdInsUpdCHCharacterRaces = new SqlCommand("uspInsUpdCHCharacterRaces", connPortal);
                CmdInsUpdCHCharacterRaces.CommandType = CommandType.StoredProcedure;
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@CampaignRaceID", CampaignRaceID);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@GameSystemID", GameSystemID);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@UserID", sUserUpdating);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@CampaignID", CampaignID);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@RaceName", RaceName);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@SubRace", SubRace);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@Description", Description);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@MakeupRequirements", MakeupRequirements);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@Photo", Photo);
                CmdInsUpdCHCharacterRaces.Parameters.AddWithValue("@Comments", Comments);

                CmdInsUpdCHCharacterRaces.ExecuteNonQuery();
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
            if (CampaignRaceID > 0)
            {
                SqlCommand CmdGetDeathRecord = new SqlCommand("select * from CHCampaignRaces where CampaignRaceID = @CampaignRaceID", connPortal);
                CmdGetDeathRecord.Parameters.AddWithValue("@CampaignRaceID", CampaignRaceID);

                SqlDataAdapter SDAGetDeathRecord = new SqlDataAdapter(CmdGetDeathRecord);
                DataTable dtDeathRecord = new DataTable();

                SDAGetDeathRecord.Fill(dtDeathRecord);

                foreach (DataRow dRow in dtDeathRecord.Rows)
                {
                    Comments = dRow["Comments"].ToString();
                    RaceName = dRow["RaceName"].ToString();
                    SubRace = dRow["SubRace"].ToString();
                    Description = dRow["Description"].ToString();
                    MakeupRequirements = dRow["MakeupRequirements"].ToString();
                    Photo = dRow["Photo"].ToString();

                    int iTemp;
                    if (int.TryParse(dRow["GameSystemID"].ToString(), out iTemp))
                        GameSystemID = iTemp;

                    if (int.TryParse(dRow["CampaignID"].ToString(), out iTemp))
                        CampaignID = iTemp;

                    RecordStatus = RecordStatuses.Active;
                }
            }
        }
    }
}