using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Data;
namespace LarpPortal.Classes
{
    [Serializable]
    public class cAddressType
    {
        public int AddressTypeID { get; set; }
        public string AddressType { get; set; }
    }

    [Serializable()]
    public class cAddress
    {
        public bool IsPrimary { get; set; }

        public int AddressTypeID { get; set; }
        public List<cAddressType> AddressTypes = new List<cAddressType>();

        public string AddressType
        {
            get
            {
                return AddressTypes.Find(x => x.AddressTypeID == AddressTypeID).AddressType;
            }
        }
        /// <summary>
        /// When any validation is perform it tell what was wrong
        /// </summary>
        public string strErrorDescription { get; set; }

        public string UserName = "";
        public int AddressID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateID { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateChanged { get; set; }
        public DateTime DateDeleted { get; set; }
        public string StateName { get; set; }
        public string GoogleString { get; set; }
        //{
        //    get { return "http://maps.google.com/preview?q=" + _StrGoogleString; }
        //}

        /// <summary>
        /// Creates Empty caddress class object
        /// </summary>
        public cAddress()
        {
            AddressID = -1;
            AddressTypes = new List<cAddressType>();
            AddressTypeID = 0;

            //SortedList sParams = new SortedList();

            //DataTable dtAddressTypes = cUtilities.LoadDataTable("uspGetAddressTypes", sParams, "LARPortal", "", "cAddress.Creator");

            //AddressTypes = new List<cAddressType>();
            //if (new DataView(dtAddressTypes, "AddressTypeID = 0", "", DataViewRowState.CurrentRows).Count == 0)
            //{
            //    cAddressType NewAddress = new cAddressType();
            //    NewAddress.AddressTypeID = 0;
            //    NewAddress.AddressType = "Choose...";
            //    AddressTypes.Add(NewAddress);
            //}

            //foreach (DataRow dRow in dtAddressTypes.Rows)
            //{
            //    cAddressType NewAddress = new cAddressType();
            //    NewAddress.AddressTypeID = dRow["AddressTypeID"].ToString().ToInt32();
            //    NewAddress.AddressType = dRow["AddressType"].ToString();
            //    AddressTypes.Add(NewAddress);
            //}

            //AddressTypeID = 0;
        }

        /// <summary>
        /// Creates a caddress class object from data stored in the database with the Address ID provided
        /// </summary>
        /// <param name="intAddressID">ID for Address</param>
        /// <param name="userName">User ID</param>
        /// 
        public cAddress(Int32 intAddressID, string strUserNameParam, int intuserID)
        {
            UserName = strUserNameParam;
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;


            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@intAddressID", intAddressID);

            try
            {
                DataSet lds = cUtilities.LoadDataSet("uspGetAddress", slParams, "LARPortal", UserName, lsRoutineName);

                if (lds.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = lds.Tables[0].Rows[0];
                    AddressID = intAddressID;//Table MDBAddresses
                    if (dRow["Address1"] != null) { Address1 = dRow["Address1"].ToString(); }
                    if (dRow["Address2"] != null) { Address2 = dRow["Address2"].ToString(); }
                    if (dRow["City"] != null) { City = dRow["City"].ToString(); }
                    if (dRow["StateID"] != null) { StateID = dRow["StateID"].ToString(); }
                    if (dRow["AddressTypeID"] != null) { AddressTypeID = (Int32)dRow["AddressTypeID"]; }
                    if (dRow["PrimaryAddress"] != null) { IsPrimary = (bool)dRow["PrimaryAddress"]; }
                    if (dRow["PostalCode"] != null) { PostalCode = dRow["PostalCode"].ToString(); }
                    if (dRow["Country"] != null) { Country = dRow["Country"].ToString(); }
                    //if (ldt.Rows[0]["MDBStatesStateName"] != null) { _StateName = ldt.Rows[0]["MDBStatesStateName"].ToString(); } //Table Table MDBStates
                    DateTime dtTemp;
                    if (DateTime.TryParse(dRow["DateAdded"].ToString(), out dtTemp))
                        DateAdded = dtTemp;
                    if (DateTime.TryParse(dRow["DateChanged"].ToString(), out dtTemp))
                        DateChanged = dtTemp;
                    if (DateTime.TryParse(dRow["DateDeleted"].ToString(), out dtTemp))
                        DateDeleted = dtTemp;
                    GoogleString = Address1 + "+" + Address2 + "+" + City + "+" + StateID + "+" + PostalCode;
                }

                if (lds.Tables.Count > 1)
                {
                    AddressTypes = new List<cAddressType>();
                    foreach (DataRow dRow in lds.Tables[1].Rows)
                    {
                        cAddressType NewAddress = new cAddressType();
                        NewAddress.AddressTypeID = dRow["AddressTypeID"].ToString().ToInt32();
                        NewAddress.AddressType = dRow["AddressType"].ToString();
                        AddressTypes.Add(NewAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }

        }

        /// <summary>
        /// This method tells if the currect state of the record is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            strErrorDescription = string.Empty;

            if (string.IsNullOrWhiteSpace(Address1))
            {
                strErrorDescription = "Address 1 must be entered";
                return false;
            }

            if (string.IsNullOrWhiteSpace(City))
            {
                strErrorDescription = "City must be entered";
                return false;
            }

            if (string.IsNullOrWhiteSpace(PostalCode))
            {
                strErrorDescription = "Postal/Zip Code must be entered";
                return false;
            }

            if (isValidZipCode(PostalCode) == false)
            {
                strErrorDescription = (PostalCode ?? string.Empty) + " is not a valid Postal Code (please enter 5 numbers)";
                return false;
            }

            return true;
        }

        public static bool isValidZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                return false;

            zipCode = zipCode.Trim();
            //Make sure all values are digits
            if (zipCode.All(x => Char.IsDigit(x)) == false)
                return false;

            // Get all the digits from the string and make sure we have 5 numeric value
            return (zipCode.Length == 5);
        }

        /// <summary>
        /// Deletes, Updates, or Creates Address in the database
        /// </summary>
        /// <param name="option">String argument to tell the function what to do. u for Update, d for Delete, i for insert, defaults to update</param>
        /// <returns>True on success</returns>
        public Boolean SaveUpdate(int userID, bool delete = false)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Boolean bUpdateComplete = false;
            SortedList slParams = new SortedList();
            try
            {
                if (delete)
                {
                    slParams.Add("@RecordID", AddressID);
                    slParams.Add("@UserID", userID);
                    bUpdateComplete = cUtilities.PerformNonQueryBoolean("uspDelMDBAddresses", slParams, "LARPortal", UserName);
                }
                else
                {
                    slParams.Add("@UserID", userID);
                    slParams.Add("@AddressID", AddressID);
                    slParams.Add("@KeyID", userID);
                    slParams.Add("@KeyType", "cUser");
                    slParams.Add("@AddressTypeID", AddressTypeID);
                    slParams.Add("@PrimaryAddress", IsPrimary);
                    slParams.Add("@Address1", Address1);
                    slParams.Add("@Address2", Address2);
                    slParams.Add("@City", City);
                    slParams.Add("@StateID", StateID);
                    slParams.Add("@PostalCode", PostalCode);
                    slParams.Add("@Country", Country);
                    bUpdateComplete = cUtilities.PerformNonQueryBoolean("uspInsUpdMDBAddresses", slParams, "LARPortal", UserName);
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }

            return bUpdateComplete;

        }

    }
}