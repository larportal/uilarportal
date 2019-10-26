using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LarpPortal.Character.History
{
	public partial class Approve : System.Web.UI.Page
	{
		public bool TextBoxEnabled = true;

		private DataSet _dsHistory = new DataSet();
		const int CHARHISTORY = 0;
		const int STAFFCOMMENTS = 1;
		const int ADDENDUMS = 2;
		const int STAFFADDENDUMCOMMENTS = 3;

		private bool _RELOAD = true;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (_RELOAD)
			{
				MethodBase lmth = MethodBase.GetCurrentMethod();
				string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

				_dsHistory = new DataSet();

				double dCPEarned = 0.0;
				if (Request.QueryString ["CharacterID"] != null)
					hidCharacterID.Value = Request.QueryString ["CharacterID"];
				else
					Response.Redirect("ApprovalList.aspx", true);

				Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
				if (UserInfo.NickName.Length > 0)
					hidAuthorName.Value = UserInfo.NickName + " " + UserInfo.LastName;
				else
					hidAuthorName.Value = UserInfo.FirstName + " " + UserInfo.LastName;

				SortedList sParams = new SortedList();
				sParams.Add("@CharacterID", hidCharacterID.Value);

				int iCharacterID = 0;
				int iUserID = 0;

				_dsHistory = Classes.cUtilities.LoadDataSet("uspGetCharacterHistory", sParams, "LARPortal", Master.UserName, lsRoutineName);

				string sCharacterInfo = "";
				if (_dsHistory.Tables [CHARHISTORY].Rows.Count > 0)
				{
					DataRow drHistory = _dsHistory.Tables [CHARHISTORY].Rows [0];
					sCharacterInfo = "<b>Character: </b> " + drHistory ["CharacterAKA"].ToString();

					int.TryParse(drHistory ["CharacterID"].ToString(), out iCharacterID);
					int.TryParse(drHistory ["CampaignPlayerID"].ToString(), out iUserID);

					hidCampaignName.Value = drHistory ["CampaignName"].ToString();
					hidNotificationEMail.Value = drHistory ["CharacterHistoryNotificationEmail"].ToString();
					if (hidNotificationEMail.Value.Length == 0)
						hidNotificationEMail.Value = "support@larportal.com";

					hidEmail.Value = drHistory ["EmailAddress"].ToString();

					int iCampaignPlayerID = 0;
					if (int.TryParse(drHistory ["CampaignPlayerID"].ToString(), out iCampaignPlayerID))
						hidCampaignPlayerID.Value = iCampaignPlayerID.ToString();

					int.TryParse(drHistory ["CharacterID"].ToString(), out iCharacterID);
					hidCharacterAKA.Value = drHistory ["CharacterAKA"].ToString();
					imgPicture.ImageUrl = "/img/BlankProfile.png";    // Default it to this so if it is not set it will display the blank profile picture.

					hidCharacterID.Value = iCharacterID.ToString();
					hidCampaignID.Value = drHistory ["CampaignID"].ToString();

					lblHistory.Text = drHistory ["CharacterHistory"].ToString().Replace("<ul>", @"<ul style=""list-style-type: disc;"">").Replace("<li>", @"<li style=""margin-left: 15px;"">");

					ckHistory.Text = "Staff has reopened the character history for " + hidCharacterAKA.Value + " for revisions.  Please make changes and resubmit the history.<br><br>" +
						"Thank you<br>" +
						drHistory ["CampaignName"].ToString() + " staff<br><br>" +
						drHistory ["CharacterHistory"].ToString();

					lblCharacterInfo.Text = sCharacterInfo;

					lblPlayedBy.Text = "";
					string sPlayerInfo = "<b>Played by: </b> " + drHistory ["PlayerName"].ToString() + " <a href='mailto:" + drHistory ["EMailAddress"].ToString().Replace(@"""", @"""""") +
						"?Subject=Character History for " + drHistory ["CharacterAKA"].ToString().Replace("'", "''") +
						"' class='LinkUnderline' style='text-decoration: underline; color: blue;'>" + drHistory ["EMailAddress"].ToString() + "</a>";
					lblPlayedBy.Text = sPlayerInfo;

					btnDone.Visible = false;
					btnApprove.Visible = true;

					if (drHistory ["DateHistoryApproved"] != DBNull.Value)
					{
						double.TryParse(drHistory ["CPAwarded"].ToString(), out dCPEarned);
						//btnApprove.Text = "Done";
						//btnApprove.CommandName = "Done";
						DateTime dtTemp;
						if (DateTime.TryParse(drHistory ["DateHistoryApproved"].ToString(), out dtTemp))
						{
							lblEditMessage.Visible = true;
							lblEditMessage.Text = "This history was approved on " + dtTemp.ToShortDateString() + " and cannot be edited.";
							TextBoxEnabled = false;
							btnCancel.Visible = false;
							btnReject.Visible = false;
							btnDone.Visible = true;
							btnApprove.Visible = false;
						}
					}
					else if (drHistory ["DateHistorySubmitted"] != DBNull.Value)
					{
						btnApprove.Text = "Approve";
						btnApprove.CommandName = "Approve";
						btnReject.Visible = true;
						DateTime dtTemp;
						divQuestions.Attributes.Add("style", "max-height: 400px; overflow-y: auto; margin-right: 10px;");
						double.TryParse(drHistory ["CPEarn"].ToString(), out dCPEarned);
						if (DateTime.TryParse(drHistory ["DateHistorySubmitted"].ToString(), out dtTemp))
						{
							lblEditMessage.Visible = true;
							lblEditMessage.Text = "This history was submitted on " + dtTemp.ToShortDateString();
							TextBoxEnabled = false;
							hidSubmitDate.Value = dtTemp.ToShortDateString();
						}
					}
				}

				tbCPAwarded.Text = dCPEarned.ToString("0.0");

				if (_dsHistory.Tables [ADDENDUMS] != null)
				{
					//DataTable dtAddendums = new DataTable();

					//DataView dvAddendum = new DataView(_dsHistory.Tables[ADDENDUMS], "", "DateAdded desc", DataViewRowState.CurrentRows);
					//rptAddendum.DataSource = dvAddendum;
					//rptAddendum.DataBind();

					DataTable dtDispAdd = new DataTable();
					dtDispAdd.Columns.Add("AddendumID", typeof(string));
					dtDispAdd.Columns.Add("Title", typeof(string));
					dtDispAdd.Columns.Add("Addendum", typeof(string));
					dtDispAdd.Columns.Add("DateAdded", typeof(DateTime));

					foreach (DataRow dRow in _dsHistory.Tables [ADDENDUMS].Rows)
					{
						DateTime dtTemp;
						DateTime.TryParse(dRow ["DateAdded"].ToString(), out dtTemp);

						DataRow dNewRow = dtDispAdd.NewRow();
						dNewRow ["AddendumID"] = dRow ["CharacterHistoryAddendumID"].ToString();
						dNewRow ["Title"] = "Addendum added " + dtTemp.ToString();
						dNewRow ["Addendum"] = dRow ["Addendum"];
						dNewRow ["DateAdded"] = dtTemp;
						dtDispAdd.Rows.Add(dNewRow);
					}
					DataView dvDispAdd = new DataView(dtDispAdd, "", "DateAdded desc", DataViewRowState.CurrentRows);
					rptAddendum.DataSource = dvDispAdd;
					rptAddendum.DataBind();
				}

				if (_dsHistory.Tables [STAFFCOMMENTS] != null)
				{
					DataView dvStaffComments = new DataView(_dsHistory.Tables [STAFFCOMMENTS], "", "DateAdded desc", DataViewRowState.CurrentRows);
					dlComments.DataSource = dvStaffComments;
					dlComments.DataBind();
				}
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect("ApprovalList.aspx", true);
		}

		protected void GetComments(string sAnswerID, DataTable dtComments)
		{
			foreach (DataRow dRow in dtComments.Rows)
			{
				string sProfileFileName = HttpContext.Current.Request.PhysicalApplicationPath + dRow ["UserPhoto"].ToString();
				sProfileFileName = sProfileFileName.Replace("~/img/Player/", "img\\Player\\");
				if (!File.Exists(sProfileFileName))
					dRow ["UserPhoto"] = "/img/BlankProfile.png";
			}
		}


		protected void rptAddendum_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
			{
			}
		}

		protected void rptAddendum_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			string i = e.CommandArgument.ToString();
			if (e.CommandName.ToUpper() == "ENTERCOMMENT")
			{
				Panel pnlNewCommentPanel = (Panel) e.Item.FindControl("pnlStaffCommentSection");
				if (pnlNewCommentPanel != null)
				{
					pnlNewCommentPanel.Visible = true;
					Image imgPlayerImage = (Image) e.Item.FindControl("imgStaffCommentProfilePicture");
					if (imgPlayerImage != null)
					{
						Classes.cPlayer PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);
						string pict = PLDemography.UserPhoto;
						imgPicture.Attributes ["onerror"] = "this.src='~/img/BlankProfile.png';";
						imgPlayerImage.ImageUrl = pict;
					}

					Button btnAddComment = (Button) e.Item.FindControl("btnAddStaffComment");
					if (btnAddComment != null)
						btnAddComment.Visible = false;
					_RELOAD = false;
				}
			}
			else if (e.CommandName.ToUpper() == "ADDCOMMENT")
			{
				int iAddendumID;
				if (int.TryParse(e.CommandArgument.ToString(), out iAddendumID))
				{
					TextBox tbNewComment = (TextBox) e.Item.FindControl("tbNewStaffCommentAddendum");
					if (tbNewComment != null)
					{
						if (tbNewComment.Text.Length > 0)
						{
							SortedList sParams = new SortedList();
							sParams.Add("@UserID", Master.UserID);
							sParams.Add("@CharAddendumsStaffCommentID", "-1");
							sParams.Add("@CharAddendumID", iAddendumID);
							sParams.Add("@CommenterID", Master.UserID);
							sParams.Add("@StaffComments", tbNewComment.Text.Trim());

							MethodBase lmth = MethodBase.GetCurrentMethod();
							string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

							DataTable dtAddResponse = Classes.cUtilities.LoadDataTable("uspInsUpdCHCharacterHistoryAddendumsStaffComments", sParams, "LARPortal", Master.UserName, lsRoutineName);

							DataList dlComments = e.Item.FindControl("dlStaffComments") as DataList;
							GetAddendumComments(iAddendumID.ToString(), dtAddResponse, dlComments);
							SendStaffAddendumCommentEMail(dtAddResponse);
						}
					}
					Panel pnlCommentSection = (Panel) e.Item.FindControl("pnlStaffCommentSection");
					if (pnlCommentSection != null)
						pnlCommentSection.Visible = false;
					Button btnAddComment = (Button) e.Item.FindControl("btnAddStaffComment");
					if (btnAddComment != null)
						btnAddComment.Visible = true;
				}
			}
			else if (e.CommandName.ToUpper() == "CANCELCOMMENT")
			{
				Panel pnlCommentSection = (Panel) e.Item.FindControl("pnlStaffCommentSection");
				if (pnlCommentSection != null)
					pnlCommentSection.Visible = false;
				Button btnAddComment = (Button) e.Item.FindControl("btnAddStaffComment");
				if (btnAddComment != null)
					btnAddComment.Visible = true;
			}
		}

		protected void GetAddendumComments(string sAddendumID, DataTable dtAddendums, DataList dtAddendumComments)
		{

			foreach (DataRow dRow in dtAddendums.Rows)
			{
				//string sProfileFileName = HttpContext.Current.Request.PhysicalApplicationPath + dRow["UserPhoto"].ToString();
				//sProfileFileName = sProfileFileName.Replace("~/img/Player/", "img\\Player\\");
				//if (!File.Exists(sProfileFileName))
				dRow ["UserPhoto"] = "/img/BlankProfile.png";
			}

			DataView dvComments = new DataView(_dsHistory.Tables [3], "CharacterHistoryAddendumID = '" + sAddendumID + "'", "DateAdded desc", DataViewRowState.CurrentRows);
			dlComments.DataSource = dvComments;
			dlComments.DataBind();
		}




		protected void SendStaffAddendumCommentEMail(DataTable dtComments)
		{
			if (hidCharacterID.Value.Length > 0)
			{
				//string sSubject = Session["LoginName"].ToString() + " has added a comment to a character history addendum.";
				//string sBody = Session["LoginName"].ToString() + " has added a comment to a character history addendum for " + hidCharacterAKA.Value + "<br><br>";
				string sSubject = hidAuthorName.Value + " has added a comment to a character history addendum.";
				string sBody = hidAuthorName.Value + " has added a comment to a character history addendum for " + hidCharacterAKA.Value + "<br><br>";

				string AddendumText = "";
				string sCommentTable = "<table border='1'><tr><th>Date Added</th><th>Added By</th><th>Comment</th></tr>";

				DataView dvComments = new DataView(dtComments, "", "DateAdded desc", DataViewRowState.CurrentRows);
				foreach (DataRowView dRow in dvComments)
				{
					AddendumText = dRow ["Addendum"].ToString();

					sCommentTable += "<tr><td>";

					DateTime dtTemp;
					if (DateTime.TryParse(dRow ["DateAdded"].ToString(), out dtTemp))
						sCommentTable += dtTemp.ToShortDateString();

					sCommentTable += "</td><td>" + dRow ["UserName"].ToString() + "</td><td>" + dRow ["StaffComments"].ToString() + "</td></tr>";
				}

				sCommentTable += "</table>";

				sBody += "The original addendum was:<br>" + AddendumText.Replace("\n", "<br>") + "<br><br>";
				sBody += "The comments for the addendum are newest first:<br><br>";

				sBody += sCommentTable;

				Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
				cEMS.SendMail(sSubject, sBody, hidNotificationEMail.Value, "", "", "CharacterHistory", Master.UserName);
			}
		}

		#region StaffComment

		protected void btnAddComment_Click(object sender, EventArgs e)
		{
			pnlCommentSection.Visible = true;

			Classes.cPlayer PLDemography = new Classes.cPlayer(Master.UserID, Master.UserName);
			string pict = PLDemography.UserPhoto;
			imgPicture.Attributes ["onerror"] = "this.src='~/img/BlankProfile.png';";
			imgStaffPicture.ImageUrl = pict;

			btnAddComment.Visible = false;
			_RELOAD = false;
		}

		protected void btnSaveNewComment_Click(object sender, EventArgs e)
		{
			if (tbNewComment.Text.Length > 0)
			{
				SortedList sParams = new SortedList();
				sParams.Add("@UserID", Master.UserID);
				sParams.Add("@CharacterID", hidCharacterID.Value);
				sParams.Add("@CharacterHistoryStaffCommentID", "-1");
				sParams.Add("@CommenterID", Master.UserID);
				sParams.Add("@StaffComments", tbNewComment.Text.Trim());

				MethodBase lmth = MethodBase.GetCurrentMethod();
				string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

				Classes.cUtilities.LoadDataTable("uspInsUpdCHCharacterHistoryStaffComments", sParams, "LARPortal", Master.UserName, lsRoutineName);

				sParams = new SortedList();
				sParams.Add("@CharacterID", hidCharacterID.Value);

				_dsHistory = Classes.cUtilities.LoadDataSet("uspGetCharacterHistory", sParams, "LARPortal", Master.UserName, lsRoutineName);
				SendStaffCommentEMail(_dsHistory.Tables [STAFFCOMMENTS]);
			}
			pnlCommentSection.Visible = false;
			btnAddComment.Visible = true;
		}

		protected void btnCancelComment_Click(object sender, EventArgs e)
		{
			pnlCommentSection.Visible = false;
			btnAddComment.Visible = true;
		}

		protected void SendStaffCommentEMail(DataTable dtComments)
		{
			DateTime dtTemp;
			if (hidCharacterID.Value.Length > 0)
			{
				//string sSubject = Session["LoginName"].ToString() + " has added a comment to a character history.";
				//string sBody = Session["LoginName"].ToString() + " has added a comment to a character history for " + hidCharacterAKA.Value + "<br><br>";
				string sSubject = hidAuthorName.Value + " has added a comment to a character history.";
				string sBody = hidAuthorName.Value + " has added a comment to a character history for " + hidCharacterAKA.Value + "<br><br>";

				string sCommentTable = "<table border='1'><tr><th>Date Added</th><th>Added By</th><th>Comment</th></tr>";

				DataView dvComments = new DataView(dtComments, "", "DateAdded desc", DataViewRowState.CurrentRows);
				foreach (DataRowView dRow in dvComments)
				{
					sCommentTable += "<tr><td>";

					if (DateTime.TryParse(dRow ["DateAdded"].ToString(), out dtTemp))
						sCommentTable += dtTemp.ToShortDateString();

					sCommentTable += "</td><td>" + dRow ["UserName"].ToString() + "</td><td>" + dRow ["StaffComments"].ToString() + "</td></tr>";
				}

				sCommentTable += "</table>";
				sBody += sCommentTable;

				Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
				cEMS.SendMail(sSubject, sBody, hidNotificationEMail.Value, "", "", "CharacterHistory", Master.UserName);
			}
		}

		#endregion

		protected void btnReject_Click(object sender, EventArgs e)
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
		}

		protected void btnApprove_Click(object sender, EventArgs e)
		{
			int iCharacterID = -1;
			int iTemp;
			if (int.TryParse(hidCharacterID.Value, out iTemp))
				iCharacterID = iTemp;

			//Classes.cCharacter cChar = new Classes.cCharacter();
			//cChar.LoadCharacter(iCharacterID);

			SortedList sParams = new SortedList();
			sParams.Add("@UserID", Master.UserID);
			sParams.Add("@CharacterID", iCharacterID);

			sParams.Add("@DateHistoryApproved", DateTime.Now);

			Classes.cUtilities.PerformNonQuery("uspInsUpdCHCharacters", sParams, "LARPortal", Master.UserName);
			Session ["UpdateHistoryMessage"] = "alert('The character history has been approved.');";

			Classes.cPoints Points = new Classes.cPoints();
			int CampaignPlayerID = 0;
			int CharacterID = 0;
			int CampaignCPOpportunityDefaultID = 0;
			int CampaignID = 0;
			double CPAwarded = 0.0;

			int.TryParse(hidCampaignPlayerID.Value, out CampaignPlayerID);
			int.TryParse(hidCharacterID.Value, out CharacterID);
			int.TryParse(hidCampaignCPOpportunityDefaultID.Value, out CampaignCPOpportunityDefaultID);
			int.TryParse(hidCampaignID.Value, out CampaignID);
			double.TryParse(tbCPAwarded.Text, out CPAwarded);

			DateTime dtDateSubmitted;
			if (!DateTime.TryParse(hidSubmitDate.Value, out dtDateSubmitted))
				dtDateSubmitted = DateTime.Now;

			Classes.cUser User = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
			string sSubject = "Character history for " + hidCharacterAKA.Value + " had been approved.";

			string sBody = hidCampaignName.Value + " staff has approved the character history for " + hidCharacterAKA.Value + "<br><br>" +
				"You have been awarded " + CPAwarded.ToString() + " CP.";
			//"<br><br>Character History:<br><br>" + ckHistory.Text;
			string sEmailToSendTo = hidEmail.Value;
			Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
			cEMS.SendMail(sSubject, sBody, sEmailToSendTo, "", "", "CharacterHistory", Master.UserName);

			//Classes.cSendNotifications SendNot = new Classes.cSendNotifications();
			//SendNot.SubjectText = sSubject;
			//SendNot.EMailBody = sBody;
			//SendNot.NotifyType = Classes.cNotificationTypes.HISTORYAPPROVE;
			//SendNot.SendNotification(cChar.CurrentUserID, _UserName);

			Points.AssignHistoryPoints(Master.UserID, CampaignPlayerID, CharacterID, CampaignCPOpportunityDefaultID, CampaignID, CPAwarded, dtDateSubmitted);

			Response.Redirect("ApprovalList.aspx", true);
		}

		protected void btnSendMessage_Click(object sender, EventArgs e)
		{
			SortedList sParams = new SortedList();
			sParams.Add("@UserID", Master.UserID);
			sParams.Add("@CharacterID", hidCharacterID.Value);
			sParams.Add("@ClearHistorySubmitted", true);

			Classes.cUtilities.PerformNonQuery("uspInsUpdCHCharacters", sParams, "LARPortal", Master.UserName);
			Session ["UpdateHistoryMessage"] = "alert('The character history has been sent back to the user.');";

			Classes.cUser User = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
			string sSubject = "Character history for " + hidCharacterAKA.Value + " needs revision";
			Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
			cEMS.SendMail(sSubject, ckHistory.Text, hidEmail.Value, "", hidNotificationEMail.Value, "CharacterHistory", Master.UserName);

			Response.Redirect("ApprovalList.aspx", true);
		}

		protected void btnDone_Click(object sender, EventArgs e)
		{
			Response.Redirect("ApprovalList.aspx", true);
		}

		public string ScrubHtml(string value)
		{
			var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
			var step2 = Regex.Replace(step1, @"\s{2,}", " ");
			return step2;
		}

	}
}