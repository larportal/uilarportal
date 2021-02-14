using System.Collections;
using System.Data;
using System.Net.Mail;
using System.Reflection;


namespace LarpPortal.Classes
{
    public enum cNotificationTypes
    {
        PELDUE,
        REGPAYDUE,
        INFOSKILLDUE,
        EVENTUPDATED,
        REGOPEN,
        REGAPPROVED,
        PELSUBMIT,
        POINTSAPPLIED,
        HISTORYSUBMIT,
        HISTORYAPPROVE,
        TEAMREQUEST,
        NOTSPECIFIED
    };

    public class cSendNotifications
    {
        public cNotificationTypes NotifyType { get; set; }
        public string SubjectText { get; set; }
        public string EMailBody { get; set; }

        public cSendNotifications()
        {
            SubjectText = "";
            EMailBody = "";
            NotifyType = cNotificationTypes.NOTSPECIFIED;
        }

        public void SendNotification(int iUserID, string sUserName)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (NotifyType == cNotificationTypes.NOTSPECIFIED)
            {
                throw new System.Exception("The NotifyType must be specified to be able to send a notification to the user.");
            }

            SortedList sParam = new SortedList();
            sParam.Add("@UserID", iUserID);
            DataTable dtPref = cUtilities.LoadDataTable("uspGetPlayerNotificationPrefs", sParam, "LARPortal", sUserName, lsRoutineName);
            dtPref.CaseSensitive = false;
            DataView dvPref = new DataView(dtPref, "NotificationCode = '" + NotifyType.ToString() + "' and isnull(DeliveryMethod,'NONE') <>  'NONE'", "", DataViewRowState.CurrentRows);
            if (dvPref.Count > 0)
            {
                string sNotifyType = dvPref[0]["DeliveryMethod"].ToString().ToUpper();
                string sAddress = "";
                string sCallingJob = "";

                sCallingJob = dvPref[0]["CallingJob"].ToString();

                if (sNotifyType.StartsWith("T"))
                {
                    sAddress = dvPref[0]["TextAddress"].ToString();
                    if (dvPref[0]["TextAddress"].ToString().Length > 0)
                    {
                        if (SubjectText.Length == 0)
                        {
                            // If there is nothing in the current subject text and the text address on the record > '' set the subject text.
                            SubjectText = dvPref[0]["TextMessageText"].ToString();
                            //                        sAddress = dvPref[0]["TextAddress"].ToString();
                        }
                    }

                    if (SubjectText.Length == 0)
                    {
                        // If the subject is blank (wasn't specified or was blank in the record) use the Email settings.
                        sNotifyType = "Email";
                    }
                }

                if (sNotifyType.StartsWith("E"))
                {
                    // Either the user specified Email, or there's missing information from the text address.
                    sAddress = dvPref[0]["EMailAddress"].ToString();
                    if (SubjectText.Length == 0)
                        SubjectText = dvPref[0]["EMailSubjectText"].ToString();
                    if (EMailBody.Length == 0)
                        EMailBody = dvPref[0]["EMailMessageText"].ToString();

                    // If the subject is blank, use the first 100 characters from the body of the email.
                    if (SubjectText.Length == 0)
                    {
                        // Take the first 100 character from body.
                        SubjectText = (EMailBody.Length <= 100 ? EMailBody : EMailBody.Substring(0, 100));
                    }
                }

                if ((sAddress.Length > 0) &&
                    (SubjectText.Length > 0) &&
                    (sCallingJob.Length > 0))
                {
                    Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
                    cEMS.SendMail(SubjectText, EMailBody, sAddress, "", "", sCallingJob, sUserName);
                }
            }
        }
    }
}
