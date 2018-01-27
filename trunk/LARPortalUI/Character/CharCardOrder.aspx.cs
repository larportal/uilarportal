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
    public partial class CharCardOrder : System.Web.UI.Page
    {
        private DataTable _dtPlaces = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            if (!IsPostBack)
            {
                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

                ShowGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            oCharSelect.LoadInfo();
            if (oCharSelect.CharacterID.HasValue)
            {
                foreach (GridViewRow dRow in gvSkills.Rows)
                {
                    HiddenField hidSkillID = (HiddenField)dRow.FindControl("hidSkillID");
                    TextBox tbSortOrder = (TextBox)dRow.FindControl("tbSortOrder");
                    if ((hidSkillID != null) &&
                         (tbSortOrder != null))
                    {
                        SortedList sParams = new SortedList();
                        sParams.Add("@CharacterSkillID", hidSkillID.Value);
                        sParams.Add("@SortOrder", tbSortOrder.Text);
                        Classes.cUtilities.PerformNonQuery("uspInsUpdCHCharacterSkills", sParams, "LARPortal", Master.UserName);
                    }
                }
                ShowGrid();
                lblmodalMessage.Text = "Character " + oCharSelect.CharacterInfo.AKA + " has been saved.";
                btnCloseMessage.Attributes.Add("data-dismiss", "modal");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
            }
        }

        private void ShowGrid()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (oCharSelect.CharacterID.HasValue)
            {
                DataTable dtCharactersForCampaign = new DataTable();
                SortedList sParam = new SortedList();
                sParam.Add("@CharacterID", oCharSelect.CharacterID.Value);
                dtCharactersForCampaign = Classes.cUtilities.LoadDataTable("uspGetCharCardSkills", sParam, "LARPortal", Master.UserName, lsRoutineName);
                gvSkills.DataSource = dtCharactersForCampaign;
                gvSkills.DataBind();
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
                    UserInfo.LastLoggedInCampaign = oCharSelect.CharacterInfo.CampaignID;
                    UserInfo.LastLoggedInCharacter = oCharSelect.CharacterID.Value;
                    UserInfo.LastLoggedInMyCharOrCamp = (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters ? "M" : "C");
                    UserInfo.Save();
                }
                ShowGrid();
            }
        }
    }
}
