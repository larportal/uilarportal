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
    public class cSite
    {

        private Int32 _SiteID = -1;
        private string _SiteName = "";
        private cAddress _SiteAddress ;
        private cPhone _SitePhone ;
        private string _URL = "";
        private string _SiteMapURL = "";
        private string _SiteDirections = "";
        private Boolean _YearRound = false;
        private string _TimeRestrictions = "";
        private Boolean _EMTCertReqd = false; // EMT Certification required
        private Boolean _CookCertReqd = false; // Cooling certification required
        private Boolean _AddWaiversReqd = false; //additional waivers required
        private string _SiteNotes = "";
        private string _Comments = "";
        private DateTime? _DateAdded;
        private DateTime? _DateChanged;
        private Int32 _UserID = -1;
        private string _UserName = "";
        private List<cSitePolicy> _SitePolicies;
        private List<cSiteImage> _SiteImages;
        private List<cSiteContact> _SiteContacts;
        private List<cSiteComment> _SiteComments;
        private List<cSiteAvailabilityDate> _SiteAvailabilityDates;
        private List<cLocation> _Locations;

        public Int32 SiteID
        {
            get { return _SiteID; }
            set { _SiteID = value; }
        }
        public string SiteName
        {
            get { return _SiteName; }
            set { _SiteName = value; }
        }
        public cAddress SiteAddress
        { get { return _SiteAddress; }
            set { _SiteAddress = value; }
        }
        public cPhone SitePhone 
        { get {return _SitePhone;}
            set { _SitePhone = value; }
        }
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }
        public string SiteMapURL
        { get { return _SiteMapURL; }
            set { _SiteMapURL = value; }
        }
        public string SiteDirections
        { get { return _SiteDirections; }
            set { _SiteDirections = value; }
        }
        public Boolean YearRound
        {
            get { return _YearRound; }
            set {_YearRound = value;}
        }
        public string TimeRestrictions
        {
            get { return _TimeRestrictions; }
            set { _TimeRestrictions = value; }
        }
        public Boolean EMTCertReqd
        {
            get { return _EMTCertReqd; }
            set { _EMTCertReqd = value; }
        }
        public Boolean CookCertReqd
        {
            get { return _CookCertReqd; }
            set { _CookCertReqd = value;  }
        }
        public Boolean AddWaiversReqd
        {
            get { return _AddWaiversReqd; }
            set { _AddWaiversReqd = value; }
        }
        public string SiteNotes
        {
            get { return _SiteNotes; }
            set { _SiteNotes = value; }
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
        public List<cSitePolicy> SitePolicies
        {
            get { return _SitePolicies; }
            set {_SitePolicies = value;}
        }
        public List<cSiteImage> SiteImages
        {
            get { return _SiteImages; }
            set { _SiteImages = value; }
        }
        public List<cSiteContact> SiteContacts
        {
            get { return _SiteContacts; }
            set { _SiteContacts = value; }
        }
        public List<cSiteComment> SiteComments
        {
            get { return _SiteComments; }
            set { _SiteComments = value; }
        }
        public List<cSiteAvailabilityDate> SiteAvailabilityDates
        {
            get { return _SiteAvailabilityDates; }
            set {_SiteAvailabilityDates = value;}
        }
        public List<cLocation> Locations
        {
            get { return _Locations; }
            set { _Locations = value; }
        }

        public cSite()
        {

        }

        public cSite(Int32 intSiteID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            _SiteID = intSiteID;
            _UserID = intUserID;
            _UserName = strUserName;
            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@intSiteID", _SiteID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetSTSitesByID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _SiteName = ldt.Rows[0]["SiteName"].ToString();
                    _SiteAddress = new cAddress(ldt.Rows[0]["AddressID"].ToString().ToInt32(), _UserName, _UserID);
                    _SitePhone = new cPhone(ldt.Rows[0]["PhoneID"].ToString().ToInt32(), _UserID,  _UserName);
                    _URL = ldt.Rows[0]["URL"].ToString().Trim();
                    _SiteMapURL = ldt.Rows[0]["SiteMapURL"].ToString().Trim();
                    _SiteDirections = ldt.Rows[0]["SiteDirections"].ToString();
                    _YearRound = ldt.Rows[0]["YearRound"].ToString().ToBoolean();
                    _TimeRestrictions = ldt.Rows[0]["TimeRestrictions"].ToString();
                    _EMTCertReqd = ldt.Rows[0]["EMTCertificationRequired"].ToString().ToBoolean();
                    _CookCertReqd = ldt.Rows[0]["CookingCertificationRequired"].ToString().ToBoolean();
                    _AddWaiversReqd = ldt.Rows[0]["AdditionalWaiversRequired"].ToString().ToBoolean();
                    _SiteNotes = ldt.Rows[0]["SiteNotes"].ToString().Trim();
                    _Comments = ldt.Rows[0]["Comments"].ToString().Trim();
                    _DateAdded = Convert.ToDateTime(ldt.Rows[0]["DateAdded"].ToString().ToString());
                    _DateChanged = Convert.ToDateTime(ldt.Rows[0]["DateChanged"].ToString().ToString());

                    LoadSitePolicies();
                    LoadSiteImages();
                    LoadSiteContacts();
                    LoadSiteComments();
                    LoadSiteAvailabilityDates();
                    LoadLocations();
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
                slParams.Add("@SiteID",_SiteID);
                slParams.Add("@SiteName", _SiteName);
                slParams.Add("@AddressID", _SiteAddress.AddressID);
                slParams.Add("@PhoneID", _SitePhone.PhoneNumberID);
                slParams.Add("@URL", _URL);
                slParams.Add("@SiteMapURL", _SiteMapURL);
                slParams.Add("@SiteDirections", _SiteDirections);
                slParams.Add("@YearRound", _YearRound);
                slParams.Add("@TimeRestrictions", _TimeRestrictions);
                slParams.Add("@EMTCertificationRequired", _EMTCertReqd);
                slParams.Add("@CookingCertificationRequired", _CookCertReqd);
                slParams.Add("@AdditionalWaiversRequired", _AddWaiversReqd);
                slParams.Add("@SiteNotes", _SiteNotes);
                slParams.Add("@Comments", _Comments);

                cUtilities.PerformNonQuery("uspInsUpdSTSites",slParams, "LarpPortal", _UserName);


                blnReturn = true;
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
            return blnReturn;
        }

        private void LoadSitePolicies()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
                slParams.Add("@SiteID", _SiteID);
                DataTable ldt = cUtilities.LoadDataTable("uspGetSitePoliciesBySiteID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    foreach (DataRow ldr in ldt.Rows)
                    {
                        cSitePolicy cAdd = new cSitePolicy(ldr["SitePolicyID"].ToString().Trim().ToInt32(), _UserID, _UserName);
                        _SitePolicies.Add(cAdd);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }
        private void LoadSiteImages()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
                slParams.Add("@SiteID", _SiteID);
                DataTable ldt = cUtilities.LoadDataTable("uspGetSiteImagesBySiteID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    foreach (DataRow ldr in ldt.Rows)
                    {
                        cSiteImage cAdd = new cSiteImage(ldr["SiteImageID"].ToString().Trim().ToInt32(), _UserID, _UserName);
                        _SiteImages.Add(cAdd);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }

        }
        private void LoadSiteContacts()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
                slParams.Add("@SiteID", _SiteID);
                DataTable ldt = cUtilities.LoadDataTable("uspGetSiteContactsBySiteID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    foreach (DataRow ldr in ldt.Rows)
                    {
                        cSiteContact cAdd = new cSiteContact(ldr["SiteContactID"].ToString().Trim().ToInt32(), _UserID, _UserName);
                        _SiteContacts.Add(cAdd);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }
        private void LoadSiteComments()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
                slParams.Add("@SiteID", _SiteID);
                DataTable ldt = cUtilities.LoadDataTable("uspGetSiteCommentsBySiteID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    foreach (DataRow ldr in ldt.Rows)
                    {
                        cSiteComment cAdd = new cSiteComment(ldr["SiteCommentID"].ToString().Trim().ToInt32(), _UserID, _UserName);
                        _SiteComments.Add(cAdd);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }
        private void LoadSiteAvailabilityDates()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
                slParams.Add("@SiteID", _SiteID);
                DataTable ldt = cUtilities.LoadDataTable("uspGetSiteAvailabilityDatesBySiteID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    foreach (DataRow ldr in ldt.Rows)
                    {
                        cSiteAvailabilityDate cAdd = new cSiteAvailabilityDate(ldr["SiteAvailabilityDateID"].ToString().Trim().ToInt32(), _UserID, _UserName);
                        _SiteAvailabilityDates.Add(cAdd);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }
        private void LoadLocations()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
                slParams.Add("@SiteID", _SiteID);
                DataTable ldt = cUtilities.LoadDataTable("uspGetSiteLocationsBySiteID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    foreach (DataRow ldr in ldt.Rows)
                    {
                        cLocation cAdd = new cLocation(ldr["LocationID"].ToString().Trim().ToInt32(), _UserID, _UserName);
                        _Locations.Add(cAdd);
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