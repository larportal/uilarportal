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
    public class cLocationType
    {

        private Int32 _LocationTypeID = -1;
        private string _Description = "";
        private string _Comments = "";
        private DateTime? _DateAdded;
        private DateTime? _DateChanged;
        private Int32 _UserID = -1;
        private string _UserName = "";

        public Int32 LocationTypeID
        {
            get { return _LocationTypeID; }
            set { _LocationTypeID = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
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

        private cLocationType()
        {

        }

        public cLocationType(Int32 intLocationTypeID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            _LocationTypeID = intLocationTypeID;
            _UserID = intUserID;
            _UserName = strUserName;
            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@LocationTypeID", _LocationTypeID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetLocationTypeByID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _Description = ldt.Rows[0]["Description"].ToString();
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
                slParams.Add("@LocationTypeID", _LocationTypeID);
                slParams.Add("@Description", _Description);
                slParams.Add("@Comments", _Comments);

                cUtilities.PerformNonQuery("uspInsUpdSTLocationTypes", slParams, "LarpPortal", _UserName);


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