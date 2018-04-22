using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character
{
    public partial class CharRelationships : System.Web.UI.Page
    {
        private bool _Reload = false;

		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
        {
			oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
			if (!IsPostBack)
            {
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            if ((!IsPostBack) || (_Reload))
            {
                MethodBase lmth = MethodBase.GetCurrentMethod();
                string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

                if (oCharSelect.CharacterID.HasValue)
                {
                    DataTable dtCharactersForCampaign = new DataTable();
                    SortedList sParam = new SortedList();
                    sParam.Add("@CampaignID", oCharSelect.CharacterInfo.CampaignID);
                    dtCharactersForCampaign = Classes.cUtilities.LoadDataTable("prGetCharactersForCampaign", sParam, "LARPortal", Master.UserName, "");

                    DataView dvCharactersForCampaign = new DataView(dtCharactersForCampaign, "", "CharacterAKA", DataViewRowState.CurrentRows);
                    gvList.DataSource = dvCharactersForCampaign;
                    gvList.DataBind();

                    sParam = new SortedList();
                    DataTable dtRelations = new DataTable();
                    dtRelations = Classes.cUtilities.LoadDataTable("select * from CHRelationTypes", sParam, "LARPortal", Master.UserName,
                        lsRoutineName, Classes.cUtilities.LoadDataTableCommandType.Text);
                    DataView dvRelations = new DataView(dtRelations, "", "RelationDescription", DataViewRowState.CurrentRows);
                    ddlRelationship.DataTextField = "RelationDescription";
                    ddlRelationship.DataValueField = "RelationTypeID";
                    ddlRelationship.DataSource = dvRelations;
                    ddlRelationship.DataBind();
                    ddlRelationship.Visible = true;

                    ddlRelationship.Attributes.Add("OnChange", "CheckForOther();");
                    tbOther.Style["display"] = "none";

                    ddlRelationshipNonChar.DataTextField = "RelationDescription";
                    ddlRelationshipNonChar.DataValueField = "RelationTypeID";
                    ddlRelationshipNonChar.DataSource = dvRelations;
                    ddlRelationshipNonChar.DataBind();
                    ddlRelationshipNonChar.Visible = true;

                    ddlRelationshipNonChar.Attributes.Add("OnChange", "CheckForOtherNonChar();");
                    tbOtherNonChar.Style["display"] = "none";

                    BindRelat();

                    if ((oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters) &&
                        (oCharSelect.CharacterInfo.CharacterType != 1))
                    {
                        btnAddNewRelate.Visible = false;
                        divList.Visible = false;

                        if (gvRelationships.Columns.Count > 2)
                        {
                            gvRelationships.Columns[gvRelationships.Columns.Count - 1].Visible = false;
                            gvRelationships.Columns[gvRelationships.Columns.Count - 2].Visible = false;
                        }
                    }
                    else
                    {
                        btnAddNewRelate.Visible = true;
                        divList.Visible = true;

                        if (gvRelationships.Columns.Count > 2)
                        {
                            gvRelationships.Columns[gvRelationships.Columns.Count - 1].Visible = true;
                            gvRelationships.Columns[gvRelationships.Columns.Count - 2].Visible = true;
                        }
                        gvList.SelectedIndexChanged += gvList_SelectedIndexChanged;
                    }
                }
            }
        }

        protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int t = gvList.SelectedIndex;
            string sChar = gvList.SelectedDataKey.Value.ToString();
            sChar = (gvList.SelectedRow.FindControl("lblCharacterAKA") as Label).Text;
            lblCharacter.Text = "Relationship to " + sChar;
            mvAddingRelationship.SetActiveView(vwExistingCharacter);
            ddlRelationship.SelectedIndex = 0;
            tbOther.Text = "";
            tbOther.Style["Display"] = "none";
            tbPlayerComments.Text = "";
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Hiding the Select Button Cell in Header Row.
                e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Hiding the Select Button Cells showing for each Data Row. 
                e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.Display, "none");

                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvList, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void btnAddNewRelate_Click(object sender, EventArgs e)
        {
            mvAddingRelationship.SetActiveView(vwNewRelate);
            tbOtherNonChar.Text = "";
            tbOtherNonChar.Style["Display"] = "none";
            tbOther.Text = "";
            tbOther.Style["Display"] = "none";
            tbCharacterName.Text = "";
            tbPlayerComments.Text = "";
            tbPlayerCommentsNonChar.Text = "";
            tbOtherNonChar.Focus();
        }

        protected void btnSaveNonChar_Click(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            Classes.cRelationship NewRel = new Classes.cRelationship();

            int iRelID;
            if (int.TryParse(hidRelateID.Value, out iRelID))
            {
                NewRel = oCharSelect.CharacterInfo.Relationships.Find(x => x.CharacterRelationshipID == iRelID);
            }
            else
            {
                NewRel.CharacterRelationshipID = -1;
                NewRel.RelationCharacterID = -1;
                NewRel.Name = tbCharacterName.Text;
                NewRel.CharacterID = oCharSelect.CharacterID.Value;
            }

            NewRel.RelationTypeID = Convert.ToInt32(ddlRelationshipNonChar.SelectedValue);
            NewRel.RelationCharacterID = -1;
            NewRel.CharacterID = oCharSelect.CharacterID.Value;

            if (ddlRelationshipNonChar.SelectedItem.Text.ToUpper() == "OTHER")
            {
                NewRel.RelationDescription = tbOtherNonChar.Text;
                NewRel.OtherDescription = tbOtherNonChar.Text;
                NewRel.RelationTypeID = -1;
            }
            else
                NewRel.RelationDescription = ddlRelationshipNonChar.SelectedItem.Text;

            NewRel.Name = tbCharacterName.Text;
            NewRel.PlayerComments = tbPlayerCommentsNonChar.Text;

            NewRel.Save(Master.UserName, Master.UserID);

            BindRelat();
            hidRelateID.Value = "";
            mvAddingRelationship.SetActiveView(vwNewRelateButton);
        }

        protected void btnCancelAdding_Click(object sender, EventArgs e)
        {
            mvAddingRelationship.SetActiveView(vwNewRelateButton);
        }

        protected void btnSaveExistingRelate_Click(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            Classes.cRelationship NewRel = new Classes.cRelationship();
            int iRelID;
            if (int.TryParse(hidRelateID.Value, out iRelID))
            {
                NewRel = oCharSelect.CharacterInfo.Relationships.Find(x => x.CharacterRelationshipID == iRelID);
            }
            else
            {
                NewRel.CharacterRelationshipID = -1;

                if (gvList.SelectedRow != null)
                {
                    NewRel.Name = (gvList.SelectedRow.FindControl("lblCharacterAKA") as Label).Text;
                }
                int iRelationCharID;
                if (int.TryParse(gvList.SelectedDataKey.Value.ToString(), out iRelationCharID))
                    NewRel.RelationCharacterID = iRelationCharID;
                NewRel.CharacterID = oCharSelect.CharacterID.Value;
            }

            NewRel.RelationTypeID = Convert.ToInt32(ddlRelationship.SelectedValue);

            if (ddlRelationship.SelectedItem.Text.ToUpper() == "OTHER")
            {
                NewRel.RelationDescription = tbOther.Text;
                NewRel.OtherDescription = tbOther.Text;
                NewRel.RelationTypeID = -1;
            }
            else
                NewRel.RelationDescription = ddlRelationship.SelectedItem.Text;

            NewRel.PlayerComments = tbPlayerComments.Text;

            NewRel.Save(Master.UserName, Master.UserID);
            BindRelat();
            hidRelateID.Value = "";
            mvAddingRelationship.SetActiveView(vwNewRelateButton);
        }

        private void BindRelat()
        {
            oCharSelect.LoadInfo();

            gvRelationships.DataSource = oCharSelect.CharacterInfo.Relationships;
            gvRelationships.DataBind();
        }

        protected void gvRelationships_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oCharSelect.LoadInfo();

            switch (e.CommandName.ToUpper())
            {
                case "EDITITEM":
                    {
                        int iRelID;
                        if (int.TryParse(e.CommandArgument.ToString(), out iRelID))
                        {
                            var CharRel = oCharSelect.CharacterInfo.Relationships.Find(x => x.CharacterRelationshipID == iRelID);
                            if (CharRel != null)
                            {
                                hidRelateID.Value = iRelID.ToString();
                                if (CharRel.RelationCharacterID < 0)
                                {
                                    mvAddingRelationship.SetActiveView(vwNewRelate);
                                    tbCharacterName.Text = CharRel.Name;
                                    if (CharRel.RelationTypeID == -1)
                                    {
                                        tbOtherNonChar.Text = CharRel.OtherDescription;
                                        tbOtherNonChar.Style["Display"] = "block";
                                    }
                                    else
                                        tbOtherNonChar.Style["Display"] = "none";

                                    ddlRelationshipNonChar.SelectedIndex = -1;
                                    foreach (ListItem li in ddlRelationshipNonChar.Items)
                                    {
                                        if ((CharRel.RelationTypeID == -1) && (li.Text.ToUpper() == "OTHER"))
                                            li.Selected = true;
                                        else
                                            if (CharRel.RelationTypeID.ToString() == li.Value)
                                                li.Selected = true;
                                    }
                                    lblCharacter.Text = "Relationship to " + CharRel.Name;
                                    tbPlayerCommentsNonChar.Text = CharRel.PlayerComments;
                                }
                                else
                                {
                                    mvAddingRelationship.SetActiveView(vwExistingCharacter);
                                    if (CharRel.RelationTypeID == -1)
                                    {
                                        tbOther.Text = CharRel.OtherDescription;
                                        tbOther.Style["Display"] = "block";
                                    }
                                    else
                                        tbOther.Style["Display"] = "none";

                                    ddlRelationship.SelectedIndex = -1;
                                    foreach (ListItem li in ddlRelationship.Items)
                                    {
                                        if ((CharRel.RelationTypeID == -1) && (li.Text.ToUpper() == "OTHER"))
                                            li.Selected = true;
                                        else
                                            if (CharRel.RelationTypeID.ToString() == li.Value)
                                                li.Selected = true;
                                    }
                                    lblCharacter.Text = "Relationship to " + CharRel.Name;
                                    tbPlayerComments.Text = CharRel.PlayerComments;
                                }
                            }
                        }
                    }
                    break;

                case "DELETEITEM":
                    {
                        int iRelID;
                        if (int.TryParse(e.CommandArgument.ToString(), out iRelID))
                        {
                            var CharRel = oCharSelect.CharacterInfo.Relationships.Find(x => x.CharacterRelationshipID == iRelID);
                            if (CharRel != null)
                            {
                                CharRel.RecordStatus = Classes.RecordStatuses.Delete;
                                CharRel.Save(Master.UserName, Master.UserID);
                                hidRelateID.Value = "";
                                mvAddingRelationship.SetActiveView(vwNewRelateButton);
                                _Reload = true;
                            }
                        }
                    }
                    break;
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
                mvAddingRelationship.SetActiveView(vwNewRelateButton);
                _Reload = true;
            }
        }

		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			string t = sender.GetType().ToString();
			oCharSelect.Reset();
			_Reload = true;
			Classes.cUser user = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
			if (user.LastLoggedInCharacter == -1)
				Response.Redirect("/default.aspx");
		}
	}
}