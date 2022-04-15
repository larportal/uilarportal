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
			tbQuestion1.Attributes.Add("onchange", "javascript: CheckSet1();");
			tbAnswer1.Attributes.Add("onchange", "javascript: CheckSet1();");
			tbQuestion2.Attributes.Add("onchange", "javascript: CheckSet2();");
			tbAnswer2.Attributes.Add("onchange", "javascript: CheckSet2();");
			tbQuestion3.Attributes.Add("onchange", "javascript: CheckSet3();");
			tbAnswer3.Attributes.Add("onchange", "javascript: CheckSet3();");
		}

		protected void btnGetPassword_Click(object sender, EventArgs e)
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
				hidUserSecurityID.Value = ValidUser.UserSecurityID.ToString();

				// Save all the values to the hidden fields so we can get them later.
				hidAnswer1.Value = ValidUser.SecurityAnswer1;
				hidAnswer2.Value = ValidUser.SecurityAnswer2;
				hidAnswer3.Value = ValidUser.SecurityAnswer3;
				hidQuestion1.Value = ValidUser.SecurityQuestion1;
				hidQuestion2.Value = ValidUser.SecurityQuestion2;
				hidQuestion3.Value = ValidUser.SecurityQuestion3;

				if ((!String.IsNullOrEmpty(ValidUser.SecurityQuestion1)) ||
					(!String.IsNullOrEmpty(ValidUser.SecurityQuestion2)) ||
					(!String.IsNullOrEmpty(ValidUser.SecurityQuestion3)))
				{
					// At least one of the questions is filled in.
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
				else
				{
					mvInfoRequest.SetActiveView(vwSecurityQuestions);
					tbQuestion1.Focus();
				}
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
			// Save the values the person has entered into hidden variables so when we go to write out the password we have them.
			hidQuestion1.Value = tbQuestion1.Text.Trim();
			hidAnswer1.Value = tbAnswer1.Text;
			hidUpdate1.Value = "1";
			hidQuestion2.Value = tbQuestion2.Text.Trim();
			hidAnswer2.Value = tbAnswer2.Text;
			hidUpdate2.Value = "1";
			hidQuestion3.Value = tbQuestion3.Text.Trim();
			hidAnswer3.Value = tbAnswer3.Text;
			hidUpdate3.Value = "1";
			mvInfoRequest.SetActiveView(vwSetPassword);
			tbPassword.Focus();
		}

		protected void btnUserQuestions_Click(object sender, EventArgs e)
		{
			// Make sure all the questions are answered correctly, else tell them they're wrong, clear the answers, put the focus on the first answer box.
			int numRequiredAnswers = 0;
			int numSuppliedAnswers = 0;
			string Answer1 = "";
			string Answer2 = "";
			string Answer3 = "";

			if (hidUserAnswer1.Value != null)
            {
				numRequiredAnswers = numRequiredAnswers + 1;
				Answer1 = hidUserAnswer1.Value;
            }
			if (hidUserAnswer2.Value != null)
			{
				numRequiredAnswers = numRequiredAnswers + 1;
				Answer2 = hidUserAnswer2.Value;
			}
			if (hidUserAnswer3.Value != null)
			{
				numRequiredAnswers = numRequiredAnswers + 1;
				Answer3 = hidUserAnswer3.Value;
			}
			if (hidUserAnswer1.Value != null) 
            {
				if (Answer1.ToUpper() == tbUserAnswer1.Text.ToUpper())
                {
					numSuppliedAnswers = numSuppliedAnswers + 1;
                }
            }
			if (hidUserAnswer2.Value != null)
            {
				if (Answer2.ToUpper() == tbUserAnswer2.Text.ToUpper())
                {
					numSuppliedAnswers = numSuppliedAnswers + 1;
				}
            }
			if (hidUserAnswer3.Value != null)
			{
				if (Answer3.ToUpper() == tbUserAnswer3.Text.ToUpper())
				{
					numSuppliedAnswers = numSuppliedAnswers + 1;
				}
			}
			if (numRequiredAnswers == numSuppliedAnswers)
            {
				mvInfoRequest.SetActiveView(vwSetPassword);
            }
            else
            {
				tbAnswer1.Text = "";
				tbAnswer2.Text = "";
				tbAnswer3.Text = "";
				// Not all answers were correct. Please try again.
				string jsString = "alert('Not all answers were correct. Please try again.');";
				ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
						"MyApplication",
						jsString,
						true);
				tbAnswer1.Focus();
            }


		}

		protected void btnSaveNewPassword_Click(object sender, EventArgs e)
		{
			int ValidPassword;
			Classes.cLogin PasswordValidate = new Classes.cLogin();
			PasswordValidate.ValidateNewPassword(tbPassword.Text);
			ValidPassword = PasswordValidate.PasswordValidation;
			if (ValidPassword == 0)
			{

				lblErrorPasswords.Text = PasswordValidate.PasswordFailMessage + ".";
				tbPassword.Text = "";
				tbPasswordConfirm.Text = "";
				divErrorPasswords.Visible = true;
				tbPassword.Focus();
			}
			else
			{
				int intUserID = 0;
				int intUserSecurityID = 0;
				if ((int.TryParse(Session["UserID"].ToString(), out intUserID)) &&
					(int.TryParse(hidUserSecurityID.Value, out intUserSecurityID)))
				{
					Classes.cLogin UpdateSecurity = new Classes.cLogin();
					UpdateSecurity.UpdateQAandPassword(intUserSecurityID, intUserID,
						hidQuestion1.Value, hidUpdate1.Value,
						hidQuestion2.Value, hidUpdate2.Value,
						hidQuestion3.Value, hidUpdate3.Value,
						hidAnswer1.Value, hidUpdate1.Value,
						hidAnswer2.Value, hidUpdate2.Value,
						hidAnswer3.Value, hidUpdate3.Value,
						tbPassword.Text);
					mvInfoRequest.SetActiveView(vwFinalStep);
				}
			}
		}

		protected void btnDone_Click(object sender, EventArgs e)
		{
			Response.Redirect("/index.aspx", false);
		}
	}
}