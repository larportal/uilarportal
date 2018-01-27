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
    public class cUserCampaigns
    {
        private int _CampaignID;
        public int CampaignID
        {
            get { return _CampaignID; }
            set { _CampaignID = value; }
        }
        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public List<cUserCampaign> lsUserCampaigns = new List<cUserCampaign>();
        public int CountOfUserCampaigns
        { get; set; }

        /// <summary>
        /// This will load the details of a particular users' campaigns
        /// Must pass a UserID
        /// </summary>
        public void Load(int UserID)
        {
            string stStoredProc = "uspGetMyCampaigns";
            string stCallingMethod = "cUserCampaigns.Load";
            int iTemp;
            CountOfUserCampaigns = 0;
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CampaignID", 0); // Returning all campaigns
            DataSet dsUserCampaigns = new DataSet();
            dsUserCampaigns = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow dRow in dsUserCampaigns.Tables[0].Rows)
            {
                if (int.TryParse(dRow["CampaignID"].ToString(), out iTemp))
                {
                    cUserCampaign UserCampaign = new cUserCampaign();
                    UserCampaign.Load(UserID, iTemp);
                    lsUserCampaigns.Add(UserCampaign);
                    CountOfUserCampaigns++;
                }
            }
        }
    }
}