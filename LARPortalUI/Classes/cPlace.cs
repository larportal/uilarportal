using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    public class cPlace
    {
        public cPlace()
        {
            CampaignPlaceID = -1;
            RecordStatus = RecordStatuses.Active;
            PlaceName = "";
            Locale = "";
            RulebookDescription = "";
            StaffComments = "";
            Comments = "";
        }

        public override string ToString()
        {
            return "ID: " + CampaignPlaceID.ToString() + "   Name: " + PlaceName;
        }

        public int CampaignPlaceID { get; set; }
        public int PlaceID { get; set; }
        public int PlaceTypeID { get; set; }
        public string PlaceName { get; set; }
        public int LocaleID { get; set; }
        public string Locale { get; set; }
        public string RulebookDescription { get; set; }
        public int PlotLeadPerson { get; set; }
        public string StaffComments { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        /// <summary>
        /// Save a place record to the database. Use this if you have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        //public void Save(int iUserID)
        //{
        //    if (RecordStatus == RecordStatuses.Delete)
        //    {
        //        if (CampaignPlaceID != -1)
        //        {
        //            SortedList sParam = new SortedList();
        //            sParam.Add("@RecordID", CharacterPlaceID);
        //            sParam.Add("@UserID", iUserID);
        //            cUtilities.PerformNonQuery("uspDelCH", sParam, "LARPortal", iUserID.ToString());
        //        }
        //    }
        //    else
        //    {
        //        SortedList sParam = new SortedList();
        //        sParam.Add("@CharacterPlaceID", CharacterPlaceID);
        //        sParam.Add("@CharacterID", CharacterID);
        //        sParam.Add("@PlaceID", PlaceID);
        //        sParam.Add("@PlaceName", PlaceName.ToString());
        //        sParam.Add("@LocatedInPlaceID", LocaleID);
        //        sParam.Add("@StaffComments", StaffComments.ToString());
        //        sParam.Add("@PlayerComments", Comments.ToString());
        //        sParam.Add("@UserID", -1);
        //        cUtilities.PerformNonQuery("uspInsUpdCHCharacterPlaces", sParam, "LARPortal", iUserID.ToString());
        //    }
        //}

        /// <summary>
        /// Load a campaign place record. Make sure to set CampaignPlaceID to the record to load.  Use this if you don't have a connection open.
        /// </summary>
        //public void Load()
        //{
        //    using (SqlConnection connPortal = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
        //    {
        //        connPortal.Open();
        //        Load(connPortal);
        //    }
        //}

        /// <summary>
        /// Load a campaign place record. Make sure to set CampaignPlaceID to the record to load.  Use this if you have a connection open.
        /// </summary>
        /// <param name="connPortal">SQL Connection to the portal.</param>
        //public void Load(SqlConnection connPortal)
        //{
        //    if (CharacterPlaceID > 0)
        //    {
        //        SqlCommand CmdGetCampaignPlaceRecord = new SqlCommand("select * from CHCampaignPlaces where CampaignPlacesID = @CampaignPlaceID", connPortal);
        //        CmdGetCampaignPlaceRecord.Parameters.AddWithValue("@CampaignPlaceID", CharacterPlaceID);

        //        SqlDataAdapter SDAGetCampaignPlaceRecord = new SqlDataAdapter(CmdGetCampaignPlaceRecord);
        //        DataTable dtCampaignPlaceRecord = new DataTable();

        //        SDAGetCampaignPlaceRecord.Fill(dtCampaignPlaceRecord);

        //        foreach (DataRow dRow in dtCampaignPlaceRecord.Rows)
        //        {
        //            Comments = dRow["Comments"].ToString();
        //            StaffComments = dRow["StaffComments"].ToString();
        //            PlaceName = dRow["PlaceName"].ToString();
        //            Locale = dRow["Locale"].ToString();
        //            RulebookDescription = dRow["RulebookDescription"].ToString();

        //            int iTemp;
        //            //if (int.TryParse(dRow["PlaceTypeID"].ToString(), out iTemp))
        //            //    PlaceTypeID = iTemp;

        //            if (int.TryParse(dRow["LocaleID"].ToString(), out iTemp))
        //                LocaleID = iTemp;

        //            if (int.TryParse(dRow["PlotLeadPerson"].ToString(), out iTemp))
        //                PlotLeadPerson = iTemp;

        //            RecordStatus = RecordStatuses.Active;
        //        }
        //    }
        //}
    }
}