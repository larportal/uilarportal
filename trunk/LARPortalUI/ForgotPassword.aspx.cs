using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
	public partial class ForgotPassword : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				mvInfoRequest.SetActiveView(vwInfoRequest);
			divInvalid.Visible = false;
		}

		protected void btnGetInfo_Click(object sender, EventArgs e)
		{
			Classes.cLogin ValidUser = new Classes.cLogin();
			ValidUser.ValidateUserForPasswordReset(txtUsername.Text, txtEmailAddress.Text, txtLastName.Text);
			if (ValidUser.MemberID == 0)
			{
				//If it's not valid flash a message with a clear button and tell them to try again.
				divInvalid.Visible = true;
				txtEmailAddress.Focus();
			}
			else  //If it's valid check for security questions.
			{
				Session["UserID"] = ValidUser.MemberID;
				//				UserSecurityID.Text = ValidUser.UserSecurityID.ToString();
				string An1 = ValidUser.SecurityAnswer1;
				string An2 = ValidUser.SecurityAnswer2;
				string An3 = ValidUser.SecurityAnswer3;
				string Qu1 = ValidUser.SecurityQuestion1;
				string Qu2 = ValidUser.SecurityQuestion2;
				string Qu3 = ValidUser.SecurityQuestion3;
				if ((!String.IsNullOrEmpty(ValidUser.SecurityQuestion1)) ||
					(!String.IsNullOrEmpty(ValidUser.SecurityQuestion2)) ||
					(!String.IsNullOrEmpty(ValidUser.SecurityQuestion3)))
				{
					mvInfoRequest.SetActiveView(vwAnswerQuestions);
					divUserQuestion2.Visible = false;
					divUserQuestion3.Visible = false;
					lblAnswerQuestionS.Visible = false;

					lblUserQuestion1.Text = ValidUser.SecurityQuestion1;
					hidUserAnswer1.Value = ValidUser.SecurityAnswer1;

					if (!String.IsNullOrEmpty(ValidUser.SecurityQuestion2))
					{
						lblAnswerQuestionS.Visible = true;
						divUserQuestion2.Visible = true;
						lblUserQuestion2.Text = ValidUser.SecurityQuestion2;
						hidUserAnswer2.Value = ValidUser.SecurityAnswer2;
					}

					if (!String.IsNullOrEmpty(ValidUser.SecurityQuestion3))
					{
						lblAnswerQuestionS.Visible = true;
						divUserQuestion3.Visible = true;
						lblUserQuestion3.Text = ValidUser.SecurityQuestion3;
						hidUserAnswer3.Value = ValidUser.SecurityAnswer3;
					}
				}
				//else //If at least one question, make the 'answer question panel visible.
				//{
				//	lblAskQuestion1.Text = "Security Question 1: " + Q1.Text;
				//	lblAskQuestion2.Text = "Security Question 2: " + Q2.Text;
				//	lblAskQuestion3.Text = "Security Question 3: " + Q3.Text;
				//	pnlAnswerQuestion.Visible = true;
				//	pnlIDYourself.Visible = false;
				//	txtAnswerQuestion1.Focus();
				//}
			}
		}

		protected void btnSupportSendEmail_Click(object sender, EventArgs e)
		{
			string strBody;
			string strSubject = "Trouble with username / password";
			strBody = "From: " + txtSupportName.Text + "<br>Email: " + txtSupportEmail.Text + "<br><br>Issue Details:<br><br>";
			strBody = strBody + txtSupportBody.Text;
			strBody = strBody + "<br><br>Answers Provided:<br>Email Address: " + txtEmailAddress.Text + "<br>";
			strBody = strBody + "Username: " + txtSupportName.Text + "<br>";

			string strCCAddress = "";
			if (chkSupportCCMe2.Checked)
				strCCAddress = txtSupportEmail.Text;

			Classes.cEmailMessageService MailServer = new Classes.cEmailMessageService();
			try
			{
				MailServer.SendMail(strSubject, strBody, txtEmailAddress.Text, strCCAddress, "", "ForgotPasswordSupportRequest", "System");
				lblSupportSentEmail.Text = "An email has been sent to LARP Portal support.";
				lblSupportSentEmail.Visible = true;
			}
			catch (Exception)
			{
				lblSupportSentEmail.Text = "An email has been sent to LARP Portal support.";
				lblSupportSentEmail.Visible = true;
			}
		}

		protected void btnSetQuestions_Click(object sender, EventArgs e)
		{
		}

		protected void btnUserQuestions_Click(object sender, EventArgs e)
		{
			mvInfoRequest.SetActiveView(vwSetPassword);
		}

		protected void btnSaveNewPassword_Click(object sender, EventArgs e)
		{

		}
	}
}