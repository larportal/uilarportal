using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Net;
//using System.Net.Mail;
using System.Data;
using LarpPortal.Classes;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

using MimeKit;
using MailKit;
using MailKit.Security;
using MailKit.Net.Smtp;


namespace LarpPortal.Classes
{
	/// <summary>
	/// Send out an email. 
	/// JBradshaw  4/19/2020  - Changed over to using MailKit.
	/// </summary>
    public class cEmailMessageService   // This is far from working.
    {
		//        private MailMessage mail { get; set; }

        /// <summary>
        /// Send email and write message to email log.
        /// </summary>
        /// <param name="subject">Subject of email.</param>
        /// <param name="body">Full HTML of the body of the email.</param>
        /// <param name="Tos">Who to send email to. Use either , or ; between multiple emails.</param>
        /// <param name="ccs">Who to cc email to. Use either , or ; between multiple emails.</param>
        /// <param name="bccs">Who to bcc email to. Use either , or ; between multiple emails.</param>
        /// <param name="CallingJob">The lookup value to use against table MDBSMTP.  Make sure this value is in that table or that you change it to use an appropriate value from that table</param>
        public void SendMail(string subject, string body, string Tos, string ccs, string bccs, string CallingJob, string UserName)
        {
            if (string.IsNullOrEmpty(Tos))
                throw new ArgumentException("There are no To email addresses");
            if (string.IsNullOrEmpty(body))
                throw new ArgumentException("The body is empty");

			//if (subject == null)
			//    throw new ArgumentException("The subject is empty");

            // RP - 7/2/2016 - Added strFrom as a passed in parameter.  
            //      Look it up and return all associated address information (SMTP client, password, port, etc) 
            //      First hard code a default just in case all else fails.
            //      Then oad defaults from table using CallingJob 'Default' first (uspGetSMTPParametersByCallingJob  @CallingJob = 'Default'
            //      Load called job and if that's found overwrite defaults
            string stStoredProc = "uspGetSMTPParametersByCallingJob";
            string stCallingMethod = "cEmailMessageService.SendMail.GetDefault";
            string strFrom = "support@larportal.com";
            string strSMTPPassword = "Picco!o7246";
            string strSMTPClient = "smtpout.secureserver.net";
            int intPort = 80;
            SortedList sParams = new SortedList();
            sParams.Add("@CallingJob", "Default");
            DataTable dtDefaultSMTP = cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", UserName,stCallingMethod);
            foreach (DataRow dRow in dtDefaultSMTP.Rows)
            {
                strFrom = dRow["FromAddress"].ToString();
                strSMTPPassword = dRow["Password"].ToString();
                strSMTPClient = dRow["SMTPClient"].ToString();
                Int32.TryParse(dRow["Port"].ToString(), out intPort);
            }
            sParams.Clear();
            sParams.Add("@CallingJob", CallingJob);
            stCallingMethod = "cEmailMessageService.SendMail.GetCallingJob";
            DataTable dtCallingJobSMTP = cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", UserName, stCallingMethod);
            foreach (DataRow dRowCJ in dtCallingJobSMTP.Rows)
            {
                strFrom = dRowCJ["FromAddress"].ToString();
                strSMTPPassword = dRowCJ["Password"].ToString();
                strSMTPClient = dRowCJ["SMTPClient"].ToString();
                Int32.TryParse(dRowCJ["Port"].ToString(), out intPort);
            }

			//         MailMessage mail = new MailMessage();
			//         mail.From = new MailAddress(strFrom);
			//         mail.Subject = subject;
			//         mail.Body = body;
			//         string[] recipients = Tos.Split(",;".ToArray());
			//         foreach (string rec in recipients)
			//             if (rec.Trim().Length > 0)     // JLB 6/5/2016 - Don't add if address is blank. Will cause errors.
			//                 mail.To.Add(rec.Trim());
			//         if (!string.IsNullOrEmpty(ccs))
			//         {
			//             string[] ccEmails = ccs.Split(",;".ToArray());
			//             foreach (string cc in ccEmails)
			//                 if (cc.Trim().Length > 0)     // JLB 6/5/2016 - Don't add if address is blank. Will cause errors.
			//                     mail.CC.Add(cc.Trim());
			//         }
			//         if (!string.IsNullOrEmpty(bccs))
			//         {
			//             string[] bccEmails = bccs.Split(",;".ToArray());
			//             foreach (string bcc in bccEmails)
			//                 if (bcc.Trim().Length > 0)     // JLB 6/5/2016 - Don't add if address is blank. Will cause errors.
			//                     mail.Bcc.Add(bcc.Trim());
			//         }
			////            SmtpClient client = new SmtpClient(strSMTPClient, intPort);


			//MailKit.


			MimeMessage MailMessage = new MimeMessage();
			BodyBuilder MailBody = new BodyBuilder();

			//			MailMessage.Subject = subject;
			MailMessage.Subject = subject ?? throw new ArgumentException("The subject is empty");
			MailMessage.From.Add(new MailboxAddress(strFrom, strFrom));

            string[] recipients = Tos.Split(",;".ToArray());
            foreach (string rec in recipients)
                if (rec.Trim().Length > 0)     // JLB 6/5/2016 - Don't add if address is blank. Will cause errors.
					MailMessage.To.Add(new MailboxAddress(rec.Trim(), rec.Trim()));
            if (!string.IsNullOrEmpty(ccs))
            {
                string[] ccEmails = ccs.Split(",;".ToArray());
                foreach (string cc in ccEmails)
                    if (cc.Trim().Length > 0)     // JLB 6/5/2016 - Don't add if address is blank. Will cause errors.
						MailMessage.Cc.Add(new MailboxAddress(cc.Trim(), cc.Trim()));
            }
            if (!string.IsNullOrEmpty(bccs))
            {
                string[] bccEmails = bccs.Split(",;".ToArray());
                foreach (string bcc in bccEmails)
                    if (bcc.Trim().Length > 0)     // JLB 6/5/2016 - Don't add if address is blank. Will cause errors.
						MailMessage.Bcc.Add(new MailboxAddress(bcc.Trim(), bcc.Trim()));
            }
			MailBody.HtmlBody = body;
			MailMessage.Body = MailBody.ToMessageBody();

			var client = new SmtpClient();

            try
            {
				client.ServerCertificateValidationCallback = (s, c, h, e) => true;
				client.Connect(strSMTPClient, intPort, SecureSocketOptions.SslOnConnect);
				client.Authenticate(strFrom, strSMTPPassword);
				client.Send(MailMessage);
				client.Disconnect(true);

                WriteEmailLog(strFrom, Tos, ccs, bccs, subject, body);
            }
			catch (Exception ex)
            {
                string t = ex.Message;
				//             //This was the catch I used in the new member login.  It needs something way better but I was in a hurry to get the programming done. - Rick
				//             //lblEmailFailed.Text = "There was an issue. Please contact us at support@larportal.com for assistance.";
				//             //lblEmailFailed.Visible = true;
            }
        }

        public void WriteEmailLog(string EmailFrom, string EmailTo, string EmailCCs, string EmailBCCs, string Subject, string Body)
        {
            string stStoredProc = "LARPortalAudit.dbo.uspInsertEmailLog";
            string stCallingMethod = "cEmailMessageService.WriteEmailLog";
            //DataTable dtCharacters = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@EmailID", -1);
            sParams.Add("@EmailFrom", EmailFrom);
            sParams.Add("@EmailTo", EmailTo);
            sParams.Add("@EmailCCs", EmailCCs);
            sParams.Add("@EmailBCCs", EmailBCCs);
            sParams.Add("@Subject", Subject);
            sParams.Add("@Body", Body);
            try
            {
                Classes.cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", stCallingMethod);
            }
            catch (Exception)
            {
                // Keep going.  Nothing to see here (at this time)
            }
        }
    }
}