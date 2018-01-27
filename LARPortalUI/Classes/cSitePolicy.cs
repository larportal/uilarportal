using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;


namespace LarpPortal.Classes
{
    public class cSitePolicy
    {

        private Int32 _SitePolicyID = -1;
        private Int32 _SiteID = -1;
        private Int32 _PolicyID = -1;
        private string _Policy = "";
        private string _Comments = "";
        private DateTime? _DateAdded;
        private DateTime? _DateChanged;
        private Int32 _UserID = -1;
        private string _UserName = "";

        public Int32 SitePolicyID
        {
            get { return _SitePolicyID; }
            set { _SitePolicyID = value; } 
        }
        public Int32 SiteID
        {
            get { return _SiteID; }
            set { _SiteID = value; }
        }
        public Int32 PolicyID 
        {
            get { return _PolicyID; }
            set { _PolicyID = value; }
        }
        public string Policy
        { get { return _Policy; }
            set { _Policy = value; }
        }
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        public DateTime? DateAdded
        {
            get { return _DateAdded; }
        }
        public DateTime? DateChanged
        {
            get { return _DateChanged; }
        }

        private cSitePolicy()
        {

        }

        public cSitePolicy(Int32 intSitePolicyID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            _SitePolicyID = intSitePolicyID;
            _UserID = intUserID;
            _UserName = strUserName;
            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@intSitePolicyID", _SitePolicyID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetSTSitePoliciesByID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _SiteID = ldt.Rows[0]["SiteID"].ToString().ToInt32();
                    _PolicyID = ldt.Rows[0]["PolicyID"].ToString().ToInt32();
                    _Policy = ldt.Rows[0]["Policy"].ToString();
                    _Comments = ldt.Rows[0]["Comments"].ToString().Trim();
                    _DateAdded = Convert.ToDateTime(ldt.Rows[0]["DateAdded"].ToString().ToString());
                    _DateChanged = Convert.ToDateTime(ldt.Rows[0]["DateChanged"].ToString().ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }
       
        public Boolean Save()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Boolean blnReturn = false;
            try
            {
                SortedList slParams = new SortedList();
                slParams.Add("@UserID", _UserID);
                slParams.Add("@SitePolicyID" , _SitePolicyID);
                slParams.Add("@SiteID",_SitePolicyID);
                slParams.Add("@PolicyID", _PolicyID);
                slParams.Add("@Policy", _Policy);
                slParams.Add("@Comments", _Comments);

                cUtilities.PerformNonQuery("Uspinsupdstsitepolicies", slParams, "LarpPortal", _UserName);


                blnReturn = true;
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
            return blnReturn;
        }

    }
}
