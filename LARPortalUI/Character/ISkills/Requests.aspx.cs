using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LarpPortal.Classes;

// JBradshaw 3/30/2019  Ability to not automatically buy parent skill added.
// JBradshaw 8/8/2021   Added require # of points required for pre-reqs.
// JBradshaw 8/15/2021  Added uspInsUpdCHCharactersSkillCompleted call.
namespace LarpPortal.Character.ISkills
{
    public partial class Requests : System.Web.UI.Page
    {
        protected DataTable _dtCampaignSkills = new DataTable();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
            //            Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if ((!IsPostBack) ||
                (ViewState["dtCharacters"] is null))
            {
                ViewState.Remove("CurrentCharID");

                LarpPortal.Controls.CharacterSelect characterSelect = new LarpPortal.Controls.CharacterSelect();
                if (characterSelect.SkillSetID.HasValue)
                    ViewState["SkillSetID"] = characterSelect.SkillSetID.Value;

                SortedList sParams = new SortedList();
                sParams.Add("@intUserID", Master.UserID);
                DataTable dtCharacters = Classes.cUtilities.LoadDataTable("uspGetCharacterIDsByUserID", sParams, "LARPortal", Master.UserName, lsRoutineName);
                ViewState["dtCharacters"] = dtCharacters;
                ddlCharacterList.DataSource = dtCharacters;
                ddlCharacterList.DataTextField = "DisplayName";
                ddlCharacterList.DataValueField = "CharacterSkillSetID";
                ddlCharacterList.DataBind();
                bool bFoundIt = false;
                if (characterSelect.SkillSetID.HasValue)
                {
                    foreach (ListItem lItem in ddlCharacterList.Items)
                    {
                        if (lItem.Value == characterSelect.SkillSetID.Value.ToString())
                        {
                            ddlCharacterList.ClearSelection();
                            lItem.Selected = true;
                            bFoundIt = true;
                        }
                    }
                }
                if (!bFoundIt)
                    ddlCharacterList.SelectedIndex = 0;
                ddlCharacterList_SelectedIndexChanged(null, null);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //    oCharSelect_CharacterChanged(null, null);
        }



        protected void ddlCharacterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@SkillSetID", ddlCharacterList.SelectedValue);
            DataSet dsSkillInfo = Classes.cUtilities.LoadDataSet("uspGetIBSkillForSkillSet", sParams, "LARPortal", Master.UserName, lsRoutineName);

            dsSkillInfo.Tables[0].TableName = "Skills";
            DataTable dtInfoSkills = dsSkillInfo.Tables[0];

            if (dtInfoSkills.Columns["StatusVisible"] == null)
                dtInfoSkills.Columns.Add("StatusVisible", typeof(string));

            if (dtInfoSkills.Columns["ButtonText"] == null)
                dtInfoSkills.Columns.Add("ButtonText", typeof(string));

            if (dtInfoSkills.Columns["KeyValue"] == null)
                dtInfoSkills.Columns.Add("KeyValue", typeof(string));

            if (dtInfoSkills.Columns["DisplayDate"] == null)
                dtInfoSkills.Columns.Add("DisplayDate", typeof(string));

            foreach (DataRow dr in dtInfoSkills.Rows)
            {
                DateTime dtStart;
                if (DateTime.TryParse(dr["StartDate"].ToString(), out dtStart))
                    dr["DisplayDate"] = dtStart.ToString("MM/dd/yyyy");
                else
                    dr["DisplayDate"] = "";

                if (DBNull.Value.Equals(dr["IBSkillRequestID"]))
                {
                    dr["KeyValue"] = "IBSkillRequestID=-1&RegistrationID=" + dr["RegistrationID"].ToString() + "&SkillNodeID=" + dr["CampaignSkillNodeID"].ToString();
                    dr["ButtonText"] = "New Request";
                    dr["StatusVisible"] = "0";
                }
                else
                {
                    dr["KeyValue"] = "IBSkillRequestID=" + dr["IBSkillRequestID"].ToString();
                    if (dr["CharacterStatus"].ToString().ToUpper() == "SUBMITTED")
                    {
                        dr["ButtonText"] = "View Request";
                        dr["StatusVisible"] = "1";
                    }
                    else
                    {
                        dr["ButtonText"] = "Edit Request";
                        dr["StatusVisible"] = "1";
                    }
                }
            }

            ViewState["AllSkills"] = dtInfoSkills;

            DataView dvInfoSkills = new DataView(dtInfoSkills, "", "", DataViewRowState.CurrentRows);

            dvInfoSkills.Sort = "StartDate";
            DataTable dtStartDate = dvInfoSkills.ToTable(true, "DisplayDate");
            ddlEventDate.DataTextField = "DisplayDate";
            ddlEventDate.DataValueField = "DisplayDate";
            ddlEventDate.DataSource = dtStartDate;
            ddlEventDate.DataBind();
            ddlEventDate.Items.Insert(0, new ListItem("No Filter", ""));

