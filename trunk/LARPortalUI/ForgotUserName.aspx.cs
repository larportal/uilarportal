using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
	public partial class ForgotUserName : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				mvInfoRequest.SetActiveView(vwInfoRequest);
			divInvalid.Visible = false;
		}

		protected void btnGetUsername_Click(object sender, EventArgs e)
		{
			Classes.cLogin ValidUser = new Classes.cLogin();
			ValidUser.GetUsernameByEmail(txtEmailAddress.Text);
			if (ValidUser.Email == "")
			{
				divInvalid.Visible = true;
			}
			else
			{
				if (ValidUser.Email == ValidUser.Username)
				{
					lblMessage.Text = "Your email address is your username.  We recommend you change your username after logging in.";
					mvInfoRequest.SetActiveView(vwSentEmail);
				}
				else
				{
					lblMessage.Text = "An email has been sent to this email address with your username.  Use that username to fill out this form and complete the process.";
					ForgotUsername(txtEmailAddress.Text);
				}
			}
		}

		protected void ForgotUsername(string EmailAddress)
		{
			string strBody;
			string FirstName = "";
			string LastName = "";
			string LoginUsername = "";
			Classes.cLogin Username = new Classes.cLogin();
			Username.GetUsernameByEmail(EmailAddress);
			FirstName = Username.FirstName;
			LastName = Username.LastName;
			LoginUsername = Username.Username;

			string strSubject = "Your LARP Portal Username";
			strBody = "Hi " + FirstName + ",<p></p><p></p>Your LARP Portal username is <b>" + LoginUsername + "</b>.  If you need further assistance please contact us ";
			strBody = strBody + "via email at support@larportal.com.<br><br>";
			strBody = strBody + @"Click <u><a href=""https://www.larportal.com"">here</a></u> to log in.";

			Classes.cEmailMessageService MailServer = new Classes.cEmailMessageService();
			try
			{
				MailServer.SendMail(strSubject, strBody, EmailAddress, "", "", "ForgotUsername", "System");
				mvInfoRequest.SetActiveView(vwSentEmail);
			}
			catch (Exception)
			{
				mvInfoRequest.SetActiveView(vwIssue);
			}
		}

		protected void btnSignUp_Click(object sender, EventArgs e)
		{
			Response.Redirect("/Register.aspx", true);
		}
	}
}