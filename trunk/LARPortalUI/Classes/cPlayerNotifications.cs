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
    public class cPlayerNotifications
    {
        public int PlayerNotificationPreferenceID { get; set; }
        public int PlayerProfileID { get; set; }
        public int NotificationID { get; set; }
        public string NotificationDesc { get; set; }
        public int DeliveryNotificationID { get; set; }
        public string DeliveryNotificationDesc { get; set; }
        public string Comments { get; set; }
        public RecordStatuses RecordStatus { get; set; }

        private cPlayerNotifications()
        {
        }

        //public cPlayerOccasionExceptions(Int32 intPlayerOcassionExceptionID, Int32 intPlayerProfileID, string strUserName, Int32 intUserID)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();   // this is where we use refelection to store the name of the method and class to use it to report errors
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
        //    _PlayerOccasionExceptionID = intPlayerOcassionExceptionID;
        //    _PlayerProfileID = intPlayerProfileID;
        //    _UserName = strUserName;
        //    _UserID = intUserID;
        //    //so lets say we wanted to load data into this class from a sql query called uspGetSomeData thats take two paraeters @Parameter1 and @Parameter2
        //    SortedList slParams = new SortedList(); // I use a sortedlist  wich is a C# hash table to store the paramter and value
        //    slParams.Add("@PlayerOccasionExceptionID", _PlayerOccasionExceptionID);
        //    try
        //    {
        //        DataTable ldt = cUtilities.LoadDataTable("uspGetPlayerOccasionExceptionByID", slParams, "LarpPortal", Master.UserName, lsRoutineName);
        //        if (ldt.Rows.Count > 0)
        //        {
        //              _PlayerProfileID = ldt.Rows[0]["PlayerProfileID"].ToString().ToInt32();
        //              _CampaignID = ldt.Rows[0]["CampaignID"].ToString().ToInt32();
        //              _OccasionID = ldt.Rows[0]["OccasionID"].ToString().ToInt32();
        //              _AttendPartial = ldt.Rows[0]["AttendPartial"].ToString().ToBoolean();
        //              _PlayerComments = ldt.Rows[0]["PlayerComments"].ToString();
        //              _Comments = ldt.Rows[0]["Comments"].ToString();
        //              _DateAdded = Convert.ToDateTime(ldt.Rows[0]["DateAdded"].ToString());
        //              _DateChanged = Convert.ToDateTime(ldt.Rows[0]["DateChanged"].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorAtServer lobjError = new ErrorAtServer();
        //        lobjError.ProcessError(ex, lsRoutineName, Master.UserName + lsRoutineName);
        //    }
        //}
        public void Save(string UserName, int UserID)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            try
            {
                SortedList sParams = new SortedList();
                sParams.Add("@UserID", UserID);
                sParams.Add("@PlayerNotificationPreferenceID", PlayerNotificationPreferenceID);
                sParams.Add("@PlayerProfileID", PlayerProfileID);
                sParams.Add("@NotificationID", NotificationID);
                sParams.Add("@DeliveryNotificationID", DeliveryNotificationID);
                sParams.Add("@Comments", Comments);

                Classes.cUtilities.PerformNonQuery("uspInsUpdPLPlayerNotificationPreferences", sParams, "LARPortal", UserName);
            }
            catch (Exception ex)
            {
                ErrorAtServer lobjError = new ErrorAtServer();
                lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            }
        }

        public void Load(int PlayerNotificationPreferenceID)
        {
            //MethodBase lmth = MethodBase.GetCurrentMethod();
            //string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            //try
            //{
            //    DataTable dtNotification = cUtilities.LoadDataTable("
            //    SortedList sParams = new SortedList();
            //    sParams.Add("@UserID", UserID);
            //    sParams.Add("@PlayerNotificationPreferenceID", PlayerNotificationPreferenceID);
            //    sParams.Add("@PlayerProfileID", PlayerProfileID);
            //    sParams.Add("@NotificationID", NotificationID);
            //    sParams.Add("@DeliveryNotificationID", DeliveryNotificationID);
            //    sParams.Add("@Comments", Comments);

            //    Classes.cUtilities.PerformNonQuery("uspInsUpdPLPlayerNotificationPreferences", sParams, "LARPortal", UserName);
            //}
            //catch (Exception ex)
            //{
            //    ErrorAtServer lobjError = new ErrorAtServer();
            //    lobjError.ProcessError(ex, lsRoutineName, UserName + lsRoutineName);
            //}
        }


    }
}