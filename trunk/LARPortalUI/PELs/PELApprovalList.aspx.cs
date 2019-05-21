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
	public partial class PELApprovalList : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ViewState["CharacterName"] = "";
				ViewState["EventDate"] = "";
				ViewState["EventName"] = "";
				ViewState["PELStatus"] = "";
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			if (Session["UpdatePELMessage"] != null)
			{
				string jsString = Session["UpdatePELMessage"].ToString();
				ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", jsString, true);
				Session.Remove("UpdatePELMessage");
			}
			BindData();
		}

		protected void BindData()
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@CampaignID", Master.CampaignID);

			DataTable dtPELs = Classes.cUtilities.LoadDataTable("uspGetPELsToApprove", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELsToApprove");

			if (!IsPostBack)
			{
				Classes.cUserOptions OptionsLoader = new Classes.cUserOptions();
				OptionsLoader.LoadUserOptions(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath, "", "");
				foreach (Classes.cUserOption Option in OptionsLoader.UserOptionList)
				{
					if ((Option.ObjectName.ToUpper() == "DDLCHARACTERNAME") &&
						(Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
						ViewState["CharacterName"] = Option.OptionValue;
					else if ((Option.ObjectName.ToUpper() == "DDLEVENTDATE") &&
							(Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
						ViewState["EventDate"] = Option.OptionValue;
					else if ((Option.ObjectName.ToUpper() == "DDLEVENTNAME") &&
							(Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
						ViewState["EventName"] = Option.OptionValue;
					else if ((Option.ObjectName.ToUpper() == "DDLSTATUS") &&
							(Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
						ViewState["PELStatus"] = Option.OptionValue;
				}
			}

			foreach (DataRow dRow in dtPELs.Rows)
			{
				if (dRow["RoleAlignment"].ToString() != "PC")
					dRow["CharacterAKA"] = dRow["RoleAlignment"].ToString();
			}

			// While creating the filter am also saving the selected values so we can go back and have the drop down list use them.
			string sRowFilter = "(1 = 1)";      // This is so it's easier to build the filter string. Now can always say 'and ....'

			if (ViewState["CharacterName"].ToString().Length > 0)
			{
				sRowFilter += " and (CharacterAKA = '" + ViewState["CharacterName"].ToString().Replace("'", "''") + "')";
			}

			if (ViewState["EventDate"].ToString().Length > 0)
			{
				sRowFilter += " and (EventStartDate = '" + ViewState["EventDate"].ToString() + "')";
			}

			if (ViewState["EventName"].ToString().Length > 0)
			{
				sRowFilter += " and (EventName = '" + ViewState["EventName"].ToString().Replace("'", "''") + "')";
			}

			if (ViewState["PELStatus"].ToString().Length > 0)
			{
				sRowFilter += " and (PELStatus = '" + ViewState["PELStatus"].ToString().Replace("'", "''") + "')";
			}

			DataView dvPELs = new DataView(dtPELs, sRowFilter, "PELStatus desc, DateSubmitted", DataViewRowState.CurrentRows);
			if (dvPELs.Count == 0)
			{
				dvPELs.RowFilter = "";
				ViewState["CharacterName"] = "";
				ViewState["EventDate"] = "";
				ViewState["EventName"] = "";
				ViewState["PELStatus"] = "";
				sRowFilter = "";
			}

			gvPELList.DataSource = dvPELs;
			gvPELList.DataBind();

			Session["PELApprovalList_dvPELs"] = dvPELs;

			DataView view = new DataView(dtPELs, sRowFilter, "EventName", DataViewRowState.CurrentRows);
			DataTable dtDistinctEvents = view.ToTable(true, "EventName");

			ddlEventName.DataSource = dtDistinctEvents;
			ddlEventName.DataTextField = "EventName";
			ddlEventName.DataValueField = "EventName";
			ddlEventName.DataBind();
			ddlEventName.Items.Insert(0, new ListItem("No Filter", ""));
			ddlEventName.SelectedIndex = -1;
			if (ViewState["EventName"].ToString().Length > 0)
				foreach (ListItem li in ddlEventName.Items)
					if (li.Value == ViewState["EventName"].ToString())
					{
						ddlEventName.ClearSelection();
						li.Selected = true;
					}
					else
						li.Selected = false;
			if (ddlEventName.SelectedIndex == -1)     // Didn't find what was selected.
				ddlEventName.SelectedIndex = 0;

			view = new DataView(dtPELs, sRowFilter, "CharacterAKA", DataViewRowState.CurrentRows);
			DataTable dtDistinctChars = view.ToTable(true, "CharacterAKA");

			ddlCharacterName.DataSource = dtDistinctChars;
			ddlCharacterName.DataTextField = "CharacterAKA";
			ddlCharacterName.DataValueField = "CharacterAKA";
			ddlCharacterName.DataBind();
			ddlCharacterName.Items.Insert(0, new ListItem("No Filter", ""));
			ddlCharacterName.SelectedIndex = -1;
			if (ViewState["CharacterName"].ToString().Length > 0)
				foreach (ListItem li in ddlCharacterName.Items)
					if (li.Value == ViewState["CharacterName"].ToString())
					{
						ddlCharacterName.ClearSelection();
						li.Selected = true;
					}
					else
						li.Selected = false;
			if (ddlCharacterName.SelectedIndex == -1)     // Didn't find what was selected.
				ddlCharacterName.SelectedIndex = 0;

			view = new DataView(dtPELs, sRowFilter, "EventStartDate", DataViewRowState.CurrentRows);
			DataTable dtDistinctDates = view.ToTable(true, "EventStartDateStr");

			ddlEventDate.DataSource = dtDistinctDates;
			ddlEventDate.DataTextField = "EventStartDateStr";
			ddlEventDate.DataValueField = "EventStartDateStr";
			ddlEventDate.DataBind();
			ddlEventDate.Items.Insert(0, new ListItem("No Filter", ""));
			ddlEventDate.SelectedIndex = -1;
			if (ViewState["EventDate"].ToString().Length > 0)
				foreach (ListItem li in ddlEventDate.Items)
					if (li.Value == ViewState["EventDate"].ToString())
					{
						ddlEventDate.ClearSelection();
						li.Selected = true;
					}
					else
						li.Selected = false;
			if (ddlEventDate.SelectedIndex == -1)     // Didn't find what was selected.
				ddlEventDate.SelectedIndex = 0;

			view = new DataView(dtPELs, sRowFilter, "PELStatus desc", DataViewRowState.CurrentRows);
			DataTable dtDistinctStatus = view.ToTable(true, "PELStatus");

			ddlStatus.DataSource = dtDistinctStatus;
			ddlStatus.DataTextField = "PELStatus";
			ddlStatus.DataValueField = "PELStatus";
			ddlStatus.DataBind();
			ddlStatus.Items.Insert(0, new ListItem("No Filter", ""));
			ddlStatus.SelectedIndex = -1;
			if (ViewState["PELStatus"].ToString().Length > 0)
				foreach (ListItem li in ddlStatus.Items)
					if (li.Value == ViewState["PELStatus"].ToString())
					{
						ddlStatus.ClearSelection();
						li.Selected = true;
					}
					else
						li.Selected = false;
			if (ddlStatus.SelectedIndex == -1)     // Didn't find what was selected.
				ddlStatus.SelectedIndex = 0;
		}

		protected void gvPELList_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			string sRegistrationID = e.CommandArgument.ToString();
			Response.Redirect("PELApprove.aspx?RegistrationID=" + sRegistrationID, false);
		}

		protected void btnApproveAll_Click(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			DataView dvPELs = Session["PELApprovalList_dvPELs"] as DataView;
			dvPELs.RowFilter = dvPELs.RowFilter + " and (DateApproved is null)";
			foreach (DataRowView dRow in dvPELs)
			{
				int iRegistrationID;
				if (int.TryParse(dRow["RegistrationID"].ToString(), out iRegistrationID))
				{
					int iPELID;

					int.TryParse(dRow["PELID"].ToString(), out iPELID);

					SortedList sParams = new SortedList();
					sParams.Add("@UserID", Master.UserID);
					sParams.Add("@PELID", iPELID);

					double dCPAwarded;
					if (double.TryParse(dRow["CPValue"].ToString(), out dCPAwarded))
						sParams.Add("@CPAwarded", dCPAwarded);
					sParams.Add("@DateApproved", DateTime.Now);

					Classes.cUtilities.PerformNonQuery("uspInsUpdCMPELs", sParams, "LARPortal", Master.UserName);

					Classes.cPoints Points = new Classes.cPoints();
					//int UserID = 0;
					int CampaignPlayerID = 0;
					int CharacterID = 0;
					int CampaignCPOpportunityDefaultID = 0;
					int EventID = 0;
					int ReasonID = 0;
					int CampaignID = 0;
					double CPAwarded = 0.0;
					DateTime dtDateSubmitted = DateTime.Now;

					//int.TryParse(Session["UserID"].ToString(), out UserID);
					int.TryParse(dRow["CampaignPlayerID"].ToString(), out CampaignPlayerID);
					int.TryParse(dRow["CharacterID"].ToString(), out CharacterID);
					int.TryParse(dRow["CampaignCPOpportunityDefaultID"].ToString(), out CampaignCPOpportunityDefaultID);
					int.TryParse(dRow["ReasonID"].ToString(), out ReasonID);
					int.TryParse(dRow["CampaignID"].ToString(), out CampaignID);
					int.TryParse(dRow["EventID"].ToString(), out EventID);
					double.TryParse(dRow["CPValue"].ToString(), out CPAwarded);
					DateTime.TryParse(dRow["DateSubmitted"].ToString(), out dtDateSubmitted);

					Points.AssignPELPoints(Master.UserID, CampaignPlayerID, CharacterID, CampaignCPOpportunityDefaultID, EventID, dRow["EventName"].ToString(), ReasonID, CampaignID, CPAwarded, dtDateSubmitted);
				}
			}
		}

		protected void ddlEventDate_SelectedIndexChanged(object sender, EventArgs e)
		{
			Classes.cUserOption Option = new Classes.cUserOption();
			Option.LoginUsername = Session["UserName"].ToString();
			Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
			Option.ObjectName = "ddlEventDate";
			Option.ObjectOption = "SelectedValue";
			Option.OptionValue = ddlEventDate.SelectedValue;
			Option.SaveOptionValue();
			ViewState["EventDate"] = ddlEventDate.SelectedValue;
		}

		protected void ddlCharacterName_SelectedIndexChanged(object sender, EventArgs e)
		{
			Classes.cUserOption Option = new Classes.cUserOption();
			Option.LoginUsername = Session["UserName"].ToString();
			Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
			Option.ObjectName = "ddlCharacterName";
			Option.ObjectOption = "SelectedValue";
			Option.OptionValue = ddlCharacterName.SelectedValue;
			Option.SaveOptionValue();

			ViewState["CharacterName"] = ddlCharacterName.SelectedValue;
		}

		protected void ddlEventName_SelectedIndexChanged(object sender, EventArgs e)
		{
			Classes.cUserOption Option = new Classes.cUserOption();
			Option.LoginUsername = Session["UserName"].ToString();
			Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
			Option.ObjectName = "ddlEventName";
			Option.ObjectOption = "SelectedValue";
			Option.OptionValue = ddlEventName.SelectedValue;
			Option.SaveOptionValue();

			ViewState["EventName"] = ddlEventName.SelectedValue;
		}

		protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			Classes.cUserOption Option = new Classes.cUserOption();
			Option.LoginUsername = Session["UserName"].ToString();
			Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
			Option.ObjectName = "ddlStatus";
			Option.ObjectOption = "SelectedValue";
			Option.OptionValue = ddlStatus.SelectedValue;
			Option.SaveOptionValue();

			ViewState["PELStatus"] = ddlStatus.SelectedValue;
		}
	}
}