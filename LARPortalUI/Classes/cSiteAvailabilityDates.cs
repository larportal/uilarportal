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
    public class cSiteAvailabilityDate
    {

        private Int32 _SiteAvailabilityDateID = -1;
        private Int32 _SiteID = -1;
        private Int32 _AvailabilityStartMonth = -1;
        private Int32 _AvailabilityStartDay  = -1;
        private Int32 _AvailabilityEndMonth = -1;
        private Int32 _AvailabilityEndDay = -1;
        private string _Comments = "";
        private DateTime? _DateAdded;
        private DateTime? _DateChanged;
        private Int32 _UserID = -1;
        private string _UserName = "";

        public Int32 SiteAvailabilityDateID
        {
            get { return _SiteAvailabilityDateID; }
            set { _SiteAvailabilityDateID = value; }
        }
        public Int32 SiteID
        {
            get { return _SiteID; }
            set { _SiteID = value; }
        }
        public Int32 AvailabilityStartMonth 
        {
            get { return _AvailabilityStartMonth; }
            set { _AvailabilityStartMonth = value; }
        }
        public Int32 AvailabilityStartDay
        {
            get { return _AvailabilityStartDay; }
            set { _AvailabilityStartDay = value;}
        }
        public Int32 AvailabilityEndMonth
        {
            get { return _AvailabilityEndMonth; }
            set { _AvailabilityEndMonth = value; }
        }
        public Int32 AvailabilityEndDay
        {
            get { return _AvailabilityEndDay; }
            set { _AvailabilityEndDay = value; }
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

        private cSiteAvailabilityDate()
        {

        }

        public cSiteAvailabilityDate(Int32 intSiteAvailabilityDateID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            _SiteAvailabilityDateID = intSiteAvailabilityDateID;
            _UserID = intUserID;
            _UserName = strUserName;
            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@SiteAvailabilityDateID", _SiteAvailabilityDateID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetSiteAvailabilityDatesByID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _SiteID = ldt.Rows[0]["SiteID"].ToString().ToInt32();
                    _AvailabilityStartMonth = ldt.Rows[0]["AvailabilityStartMonth"].ToString().ToInt32();
                    _AvailabilityStartDay = ldt.Rows[0]["AvailabilityStartDay"].ToString().ToInt32();
                    _AvailabilityEndMonth = ldt.Rows[0]["AvailabilityEndMonth"].ToString().ToInt32();
                    _AvailabilityEndDay = ldt.Rows[0]["AvailabilityEndDay"].ToString().ToInt32();
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
                slParams.Add("@SiteAvailabilityDateID", _SiteAvailabilityDateID);
                slParams.Add("@SiteID",_SiteID);
                slParams.Add("@AvailabilityStartMonth", _AvailabilityStartMonth);
                slParams.Add("@AvailabilityStartDay", _AvailabilityStartDay);
                slParams.Add("@AvailabilityEndMonth", _AvailabilityEndMonth);
                slParams.Add("@AvailabilityEndDay", _AvailabilityEndDay);
                slParams.Add("@Comments", _Comments);

                cUtilities.PerformNonQuery("uspInsUpdSTSiteAvailabilityDates", slParams, "LarpPortal", _UserName);


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