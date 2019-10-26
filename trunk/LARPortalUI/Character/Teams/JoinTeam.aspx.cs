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
    public partial class JoinTeam : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnCloseMessage.Attributes.Add("data-dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            oCharSelect.LoadInfo();

            if (oCharSelect.CharacterID.HasValue)
            {
                SortedList slParameters = new SortedList();
                slParameters.Add("@CharacterID", oCharSelect.CharacterID.Value);
                DataTable dtTeams = cUtilities.LoadDataTable("uspGetTeamsForCampaignAndCharacterInd", slParameters, "LARPortal", Master.UserName, lsRoutineName + ".CampaignAndCharacterInd");

                BindData();
            }
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

        protected void gvAvailable_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            oCharSelect.LoadInfo();

            SortedList sParams = new SortedList();

            int iTeamID;
            int.TryParse(e.CommandArgument.ToString(), out iTeamID);

            if (e.CommandName.ToUpper() == "LEAVETEAM")
            {
                sParams = new SortedList();
                sParams.Add("@TeamID", iTeamID);
                sParams.Add("@CharacterID", oCharSelect.CharacterID.Value);
                DataTable dtCurrentMembers = cUtilities.LoadDataTable("uspGetTeamMemberCounts", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetTeamMemberCounts");

                DataView dvApprovers = new DataView(dtCurrentMembers, "Approvers = 1 and CharApprover = 1", "", DataViewRowState.CurrentRows);
                if (dvApprovers.Count == 1)
                {
                    // There is only a single approver and this character is them - can't remove them.
                    lblmodalMessage.Text = "This character is the only approver for the team. Since it's the only approver it cannot be removed.<br><br>" +
                        "To remove this character assign another character the approval right and then this can be deleted.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
                    return;
                }

                DataView dvMember = new DataView(dtCurrentMembers, "Members = 1 and CharMember = 1", "", DataViewRowState.CurrentRows);
                if (dvMember.Count == 1)
                {
                    // There is only a single member and this character is them - can't remove them.
                    lblmodalMessage.Text = "This character is the only member of the team. Since it's the only member it cannot be removed.<br><br>" +
                        "To remove this character add another character and then this can be deleted.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
                    return;
                }
            }

            string sStatus = "";

            if (e.CommandName.ToUpper() == "JOINTEAM")
                sStatus = "Team Member Requested";
            else if (e.CommandName.ToUpper() == "ACCEPTINVITE")
                sStatus = "Team Member";
            else if ((e.CommandName.ToUpper() == "DECLINEINVITE") ||
                     (e.CommandName.ToUpper() == "LEAVETEAM") ||
                     (e.CommandName.ToUpper() == "CANCELREQUEST"))
                sStatus = "DEL";

            if (sStatus == "DEL")
            {
                sParams = new SortedList();
                sParams.Add("@TeamID", iTeamID);
                sParams.Add("@CharacterID", oCharSelect.CharacterID.Value);
                sParams.Add("@UserID", Master.UserID);
                cUtilities.PerformNonQuery("uspDelCMTeamMembers", sParams, "LARPortal", Master.UserName);
            }
            else
            {
                string sSQL = "select RoleID from MDBRoles where RoleTier = 'Team' and DateDeleted is null and RoleDescription like '%" + sStatus + "%'";
                DataTable dtRoles = Classes.cUtilities.LoadDataTable(sSQL, sParams, "LARPortal", Master.UserName, lsRoutineName, cUtilities.LoadDataTableCommandType.Text);

                if (dtRoles.Rows.Count > 0)
                {
                    int iRoleID;
                    if (int.TryParse(dtRoles.Rows[0]["RoleID"].ToString(), out iRoleID))
                    {
                        sParams = new SortedList();
                        sParams.Add("@TeamID", iTeamID);
                        sParams.Add("@CharacterID", oCharSelect.CharacterID.Value);
                        sParams.Add("@RoleID", iRoleID);
                        sParams.Add("@UserID", Master.UserID);
                        cUtilities.PerformNonQuery("uspInsUpdCMTeamMembers", sParams, "LARPortal", Master.UserName);
                    }
                }
            }
        }

        public void BindData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList slParameters = new SortedList();
            slParameters.Add("@CharacterID", oCharSelect.CharacterID.Value);
            DataTable dtTeams = cUtilities.LoadDataTable("uspGetTeamsForCampaignAndCharacterInd", slParameters, "LARPortal", Master.UserName, lsRoutineName + ".CampaignAndCharacterInd");

            if (dtTeams.Columns["Accept"] == null)
                dtTeams.Columns.Add("Accept", typeof(string));

            if (dtTeams.Columns["Join"] == null)
                dtTeams.Columns.Add("Join", typeof(string));

            if (dtTeams.Columns["Message"] == null)
                dtTeams.Columns.Add("Message", typeof(string));

            if (dtTeams.Columns["SendEMail"] == null)
                dtTeams.Columns.Add("SendEMail", typeof(string));

            if (dtTeams.Columns["DisplayLeave"] == null)
                dtTeams.Columns.Add("DisplayLeave", typeof(string));

            foreach (DataRow dRow in dtTeams.Rows)
            {
                dRow["Message"] = "";
                dRow["Accept"] = "0";
                if (dRow["Requested"].ToString() == "1")
                {
                    dRow["Message"] = "Request Pending";
                    dRow["Join"] = "0";
                    dRow["Accept"] = "1";
                }
                else if (dRow["Invited"].ToString() == "1")
                {
                    dRow["Message"] = "Invited";
                    dRow["Join"] = "0";
                    dRow["Accept"] = "1";
                }
                else
                {
                    dRow["Join"] = "1";
                    dRow["DisplayLeave"] = "1";
                }
            }

            DataView dvMember = new DataView(dtTeams, "Approval = 1 or Member = 1 or Requested = 1", "TeamName", DataViewRowState.CurrentRows);
            gvMembers.DataSource = dvMember;
            gvMembers.DataBind();

            DataView dvAvailable = new DataView(dtTeams, "Approval = 0 and Member = 0 and Requested = 0", "TeamName", DataViewRowState.CurrentRows);
            gvAvailable.DataSource = dvAvailable;
            gvAvailable.DataBind();
        }
    }
}
