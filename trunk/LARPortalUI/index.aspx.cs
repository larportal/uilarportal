using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
	public partial class index : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			tbUserName.Attributes.Add("PlaceHolder", "Username");
			tbPassword.Attributes.Add("PlaceHolder", "Password");
			btnClose.Attributes.Add("data-dismiss", "modal");
			tbUserName.Focus();

			// Added to redirect http to https
			setSecureProtocol(true);
			//
			if (!IsPostBack)
			{
				// Destroys everything in the session which is essentially what logging out does.
				Session.Clear();
				Session ["LoginName"] = "Guest";                    // Until login changes it
				Session ["UserID"] = 0;                         // Until login changes it
				Session ["UserName"] = "Guest";
				Session ["Guest"] = "Y";
				Session ["SecurityRole"] = 0;                   // Until login changes it
				Session ["CurrentPagePermission"] = "True";     // We'll assume that wherever you were last you can still be there when the system takes you there on login
				Session.Remove("SuperUser");                    // Don't care what SuperUser value is, if it exists that's good enough.

				string SiteOpsMode;
				Classes.cLogin OpsMode = new Classes.cLogin();
				OpsMode.SetSiteOperationalMode();
				SiteOpsMode = OpsMode.SiteOperationalMode;
				Session ["OperationalMode"] = SiteOpsMode;

				//	txtName.Visible = false;
				//txtLastLocation.Visible = false;
				//txtLastCharacter.Visible = false;
				//txtLastCampaign.Visible = false;
				//txtUserID.Visible = false;
				//lblPasswordReqs.ToolTip = "LARP Portal login passwords must be at least 7 characters long and contain at least " +
				//	"1 uppercase letter, 1 lowercse letter, 1 number and 1 special character";
				//if (!IsPostBack)
				//{
				//	txtUserName.Attributes.Add("Placeholder", "Username");
				//	txtUserName.Focus();
				//	txtPassword.Attributes.Add("Placeholder", "Password");
				//	txtEmail.Attributes.Add("Placeholder", "Email");
				//	txtNewUsername.Attributes.Add("Placeholder", "Username");
				//	txtFirstName.Attributes.Add("Placeholder", "First Name");
				//	txtLastName.Attributes.Add("Placeholder", "Last Name");
				//	txtPasswordNew.Attributes.Add("Placeholder", "Password");
				//	txtPasswordNewRetype.Attributes.Add("Placeholder", "Retype Password");
				//	btnSignUp.Visible = false;
				//}
			}
		}

		protected void btnRegister_Click(object sender, EventArgs e)
		{
			Response.Redirect("Register.aspx", false);
		}

		public static void setSecureProtocol(bool bSecure)
		{
			string redirectURL = null;
			HttpContext context = HttpContext.Current;
			// If we want HTTPS and it is currently HTTP
			if (bSecure && !context.Request.IsSecureConnection)
				redirectURL = context.Request.Url.ToString().Replace("http:", "https:");
			else
				// If we want HTTP and it is currently HTTPS
				if (!bSecure && context.Request.IsSecureConnection)
				redirectURL = context.Request.Url.ToString().Replace("https:", "http:");
			//else
			// in all other cases we don't need to redirect

			if (context.Request.IsLocal)
				redirectURL = null;
			// check if we need to redirect, and if so use redirectUrl to do the job
			if (redirectURL != null)
				context.Response.Redirect(redirectURL);
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			//Session["AttemptedPassword"] = tbPassword.Text;
			//Session["AttemptedUsername"] = tbUserName.Text;
			Classes.cLogin Login = new Classes.cLogin();
			Login.Load(tbUserName.Text, tbPassword.Text);
			if (Login.MemberID == 0) // Invalid user, fall straight to fail logic
			{
				Session ["SecurityRole"] = 0;
				lblInvalidLogin.Visible = true;
				Login.LoginFail(tbUserName.Text, tbPassword.Text);
			}
			else
			{
				if (Login.SuperUser)
					Session ["SuperUser"] = 1;

				// Valid member. Is there a lock?
				if (Login.SecurityResetCode != "")
				{
					if (Login.LoginCount == 0) // New user.  First time activation.
					{
						hidActivateCode.Value = Login.SecurityResetCode;
						ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openActivationCode();", true);
						Session ["SavePassword"] = tbPassword.Text;
						tbActivationCode.Focus();
					}
					else // Existing user with a bigger problem
					{
						// TODO-Rick-3 Define how to handle user account locks on attempted login
					}
				}
				else  // Valid member.  Login.
				{
					Login.CheckForEmail(Login.MemberID);
					MemberLogin(Login);
				}
			}
		}

		protected void btnValidateAccount_Click(object sender, EventArgs e)
		{
			Classes.cLogin Login = new Classes.cLogin();
			Login.Load(tbUserName.Text, Session ["SavePassword"].ToString());
			//if (txtSecurityResetCode.Text == Login.SecurityResetCode)
			//{
			Login.SecurityResetCode = "";
			Login.ClearNewAccount(Login.UserSecurityID, Login.MemberID);
			MemberLogin(Login);
			//}
			//else
			//{
			//	lblInvalidActivationKey.Visible = true;
			//	txtSecurityResetCode.Text = "";
			//	txtSecurityResetCode.Focus();
			//}
		}

		//protected void txtSecurityResetCode_TextChanged(object sender, EventArgs e)
		//{

		//}

		/// <summary>
		/// The person has already been verified so figure out where to go and save the assorted variables.
		/// </summary>
		/// <param name="Login">The login class that has already been loaded and validated.</param>
		protected void MemberLogin(Classes.cLogin Login)
		{
			string sLoginPassword = tbPassword.Text;
			if ((string.IsNullOrEmpty(sLoginPassword)) &&
				(Session ["SavePassword"] != null))
			{
				if (!string.IsNullOrEmpty(Session ["SavePassword"].ToString()))
					sLoginPassword = Session ["SavePassword"].ToString();
			}

			Session.Clear();

			int NumberOfCampaigns = 0;
			Session ["MemberEmailAddress"] = Login.Email;
			Session ["SecurityRole"] = Login.SecurityRoleID;
			NumberOfCampaigns = Login.NumberOfCampaigns;

			Session ["LoginName"] = Login.FirstName;
			Session ["UserName"] = Login.Username;
			Session ["UserFullName"] = Login.FirstName + " " + Login.LastName;
			Session ["LoginPassword"] = Login.Password;
			Session ["UserID"] = Login.MemberID;
			Session.Remove("SuperUser");
			if (Login.SuperUser)
				Session ["SuperUser"] = 1;
			//			Session.Remove("Guest");

			// Get OS and browser settings and save them to session variables
			HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
			string UserAgent = HttpContext.Current.Request.UserAgent;
			Session ["IPAddress"] = HttpContext.Current.Request.UserHostAddress;
			Session ["Browser"] = bc.Browser;
			Session ["BrowserVersion"] = bc.Version;
			Session ["Platform"] = bc.Platform;
			Session ["OSVersion"] = Request.UserAgent;

			Login.LoginAudit(Login.MemberID, tbUserName.Text, sLoginPassword, HttpContext.Current.Request.UserHostAddress, bc.Browser, bc.Version, bc.Platform, Request.UserAgent);
			string sPageGoingTo = "";
			//if (String.IsNullOrEmpty(Login.LastLoggedInLocation))
			//	sPageGoingTo = "Default.aspx";
			//else
			//{
			//	sPageGoingTo = Login.LastLoggedInLocation;
			//	if (sPageGoingTo.StartsWith("/"))
			//		sPageGoingTo = sPageGoingTo.Substring(2, sPageGoingTo.Length - 1);
			//}

			sPageGoingTo = "Default.aspx";
			Response.Redirect(sPageGoingTo, true);
		}

		protected void btnContactUs_Click(object sender, EventArgs e)
		{
			Response.Redirect("Contact.aspx", false);
		}

		protected void btnGuest_Click(object sender, EventArgs e)
		{
			Response.Redirect("Contact.aspx", false);
		}

		//protected void btnSignUp_Click(object sender, EventArgs e)
		//{
		//	//if (Session["AttemptedPassword"] == null)
		//	//	txtPasswordNew.Text = "";
		//	//else
		//	//{
		//	//	txtPasswordNew.Text = Session["AttemptedPassword"].ToString();
		//	//	txtPasswordNew.Attributes.Add("value", txtPasswordNew.Text);
		//	//}
		//	//if (Session["AttemptedPasswordRetype"] == null)
		//	//	txtPasswordNewRetype.Text = "";
		//	//else
		//	//{
		//	//	txtPasswordNewRetype.Text = Session["AttemptedPasswordRetype"].ToString();
		//	//	txtPasswordNewRetype.Attributes.Add("value",txtPasswordNewRetype.Text);
		//	//}
		//	//if (Page.IsValid)
		//	//{
		//	//	lblSignUpErrors.Text = "";
		//	//	// 1 - No duplicate usernames allowed
		//	//	Classes.cLogin Login = new Classes.cLogin();
		//	//	Login.CheckForExistingUsername(txtNewUsername.Text);
		//	//	if (Login.MemberID != 0)  // UserID is taken
		//	//	{
		//	//		lblSignUpErrors.Text = "This username is already in use.  Please select a different one.";
		//	//	}
		//	//	// 2 - Password must meet parameter standards
		//	//	int ValidPassword;
		//	//	Classes.cLogin PasswordValidate = new Classes.cLogin();
		//	//	PasswordValidate.ValidateNewPassword(txtPasswordNew.Text);
		//	//	ValidPassword = PasswordValidate.PasswordValidation;
		//	//	if (ValidPassword == 0)
		//	//	{
		//	//		if (lblSignUpErrors.Text != "")
		//	//		{
		//	//			lblSignUpErrors.Text = lblSignUpErrors.Text + "<p></p>" + PasswordValidate.PasswordFailMessage + ".";
		//	//		}
		//	//		else
		//	//		{
		//	//			lblSignUpErrors.Text = PasswordValidate.PasswordFailMessage + ".";
		//	//		}
		//	//	}
		//	//	// 3 - Both passwords must be the same
		//	//	if (txtPasswordNew.Text != txtPasswordNewRetype.Text)
		//	//		//set an error message
		//	//	{
		//	//		if (lblSignUpErrors.Text != "")
		//	//		{
		//	//			lblSignUpErrors.Text = lblSignUpErrors.Text + "<p></p>Passwords don't match.  Please re-enter.";
		//	//		}
		//	//		else
		//	//		{
		//	//			lblSignUpErrors.Text = "Passwords don't match.  Please re-enter.";
		//	//		}
		//	//		txtPasswordNew.Text = "";
		//	//		txtPasswordNewRetype.Text = "";
		//	//	}
		//	//	// 4 - New request - If the email address is already on file, warn them and suggest they go to the Forgot Username / Password section
		//	//	Classes.cLogin ExistingEmailAddress = new Classes.cLogin();
		//	//	ExistingEmailAddress.GetUsernameByEmail(txtEmail.Text);
		//	//	if(ExistingEmailAddress.Username != "")
		//	//		if (lblSignUpErrors.Text != "")
		//	//		{
		//	//			lblSignUpErrors.Text = lblSignUpErrors.Text + "<p></p>This email address is already associated with an account.  If you've forgotten your username or password, please use the link above.";
		//	//		}
		//	//		else
		//	//		{
		//	//			lblSignUpErrors.Text = "This email address is already associated with an account.  If you've forgotten your username or password, please use the link above.";
		//	//		}
		//	//	// If there were errors, display them and return to form
		//	//	if (lblSignUpErrors.Text != "")
		//	//	{
		//	//		lblSignUpErrors.Visible = true;
		//	//		txtNewUsername.Focus();
		//	//	}
		//	//	else
		//	//	{
		//	//		// Everything is ok.  Create the record.  If successful, go to the member demographics screen.
		//	//		Classes.cUser NewUser = new Classes.cUser(txtNewUsername.Text, txtPasswordNew.Text);
		//	//		NewUser.FirstName = txtFirstName.Text;
		//	//		NewUser.LastName = txtLastName.Text;
		//	//		NewUser.LoginPassword = txtPasswordNew.Text;
		//	//		NewUser.LoginEmail = txtEmail.Text;
		//	//		NewUser.LoginName = txtNewUsername.Text;
		//	//		NewUser.Save();
		//	//		Classes.cLogin Activation = new Classes.cLogin();
		//	//		Activation.Load(txtNewUsername.Text, txtPasswordNew.Text);
		//	//		string ActivationKey = "";
		//	//		ActivationKey = Activation.SecurityResetCode;
		//	//		GenerateWelcomeEmail(txtFirstName.Text, txtLastName.Text, txtNewUsername.Text, txtEmail.Text, ActivationKey);
		//	//		Response.Write("<script>");
		//	//		Response.Write("window.open('NewUserLoginDirections.aspx','_blank')");
		//	//		Response.Write("</script>");
		//	//		// TODO-Rick-0e Account for versioning of 'terms of use' and keeping track of date/time and which version user agreed to
		//	//	}
		//	//}
		//	//else
		//	//{
		//	//	// TODO-Rick-3 On create user if something totally unexpected is wrong put up a message
		//	//}
		//}

		//protected void GenerateWelcomeEmail(string FirstName, string LastName, string Username, string strTo, string ActivationKey)
		//{
		//	//string strBody;
		//	//string strSubject = "Your LARP Portal Activation Key";
		//	//strBody = "Hi " + FirstName + "<p></p>Welcome to LARP Portal.  The activation key for your new account is " + ActivationKey + ".  To activate your ";
		//	//strBody = strBody + "account return to www.larportal.com.  Enter your username and password into the Member Login section and click the Login ";
		//	//strBody = strBody + "button.  When the site prompts you for your activation key, enter it and click the Login button again.<p></p>If you have ";
		//	//strBody = strBody + "any questions please email us at support@larportal.com.";
		//	//Classes.cEmailMessageService NotifyStaff = new Classes.cEmailMessageService();
		//	//try
		//	//{
		//	//	NotifyStaff.SendMail(strSubject, strBody, strTo, "" , "", "ActivationKey", Username);
		//	//}
		//	//catch (Exception)
		//	//{
		//	//	lblEmailFailed.Text = "There was an issue. Please contact us at support@larportal.com for assistance.";
		//	//	lblEmailFailed.Visible = true;
		//	//}
		//}

		//protected void chkTermsOfUse_CheckedChanged(object sender, EventArgs e)
		//{
		//	//if (chkTermsOfUse.Checked == true)
		//	//{
		//	//	btnSignUp.Visible = true;
		//	//	btnSignUp.Focus();
		//	//}
		//	//else
		//	//{
		//	//	btnSignUp.Visible = false;
		//	//}
		//}

		//protected void txtPasswordNewRetype_TextChanged(object sender, EventArgs e)
		//{
		//	//Session["AttemptedPasswordRetype"] = txtPasswordNewRetype.Text;
		//	//chkTermsOfUse.Focus();
		//	//if (Session["AttemptedPassword"] == null)
		//	//	txtPasswordNew.Text = "";
		//	//else
		//	//{
		//	//	txtPasswordNew.Text = Session["AttemptedPassword"].ToString();
		//	//	txtPasswordNew.Attributes.Add("value", txtPasswordNew.Text);
		//	//}
		//	//if (Session["AttemptedPasswordRetype"] == null)
		//	//	txtPasswordNewRetype.Text = "";
		//	//else
		//	//{
		//	//	txtPasswordNewRetype.Text = Session["AttemptedPasswordRetype"].ToString();
		//	//	txtPasswordNewRetype.Attributes.Add("value", txtPasswordNewRetype.Text);
		//	//}
		//}

		//protected void txtPasswordNew_TextChanged(object sender, EventArgs e)
		//{
		//	//Session["AttemptedPassword"] = txtPasswordNew.Text;
		//	//txtPasswordNewRetype.Focus();
		//	//if (Session["AttemptedPassword"] == null)
		//	//	txtPasswordNew.Text = "";
		//	//else
		//	//{
		//	//	txtPasswordNew.Text = Session["AttemptedPassword"].ToString();
		//	//	txtPasswordNew.Attributes.Add("value", txtPasswordNew.Text);
		//	//}
		//}

		//protected void txtEmail_TextChanged(object sender, EventArgs e)
		//{
		//	//if(txtEmail.Text.Length > 0)
		//	//	chkTermsOfUse.Visible = true;
		//	//txtPasswordNew.Focus();
		//}
	}
}