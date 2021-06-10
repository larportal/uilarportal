using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
	public partial class Register : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Classes.cLogin LearnMore = new Classes.cLogin();
				LearnMore.getTermsOfUse();
				lblTestOfUseMessage.Text = LearnMore.TermsOfUseText;
				btnSignUp.Attributes.Add("disabled", "true");
				lblPasswordReqs.ToolTip = "LARP Portal login passwords must be at least 7 characters long and contain at least " +
				"1 uppercase letter, 1 lowercse letter, 1 number and 1 special character";
			}
			btnCloseMessage.Attributes.Add("data-dismiss", "modal");
		}

		protected void txtPasswordNew_TextChanged(object sender, EventArgs e)
		{

		}

		protected void txtPasswordNewRetype_TextChanged(object sender, EventArgs e)
		{

		}

		protected void chkTermsOfUse_CheckedChanged(object sender, EventArgs e)
		{

		}

		protected void btnGuest_Click(object sender, EventArgs e)
		{
			Response.Redirect("Contact.aspx", false);
		}

		protected void btnSignUp_Click(object sender, EventArgs e)
		{
			//	if (Session["AttemptedPassword"] == null)
			//		txtPasswordNew.Text = "";
			//	else
			//	{
			//		txtPasswordNew.Text = Session["AttemptedPassword"].ToString();
			//		txtPasswordNew.Attributes.Add("value", txtPasswordNew.Text);
			//	}
			//	if (Session["AttemptedPasswordRetype"] == null)
			//		txtPasswordNewRetype.Text = "";
			//	else
			//	{
			//		txtPasswordNewRetype.Text = Session["AttemptedPasswordRetype"].ToString();
			//		txtPasswordNewRetype.Attributes.Add("value", txtPasswordNewRetype.Text);
			//	}
			//	if (Page.IsValid)
			//	{
					lblSignUpErrors.Text = "";
					// 1 - No duplicate usernames allowed
					Classes.cLogin Login = new Classes.cLogin();
					Login.CheckForExistingUsername(txtNewUsername.Text);
					if (Login.MemberID != 0)  // UserID is taken
					{
						lblSignUpErrors.Text = "This username is already in use.  Please select a different one.";
					}
					// 2 - Password must meet parameter standards
					int ValidPassword;
					Classes.cLogin PasswordValidate = new Classes.cLogin();
					PasswordValidate.ValidateNewPassword(txtPasswordNew.Text);
					ValidPassword = PasswordValidate.PasswordValidation;
					if (ValidPassword == 0)
					{
						if (lblSignUpErrors.Text != "")
						{
							lblSignUpErrors.Text = lblSignUpErrors.Text + "<p></p>" + PasswordValidate.PasswordFailMessage + ".";
						}
						else
						{
							lblSignUpErrors.Text = PasswordValidate.PasswordFailMessage + ".";
						}
					}
			// 3 - New request - If the email address is already on file, warn them and suggest they go to the Forgot Username / Password section
			Classes.cLogin ExistingEmailAddress = new Classes.cLogin();
			ExistingEmailAddress.GetUsernameByEmail(txtEmail.Text);
			if (ExistingEmailAddress.Username != "")
				if (lblSignUpErrors.Text != "")
				{
					lblSignUpErrors.Text = lblSignUpErrors.Text + "<p></p>This email address is already associated with an account.  If you've forgotten your username or password, please use the link above.";
				}
				else
				{
					lblSignUpErrors.Text = "This email address is already associated with an account.  If you've forgotten your username or password, please use the link above.";
				}
			// If there were errors, display them and return to form
			if (lblSignUpErrors.Text != "")
			{
				lblSignUpErrors.Visible = true;
				txtNewUsername.Focus();
			}
			else
			{
				// Everything is ok.  Create the record.  If successful, go to the member demographics screen.
				Classes.cUser NewUser = new Classes.cUser(txtNewUsername.Text, txtPasswordNew.Text, Session.SessionID);
				NewUser.FirstName = txtFirstName.Text;
				NewUser.LastName = txtLastName.Text;
				NewUser.LoginPassword = txtPasswordNew.Text;
				NewUser.LoginEmail = txtEmail.Text;
				NewUser.LoginName = txtNewUsername.Text;
				NewUser.Save();
				Classes.cLogin Activation = new Classes.cLogin();
				Activation.Load(txtNewUsername.Text, txtPasswordNew.Text);
				string ActivationKey = "";
				ActivationKey = Activation.SecurityResetCode;
				GenerateWelcomeEmail(txtFirstName.Text, txtLastName.Text, txtNewUsername.Text, txtEmail.Text, ActivationKey);
				Response.Redirect("/NewUserDirections.aspx", true);
				// TODO-Rick-0e Account for versioning of 'terms of use' and keeping track of date/time and which version user agreed to
			}
			//	}
			//	else
			//	{
			//		// TODO-Rick-3 On create user if something totally unexpected is wrong put up a message
			//	}

		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("/Default.aspx", true);
		}

		protected void GenerateWelcomeEmail(string FirstName, string LastName, string Username, string strTo, string ActivationKey)
		{
			string strBody;
			string strSubject = "Your LARP Portal Activation Key";
			strBody = "Hi " + FirstName + "<p></p>Welcome to LARP Portal.  The activation key for your new account is " + ActivationKey + ".  To activate your ";
			strBody = strBody + "account return to www.larportal.com.  Enter your username and password and click the Login ";
			strBody = strBody + "button.  When the site prompts you for your activation key, enter it and click the Login button again.<p></p>If you have ";
			strBody = strBody + "any questions please email us at support@larportal.com.";
			Classes.cEmailMessageService NotifyStaff = new Classes.cEmailMessageService();
			try
			{
				NotifyStaff.SendMail(strSubject, strBody, strTo, "", "support@larportal.com", "ActivationKey", Username);
			}
			catch (Exception)
			{
				lblSignUpErrors.Text = "There was an issue. Please contact us at support@larportal.com for assistance.";
				lblSignUpErrors.Visible = true;
			}
		}
	}
}