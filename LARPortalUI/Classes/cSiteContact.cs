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
    public class cSiteContact
    {

        private Int32 _SiteContactID = -1;
        private Int32 _SiteID = -1;
        private Int32 _ContactTypeID = -1;
        private string _FirstName = "";
        private string _LastName = "";
        private Boolean _UseSiteAddress = false;
        private Int32 _PrimaryAddressID = -1;
        private cAddress _PrimaryAddress;
        private Int32 _PrimaryPhoneID = -1;
        private cPhone _PrimaryPhone;
        private Int32 _PrimaryEmailID = -1;
        private cEMail _PrimaryEmail;
        private Int32 _AlternateSeasonalAddressID = -1;
        private cAddress _AlternateSeasonalAddress;
        private Int32 _AlternateSeasonalPhoneID = -1;
        private cPhone _AlternateSeasonalPhone;
        private Int32 _AlternateSeasonalEmailID = -1;
        private cEMail _AlternateSeasonalEmail;
        private string _Comments = "";
        private DateTime? _DateAdded;
        private DateTime? _DateChanged;
        private Int32 _UserID = -1;
        private string _UserName = "";

        public Int32 SiteContactID
        {
            get { return _SiteContactID; }
            set { _SiteContactID = value; }
        }
        public Int32 SiteID
        {
            get { return _SiteID; }
            set { _SiteID = value; }
        }
        public Int32 ContactTypeID 
        {
            get { return _ContactTypeID; }
            set { _ContactTypeID = value; }
        }
        public string FirstName
        { 
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public Boolean UseSiteAddress
        {
            get { return _UseSiteAddress; }
            set { _UseSiteAddress = value; }
        }
        public Int32 PrimaryAddressID
        {
            get { return _PrimaryAddressID; }
            set { _PrimaryAddressID = value;
                  _PrimaryAddress = new cAddress(_PrimaryAddressID,_UserName, _UserID);}
        }
        public cAddress PrimaryAddress
        {
            get { return _PrimaryAddress; }
            set { _PrimaryAddress = value; }
        }
        public Int32 PrimaryPhoneID
        {
            get { return _PrimaryPhoneID; }
            set
            {
                _PrimaryPhoneID = value;
                _PrimaryPhone = new cPhone(_PrimaryPhoneID, _UserID, _UserName);
            }
        }
        public cPhone PrimaryPhone
        {
            get { return _PrimaryPhone; }
            set { _PrimaryPhone = value; }
        }
        public Int32 PrimaryEmailID
        {
            get { return _PrimaryEmailID; }
            set
            {
                _PrimaryEmailID = value;
                _PrimaryEmail = new cEMail(_PrimaryEmailID, _UserName, _UserID);
            }
        }
        public cEMail PrimaryEmail
        {
            get { return _PrimaryEmail; }
            set { _PrimaryEmail = value; }
        }
        public Int32 AlternateSeasonalAddressID
        {
            get { return _AlternateSeasonalAddressID; }
            set
            {
                _AlternateSeasonalAddressID = value;
                _AlternateSeasonalAddress = new cAddress(_AlternateSeasonalAddressID, _UserName, _UserID);
            }
        }
        public cAddress AlternateSeasonalAddress
        {
            get { return _AlternateSeasonalAddress; }
            set { _AlternateSeasonalAddress = value; }
        }
        public Int32 AlternateSeasonalPhoneID
        {
            get { return _AlternateSeasonalPhoneID; }
            set
            {
                _AlternateSeasonalPhoneID = value;
                _AlternateSeasonalPhone = new cPhone(_AlternateSeasonalPhoneID, _UserID, _UserName);
            }
        }
        public cPhone AlternateSeasonalPhone
        {
            get { return _AlternateSeasonalPhone; }
            set { _AlternateSeasonalPhone = value; }
        }
        public Int32 AlternateSeasonalEmailID
        {
            get { return _AlternateSeasonalEmailID; }
            set
            {
                _AlternateSeasonalEmailID = value;
                _AlternateSeasonalEmail = new cEMail(_AlternateSeasonalEmailID, _UserName, _UserID);
            }
        }
        public cEMail AlternateSeasonalEmail
        {
            get { return _AlternateSeasonalEmail; }
            set { _AlternateSeasonalEmail = value; }
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

        private cSiteContact()
        {

        }

        public cSiteContact(Int32 intSiteContactID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            _SiteContactID = intSiteContactID;
            _UserID = intUserID;
            _UserName = strUserName;
            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@SiteContactID", _SiteContactID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetSiteContactByID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _SiteID = ldt.Rows[0]["SiteID"].ToString().ToInt32();
                    _ContactTypeID = ldt.Rows[0]["ContactTypeID"].ToString().ToInt32();
                    _FirstName = ldt.Rows[0]["FirstName"].ToString();
                    _FirstName = ldt.Rows[0]["LastName"].ToString();
                    _UseSiteAddress = ldt.Rows[0]["UseSiteAddress"].ToString().ToBoolean();
                    _PrimaryAddressID = ldt.Rows[0]["PrimaryAddressID"].ToString().ToInt32();
                    _PrimaryPhoneID = ldt.Rows[0]["PrimaryPhoneID"].ToString().ToInt32();
                    _PrimaryEmailID = ldt.Rows[0]["PrimaryEmailID"].ToString().ToInt32();
                    _AlternateSeasonalAddressID = ldt.Rows[0]["AlternateSeasonalAddressID"].ToString().ToInt32();
                    _AlternateSeasonalPhoneID = ldt.Rows[0]["AlternateSeasonalPhoneID"].ToString().ToInt32();
                    _AlternateSeasonalEmailID = ldt.Rows[0]["AlternateSeasonalEmailID"].ToString().ToInt32();
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
                slParams.Add("@SiteContactID", _SiteContactID);
                slParams.Add("@SiteID",_SiteContactID);
                slParams.Add("@ContactTypeID", _ContactTypeID);
                slParams.Add("@FirstName", _FirstName);
                slParams.Add("@LastName", _LastName);
                slParams.Add("@UseSiteAddress", _UseSiteAddress);
                slParams.Add("@PrimaryAddressID", _PrimaryAddressID);
                slParams.Add("@PrimaryPhoneID", _PrimaryPhoneID);
                slParams.Add("@PrimaryEmailID", _PrimaryEmailID);
                slParams.Add("@AlternateSeasonalAddressID", _AlternateSeasonalAddressID);
                slParams.Add("@AlternateSeasonalPhoneID", _AlternateSeasonalPhoneID);
                slParams.Add("@AlternateSeasonalEmailID", _AlternateSeasonalEmailID);
                slParams.Add("@Comments", _Comments);

                cUtilities.PerformNonQuery("uspInsUpdSTSiteContacts", slParams, "LarpPortal", _UserName);


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