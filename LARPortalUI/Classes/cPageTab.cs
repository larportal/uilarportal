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
    public class cPageTab
    {
        private int _SecurityRoleTabID;
        public int SecurityRoleTabID
        {
            get { return _SecurityRoleTabID; }
            set { _SecurityRoleTabID = value; }
        }
        public int SecurityRoleID { get; set; }
        public string SecurityRoleName { get; set; }
        public string CallsPageName { get; set; }
        public string TabName { get; set; }
        public string TabAlert { get; set; }
        public int SortOrder { get; set; }
        public int UserID { get; set; }
        public string TabClass { get; set; }
        public string TabIcon { get; set; }

        /// <summary>
        /// This will load the tabs associated with a given security role.
        /// Requires a security roleID
        /// </summary>
        public void Load(int TabID)
        {
            int iTemp;
            string stStoredProc = "uspGetSecurityRoleTabs";
            string stCallingMethod = "cTab.Load";
            SortedList slParameters = new SortedList();
            slParameters.Add("@SecurityRoleTabID", TabID);
            DataSet dsTab = new DataSet();
            dsTab = cUtilities.LoadDataSet(stStoredProc, slParameters, "LARPortal", UserID.ToString(), stCallingMethod);
            dsTab.Tables[0].TableName = "MDBSecurityRoleTabs";
            foreach (DataRow dRow in dsTab.Tables["MDBSecurityRoleTabs"].Rows)
            {
                if (int.TryParse(dRow["SecurityRoleTabID"].ToString(), out iTemp))
                    SecurityRoleTabID = iTemp;
                if (int.TryParse(dRow["SecurityRoleID"].ToString(), out iTemp))
                    SecurityRoleID = iTemp;
                if (int.TryParse(dRow["SortOrder"].ToString(), out iTemp))
                    SortOrder = iTemp;
                SecurityRoleName = dRow["SecurityRoleName"].ToString();
                CallsPageName = dRow["CallsPageName"].ToString();
                TabName = dRow["TabName"].ToString();
                TabAlert = dRow["TabAlert"].ToString();
                TabClass = dRow["TabClass"].ToString();
                TabIcon = dRow["TabIcon"].ToString();

            }
        }
    }
}


