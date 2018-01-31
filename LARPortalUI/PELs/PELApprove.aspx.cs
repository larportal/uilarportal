using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.PELs
{
    public partial class PELApprove : System.Web.UI.Page
    {
        public bool TextBoxEnabled = true;
        private DataTable _dtPELComments = new DataTable();
        private DataTable _dtAddendumComments = null;
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
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (!IsPostBack)
            {
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

                DataSet dsQuestions = Classes.cUtilities.LoadDataSet("uspGetPELQuestionsAndAnswers", sParams, "LARPortal", Master.UserName, lsRoutineName);

                string sEventInfo = "";
                if (dsQuestions.Tables[0].Rows.Count > 0)
                {
                    sEventInfo = "<b>Event: </b> " + dsQuestions.Tables[0].Rows[0]["EventDescription"].ToString();

                    hidEventDesc.Value = dsQuestions.Tables[0].Rows[0]["EventDescription"].ToString();
                    hidEventID.Value = dsQuestions.Tables[0].Rows[0]["EventID"].ToString();

                    DateTime dtEventDate;
                    if (DateTime.TryParse(dsQuestions.Tables[0].Rows[0]["EventStartDate"].ToString(), out dtEventDate))
                    {
                        sEventInfo += "&nbsp;&nbsp;<b>Event Date: </b> " + dtEventDate.ToShortDateString();
                        hidEventDate.Value = dtEventDate.ToShortDateString();
                    }

                    int.TryParse(dsQuestions.Tables[0].Rows[0]["CharacterID"].ToString(), out iCharacterID);
                    int.TryParse(dsQuestions.Tables[0].Rows[0]["UserID"].ToString(), out iUserID);

                    sEventInfo += "<br><b>Player: </b> ";
                    if (String.IsNullOrEmpty(dsQuestions.Tables[0].Rows[0]["NickName"].ToString()))
                    {
                        sEventInfo += dsQuestions.Tables[0].Rows[0]["NickName"].ToString() + " " + dsQuestions.Tables[0].Rows[0]["LastName"].ToString();
                        hidPlayerName.Value = dsQuestions.Tables[0].Rows[0]["NickName"].ToString();
                    }
                    else
                    {
                        sEventInfo += dsQuestions.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsQuestions.Tables[0].Rows[0]["LastName"].ToString();
                        hidPlayerName.Value = dsQuestions.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsQuestions.Tables[0].Rows[0]["LastName"].ToString();
                    }

                    string sCharAKA = dsQuestions.Tables[0].Rows[0]["CharacterAKA"].ToString().Replace("'", "''");
                    if (sCharAKA.Length > 0)
                        sCharAKA = "PEL for " + sCharAKA;
                    else
                        sCharAKA = "PEL Comments";

                    sEventInfo += " " + " <a href='mailto:" + dsQuestions.Tables[0].Rows[0]["PlayerEMailAddress"].ToString().Replace(@"""", @"""""") +
                        "?Subject=" + sCharAKA +
                        "' class='LinkUnderline' style='text-decoration: underline; color: blue;'>" + dsQuestions.Tables[0].Rows[0]["PlayerEMailAddress"].ToString() + "</a>";

                    hidPELNotificationEMail.Value = dsQuestions.Tables[0].Rows[0]["PELNotificationEMail"].ToString();
                    if (hidPELNotificationEMail.Value.Length == 0)
                        hidPELNotificationEMail.Value = "support@larportal.com";

                    int iCampaignPlayerID = 0;
                    if (int.TryParse(dsQuestions.Tables[0].Rows[0]["CampaignPlayerID"].ToString(), out iCampaignPlayerID))
                        hidCampaignPlayerID.Value = iCampaignPlayerID.ToString();

                    int.TryParse(dsQuestions.Tables[0].Rows[0]["CharacterID"].ToString(), out iCharacterID);
                    if (iCharacterID != 0)
                    {
                        Classes.cCharacter cChar = new Classes.cCharacter();
                        cChar.LoadCharacter(iCharacterID);
                        sEventInfo += "&nbsp;&nbsp;<b>Character: </b> " + dsQuestions.Tables[0].Rows[0]["CharacterAKA"].ToString();
                        hidCharacterAKA.Value = dsQuestions.Tables[0].Rows[0]["CharacterAKA"].ToString();
                        imgPicture.ImageUrl = "/img/BlankProfile.png";    // Default it to this so if it is not set it will display the blank profile picture.
                        if (cChar.ProfilePicture != null)
                            if (!string.IsNullOrEmpty(cChar.ProfilePicture.PictureURL))
                                imgPicture.ImageUrl = cChar.ProfilePicture.PictureURL;
                        imgPicture.Attributes["onerror"] = "this.src='~/img/BlankProfile.png';";
                        hidCharacterID.Value = iCharacterID.ToString();
                        hidCampaignID.Value = cChar.CampaignID.ToString();
                    }
                    else
                    {
                        Classes.cPlayer PLDemography = null;
                        PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);

                        imgPicture.ImageUrl = "/img/BlankProfile.png";    // Default it to this so if it is not set it will display the blank profile picture.
                        if (!string.IsNullOrEmpty(PLDemography.UserPhoto))
                            imgPicture.ImageUrl = PLDemography.UserPhoto;
                        imgPicture.Attributes["onerror"] = "this.src='~/img/BlankProfile.png';";
                    }

                    lblEventInfo.Text = sEventInfo;

                    int iTemp;
                    if (int.TryParse(dsQuestions.Tables[0].Rows[0]["PELID"].ToString(), out iTemp))
                        hidPELID.Value = iTemp.ToString();
                    if (dsQuestions.Tables[0].Rows[0]["PELDateApproved"] != DBNull.Value)
                    {
                        double.TryParse(dsQuestions.Tables[0].Rows[0]["CPAwarded"].ToString(), out dCPEarned);
                        btnSave.Text = "Done";
                        btnSave.CommandName = "Done";
                        DateTime dtTemp;
                        if (DateTime.TryParse(dsQuestions.Tables[0].Rows[0]["PELDateApproved"].ToString(), out dtTemp))
                        {
                            lblEditMessage.Visible = true;
                            lblEditMessage.Text = "<br>This PEL was approved on " + dtTemp.ToShortDateString() + " and cannot be edited.";
                            TextBoxEnabled = false;
                            btnCancel.Visible = false;
                        }
                    }
                    else if (dsQuestions.Tables[0].Rows[0]["PELDateSubmitted"] != DBNull.Value)
                    {
                        btnSave.Text = "Approve";
                        btnSave.CommandName = "Approve";
                        DateTime dtTemp;
                        divQuestions.Attributes.Add("style", "max-height: 400px; overflow-y: auto; margin-right: 10px;");
                        double.TryParse(dsQuestions.Tables[0].Rows[0]["CPEarn"].ToString(), out dCPEarned);
                        int iCampaignCPOpportunityDefaultID = 0;
                        if (int.TryParse(dsQuestions.Tables[0].Rows[0]["CampaignCPOpportunityDefaultID"].ToString(), out iCampaignCPOpportunityDefaultID))
                            hidCampaignCPOpportunityDefaultID.Value = iCampaignCPOpportunityDefaultID.ToString();
                        int iReasonID = 0;
                        if (int.TryParse(dsQuestions.Tables[0].Rows[0]["ReasonID"].ToString(), out iReasonID))
                            hidReasonID.Value = iReasonID.ToString();

                        if (DateTime.TryParse(dsQuestions.Tables[0].Rows[0]["PELDateSubmitted"].ToString(), out dtTemp))
                        {
                            lblEditMessage.Visible = true;
                            lblEditMessage.Text = "<br>This PEL was submitted on " + dtTemp.ToShortDateString();
                            TextBoxEnabled = false;
                            hidSubmitDate.Value = dtTemp.ToShortDateString();
                        }
                    }
                    if (hidPELID.Value.Length != 0)
                    {
                        // Load the comments for this PEL so we can display them in DataItemBound.
                        SortedList sParamsForComments = new SortedList();
                        sParamsForComments.Add("@PELID", hidPELID.Value);
                        _dtPELComments = Classes.cUtilities.LoadDataTable("uspGetPELStaffComments", sParamsForComments, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELStaffComments");
                    }
                }

                foreach (DataRow dRow in dsQuestions.Tables[0].Rows)
                {
                    dRow["Answer"] = dRow["Answer"].ToString().Replace("\n", "<br>");
                }

                tbCPAwarded.Text = dCPEarned.ToString("0.0");
                DataView dvQuestions = new DataView(dsQuestions.Tables[0], "", "SortOrder", DataViewRowState.CurrentRows);
                rptQuestions.DataSource = dvQuestions;
                rptQuestions.DataBind();

                if (dsQuestions.Tables[2] != null)
                    _dtAddendumComments = dsQuestions.Tables[2];

                if (dsQuestions.Tables[1] != null)
                {
                    DataTable dtNewAddendum = new DataTable();
                    dtNewAddendum.Columns.Add("Title", typeof(string));
                    dtNewAddendum.Columns.Add("Addendum", typeof(string));
                    dtNewAddendum.Columns.Add("PELsAddendumID", typeof(string));

                    DataView dvAddendum = new DataView(dsQuestions.Tables[1], "", "DateAdded desc", DataViewRowState.CurrentRows);
                    foreach (DataRowView dAdd in dvAddendum)
                    {
                        DataRow dNewRow = dtNewAddendum.NewRow();
                        DateTime dtDate;
                        dNewRow["Title"] = "Addendum ";
                        if (DateTime.TryParse(dAdd["DateAdded"].ToString(), out dtDate))
                            dNewRow["Title"] += dtDate.ToString("MM/dd/yyyyy hh:mm:ss tt");
                        dNewRow["Addendum"] = dAdd["Addendum"].ToString();
                        dNewRow["PELsAddendumID"] = dAdd["PELsAddendumID"].ToString();
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

            if ((e.CommandName.ToUpper() == "SAVE") || (e.CommandName.ToUpper() == "SUBMIT"))
            {
                foreach (RepeaterItem item in rptQuestions.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        SortedList sParams = new SortedList();

                        TextBox tbAnswer = (TextBox)item.FindControl("tbAnswer");
                        HiddenField hidQuestionID = (HiddenField)item.FindControl("hidQuestionID");
                        HiddenField hidAnswerID = (HiddenField)item.FindControl("hidAnswerID");
                        int iQuestionID = 0;
                        int iAnswerID = 0;

                        if (hidQuestionID != null)
                            int.TryParse(hidQuestionID.Value, out iQuestionID);
                        if (hidAnswerID != null)
                            int.TryParse(hidAnswerID.Value, out iAnswerID);

                        if (iPELID == 0)
                            iPELID = -1;
                        if (iAnswerID == 0)
                            iAnswerID = -1;

                        sParams.Add("@PELAnswerID", iAnswerID);
                        sParams.Add("@PELQuestionsID", iQuestionID);
                        sParams.Add("@PELID", iPELID);
                        sParams.Add("@Answer", tbAnswer.Text);
                        sParams.Add("@RegistrationID", hidRegistrationID.Value);

                        DataSet dsPELS = Classes.cUtilities.LoadDataSet("uspPELsAnswerInsUpd", sParams, "LARPortal", Master.UserName, lsRoutineName);

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
            }
            if (e.CommandName.ToUpper() == "APPROVE")
            {
                SortedList sParams = new SortedList();
                sParams.Add("@UserID", Master.UserID);
                sParams.Add("@PELID", iPELID);

                double dCPAwarded;
                if (double.TryParse(tbCPAwarded.Text, out dCPAwarded))
                    sParams.Add("@CPAwarded", dCPAwarded);
                sParams.Add("@DateApproved", DateTime.Now);

                Classes.cUtilities.PerformNonQuery("uspInsUpdCMPELs", sParams, "LARPortal", Master.UserName);
                Session["UpdatePELMessage"] = "alert('The PEL has been approved.');";

                Classes.cPoints Points = new Classes.cPoints();
                //int UserID = 0;
                int CampaignPlayerID = 0;
                int CharacterID = 0;
                int CampaignCPOpportunityDefaultID = 0;
                int EventID = 0;
                int ReasonID = 0;
                int CampaignID = 0;
                double CPAwarded = 0.0;

                //int.TryParse(Session["UserID"].ToString(), out UserID);
                int.TryParse(hidCampaignPlayerID.Value, out CampaignPlayerID);
                int.TryParse(hidCharacterID.Value, out CharacterID);
                int.TryParse(hidCampaignCPOpportunityDefaultID.Value, out CampaignCPOpportunityDefaultID);
                int.TryParse(hidReasonID.Value, out ReasonID);
                int.TryParse(hidCampaignID.Value, out CampaignID);
                int.TryParse(hidEventID.Value, out EventID);
                double.TryParse(tbCPAwarded.Text, out CPAwarded);

                DateTime dtDateSubmitted;
                if (!DateTime.TryParse(hidSubmitDate.Value, out dtDateSubmitted))
                    dtDateSubmitted = DateTime.Now;

                Points.AssignPELPoints(Master.UserID, CampaignPlayerID, CharacterID, CampaignCPOpportunityDefaultID, EventID, hidEventDesc.Value, ReasonID, CampaignID, CPAwarded, dtDateSubmitted);
            }

            Response.Redirect("PELApprovalList.aspx", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("PELApprovalList.aspx", true);
        }

        protected void rptQuestions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            string i = e.CommandArgument.ToString();
            if (e.CommandName.ToUpper() == "ENTERCOMMENT")
            {
                Panel pnlNewCommentPanel = (Panel)e.Item.FindControl("pnlCommentSection");
                if (pnlNewCommentPanel != null)
                {
                    pnlNewCommentPanel.Visible = true;
                    Image imgPlayerImage = (Image)e.Item.FindControl("imgProfilePicture");
                    if (imgPlayerImage != null)
                    {
                        //string uName = "";
                        //int uID = 0;

                        //if (Session["Username"] != null)
                        //    uName = Session["Username"].ToString();
                        //if (Session["UserID"] != null)
                        //    int.TryParse(Session["UserID"].ToString(), out uID);

                        Classes.cPlayer PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);
                        string pict = PLDemography.UserPhoto;
                        imgPicture.Attributes["onerror"] = "this.src='~/img/BlankProfile.png';";
						if (String.IsNullOrEmpty(pict.Trim()))
							imgPlayerImage.ImageUrl = "/img/BlankProfile.png";
						else
	                        imgPlayerImage.ImageUrl = pict;
                    }

                    Button btnAddComment = (Button)e.Item.FindControl("btnAddComment");
                    if (btnAddComment != null)
                        btnAddComment.Visible = false;
                }
            }
            else if (e.CommandName.ToUpper() == "ADDCOMMENT")
            {
                int iAnswerID;
                if (int.TryParse(e.CommandArgument.ToString(), out iAnswerID))
                {
                    TextBox tbNewComment = (TextBox)e.Item.FindControl("tbNewComment");
                    if (tbNewComment != null)
                    {
                        if (tbNewComment.Text.Length > 0)
                        {
                            SortedList sParams = new SortedList();
                            sParams.Add("@UserID", Master.UserID);
                            sParams.Add("@PELAnswerID", iAnswerID);
                            sParams.Add("@PELStaffCommentID", "-1");
                            sParams.Add("@CommenterID", Master.UserID);
                            sParams.Add("@StaffComments", tbNewComment.Text.Trim());

                            DataTable dtAddResponse = Classes.cUtilities.LoadDataTable("uspInsUpdCMPELStaffComments", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspInsUpdCMPELStaffComments");

                            SortedList sParamsForComments = new SortedList();
                            sParamsForComments.Add("@PELID", hidPELID.Value);
                            _dtPELComments = Classes.cUtilities.LoadDataTable("uspGetPELStaffComments", sParamsForComments, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELStaffComments");

                            DataList dlComments = e.Item.FindControl("dlComments") as DataList;
                            GetComments(iAnswerID.ToString(), dlComments);
                            SendStaffCommentEMail(_dtPELComments);
                        }
                    }
                    Panel pnlCommentSection = (Panel)e.Item.FindControl("pnlCommentSection");
                    if (pnlCommentSection != null)
                        pnlCommentSection.Visible = false;
                    Button btnAddComment = (Button)e.Item.FindControl("btnAddComment");
                    if (btnAddComment != null)
                        btnAddComment.Visible = true;
                }
            }
            else if (e.CommandName.ToUpper() == "CANCELCOMMENT")
            {
                Panel pnlCommentSection = (Panel)e.Item.FindControl("pnlCommentSection");
                if (pnlCommentSection != null)
                    pnlCommentSection.Visible = false;
                Button btnAddComment = (Button)e.Item.FindControl("btnAddComment");
                if (btnAddComment != null)
                    btnAddComment.Visible = true;
            }
        }

        public void rptQuestions_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView dr = (DataRowView)DataBinder.GetDataItem(e.Item);
                string sAnswerID = dr["PELAnswerID"].ToString();

                DataList dlComments = e.Item.FindControl("dlComments") as DataList;
                if (dlComments != null)
                {
                    GetComments(sAnswerID, dlComments);
                }
                TextBox tbNewComment = (TextBox)e.Item.FindControl("tbNewComment");
                if (tbNewComment != null)
                    tbNewComment.Attributes.Add("PlaceHolder", "Enter comment here.");
            }
        }

        protected void btnAddComment_Command(object sender, CommandEventArgs e)
        {

        }

        protected void GetComments(string sAnswerID, DataList dlComments)
        {
            foreach (DataRow dRow in _dtPELComments.Rows)
            {
                string sProfileFileName = HttpContext.Current.Request.PhysicalApplicationPath + dRow["UserPhoto"].ToString();
                sProfileFileName = sProfileFileName.Replace("~/img/Player/", "img\\Player\\");
                if (!File.Exists(sProfileFileName))
                    dRow["UserPhoto"] = "/img/BlankProfile.png";
            }
            try
            {
                DataView dvComments = new DataView(_dtPELComments, "PELAnswerID = '" + sAnswerID + "'", "DateAdded desc", DataViewRowState.CurrentRows);
                dlComments.DataSource = dvComments;
                dlComments.DataBind();
            }
            catch
            {

            }
        }

        protected void SendStaffCommentEMail(DataTable dtComments)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (hidPELNotificationEMail.Value.Length > 0)
            {
                string sEventDate = "";
                DateTime dtTemp;
                if (DateTime.TryParse(hidEventDate.Value, out dtTemp))
                    sEventDate = " that took place on " + dtTemp.ToShortDateString();

                //string sSubject = Session["LoginName"].ToString() + " has added a comment to a PEL.";
                //string sBody = Session["LoginName"].ToString() + " has added a comment to a PEL for " + hidCharacterAKA.Value + " for the event " + hidEventDesc.Value + sEventDate + "<br><br>";
                string sSubject = hidAuthorName.Value + " has added a comment to a PEL.";
                string sBody = hidAuthorName.Value + " has added a comment to a PEL for " + hidCharacterAKA.Value + " for the event " + hidEventDesc.Value + sEventDate + "<br><br>";

                string sCommentTable = "<table border='1'><tr><th>Date Added</th><th>Added By</th><th>Comment</th></tr>";

                DataView dvComments = new DataView(dtComments, "", "DateAdded desc", DataViewRowState.CurrentRows);
                foreach (DataRowView dRow in dvComments)
                {
                    sCommentTable += "<tr><td>";

                    if (DateTime.TryParse(dRow["DateAdded"].ToString(), out dtTemp))
                        sCommentTable += dtTemp.ToShortDateString();

                    sCommentTable += "</td><td>" + dRow["UserName"].ToString() + "</td><td>" + dRow["StaffComments"].ToString() + "</td></tr>";
                }

                sCommentTable += "</table>";
                sBody += sCommentTable;

                Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
                cEMS.SendMail(sSubject, sBody, hidPELNotificationEMail.Value, "", "", "PELComments", Master.UserName);
            }
        }

        protected void rptAddendum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView dr = (DataRowView)DataBinder.GetDataItem(e.Item);
                string sAnswerID = dr["PELsAddendumID"].ToString();

                //string sAnswerID = e.CommandArgument.ToString();
                DataList dlComments = e.Item.FindControl("dlStaffComments") as DataList;
                if (dlComments != null)
                {
                    GetAddendumComments(sAnswerID, dlComments);
                }
                TextBox tbNewComment = (TextBox)e.Item.FindControl("tbNewStaffCommentAddendum");
                if (tbNewComment != null)
                    tbNewComment.Attributes.Add("PlaceHolder", "Enter comment here.");
            }
        }

        protected void rptAddendum_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            string i = e.CommandArgument.ToString();
            if (e.CommandName.ToUpper() == "ENTERCOMMENT")
            {
                Panel pnlNewCommentPanel = (Panel)e.Item.FindControl("pnlStaffCommentSection");
                if (pnlNewCommentPanel != null)
                {
                    pnlNewCommentPanel.Visible = true;
                    Image imgPlayerImage = (Image)e.Item.FindControl("imgStaffCommentProfilePicture");
                    if (imgPlayerImage != null)
                    {
						imgPicture.ImageUrl = "/img/BlankProfile.png";
                        Classes.cPlayer PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);
                        string pict = PLDemography.UserPhoto;
                        imgPicture.Attributes["onerror"] = "this.src='~/img/BlankProfile.png';";
                        imgPlayerImage.ImageUrl = pict;
                    }

                    Button btnAddComment = (Button)e.Item.FindControl("btnAddStaffComment");
                    if (btnAddComment != null)
                        btnAddComment.Visible = false;
                }
            }
            else if (e.CommandName.ToUpper() == "ADDCOMMENT")
            {
                int iAnswerID;
                if (int.TryParse(e.CommandArgument.ToString(), out iAnswerID))
                {
                    TextBox tbNewComment = (TextBox)e.Item.FindControl("tbNewStaffCommentAddendum");
                    if (tbNewComment != null)
                    {
                        if (tbNewComment.Text.Length > 0)
                        {
                            SortedList sParams = new SortedList();
                            sParams.Add("@UserID", Master.UserID);
                            sParams.Add("@PELsAddendumsStaffCommentID", "-1");
                            sParams.Add("@PELsAddendumID", iAnswerID);
                            sParams.Add("@CommenterID", Master.UserID);
                            sParams.Add("@StaffComments", tbNewComment.Text.Trim());

                            DataTable dtAddResponse = Classes.cUtilities.LoadDataTable("uspInsUpdCMPELsAddendumsStaffComments", sParams, "LARPortal", Master.UserName, 
                                lsRoutineName + ".uspInsUpdCMPELsAddendumsStaffComments");

                            SortedList sParamsForComments = new SortedList();
                            sParamsForComments.Add("@PELID", hidPELID.Value);
                            _dtAddendumComments = Classes.cUtilities.LoadDataTable("uspGetAddendumsStaffComments", sParamsForComments, "LARPortal", Master.UserName, 
                                lsRoutineName + ".uspGetAddendumsStaffComments");

                            DataList dlComments = e.Item.FindControl("dlStaffComments") as DataList;
                            GetAddendumComments(iAnswerID.ToString(), dlComments);
                            SendStaffAddendumCommentEMail(_dtAddendumComments);
                        }
                    }
                    Panel pnlCommentSection = (Panel)e.Item.FindControl("pnlStaffCommentSection");
                    if (pnlCommentSection != null)
                        pnlCommentSection.Visible = false;
                    Button btnAddComment = (Button)e.Item.FindControl("btnAddStaffComment");
                    if (btnAddComment != null)
                        btnAddComment.Visible = true;
                }
            }
            else if (e.CommandName.ToUpper() == "CANCELCOMMENT")
            {
                Panel pnlCommentSection = (Panel)e.Item.FindControl("pnlStaffCommentSection");
                if (pnlCommentSection != null)
                    pnlCommentSection.Visible = false;
                Button btnAddComment = (Button)e.Item.FindControl("btnAddStaffComment");
                if (btnAddComment != null)
                    btnAddComment.Visible = true;
            }
        }

        protected void GetAddendumComments(string sPELsAddendum, DataList dlComments)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (_dtAddendumComments != null)
            {
                foreach (DataRow dRow in _dtAddendumComments.Rows)
                {
                    string sProfileFileName = HttpContext.Current.Request.PhysicalApplicationPath + dRow["UserPhoto"].ToString();
                    sProfileFileName = sProfileFileName.Replace("~/img/Player/", "img\\Player\\");
                    if (!File.Exists(sProfileFileName))
                        dRow["UserPhoto"] = "/img/BlankProfile.png";
                }

                DataView dvComments = new DataView(_dtAddendumComments, "PELsAddendumID = '" + sPELsAddendum + "'", "DateAdded desc", DataViewRowState.CurrentRows);
                dlComments.DataSource = dvComments;
                dlComments.DataBind();
            }
        }

        protected void SendStaffAddendumCommentEMail(DataTable dtComments)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (hidPELNotificationEMail.Value.Length > 0)
            {
                string sEventDate = "";
                DateTime dtTemp;
                if (DateTime.TryParse(hidEventDate.Value, out dtTemp))
                    sEventDate = " that took place on " + dtTemp.ToShortDateString();

                //string sSubject = Session["LoginName"].ToString() + " has added a comment to a PEL Addendum.";
                //string sBody = Session["LoginName"].ToString() + " has added a comment to a PEL Addendum for " + hidCharacterAKA.Value + " for the event " + hidEventDesc.Value + sEventDate + "<br><br>";
                string sSubject = hidAuthorName.Value + " has added a comment to a PEL Addendum.";
                string sBody = hidAuthorName.Value + " has added a comment to a PEL Addendum for " + hidCharacterAKA.Value + " for the event " + hidEventDesc.Value + sEventDate + "<br><br>";

                string AddendumText = "";
                string sCommentTable = "<table border='1'><tr><th>Date Added</th><th>Added By</th><th>Comment</th></tr>";

                DataView dvComments = new DataView(dtComments, "", "DateAdded desc", DataViewRowState.CurrentRows);
                foreach (DataRowView dRow in dvComments)
                {
                    AddendumText = dRow["Addendum"].ToString();

                    sCommentTable += "<tr><td>";

                    if (DateTime.TryParse(dRow["DateAdded"].ToString(), out dtTemp))
                        sCommentTable += dtTemp.ToShortDateString();

                    sCommentTable += "</td><td>" + dRow["UserName"].ToString() + "</td><td>" + dRow["StaffComments"].ToString() + "</td></tr>";
                }

                sCommentTable += "</table>";

                sBody += "The original addendum was:<br>" + AddendumText.Replace("\n", "<br>") + "<br><br>";
                sBody += "The comments for the addendum are newest first:<br><br>";

                sBody += sCommentTable;

                Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
                cEMS.SendMail(sSubject, sBody, hidPELNotificationEMail.Value, "", "", "PELComments", Master.UserName);
            }
        }
    }
}
