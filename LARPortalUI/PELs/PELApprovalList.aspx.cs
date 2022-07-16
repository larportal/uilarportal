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
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            lblCampaignName.Text = Master.CampaignName;

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

            if (dtPELs.Columns["AddendumImage"] == null)
                dtPELs.Columns.Add("AddendumImage", typeof(string));
            if (dtPELs.Columns["StaffCommentsImages"] == null)
                dtPELs.Columns.Add("StaffCommentsImage", typeof(string));

            foreach (DataRow dRow in dtPELs.Rows)
            {
                if (dRow["Addendum"].ToString() == "Y")
                    dRow["AddendumImage"] = "../img/checkbox.png";
                else
                    dRow["AddendumImage"] = "../img/uncheckbox.png";

                if (dRow["StaffComments"].ToString() == "Y")
                    dRow["StaffCommentsImage"] = "../img/checkbox.png";
                else
                    dRow["StaffCommentsImage"] = "../img/uncheckbox.png";
            }

            string sCharacterNameFilter = "";
            string sEventDateFilter = "";
            string sEventNameFilter = "";
            string sPELStatusFilter = "";
            string sSortField = "";
            string sSortDir = "ASC";

            Classes.cUserOptions OptionsLoader = new Classes.cUserOptions();
            OptionsLoader.LoadUserOptions(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
            foreach (Classes.cUserOption Option in OptionsLoader.UserOptionList)
            {
                if ((Option.ObjectName.ToUpper() == "DDLCHARACTERNAME") &&
                    (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sCharacterNameFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "DDLEVENTDATE") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sEventDateFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "DDLEVENTNAME") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sEventNameFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "DDLSTATUS") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sPELStatusFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "GVPELLIST") &&
                        (Option.ObjectOption.ToUpper() == "SORTFIELD"))
                    sSortField = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "GVPELLIST") &&
                        (Option.ObjectOption.ToUpper() == "SORTDIR"))
                    sSortDir = Option.OptionValue;
            }

            if (sSortField.Length == 0)
            {
                sSortField = "PELStatus";
                Classes.cUserOption Option = new Classes.cUserOption();
                Option.SaveOptionValue(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath, "gvPELList", "SortField", sSortField, "");
            }

            foreach (DataRow dRow in dtPELs.Rows)
            {
                if (dRow["RoleAlignment"].ToString() != "PC")
                    dRow["CharacterAKA"] = dRow["RoleAlignment"].ToString();
            }

            // While creating the filter am also saving the selected values so we can go back and have the drop down list use them.
            string sRowFilter = "(1 = 1)";      // This is so it's easier to build the filter string. Now can always say 'and ....'

            if (sCharacterNameFilter.Length > 0)
            {
                sRowFilter += " and (CharacterAKA = '" + sCharacterNameFilter.Replace("'", "''") + "')";
            }

            if (sEventDateFilter.Length > 0)
            {
                sRowFilter += " and (EventStartDate = '" + sEventDateFilter + "')";
            }

            if (sEventNameFilter.Length > 0)
            {
                sRowFilter += " and (EventName = '" + sEventNameFilter.Replace("'", "''") + "')";
            }

            if (sPELStatusFilter.Length > 0)
            {
                sRowFilter += " and (PELStatus = '" + sPELStatusFilter.Replace("'", "''") + "')";
            }



            DataView dvPELs = new DataView(dtPELs);
            try
            {
                dvPELs.RowFilter = sRowFilter;
            }
            catch
            {
                // Means there was something wrong - probably somebody mucked with the filter to we are going to clear the row filter.
                sRowFilter = "";
            }

            try
            {
                string sSortString = sSortField + " " + sSortDir;
                if (sSortField.ToUpper().StartsWith("PELSTATUS"))
                    sSortString += ", DateSubmitted";
                dvPELs.Sort = sSortString;
            }
            catch
            {
                // Means there was something wrong - probably somebody mucked with the sort order to we are going to clear the row filter.
                sSortField = "";
                sSortDir = "";
            }

            dvPELs.RowStateFilter = DataViewRowState.CurrentRows;

            gvPELList.DataSource = dvPELs;
            gvPELList.DataBind();

            Session["PELApprovalList_dvPELs"] = dvPELs;

            // Now get the list of event names so the person can filter on it.
            DataView view = new DataView(dtPELs, sRowFilter, "EventName", DataViewRowState.CurrentRows);
            DataTable dtDistinctEvents = view.ToTable(true, "EventName");

            ddlEventName.DataSource = dtDistinctEvents;
            ddlEventName.DataTextField = "EventName";
            ddlEventName.DataValueField = "EventName";
            ddlEventName.DataBind();
            ddlEventName.Items.Insert(0, new ListItem("No Filter", ""));
            ddlEventName.SelectedIndex = -1;
            if (sEventNameFilter.Length > 0)
                foreach (ListItem li in ddlEventName.Items)
                    if (li.Value == sEventNameFilter)
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
            if (sCharacterNameFilter.Length > 0)
                foreach (ListItem li in ddlCharacterName.Items)
                    if (li.Value == sCharacterNameFilter)
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
            if (sEventDateFilter.Length > 0)
                foreach (ListItem li in ddlEventDate.Items)
                    if (li.Value == sEventDateFilter)
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
            if (sPELStatusFilter.Length > 0)
                foreach (ListItem li in ddlStatus.Items)
                    if (li.Value == sPELStatusFilter)
                    {
                        ddlStatus.ClearSelection();
                        li.Selected = true;
                    }
                    else
                        li.Selected = false;
            if (ddlStatus.SelectedIndex == -1)     // Didn't find what was selected.
                ddlStatus.SelectedIndex = 0;

            DisplaySorting(sSortField, sSortDir);
        }

        protected void gvPELList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() != "SORT")
            {
                string sRegistrationID = e.CommandArgument.ToString();
                Response.Redirect("PELApprove.aspx?RegistrationID=" + sRegistrationID, false);
            }
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
            //			ViewState["EventDate"] = ddlEventDate.SelectedValue;
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

            //			ViewState["CharacterName"] = ddlCharacterName.SelectedValue;
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

            //			ViewState["EventName"] = ddlEventName.SelectedValue;
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

            //			ViewState["PELStatus"] = ddlStatus.SelectedValue;
        }

        protected void gvPELList_Sorting(object sender, GridViewSortEventArgs e)
        {
            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoadOptionValue(Master.UserName, HttpContext.Current.Request.Url.AbsolutePath, "gvPELList", "SortField");
            string sSortField = Option.OptionValue;
            Option.LoadOptionValue(Master.UserName, HttpContext.Current.Request.Url.AbsolutePath, "gvPELList", "SortDir");
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
            Option.ObjectName = "gvPELList";
            Option.ObjectOption = "SortField";
            Option.OptionValue = sSortField;
            Option.SaveOptionValue();

            Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "gvPELList";
            Option.ObjectOption = "SortDir";
            Option.OptionValue = sSortDir;
            Option.SaveOptionValue();
        }

        private void DisplaySorting(string sSortField, string sSortDir)
        {
            if (gvPELList.HeaderRow != null)
            {
                int iSortedColumn = 0;
                foreach (DataControlFieldHeaderCell cHeaderCell in gvPELList.HeaderRow.Cells)
                {
                    if (cHeaderCell.ContainingField.SortExpression == sSortField)
                    {
                        iSortedColumn = gvPELList.HeaderRow.Cells.GetCellIndex(cHeaderCell);
                    }
                }
                if (iSortedColumn != 0)
                {
                    Label lblArrowLabel = new Label();
                    if (sSortDir.Length == 0)
                        lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-up'> </span></a>";
                    else
                        lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-down'> </span></a>";
                    gvPELList.HeaderRow.Cells[iSortedColumn].Controls.Add(lblArrowLabel);
                }
            }
        }

        protected void gvPELList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int NumCells = e.Row.Cells.Count;
                for (int i = 0; i < NumCells - 1; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
    }
}