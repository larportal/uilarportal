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
    public class cSiteComment
    {
        private Int32 _SiteCommentID = -1;
        private Int32 _SiteID = -1;
        private Int32 _AddedBy = -1;
        private Int32 _CampaignID = -1;
        private string _SiteComments = "";
        private string _Comments = "";
        private DateTime? _DateAdded;
        private DateTime? _DateChanged;
        private Int32 _UserID = -1;
        private string _UserName = "";

        public Int32 SiteCommentID
        {
            get { return _SiteCommentID; }
            set { _SiteCommentID = value; }
        }
        public Int32 SiteID
        {
            get { return _SiteID; }
            set { _SiteID = value; }
        }
        public Int32 AddedBy 
        {
            get { return _AddedBy; }
            set { _AddedBy = value; }
        }
        public Int32 CampaignID
        {
            get { return _CampaignID; }
            set { _CampaignID = value;}
        }
        public string SiteComments
        {
            get { return _SiteComments; }
            set { _SiteComments = value; }
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

        private cSiteComment()
        {

        }

        public cSiteComment(Int32 intSiteCommentID, Int32 intUserID, string strUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            _SiteCommentID = intSiteCommentID;
            _UserID = intUserID;
            _UserName = strUserName;
            //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
            SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
            slParams.Add("@SiteCommentID", _SiteCommentID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetSiteCommentsByID", slParams, "LarpPortal", _UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                    _SiteID = ldt.Rows[0]["SiteID"].ToString().ToInt32();
                    _AddedBy = ldt.Rows[0]["AddedBy"].ToString().ToInt32();
                    _CampaignID = ldt.Rows[0]["CampaignID"].ToString().ToInt32();
                    _SiteComments = ldt.Rows[0]["SiteComments"].ToString();
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
                slParams.Add("@SiteCommentID", _SiteCommentID);
                slParams.Add("@SiteID",_SiteID);
                slParams.Add("@AddedBy", _AddedBy);
                slParams.Add("@CampaignID", _CampaignID);
                slParams.Add("@SiteComments", _SiteComments);
                slParams.Add("@Comments", _Comments);

                cUtilities.PerformNonQuery("uspInsUpdSTSiteComments", slParams, "LarpPortal", _UserName);


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