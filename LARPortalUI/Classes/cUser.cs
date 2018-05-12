using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Xml.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace LarpPortal.Classes
{
    public class cUser
    {
        public Int32 UserID { get; set; }
        public string LoginName { get; set; }
        public Int32 PrimaryEmailID { get; set; }
        public cEMail PrimaryEmailAddress  { get; set; }
        public List<cEMail> UserEmails { get; set; }
        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }
        public Int32 SecurityRoleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string NickName { get; set; }
        public string ForumUserName { get; set; }
        public string AuthorName { get; set; }
        public Int32 NotificationPreference { get; set; }
        public string NotificationPreferenceString { get; set; }
        public Int32 PrimaryAddressID { get; set; }
        public cAddress PrimaryAddress { get; set; }
        public List<cAddress> UserAddresses { get; set; }
        public Int32 PrimaryPhoneNumberID { get; set; }
        public cPhone PrimaryPhone { get; set; }
        public List<cPhone> UserPhones { get; set; }
        public Int32 DeliveryPreferenceID { get; set; }
        public string DeliveryPreferenceString { get; set; }
        public string LastLoggedInLocation { get; set; }
        //public cUserCampaign UserCampaign;
        public List<cUserCampaign> UserCampaigns { get; set; }
        public int LastLoggedInCampaign { get; set; }
        public int LastLoggedInCharacter { get; set; }              // JLB 7/11/2015 Added to save the last character that was saved.
        public string LastLoggedInMyCharOrCamp { get; set; }        // RGP - 5/27/2017
        public Int32 XRefNumber { get; set; }
        public string Comments { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateChanged { get; set; }
        public string SessionID { get; set; }

        //public cSecurity cUserSecurity;
        public cBank UserCPBank { get; set; }
        //public List<cNoifications> UserNotifications;
        //public cCalandar UserCalandar;
        //public List<LarpPortal.Classes.cCharacter> UserCharacters;


        private cUser()
        {
            UserID = -1;
            LoginName = "";
            PrimaryEmailID = -1;
            PrimaryEmailAddress = new cEMail();
            UserEmails = new List<cEMail>();
            LoginEmail = "";
            LoginPassword = "";
            SecurityRoleID = -1;
            FirstName = "";
            LastName = "";
            MiddleName = "";
            NickName = "";
            ForumUserName = "";
            AuthorName = "";
            NotificationPreference = -1;
            PrimaryAddressID = -1;
            PrimaryAddress = new cAddress();
            UserAddresses = new List<cAddress>();
            PrimaryPhoneNumberID = -1;
            PrimaryPhone = new cPhone();
            UserPhones = new List<cPhone>();
            DeliveryPreferenceID = -1;
            LastLoggedInLocation = "";
            UserCampaigns = new List<cUserCampaign>();
            LastLoggedInCampaign = 0;
            LastLoggedInCharacter = 0;
            LastLoggedInMyCharOrCamp = "";
            XRefNumber = -1;
            Comments = "";
            UserCPBank = new cBank();
        }

        public cUser(string strLoginName, string strLoginPassword, string sSessionID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			// JLB 5/12/2018 - Put in default values. It appeared as if the values were defaulting.
			UserID = -1;
			LoginName = "";
			PrimaryEmailID = -1;
			PrimaryEmailAddress = new cEMail();
			UserEmails = new List<cEMail>();
			LoginEmail = "";
			LoginPassword = "";
			SecurityRoleID = -1;
			FirstName = "";
			LastName = "";
			MiddleName = "";
			NickName = "";
			ForumUserName = "";
			AuthorName = "";
			NotificationPreference = -1;
			PrimaryAddressID = -1;
			PrimaryAddress = new cAddress();
			UserAddresses = new List<cAddress>();
			PrimaryPhoneNumberID = -1;
			PrimaryPhone = new cPhone();
			UserPhones = new List<cPhone>();
			DeliveryPreferenceID = -1;
			LastLoggedInLocation = "";
			UserCampaigns = new List<cUserCampaign>();
			LastLoggedInCampaign = 0;
			LastLoggedInCharacter = 0;
			LastLoggedInMyCharOrCamp = "";
			XRefNumber = -1;
			Comments = "";
			UserCPBank = new cBank();

			SortedList slParams = new SortedList();
            LoginName = strLoginName;
            LoginPassword = strLoginPassword;
            SessionID = sSessionID;

            slParams = new SortedList();
            slParams.Add("@LoginUserName", LoginName);
            try
            {
                DataSet ldt = cUtilities.LoadDataSet("uspGetUserByLoginName", slParams, "LARPortal", LoginName, lsRoutineName);
                ldt.Tables[0].TableName = "UserInfo";
                ldt.Tables[1].TableName = "AddressInfo";
                ldt.Tables[2].TableName = "AddressType";
                ldt.Tables[3].TableName = "PhoneNumber";
                ldt.Tables[4].TableName = "PhoneType";
                ldt.Tables[5].TableName = "PhoneProviders";

                if (ldt.Tables["UserInfo"].Rows.Count > 0)
                {
                    DataRow dUserInfo = ldt.Tables["UserInfo"].Rows[0];
                    UserID = dUserInfo["UserID"].ToString().ToInt32();
                    PrimaryEmailID = dUserInfo["EmailID"].ToString().ToInt32();
                    SecurityRoleID = dUserInfo["SecurityRoleID"].ToString().ToInt32();
                    FirstName = dUserInfo["FirstName"].ToString();
                    MiddleName = dUserInfo["MiddleName"].ToString();
                    LastName = dUserInfo["LastName"].ToString();
                    NickName = dUserInfo["NickName"].ToString();
                    PrimaryPhoneNumberID = dUserInfo["PrimaryPhoneID"].ToString().ToInt32();
                    PrimaryAddressID = dUserInfo["PrimaryAddressID"].ToString().ToInt32();
                    ForumUserName = dUserInfo["ForumUsername"].ToString();
                    NotificationPreference = dUserInfo["NotificationPreferenceID"].ToString().ToInt32();
                    DeliveryPreferenceID = dUserInfo["DeliveryPreferenceID"].ToString().ToInt32();
                    LastLoggedInLocation = dUserInfo["LastLoggedInLocation"].ToString();
                    LastLoggedInCampaign = dUserInfo["LastLoggedInCampaign"].ToString().ToInt32();

                    LastLoggedInCharacter = dUserInfo["LastLoggedInCharacter"].ToString().ToInt32();
                    LastLoggedInMyCharOrCamp = dUserInfo["LastLoggedInMyCharOrCamp"].ToString();

                    XRefNumber = dUserInfo["XRefNumber"].ToString().ToInt32();
                    DateAdded = Convert.ToDateTime(dUserInfo["DateAdded"].ToString());
                    DateChanged = Convert.ToDateTime(dUserInfo["DateChanged"].ToString());
                }

                LoadAddresses(ldt.Tables["AddressInfo"], ldt.Tables["AddressType"], strLoginName);
                LoadPhones(ldt.Tables["PhoneNumber"], ldt.Tables["PhoneType"], ldt.Tables["PhoneProviders"]);
                LoadEmails();
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, LoginName + lsRoutineName);
            }
        }

        private void LoadAddresses(DataTable AddressInfo, DataTable AddressType, string sLoginName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            // First create a list of address types. This will be copied into each record.
            List<cAddressType> AddTypes = new List<cAddressType>();
            foreach (DataRow dRow in AddressType.Rows)
            {
                cAddressType NewType = new cAddressType();
                NewType.AddressType = dRow["AddressType"].ToString();
                int iTemp;
                if (int.TryParse(dRow["AddressTypeID"].ToString(), out iTemp))
                    NewType.AddressTypeID = iTemp;
                AddTypes.Add(NewType);
            }

            UserAddresses = new List<cAddress>();
            PrimaryAddress = new cAddress();

            foreach (DataRow dRow in AddressInfo.Rows)
            {
                cAddress NewAdd = new cAddress();
                NewAdd.AddressTypes.AddRange(AddTypes);
                NewAdd.Address1 = dRow["Address1"].ToString();
                NewAdd.Address2 = dRow["Address2"].ToString();
                NewAdd.City = dRow["City"].ToString();
                NewAdd.StateID = dRow["StateID"].ToString();
                NewAdd.PostalCode = dRow["PostalCode"].ToString();
                NewAdd.Country = dRow["Country"].ToString();
                NewAdd.UserName = sLoginName;

                int iTemp;
                if (int.TryParse(dRow["AddressID"].ToString(), out iTemp))
                    NewAdd.AddressID = iTemp;
                if (int.TryParse(dRow["AddressTypeID"].ToString(), out iTemp))
                    NewAdd.AddressTypeID = iTemp;

                bool bTemp;
                if (bool.TryParse(dRow["PrimaryAddress"].ToString(), out bTemp))
                {
                    NewAdd.IsPrimary = true;
                    PrimaryAddress = NewAdd;
                }
                else
                    NewAdd.IsPrimary = false;

                UserAddresses.Add(NewAdd);
            }
        }

        private void LoadPhones(DataTable dtPhoneNumbers, DataTable dtPhoneTypes, DataTable dtPhoneProviders)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            List<cPhoneType> PhoneTypes = new List<cPhoneType>();
            foreach (DataRow dRow in dtPhoneTypes.Rows)
            {
                cPhoneType NewType = new cPhoneType();
                NewType.PhoneType = dRow["PhoneType"].ToString();
                int iTemp;
                if (int.TryParse(dRow["PhoneTypeID"].ToString(), out iTemp))
                    NewType.PhoneTypeID = iTemp;
                if (int.TryParse(dRow["SortOrder"].ToString(), out iTemp))
                    NewType.SortOrder = iTemp;
                PhoneTypes.Add(NewType);
            }

            List<cPhoneProvider> Providers = new List<cPhoneProvider>();
            foreach (DataRow dRow in dtPhoneProviders.Rows)
            {
                cPhoneProvider NewProv = new cPhoneProvider();
                NewProv.ProviderGateway = dRow["ProviderGateway"].ToString();
                NewProv.ProviderName = dRow["ProviderName"].ToString();
                int iTemp;
                if (int.TryParse(dRow["PhoneProviderID"].ToString(), out iTemp))
                    NewProv.PhoneProviderID = iTemp;
                if (int.TryParse(dRow["SortOrder"].ToString(), out iTemp))
                    NewProv.SortOrder = iTemp;
                Providers.Add(NewProv);
            }

            UserPhones = new List<cPhone>();

            foreach (DataRow dRow in dtPhoneNumbers.Rows)
            {
                cPhone NewPhone = new cPhone
                {
                    IDD = dRow["IDD"].ToString(),
                    CountryCode = dRow["CountryCode"].ToString(),
                    AreaCode = dRow["AreaCode"].ToString(),
                    PhoneNumber = dRow["PhoneNumber"].ToString(),
                    Extension = dRow["Extension"].ToString(),
                    Comments = dRow["Comments"].ToString()
                };

                NewPhone.PhoneTypes = PhoneTypes;
                NewPhone.ProviderList = Providers;

                int iTemp;
                if (int.TryParse(dRow["PhoneNumberID"].ToString(), out iTemp))
                    NewPhone.PhoneNumberID = iTemp;
                if (int.TryParse(dRow["PhoneTypeID"].ToString(), out iTemp))
                    NewPhone.PhoneTypeID = iTemp;
				if (int.TryParse(dRow ["ProviderID"].ToString(), out iTemp))
					NewPhone.ProviderID = iTemp;
                bool bTemp;
                NewPhone.IsPrimary = false;
                if (bool.TryParse(dRow["PrimaryPhone"].ToString(), out bTemp))
                {
                    if (bTemp)
                    {
                        NewPhone.IsPrimary = true;
                        PrimaryPhone = NewPhone;
                        PrimaryPhoneNumberID = NewPhone.PhoneNumberID;
                    }
                }
                UserPhones.Add(NewPhone);
            }
        }

        private void LoadEmails()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            try
            {
                PrimaryEmailAddress = new cEMail(PrimaryEmailID, LoginName, UserID);
                UserEmails = new List<cEMail>();

                SortedList slParams = new SortedList();
                slParams.Add("@intKeyID", UserID);
                slParams.Add("@strKeyType", "MDBUsers");

                DataTable ldt = cUtilities.LoadDataTable("uspGetEmailsByKeyInfo", slParams, "LARPortal", LoginName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    foreach (DataRow ldr in ldt.Rows)
                    {
                        cEMail cPh = new cEMail(ldr["EmailID"].ToString().Trim().ToInt32(), LoginName, UserID);
                        UserEmails.Add(cPh);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, LoginName + lsRoutineName);
            }
        }

        public void SetCampaignForCharacter(int UserID, int CharacterID)
        {
            int iTemp = 0;
            string stStoredProc = "uspGetCampaignForCharacter";
            string stCallingMethod = "cUser.SetCampaignsForCharacter";
            SortedList slParams = new SortedList();
            DataTable dtCampaign = new DataTable();
            slParams.Add("@CharacterID", CharacterID);
            dtCampaign = cUtilities.LoadDataTable(stStoredProc, slParams, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow drow in dtCampaign.Rows)
            {
                if (int.TryParse(drow["CampaignID"].ToString(), out iTemp))
                {
                    if (iTemp != 0)
                        LastLoggedInCampaign = iTemp;
                }
            }
        }

        public void SetCharacterForCampaignUser(int UserID, int CampaignID)
        {
            int iTemp = 0;
            string stStoredProc = "uspGetCharacterForCampaignUser";
            string stCallingMethod = "cUser.SetCharacterForCampaignUser";
            SortedList slParams = new SortedList();
            DataTable dtCharacters = new DataTable();
            slParams.Add("@UserID", UserID);
            slParams.Add("@CampaignID", CampaignID);
            dtCharacters = cUtilities.LoadDataTable(stStoredProc, slParams, "LARPortal", UserID.ToString(), stCallingMethod);
            foreach (DataRow drow in dtCharacters.Rows)
            {
                if (int.TryParse(drow["CharacterID"].ToString(), out iTemp))
                {
                    if (iTemp != 0)
                    {
                        LastLoggedInCharacter = iTemp;
                    }
                }
            }
        }

        public Boolean Save()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            Boolean blnReturn = false;
            try
            {
                string stStoredProc = "uspInsUpdMDBUsers";
                SortedList slParams = new SortedList();
                slParams.Add("@UserID", UserID);
                slParams.Add("@LoginUsername", LoginName);
                slParams.Add("@EmailID", PrimaryEmailID);
                slParams.Add("@SecurityRoleID", SecurityRoleID);
                slParams.Add("@FirstName", FirstName);
                slParams.Add("@MiddleName", MiddleName);
                slParams.Add("@LastName", LastName);
                slParams.Add("@Nickname", NickName);
                slParams.Add("@PrimaryPhoneID", PrimaryPhoneNumberID);
                slParams.Add("@PrimaryAddressID", PrimaryAddressID);
                slParams.Add("@ForumUsername", ForumUserName);
                slParams.Add("@NotificationPreferenceID", NotificationPreference);
                slParams.Add("@DeliveryPreferenceID", DeliveryPreferenceID);

                // TODO: Fix the LastLoggedInLocation
                if (LastLoggedInLocation == null)
                    LastLoggedInLocation = "MemberDemographics.aspx";
                //slParams.Add("@LastLoggedInLocation", LastLoggedInLocation);
                //if (_LastLoggedInCampaign == null)
                //    _LastLoggedInCampaign = 0;
                slParams.Add("@LastLoggedInCampaign", LastLoggedInCampaign);
                slParams.Add("@LastLoggedInCharacter", LastLoggedInCharacter);     //JLB 07/11/2015 Save the last character selected.
                slParams.Add("@LastLoggedInMyCharOrCamp", LastLoggedInMyCharOrCamp);   //RGP 5/27/2017
                slParams.Add("@XRefNumber", XRefNumber);
                slParams.Add("@Comments", Comments);
                slParams.Add("@LogonPassword", LoginPassword);
                if (LoginEmail == null)
                    LoginEmail = "";
                slParams.Add("@EmailAddress", LoginEmail);
                blnReturn = cUtilities.PerformNonQueryBoolean(stStoredProc, slParams, "LARPortal", LoginName);

                blnReturn = true;
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, LoginName + lsRoutineName);
                blnReturn = false;
            }
            return blnReturn;
        }
    }
}