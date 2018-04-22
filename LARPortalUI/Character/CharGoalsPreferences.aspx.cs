using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character
{
    public partial class CharGoalsPreferences : System.Web.UI.Page
    {
		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
        {
			oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
			if (ViewState["CurrentCharacter"] == null)
                ViewState["CurrentCharacter"] = "";

            if (!IsPostBack)
            {
                divUserDef1.Visible = false;
                divUserDef2.Visible = false;
                divUserDef3.Visible = false;
                divUserDef4.Visible = false;
                divUserDef5.Visible = false;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            divUserDef1.Visible = false;
            divUserDef2.Visible = false;
            divUserDef3.Visible = false;
            divUserDef4.Visible = false;
            divUserDef5.Visible = false;

			oCharSelect.LoadInfo();

            if (oCharSelect.CharacterID.HasValue)
            {
                SortedList sParam = new SortedList();
                sParam.Add("@CharacterID", oCharSelect.CharacterID.Value);
                DataTable dtUserDef = Classes.cUtilities.LoadDataTable("uspGetCharacterUserDef", sParam, "LARPortal", Master.UserName, lsRoutineName);
                foreach (DataRow dRow in dtUserDef.Rows)
                {
                    bool UseValue = false;
                    if (Boolean.TryParse(dRow["UseUserDefinedField1"].ToString(), out UseValue))
                        if (UseValue)
                        {
                            divUserDef1.Visible = true;
                            lblUserDef1.Text = dRow["UserDefinedField1"].ToString();
                            tbUserField1.Text = dRow["Field1Value"].ToString();
                            lblUserField1.Text = dRow["Field1Value"].ToString();
                        }

                    if (Boolean.TryParse(dRow["UseUserDefinedField2"].ToString(), out UseValue))
                        if (UseValue)
                        {
                            divUserDef2.Visible = true;
                            lblUserDef2.Text = dRow["UserDefinedField2"].ToString();
                            tbUserField2.Text = dRow["Field2Value"].ToString();
                            lblUserField2.Text = dRow["Field2Value"].ToString();
                        }

                    if (Boolean.TryParse(dRow["UseUserDefinedField3"].ToString(), out UseValue))
                        if (UseValue)
                        {
                            divUserDef3.Visible = true;
                            lblUserDef3.Text = dRow["UserDefinedField3"].ToString();
                            tbUserField3.Text = dRow["Field3Value"].ToString();
                            lblUserField3.Text = dRow["Field3Value"].ToString();
                        }

                    if (Boolean.TryParse(dRow["UseUserDefinedField4"].ToString(), out UseValue))
                        if (UseValue)
                        {
                            divUserDef4.Visible = true;
                            lblUserDef4.Text = dRow["UserDefinedField4"].ToString();
                            tbUserField4.Text = dRow["Field4Value"].ToString();
                            lblUserField4.Text = dRow["Field4Value"].ToString();
                        }

                    if (Boolean.TryParse(dRow["UseUserDefinedField5"].ToString(), out UseValue))
                        if (UseValue)
                        {
                            divUserDef5.Visible = true;
                            lblUserDef5.Text = dRow["UserDefinedField5"].ToString();
                            tbUserField5.Text = dRow["Field5Value"].ToString();
                            lblUserField5.Text = dRow["Field5Value"].ToString();
                        }

                    if ((oCharSelect.CharacterInfo.CharacterType != 1) && (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters))
                    {
                        btnSave.Enabled = false;
                        tbUserField1.Visible = false;
                        lblUserField1.Visible = true;
                        tbUserField2.Visible = false;
                        lblUserField2.Visible = true;
                        tbUserField3.Visible = false;
                        lblUserField3.Visible = true;
                        tbUserField4.Visible = false;
                        lblUserField4.Visible = true;
                        tbUserField5.Visible = false;
                        lblUserField5.Visible = true;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        tbUserField1.Visible = true;
                        lblUserField1.Visible = false;
                        tbUserField2.Visible = true;
                        lblUserField2.Visible = false;
                        tbUserField3.Visible = true;
                        lblUserField3.Visible = false;
                        tbUserField4.Visible = true;
                        lblUserField4.Visible = false;
                        tbUserField5.Visible = true;
                        lblUserField5.Visible = false;
                    }
                }

                DataTable dtUserValues = Classes.cUtilities.LoadDataTable("uspGetCharacterUserDef", sParam, "LARPortal", Master.UserName, lsRoutineName);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();
            if (oCharSelect.CharacterID.HasValue)
            {
                SortedList sParam = new SortedList();
                sParam.Add("@CharacterID", oCharSelect.CharacterID.Value);
                sParam.Add("@UserDefinedField1", tbUserField1.Text.Trim());
                sParam.Add("@UserDefinedField2", tbUserField2.Text.Trim());
                sParam.Add("@UserDefinedField3", tbUserField3.Text.Trim());
                sParam.Add("@UserDefinedField4", tbUserField4.Text.Trim());
                sParam.Add("@UserDefinedField5", tbUserField5.Text.Trim());

                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

                DataTable dtUserDefined = Classes.cUtilities.LoadDataTable("uspInsUpdCHCharacterUserDefined", sParam, "LARPortal", Master.UserName, lsRoutineName);

                lblmodalMessage.Text = "Character " + oCharSelect.CharacterInfo.AKA + " has been saved.";
                btnCloseMessage.Attributes.Add("data-dismiss", "modal");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
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
					Master.ChangeSelectedCampaign();
                }
            }
        }

		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			string t = sender.GetType().ToString();
			oCharSelect.Reset();
			Classes.cUser user = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
			if (user.LastLoggedInCharacter == -1)
				Response.Redirect("/default.aspx");
		}
	}
}
