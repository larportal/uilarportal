using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LarpPortal.Classes
{
    class cCampaignRaces
    {
        public cCampaignRaces()
        {
            CampaignID = -1;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int CampaignID { get; set; }
        public List<cRace> RaceList = new List<cRace>();

        /// <summary>
        /// Load a character death record. Make sure to set CharacterDeathID to the record to load.  Use this if you have a connection open.
        /// </summary>
        /// <param name="connPortal">SQL Connection to the portal.</param>
        public void Load(string sUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (CampaignID > 0)
            {
                SortedList sParam = new SortedList();
                sParam.Add("@CampaignID", CampaignID.ToString());
                DataTable dtRaceList = new DataTable();

                try
                {
                    dtRaceList = cUtilities.LoadDataTable("uspGetCampaignRaces", sParam, "LARPortal", sUserID, lsRoutineName);
                }
                catch (Exception ex)
                {
                    string t = ex.Message;
                }
                foreach (DataRow dRace in dtRaceList.Rows)
                {
                    int iCampaignRaceID;
                    int iGameSystemID;
                    int iCampaignID;

                    if ((int.TryParse(dRace["CampaignRaceID"].ToString(), out iCampaignRaceID)) &&
                        (int.TryParse(dRace["GameSystemID"].ToString(), out iGameSystemID)) &&
                        (int.TryParse(dRace["CampaignID"].ToString(), out iCampaignID)))
                    {
                        cRace NewRace = new cRace();
                        NewRace.CampaignRaceID = iCampaignRaceID;
                        NewRace.GameSystemID = iGameSystemID;
                        NewRace.CampaignID = iCampaignID;
                        NewRace.RaceName = dRace["RaceName"].ToString();
                        NewRace.SubRace = dRace["SubRace"].ToString();
                        NewRace.Description = dRace["Description"].ToString();
                        NewRace.MakeupRequirements = dRace["MakeupRequirements"].ToString();
                        NewRace.Photo = dRace["Photo"].ToString();
                        NewRace.Comments = dRace["Comments"].ToString();
                        RaceList.Add(NewRace);
                    }
                }
            }
        }
    }
}