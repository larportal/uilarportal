using LarpPortal.Reports;
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
			string strBody = "";
			string strSubject = "Your LARP Portal Activation Key";
			//OLD EMAIL BODY
			//strBody = "Hi " + FirstName + "<p></p>Welcome to LARP Portal.  The activation key for your new account is " + ActivationKey + ".  To activate your ";
			//strBody = strBody + "account return to www.larportal.com.  Enter your username and password and click the Login ";
			//strBody = strBody + "button.  When the site prompts you for your activation key, enter it and click the Login button again.<p></p>If you have ";
			//strBody = strBody + "any questions please email us at support@larportal.com.";
			//strBody = strBody + "<p></p><p></p>If you don't receive your email within a few minutes DO NOT create a new account. Check your spam filter. ";
			//strBody = strBody + "If you still don't have it, send an email to support@larportal.com and we will send you the email from a different account.";

			//************** NEW AI GENERATED VERSION OF MONIQUE'S EMAIL ********************************
			strBody = "<html><head><style>";
			strBody = strBody + "body {{";
			strBody = strBody + "font -family: Arial, sans-serif;";
			strBody = strBody + "line -height: 1.6;";
			strBody = strBody + "}}";
			strBody = strBody + "h2 {{";
			strBody = strBody + "color: #2C3E50;";
			strBody = strBody + "}}";
			strBody = strBody + "p {{";
			strBody = strBody + "margin: 0 0 10px;";
			strBody = strBody + "}}";
			strBody = strBody + "a {{";
			strBody = strBody + "color: #2980B9;";
			strBody = strBody + "text -decoration: none;";
			strBody = strBody + "}}";
			strBody = strBody + "</style></head><body>";



			strBody = strBody + "<h2>Welcome to LARP Portal</h2>";
			strBody = strBody + "<p>Hi " + FirstName + ",</p>";
			strBody = strBody + "<p>Welcome to LARP Portal! We are so glad that you are joining this great community.</p>";
			strBody = strBody + "<p>Your LARP Portal activation key is <strong>" + ActivationKey + "</strong>.</p>";
			strBody = strBody + "<h3>To activate your account:</h3>";
			strBody = strBody + "<ol>";
			strBody = strBody + "<li>Return to <a href='http://www.larportal.com'>www.larportal.com</a>.</li>";
			strBody = strBody + "<li>Enter your username and password.</li>";
			strBody = strBody + "<li>Click the “Login” button.</li>";
			strBody = strBody + "<li>Copy & paste the activation key above.</li>";
			strBody = strBody + "<li>Hit the “Login” button again.</li>";
			strBody = strBody + "<li>From the dropdown list, choose the first campaign that you'd like to sign up for. You can add more later.</li>";
			strBody = strBody + "<li>Check off whether you want to PC, NPC or both. If you're going to take on a staff role, choose NPC, then contact the game owner or GM to update your permissions.</li>";
			strBody = strBody + "<li>Click the Sign up button</li>";
			strBody = strBody + "</ol>";
            //strBody = strBody + "<p>If you don’t receive your email within a few minutes, <strong>DO NOT</strong> create a new account.</p>";
            //strBody = strBody + "<ul>";
            //strBody = strBody + "<li>Check your spam filter.</li>";
            //strBody = strBody + "<li>If you still don’t have it, send an email to <a href='mailto:support@larportal.com'>support@larportal.com</a>, and we will send you the email from a different account.</li></ul>";
            strBody = strBody + "<h3>What do I do now?</h3><p>Helpful pointers:</p><ul>";
			strBody = strBody + "<li>You only need one account no matter how many games you play. Your access will adjust based on whether you PC, NPC, or Staff.</li>";
			strBody = strBody + "<li>The <strong>Home Page</strong> offers quick access to things like updating your character, registering for events, and submitting Post Event Letters/Surveys.</li>";
			strBody = strBody + "<li>You can create multiple characters in a campaign and different versions of the same character.</li>";
			strBody = strBody + "<li>Remember to hit SAVE when making changes.</li>";
			strBody = strBody + "<li>View <strong>Points</strong> earned for various contributions under the POINTS section.</li>";
			strBody = strBody + "<li>The <strong>Calendar</strong> can be filtered to view all games or just your games.</li>";
			strBody = strBody + "</ul><h3>New Player Checklist</h3>";
			strBody = strBody + "<p>We’ve included links to our YouTube tutorial videos for guidance.</p>";
			strBody = strBody + "<table border='1' cellpadding='8'>";
			strBody = strBody + "<tr><th>Module/Screen</th><th>Description</th></tr>";
			strBody = strBody + "<tr><td>PLAYER Profile</td><td><a href='https://youtu.be/8xOA241NJ1Q'>Complete your Player Profile</a><br />";
			strBody = strBody + "<p>Fill in essential details like your address, phone numbers, pronouns, etc. (Required: Name, email, zip code, and DOB).</p></td></tr>";
			strBody = strBody + "<tr><td>PLAYER Preferences</td><td><p><strong>Choose your notifications:</strong></p>";
			strBody = strBody + "<p>Set up notification preferences to stay informed via email or text.</p></td></tr>";
			strBody = strBody + "<tr><td>PLAYER Medical Info</td><td><a href='https://youtu.be/hh_HZaCrc4Y'>Add any medical info</a><br />";
			strBody = strBody + "<p>Fill in details like medical conditions or allergies for event staff reference.</p></td></tr>";
			strBody = strBody + "<tr><td>CHARACTER</td><td><a href='https://youtu.be/MR6LLY5wv3g'>Start your character</a><br />";
			strBody = strBody + "<p>Add your character header and details to register for events as a PC.</p></td></tr>";
			strBody = strBody + "<tr><td>CAMPAIGN Events</td><td><a href='https://youtu.be/v6CZd3iW_SE'>Register for an event</a><br />";
			strBody = strBody + "<p>Pick your role (PC, NPC, or Staff) and provide necessary information for event participation.</p></td></tr>";
			strBody = strBody + "</table>";



			strBody = strBody + "</body></html>";
			//************** END OF NEW AI GENERATED VERSION OF MONIQUE'S EMAIL *************************
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

    //    protected void btnTerms_Click(object sender, EventArgs e)
    //    {
    //        if (btnTerms.Text == "Show")
    //        {
    //            btnTerms.Text = "Hide";
				//pnlTermsOfUse.Visible = true;
    //        }
    //        else
    //        {
    //            btnTerms.Text = "Show";
				//pnlTermsOfUse.Visible = false;
    //        }
    //    }
    }
}