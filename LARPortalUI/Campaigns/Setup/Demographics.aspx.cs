using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
                LoadCampaign();
            }

            lblHeaderCampaignName.Text = " - " + Master.CampaignName;
        }

        protected void LoadCampaign()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            string sGameSystem = "";
            string sOwner = "";
            string sStatusID = "";

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DataTable sCampList = Classes.cUtilities.LoadDataTable("uspGetCampaigns", sParams, "LARPortal", Master.UserName, lsRoutineName);

            foreach (DataRow dRow in sCampList.Rows)
            {
                tbCampaignName.Text = dRow["CampaignName"].ToString();
                DateTime dt;
                if (DateTime.TryParse(dRow["CampaignStartDate"].ToString(), out dt))
                    tbDateStarted.Text = dt.ToShortDateString();
                if (DateTime.TryParse(dRow["ProjectedEndDate"].ToString(), out dt))
                    tbExpectedEndDate.Text = dt.ToShortDateString();
                sGameSystem = dRow["GameSystemID"].ToString();
                tbLARPPortalType.Text = dRow["PortalAccessType"].ToString();
                sOwner = dRow["PrimaryOwnerID"].ToString();
                sStatusID = dRow["StatusID"].ToString();
            }
            sParams = new SortedList();
            DataTable dtGameSystem = Classes.cUtilities.LoadDataTable("uspGetGameSystems", sParams, "LARPortal", Master.UserName, lsRoutineName);

            DataView dvGameSystem = new DataView(dtGameSystem, "", "GameSystemName", DataViewRowState.CurrentRows);
            ddlGameSystem.DataSource = dvGameSystem;
            ddlGameSystem.DataTextField = "GameSystemName";
            ddlGameSystem.DataValueField = "GameSystemID";
            ddlGameSystem.DataBind();

            bool bFoundIt = false;
            foreach (ListItem li in ddlGameSystem.Items)
            {
                if (li.Value == sGameSystem)
                {
                    ddlGameSystem.ClearSelection();
                    li.Selected = true;
                    bFoundIt = true;
                }
            }
            if (!bFoundIt)
                ddlGameSystem.SelectedIndex = 0;

            sParams = new SortedList();
            //            sParams.Add("@UserID", sOwner);
            DataTable dtUser = Classes.cUtilities.LoadDataTable("uspGetUsers", sParams, "LARPortal", Master.UserName, lsRoutineName);
            foreach (DataRow dRow in dtUser.Rows)
            {
                if (dRow["UserID"].ToString() == sOwner)
                {
                    tbOwner.Text = dRow["FullName"].ToString();
                }
            }

            sParams = new SortedList();
            sParams.Add("@StatusType", "Campaign");
            DataTable dtStatus = Classes.cUtilities.LoadDataTable("uspGetStatus", sParams, "LARPortal", Master.UserName, lsRoutineName);
            DataView dvStatus = new DataView(dtStatus, "", "StatusName", DataViewRowState.CurrentRows);
            ddlCampaignStatus.DataSource = dvStatus;
            ddlCampaignStatus.DataTextField = "StatusName";
            ddlCampaignStatus.DataValueField = "StatusID";
            ddlCampaignStatus.DataBind();

            bFoundIt = false;
            foreach (ListItem li in ddlCampaignStatus.Items)
            {
                if (li.Value == sStatusID)
                {
                    ddlCampaignStatus.ClearSelection();
                    li.Selected = true;
                    bFoundIt = true;
                }
            }

            if (!bFoundIt)
                ddlCampaignStatus.SelectedIndex = 0;
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DateTime dt = new DateTime();
            if (DateTime.TryParse(tbDateStarted.Text, out dt))
                sParams.Add("@CampaignStartDate", dt);
            if (DateTime.TryParse(tbExpectedEndDate.Text, out dt))
                sParams.Add("@ProjectedEndDate", dt);
            sParams.Add("@GameSystemID", ddlGameSystem.SelectedValue);
            sParams.Add("@StatusID", ddlCampaignStatus.SelectedValue);
            sParams.Add("UserID", Master.UserID);

            Classes.cUtilities.PerformNonQuery("uspInsUpdCMCampaigns", sParams, "LARPortal", Master.UserName);

            lblmodalMessage.Text = "The campaign info has been saved.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
        }
    }
}