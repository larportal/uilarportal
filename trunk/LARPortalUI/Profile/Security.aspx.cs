using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Profile
{
    public partial class Security : System.Web.UI.Page
    {
        string uName = string.Empty;
        int uID = 0;
        Classes.cLogin UserLoggedIn = null;
        Classes.cUser Demography = null;
        Classes.cPlayer PLDemography = null;

        //string _UserName = "";
        //int _UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword.Attributes.Add("PlaceHolder", "Password");
            txtPassword2.Attributes.Add("PlaceHolder", "Password");
            txtSecurityQuestion1.Attributes.Add("PlaceHolder", "Enter your security question 1");
            txtSecurityAnswer1.Attributes.Add("PlaceHolder", "Enter your security answer 1");
            txtSecurityQuestion2.Attributes.Add("PlaceHolder", "Enter your security question 2");
            txtSecurityAnswer2.Attributes.Add("PlaceHolder", "Enter your security answer 2");
            txtSecurityQuestion3.Attributes.Add("PlaceHolder", "Enter your security question 3");
            txtSecurityAnswer3.Attributes.Add("PlaceHolder", "Enter your security answer 3");

            lblError.Text = string.Empty;
            lblErrorQuestions.Text = string.Empty;
            lblErrorQuestion2.Text = string.Empty;
            lblErrorQuestion3.Text = string.Empty;
            lblPasswordReqs.Text = "<span class=" + "\"" + "glyphicon glyphicon-question-sign" + "\"" + "></span>";
            lblPasswordReqs.ToolTip = "LARP Portal login passwords must be at least 7 characters long and contain at least " +
               "1 uppercase letter, 1 lowercse letter, 1 number and 1 special character";

            if (Session["Username"] == null)
            {
                btnSave.Visible = false;
                return;
            }

            //if (Session["UserName"] != null)
            //    _UserName = Session["UserName"].ToString();
            //if (Session["UserID"] != null)
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);

            //Session["ActiveLeftNav"] = "Security";
            //This method is not loading my email so nothing works

            Demography = new Classes.cUser(uName, "Password", Session.SessionID);
            PLDemography = new Classes.cPlayer(uID, uName);
            UserLoggedIn = new Classes.cLogin();
            //this needs to be done in order to show security questions as per ForgotPassword existing logic
            UserLoggedIn.ValidateUserForPasswordReset(uName, Demography.PrimaryEmailAddress.EmailAddress, Demography.LastName);

            if (IsPostBack == false)
            {
                txtUsername.Text = uName;
                txtFirstName.Text = Demography.FirstName;
                if (Demography.MiddleName.Length > 0)
                    txtMI.Text = Demography.MiddleName.Substring(0, 1);
                txtLastName.Text = Demography.LastName;
                txtNickName.Text = Demography.NickName;

                if (string.IsNullOrWhiteSpace(Demography.PrimaryEmailAddress.EmailAddress))
                {
                    lblError.Text = "Primary email must be setup first";
                    return;
                }

                txtSecurityAnswer1.Text = UserLoggedIn.SecurityAnswer1;
                txtSecurityAnswer2.Text = UserLoggedIn.SecurityAnswer2;
                txtSecurityAnswer3.Text = UserLoggedIn.SecurityAnswer3;

                txtSecurityQuestion1.Text = UserLoggedIn.SecurityQuestion1;
                txtSecurityQuestion2.Text = UserLoggedIn.SecurityQuestion2;
                txtSecurityQuestion3.Text = UserLoggedIn.SecurityQuestion3;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtPassword.Text.Equals(txtPassword2.Text))
            {
                lblError.Text = "Passwords do not match";
                return;
            }

            //Validate password
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                UserLoggedIn.ValidateNewPassword(txtPassword.Text);
                if (UserLoggedIn.PasswordValidation == 0)
                {
                    lblError.Text = UserLoggedIn.PasswordFailMessage;
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtSecurityQuestion1.Text))
            {
                lblErrorQuestions.Text = "Please enter security question";
                txtSecurityQuestion1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSecurityAnswer1.Text))
            {
                lblErrorQuestions.Text = "Please enter security answer";
                txtSecurityAnswer1.Focus();
                return;
            }

            string strEnterAnswer = "Please enter answer to question";
            string strEnterQuestion = "Please enter question to answer";
            //If question entered and answer not entered
            if (!string.IsNullOrWhiteSpace(txtSecurityQuestion2.Text) && string.IsNullOrWhiteSpace(txtSecurityAnswer2.Text))
            {
                lblErrorQuestion2.Text = strEnterAnswer;
                lblErrorQuestion2.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtSecurityQuestion3.Text) && string.IsNullOrWhiteSpace(txtSecurityAnswer3.Text))
            {
                lblErrorQuestion3.Text = strEnterAnswer;
                lblErrorQuestion3.Focus();
                return;
            }

            //If answer entered and question not entered
            if (string.IsNullOrWhiteSpace(txtSecurityQuestion2.Text) && !string.IsNullOrWhiteSpace(txtSecurityAnswer2.Text))
            {
                lblErrorQuestion2.Text = strEnterQuestion;
                lblErrorQuestion2.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSecurityQuestion3.Text) && !string.IsNullOrWhiteSpace(txtSecurityAnswer3.Text))
            {
                lblErrorQuestion3.Text = strEnterQuestion;
                lblErrorQuestion3.Focus();
                return;
            }

            //Make sure that questions and answers do not repeat

            UserLoggedIn.UpdateQAandPassword(
                UserLoggedIn.UserSecurityID,
                uID,
                txtSecurityQuestion1.Text, "1",
                txtSecurityQuestion2.Text, "1",
                txtSecurityQuestion3.Text, "1",
                txtSecurityAnswer1.Text, "1",
                txtSecurityAnswer2.Text, "1",
                txtSecurityAnswer3.Text, "1",
                string.IsNullOrWhiteSpace(txtPassword.Text) ? UserLoggedIn.Password : txtPassword.Text);

            lblError.Text = "Changes saved successfully.";
        }
    }
}
