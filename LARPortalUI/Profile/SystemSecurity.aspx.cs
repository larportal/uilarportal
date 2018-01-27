using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Profile
{
    public partial class SystemSecurity : System.Web.UI.Page
    {
        //string _UserName = "";
        //int _UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserName"] != null)
            //    _UserName = Session["UserName"].ToString();
            //if (Session["UserID"] != null)
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);

            tbOrigPassword.Attributes.Add("PlaceHolder", "Password");
            tbPassword.Attributes.Add("PlaceHolder", "Password");
            tbPasswordConfirm.Attributes.Add("PlaceHolder", "Password");
            tbQuestion1.Attributes.Add("PlaceHolder", "Enter your security question 1");
            tbAnswer1.Attributes.Add("PlaceHolder", "Enter your security answer 1");
            tbQuestion2.Attributes.Add("PlaceHolder", "Enter your security question 2");
            tbAnswer2.Attributes.Add("PlaceHolder", "Enter your security answer 2");
            tbQuestion3.Attributes.Add("PlaceHolder", "Enter your security question 3");
            tbAnswer3.Attributes.Add("PlaceHolder", "Enter your security answer 3");
            btnCloseModal.Attributes.Add("-data-dismiss", "modal");

            string sPasswordHelp = "LARP Portal login passwords must be at least 7 characters long and contain at least " +
               "1 uppercase letter, 1 lowercse letter, 1 number and 1 special character";

            lblPasswordReqs.ToolTip = sPasswordHelp;
            lblPasswordConfirmReqs.ToolTip = sPasswordHelp;

            divErrorPasswords.Visible = false;
            divErrorQuestion1.Visible = false;
            divErrorQuestion2.Visible = false;
            divErrorQuestion3.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Classes.cUser Demography = new Classes.cUser(Master.UserName, "Password", Session.SessionID);
                Classes.cPlayer PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);
                Classes.cLogin UserLoggedIn = new Classes.cLogin();
                UserLoggedIn.ValidateUserForPasswordReset(Master.UserName, Demography.PrimaryEmailAddress.EmailAddress, Demography.LastName);

                hidOrigPassword.Value = UserLoggedIn.Password;

                tbAnswer1.Text = UserLoggedIn.SecurityAnswer1;
                tbAnswer2.Text = UserLoggedIn.SecurityAnswer2;
                tbAnswer3.Text = UserLoggedIn.SecurityAnswer3;

                tbQuestion1.Text = UserLoggedIn.SecurityQuestion1;
                tbQuestion2.Text = UserLoggedIn.SecurityQuestion2;
                tbQuestion3.Text = UserLoggedIn.SecurityQuestion3;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            divErrorPasswords.Visible = false;
            divErrorQuestion1.Visible = false;
            divErrorQuestion2.Visible = false;
            divErrorQuestion3.Visible = false;

            if ((!hidOrigPassword.Value.Equals(tbOrigPassword.Text)) &&
                (tbOrigPassword.Text.Length > 0))
            {
                lblErrorPasswords.Text = "The original password is incorrect.";
                divErrorPasswords.Visible = true;
                return;
            }

            if ((tbOrigPassword.Text.Length == 0) &&
                ((tbPassword.Text.Length > 0) ||
                 (tbPasswordConfirm.Text.Length > 0)))
            {
                lblErrorPasswords.Text = "If you want to change your password you must enter the original password.";
                divErrorPasswords.Visible = true;
                return;
            }

            if (!tbPassword.Text.Equals(tbPasswordConfirm.Text))
            {
                lblErrorPasswords.Text = "The new passwords must be the same.";
                divErrorPasswords.Visible = true;
                return;
            }

            Classes.cUser Demography = new Classes.cUser(Master.UserName, "Password", Session.SessionID);
            Classes.cPlayer PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);
            Classes.cLogin UserLoggedIn = new Classes.cLogin();
            //this needs to be done in order to show security questions as per ForgotPassword existing logic
            UserLoggedIn.ValidateUserForPasswordReset(Master.UserName, Demography.PrimaryEmailAddress.EmailAddress, Demography.LastName);

            //Validate password
            if (!string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                UserLoggedIn.ValidateNewPassword(tbPassword.Text);
                if (UserLoggedIn.PasswordValidation == 0)
                {
                    lblErrorPasswords.Text = UserLoggedIn.PasswordFailMessage;
                    divErrorQuestion1.Visible = true;
                    return;
                }
            }

            if ((string.IsNullOrWhiteSpace(tbQuestion1.Text)) ||
                (string.IsNullOrWhiteSpace(tbAnswer1.Text)))
            {
                lblErrorQuestion1.Text = "Question 1 and Answer 1 are required.";
                divErrorQuestion1.Visible = true;
                tbQuestion1.Focus();
                return;
            }


            //If question entered and answer not entered
            if (!string.IsNullOrWhiteSpace(tbQuestion2.Text) && string.IsNullOrWhiteSpace(tbAnswer2.Text))
            {
                lblErrorQuestion2.Text = "If you enter question 2 you must enter the answer also.";
                lblErrorQuestion2.Focus();
                divErrorQuestion2.Visible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(tbQuestion2.Text) && !string.IsNullOrWhiteSpace(tbAnswer2.Text))
            {
                lblErrorQuestion2.Text = "If you enter answer 2 you must enter the question also.";
                lblErrorQuestion2.Focus();
                divErrorQuestion2.Visible = true;
                return;
            }

            //If question entered and answer not entered
            if (!string.IsNullOrWhiteSpace(tbQuestion3.Text) && string.IsNullOrWhiteSpace(tbAnswer3.Text))
            {
                lblErrorQuestion3.Text = "If you enter question 3 you must enter the answer also.";
                lblErrorQuestion3.Focus();
                divErrorQuestion3.Visible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(tbQuestion3.Text) && !string.IsNullOrWhiteSpace(tbAnswer3.Text))
            {
                lblErrorQuestion3.Text = "If you enter answer 3 you must enter the question also.";
                lblErrorQuestion3.Focus();
                divErrorQuestion3.Visible = true;
                return;
            }


            //Make sure that questions and answers do not repeat

            UserLoggedIn.UpdateQAandPassword(
                UserLoggedIn.UserSecurityID,
                Master.UserID,
                tbQuestion1.Text, "1",
                tbQuestion2.Text, "1",
                tbQuestion3.Text, "1",
                tbAnswer1.Text, "1",
                tbAnswer2.Text, "1",
                tbAnswer3.Text, "1",
                string.IsNullOrWhiteSpace(tbPassword.Text) ? UserLoggedIn.Password : tbPassword.Text);

            lblErrorPasswords.Text = "Changes saved successfully.";
        }
    }
}