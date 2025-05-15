using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using LarpPortal.Classes;

namespace LarpPortal.Profile
{
    public partial class myCampaigns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnCloseMessage.Attributes.Add("data-dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCampaigns();
            }
        }

        protected void LoadCampaigns()
        {
            Classes.cUserCampaigns CampaignChoices = new Classes.cUserCampaigns();
            CampaignChoices.UserDisplayMyCampaigns = false;       //  JB  5/9/2025  Display only campaigns they want to see.

            CampaignChoices.Load(Master.UserID);
            DataView dvCampaigns = new DataView(Classes.cUtilities.CreateDataTable(CampaignChoices.lsUserCampaigns), "", "CampaignName", DataViewRowState.CurrentRows);
            gvCampaigns.DataSource = dvCampaigns;
            gvCampaigns.DataBind();
        }

        protected void gvCampaigns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.HtmlControls.HtmlInputCheckBox swDisplayCampaign = (System.Web.UI.HtmlControls.HtmlInputCheckBox)e.Row.FindControl("swDisplayCampaign");
                HiddenField hidDisplay = (HiddenField)e.Row.FindControl("hidDisplay");
                if ((swDisplayCampaign != null) &&
                    (hidDisplay != null))
                {
                    bool bDisplay;
                    if (Boolean.TryParse(hidDisplay.Value, out bDisplay))
                        if (bDisplay)
                            swDisplayCampaign.Checked = true;
                        else
                            swDisplayCampaign.Checked = false;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iNumSelected = 0;

            foreach (GridViewRow gvRow in gvCampaigns.Rows)
            {
                HtmlInputCheckBox swDisplayCampaign = gvRow.FindControl("swDisplayCampaign") as HtmlInputCheckBox;
                if (swDisplayCampaign != null)
                    if (swDisplayCampaign.Checked)
                        iNumSelected++;
            }
            if (iNumSelected == 0)
            {
                lblErrorMessage.Text = "You must have at least one campaign selected.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openErrorMessage();", true);
                return;
            }

            foreach (GridViewRow gvRow in gvCampaigns.Rows)
            {
                HtmlInputCheckBox swDisplayCampaign = gvRow.FindControl("swDisplayCampaign") as HtmlInputCheckBox;
				HiddenField hidCampaignPlayerID = gvRow.FindControl("hidCampaignPlayerID") as HiddenField;

                if ((swDisplayCampaign != null) &&
                    (hidCampaignPlayerID != null))
                {
                    int iCampaignPlayerID;

                    if (int.TryParse(hidCampaignPlayerID.Value, out iCampaignPlayerID))
                    {
                        SortedList sParams = new SortedList();

                        sParams.Add("@CampaignPlayerID", iCampaignPlayerID);
                        sParams.Add("@UserDisplayMyCampaigns", swDisplayCampaign.Checked);
                        cUtilities.PerformNonQuery("uspInsUpdCMCampaignPLayers", sParams, "LARPortal", Master.UserName);
                    }
                }
            }
            lblmodalMessage.Text = "Your campaigns have been saved.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
            Session["ReloadCampaigns"] = "Y";
        }
    }
}