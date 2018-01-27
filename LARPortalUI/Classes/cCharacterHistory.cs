using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Reflection;

using LarpPortal.Classes;

namespace LarpPortal.Classes
{
    public class cCharacterHistoryAddendum
    {
        public int AddendumID { get; set; }
        public string Addendum { get; set; }
        public int CharacterID { get; set; }
        public DateTime DateAdded { get; set; }

        public cCharacterHistoryAddendum(int cCharacterID)
        {
            AddendumID = -1;
            CharacterID = cCharacterID;
        }

        public void Delete(string UserName, int UserID)
        {
            SortedList sParam = new SortedList();

            sParam.Add("@UserID", UserID);
            sParam.Add("@CharacterHistoryAddendumID", AddendumID);
            cUtilities.PerformNonQuery("uspDelCHCharacterHistoryAddendums", sParam, "LARPortal", UserName);
        }

        public void Save(string UserName, int UserID)
        {
            SortedList sParam = new SortedList();
            sParam.Add("@CharacterHistoryAddendumID", AddendumID);
            sParam.Add("@Addendum", Addendum);
            sParam.Add("@CharacterID", CharacterID);
            sParam.Add("@UserID", UserID);
            cUtilities.PerformNonQuery("uspInpUpsCHCharacterHistoryAddendums", sParam, "LARPortal", UserName);
        }
    }

    public class cCharacterHistoryAddendumsStaffComments
    {
        public int AddendumsStaffCommentsID { get; set; }
        public int CommenterID { get; set; }
        public int CharacterID { get; set; }
        public string StaffComments { get; set; }
        public DateTime DateAdded { get; set; }

        public cCharacterHistoryAddendumsStaffComments(int cCharacterID)
        {
            AddendumsStaffCommentsID = -1;
            CharacterID = cCharacterID;
        }

        public void Delete(string UserName, int UserID)
        {
            SortedList sParam = new SortedList();

            sParam.Add("@UserID", UserID);
            sParam.Add("@CharacterHistoryAddendumsStaffCommentsID", AddendumsStaffCommentsID);
            cUtilities.PerformNonQuery("uspDelCHCharacterHistoryAddendumsStaffComments", sParam, "LARPortal", UserName);
        }

        public void Save(string UserName, int UserID)
        {
            SortedList sParam = new SortedList();
            sParam.Add("@CharacterHistoryAddendumsStaffCommentsID", AddendumsStaffCommentsID);
            sParam.Add("@CommenterID", CommenterID);
            sParam.Add("@StaffComments", StaffComments);
            sParam.Add("@CharacterID", CharacterID);
            sParam.Add("@UserID", UserID);
            cUtilities.PerformNonQuery("uspInsUpdCHCharacterHistoryAddendumsStaffComments", sParam, "LARPortal", UserName);
        }
    }

    public class cCharacterHistoryStaffComments
    {
        public int StaffCommentID { get; set; }
        public int CharacterID { get; set; }
        public int CommenterID { get; set; }
        public string StaffComments { get; set; }
        public DateTime DateAdded { get; set; }

        public cCharacterHistoryStaffComments(int cCharacterID)
        {
            StaffCommentID = -1;
            CharacterID = cCharacterID;
        }

        public void Delete(string UserName, int UserID)
        {
            SortedList sParam = new SortedList();
            sParam.Add("@UserID", UserID);
            sParam.Add("@CharacterHistoryStaffCommentID", StaffCommentID);
            cUtilities.PerformNonQuery("uspDelCHCharacterHistoryStaffComments", sParam, "LARPortal", UserName);
        }

        public void Save(string UserName, int UserID)
        {
            SortedList sParam = new SortedList();
            sParam.Add("@CharacterHistoryStaffCommentID", StaffCommentID);
            sParam.Add("@CommenterID", CommenterID);
            sParam.Add("@StaffComments", StaffComments);
            sParam.Add("@CharacterID", CharacterID);
            sParam.Add("@UserID", UserID);
            cUtilities.PerformNonQuery("uspInsUpdCHCharacterHistoryStaffComments", sParam, "LARPortal", UserName);
        }
    }