            ddlEventDate.SelectedIndex = 0;

            dvInfoSkills.Sort = "Breadcrumbs";
            DataTable dtSkillName = dvInfoSkills.ToTable(true, "Breadcrumbs");
            ddlSkillName.DataTextField = "Breadcrumbs";
            ddlSkillName.DataValueField = "Breadcrumbs";
            ddlSkillName.DataSource = dtSkillName;
            ddlSkillName.DataBind();
            ddlSkillName.Items.Insert(0, new ListItem("No Filter", ""));

            ddlSkillName.SelectedIndex = 0;

            string sEventDateFilter = "";
            string sSkillNameFilter = "";

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
            }

            if (!string.IsNullOrEmpty(sEventDateFilter))
            {
                foreach (ListItem litem in ddlEventDate.Items)
                {
                    if (litem.Value == sEventDateFilter)
                    {
                        ddlEventDate.ClearSelection();
                        litem.Selected = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(sSkillNameFilter))
            {
                foreach (ListItem litem in ddlSkillName.Items)
                {
                    if (litem.Value == sSkillNameFilter)
                    {
                        ddlSkillName.ClearSelection();
                        litem.Selected = true;
                    }
                }
            }

            gvRegisteredEvents.DataSource = dvInfoSkills;
            gvRegisteredEvents.DataBind();

            ddlFilterChanged_SelectedIndexChanged(null, null);
        }

        protected void gvAvailableSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlRequestInfo.Visible = true;
        }

        protected void gvRegisteredEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sAction = e.CommandName.ToString();
            string URLParameters = e.CommandArgument.ToString();
            if (sAction.ToUpper().StartsWith("VIEW"))
                URLParameters += "&ViewOnly=1";

            Response.Redirect("RequestEdit.aspx?" + URLParameters, true);
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
            Option.OptionValue = ddlEventDate.SelectedValue;
            Option.SaveOptionValue();

            DataTable dtAllSkills = (DataTable)ViewState["AllSkills"];

            string sEventDateFilter = ddlEventDate.SelectedValue;
            string sSkillNameFilter = ddlSkillName.SelectedValue;

            DataView dvEvents = new DataView(dtAllSkills, "", "", DataViewRowState.CurrentRows);
            string sRowFilter = "";
            if (!string.IsNullOrEmpty(sEventDateFilter))
                sRowFilter = "StartDate='" + sEventDateFilter + "'";
            if (!string.IsNullOrEmpty(sSkillNameFilter))
                if (ddlSkillName.SelectedValue != "")
                {
                    if (sRowFilter.Length > 0)
                        sRowFilter += " and ";
                    sRowFilter += "BreadCrumbs='" + sSkillNameFilter.Replace("'", "''") + "'";
                }

            dvEvents.RowFilter = sRowFilter;
            if (dvEvents.Count == 0)
                sRowFilter = "";

            bool bFoundItem = false;

            dvEvents.Sort = "StartDate";
            DataTable dtStartDate = dvEvents.ToTable(true, "DisplayDate");
            ddlEventDate.DataTextField = "DisplayDate";
            ddlEventDate.DataValueField = "DisplayDate";
            ddlEventDate.DataSource = dtStartDate;
            ddlEventDate.DataBind();
            ddlEventDate.Items.Insert(0, new ListItem("No Filter", ""));

            if (sEventDateFilter.Length > 0)
            {
                foreach (ListItem lItem in ddlEventDate.Items)
                {
                    if (lItem.Value == sEventDateFilter)
                    {
                        ddlEventDate.ClearSelection();
                        lItem.Selected = true;
                        bFoundItem = true;
                    }
                }
            }
            if (!bFoundItem)
                ddlEventDate.SelectedIndex = 0;

            bFoundItem = false;
            dvEvents.Sort = "Breadcrumbs";
            DataTable dtSkillName = dvEvents.ToTable(true, "Breadcrumbs");
            ddlSkillName.DataTextField = "Breadcrumbs";
            ddlSkillName.DataValueField = "Breadcrumbs";
            ddlSkillName.DataSource = dtSkillName;
            ddlSkillName.DataBind();
            ddlSkillName.Items.Insert(0, new ListItem("No Filter", ""));

            if (sSkillNameFilter.Length > 0)
            {
                foreach (ListItem lItem in ddlSkillName.Items)
                {
                    if (lItem.Value == sSkillNameFilter)
                    {
                        ddlSkillName.ClearSelection();
                        lItem.Selected = true;
                        bFoundItem = true;
                    }
                }
            }
            if (!bFoundItem)
                ddlSkillName.SelectedIndex = 0;

            gvRegisteredEvents.DataSource = dvEvents;
            gvRegisteredEvents.DataBind();

            //    ddlCharacterList_SelectedIndexChanged(null, null);
        }
    }
}
