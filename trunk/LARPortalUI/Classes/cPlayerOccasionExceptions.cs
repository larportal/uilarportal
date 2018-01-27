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
    public class cPlayerOccasionExceptions
    {
        public Int32 PlayerOccasionExceptionID { get; set; }
        public Int32 PlayerProfileID { get; set; }
        public Int32 CampaignID { get; set; }
        public Int32 OccasionID { get; set; }
        public Boolean AttendPartial { get; set; }
        public string PlayerComments { get; set; }
        public string Comments { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateChanged { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        //public Int32 PlayerOccasionExceptionID
        //{
        //    get { return _PlayerOccasionExceptionID; }
        //}
        //public Int32 PlayerProfileID
        //{
        //    get { return _PlayerProfileID; }
        //}
        //public Int32 CampaignID
        //{
        //    get { return _CampaignID; }
        //    set { _CampaignID = value; }
        //}
        //public Int32 OccasionID
        //{
        //    get { return _OccasionID; }
        //    set { _OccasionID = value; }
        //}
        //public Boolean AttendPartial
        //{
        //    get { return _AttendPartial; }
        //    set { _AttendPartial = value; }
        //}
        //public string PlayerComments
        //{
        //    get { return _PlayerComments; }
        //    set { _PlayerComments = value; }
        //}
        //public string Comments
        //{
        //    get { return _Comments; }
        //    set { _Comments = value; }
        //}
        //public DateTime DateAdded
        //{
        //    get { return _DateAdded; }
        //}
        //public DateTime DateChanged
        //{
        //    get { return _DateChanged; }
        //}



        public cPlayerOccasionExceptions()
        {
            PlayerOccasionExceptionID = -1;
            PlayerProfileID = -1;
            CampaignID = -1;
            OccasionID = -1;
            AttendPartial = false;
            PlayerComments = "";
            Comments = "";
            RecordStatus = RecordStatuses.Active;
        }

        public cPlayerOccasionExceptions(Int32 intPlayerOcassionExceptionID, Int32 intPlayerProfileID, string UserName, Int32 UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            PlayerOccasionExceptionID = intPlayerOcassionExceptionID;
            PlayerProfileID = intPlayerProfileID;

            SortedList slParams = new SortedList();
            slParams.Add("@PlayerOccasionExceptionID", PlayerOccasionExceptionID);
            try
            {
                DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerOccasionExceptionByID", slParams, "LarpPortal", UserName, lsRoutineName);
                if (ldt.Rows.Count > 0)
                {
                      PlayerProfileID = ldt.Rows[0]["PlayerProfileID"].ToString().ToInt32();
                      CampaignID = ldt.Rows[0]["CampaignID"].ToString().ToInt32();
                      OccasionID = ldt.Rows[0]["OccasionID"].ToString().ToInt32();
                      AttendPartial = ldt.Rows[0]["AttendPartial"].ToString().ToBoolean();
                      PlayerComments = ldt.Rows[0]["PlayerComments"].ToString();
                      Comments = ldt.Rows[0]["Comments"].ToString();
                      DateAdded = Convert.ToDateTime(ldt.Rows[0]["DateAdded"].ToString());
                      DateChanged = Convert.ToDateTime(ldt.Rows[0]["DateChanged"].ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        public Boolean Save(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            Boolean blnReturn = false;
            try
            {
                SortedList slParams = new SortedList();
                slParams.Add("@UserID", UserID);
                slParams.Add("@PlayerOccasionExceptionID", PlayerOccasionExceptionID);
                slParams.Add("@PlayerProfileID", PlayerProfileID);
                slParams.Add("@CampaignID", CampaignID);
                slParams.Add("@OccasionID", OccasionID);
                slParams.Add("@AttendPartial", AttendPartial);
                slParams.Add("@PlayerComments", PlayerComments);
                slParams.Add("@Comments", Comments);
                blnReturn = cUtilities.PerformNonQueryBoolean("uspInsUpdPLPlayerOccasionExceptions", slParams, "LARPortal", UserName);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
                blnReturn = false;
            }
            return blnReturn;
        }
    }
}