    public class cCharacterHistory
    {
        public int CharacterID { get; set; }
        public string History { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateApproved { get; set; }
        public bool ClearSubmitted { get; set; }
        public bool ClearApproved { get; set; }
        public string NotificationEMail { get; set; }
        public string PlayerName { get; set; }
        public string EmailAddress { get; set; }
        public string CampaignName { get; set; }
        public string CharacterAKA { get; set; }
        public string CharacterFirstName { get; set; }
        public string CharacterMiddleName { get; set; }
        public string CharacterLastName { get; set; }
        public double CPAwarded { get; set; }
        public double CPEarn { get; set; }
        public int CampaignPlayerID { get; set; }

        public List<cCharacterHistoryAddendum> Addendums = new List<cCharacterHistoryAddendum>();
        public List<cCharacterHistoryAddendumsStaffComments> AddendumStaffComments = new List<cCharacterHistoryAddendumsStaffComments>();
        public List<cCharacterHistoryStaffComments> StaffComments = new List<cCharacterHistoryStaffComments>();

        public cCharacterHistory()
        {
            ClearSubmitted = false;
            ClearApproved = false;
            NotificationEMail = "support@LARPortal.com";
        }

        public void Load(int iCharacterID, int UserID = -1)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            sParam.Add("@CharacterID", iCharacterID);

            int iTemp;
            DateTime dtTemp;
            double dTemp = 0.0;

            DataSet dsHistory = cUtilities.LoadDataSet("uspGetCharacterHistory", sParam, "LARPortal", UserID.ToString(), lsRoutineName);

            foreach (DataRow dRow in dsHistory.Tables[0].Rows)
            {
                History = dRow["CharacterHistory"].ToString();
                CharacterID = iCharacterID;
                if (DateTime.TryParse(dRow["DateHistorySubmitted"].ToString(), out dtTemp))
                    DateSubmitted = dtTemp;
                if (DateTime.TryParse(dRow["DateHistoryApproved"].ToString(), out dtTemp))
                    DateApproved = dtTemp;
                if (dRow["CharacterHistoryNotificationEmail"].ToString().Length > 0)
                    NotificationEMail = dRow["CharacterHistoryNotificationEmail"].ToString().Trim();
                PlayerName = dRow["PlayerName"].ToString();
                EmailAddress = dRow["EmailAddress"].ToString();
                CampaignName = dRow["CampaignName"].ToString();

                CharacterAKA = dRow["CharacterAKA"].ToString();
                CharacterFirstName = dRow["CharacterFirstName"].ToString();
                CharacterMiddleName = dRow["CharacterMiddleName"].ToString();
                CharacterLastName = dRow["CharacterLastName"].ToString();

                if (int.TryParse(dRow["CampaignPlayerID"].ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
                else
                    CampaignPlayerID = 0;

                if (double.TryParse(dRow["CPAwarded"].ToString(), out dTemp))
                    CPAwarded = dTemp;
                else
                    CPAwarded = 0;

                if (double.TryParse(dRow["CPEarn"].ToString(), out dTemp))
                    CPEarn = dTemp;
                else
                    CPEarn = 0;
            }

            if (dsHistory.Tables.Count >= 2)
            {
                foreach (DataRow dRow in dsHistory.Tables[1].Rows)
                {
                    cCharacterHistoryStaffComments NewStaffComment = new cCharacterHistoryStaffComments(iCharacterID);
                    if (int.TryParse(dRow["CharacterHistoryStaffCommentID"].ToString(), out iTemp))
                        NewStaffComment.StaffCommentID = iTemp;
                    if (int.TryParse(dRow["CommenterID"].ToString(), out iTemp))
                        NewStaffComment.CommenterID = iTemp;
                    NewStaffComment.StaffComments = dRow["StaffComments"].ToString();
                    if (DateTime.TryParse(dRow["DateAdded"].ToString(), out dtTemp))
                        NewStaffComment.DateAdded = dtTemp;

                    StaffComments.Add(NewStaffComment);
                }

                if (dsHistory.Tables.Count >= 3)
                {
                    foreach (DataRow dRow in dsHistory.Tables[2].Rows)
                    {
                        cCharacterHistoryAddendum NewAddendum = new cCharacterHistoryAddendum(iCharacterID);
                        if (int.TryParse(dRow["CharacterHistoryAddendumID"].ToString(), out iTemp))
                            NewAddendum.AddendumID = iTemp;
                        NewAddendum.Addendum = dRow["Addendum"].ToString();
                        if (DateTime.TryParse(dRow["DateAdded"].ToString(), out dtTemp))
                            NewAddendum.DateAdded = dtTemp;
                        Addendums.Add(NewAddendum);
                    }

                    if (dsHistory.Tables.Count >= 4)
                    {
                        foreach (DataRow dRow in dsHistory.Tables[3].Rows)
                        {
                            cCharacterHistoryAddendumsStaffComments NewAddendumStaff = new cCharacterHistoryAddendumsStaffComments(iCharacterID);
                            if (int.TryParse(dRow["CharacterHistoryAddendumsStaffCommentsID"].ToString(), out iTemp))
                                NewAddendumStaff.AddendumsStaffCommentsID = iTemp;
                            if (int.TryParse(dRow["CommenterID"].ToString(), out iTemp))
                                NewAddendumStaff.CommenterID = iTemp;
                            NewAddendumStaff.StaffComments = dRow["StaffComments"].ToString();
                            if (DateTime.TryParse(dRow["DateAdded"].ToString(), out dtTemp))
                                NewAddendumStaff.DateAdded = dtTemp;
                            AddendumStaffComments.Add(NewAddendumStaff);
                        }
                    }
                }
            }
        }

        public void Save(int CharacterID, int UserID, string UserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParam = new SortedList();
            sParam.Add("@CharacterID", CharacterID);
            sParam.Add("@UserID", UserID);
            sParam.Add("@CharacterHistory", History);
            if (ClearSubmitted)
                sParam.Add("@ClearHistorySubmitted", true);
            else
                sParam.Add("@DateHistorySubmitted", DateSubmitted);
            if (ClearApproved)
                sParam.Add("@ClearHistoryApproved", true);
            else
                sParam.Add("@DateHistoryApproved", DateApproved);

            cUtilities.PerformNonQuery("uspInsUpdCHCharacters", sParam, "LARPortal", UserName);

            foreach (cCharacterHistoryAddendum obj in Addendums)
                obj.Save(UserName, UserID);

            foreach (cCharacterHistoryStaffComments obj in StaffComments)
                obj.Save(UserName, UserID);

            foreach (cCharacterHistoryAddendumsStaffComments obj in AddendumStaffComments)
                obj.Save(UserName, UserID);

        }
    }
}