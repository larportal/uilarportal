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

            Classes.cUserOptions OptionsLoader = new Classes.cUserOptions();
            OptionsLoader.LoadUserOptions(Session["UserName"].ToString(), HttpContext.Current.Request.Url.AbsolutePath);
            foreach (Classes.cUserOption Option in OptionsLoader.UserOptionList)
            {
                //if ((Option.ObjectName.ToUpper() == "DDLCHARACTERNAME") &&
                //    (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                //    sCharacterNameFilter = Option.OptionValue;
                //else if ((Option.ObjectName.ToUpper() == "DDLEVENTDATE") &&
                //        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                //    sEventDateFilter = Option.OptionValue;
                //else
                if ((Option.ObjectName.ToUpper() == "DDLEVENTNAME") &&
                        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                    sEventNameFilter = Option.OptionValue;
                //else if ((Option.ObjectName.ToUpper() == "DDLSTATUS") &&
                //        (Option.ObjectOption.ToUpper() == "SELECTEDVALUE"))
                //    sPELStatusFilter = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "GVIBSKILLLIST") &&
                        (Option.ObjectOption.ToUpper() == "SORTFIELD"))
                    sSortField = Option.OptionValue;
                else if ((Option.ObjectName.ToUpper() == "GVIBSKILLLIST") &&
                        (Option.ObjectOption.ToUpper() == "SORTDIR"))
                    sSortDir = Option.OptionValue;
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

            dtSkillList = Classes.cUtilities.LoadDataTable("uspGetSubmittedIBSkills", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELsForUser");
            if (dtSkillList.Columns["EventDisplayName"] == null)
            {
                dtSkillList.Columns.Add("EventDisplayName", typeof(string));
            }

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
                    dRow["EventDisplayName"] = dRow["EventName"].ToString();
                    DateTime dEventDate = new DateTime();
                    if (DateTime.TryParse(dRow["EventDate"].ToString(), out dEventDate))
                        dRow["EventDisplayName"] = dRow["EventName"].ToString() + " - " + dEventDate.ToShortDateString();
                }

                // Now get the list of event names so the person can filter on it.
                DataView view = new DataView(dtSkillList, "", "EventName", DataViewRowState.CurrentRows);
                if (sEventNameFilter.ToString().Length > 0)
                    sRowFilter = "EventName = '" + sEventNameFilter.ToString().Replace("'", "''") + "'";
                view.RowFilter = sRowFilter;

                DataTable dtDistinctEvents = view.ToTable(true, "EventDisplayName", "EventName");

                ddlEventName.DataSource = dtDistinctEvents;
                ddlEventName.DataTextField = "EventDisplayName";
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

                string sSortExp = sSortField + " " + sSortDir;
                DataView dvPELs = new DataView(dtSkillList, sRowFilter, sSortExp, DataViewRowState.CurrentRows);       // "ActualArrivalDate desc, ActualArrivalTime desc, RegistrationID desc", DataViewRowState.CurrentRows);
                gvIBSkillList.DataSource = dvPELs;
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


    }
}