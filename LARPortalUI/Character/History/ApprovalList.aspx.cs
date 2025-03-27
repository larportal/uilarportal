using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character.History
{
    public partial class ApprovalList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (Session ["UpdateHistoryMessage"] != null)
			{
				//string jsString = Session ["UpdateHistoryMessage"].ToString();
				//ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", jsString, true);
				Session.Remove("UpdateHistoryMessage");
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			BindData();
		}

		protected void BindData()
		{
			SortedList sParams = new SortedList();
			DataTable dtCharHistory = new DataTable();

			int iSessionID = 0;
			int.TryParse(Session ["CampaignID"].ToString(), out iSessionID);
			sParams.Add("@CampaignID", iSessionID);

			dtCharHistory = Classes.cUtilities.LoadDataTable("uspGetCampaignCharacterHistory", sParams, "LARPortal", Session ["UserName"].ToString(), "PELApprovalList.Page_PreRender");

			if (!IsPostBack)
			{
				ddlCharacterName.DataSource = dtCharHistory;
				ddlCharacterName.DataTextField = "CharacterAKA";
				ddlCharacterName.DataValueField = "CharacterAKA";
				ddlCharacterName.DataBind();

				if (dtCharHistory.Rows.Count > 1)
				{
					ListItem liNoFilter = new ListItem();
					liNoFilter.Text = "No Filter";
					liNoFilter.Value = "";
					ddlCharacterName.Items.Insert(0, liNoFilter);
				}
			}

			string sSelectedChar = "";
			string sStatus = "";
			string sRowFilter = "";
			string sSortField = "";
			string sSortDir = "";

			//Classes.cUserOption Option = new Classes.cUserOption();
			//Option.LoadOptionValue(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath, "ddlCharacterName", "SelectedValue");
			//sSelectedChar = Option.OptionValue;

			//Option.LoadOptionValue(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath, "ddlStatus", "SelectedValue");
			//sStatus = Option.OptionValue;


			Classes.cUserOptions OptionsLoader = new Classes.cUserOptions();
			OptionsLoader.LoadUserOptions(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
			foreach (Classes.cUserOption Option in OptionsLoader.UserOptionList)
			{
				if ((Option.ObjectName.ToUpper() == "DDLCHARACTERNAME") &&
					(Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
					sSelectedChar = Option.OptionValue;
				else if ((Option.ObjectName.ToUpper() == "DDLSTATUS") &&
						(Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
					sStatus = Option.OptionValue;
				else if ((Option.ObjectName.ToUpper() == "GVIBSKILLLIST") &&
						(Option.ObjectOption.ToUpper() == "SORTFIELD"))
					sSortField = Option.OptionValue;
				else if ((Option.ObjectName.ToUpper() == "GVIBSKILLLIST") &&
						(Option.ObjectOption.ToUpper() == "SORTDIR"))
					sSortDir = Option.OptionValue;
			}

			if (string.IsNullOrEmpty(sSortDir))
				sSortDir = "";

			if (String.IsNullOrEmpty(sSortField))
			{
				sSortField = "CampaignName";
				sSortDir = "";
			}

			if (!string.IsNullOrEmpty(sSelectedChar))
			{
				ddlCharacterName.ClearSelection();
				foreach (ListItem lItem in ddlCharacterName.Items)
				{
					if (lItem.Value == sSelectedChar)
					{
						sRowFilter = "(CharacterAKA = '" + lItem.Value.Replace("'", "''") + "')";
						ddlCharacterName.ClearSelection();
						lItem.Selected = true;
					}
				}
				if (ddlCharacterName.SelectedIndex < 0)
					ddlCharacterName.SelectedIndex = 0;
			}
			else
			{
				sRowFilter = "(DateHistorySubmitted is not null)";
				if (sStatus.Length > 0)
				{
					ddlStatus.ClearSelection();
					foreach (ListItem lItem in ddlStatus.Items)
					{
						if (lItem.Value == sStatus)
						{

							switch (sStatus)
							{
								case "A":
									sRowFilter = "(DateHistoryApproved is not null)";
									break;

								case "S":
									sRowFilter = "(DateHistoryApproved is null) and (DateHistorySubmitted is not null)";
									break;

								default:
									sRowFilter = "(DateHistorySubmitted is not null)";
									break;
							}
						}
					}
					if (ddlStatus.SelectedIndex < 0)
						ddlStatus.SelectedIndex = 0;
				}
			}

			if (dtCharHistory.Columns ["HistoryStatus"] == null)
				dtCharHistory.Columns.Add("HistoryStatus", typeof(string));

			if (dtCharHistory.Columns ["ShortHistory"] == null)
				dtCharHistory.Columns.Add("ShortHistory", typeof(string));

			foreach (DataRow dRow in dtCharHistory.Rows)
			{
				string sRawHistory = dRow ["CharacterHistory"].ToString();
				string ScrubbedHistory = ScrubHtml(sRawHistory);

				if (ScrubbedHistory.Length > 100)
					dRow ["ShortHistory"] = ScrubbedHistory.Substring(0, 95) + "...";
				else
					dRow ["ShortHistory"] = ScrubbedHistory;
				if (dRow ["DateHistoryApproved"] == DBNull.Value)
					dRow ["HistoryStatus"] = "Submitted";
				else
					dRow ["HistoryStatus"] = "Approved";
			}

			string sSortExp = sSortField + " " + sSortDir;
			DataView dvPELs = new DataView(dtCharHistory, sRowFilter, sSortExp, DataViewRowState.CurrentRows);
			gvHistoryList.DataSource = dvPELs;
			gvHistoryList.DataBind();
			DisplaySorting(sSortField, sSortDir);
		}

		protected void gvHistoryList_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName != "Sort")
			{
				string sCharacterID = e.CommandArgument.ToString();
				Response.Redirect("Approve.aspx?CharacterID=" + sCharacterID, false);
			}
		}

		public string ScrubHtml(string value)
		{
			var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
			var step2 = Regex.Replace(step1, @"\s{2,}", " ");
			return step2;
		}

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
			Classes.cUserOption Option = new Classes.cUserOption();
			if (ddlStatus.SelectedIndex == 0)
			{
				Option.DeleteOption(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath, "ddlStatus", "SelectedValue");
			}
			else
			{
				Option.LoginUsername = Session["UserName"].ToString();
				Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
				Option.ObjectName = "ddlStatus";
				Option.ObjectOption = "SelectedValue";
				Option.OptionValue = ddlStatus.SelectedValue;
				Option.SaveOptionValue();
			}
		}

		protected void ddlCharacterName_SelectedIndexChanged(object sender, EventArgs e)
        {
			Classes.cUserOption Option = new Classes.cUserOption();
			if (ddlCharacterName.SelectedIndex == 0)
			{
				Option.DeleteOption(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath, "ddlCharacterName", "SelectedValue");
			}
			else
			{
				Option.LoginUsername = Session["UserName"].ToString();
				Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
				Option.ObjectName = "ddlCharacterName";
				Option.ObjectOption = "SelectedValue";
				Option.OptionValue = ddlCharacterName.SelectedValue;
				Option.SaveOptionValue();
			}
		}

        protected void gvHistoryList_Sorting(object sender, GridViewSortEventArgs e)
        {
			Classes.cUserOption Option = new Classes.cUserOption();
			Option.LoadOptionValue(Master.UserName, HttpContext.Current.Request.Url.AbsolutePath, "gvIBSkillList", "SortField");
			string sSortField = Option.OptionValue;
			Option.LoadOptionValue(Master.UserName, HttpContext.Current.Request.Url.AbsolutePath, "gvIBSkillList", "SortDir");
			string sSortDir = Option.OptionValue;

			if (e.SortExpression == sSortField)
			{
				if (String.IsNullOrEmpty(sSortDir))
					sSortDir = "DESC";
				else
					sSortDir = "";
			}
			else
			{
				sSortField = e.SortExpression;
				sSortDir = "";
			}

			Option = new Classes.cUserOption();
			Option.LoginUsername = Master.UserName;
			Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
			Option.ObjectName = "gvIBSkillList";
			Option.ObjectOption = "SortField";
			Option.OptionValue = sSortField;
			Option.SaveOptionValue();

			Option = new Classes.cUserOption();
			Option.LoginUsername = Master.UserName;
			Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
			Option.ObjectName = "gvIBSkillList";
			Option.ObjectOption = "SortDir";
			Option.OptionValue = sSortDir;
			Option.SaveOptionValue();

		}

		private void DisplaySorting(string sSortField, string sSortDir)
		{
			if (gvHistoryList.HeaderRow != null)
			{
				int iSortedColumn = -1;
				foreach (DataControlFieldHeaderCell cHeaderCell in gvHistoryList.HeaderRow.Cells)
				{
					if (cHeaderCell.ContainingField.SortExpression.ToUpper() == sSortField.ToUpper())
					{
						iSortedColumn = gvHistoryList.HeaderRow.Cells.GetCellIndex(cHeaderCell);
					}
				}
				if (iSortedColumn != -1)
				{
					Label lblArrowLabel = new Label();
					if (sSortDir.Length == 0)
						lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-up'> </span></a>";
					else
						lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-down'> </span></a>";
					gvHistoryList.HeaderRow.Cells[iSortedColumn].Controls.Add(lblArrowLabel);
				}
			}
		}


	}
}