using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LarpPortal.Classes;

namespace LarpPortal.Character.Teams
{
    public partial class ManageTeams : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
            btnCloseMessage.Attributes.Add("data-dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            oCharSelect.LoadInfo();

            if (!IsPostBack)
            {
                SortedList slParameters = new SortedList();
                slParameters.Add("@CharacterID", oCharSelect.CharacterID.Value);
                DataTable dtTeamList = cUtilities.LoadDataTable("uspGetTeamMembers", slParameters, "LARPortal", Master.UserName, lsRoutineName + ".GetTeams");

                DataTable dtTeams = new DataView(dtTeamList, "Approval", "TeamName", DataViewRowState.CurrentRows).ToTable();
                ddlTeams.DataSource = dtTeams;
                ddlTeams.DataTextField = "TeamName";
                ddlTeams.DataValueField = "TeamID";
                ddlTeams.DataBind();

                if (dtTeams.Rows.Count > 0)
                {
                    ddlTeams.ClearSelection();
                    ddlTeams.Items[0].Selected = true;
                    hidTeamID.Value = ddlTeams.SelectedValue;
                    if (dtTeams.Rows.Count == 1)
                    {
                        lblTeamName.Text = ddlTeams.SelectedItem.Text;
                        lblTeamName.Visible = true;
                        ddlTeams.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("/Character/Teams/JoinTeam.aspx", true);
                }
            }

            if ((!IsPostBack) || (Session["TeamMembers"] == null))
            {
                if (ddlTeams.SelectedValue != null)
                {
                    SortedList slParameters = new SortedList();
                    slParameters.Add("@TeamID", ddlTeams.SelectedValue);
                    DataTable dtTeamMembers = cUtilities.LoadDataTable("uspGetTeamMembers", slParameters, "LARPortal", Master.UserName, lsRoutineName + ".GetMembers");
                    Session["TeamMembers"] = dtTeamMembers;

                    BindData();
                }
            }
        }

        //private void SendSubmittedEmail(string sHistory, Classes.cCharacterHistory cHist)
        //{
        //    try
        //    {
        //        if (hidNotificationEMail.Value.Length > 0)
        //        {
        //            Classes.cUser User = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
        //            string sSubject = cHist.CampaignName + " character history from " + cHist.PlayerName + " - " + cHist.CharacterAKA;

        //            string sBody = (string.IsNullOrEmpty(User.NickName) ? User.FirstName : User.NickName) +
        //                " " + User.LastName + " has submitted a character history for " + cHist.CharacterAKA + ".<br><br>" +
        //                 sHistory;
        //            Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
        //            cEMS.SendMail(sSubject, sBody, cHist.NotificationEMail, "", "", "CharacterHistory", Master.UserName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Write the exception to error log and then throw it again...
        //        Classes.ErrorAtServer lobjError = new Classes.ErrorAtServer();
        //        lobjError.ProcessError(ex, "CharacterEdit.aspx.SendSubmittedEmail", "", Session.SessionID);
        //    }
        //}

        protected void gvAvailable_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string t = e.CommandArgument.ToString();
            string j = e.CommandName;

            DataTable dtTeamMembers = Session["TeamMembers"] as DataTable;
            DataView dv = new DataView(dtTeamMembers, "CharacterID = " + e.CommandArgument.ToString(), "CharacterLastName", DataViewRowState.CurrentRows);

            //lblChangesNotSaved.Visible = true;
            //lblChangesNotSaved2.Visible = true;
            switch (e.CommandName.ToUpper())
            {
                case "INVITECHAR":
                    if (dv.Count > 0)
                    {
                        dv[0]["Invited"] = true;
                        dv[0]["Message"] = "Invited";
                        dv[0]["UpdateRecord"] = true;
                        dv[0]["SendEMail"] = "Yes";
                    }
                    break;

                case "APPROVECHAR":
                case "ACCEPTCHAR":
                    if (dv.Count > 0)
                    {
                        dv[0]["Member"] = true;
                        dv[0]["Invited"] = false;
                        dv[0]["Message"] = "Approved";
                        dv[0]["UpdateRecord"] = true;
                    }
                    break;

                case "DENYCHAR":
                case "REVOKECHAR":
                    if (dv.Count > 0)
                    {
                        dv[0]["Member"] = false;
                        dv[0]["Approval"] = false;
                        dv[0]["Requested"] = false;
                        dv[0]["Invited"] = false;
                        dv[0]["DisplayInvite"] = "0";
                        dv[0]["DisplayApprove"] = "0";
                        dv[0]["DisplayRevoke"] = "0";
                        dv[0]["DisplayAccept"] = "0";
                        dv[0]["Message"] = "Removed";
                        dv[0]["UpdateRecord"] = true;
                    }
                    break;
            }
            Session["TeamMembers"] = dtTeamMembers;
            BindData();
        }


