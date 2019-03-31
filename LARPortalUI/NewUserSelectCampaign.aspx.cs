using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
	public partial class NewUserSelectCampaign : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (hidUsername.Value == "")
				if (Session["Username"] != null)
					hidUsername.Value = Session["Username"].ToString();

			if (hidUserID.Value == "")
			{
				int iTemp;
				if (Session["UserID"] != null)
					if (int.TryParse(Session["UserID"].ToString(), out iTemp))
						hidUserID.Value = iTemp.ToString();
			}

			if (hidMemberEmailAddress.Value == "")
				hidMemberEmailAddress.Value = Session["MemberEmailAddress"].ToString();
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadCampaigns();
		}


		protected void LoadCampaigns()
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			ddlCampaign.Items.Clear();
			DataTable dtCampaigns = new DataTable();
			SortedList sParams = new SortedList();
			sParams.Add("@GameSystemID", 0);
			sParams.Add("@EndDate", DateTime.Now.ToShortDateString());
			sParams.Add("@GameSystemFilter", 0);
			sParams.Add("@CampaignFilter", 0);
			sParams.Add("@GenreFilter", 0);
			sParams.Add("@StyleFilter", 0);
			sParams.Add("@TechLevelFilter", 0);
			sParams.Add("@SizeFilter", 0);

			dtCampaigns = Classes.cUtilities.LoadDataTable("uspGetCampaignsByName", sParams, "LARPortal", hidUsername.ToString(), lsRoutineName);
			ddlCampaign.DataTextField = "CampaignName";
			ddlCampaign.DataValueField = "CampaignID";
			ddlCampaign.DataSource = dtCampaigns;
			ddlCampaign.DataBind();
			ddlCampaign.Items.Insert(0, new ListItem("Select Campaign", "0"));
			ddlCampaign.SelectedIndex = 0;
		}

		protected void cblRole_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool b = cblRole.Items.Cast<ListItem>().Any(i => i.Selected);
			if (b == true)
			{
				hidRole.Value = "1";
				if (hidCampaign.Value == "1")
					btnSave.Visible = true;
				else
					btnSave.Visible = false;
			}
			else
			{
				hidRole.Value = "0";
				btnSave.Visible = false;
			}
		}

		protected void ddlCampaign_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlCampaign.SelectedIndex == 0)
			{
				hidCampaign.Value = "0";
				btnSave.Visible = false;
			}
			else
			{
				hidCampaign.Value = "1";
				if (hidRole.Value == "1")
					btnSave.Visible = true;
				else
					btnSave.Visible = false;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			int intCampaignID = 0;
			int.TryParse(ddlCampaign.SelectedValue, out intCampaignID);
			string RequestEmail;

			int iUserID = 0;
			if (!int.TryParse(hidUserID.Value, out iUserID))
				iUserID = 0;

			Classes.cCampaignBase Campaign = new Classes.cCampaignBase(intCampaignID, hidUsername.Value, iUserID);
			if (Campaign.JoinRequestEmail == "")
				RequestEmail = Campaign.InfoRequestEmail;
			else
				RequestEmail = Campaign.JoinRequestEmail;
			if (RequestEmail.Contains("@"))
			{
				// It has a "@".  Assume the email format is close enough, go on.
			}
			else
			{
				RequestEmail = "";
			}
			foreach (ListItem item in cblRole.Items)
			{
				if (item.Selected)
				{
					switch (item.Value)
					{
						case "NPC":
							// Permanent NPC needs approval
							if (RequestEmail == "")
								SignUpForSelectedRole(6, iUserID, intCampaignID, 55);
							else
							{
								SendApprovalEmail(intCampaignID, iUserID, 6, RequestEmail);
								SignUpForSelectedRole(10, iUserID, intCampaignID, 56);
							}
							Response.Redirect("~/Default.aspx");
							break;
						case "6False":
							// Permanent NPC no approval
							SignUpForSelectedRole(6, iUserID, intCampaignID, 55);
							break;
						case "EventNPC":
							// Event NPC needs approval
							if (RequestEmail == "")
								SignUpForSelectedRole(7, iUserID, intCampaignID, 55);
							else
							{
								SendApprovalEmail(intCampaignID, iUserID, 7, RequestEmail);
								SignUpForSelectedRole(10, iUserID, intCampaignID, 56);
							}
							Response.Redirect("~/Default.aspx");
							break;
						case "7False":
							// Event NPC no approval
							SignUpForSelectedRole(7, iUserID, intCampaignID, 55);
							break;
						case "PC":
							// PC needs approval
							if (RequestEmail == "")
								SignUpForSelectedRole(8, iUserID, intCampaignID, 55);
							else
							{
								SendApprovalEmail(intCampaignID, iUserID, 8, RequestEmail);
								SignUpForSelectedRole(8, iUserID, intCampaignID, 56);
							}
							Response.Redirect("~/Default.aspx");
							break;
						case "8False":
							// PC no approval
							SignUpForSelectedRole(8, iUserID, intCampaignID, 55);
							break;
						case "10True":
							// NPC needs approval
							if (RequestEmail == "")
								SignUpForSelectedRole(10, iUserID, intCampaignID, 55);
							else
							{
								SendApprovalEmail(intCampaignID, iUserID, 10, RequestEmail);
								SignUpForSelectedRole(10, iUserID, intCampaignID, 56);
							}
							break;
						case "10False":
							// NPC no approval
							SignUpForSelectedRole(10, iUserID, intCampaignID, 55);
							break;
						default:
							// Technically we shouldn't be able to get here so do nothing
							Response.Redirect("~/Default.aspx");
							break;
					}
				}
			}
			Response.Redirect("~/Default.aspx");
		}

		protected void SignUpForSelectedRole(int RoleToSignUp, int UserID, int CampaignID, int StatusID)
		{
			int CampaignPlayerID = 0;
			Classes.cUserCampaign CampaignPlayer = new Classes.cUserCampaign();
			CampaignPlayer.Load(UserID, CampaignID);
			CampaignPlayerID = CampaignPlayer.CampaignPlayerID; // if this comes back empty (-1) make one
			if (CampaignPlayerID == -1)
			{
				CreatePlayerInCampaign(UserID, CampaignID);
				CampaignPlayer.Load(UserID, CampaignID);
				CampaignPlayerID = CampaignPlayer.CampaignPlayerID;
			}
			int RoleAlignment = 2;
			if (RoleToSignUp == 8)
				RoleAlignment = 1;
			Classes.cPlayerRole PlayerRole = new Classes.cPlayerRole();
			PlayerRole.CampaignPlayerRoleID = -1;
			PlayerRole.CampaignPlayerID = CampaignPlayerID;
			PlayerRole.RoleID = RoleToSignUp;
			PlayerRole.RoleAlignmentID = RoleAlignment;
			PlayerRole.Save(UserID);
			if (hidUsername.Value != "")
			{
				Classes.cUser LastLogged = new Classes.cUser(hidUsername.Value, "Password", Session.SessionID);
				string LastCampaign = LastLogged.LastLoggedInCampaign.ToString();
				if (LastCampaign == null || LastCampaign == "0")
				{
					LastLogged.LastLoggedInCampaign = CampaignID;
					LastLogged.Save();
					Session["CampaignID"] = CampaignID;
				}
			}
		}

		protected void CreatePlayerInCampaign(int UserID, int CampaignID)
		{
			Classes.cUserCampaign UserCampaign = new Classes.cUserCampaign();
			UserCampaign.CampaignPlayerID = -1;
			UserCampaign.CampaignID = CampaignID;
			UserCampaign.Save(UserID);
		}

		protected void SendApprovalEmail(int CampaignID, int UserID, int Role, string RequestEmail)
		{
			string strFromUser = "playerservices";
			string strFromDomain = "larportal.com";
			string strFrom = strFromUser + "@" + strFromDomain;
			string strTo = RequestEmail;
			//string strSMTPPassword = "Piccolo1";
			string strSubject = "";
			string strBody = "";
			string CampaignName = "";
			string PlayerFirstName = "";  // Needs defining - Look up based on UserID
			string PlayerLastName = "";  // Needs defining
			string PlayerEmailAddress = "";  // Needs defining
			Classes.cCampaignBase Campaign = new Classes.cCampaignBase(CampaignID, "Username", UserID);
			CampaignName = Campaign.CampaignName;
			Classes.cUser UserInfo = new Classes.cUser(hidUsername.Value, "Password", Session.SessionID);
			PlayerFirstName = UserInfo.FirstName;
			PlayerLastName = UserInfo.LastName;
			PlayerEmailAddress = hidMemberEmailAddress.Value;
			switch (Role)
			{
				case 6:
					strSubject = "Request to be a Permanent NPC for " + CampaignName;
					strBody = "A request to be a permanent NPC " + CampaignName + " has been received through LARP Portal.  The player information is below.<p></p><p></p>";
					strBody = strBody + PlayerFirstName + " " + PlayerLastName + "<p></p>" + PlayerEmailAddress + "<p></p><p></p>";
					strBody = strBody + "Please reply to the player's request within 48 hours.<p></p><p></p>";
					strBody = strBody + "Thank you<p></p><p></p>LARP Portal staff";
					break;
				case 7:
					strSubject = "Request to be an Event NPC for " + CampaignName;
					strBody = "A request to be an Evnet NPC " + CampaignName + " has been received through LARP Portal.  The player information is below.<p></p><p></p>";
					strBody = strBody + PlayerFirstName + " " + PlayerLastName + "<p></p>" + PlayerEmailAddress + "<p></p><p></p>";
					strBody = strBody + "Please reply to the player's request within 48 hours.<p></p><p></p>";
					strBody = strBody + "Thank you<p></p><p></p>LARP Portal staff";
					break;
				case 8:
					strSubject = "Request to PC " + CampaignName;
					strBody = "A request to PC " + CampaignName + " has been received through LARP Portal.  The player information is below.<p></p><p></p>";
					strBody = strBody + PlayerFirstName + " " + PlayerLastName + "<p></p>" + PlayerEmailAddress + "<p></p><p></p>";
					strBody = strBody + "Please reply to the player's request within 48 hours.<p></p><p></p>";
					strBody = strBody + "Thank you<p></p><p></p>LARP Portal staff";
					break;
				case 10:
					strSubject = "Request to NPC " + CampaignName;
					strBody = "A request to NPC " + CampaignName + " has been received through LARP Portal.  The player information is below.<p></p><p></p>";
					strBody = strBody + PlayerFirstName + " " + PlayerLastName + "<p></p>" + PlayerEmailAddress + "<p></p><p></p>";
					strBody = strBody + "Please reply to the player's request within 48 hours.<p></p><p></p>";
					strBody = strBody + "Thank you<p></p><p></p>LARP Portal staff";
					break;
				default:
					break;
			}
			Classes.cEmailMessageService NotifyStaff = new Classes.cEmailMessageService();
			try
			{
				NotifyStaff.SendMail(strSubject, strBody, strTo, "", "", "PointsEmail", hidUsername.Value);
			}
			catch (Exception)
			{
				//lblEmailFailed.Text = "There was an issue. Please contact us at support@larportal.com for assistance.";
			}
		}
	}
}