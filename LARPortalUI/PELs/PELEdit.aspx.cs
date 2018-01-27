using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.PELs
{
    public partial class PELEdit : System.Web.UI.Page
    {
        public bool TextBoxEnabled = true;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (hidTextBoxEnabled.Value == "1")
                TextBoxEnabled = true;

            if (!IsPostBack)
            {
                DataSet dsQuestions = new DataSet();

                if (Request.QueryString["RegistrationID"] == null)
                    Response.Redirect("PELList.aspx", true);

                hidRegistrationID.Value = Request.QueryString["RegistrationID"];

                SortedList sParams = new SortedList();
                sParams.Add("@RegistrationID", hidRegistrationID.Value);

                dsQuestions = Classes.cUtilities.LoadDataSet("uspGetPELQuestionsAndAnswers", sParams, "LARPortal", Master.UserName, "PELEdit.Page_PreRender");

                int iCharacterID = 0;
                int iUserID = 0;

                DataTable dtQuestions = dsQuestions.Tables[0];

                string sEventInfo = "";
                if (dtQuestions.Rows.Count > 0)
                {
                    hidPELTemplateID.Value = dtQuestions.Rows[0]["PELTemplateID"].ToString();
                    sEventInfo = "<b>Event: </b> " + dtQuestions.Rows[0]["EventName"].ToString();

                    DateTime dtEventDate;
                    if (DateTime.TryParse(dtQuestions.Rows[0]["EventStartDate"].ToString(), out dtEventDate))
                    {
                        sEventInfo += "&nbsp;&nbsp;<b>Event Date: </b> " + dtEventDate.ToShortDateString();
                        ViewState["EventDate"] = dtEventDate.ToShortDateString();
                    }

                    ViewState["EventName"] = dtQuestions.Rows[0]["EventName"].ToString();
                    ViewState["PELNotificationEMail"] = dtQuestions.Rows[0]["PELNotificationEMail"].ToString();
                    string sPlayerName = dtQuestions.Rows[0]["NickName"].ToString();
                    if (sPlayerName.Length == 0)
                        sPlayerName = dtQuestions.Rows[0]["FirstName"].ToString();
                    sPlayerName += " " + dtQuestions.Rows[0]["LastName"].ToString();
                    ViewState["PlayerName"] = sPlayerName;

                    int.TryParse(dtQuestions.Rows[0]["CharacterID"].ToString(), out iCharacterID);
                    int.TryParse(dtQuestions.Rows[0]["UserID"].ToString(), out iUserID);

                    if (iCharacterID != 0)
                    {
                        // A character.
                        sEventInfo += "&nbsp;&nbsp;<b>Character: </b> " + dtQuestions.Rows[0]["CharacterAKA"].ToString();
                        ViewState["CharacterAKA"] = dtQuestions.Rows[0]["CharacterAKA"].ToString();
                    }
                    else
                    {
                        // Non character.
                        sEventInfo += "&nbsp;&nbsp;<b>Player: </b> " + dtQuestions.Rows[0]["NickName"].ToString();
                        ViewState["CharacterAKA"] = "a non-player.";
                    }

                    int.TryParse(dtQuestions.Rows[0]["CharacterID"].ToString(), out iCharacterID);
                    if (iCharacterID != 0)
                    {
                        Classes.cCharacter cChar = new Classes.cCharacter();
                        cChar.LoadCharacter(iCharacterID);
                        //imgPicture.ImageUrl = "/img/BlankProfile.png";    // Default it to this so if it is not set it will display the blank profile picture.
                        //if (cChar.ProfilePicture != null)
                        //    if (!string.IsNullOrEmpty(cChar.ProfilePicture.PictureURL))
                        //        imgPicture.ImageUrl = cChar.ProfilePicture.PictureURL;
                        //imgPicture.Attributes["onerror"] = "this.src='~/img/BlankProfile.png';";

                    }
                    else
                    {
                        Classes.cPlayer PLDemography = null;

                        //string uName = "";
                        //int uID = 0;
                        //if (!string.IsNullOrEmpty(Session["Username"].ToString()))
                        //    uName = Session["Username"].ToString();
                        //int.TryParse(Session["UserID"].ToString(), out uID);

                        PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);

                        //imgPicture.ImageUrl = "/img/BlankProfile.png";    // Default it to this so if it is not set it will display the blank profile picture.
                        //if (!string.IsNullOrEmpty(PLDemography.UserPhoto))
                        //    imgPicture.ImageUrl = PLDemography.UserPhoto;
                        //imgPicture.Attributes["onerror"] = "this.src='~/img/BlankProfile.png';";
                    }

                    lblEventInfo.Text = sEventInfo;

                    int iTemp;
                    if (int.TryParse(dtQuestions.Rows[0]["PELID"].ToString(), out iTemp))
                        hidPELID.Value = iTemp.ToString();
                    if (dtQuestions.Rows[0]["PELDateApproved"] != DBNull.Value)
                    {
                        btnSubmit.Visible = false;
                        pnlSaveReminder.Visible = false;
                        btnSave.Text = "Done";
                        btnSave.CommandName = "Done";
                        DateTime dtTemp;
                        if (DateTime.TryParse(dtQuestions.Rows[0]["PELDateApproved"].ToString(), out dtTemp))
                        {
                            lblEditMessage.Visible = true;
                            lblEditMessage.Text = "<br>This PEL was approved on " + dtTemp.ToShortDateString() + " and cannot be edited.";
                            TextBoxEnabled = false;
                            hidTextBoxEnabled.Value = "0";
                            btnCancel.Visible = false;
                            foreach (DataRow dRow in dtQuestions.Rows)
                            {
                                dRow["Answer"] = dRow["Answer"].ToString().Replace("\n", "<br>");
                            }
                        }
                    }
                    else if (dtQuestions.Rows[0]["PELDateSubmitted"] != DBNull.Value)
                    {
                        btnSubmit.Visible = false;
                        pnlSaveReminder.Visible = false;
                        btnSave.Text = "Done";
                        btnSave.CommandName = "Done";
                        DateTime dtTemp;
                        if (DateTime.TryParse(dtQuestions.Rows[0]["PELDateSubmitted"].ToString(), out dtTemp))
                        {
                            lblEditMessage.Visible = true;
                            lblEditMessage.Text = "<br>This PEL was submitted on " + dtTemp.ToShortDateString() + " and cannot be edited.";
                            TextBoxEnabled = false;
                            hidTextBoxEnabled.Value = "0";
                            btnCancel.Visible = false;
                            foreach (DataRow dRow in dtQuestions.Rows)
                            {
                                dRow["Answer"] = dRow["Answer"].ToString().Replace("\n", "<br>");
                            }
                        }
                    }
                }

                DataView dvQuestions = new DataView(dtQuestions, "", "SortOrder", DataViewRowState.CurrentRows);
                rptQuestions.DataSource = dvQuestions;
                rptQuestions.DataBind();

                if (dsQuestions.Tables.Count > 1)
                {
                    DataTable dtNewAddendum = new DataTable();
                    dtNewAddendum.Columns.Add("Title", typeof(string));
                    dtNewAddendum.Columns.Add("Addendum", typeof(string));

                    DataView dvAddendum = new DataView(dsQuestions.Tables[1], "", "DateAdded desc", DataViewRowState.CurrentRows);
                    foreach (DataRowView dAdd in dvAddendum)
                    {
                        DataRow dNewRow = dtNewAddendum.NewRow();
                        DateTime dtDate;
                        dNewRow["Title"] = "Addendum ";
                        if (DateTime.TryParse(dAdd["DateAdded"].ToString(), out dtDate))
                            dNewRow["Title"] += dtDate.ToString("MM/dd/yyyy hh:mm:ss tt");
                        dNewRow["Addendum"] = dAdd["Addendum"].ToString();
                        dtNewAddendum.Rows.Add(dNewRow);
                    }

                    rptAddendum.DataSource = dtNewAddendum;
                    rptAddendum.DataBind();
                }
            }
        }

        protected void ProcessButton(object sender, CommandEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            int iPELID = -1;
            int iTemp;
            if (int.TryParse(hidPELID.Value, out iTemp))
                iPELID = iTemp;

            // Need to build the body for the e-mail we are going to send.
            //Characters
            //    Event
            //    Player

            string sEMailBody = "";

            if ((e.CommandName.ToUpper() == "SAVE") || (e.CommandName.ToUpper() == "SUBMIT"))
            {
                foreach (RepeaterItem item in rptQuestions.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        SortedList sParams = new SortedList();

                        Label lblQuestion = (Label)item.FindControl("lblQuestion");
                        TextBox tbAnswer = (TextBox)item.FindControl("tbAnswer");
                        HiddenField hidQuestionID = (HiddenField)item.FindControl("hidQuestionID");
                        HiddenField hidAnswerID = (HiddenField)item.FindControl("hidAnswerID");
                        int iQuestionID = 0;
                        int iAnswerID = 0;
                        int iPELTemplateID = 0;

                        if (hidQuestionID != null)
                            int.TryParse(hidQuestionID.Value, out iQuestionID);
                        if (hidAnswerID != null)
                            int.TryParse(hidAnswerID.Value, out iAnswerID);
                        if (hidPELTemplateID != null)
                            int.TryParse(hidPELTemplateID.Value, out iPELTemplateID);

                        if (iPELID == 0)
                            iPELID = -1;
                        if (iAnswerID == 0)
                            iAnswerID = -1;

                        sParams.Add("@PELAnswerID", iAnswerID);
                        sParams.Add("@PELQuestionsID", iQuestionID);
                        sParams.Add("@PELID", iPELID);
                        sParams.Add("@PELTemplateID", iPELTemplateID);
                        sParams.Add("@Answer", tbAnswer.Text);
                        sParams.Add("@RegistrationID", hidRegistrationID.Value);

                        sEMailBody += lblQuestion.Text + "<br>" +
                            tbAnswer.Text.Replace(Environment.NewLine, "<br>") + "<br><br>";

                        DataSet dsPELS = new DataSet();
                        dsPELS = Classes.cUtilities.LoadDataSet("uspPELsAnswerInsUpd", sParams, "LARPortal", Master.UserName, lsRoutineName);

                        if ((dsPELS.Tables.Count > 1) && (iPELID == -1))
                        {
                            if (int.TryParse(dsPELS.Tables[1].Rows[0]["PELID"].ToString(), out iTemp))
                            {
                                iPELID = iTemp;
                                hidPELID.Value = iTemp.ToString();
                            }
                        }
                    }
                    Session["UpdatePELMessage"] = "alert('The PEL has been saved.');";
                }

            }
            if (e.CommandName.ToUpper() == "SUBMIT")
            {
                SortedList sParams = new SortedList();
                sParams.Add("@UserID", Master.UserID);
                sParams.Add("@PELID", iPELID);
                sParams.Add("@DateSubmitted", DateTime.Now);

                Classes.cUtilities.PerformNonQuery("uspInsUpdCMPELs", sParams, "LARPortal", Master.UserName);
                Session["UpdatePELMessage"] = "alert('The PEL has been saved and submitted.');";
                SendEmailPELSubmitted(sEMailBody);
            }

            Response.Redirect("PELList.aspx", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("PELList.aspx", true);
        }

        private void SendEmailPELSubmitted(string sEmailBody)
        {
            try
            {
                if (ViewState["PELNotificationEMail"].ToString().Length > 0)
                {
                    string sPlayerName = ViewState["PlayerName"].ToString();
                    string sCharacterName = "";
                    if (ViewState["CharacterAKA"] != null)
                        sCharacterName = ViewState["CharacterAKA"].ToString();

                    string sEventDate = "";
                    if (ViewState["EventDate"] != null)
                    {
                        DateTime dtTemp;
                        if (DateTime.TryParse(ViewState["EventDate"].ToString(), out dtTemp))
                            sEventDate = " that took place on " + ViewState["EventDate"].ToString();
                    }

                    string sEventName = ViewState["EventName"].ToString();
                    string sPELNotificationEMail = ViewState["PELNotificationEMail"].ToString();

                    string sSubject = "PEL Submitted: " + sPlayerName + " has submitted a PEL.";
                    string sBody = sPlayerName + " has submitted a PEL for " + sCharacterName + " for the event " + sEventName + sEventDate + "<br><br>" +
                        sEmailBody;

                    Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
                    cEMS.SendMail(sSubject, sBody, sPELNotificationEMail, "", "", "PEL", Master.UserName);
                }
            }
            catch (Exception ex)
            {
                // Write the exception to error log and then throw it again...
                Classes.ErrorAtServer lobjError = new Classes.ErrorAtServer();
                lobjError.ProcessError(ex, "PELEdit.aspx.SendEmailPELSubmitted", "", Session.SessionID);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            AutosaveAnswers();
        }

        protected void AutosaveAnswers()
        {
            int iPELID = -1;
            int iTemp = 0;

            if (int.TryParse(hidPELID.Value, out iTemp))
                iPELID = iTemp;

            foreach (RepeaterItem item in rptQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    SortedList sParams = new SortedList();

                    Label lblQuestion = (Label)item.FindControl("lblQuestion");
                    TextBox tbAnswer = (TextBox)item.FindControl("tbAnswer");
                    HiddenField hidQuestionID = (HiddenField)item.FindControl("hidQuestionID");
                    HiddenField hidAnswerID = (HiddenField)item.FindControl("hidAnswerID");
                    int iQuestionID = 0;
                    int iAnswerID = 0;
                    int iPELTemplateID = 0;

                    if (hidQuestionID != null)
                        int.TryParse(hidQuestionID.Value, out iQuestionID);
                    if (hidAnswerID != null)
                        int.TryParse(hidAnswerID.Value, out iAnswerID);
                    if (hidPELTemplateID != null)
                        int.TryParse(hidPELTemplateID.Value, out iPELTemplateID);

                    if (iPELID == 0)
                        iPELID = -1;
                    if (iAnswerID == 0)
                        iAnswerID = -1;

                    sParams.Add("@PELAnswerID", iAnswerID);
                    sParams.Add("@PELQuestionsID", iQuestionID);
                    sParams.Add("@PELID", iPELID);
                    sParams.Add("@PELTemplateID", iPELTemplateID);
                    sParams.Add("@Answer", tbAnswer.Text);
                    sParams.Add("@RegistrationID", hidRegistrationID.Value);

                    DataSet dsPELS = new DataSet();
                    dsPELS = Classes.cUtilities.LoadDataSet("uspPELsAnswerInsUpd", sParams, "LARPortal", Master.UserName, "PELEdit.btnSave_Click");
                    if (iAnswerID == -1)
                    {
                        int.TryParse(dsPELS.Tables[0].Rows[0]["PELAnswerID"].ToString(), out iTemp);
                        hidAnswerID.Value = iTemp.ToString();
                    }

                    if ((dsPELS.Tables.Count > 1) && (iPELID == -1))
                    {
                        if (int.TryParse(dsPELS.Tables[1].Rows[0]["PELID"].ToString(), out iTemp))
                        {
                            iPELID = iTemp;
                            hidPELID.Value = iTemp.ToString();
                        }
                    }
                }
            }
        }
    }
}
