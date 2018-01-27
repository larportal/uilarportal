using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    public class cCharacterPlace
    {
        public int CharacterPlaceID { get; set; }
        public int CharacterID { get; set; }
        public int? CampaignPlaceID { get; set; }
        public int PlaceTypeID { get; set; }
        public string PlaceName { get; set; }
        public int LocaleID { get; set; }
        public string LocaleName { get; set; }
        public string StaffComments { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        public cCharacterPlace()
        {
            CharacterPlaceID = -1;
            CharacterID = -1;
            CampaignPlaceID = null;
            PlaceTypeID = -1;
            PlaceName = "";
            LocaleID = -1;
            LocaleName = "";
            StaffComments = "";
            Comments = "";
            RecordStatus = RecordStatuses.Active;
        }

        public override string ToString()
        {
            return "ID: " + CharacterPlaceID.ToString() + "   Name: " + PlaceName + " - " + CampaignPlaceID.ToString();
        }


        /// <summary>
        /// Save a place record to the database. Use this if you have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        public void Save(int iUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (RecordStatus == RecordStatuses.Delete)
            {
                if (CharacterPlaceID != -1)
                {
                    SortedList sParam = new SortedList();
                    sParam.Add("@RecordID", CharacterPlaceID);
                    sParam.Add("@UserID", iUserID);
                    cUtilities.PerformNonQuery("uspDelCHCharacterPlaces", sParam, "LARPortal", iUserID.ToString());
                }
            }
            else
            {
                SortedList sParam = new SortedList();
                sParam.Add("@CharacterPlaceID", CharacterPlaceID);
                sParam.Add("@CharacterID", CharacterID);
                if (CampaignPlaceID.HasValue)
                    sParam.Add("@PlaceID", CampaignPlaceID.Value);
                else
                    sParam.Add("@ClearCampaignPlaceID", 1);
                sParam.Add("@PlaceName", PlaceName.ToString());
                sParam.Add("@LocatedInPlaceID", LocaleID);
                sParam.Add("@StaffComments", StaffComments.ToString());
                sParam.Add("@PlayerComments", Comments.ToString());
                sParam.Add("@UserID", iUserID);
                cUtilities.PerformNonQuery("uspInsUpdCHCharacterPlaces", sParam, "LARPortal", iUserID.ToString());
            }
        }

        /// <summary>
        /// Load a character place record. Make sure to set CampaignPlaceID to the record to load.
        /// </summary>
        public void Load(string UserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (CharacterPlaceID > 0)
            {
                SortedList sParams = new SortedList();
                sParams.Add("@CharacterPlaceID", CharacterPlaceID);

                DataTable dtPlaces = cUtilities.LoadDataTable("uspGetCharacterPlaces", sParams, "LARPortal", UserName, lsRoutineName);

                foreach (DataRow dRow in dtPlaces.Rows)
                {
                    PlaceName = dRow["PlaceName"].ToString();
                    Comments = dRow["PlayerComments"].ToString();
                    LocaleName = dRow["LocaleName"].ToString();

                    int iTemp;

                    if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                        CharacterID = iTemp;

                    CampaignPlaceID = null;
                    if (int.TryParse(dRow["CampaignPlaceID"].ToString(), out iTemp))
                        if (iTemp > 0)
                            CampaignPlaceID = iTemp;

                    if (int.TryParse(dRow["LocaleID"].ToString(), out iTemp))
                        LocaleID = iTemp;

                    RecordStatus = RecordStatuses.Active;
                }
            }
        }

        /// <summary>
        /// Load a character place record. Make sure to set CampaignPlaceID to the record to load.  Use this if you have a connection open.
        /// </summary>
        /// <param name="connPortal">SQL Connection to the portal.</param>
        //public void Load(SqlConnection connPortal)
        //{
        //    if (CampaignPlaceID > 0)
        //    {
        //        SqlCommand CmdGetCampaignPlaceRecord = new SqlCommand("select * from CHCampaignPlaces where CampaignPlacesID = @CampaignPlaceID", connPortal);
        //        CmdGetCampaignPlaceRecord.Parameters.AddWithValue("@CampaignPlaceID", CampaignPlaceID);

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