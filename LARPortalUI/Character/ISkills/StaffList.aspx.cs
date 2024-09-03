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
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
            Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
        }

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
            string sSkillNameFilter = "";
            string sEventDateFilter = "";
            string sStaffStatus = "";
            string sAssignedToFilter = "";
            string sCharacterFilter = "";
            string sSkillTypeFilter = "";

            Classes.cUserOptions OptionsLoader = new Classes.cUserOptions();
            OptionsLoader.LoadUserOptions(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
            foreach (Classes.cUserOption Option in OptionsLoader.UserOptionList)
            {
                if ((Option.ObjectName.ToUpper() == "DDLEVENTDATE") &&
                    (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sEventDateFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "DDLSKILLNAME") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sSkillNameFilter = Option.OptionValue;
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
                else if ((Option.ObjectName.ToUpper() == "DDLCHARACTERLIST") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sCharacterFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "DDLSKILLTYPE") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sSkillTypeFilter = Option.OptionValue;
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
            hidEventID.Value = "";

            lblAssignedTo.Text = "";
            lblCharacter.Text = "";
            lblEventDate.Text = "";
            lblSkillName.Text = "";
            lblSkillType.Text = "";
            lblStaffStatus.Text = "";

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

                btnPassiveSkills.Visible = false;

                // Now get the list of skill names so the person can filter on it.
                DataView view = new DataView(dtSkillList, "", "SkillName", DataViewRowState.CurrentRows);
                if (sSkillNameFilter.Length > 0)
                    sRowFilter = "SkillName = '" + sSkillNameFilter.ToString().Replace("'", "''") + "'";
                if (sEventDateFilter.Length > 0)
                {
                    btnPassiveSkills.Visible = true;
                    if (sRowFilter.Length > 0)
                        sRowFilter += " and ";
                    sRowFilter += "EventDate = '" + sEventDateFilter + "'";
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
                    sRowFilter += "AssignedToID = '" + sAssignedToFilter.Replace("'", "''") + "'";
                }
                if (sCharacterFilter.Length > 0)
                {
                    if (sRowFilter.Length > 0)
                        sRowFilter += " and ";
                    sRowFilter += "CharName = '" + sCharacterFilter.Replace("'", "''") + "'";
                }
                if (sSkillTypeFilter.Length > 0)
                {
                    if (sRowFilter.Length > 0)
                        sRowFilter += " and ";
                    sRowFilter += "SkillTypeDescription = '" + sSkillTypeFilter.Replace("'", "''") + "'";
                }

                view.RowFilter = sRowFilter;
                if (view.Count == 0)
                {
                    view.RowFilter = "";
                    sRowFilter = "";
                }

                bool bItemFound = false;

                DataTable dtDistinctEvents = view.ToTable(true, "SkillName");

                ddlSkillName.DataSource = dtDistinctEvents;
                ddlSkillName.DataTextField = "SkillName";
                ddlSkillName.DataValueField = "SkillName";
                ddlSkillName.DataBind();
                ddlSkillName.Items.Insert(0, new ListItem("No Filter", ""));

                ddlSkillName.SelectedIndex = -1;
                if (sSkillNameFilter.Length > 0)
                {
                    foreach (ListItem li in ddlSkillName.Items)
                    {
                        if (li.Value == sSkillNameFilter)
                        {
                            ddlSkillName.ClearSelection();
                            li.Selected = true;
                            bItemFound = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (!bItemFound)
                    ddlSkillName.SelectedIndex = 0;

                //if (dtDistinctEvents.Rows.Count == 1)
                //{
                //    ddlSkillName.Visible = false;
                //    lblSkillName.Text = dtDistinctEvents.Rows[0]["SkillName"].ToString();
                //}
                //else
                //{
                //    ddlSkillName.Visible = true;
                //    lblSkillName.Text = "";
                //}

                bItemFound = false;
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

                DataView dvDates = new DataView(dtEventDate, "", "ActualDate desc", DataViewRowState.CurrentRows);
                ddlEventDate.DataSource = dvDates;
                ddlEventDate.DataTextField = "DisplayDate";
                ddlEventDate.DataValueField = "DisplayDate";
                ddlEventDate.DataBind();
                ddlEventDate.Items.Insert(0, new ListItem("No Filter", ""));

                ddlEventDate.SelectedIndex = -1;
                if (sEventDateFilter.Length > 0)
                {
                    foreach (ListItem li in ddlEventDate.Items)
                    {
                        //  Because in the drop down list it's 
                        if (li.Value == sEventDateFilter)
                        {
                            ddlEventDate.ClearSelection();
                            li.Selected = true;
                            bItemFound = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (!bItemFound)
                    ddlEventDate.SelectedIndex = 0;

                //if (dvDates.Count == 1)
                //{
                //    ddlEventDate.Visible = false;
                //    lblEventDate.Text = dvDates[0]["DisplayDate"].ToString();
                //}
                //else
                //{
                //    ddlEventDate.Visible = true;
                //    lblEventDate.Text = "";
                //}

                bItemFound = false;
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
                        if (li.Value == sStaffStatus)
                        {
                            ddlStaffStatus.ClearSelection();
                            li.Selected = true;
                            bItemFound = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (!bItemFound)
                    ddlStaffStatus.SelectedIndex = 0;

                //if (dvStatuses.Count == 1)
                //{
                //    ddlStaffStatus.Visible = false;
                //    lblStaffStatus.Text = dvStatuses[0]["StaffStatus"].ToString();
                //}
                //else
                //{
                //    ddlStaffStatus.Visible = true;
                //    lblStaffStatus.Text = "";
                //}

                bItemFound = false;
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
                            bItemFound = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (!bItemFound)
                    ddlAssignedTo.SelectedIndex = 0;

                //if (dtAssignedTo.Rows.Count == 1)
                //{
                //    ddlAssignedTo.Visible = false;
                //    lblAssignedTo.Text = dtAssignedTo.Rows[0]["AssignedTo"].ToString();
                //}
                //else
                //{
                //    ddlAssignedTo.Visible = true;
                //    lblAssignedTo.Text = "";
                //}








                bItemFound = false;
                DataTable dtCharacterList = view.ToTable(true, "CharName");
                DataView dvCharacterList = new DataView(dtCharacterList, "", "CharName", DataViewRowState.CurrentRows);

                ddlCharacterList.DataSource = dvCharacterList;
                ddlCharacterList.DataTextField = "CharName";
                ddlCharacterList.DataValueField = "CharName";
                ddlCharacterList.DataBind();
                ddlCharacterList.Items.Insert(0, new ListItem("No Filter", ""));

                ddlCharacterList.SelectedIndex = -1;
                if (sCharacterFilter.Length > 0)
                {
                    foreach (ListItem li in ddlCharacterList.Items)
                    {
                        if (li.Value == sCharacterFilter)
                        {
                            ddlCharacterList.ClearSelection();
                            li.Selected = true;
                            bItemFound = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (!bItemFound)
                    ddlCharacterList.SelectedIndex = 0;

                //if (dtCharacterList.Rows.Count == 1)
                //{
                //    ddlCharacterList.Visible = false;
                //    lblCharacter.Text = dtCharacterList.Rows[0]["CharName"].ToString();
                //}
                //else
                //{
                //    ddlAssignedTo.Visible = true;
                //    lblAssignedTo.Text = "";
                //}

                bItemFound = false;
                DataTable dtInfoSkillType = view.ToTable(true, "SkillTypeDescription");
                DataView dvInfoSkillType = new DataView(dtInfoSkillType, "", "SkillTypeDescription", DataViewRowState.CurrentRows);

                ddlSkillType.DataSource = dvInfoSkillType;
                ddlSkillType.DataTextField = "SkillTypeDescription";
                ddlSkillType.DataValueField = "SkillTypeDescription";
                ddlSkillType.DataBind();
                ddlSkillType.Items.Insert(0, new ListItem("No Filter", ""));

                bItemFound = false;
                ddlSkillType.SelectedIndex = -1;
                if (sSkillTypeFilter.Length > 0)
                {
                    foreach (ListItem li in ddlSkillType.Items)
                    {
                        if (li.Value == sSkillTypeFilter)
                        {
                            ddlAssignedTo.ClearSelection();
                            li.Selected = true;
                            bItemFound = true;
                        }
                        else
                            li.Selected = false;
                    }
                }
                if (!bItemFound)
                    ddlSkillType.SelectedIndex = 0;

                string sSortExp = sSortField + " " + sSortDir;
                DataView dvSkills = new DataView(dtSkillList, sRowFilter, sSortExp, DataViewRowState.CurrentRows);       // "ActualArrivalDate desc, ActualArrivalTime desc, RegistrationID desc", DataViewRowState.CurrentRows);

                gvIBSkillList.DataSource = dvSkills;
                gvIBSkillList.DataBind();

                //  If the person has filtered by the date, it means it's limited to one event.
                //  The only time the event ID is important is when we do passive skills which is when someone has filtered.
                //  So the only time the event ID is important is when there's a single event so we grab the event ID off the first record.
                try
                {
                    //  We are going to make sure the event ID on the record really is an int because the sp will blow up if it's not.
                    int iEventID = 0;
                    if (int.TryParse(dvSkills[0]["EventID"].ToString(), out iEventID))
                        hidEventID.Value = iEventID.ToString();
                }
                catch
                {
                    //  Something went wrong and there's nothing to do.
                    //  The event ID will be blank.
                }
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

        protected void ddlFilterChanged_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.cUserOption Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlSkillName";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlSkillName.SelectedValue;
            Option.SaveOptionValue();

            Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlEventDate";
            Option.ObjectOption = "SelectedValue";
            DateTime dt;
            if (DateTime.TryParse(ddlEventDate.SelectedValue, out dt))
                Option.OptionValue = String.Format("{0:MM/dd/yyyy}", dt);
            else
                Option.OptionValue = "";
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

            Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlCharacterList";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlCharacterList.SelectedValue;
            Option.SaveOptionValue();

            Option = new Classes.cUserOption();
            Option.LoginUsername = Master.UserName;
            Option.PageName = HttpContext.Current.Request.Url.AbsolutePath;
            Option.ObjectName = "ddlSkillType";
            Option.ObjectOption = "SelectedValue";
            Option.OptionValue = ddlSkillType.SelectedValue;
            Option.SaveOptionValue();
        }

        protected void btnPassiveSkills_Click(object sender, EventArgs e)
        {
            SortedList sParams = new SortedList();
            sParams.Add("@EventID", hidEventID.Value);
            Classes.cUtilities.PerformNonQuery("uspInsertIBPassive", sParams, "LARPortal", Master.UserName);
        }

        protected void MasterPage_CampaignChanged(object sender, EventArgs e)
        {
            Response.Redirect("/default.aspx");
        }
    }
}