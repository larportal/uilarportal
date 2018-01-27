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
    public class cLocationMaintenance
    {

        private Int32 _LocationMaintenanceID = -1;
        private Int32 _LocationID = -1;
        private string _PreEventMaintenance = "";
        private string _PostEventMaintenance = "";
        private Int32 _LogDamage = -1;
        private string _Comments = "";
        private DateTime? _DateAdded;
        private DateTime? _DateChanged;
        private Int32 _UserID = -1;
        private string _UserName = "";

        public Int32 LocationMaintenanceID
        {
            get { return _LocationMaintenanceID; }
            set { _LocationMaintenanceID = value; }
        }
        public Int32 LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }
        public string PreEventMaintenace
        { 
            get { return _PreEventMaintenance; }
            set { _PreEventMaintenance = value; }
        }
        public string PostEventMaintenance
        {
            get { return _PostEventMaintenance; }
            set { _PostEventMaintenance = value; }
        }
        public Int32 LogDamage
        {
            get { return _LogDamage; }
            set { _LogDamage = value;}
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

        private cLocationMaintenance()
        {

        }

        public cLocationMaintenance(Int32 intLocationMaintenanceID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            _LocationMaintenanceID = intLocationMaintenanceID;
            _UserID = intUserID;
            _UserName = strUserName;
            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@LocationMaintenanceID", _LocationMaintenanceID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetLocationMaintenanceByID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _LocationID = ldt.Rows[0]["LocationID"].ToString().ToInt32();
                    _PreEventMaintenance = ldt.Rows[0]["PreEventMaintenance"].ToString();
                    _PreEventMaintenance = ldt.Rows[0]["PostEventMaintenance"].ToString();
                    _LogDamage = ldt.Rows[0]["LogDamage"].ToString().ToInt32();
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
                slParams.Add("@LocationMaintenanceID", _LocationMaintenanceID);
                slParams.Add("@LocationID",LocationID);
                slParams.Add("@PreEventMaintenance", _PreEventMaintenance);
                slParams.Add("@PostEventMaintenance", _PostEventMaintenance);
                slParams.Add("@LogDamage", _LogDamage);
                slParams.Add("@Comments", _Comments);

                cUtilities.PerformNonQuery("uspInsUpdSTLocationMaintenance", slParams, "LarpPortal", _UserName);


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