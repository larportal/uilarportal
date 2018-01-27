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
    public class cLocation
    {
        private Int32 _LocationID = 1;
        private Int32 _SiteID = -1;
        private string _LocationName = "";
        private Int32 _LocationTypeID = -1;
        private string _Dimensions = "";
        private Int32 _NumberOfMovableBeds = -1;
        private Int32 _NumberOfUnmovableBeds = -1;
        private Int32 _NumberOfTopBunks = -1;
        private Int32 _NumberOfDoors = -1;
        private Int32 _Status = -1;
        private Boolean _Heated = false;
        private Boolean _Electricity = false;
        private Boolean _RunningWater = false;
        private Boolean _CookingFacilities = false;
        private Int32 _NumberOfToilets = -1;
        private Int32 _NumberOfShowers = -1;
        private Boolean _HandicappedAccessible = false;
        private Boolean _CamperHookup = false;
        private Boolean _SmokingAllowed = false;
        private Int32 _NumberParkingSpaces = -1;
        private DateTime? _UnavilableStartDate;
        private DateTime? _UnavailableEndDate;
        private string _ResourcesAvailable = "";
        private string _LocationNotes = "";
        private string _Comments = "";
        private DateTime? _DateAdded;
        private DateTime? _DateChanged;
        private Int32 _UserID = -1;
        private string _UserName = "";
        private cLocationType _LocationTypeInfo;
        private List<cLocationMaintenance> _LocationMaintenance;

        public Int32 LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }
        public Int32 SiteID
        {
            get { return _SiteID; }
            set { _SiteID = value; }
        }
        public string LocationName
        {
            get { return _LocationName; }
            set { _LocationName = value; }
        }
        public Int32 LocationType
        {
            get { return _LocationTypeID; }
            set { 
                _LocationTypeID = value;
                _LocationTypeInfo = new cLocationType(_LocationTypeID, _UserID, _UserName);
                }
        }
        public string Dimensions
        {
            get { return _Dimensions; }
            set { _Dimensions = value; }
        }
        public Int32 NumberOfMovableBeds
        {
            get { return _NumberOfMovableBeds; }
            set { _NumberOfMovableBeds = value; }
        }
        public Int32 NumberOfUnmovableBeds
        {
            get { return _NumberOfUnmovableBeds; }
            set { _NumberOfUnmovableBeds = value; }
        }
        public Int32 NumberTopBunks
        {
            get { return _NumberOfTopBunks; }
            set { _NumberOfTopBunks = value; }
        }
        public Int32 NuberOfDoors
        {
            get { return _NumberOfDoors; }
            set { _NumberOfDoors = value; }
        }
        public Int32 Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public Boolean Heated
        {
            get { return _Heated; }
            set { _Heated = value; }
        }
        public Boolean Electricity
        {
            get { return _Electricity; }
            set { _Electricity = value; }
        }
        public Boolean RunningWater
        {
            get { return _RunningWater; }
            set { _RunningWater = value;  }
        }
        public Boolean CookingFacilities
        {
            get { return _CookingFacilities; }
            set { _CookingFacilities = value; }
        }
        public Int32 NumberOfToilets
        {
            get { return _NumberOfToilets; }
            set { _NumberOfToilets = value; }
        }
        public Int32 NumberOfShowers
        {
            get { return _NumberOfShowers; }
            set { _NumberOfShowers = value; }
        }
        public Boolean HandicappedAccessable
        {
            get { return _HandicappedAccessible; }
            set { _HandicappedAccessible = value; }
        }
        public Boolean CamperHookup
        {
            get { return _CamperHookup; }
            set { _CamperHookup = value; }
        }
        public Boolean SmokingAllowed
        {
            get { return _SmokingAllowed; }
            set { _SmokingAllowed = value;  }
        }
        public Int32 NumberParkingSpaces
        {
            get { return _NumberParkingSpaces; }
            set { _NumberParkingSpaces = value; }
        }
        public DateTime? UnavailableStartDate
        {
            get { return _UnavilableStartDate; }
            set { _UnavilableStartDate = value;  }
        }
        public DateTime? UnavailableEndDate
        {
            get { return _UnavailableEndDate; }
            set { _UnavailableEndDate = value; }
        }
        public string ResourcesAvailable
        {
            get { return _ResourcesAvailable; }
            set { _ResourcesAvailable = value; }
        }
        public string LocationNotes
        {
            get { return _LocationNotes; }
            set { _LocationNotes = value; }
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
        public cLocationType LocationTypeInfo
        {
            get { return _LocationTypeInfo; }
            set { _LocationTypeInfo = value; }
        }
        public List<cLocationMaintenance> LocationMaintenance
        {
            get { return _LocationMaintenance; }
            set { _LocationMaintenance = value; }
        }


        private cLocation()
        {

        }

        public cLocation(Int32 intLocationID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            _LocationID = intLocationID;
            _UserID = intUserID;
            _UserName = strUserName;
            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@LocationID", _SiteID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetLocationByID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _SiteID = ldt.Rows[0]["SiteID"].ToString().ToInt32();
                    _LocationName = ldt.Rows[0]["SiteName"].ToString();
                    _LocationTypeID = ldt.Rows[0]["LocationTypeID"].ToString().ToInt32();
                    _Dimensions = ldt.Rows[0]["Dimensions"].ToString();
                    _NumberOfMovableBeds = ldt.Rows[0]["NumberOfMovableBeds"].ToString().ToInt32();
                    _NumberOfUnmovableBeds = ldt.Rows[0]["NumberOfUnmovableBeds"].ToString().ToInt32();
                    _NumberOfTopBunks = ldt.Rows[0]["NumberTopBunks"].ToString().ToInt32();
                    _NumberOfDoors = ldt.Rows[0]["NumberDoors"].ToString().ToInt32();
                    _Status = ldt.Rows[0]["Status"].ToString().ToInt32();
                    _Heated = ldt.Rows[0]["Heated"].ToString().ToBoolean();
                    _Electricity = ldt.Rows[0]["Electricity"].ToString().ToBoolean();
                    _RunningWater = ldt.Rows[0]["RunningWater"].ToString().ToBoolean();
                    _CookingFacilities = ldt.Rows[0]["CookingFacilities"].ToString().ToBoolean();
                    _NumberOfToilets = ldt.Rows[0]["NumberOfToilets"].ToString().ToInt32();
                    _NumberOfShowers = ldt.Rows[0]["NumberOfShowers"].ToString().ToInt32();
                    _HandicappedAccessible = ldt.Rows[0]["HandicappedAccessble"].ToString().ToBoolean();
                    _CamperHookup = ldt.Rows[0]["CamperHookup"].ToString().ToBoolean();
                    _SmokingAllowed = ldt.Rows[0]["SmokingAllowed"].ToString().ToBoolean();
                    _NumberParkingSpaces = ldt.Rows[0]["NumberParkingSpaces"].ToString().ToInt32();
                    _UnavilableStartDate = Convert.ToDateTime(ldt.Rows[0]["UnavailableStartDate"].ToString());
                    _UnavailableEndDate = Convert.ToDateTime(ldt.Rows[0]["UnavailableEndDate"].ToString());
                    _ResourcesAvailable = ldt.Rows[0]["ResourcesAvalable"].ToString();
                    _LocationNotes = ldt.Rows[0]["LocationNotes"].ToString();
                    _Comments = ldt.Rows[0]["Comments"].ToString().Trim();
                    _DateAdded = Convert.ToDateTime(ldt.Rows[0]["DateAdded"].ToString().ToString());
                    _DateChanged = Convert.ToDateTime(ldt.Rows[0]["DateChanged"].ToString().ToString());
                }
                _LocationTypeInfo = new cLocationType(_LocationTypeID, _UserID, _UserName);
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
                slParams.Add("@LocationID", _LocationID);
                slParams.Add("@SiteID",_SiteID);
                slParams.Add("@LocationName", _LocationName);
                slParams.Add("@LocationTypeID", _LocationTypeID);
                slParams.Add("@Dimensions", _Dimensions);
                slParams.Add("@NumberOfMovableBeds", _NumberOfMovableBeds);
                slParams.Add("@NumberOfUnmovableBeds", _NumberOfUnmovableBeds);
                slParams.Add("@NumberTopBunks", _NumberOfTopBunks);
                slParams.Add("@NumberDoors", _NumberOfDoors);
                slParams.Add("@Status", _Status);
                slParams.Add("@Heated", _Heated);
                slParams.Add("@Electricity", _Electricity);
                slParams.Add("@RunningWater", _RunningWater);
                slParams.Add("@CookingFacilities", _CookingFacilities);
                slParams.Add("@NumberOfToilets", _NumberOfToilets);
                slParams.Add("@NumberOfShowers", _NumberOfShowers);
                slParams.Add("@HandicappedAccessble", _HandicappedAccessible);
                slParams.Add("@CamperHookup", _CamperHookup);
                slParams.Add("@SmokingAllowed", _SmokingAllowed);
                slParams.Add("@NumberParkingSpaces", _NumberParkingSpaces);
                slParams.Add("@UnavailableStartDate", _UnavilableStartDate);
                slParams.Add("@UnavailableEndDate", _UnavailableEndDate);
                slParams.Add("@ResourcesAvalable", _ResourcesAvailable);
                slParams.Add("@LocationNotes", _LocationNotes);
                slParams.Add("@Comments", _Comments);
                cUtilities.PerformNonQuery("uspInsUpdSTLocations", slParams, "LarpPortal", _UserName);

                

                blnReturn = true;
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
            return blnReturn;
        }

        private void LoadLocationMaintenance()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
                slParams.Add("@LocationID", _LocationID);
                DataTable ldt = cUtilities.LoadDataTable("uspGetLocationMaintenanceByLocationID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    foreach (DataRow ldr in ldt.Rows)
                    {
                        cLocationMaintenance cAdd = new cLocationMaintenance(ldr["LocationMaintenanceID"].ToString().Trim().ToInt32(), _UserID, _UserName);
                        _LocationMaintenance.Add(cAdd);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }

    }


}