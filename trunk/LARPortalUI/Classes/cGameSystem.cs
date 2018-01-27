using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

namespace LarpPortal.Classes
{
    public class cGameSystem
    {
        private int _GameSystemID;
        public int GameSystemID
        {
            get { return _GameSystemID; }
            set { _GameSystemID = value; }
        }
        public string GameSystemName { get; set; }
        public string GameSystemURL { get; set; }
        public string GameSystemWebPageDescription { get; set; }
        public string GameSystemLogo { get; set; }
        public int GameSystemLogoHeight { get; set; }
        public int GameSystemLogoWidth { get; set; }

        /// <summary>
        /// This will load the details of a particular game system
        /// </summary>
        public void Load(int GameSystemIDToLoad, int UserID)
        {
            string stStoredProc = "uspGetGameSystemByID";
            string stCallingMethod = "cGameSystem.Load";
            int iTemp;
            SortedList slParameters = new SortedList();
            slParameters.Add("@GameSystemID", GameSystemIDToLoad);
            DataSet dsGameSystem = new DataSet();
            dsGameSystem = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            dsGameSystem.Tables[0].TableName = "CMGameSystem";

            foreach (DataRow dRow in dsGameSystem.Tables["CMGameSystem"].Rows)
            {
                if (int.TryParse(dRow["GameSystemID"].ToString(), out iTemp))
                    GameSystemID = iTemp;
                if (int.TryParse(dRow["GameSystemLogoWidth"].ToString(), out iTemp))
                    GameSystemLogoWidth = iTemp;
                if (int.TryParse(dRow["GameSystemLogoHeight"].ToString(), out iTemp))
                    GameSystemLogoHeight = iTemp;
                GameSystemName = dRow["GameSystemName"].ToString();
                GameSystemURL = dRow["GameSystemURL"].ToString();
                GameSystemWebPageDescription = dRow["GameSystemWebPageDescription"].ToString();
                GameSystemLogo = dRow["GameSystemLogo"].ToString();
            }

        }

        /// <summary>
        /// Save will handle both insert new record and update old record.
        /// Set GameSystemID = -1 for insert and GameSystemID = record to update for existing record.
        /// </summary>
        public void Save(int UserID)
        {
            // 
            string stStoredProc = "uspInsUpdCMGameSystems";
            //string stCallingMethod = "cGameSystem.Save";
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@GameSystemName", GameSystemName);
            slParameters.Add("@GameSystemURL", GameSystemURL);
            slParameters.Add("@GameSystemWebPageDescription", GameSystemWebPageDescription);
            cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
        }

        public void Delete(int UserID)
        {
            string stStoredProc = "uspDelCMGameSystems";
            SortedList slParameters = new SortedList();
            slParameters.Add("@RecordID", GameSystemID);
            slParameters.Add("@UserID", UserID.ToString());
            cUtilities.PerformNonQuery(stStoredProc, slParameters, "LARPortal", UserID.ToString());
        }
    }
}