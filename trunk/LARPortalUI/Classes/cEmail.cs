using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;


namespace LarpPortal.Classes
{
    [Serializable]
    public class cEmailType
    {
        public int EmailTypeID { get; set; }
        public string EmailType { get; set; }
    }

    [Serializable()]
    public class cEMail
    {

        private Int32 _EMailID = -1;
        private string _EMailAddress = "";
        private string _Comments = "";
        private DateTime _DateAdded = DateTime.Now;
        private DateTime _DateChanged = DateTime.Now;
        private DateTime? _DateDeleted = null;
        private string _UserName = "";
        private Int32 _UserID = -1;


        // ID for the EMail Address
        public Int32 EMailID
        {
            get { return _EMailID; }
            set { _EMailID = value; }
        }
        
        //Email Address
        public string EmailAddress
        {
            get { return _EMailAddress; }
            set { _EMailAddress = value; }
        }

        public int EmailTypeID { get; set; }

        public bool IsPrimary { get; set; }

        // Comments about the email address
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        //Date Email Address Added
        public DateTime DateAdded
        {
            get { return _DateAdded; }
            //set { _DateAdded = value; }
        }
        //Date Email Address Changed
        public DateTime DateChanged
        {
            get { return _DateChanged; }
            //set { _DateChanged = value; }
        }
        // Date Email Address Flagged as Deleted, null vlaue if not deleted
        public DateTime? DateDeleted
        {
            get { return _DateDeleted; }
            //set { _DateDeleted = value; }
        }

        public List<cEmailType> EmailTypes = new List<cEmailType>();

        public string EmailType
        {
            get
            {
                if (EmailTypes.Count > 0)
                {
                    var Rec = EmailTypes.Find(x => x.EmailTypeID == EmailTypeID);
                    if (Rec != null)
                        return Rec.EmailType;
                    else
                        return "";
                }
                else
                    return "";
            }
        }

        public static string ErrorMessage { get; private set; }

        public string strErrorMessage { get; set; }

        public cEMail()
        {
            EMailID = -1;

            SortedList sParams = new SortedList();

            DataTable dtEmailTypes = cUtilities.LoadDataTable("uspGetEmailTypes", sParams, "LARPortal", "", "cEmail.Creator");

            EmailTypes = new List<cEmailType>();
            if (new DataView(dtEmailTypes, "EmailTypeID = 0", "", DataViewRowState.CurrentRows).Count == 0)
            {
                cEmailType NewEmailType = new cEmailType();
                NewEmailType.EmailTypeID = 0;
                NewEmailType.EmailType = "Choose...";
                EmailTypes.Add(NewEmailType);
            }

            foreach (DataRow dRow in dtEmailTypes.Rows)
            {
                cEmailType NewEmailType = new cEmailType();
                NewEmailType.EmailTypeID = dRow["EmailTypeID"].ToString().ToInt32();
                NewEmailType.EmailType = dRow["EmailType"].ToString();
                EmailTypes.Add(NewEmailType);
            }

            EmailTypeID = 0;
        }

        // Passing an ID of -1 should produce the devault values, and on update should generate a new record. 
        public cEMail(Int32 intEMailID, string strUserName, Int32 intUserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            _UserName = strUserName;
            _UserID = intUserID;
            _EMailID = intEMailID;
            try
            {
                SortedList slParams = new SortedList();
                slParams.Add("@intEMailID", intEMailID);
                
                // need to put the correct stored procedure in to the class
                DataSet lds = cUtilities.LoadDataSet("uspGetEmailData", slParams, "LARPortal", strUserName, lsRoutineName);
                if (lds.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = lds.Tables[0].Rows[0];
                    _EMailID = dRow["EMailID"].ToString().ToInt32();

                    // Now checking for both DBNull and null. Added the bool.TryParse because I'm OCD.      JBradshaw 9/14/2015
                    bool bTemp;
                    if (dRow["PrimaryEmail"] != DBNull.Value)
                        if (dRow["PrimaryEmail"] != null)
                            if (bool.TryParse(dRow["PrimaryEmail"].ToString(), out bTemp))
                                IsPrimary = bTemp;

                    // Now checking for both DBNull and null. Added the int.TryParse because I'm OCD.      JBradshaw 9/14/2015
                    int iTemp;
                    if (dRow["EmailTypeID"] != DBNull.Value)
                        if (dRow["EmailTypeID"] != null)
                            if (int.TryParse(dRow["EmailTypeID"].ToString(), out iTemp))
                                EmailTypeID = iTemp;

                    _EMailAddress = dRow["EMailAddress"].ToString().Trim();
                    _Comments = dRow["Comments"].ToString().Trim();
                    _DateAdded = Convert.ToDateTime(dRow["DateAdded"].ToString());
                    _DateChanged = Convert.ToDateTime(dRow["DateChanged"].ToString());
                    if (dRow["DateDeleted"] == DBNull.Value)
                    {
                        _DateDeleted = null;
                    }
                    else
                    {
                        _DateDeleted = Convert.ToDateTime(dRow["DateDeleted"].ToString());
                    }
                }
                if (lds.Tables.Count > 1)
                {
                    EmailTypes = new List<cEmailType>();
                    foreach (DataRow dRow in lds.Tables[1].Rows)
                    {
                        cEmailType NewEmailType = new cEmailType();
                        NewEmailType.EmailTypeID = dRow["EmailTypeID"].ToString().ToInt32();
                        NewEmailType.EmailType = dRow["EmailType"].ToString();
                        EmailTypes.Add(NewEmailType);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, _UserName + lsRoutineName);
            }
        }

        public Boolean SaveUpdate(int userID, bool delete = false)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Boolean blnReturn = false;
            try
            {
                SortedList slParams = new SortedList();
                if (delete)
                {
                    slParams.Add("@RecordID", EMailID);
                    slParams.Add("@UserID", userID);
                    blnReturn = cUtilities.PerformNonQueryBoolean("uspDelMDBEmails", slParams, "LARPortal", _UserName + string.Empty);
                }
                else
                {
                    slParams.Add("@UserID", userID);
                    slParams.Add("@EmailID", _EMailID);
                    slParams.Add("@KeyID", userID);
                    slParams.Add("@KeyType", "MDBUsers");
                    slParams.Add("@EmailTypeID", EmailTypeID);
                    slParams.Add("@EmailAddress", _EMailAddress);
                    slParams.Add("@PrimaryEmail", IsPrimary);
                    slParams.Add("@Comments", _Comments);
                    blnReturn = cUtilities.PerformNonQueryBoolean("uspInsUpdMDBEmails", slParams, "LARPortal", _UserName);
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

        public static bool isValidEmail(string email)
        {
            ErrorMessage = string.Empty;

            email = (email + string.Empty).Trim(); //If null set as empty string

            bool isValid = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);

            if (isValid == false)
                ErrorMessage = email + " is not a valid Email";

            return isValid;
        }

        public bool IsValid()
        {
            strErrorMessage = string.Empty;

            if (isValidEmail(EmailAddress) == false)
            {
                strErrorMessage = (EmailAddress + string.Empty).Trim() + " is not a valid Email Address";
                return false;
            }

            if (EmailTypeID < 1)
            {
                strErrorMessage = "Email Type is not a valid type";
                return false;
            }

            return true;
        }
    }
}