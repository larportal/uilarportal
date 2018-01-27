using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;

namespace LarpPortal.Classes
{
    public class cAdminViews
    {
        //public List<cPageTab> lsPageTabs = new List<cPageTab>();
        //public string SecurityResetCode { get; set; }
        //public DateTime SecurityLockoutDate { get; set; }
        //public int UserSecurityID { get; set; }
        public int UserID { get; set; }
        public DateTime LoginEST { get; set; }
        public string Player { get; set; }
        public string UsernameUsed { get; set; }
        public string PasswordUsed { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string IPAddress { get; set; }
        public int LoginAuditCount { get; set; }
        public int CharacterCount { get; set; }
        public int CharacterSkillCount { get; set; }
        public string CharacterAKA { get; set; }
        public string TeamName { get; set; }
        public string CharacterFirstName { get; set; }
        public string CharacterMiddleName { get; set; }
        public string CharacterLastName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string WhereFrom { get; set; }
        public string CurrentHome { get; set; }
        public string PlayerComments { get; set; }
        public string World { get; set; }
        public string DescriptorValue { get; set; }
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public int PlotLeadPerson { get; set; }
        public int CharacterSkillSetID { get; set; }
        public int SkillTypeID { get; set; }
        public int SkillTypeID2 { get; set; }
        public double TotalCP { get; set; }
        public string AttributeDesc { get; set; }
        public string OrderOrigin { get; set; }
        public int RegistrationID { get; set; }
        public string EventName { get; set;}
        public string DateRegistered { get; set;}
        public string OrderName { get; set;}
        public string Description { get; set;}
        public string EmailAddress { get; set;}
        public string PlayerCommentsToStaff { get; set;}
        public int RegCount { get; set; }
 
        /// <summary>
        /// This will get all user logins for the past 30 days
        /// May eventually make it day parameter passable but defaulting 30 for now
        /// </summary>
        public DataTable GetLoginAudit()
        {
            LoginAuditCount = 0;
            int iTemp = 0;
            DateTime dtTemp;
            string stStoredProc = "uspAdminGetLoginAuditLog";
            string stCallingMethod = "cAdminViews.GetLoginAudit";
            SortedList slParameters = new SortedList();
            DataTable dtLoginAudit = new DataTable();
            DataSet dsLoginAudit = new DataSet();
            dtLoginAudit = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", "Usename", stCallingMethod);
            foreach (DataRow dRow in dtLoginAudit.Rows)
            {
                if (int.TryParse(dRow["UserID"].ToString(), out iTemp))
                    UserID = iTemp;
                if (DateTime.TryParse(dRow["LoginEST"].ToString(), out dtTemp))
                    LoginEST = dtTemp;
                Player = dRow["Player"].ToString();
                UsernameUsed = dRow["UsernameUsed"].ToString();
                PasswordUsed = dRow["PasswordUsed"].ToString();
                Browser = dRow["Browser"].ToString();
                BrowserVersion = dRow["BrowserVersion"].ToString();
                OS = dRow["OS"].ToString();
                OSVersion = dRow["OSVersion"].ToString();
                IPAddress = dRow["IPAddress"].ToString();
                LoginAuditCount++;
            }
            return dtLoginAudit;
        }

        /// <summary>
        /// This will return a list of all Fifth Gate Characters
        /// </summary>
        public DataTable FifthGateCharacterList()
        {
            CharacterCount = 0;
            int iTemp = 0;
            double dTemp;
            string stStoredProc = "uspAdminFifthGateCharacterList";
            string stCallingMethod = "cAdminViews.FifthGateCharacterList";
            SortedList slParameters = new SortedList();
            DataTable dtCharacters = new DataTable();
            DataSet dsCharacters = new DataSet();
            dtCharacters = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", "Username", stCallingMethod);
            foreach (DataRow dRow in dtCharacters.Rows)
            {
                if (int.TryParse(dRow["PlotLeadPerson"].ToString(), out iTemp))
                    PlotLeadPerson = iTemp;
                if (int.TryParse(dRow["CharacterSkillSetID"].ToString(), out iTemp))
                    CharacterSkillSetID = iTemp;
                if (double.TryParse(dRow["TotalCP"].ToString(), out dTemp))
                    TotalCP = dTemp;
                CharacterAKA = dRow["CharacterAKA"].ToString();
                TeamName = dRow["TeamName"].ToString();
                CharacterFirstName = dRow["CharacterFirstName"].ToString();
                CharacterMiddleName = dRow["CharacterMiddleName"].ToString();
                CharacterLastName = dRow["CharacterLastName"].ToString();
                FirstName = dRow["FirstName"].ToString();
                LastName = dRow["LastName"].ToString();
                DateOfBirth = dRow["DateOfBirth"].ToString();
                WhereFrom = dRow["WhereFrom"].ToString();
                CurrentHome = dRow["CurrentHome"].ToString();
                PlayerComments = dRow["PlayerComments"].ToString();
                CharacterCount++;
            }
            return dtCharacters;
        }

        /// <summary>
        /// This will return a list of all Fifth Gate Registrations
        /// </summary>
        public DataTable FifthGateRegistrations(int EventID, int SkillTypeID)
        {
            RegCount = 0;
            int iTemp = 0;
            string stStoredProc = "uspAdminFifthGateRegistrations";
            string stCallingMethod = "cAdminViews.FifthGateRegistrations";
            SortedList slParameters = new SortedList();
            slParameters.Add("@EventID", EventID);  // 512 SilverFire - 530 Wrathborn
            slParameters.Add("@SkillTypeID", SkillTypeID);   // 24 Orders - 25 Origins
            DataTable dtRegistrations = new DataTable();
            DataSet dsRegistrations = new DataSet();
            dtRegistrations = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", "Username", stCallingMethod);
            foreach (DataRow dRow in dtRegistrations.Rows)
            {
                if (int.TryParse(dRow["RegistrationID"].ToString(), out iTemp))
                    RegistrationID = iTemp;
                EventName = dRow["EventName"].ToString();
                DateRegistered = dRow["DateRegistered"].ToString();
                CharacterAKA = dRow["CharacterAKA"].ToString();
                OrderName = dRow["OrderName"].ToString();
                TeamName = dRow["TeamName"].ToString();
                FirstName = dRow["FirstName"].ToString();
                LastName = dRow["LastName"].ToString();
                Description = dRow["Description"].ToString();
                EmailAddress = dRow["EmailAddress"].ToString();
                PlayerCommentsToStaff = dRow["PlayerCommentsToStaff"].ToString();
              
                RegCount++;
            }
            return dtRegistrations;
        }

        /// <summary>
        /// This will return a list of all Fifth Gate Character Skill
        /// </summary>
        public DataTable FifthGateCharacterSkillList()
        {
            CharacterSkillCount = 0;
            int iTemp = 0;
            string stStoredProc = "uspAdminFifthGateCharacterSkillList";
            string stCallingMethod = "cAdminViews.FifthGateCharacterSkillList";
            SortedList slParameters = new SortedList();
            DataTable dtCharacterSkills = new DataTable();
            DataSet dsCharacterSkills = new DataSet();
            dtCharacterSkills = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", "Username", stCallingMethod);
            foreach (DataRow dRow in dtCharacterSkills.Rows)
            {
                if (int.TryParse(dRow["SkillTypeID"].ToString(), out iTemp))
                    SkillTypeID = iTemp;
                if (int.TryParse(dRow["SkillID"].ToString(), out iTemp))
                    SkillID = iTemp;
                CharacterAKA = dRow["CharacterAKA"].ToString();
                FirstName = dRow["FirstName"].ToString();
                LastName = dRow["LastName"].ToString();
                World = dRow["World"].ToString();
                DescriptorValue = dRow["DescriptorValue"].ToString();
                SkillName = dRow["SkillName"].ToString();
                AttributeDesc = dRow["AttributeDesc"].ToString();
                OrderOrigin = dRow["OrderOrigin"].ToString();
                TeamName = dRow["Team"].ToString();
                CharacterSkillCount++;
            }
            return dtCharacterSkills;
        }

        /// <summary>
        /// This will return a list of all Madrigal Characters
        /// </summary>
        public DataTable MadrigalCharacterList()
        {
            CharacterCount = 0;
            int iTemp = 0;
            double dTemp;
            string stStoredProc = "uspAdminMadrigalCharacterList";
            string stCallingMethod = "cAdminViews.MadrigalCharacterList";
            SortedList slParameters = new SortedList();
            DataTable dtCharacters = new DataTable();
            DataSet dsCharacters = new DataSet();
            dtCharacters = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", "Username", stCallingMethod);
            foreach (DataRow dRow in dtCharacters.Rows)
            {
                if (int.TryParse(dRow["PlotLeadPerson"].ToString(), out iTemp))
                    PlotLeadPerson = iTemp;
                if (int.TryParse(dRow["CharacterSkillSetID"].ToString(), out iTemp))
                    CharacterSkillSetID = iTemp;
                if (double.TryParse(dRow["TotalCP"].ToString(), out dTemp))
                    TotalCP = dTemp;
                CharacterAKA = dRow["CharacterAKA"].ToString();
                TeamName = dRow["TeamName"].ToString();
                CharacterFirstName = dRow["CharacterFirstName"].ToString();
                CharacterMiddleName = dRow["CharacterMiddleName"].ToString();
                CharacterLastName = dRow["CharacterLastName"].ToString();
                FirstName = dRow["FirstName"].ToString();
                LastName = dRow["LastName"].ToString();
                DateOfBirth = dRow["DateOfBirth"].ToString();
                WhereFrom = dRow["Country"].ToString();
                CurrentHome = dRow["CurrentHome"].ToString();
                PlayerComments = dRow["PlayerComments"].ToString();
                CharacterCount++;
            }
            return dtCharacters;
        }

        /// <summary>
        /// This will return a list of all Madrigal Character Skill
        /// </summary>
        public DataTable MadrigalCharacterSkillList()
        {
            CharacterSkillCount = 0;
            int iTemp = 0;
            string stStoredProc = "uspAdminMadrigalCharacterSkillList";
            string stCallingMethod = "cAdminViews.MadrigalCharacterSkillList";
            SortedList slParameters = new SortedList();
            DataTable dtCharacterSkills = new DataTable();
            DataSet dsCharacterSkills = new DataSet();
            dtCharacterSkills = cUtilities.LoadDataTable(stStoredProc, slParameters, "LARPortal", "Username", stCallingMethod);
            foreach (DataRow dRow in dtCharacterSkills.Rows)
            {
                if (int.TryParse(dRow["SkillTypeID"].ToString(), out iTemp))
                    SkillTypeID = iTemp;
                if (int.TryParse(dRow["SkillID"].ToString(), out iTemp))
                    SkillID = iTemp;
                CharacterAKA = dRow["CharacterAKA"].ToString();
                FirstName = dRow["FirstName"].ToString();
                LastName = dRow["LastName"].ToString();
                World = dRow["Country"].ToString();
                DescriptorValue = dRow["DescriptorValue"].ToString();
                SkillName = dRow["SkillName"].ToString();
                AttributeDesc = dRow["AttributeDesc"].ToString();
                OrderOrigin = dRow["OrderOrigin"].ToString();
                TeamName = dRow["Team"].ToString();
                CharacterSkillCount++;
            }
            return dtCharacterSkills;
        }

    }
}


