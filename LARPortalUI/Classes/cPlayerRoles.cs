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
    public class cPlayerRoles
    {
        private int _CampaignPlayerRoleID;
        public int CampaignPlayerRoleID
        {
            get { return _CampaignPlayerRoleID; }
            set { _CampaignPlayerRoleID = value; }
        }
        public string _PlayerRoleString = "";
        public string PlayerRoleString
        {
            get { return _PlayerRoleString; }
            set { _PlayerRoleString = value; }
        }

        public List<cPlayerRole> lsPlayerRoles = new List< cPlayerRole>();
        
        /// <summary>
        /// This will load the details of a particular player role
        /// UserID optional to return a specific users' roles, else 0 to return all users
        /// CampaignPlayerIDToLoad optional to return a specific role in a specific campaign, else 0 for all roles
        /// CampaignIDToLoad optional to return all roles for a player in a specific campaign, else 0 for all campaigns
        /// Mix the parameters to get mixed results
        /// </summary>
        public void Load(int UserID, int CampaignPlayerRoleIDToLoad, int CampaignIDToLoad, DateTime RoleExpirationDate)
        {
            string stStoredProc = "uspGetPlayerRoles";
            string stCallingMethod = "cPlayerRoles.Load";
            int iTemp;
            PlayerRoleString = CampaignIDToLoad.ToString() + ":/";
            SortedList slParameters = new SortedList();
            slParameters.Add("@UserID", UserID);
            slParameters.Add("@CampaignPlayerRoleID", CampaignPlayerRoleIDToLoad);
            slParameters.Add("@CampaignID", CampaignIDToLoad);
            slParameters.Add("@RoleExpirationDate", RoleExpirationDate);
            DataSet dsPlayerRoles = new DataSet();
            dsPlayerRoles = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow dRow in dsPlayerRoles.Tables[0].Rows)
            {
                if (int.TryParse(dRow["CampaignPlayerRoleID"].ToString(), out iTemp))
                {
                    cPlayerRole PlayerRole = new cPlayerRole();
                    PlayerRole.Load(UserID, iTemp, 0);
                    lsPlayerRoles.Add(PlayerRole);
                    PlayerRoleString = PlayerRoleString + PlayerRole.RoleID.ToString() + "/";
                }
            }
        }
    }
}