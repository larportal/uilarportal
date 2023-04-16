using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character.ISkills
{
    public partial class StaffList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
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

            string sSortField = "";
            string sSortDir = "";
            string sEventNameFilter = "";
            string sEventDateFilter = "";
            string sStaffStatus = "";
            string sAssignedToFilter = "";

            Classes.cUserOptions OptionsLoader = new Classes.cUserOptions();
            OptionsLoader.LoadUserOptions(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
            foreach (Classes.cUserOption Option in OptionsLoader.UserOptionList)
            {
                if ((Option.ObjectName.ToUpper() == "DDLEVENTDATE") &&
                    (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sEventDateFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "DDLEVENTNAME") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sEventNameFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "DDLSTAFFSTATUS") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sStaffStatus = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "GVIBSKILLLIST") &&
                        (Option.ObjectOption.ToUpper() == "SORTFIELD"))
                    sSortField = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "GVIBSKILLLIST") &&
                        (Option.ObjectOption.ToUpper() == "SORTDIR"))
                    sSortDir = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "DDLASSIGNEDTO") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sAssignedToFilter = Option.OptionValue;

            }

            if (sSortField.Length == 0)
            {
                sSortField = "CharName";
                Classes.cUserOption Option = new Classes.cUserOption();
                Option.SaveOptionValue(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath, "gvIBSkillList", "SortField", sSortField, "");
            }


            DataTable dtSkillList = new DataTable();
            SortedList sParams = new SortedList();

            sParams.Add("@CampaignID", Master.CampaignID);

            dtSkillList = Classes.cUtilities.LoadDataTable("uspGetSubmittedIBSkills", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetSubmittedIBSkills");

            string sRowFilter = "";
            if (dtSkillList.Rows.Count > 0)
            {
                mvISkillList.SetActiveView(vwISKillList);

                if (dtSkillList.Columns["ButtonText"] == null)
                    dtSkillList.Columns.Add(new DataColumn("ButtonText", typeof(string)));

                if (dtSkillList.Columns["ShortRequest"] == null)
                    dtSkillList.Columns.Add("ShortRequest", typeof(string));

                foreach (DataRow dRow in dtSkillList.Rows)
                {
                    dRow["ButtonText"] = "View";
                    if (dRow["RequestText"].ToString().Length > 25)
                    {
                        dRow["ShortRequest"] = dRow["RequestText"].ToString().Substring(0, 25) + "...";
                    }
                    else
                        dRow["ShortRequest"] = dRow["RequestText"].ToString();
                }

                // Now get the list of event names so the person can filter on it.
                DataView view = new DataView(dtSkillList, "", "EventName", DataViewRowState.CurrentRows);
                if (sEventNameFilter.Length > 0)
                    sRowFilter = "EventName = '" + sEventNameFilter.ToString().Replace("'", "''") + "'";
                if (sEventDateFilter.Length > 0)
                {
                    if (sRowFilter.Length > 0)
                        sRowFilter += " and ";
                    sRowFilter += "EventDate = '" + sRowFilter + "'";
                }
                if (sStaffStatus.Length > 0)
                {
                    if (sRowFilter.Length > 0)
                        sRowFilter += " and ";
                    sRowFilter += "StaffStatus = '" + sStaffStatus.Replace("'", "''") + "'";
                }
                if ((sAssignedToFilter.Length > 0) &&
                    (sAssignedToFilter != "-1"))
                {
                    if (sRowFilter.Length > 0)
                        sRowFilter += " and ";
                    sRowFilter += "AssignedTo = '" + sAssignedToFilter.Replace("'", "''") + "'";
                }

                view.RowFilter = sRowFilter;
                if (view.Count == 0)
                {
                    view.RowFilter = "";
                    sRowFilter = "";
                }

                DataTable dtDistinctEvents = view.ToTable(true, "EventName");

                ddlEventName.DataSource = dtDistinctEvents;
                ddlEventName.DataTextField = "EventName";
                ddlEventName.DataValueField = "EventName";
                ddlEventName.DataBind();
                ddlEventName.Items.Insert(0, new ListItem("No Filter", ""));

                ddlEventName.SelectedIndex = -1;
                if (sEventNameFilter.Length > 0)
                {
                    foreach (ListItem li in ddlEventName.Items)
                    {
                        if (li.Value == sEventNameFilter)
                        {
                            ddlEventName.ClearSelection();
                            li.Selected = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (ddlEventName.SelectedIndex == -1)     // Didn't find what was selected.
                    ddlEventName.SelectedIndex = 0;

                DataTable dtDistinctEventsDates = view.ToTable(true, "EventDate");

                //  Since there's a possibility that the date might not be a reasonable date, I'm going to build a table with the valid dates.
                DataTable dtEventDate = new DataTable();
                dtEventDate.Columns.Add("DisplayDate", typeof(string));
                dtEventDate.Columns.Add("ActualDate", typeof(DateTime));

                foreach (DataRow dRow in dtDistinctEventsDates.Rows)
                {
                    DateTime dtTemp = new DateTime();
                    if (DateTime.TryParse(dRow["EventDate"].ToString(), out dtTemp))
                    {
                        DataRow dNewRow = dtEventDate.NewRow();
                        dNewRow["DisplayDate"] = string.Format("{0:MM/dd/yyyy}", dtTemp);
                        dNewRow["ActualDate"] = dtTemp;
                        dtEventDate.Rows.Add(dNewRow);
                    }
                }

                DataView dvDates = new DataView(dtEventDate, "", "ActualDate", DataViewRowState.CurrentRows);
                ddlEventDate.DataSource = dvDates;
                ddlEventDate.DataTextField = "DisplayDate";
                ddlEventDate.DataValueField = "ActualDate";
                ddlEventDate.DataBind();
                ddlEventDate.Items.Insert(0, new ListItem("No Filter", ""));

                ddlEventDate.SelectedIndex = -1;
                if (sEventDateFilter.Length > 0)
                {
                    foreach (ListItem li in ddlEventDate.Items)
                    {
                        if (li.Value == sEventDateFilter)
                        {
                            ddlEventDate.ClearSelection();
                            li.Selected = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (ddlEventDate.SelectedIndex == -1)
                    ddlEventDate.SelectedIndex = 0;

                DataTable dtStaffStatuses = view.ToTable(true, "StaffStatus");
                DataView dvStatuses = new DataView(dtStaffStatuses, "", "StaffStatus", DataViewRowState.CurrentRows);
                ddlStaffStatus.DataSource = dvStatuses;
                ddlStaffStatus.DataTextField = "StaffStatus";
                ddlStaffStatus.DataValueField = "StaffStatus";
                ddlStaffStatus.DataBind();
                ddlStaffStatus.Items.Insert(0, new ListItem("No Filter", ""));

                ddlStaffStatus.SelectedIndex = -1;
                if (sStaffStatus.Length > 0)
                {
                    foreach (ListItem li in ddlStaffStatus.Items)
                    {
                        //if (li.Text == "")
                        //    li.Text = "<No Status>";

                        if (li.Value == sStaffStatus)
                        {
                            ddlStaffStatus.ClearSelection();
                            li.Selected = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (ddlStaffStatus.SelectedIndex == -1)
                    ddlStaffStatus.SelectedIndex = 0;


                DataTable dtAssignedTo = view.ToTable(true, "AssignedTo", "AssignedToID");

                foreach (DataRow dRow in dtAssignedTo.Rows)
                {
                    if (dRow["AssignedToID"].ToString() == "0")
                        dRow["AssignedTo"] = "Not Assigned";
                }

                ddlAssignedTo.DataSource = dtAssignedTo;
                ddlAssignedTo.DataTextField = "AssignedTo";
                ddlAssignedTo.DataValueField = "AssignedToID";
                ddlAssignedTo.DataBind();
                ddlAssignedTo.Items.Insert(0, new ListItem("No Filter", "-1"));

                ddlAssignedTo.SelectedIndex = -1;
                if (sAssignedToFilter.Length > 0)
                {
                    foreach (ListItem li in ddlAssignedTo.Items)
                    {
                        if (li.Value == sAssignedToFilter)
                        {
                            ddlAssignedTo.ClearSelection();
                            li.Selected = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (ddlAssignedTo.SelectedIndex == -1)     // Didn't find what was selected.
                    ddlAssignedTo.SelectedIndex = 0;

                string sSortExp = sSortField + " " + sSortDir;
                DataView dvSkills = new DataView(dtSkillList, sRowFilter, sSortExp, DataViewRowState.CurrentRows);       // "ActualArrivalDate desc, ActualArrivalTime desc, RegistrationID desc", DataViewRowState.CurrentRows);

                gvIBSkillList.DataSource = dvSkills;
                gvIBSkillList.DataBind();

                DisplaySorting(sSortField, sSortDir);
            }
            else
            {
                mvISkillList.SetActiveView(vwNoISkills);
                //lblCampaignName.Text = Master.CampaignName;
            }

        }

        protected void gvIBSkillList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() != "SORT")
            {
                string sRegistrationID = e.CommandArgument.ToString();
                Response.Redirect("StaffAnswerComment.aspx?RequestSkillID=" + sRegistrationID, true);
            }
        }


        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
        }

        protected void gvIBSkillList_Sorting(object sender, GridViewSortEventArgs e)
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
            if (gvIBSkillList.HeaderRow != null)
            {
                int iSortedColumn = -1;
                foreach (DataControlFieldHeaderCell cHeaderCell in gvIBSkillList.HeaderRow.Cells)
                {
                    if (cHeaderCell.ContainingField.SortExpression.ToUpper() == sSortField.ToUpper())
                    {
                        iSortedColumn = gvIBSkillList.HeaderRow.Cells.GetCellIndex(cHeaderCell);
                    }
                }
                if (iSortedColumn != -1)
                {
                    Label lblArrowLabel = new Label();
                    if (sSortDir.Length == 0)
                        lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-up'> </span></a>";
                    else
                        lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-down'> </span></a>";
                    gvIBSkillList.HeaderRow.Cells[iSortedColumn].Controls.Add(lblArrowLabel);
                }
            }
        }

        protected void dllFilterChanged_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlEventName";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlEventName.SelectedValue;
            Option.SaveOptionValue();

            Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlEventDate";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlEventDate.SelectedValue;
            Option.SaveOptionValue();

            Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlStaffStatus";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlStaffStatus.SelectedValue;
            Option.SaveOptionValue();

            Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlAssignedTo";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlAssignedTo.SelectedValue;
            Option.SaveOptionValue();

        }
    }
}