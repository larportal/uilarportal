using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LarpPortal.Classes
{
    [Serializable]
    public class cPhoneType
    {
        public int PhoneTypeID { get; set; }
        public string PhoneType { get; set; }
        public int SortOrder { get; set; }
    }

    [Serializable()]
    public class cPhoneProvider
    {
        public int PhoneProviderID { get; set; }
        public string ProviderName { get; set; }
        public string ProviderGateway { get; set; }     // The provider gateway is what you put at the end of email address to send a text as an email; ex. 2035551212@vtext.com
        public int SortOrder { get; set; }
    }

    [Serializable()]
    public class cPhone
    {
        private Int32 _UserID = -1;
        private Int32 _PhoneNumberID = -1;
        private Int32 _PhoneTypeID = -1;
        private string _PhoneTypeDescription = "";
        private string _IDD = "";
        private string _CountryCode = "";
        private string _AreaCode = "";
        private string _PhoneNumber = "";
        private string _Extension = "";
        private string _Comments = "";
        private string _UserName = "";
        private int? _ProviderID = null;

        public bool IsPrimary { get; set; }

        public Int32 PhoneNumberID
        {
            get { return _PhoneNumberID; }
            set { _PhoneNumberID = value; }
        }
        public Int32 PhoneTypeID
        {
            get { return _PhoneTypeID; }
            set
            {
                _PhoneTypeID = value;
            }
        }

        public List<cPhoneType> PhoneTypes = new List<cPhoneType>();
        public List<cPhoneProvider> ProviderList = new List<cPhoneProvider>();

        public string PhoneType
        {
            get
            {
                return PhoneTypes.Find(x => x.PhoneTypeID == PhoneTypeID).PhoneType;
            }
        }

        public string IDD
        {
            get { return _IDD; }
            set { _IDD = value; }
        }
        public string CountryCode
        {
            get { return _CountryCode; }
            set { _CountryCode = value; }
        }
        public string AreaCode
        {
            get { return _AreaCode; }
            set { _AreaCode = value; }
        }
        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
        }
        public string Extension
        {
            get { return _Extension; }
            set { _Extension = value; }
        }
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        public string Provider
        {
            get
            {
                string ProvName = "";
                if (ProviderList.Count > 0)
                    if (ProviderID.HasValue)
                        try
                        {
                            ProvName = ProviderList.Single(x => x.PhoneProviderID == ProviderID.Value).ProviderName;
                        }
                        catch
                        {
                            // Means the value wasn't found. Set it to blank.
                            ProvName = "";
                        }
                return ProvName;
            }
        }

        public int? ProviderID
        {
            get { return _ProviderID; }
            set { _ProviderID = value; }
        }

        public cPhone()
        {
            PhoneNumberID = -1;
            ProviderList = new List<cPhoneProvider>();
            PhoneTypes = new List<cPhoneType>();
            PhoneTypeID = 0;
            //SortedList sParams = new SortedList();

            //DataTable dtPhoneTypes = cUtilities.LoadDataTable("uspGetPhoneTypes", sParams, "LARPortal", "", "cPhone.Creator");

            //// Get the list of providers. This is used both for the lookup but also be available for the drop down list.     JBradshaw 5/26/2017
            //DataTable dtPhoneProv = cUtilities.LoadDataTable("uspGetPhoneProviders", sParams, "LARPortal", "", "cPhone.Creator.PhoneProviders");
            //ProviderList = new List<cPhoneProvider>();
            //foreach (DataRow dRow in dtPhoneProv.Rows)
            //{
            //    cPhoneProvider Prov = new cPhoneProvider();
            //    Prov.ProviderGateway = dRow["ProviderGateway"].ToString();
            //    Prov.ProviderName = dRow["ProviderName"].ToString();
            //    int iTemp;
            //    if (int.TryParse(dRow["PhoneProviderID"].ToString(), out iTemp))
            //        Prov.PhoneProviderID = iTemp;
            //    ProviderList.Add(Prov);
            //}

            //PhoneTypes = new List<cPhoneType>();
            //if (new DataView(dtPhoneTypes, "PhoneTypeID = 0", "", DataViewRowState.CurrentRows).Count == 0)
            //{
            //    cPhoneType NewPhoneNumber = new cPhoneType();
            //    NewPhoneNumber.PhoneTypeID = 0;
            //    NewPhoneNumber.PhoneType = "Choose...";
            //    PhoneTypes.Add(NewPhoneNumber);
            //}

            //foreach (DataRow dRow in dtPhoneTypes.Rows)
            //{
            //    cPhoneType NewPhoneNumber = new cPhoneType();
            //    NewPhoneNumber.PhoneTypeID = dRow["PhoneTypeID"].ToString().ToInt32();
            //    NewPhoneNumber.PhoneType = dRow["PhoneType"].ToString();
            //    PhoneTypes.Add(NewPhoneNumber);
            //}

            //PhoneTypeID = 0;
        }

        public cPhone(Int32 intPhoneNumberID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            _PhoneNumberID = intPhoneNumberID;
            _UserID = intUserID;
            _UserName = strUserName;
            try
            {
                SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
                slParams.Add("@intPhoneNumberID", _PhoneNumberID);
                DataSet lds = cUtilities.LoadDataSet("uspGetPhoneNumber", slParams, "LARPortal", _UserName, lsRoutineName);
                if (lds.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = lds.Tables[0].Rows[0];
                    _PhoneTypeID = dRow["PhoneTypeID"].ToString().Trim().ToInt32();
                    if (dRow["PrimaryPhone"] != null) { IsPrimary = (bool)dRow["PrimaryPhone"]; }

                    _PhoneTypeDescription = dRow["PhoneType"].ToString();
                    _IDD = dRow["IDD"].ToString();
                    _CountryCode = dRow["CountryCode"].ToString();
                    _AreaCode = dRow["AreaCode"].ToString();
                    _PhoneNumber = dRow["PhoneNumber"].ToString();
                    _Extension = dRow["Extension"].ToString();
                    _Comments = dRow["Comments"].ToString();
                    int iTemp;
                    if (int.TryParse(dRow["ProviderID"].ToString(), out iTemp))
                        _ProviderID = iTemp;
                    else
                        _ProviderID = null;
                }
                if (lds.Tables.Count > 1)
                {
                    PhoneTypes = new List<cPhoneType>();
                    foreach (DataRow dRow in lds.Tables[1].Rows)
                    {
                        cPhoneType NewPhoneNumber = new cPhoneType();
                        NewPhoneNumber.PhoneTypeID = dRow["PhoneTypeID"].ToString().ToInt32();
                        NewPhoneNumber.PhoneType = dRow["PhoneType"].ToString();
                        PhoneTypes.Add(NewPhoneNumber);
                    }
                }
                if (lds.Tables.Count > 2)
                {
                    ProviderList = new List<cPhoneProvider>();
                    foreach (DataRow dRow in lds.Tables[2].Rows)
                    {
                        cPhoneProvider Prov = new cPhoneProvider();
                        Prov.ProviderGateway = dRow["ProviderGateway"].ToString();
                        Prov.ProviderName = dRow["ProviderName"].ToString();
                        int iTemp;
                        if (int.TryParse(dRow["PhoneProviderID"].ToString(), out iTemp))
                            Prov.PhoneProviderID = iTemp;
                        ProviderList.Add(Prov);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }

        //private void GetPhoneTypeDescription()
        //{

        //    MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

        //    try
        //    {
        //        SortedList slParams = new SortedList();
        //        slParams.Add("@PhoneTypeID", _PhoneTypeID);
        //        _PhoneTypeDescription = cUtilities.ReturnStringFromSQL("somestoredprocedure", "PhoneType", slParams, "LarpPortal", _UserName, lsRoutineName);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorAtServer lobjError = new ErrorAtServer();
        //        lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);

        //    }            
        //}

        public static bool isValidPhoneNumber(string strPhone, int length)
        {
            ErrorDescription = string.Empty;

            if (string.IsNullOrWhiteSpace(strPhone))
            {
                ErrorDescription = "Phone Number cannont be empty";
                if (length == 3)
                    ErrorDescription = "Area Code cannot be empty";
                return false;
            }

            strPhone = strPhone.Trim();
            if (length == 10)
            {
                Regex phoneExp = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
                if (phoneExp.IsMatch(strPhone) == false)
                {
                    ErrorDescription = "Phone number must be a 10 digit number";
                    return false;
                }
                return true;
            }

            if (length == 7)
            {
                Regex phoneExp = new Regex(@"^([0-9]{3})[-. ]?([0-9]{4})$");
                if (phoneExp.IsMatch(strPhone) == false)
                {
                    ErrorDescription = "Phone number must be a 7 digit number";
                    return false;
                }
                return true;
            }
            //Make sure all values are digits
            if (strPhone.All(x => Char.IsDigit(x)) == false)
                return false;
            //This line is a substitute to remove any non-digits and only if we ever disable check above
            //string strPhone = string.Join(string.Empty, strPhone.Where(x => Char.IsDigit(x)).ToArray());

            //800s, 900, and zero digits on first position are not okay
            //if (strPhone.StartsWith("8") || strPhone.StartsWith("9") || strPhone.StartsWith("0"))
            //    return false;

            // Get all the digits from the string and make sure we have ten numeric value
            return (strPhone.Length == length);
        }

        public string strErrorDescription { get; set; }

        public static string ErrorDescription { get; set; }

        public bool isValidPhoneNumber()
        {
            return isValidPhoneNumber(AreaCode + PhoneNumber, 10);
        }

        public bool IsValid()
        {
            strErrorDescription = string.Empty;

            if (isValidPhoneNumber(AreaCode, 3) == false)
            {
                strErrorDescription = (AreaCode + "") + " is not a valid Area Code must be a 3 digit number";
                return false;
            }

            if (isValidPhoneNumber(PhoneNumber, 7) == false)
            {
                strErrorDescription = (PhoneNumber + "") + " is not a valid Phone Number must be a 7 digit number";
                return false;
            }
            return true;
        }

        public Boolean SaveUpdate(int userID, bool delete = false)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Boolean bUpdateComplete = false;
            Boolean blnReturn = false;
            try
            {
                SortedList slParams = new SortedList();
                if (delete)
                {
                    slParams.Add("@RecordID", PhoneNumberID);
                    slParams.Add("@UserID", userID);
                    bUpdateComplete = cUtilities.PerformNonQueryBoolean("uspDelMDBPhoneNumbers", slParams, "LARPortal", _UserName + string.Empty);
                }
                else
                {
                    slParams.Add("@UserID", userID);
                    slParams.Add("@PhoneNumberID", _PhoneNumberID);
                    slParams.Add("@KeyID", userID);
                    slParams.Add("@KeyType", "cUser"); //I did to hard code this because the get uses this value and there is no property to set for this                
                    slParams.Add("@PhoneTypeID", _PhoneTypeID);
                    slParams.Add("@PrimaryPhone", IsPrimary);
                    slParams.Add("@IDD", _IDD);
                    slParams.Add("@CountryCode", _CountryCode + string.Empty); //if null insert empty string
                    slParams.Add("@AreaCode", _AreaCode + string.Empty);
                    slParams.Add("@PhoneNumber", _PhoneNumber + string.Empty);
                    slParams.Add("@Extension", _Extension + string.Empty);
                    if (_ProviderID.HasValue)
                    {
                        int iMobileType = PhoneTypes.First(x => x.PhoneType.ToUpper().StartsWith("MOBIL")).PhoneTypeID;
                        if (iMobileType == _PhoneTypeID)
                            slParams.Add("@ProviderID", _ProviderID.Value);
                        else
                            slParams.Add("@ClearProviderID", 1);
                    }
                    else
                        slParams.Add("@ClearProviderID", 1);

                    slParams.Add("@Comments", _Comments + string.Empty);
                    blnReturn = cUtilities.PerformNonQueryBoolean("uspInsUpdMDBPhoneNumbers", slParams, "LARPortal", _UserName);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
                blnReturn = false;
            }
            return blnReturn;
        }


    }


}