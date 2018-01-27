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
    public class cGameSystems
    {
        private int _GameSystemID;
        public int GameSystemID
        {
            get { return _GameSystemID; }
            set { _GameSystemID = value; }
        }
        public string GameSystemName { get; set; }
        public List<cGameSystem> lsGameSystems = new List<cGameSystem>();

        /// <summary>
        /// This will load a table of game systems
        /// GameSystemID optional to return a specific users' roles, else 0 to return all game systems
        /// </summary>
        public DataTable LoadGameSystemsByName(int UserID)
        {
            string stStoredProc = "uspGetGameSystemsByName";
            string stCallingMethod = "cGameSystems.LoadGameSystemsByName";
            string strUsername = UserID.ToString();
            SortedList slParameters = new SortedList();
            DataTable dtGameSystems = new DataTable();
            dtGameSystems = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            return dtGameSystems;
        }

        public DataTable CampaignsByGameSystem(int GameSystemID, string EndDate, int UserID)
        {
            string stStoredProc = "uspGetCampaignsByGameSystem";
            string stCallingMethod = "cGameSystems.CampaignsByGameSystem";
            string strUsername = UserID.ToString();
            SortedList slParameters = new SortedList();
            DataTable dtCampaigns = new DataTable();
            if (String.IsNullOrEmpty(EndDate))
            {
                EndDate = "1960-01-01";  //Using an arbitrary old date that is older than any end date in the system
            }
            slParameters.Add("@GameSystemID", GameSystemID);
            slParameters.Add("@EndDate", EndDate);
            dtCampaigns = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", strUsername, stCallingMethod);
            return dtCampaigns;
        }

        public DataTable GameSystemURLAndDesription(int GameSystemID)
        {
            string stStoredProc = "uspGetGameSystemsURLandDescription";
            string stCallingMethod = "cGameSystems.GameSystemURLAndDesription";
            SortedList slParameters = new SortedList();
            slParameters.Add("@GameSystemID", GameSystemID);
            DataTable dtGameSystem = new DataTable();
            dtGameSystem = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", "0", stCallingMethod);
            return dtGameSystem;
        }
    }
}