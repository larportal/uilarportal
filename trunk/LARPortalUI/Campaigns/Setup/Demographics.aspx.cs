using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Campaigns.Setup
{
    public partial class Demographics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadlblCampaignName();
            }

            lblHeaderCampaignName.Text = " - " + Master.CampaignName;
        }

        protected void LoadlblCampaignName()
        {
            LoadSavedData();
            LoadGenres();
            LoadPeriods();
        }

        protected void LoadSavedData()
        {
        }

        protected void LoadGenres()
        {
        }

        protected void LoadPeriods()
        {
        }

        protected void LoadddlCampaignStatus(int CurrentStatusID)
        {
        }

        protected void LoadddlGameSystem(int CurrentGameSystemID)
        {
        }

        protected void LoadddlSite(int CurrentSiteID)
        {
        }

        protected void LoadddlSize(int CurrentSize)
        {
        }

        protected void LoadddlStyle(int CurrentStyle)
        {
        }

        protected void LoadddlTechLevel(int CurrentTech)
        {
        }

        protected void LoadddlFrequency()
        {

        }

        protected void LoadddlWaiver()
        {

        }

        protected void ddlCampaignStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlGameSystem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPrimarySite_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlStyle_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTechLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlWaiver_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnEditGenres_Click(object sender, EventArgs e)
        {

        }

        protected void btnEditPeriods_Click(object sender, EventArgs e)
        {

        }


        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
        }

        protected void gvSites_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvSites_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gvSites_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gvSites_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvSites_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnEditSite_Click(object sender, EventArgs e)
        {
        }

        protected void btnSaveSites_Click(object sender, EventArgs e)
        {

            string jsString = "alert('Site changes have been saved.');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                    "MyApplication",
                    jsString,
                    true);
        }

        protected void btnSaveGenres_Click(object sender, EventArgs e)
        {

        }

        protected void btnSavePeriods_Click(object sender, EventArgs e)
        {

        }
    }
}