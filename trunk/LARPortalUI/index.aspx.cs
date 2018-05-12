using System;
using System.Collections.Generic;
using System.IO;
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
				mvMainScreen.SetActiveView(vwLogin);

				// Destroys everything in the session which is essentially what logging out does.
				Session.Clear();
				Session ["LoginName"] = "Guest";                // Until login changes it
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
				if (!String.IsNullOrEmpty(Login.SecurityResetCode))
				{
					if (Login.LoginCount == 0) // New user.  First time activation.
					{
						mvMainScreen.SetActiveView(vwActivate);
						hidActivateCode.Value = Login.SecurityResetCode;
						Session["SavePassword"] = tbPassword.Text;
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

		/// <summary>
		/// If this is called it means the activation code was entered and is correct.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnValidateAccount_Click(object sender, EventArgs e)
		{
			Classes.cLogin Login = new Classes.cLogin();
			Login.Load(tbUserName.Text, Session ["SavePassword"].ToString());
			Login.SecurityResetCode = "";
			Login.ClearNewAccount(Login.UserSecurityID, Login.MemberID);
			MemberLogin(Login);
		}

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

			// Get OS and browser settings and save them to session variables
			HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
			string UserAgent = HttpContext.Current.Request.UserAgent;
			Session ["IPAddress"] = HttpContext.Current.Request.UserHostAddress;
			Session ["Browser"] = bc.Browser;
			Session ["BrowserVersion"] = bc.Version;
			Session ["Platform"] = bc.Platform;
			Session ["OSVersion"] = Request.UserAgent;

			// Login the user and get the last logged in location so we can go there.
			Login.LoginAudit(Login.MemberID, tbUserName.Text, sLoginPassword, HttpContext.Current.Request.UserHostAddress, bc.Browser, bc.Version, bc.Platform, Request.UserAgent);

			if (NumberOfCampaigns < 1)
			{
				Response.Redirect("NewUserSelectCampaign.aspx", true);
			}
			else
			{
				string sPageGoingTo = "Default.aspx";
				if (!String.IsNullOrEmpty(Login.LastLoggedInLocation))
				{
					sPageGoingTo = Login.LastLoggedInLocation;
					if (sPageGoingTo.StartsWith("/"))
						sPageGoingTo = sPageGoingTo.Substring(1);
				}

				// If the page doesn't exist, go to the default page.
				if (!File.Exists(Server.MapPath(sPageGoingTo)))
					sPageGoingTo = "Default.aspx";

				Response.Redirect(sPageGoingTo, true);
			}
		}

		protected void btnContactUs_Click(object sender, EventArgs e)
		{
			Response.Redirect("Contact.aspx", false);
		}

		protected void btnGuest_Click(object sender, EventArgs e)
		{
			Response.Redirect("Contact.aspx", false);
		}
	}
}