        public void BindData()
        {
            if (Session["TeamMembers"] != null)
            {
                DataTable dtTeamMembers = Session["TeamMembers"] as DataTable;

                if (dtTeamMembers.Columns["DisplayInvite"] == null)
                    dtTeamMembers.Columns.Add("DisplayInvite", typeof(string));
                if (dtTeamMembers.Columns["DisplayApprove"] == null)
                    dtTeamMembers.Columns.Add("DisplayApprove", typeof(string));
                if (dtTeamMembers.Columns["DisplayAccept"] == null)
                    dtTeamMembers.Columns.Add("DisplayAccept", typeof(string));
                if (dtTeamMembers.Columns["DisplayRevoke"] == null)
                    dtTeamMembers.Columns.Add("DisplayRevoke", typeof(string));
                if (dtTeamMembers.Columns["DisplayCancelRevoke"] == null)
                    dtTeamMembers.Columns.Add("DisplayCancelRevoke", typeof(string));
                if (dtTeamMembers.Columns["EnableRevoke"] == null)
                    dtTeamMembers.Columns.Add("EnableRevoke", typeof(string));

                if (dtTeamMembers.Columns["Message"] == null)
                    dtTeamMembers.Columns.Add("Message", typeof(string));
                if (dtTeamMembers.Columns["SendEMail"] == null)
                    dtTeamMembers.Columns.Add("SendEMail", typeof(string));
                if (dtTeamMembers.Columns["ShowInMember"] == null)
                    dtTeamMembers.Columns.Add("ShowInMember", typeof(string));

                if (dtTeamMembers.Columns["UpdateRecord"] == null)
                {
                    DataColumn dValueChanged = new DataColumn("UpdateRecord", typeof(Boolean));
                    dValueChanged.DefaultValue = false;
                    dtTeamMembers.Columns.Add(dValueChanged);
                }

                foreach (DataRow dRow in dtTeamMembers.Rows)
                {
                    dRow["ShowInMember"] = "N";
                    dRow["DisplayInvite"] = "0";
                    dRow["DisplayApprove"] = "0";       // 1
                    dRow["DisplayAccept"] = "0";
                    dRow["DisplayRevoke"] = "0";
                    dRow["DisplayCancelRevoke"] = "0";
                    dRow["EnableRevoke"] = "0";

                    if (((bool)dRow["Approval"]) ||
                        ((bool)dRow["Member"]))
                    {
                        dRow["DisplayInvite"] = "0";
                        dRow["DisplayApprove"] = "0";
                        dRow["DisplayRevoke"] = "1";
                        dRow["DisplayAccept"] = "0";
                        dRow["ShowInMember"] = "Y";
                    }
                    else if ((bool)dRow["Requested"])
                    {
                        dRow["DisplayInvite"] = "0";
                        dRow["DisplayApprove"] = "0";       // 1
                        dRow["DisplayAccept"] = "1";
                        dRow["DisplayRevoke"] = "0";
                        dRow["ShowInMember"] = "Y";
                    }
                    else if ((bool)dRow["Invited"])
                    {
                        dRow["DisplayApprove"] = "0";
                        dRow["DisplayInvite"] = "0";
                        dRow["DisplayAccept"] = "0";
                        dRow["DisplayRevoke"] = "1";
                        dRow["Message"] = "Invited";
                    }
                    else
                    {
                        dRow["DisplayApprove"] = "0";
                        dRow["DisplayInvite"] = "1";
                        dRow["DisplayAccept"] = "0";
                        dRow["DisplayRevoke"] = "0";
                    }
                }

                DataView dvApprovers = new DataView(dtTeamMembers, "Approval", "", DataViewRowState.CurrentRows);
                if (dvApprovers.Count == 1)
                {
                    // Means there is only one approver so we are going to disable the button.
                    dvApprovers[0]["DisplayRevoke"] = "0";
                    dvApprovers[0]["DisplayCancelRevoke"] = "1";
                    dvApprovers[0]["EnableRevoke"] = "1";
                }

                DataView dvMember = new DataView(dtTeamMembers, "ShowInMember = 'Y'", "CharacterAKA", DataViewRowState.CurrentRows);
                gvMembers.DataSource = dvMember;
                gvMembers.DataBind();

                DataView dvAvailable = new DataView(dtTeamMembers, "ShowInMember <> 'Y'", "Requested desc, CharacterAKA", DataViewRowState.CurrentRows);
                gvAvailable.DataSource = dvAvailable;
                gvAvailable.DataBind();

                Session["TeamMembers"] = dtTeamMembers;
            }
        }

