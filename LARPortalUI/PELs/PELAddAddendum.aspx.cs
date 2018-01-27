using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.PELs
{
    public partial class PELAddAddendum : System.Web.UI.Page
    {
        public bool TextBoxEnabled = true;
        //private //string _UserName = "";
        //private int _UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(Session["Username"].ToString()))
            //    _UserName = Session["Username"].ToString();
            //if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataSet dsQuestions = new DataSet();

                double dCPEarned = 0.0;
                if (Request.QueryString["RegistrationID"] != null)
                    hidRegistrationID.Value = Request.QueryString["RegistrationID"];
                else
                    Response.Redirect("PELApprovalList.aspx", true);

                Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
                if (UserInfo.NickName.Length > 0)
                    hidAuthorName.Value = UserInfo.NickName + " " + UserInfo.LastName;
                else
                    hidAuthorName.Value = UserInfo.FirstName + " " + UserInfo.LastName;

                SortedList sParams = new SortedList();
                sParams.Add("@RegistrationID", hidRegistrationID.Value);

                int iCharacterID = 0;
                int iUserID = 0;

                dsQuestions = Classes.cUtilities.LoadDataSet("uspGetPELQuestionsAndAnswers", sParams, "LARPortal", Master.UserName, "PELEdit.Page_PreRender");

                DataTable dtQuestions = dsQuestions.Tables[0];

                string sEventInfo = "";
                if (dtQuestions.Rows.Count > 0)
                {
                    sEventInfo = "<b>Event: </b> " + dtQuestions.Rows[0]["EventDescription"].ToString();

                    hidEventDesc.Value = dtQuestions.Rows[0]["EventDescription"].ToString();
                    hidEventID.Value = dtQuestions.Rows[0]["EventID"].ToString();

                    DateTime dtEventDate;
                    if (DateTime.TryParse(dtQuestions.Rows[0]["EventStartDate"].ToString(), out dtEventDate))
                    {
                        sEventInfo += "&nbsp;&nbsp;<b>Event Date: </b> " + dtEventDate.ToShortDateString();
                        hidEventDate.Value = dtEventDate.ToShortDateString();
                    }

                    int.TryParse(dtQuestions.Rows[0]["CharacterID"].ToString(), out iCharacterID);
                    int.TryParse(dtQuestions.Rows[0]["UserID"].ToString(), out iUserID);

                    sEventInfo += "&nbsp;&nbsp;<b>Player: </b> ";
                    if (!String.IsNullOrEmpty(dtQuestions.Rows[0]["NickName"].ToString()))
                    {
                        sEventInfo += dtQuestions.Rows[0]["NickName"].ToString();
                        hidPlayerName.Value = dtQuestions.Rows[0]["NickName"].ToString();
                    }
                    else
                    {
                        sEventInfo += dtQuestions.Rows[0]["FirstName"].ToString() + " " + dtQuestions.Rows[0]["LastName"].ToString();
                        hidPlayerName.Value = dtQuestions.Rows[0]["FirstName"].ToString() + " " + dtQuestions.Rows[0]["LastName"].ToString();
                    }

                    hidPELNotificationEMail.Value = dtQuestions.Rows[0]["PELNotificationEMail"].ToString();
                    if (hidPELNotificationEMail.Value.Length == 0)
                        hidPELNotificationEMail.Value = "support@larportal.com,jbradshaw@pobox.com";

                    int iCampaignPlayerID = 0;
                    if (int.TryParse(dtQuestions.Rows[0]["CampaignPlayerID"].ToString(), out iCampaignPlayerID))
                        hidCampaignPlayerID.Value = iCampaignPlayerID.ToString();

                    int.TryParse(dtQuestions.Rows[0]["CharacterID"].ToString(), out iCharacterID);
                    if (iCharacterID != 0)
                    {
                        Classes.cCharacter cChar = new Classes.cCharacter();
                        cChar.LoadCharacter(iCharacterID);
                        sEventInfo += "&nbsp;&nbsp;<b>Character: </b> " + dtQuestions.Rows[0]["CharacterAKA"].ToString();
                        hidCharacterAKA.Value = dtQuestions.Rows[0]["CharacterAKA"].ToString();

                        // TODO Put picture back in in PELAddendum
                        //imgPicture.ImageUrl = "/img/BlankProfile.png";    // Default it to this so if it is not set it will display the blank profile picture.
                        //if (cChar.ProfilePicture != null)
                        //    if (!string.IsNullOrEmpty(cChar.ProfilePicture.PictureURL))
                        //        imgPicture.ImageUrl = cChar.ProfilePicture.PictureURL;
                        //imgPicture.Attributes["onerror"] = "this.src='~/img/BlankProfile.png';";
                        hidCharacterID.Value = iCharacterID.ToString();
                        hidCampaignID.Value = cChar.CampaignID.ToString();
                    }
                    else
                    {
                        Classes.cPlayer PLDemography = null;

                        //string uName = "";
                        //if (!string.IsNullOrEmpty(Session["Username"].ToString()))
                        //    uName = Session["Username"].ToString();

                        PLDemography = new Classes.cPlayer(iUserID, Master.UserName);

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
                        double.TryParse(dtQuestions.Rows[0]["CPAwarded"].ToString(), out dCPEarned);
                        DateTime dtTemp;
                        if (DateTime.TryParse(dtQuestions.Rows[0]["PELDateApproved"].ToString(), out dtTemp))
                        {
                            lblEditMessage.Visible = true;
                            lblEditMessage.Text = "<br>This PEL was approved on " + dtTemp.ToShortDateString() + ".";
                            TextBoxEnabled = false;
                            //btnCancel.Visible = false;
                        }
                    }
                    else if (dtQuestions.Rows[0]["PELDateSubmitted"] != DBNull.Value)
                    {
                        //btnSave.Text = "Approve";
                        //btnSave.CommandName = "Approve";
                        DateTime dtTemp;
                        divQuestions.Attributes.Add("style", "max-height: 400px; overflow-y: auto; margin-right: 10px;");
                        double.TryParse(dtQuestions.Rows[0]["CPEarn"].ToString(), out dCPEarned);
                        int iCampaignCPOpportunityDefaultID = 0;
                        if (int.TryParse(dtQuestions.Rows[0]["CampaignCPOpportunityDefaultID"].ToString(), out iCampaignCPOpportunityDefaultID))
                            hidCampaignCPOpportunityDefaultID.Value = iCampaignCPOpportunityDefaultID.ToString();
                        int iReasonID = 0;
                        if (int.TryParse(dtQuestions.Rows[0]["ReasonID"].ToString(), out iReasonID))
                            hidReasonID.Value = iReasonID.ToString();

                        if (DateTime.TryParse(dtQuestions.Rows[0]["PELDateSubmitted"].ToString(), out dtTemp))
                        {
                            lblEditMessage.Visible = true;
                            lblEditMessage.Text = "<br>This PEL was submitted on " + dtTemp.ToShortDateString();
                            TextBoxEnabled = false;
                            hidSubmitDate.Value = dtTemp.ToShortDateString();
                        }
                    }
                }

                foreach (DataRow dRow in dtQuestions.Rows)
                {
                    dRow["Answer"] = dRow["Answer"].ToString().Replace("\n", "<br>");
                }

                DataView dvQuestions = new DataView(dtQuestions, "", "SortOrder", DataViewRowState.CurrentRows);
                rptQuestions.DataSource = dvQuestions;
                rptQuestions.DataBind();

                if (dsQuestions.Tables[1] != null)
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("PELList.aspx", true);
        }

        protected void btnAddComment_Command(object sender, CommandEventArgs e)
        {

        }

        protected void SendStaffCommentEMail(string AddendumText)
        {
            if (hidPELNotificationEMail.Value.Length > 0)
            {
                string sEventDate = "";
                DateTime dtTemp;
                if (DateTime.TryParse(hidEventDate.Value, out dtTemp))
                    sEventDate = " that took place on " + dtTemp.ToShortDateString();

                // Change from displaying the user login name to the actual player name.
                //string sSubject = Session["UserName"].ToString() + " has added an addendum to a PEL.";
                string sSubject = hidAuthorName.Value + " has added an addendum to a PEL.";
                //string sBody = Session["UserName"].ToString() + " has added an addendum to a PEL for the event " + hidEventDesc.Value + sEventDate + "<br><br>" +
                //    AddendumText.Replace(Environment.NewLine, "<br>");
                string sBody = hidAuthorName.Value + " has added an addendum to a PEL for the event " + hidEventDesc.Value + sEventDate + "<br><br>" +
                    AddendumText.Replace(Environment.NewLine, "<br>");

                Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
                cEMS.SendMail(sSubject, sBody, hidPELNotificationEMail.Value, "", "", "PELAddendum", Master.UserName);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iPELID = -1;
            int iTemp;
            if (int.TryParse(hidPELID.Value, out iTemp))
                iPELID = iTemp;

            SortedList sParams = new SortedList();
            sParams.Add("@PELID", iPELID.ToString());
            sParams.Add("@Addendum", tbAddendum.Text);

            Classes.cUtilities.PerformNonQuery("uspInsUpdCMPELsAddendums", sParams, "LARPortal", Master.UserName);

            SendStaffCommentEMail(tbAddendum.Text);

            Session["UpdatePELMessage"] = "Your addendum has been added to your PEL.";

            Response.Redirect("PELList.aspx", true);
        }
    }
}
