using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

using LarpPortal.Classes;

namespace LarpPortal.Classes
{
    [Serializable]
    public class cCharacterItems
    {
        public cCharacterItems()
        {
            RecordStatus = RecordStatuses.Active;
        }

        public cCharacterItems(int iCharacterItemID, int iCharacterID, string sItemDescription, int? iTempPictureID)
        {
            CharacterItemID = iCharacterItemID;
            CharacterID = iCharacterID;
            ItemDescription = sItemDescription;
            ItemPictureID = iTempPictureID;
            RecordStatus = RecordStatuses.Active;
        }
        
        public override string ToString()
        {
            return "ID: " + CharacterItemID.ToString() + "  " + ItemDescription;
        }

        public int CharacterItemID { get; set; }
        public int CharacterID { get; set; }
        public string ItemDescription { get; set; }
        public int? ItemPictureID { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        ///TODO JLB Fix cPicture Save to use cUtilities
        /// <summary>
        /// Save an picture record to the database. Use this if you have a connection open.
        /// </summary>
        /// <param name="sUserUpdating">Name of the user who is saving the record.</param>
        public void Save(string sUserUpdating)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (RecordStatus == RecordStatuses.Delete)
            {
                SortedList sParams = new SortedList();
                sParams.Add("@CharacterItemID", ItemPictureID.ToString());
                cUtilities.LoadDataTable("uspDelCHCharacterItem", sParams, "LARportal", "", lsRoutineName);
            }
            else
            {
                SortedList sParams = new SortedList();
                sParams.Add("@CharacterItemID", ItemPictureID.ToString());
                sParams.Add("@CharacterID", CharacterID.ToString());
                sParams.Add("@ItemDescription", ItemDescription);
                sParams.Add("@ItemPictureID", ItemPictureID.ToString());
                DataTable dtItem = new DataTable();
                dtItem = cUtilities.LoadDataTable("uspInsUpdCHCharacterItems", sParams, "LARportal", "", lsRoutineName);

                foreach (DataRow dRow in dtItem.Rows)
                {
                    int ItemID;
                    if (int.TryParse(dRow["CharacterItemID"].ToString(), out ItemID))
                        CharacterItemID = ItemID;
                }
            }
        }

        /// <summary>
        /// Load a picture record. 
        /// </summary>
        /// <param name="iPictureID">Picture ID to load.</param>
        /// <param name="sUserID">User ID loading picture.</param>
        public void Load(int iCharacterItemID, string sUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (iCharacterItemID > 0)
            {
                SortedList sParam = new SortedList();
                sParam.Add("@CharacterItemID", iCharacterItemID.ToString());

                DataTable dtPicture = new DataTable();
                dtPicture = cUtilities.LoadDataTable("uspGetCHCharacterItems", sParam, "LARPortal", "", lsRoutineName);

                foreach (DataRow dRow in dtPicture.Rows)
                {
                    int iTemp;

                    if (int.TryParse(dRow["CharacterItemID"].ToString(), out iTemp))
                        CharacterItemID = iCharacterItemID;
                    if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                        CharacterID = iTemp;
                    ItemDescription = dRow["ItemDescription"].ToString();
                    if (int.TryParse(dRow["ItemPictureID"].ToString(), out iTemp))
                        ItemPictureID = iTemp;
                    else
                        ItemPictureID = null;
                }
            }
        }

        /// <summary>
        /// Load a picture record. To use this routine make sure to have already set the PictureID
        /// </summary>
        public void Load(string sUserID)
        {
            Load(CharacterItemID, sUserID);
        }

        //public void CreateNewPictureRecord(string sUserID)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

        //    SortedList sParam = new SortedList();
        //    sParam.Add("@CreatedBy", sUserID);

        //    DataTable dtNewPicture = new DataTable();
        //    dtNewPicture = cUtilities.LoadDataTable("uspMDBPicturesNewPicture", sParam, "LARPortal", sUserID, lsRoutineName);

        //    foreach ( DataRow dRow in dtNewPicture.Rows )
        //    {
        //        int.TryParse(dRow[0].ToString(), out PictureID);
        //    }
        //}
    }
}