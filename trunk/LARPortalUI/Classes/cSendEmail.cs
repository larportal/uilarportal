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
    public class cSendEmail
    {
        protected void SendEmail(string SendFrom, string SendTo, string EmailSubject, string EmailBody)
        {
            //MailMessage objMail = new MailMessage("Sending From", "Sending To","Email Subject", "Email Body");
            //NetworkCredential objNC = new NetworkCredential("Sender Email","Sender Password");
            //SmtpClient objsmtp = new SmtpClient("smtp.live.com", 587); // for hotmail
            //objsmtp.EnableSsl = true;
            //objsmtp.Credentials = objNC;
            //objsmtp.Send(objMail);
            MailMessage objMail = new MailMessage();
        }
        
    }
}