        protected void chkBoxApprover_CheckedChanged(object sender, EventArgs e)
        {
            //lblChangesNotSaved.Visible = true;
            //lblChangesNotSaved2.Visible = true;

            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
            CheckBox chkBoxApprover = (CheckBox)gvMembers.Rows[index].FindControl("chkBoxApprover");
            HiddenField hidCharacterID = (HiddenField)gvMembers.Rows[index].FindControl("hidCharacterID");

            if ((chkBoxApprover != null) &&
                (hidCharacterID != null) &&
                (Session["TeamMembers"] != null))
            {
                DataTable dtTeamMembers = Session["TeamMembers"] as DataTable;

                if (!chkBoxApprover.Checked)
                {
                    DataView dv = new DataView(dtTeamMembers, "Approval and CharacterID <> " + hidCharacterID.Value, "", DataViewRowState.CurrentRows);
                    if (dv.Count == 0)
                    {
                        lblmodalMessage.Text = "There must be at least one person who has approval privileges.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
                        chkBoxApprover.Checked = true;
                        return;
                    }
                }
                DataRow[] dChar = dtTeamMembers.Select("CharacterID = " + hidCharacterID.Value);

                foreach (DataRow dRow in dChar)
                {
                    dRow["Approval"] = chkBoxApprover.Checked;
                    dRow["UpdateRecord"] = true;
                }
                Session["TeamMembers"] = dtTeamMembers;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (Session["TeamMembers"] != null)
            {
                int Approval = -1;
                int Member = -1;
                int Invited = -1;
                int Requested = -1;

                DataTable dtTeamMembers = Session["TeamMembers"] as DataTable;

                SortedList sParams = new SortedList();
                DataTable dtStatus = cUtilities.LoadDataTable("select RoleID, RoleDescription from MDBRoles where RoleTier='Team'", sParams, "LARPortal",
                    Master.UserName, lsRoutineName, cUtilities.LoadDataTableCommandType.Text);

                foreach (DataRow dStatus in dtStatus.Rows)
                {
                    string sRoles = dStatus["RoleDescription"].ToString().ToUpper();
                    if (sRoles.Contains("APPROVAL"))
                        int.TryParse(dStatus["RoleID"].ToString(), out Approval);
                    else if (sRoles.Contains("REQUESTED"))
                        int.TryParse(dStatus["RoleID"].ToString(), out Requested);
                    else if (sRoles.Contains("INVITED"))
                        int.TryParse(dStatus["RoleID"].ToString(), out Invited);
                    else if (sRoles == "TEAM MEMBER")
                        int.TryParse(dStatus["RoleID"].ToString(), out Member);
                }

                DataView dv = new DataView(dtTeamMembers, "UpdateRecord", "", DataViewRowState.CurrentRows);

                foreach (DataRowView dRow in dv)
                {
                    string sProcedureName = "uspInsUpdCMTeamMembers";

                    int CharID = -1;
                    int TeamID = -1;
                    int.TryParse(dRow["CharacterID"].ToString(), out CharID);
                    int.TryParse(dRow["TeamID"].ToString(), out TeamID);
                    sParams = new SortedList();
                    sParams.Add("@UserID", Master.UserID);
                    sParams.Add("@TeamID", TeamID);
                    sParams.Add("@CharacterID", CharID);

                    if ((bool)dRow["Approval"])
                        sParams.Add("@RoleID", Approval);
                    else if ((bool)dRow["Member"])
                        sParams.Add("@RoleID", Member);
                    else if ((bool)dRow["Requested"])
                        sParams.Add("@RoleID", Requested);
                    else if ((bool)dRow["Invited"])
                        sParams.Add("@RoleID", Invited);
                    else
                        sProcedureName = "uspDelCMTeamMembers";

                    cUtilities.PerformNonQuery(sProcedureName, sParams, "LARPortal", Master.UserName);

                    if (dRow["SendEMail"].ToString().Length > 0)
                    {
                        sParams = new SortedList();
                        sParams.Add("@TeamID", TeamID);
                        // Fixed Current User filter. Using wrong field.
                        DataView dvApprover = new DataView(dtTeamMembers, "CurrentUserID = " + Master.UserID.ToString(), "", DataViewRowState.CurrentRows);
                        if (dvApprover.Count == 0)
                        {
                            dvApprover = new DataView(dtTeamMembers, "Approval = 1", "", DataViewRowState.CurrentRows);
                        }
                        if (dvApprover.Count > 0)
                        {
                            DataRowView dApprover = dvApprover[0];
                            string sBody = dRow["CharacterAKA"].ToString() + "<br><br>" +
                                dApprover["CharacterAKA"].ToString() + " has invited you to join the " + dApprover["TeamName"].ToString() +
                                " team.  To accept visit larportal.com and Go to Character > Join Team to accept or decline." +
                                "<br><br>" +
                                "Thanks!";
                            string sSubject = dApprover["CharFullName"].ToString() + " has invited you to join " + dApprover["TeamName"].ToString() + " team.";

                            Classes.cEmailMessageService cEMS = new Classes.cEmailMessageService();
                            cEMS.SendMail(sSubject, sBody, dRow["EmailAddress"].ToString(), "", "", "Teams", Master.UserName);
                        }
                        dRow["SendEMail"] = "";
                    }
                }
            }
            lblmodalMessage.Text = "Changes have been saved.";
            //lblChangesNotSaved.Visible = false;
            //lblChangesNotSaved2.Visible = false;
            Session.Remove("TeamMembers");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
        }

        protected void oCharSelect_CharacterChanged(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            if (oCharSelect.CharacterInfo != null)
            {
                if (oCharSelect.CharacterID.HasValue)
                {
                    Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
                    UserInfo.LastLoggedInCampaign = oCharSelect.CampaignID.Value;
                    UserInfo.LastLoggedInCharacter = oCharSelect.CharacterID.Value;
					UserInfo.LastLoggedInSkillSetID = oCharSelect.SkillSetID.Value;
                    UserInfo.LastLoggedInMyCharOrCamp = (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters ? "M" : "C");
                    UserInfo.Save();
                }
            }
        }

        protected void ddlTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("TeamMembers");
        }
    }
}
