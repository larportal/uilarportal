using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character
{
    public partial class CharAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbCharacterName.Attributes.Add("PlaceHolder", "The nickname for the character.");
            }

            btnCloseError.Attributes.Add("data-dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlUserCampaigns.SelectedIndex = 0;
                ddlUserCampaigns.Items.Clear();
                Classes.cUserCampaigns CampaignChoices = new Classes.cUserCampaigns();
                CampaignChoices.Load(Master.UserID);

                if (CampaignChoices.CountOfUserCampaigns == 0)
                {
                    mvCharacterCreate.SetActiveView(vwNoCampaigns);
                }
                else
                {
                    DataView dvCampaigns = new DataView(Classes.cUtilities.CreateDataTable(CampaignChoices.lsUserCampaigns), "", "CampaignName", DataViewRowState.CurrentRows);
                    ddlUserCampaigns.DataTextField = "CampaignName";
                    ddlUserCampaigns.DataValueField = "CampaignID";
                    ddlUserCampaigns.DataSource = dvCampaigns;
                    ddlUserCampaigns.DataBind();
                    ddlUserCampaigns_SelectedIndexChanged(null, null);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (tbCharacterName.Text.Trim().Length > 0)
            {
                // Reset the user control so it will load the characters the next time it's used.
                oCharSelect.Reset();

                SortedList sParam = new SortedList();
                sParam.Add("@CampaignID", ddlUserCampaigns.SelectedValue);
                sParam.Add("@CharacterAKA", tbCharacterName.Text.Trim());
                sParam.Add("@RoleAlignmentID", ddlCharacterType.SelectedValue);
                sParam.Add("@UserID", Master.UserID);
                if (ddlCharacterType.SelectedValue != "1")      // Non PC Character.
                    sParam.Add("@CurrentUserID", ddlPlayer.SelectedValue);

                DataTable dtCharInfo = new DataTable();
                dtCharInfo = Classes.cUtilities.LoadDataTable("uspInsCreateNewCharacter", sParam, "LARPortal", Master.UserName, lsRoutineName);

                if (dtCharInfo.Rows.Count > 0)
                {
                    if (ddlCharacterType.SelectedValue == "1")
                    {
                        string sCharId = dtCharInfo.Rows[0]["CharacterID"].ToString();
						int iCharID;
						int iCampaignID;
						if ((int.TryParse(sCharId, out iCharID)) &&
							(int.TryParse(ddlUserCampaigns.SelectedValue, out iCampaignID)))
						{
							Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "Password", Session.SessionID);
							UserInfo.LastLoggedInCharacter = iCharID;
							UserInfo.LastLoggedInCampaign = iCampaignID;
							UserInfo.Save();
						}
                        Response.Redirect("CharInfo.aspx");
                    }
                    else
                    {
                        tbCharacterName.Text = "";
                        ddlPlayer.SelectedIndex = 0;
                        lblmodalMessage.Text = "The character has been created..";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
                    }
                }
                else
                {
                    lblmodalMessage.Text = "There a problem saving the character. The technical staff has been contacted.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
                }
            }
        }

        protected void ddlUserCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@UserID", Master.UserID);
            sParams.Add("@CampaignID", ddlUserCampaigns.SelectedValue);

            DataTable dtEditCampaigns = Classes.cUtilities.LoadDataTable("uspGetPrivCampaignCharacterEdit", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetPrivs");
            DataView dvEditCampaigns = new DataView(dtEditCampaigns, "CampaignID = " + ddlUserCampaigns.SelectedValue, "", DataViewRowState.CurrentRows);

            sParams = new SortedList();
            sParams.Add("@UserID", Master.UserID);
            sParams.Add("@CampaignID", ddlUserCampaigns.SelectedValue);
            DataTable dtCampaignRoles = Classes.cUtilities.LoadDataTable("uspGetUserCampaignRoleInfo", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetRoleInfo");

            int PCCount = dtCampaignRoles.Select("RoleDescription LIKE '%PC%' and RoleAlignmentDesc = 'PC'").Length;

            ddlCharacterType.ClearSelection();

            if (dvEditCampaigns.Count == 0)
            {
                divPlayer.Visible = false;
                if (PCCount > 0)
                {
                    ddlCharacterType.SelectedValue = "1";
                    divPlayerType.Visible = false;
                }
            }
            else
            {
                // Only go get the players if the user can choose type. Do this here because we only need to do it once.
                divPlayerType.Visible = true;
                divPlayer.Visible = true;

                sParams = new SortedList();
                sParams.Add("@CampaignID", ddlUserCampaigns.SelectedValue);
                DataTable dtNPCPlayers = Classes.cUtilities.LoadDataTable("uspGetCampaignNPCPlayers", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetPlayers");

                if (dtNPCPlayers.Columns["DisplayValue"] == null)
                    dtNPCPlayers.Columns.Add(new DataColumn("DisplayValue", typeof(string)));

                foreach (DataRow dRow in dtNPCPlayers.Rows)
                {
                    if (dRow["NickName"].ToString().Length > 0)
                        dRow["DisplayValue"] = dRow["NickName"].ToString();
                    else
                        dRow["DisplayValue"] = dRow["FirstName"].ToString();
                    dRow["DisplayValue"] += " " + dRow["LastName"].ToString() + " - " + dRow["loginUserName"].ToString();
                }
                DataView dvNPCPlayers = new DataView(dtNPCPlayers, "", "LastName, FirstName, LoginUserName", DataViewRowState.CurrentRows);
                ddlPlayer.DataSource = dvNPCPlayers;
                ddlPlayer.DataTextField = "DisplayValue";
                ddlPlayer.DataValueField = "UserID";
                ddlPlayer.DataBind();
                ddlPlayer.Items.Insert(0, new ListItem("Select Player...", ""));
                ddlPlayer.ClearSelection();
                ddlPlayer.Items[0].Selected = true;
                ddlCharacterType_SelectedIndexChanged(null, null);
            }
        }

        protected void ddlCharacterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCharacterType.SelectedValue == "2")
                divPlayer.Visible = true;
            else
                divPlayer.Visible = false;
        }
    }
}
