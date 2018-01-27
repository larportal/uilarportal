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

			string sSelectedChar = "";

			string sRowFilter = "";

			if (ddlCharacterName.SelectedIndex > 0)
			{
				if (ddlCharacterName.SelectedValue.Length > 0)
				{
					sRowFilter += "(CharacterAKA = '" + ddlCharacterName.SelectedValue.Replace("'", "''") + "')";
					sSelectedChar = ddlCharacterName.SelectedValue;
				}
				ddlStatus.SelectedIndex = 0;
			}
			else
			{
				switch (ddlStatus.SelectedValue)
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

			DataView dvPELs = new DataView(dtCharHistory, sRowFilter, "CharacterAKA", DataViewRowState.CurrentRows);
			gvHistoryList.DataSource = dvPELs;
			gvHistoryList.DataBind();

			if (!IsPostBack)
			{
				ddlCharacterName.DataSource = dvPELs;
				ddlCharacterName.DataTextField = "CharacterAKA";
				ddlCharacterName.DataValueField = "CharacterAKA";
				ddlCharacterName.DataBind();

				if (dvPELs.Count > 1)
				{
					ListItem liNoFilter = new ListItem();
					liNoFilter.Text = "No Filter";
					liNoFilter.Value = "";
					ddlCharacterName.Items.Insert(0, liNoFilter);
				}
			}
		}

		protected void gvHistoryList_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			string sCharacterID = e.CommandArgument.ToString();
			Response.Redirect("Approve.aspx?CharacterID=" + sCharacterID, false);
		}

		public string ScrubHtml(string value)
		{
			var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
			var step2 = Regex.Replace(step1, @"\s{2,}", " ");
			return step2;
		}
	}
}