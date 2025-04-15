using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character.History
{
    public partial class Edit : System.Web.UI.Page
    {
        bool _Reload = false;

		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
        {
			oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
			btnCloseMessage.Attributes.Add("data-dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
			oCharSelect.LoadInfo();

			try
			{
				if ((!IsPostBack) || (_Reload))
				{
					divHistory.Visible = true;
					divNoCharacter.Visible = false;
					divSaveButtons.Visible = true;

					Classes.cCharacterHistory cCharHist = new Classes.cCharacterHistory();
					cCharHist.Load(oCharSelect.CharacterID.Value, Master.UserID);

					ckEditor.Text = cCharHist.History;
					if (cCharHist.History != null)
						lblHistory.Text = cCharHist.History.Replace("<ul>", @"<ul style=""list-style-type: disc;"">").Replace("<li>", @"<li style=""margin-left: 15px;"">");
					else
						lblHistory.Text = "";

					hidNotificationEMail.Value = cCharHist.NotificationEMail;

					if (cCharHist.Addendums.Count > 0)
					{
						DataTable dtDispAdd = new DataTable();
						dtDispAdd.Columns.Add("Title", typeof(string));
						dtDispAdd.Columns.Add("Addendum", typeof(string));
						dtDispAdd.Columns.Add("DateAdded", typeof(DateTime));

						foreach (Classes.cCharacterHistoryAddendum cAdd in cCharHist.Addendums)
						{
							DataRow dNewRow = dtDispAdd.NewRow();
							dNewRow ["Title"] = "Addendum added " + cAdd.DateAdded.ToString();
							dNewRow ["Addendum"] = cAdd.Addendum;
							dNewRow ["DateAdded"] = cAdd.DateAdded;
							dtDispAdd.Rows.Add(dNewRow);
						}
						DataView dvDispAdd = new DataView(dtDispAdd, "", "DateAdded desc", DataViewRowState.CurrentRows);
						rptAddendum.DataSource = dvDispAdd;
						rptAddendum.DataBind();
					}
					else
					{
						rptAddendum.DataSource = null;
						rptAddendum.DataBind();
					}

					if ((oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters) &&
						(oCharSelect.CharacterInfo.CharacterType != 1))
					{
						btnAddAddendum.Visible = false;
						btnSubmit.Visible = false;
						btnSave.Visible = false;
						ckEditor.Visible = false;
						lblHistory.Visible = true;
					}
					else
					{
						btnAddAddendum.Visible = true;
						btnSubmit.Visible = true;
						btnSave.Visible = true;

						if (cCharHist.DateSubmitted.HasValue)
						{
							ckEditor.Visible = false;
							lblHistory.Visible = true;
							btnSave.Visible = false;
							btnSubmit.Visible = false;
							btnAddAddendum.Visible = true;
						}
						else
						{
							ckEditor.Visible = true;
							lblHistory.Visible = false;
							btnSave.Visible = true;
							btnSubmit.Visible = true;
							btnAddAddendum.Visible = false;
						}
					}
				}
				else
				{
					divHistory.Visible = false;
					divSaveButtons.Visible = false;
					divNoCharacter.Visible = true;
				}
			}
			catch (Exception ex)
			{
				string t = ex.Message;
			}
        }

        protected void ProcessButton(object sender, CommandEventArgs e)
        {
			oCharSelect.LoadInfo();

            Classes.cCharacterHistory cCharHist = new Classes.cCharacterHistory();
            cCharHist.Load(oCharSelect.CharacterID.Value, Master.UserID);
            cCharHist.History = ckEditor.Text;
            if (e.CommandName.ToUpper() == "SUBMIT")
            {
                cCharHist.DateSubmitted = DateTime.Now;
                lblmodalMessage.Text = "The character history has been submitted.";
                SendSubmittedEmail(ckEditor.Text, cCharHist);
            }
            else
                lblmodalMessage.Text = "The character history has been saved.";

            cCharHist.Save(oCharSelect.CharacterID.Value, Master.UserID, Master.UserName);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
            _Reload = true;
        }

        private void SendSubmittedEmail(string sHistory, Classes.cCharacterHistory cHist)
        {
            try
            {
                Classes.cUser User = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
                string sSubject = cHist.CampaignName + " character history from " + cHist.PlayerName + " - " + cHist.CharacterAKA;

                string sBody = (string.IsNullOrEmpty(User.NickName) ? User.FirstName : User.NickName) +
                    " " + User.LastName + " has submitted a character history for " + cHist.CharacterAKA + ".<br><br>" +
                     sHistory;

                if (hidNotificationEMail.Value.Length > 0)
                {
                    Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
                    cEMS.SendMail(sSubject, sBody, cHist.NotificationEMail, "", "", "CharacterHistory", Master.UserName);
                }
                //Classes.cSendNotifications SendNot = new Classes.cSendNotifications();
                //SendNot.SubjectText = sSubject;
                //SendNot.EMailBody = sBody;
                //SendNot.NotifyType = Classes.cNotificationTypes.HISTORYSUBMIT;
                //SendNot.SendNotification(Master.UserID, Master.UserName);
            }
            catch (Exception ex)
            {
                // Write the exception to error log and then throw it again...
                Classes.ErrorAtServer lobjError = new Classes.ErrorAtServer();
                lobjError.ProcessError(ex, "CharacterEdit.aspx.SendSubmittedEmail", "", Session.SessionID);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            AutosaveAnswers();
        }

        protected void AutosaveAnswers()
        {
            //            int iPELID = -1;
            int iTemp = 0;

            // Nothing to do....
            iTemp = iTemp + 15;

        }

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeMessage();", true);
        }

        protected void btnSaveAddendum_Click(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            if (oCharSelect.CharacterInfo != null)
            {
                if (oCharSelect.CharacterID.HasValue)
                {
                    Classes.cCharacterHistoryAddendum cHistAdd = new Classes.cCharacterHistoryAddendum(oCharSelect.CharacterID.Value);
                    cHistAdd.Addendum = CKEAddendum.Text;
                    cHistAdd.Save(Master.UserName, Master.UserID);
                }
            }
            _Reload = true;
        }

        protected void oCharSelect_CharacterChanged(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            if (oCharSelect.CharacterInfo != null)
            {
                if (oCharSelect.CharacterID.HasValue)
                {
                    Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
                    UserInfo.LastLoggedInCampaign = oCharSelect.CampaignID.Value;
                    UserInfo.LastLoggedInCharacter = oCharSelect.CharacterID.Value;
                    UserInfo.LastLoggedInMyCharOrCamp = (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters ? "M" : "C");
                    UserInfo.Save();
					Master.ChangeSelectedCampaign();
                }
                _Reload = true;
            }
        }

		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			string t = sender.GetType().ToString();
			oCharSelect.Reset();
			_Reload = true;
			Classes.cUser user = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
			if (user.LastLoggedInCharacter == -1)
				Response.Redirect("/default.aspx");
		}
	}
}
