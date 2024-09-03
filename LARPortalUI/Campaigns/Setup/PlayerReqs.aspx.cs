using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Campaigns.Setup
{
    public partial class PlayerReqs : System.Web.UI.Page
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
                LoadSavedData();
            }

            lblHeaderCampaignName.Text = " - " + Master.CampaignName;
        }

        protected void LoadSavedData()
        {
            Classes.cCampaignBase Campaigns = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);
            tbMinAge.Text = Campaigns.MinimumAge.ToString();
            tbMembershipFee.Text = Campaigns.MembershipFee.ToString();
            tbSuperAge.Text = Campaigns.MinimumAgeWithSupervision.ToString();
            bool bFound = false;
            foreach (ListItem item in ddlFrequency.Items)
            {
                if (item.Value == Campaigns.MembershipFeeFrequency)
                {
                    ddlFrequency.ClearSelection();
                    item.Selected = true;
                    bFound = true;
                }
            }
            if (!bFound)
                ddlFrequency.SelectedIndex = 0;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            {
                int iTemp = 0;
                Classes.cCampaignBase Campaigns = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);
                if (int.TryParse(tbMinAge.Text, out iTemp))
                    Campaigns.MinimumAge = iTemp;
                if (int.TryParse(tbMembershipFee.Text, out iTemp))
                    Campaigns.MembershipFee = iTemp;
                if (int.TryParse(tbSuperAge.Text, out iTemp))
                    Campaigns.MinimumAgeWithSupervision = iTemp;

                Campaigns.Save();

                lblmodalMessage.Text = "Campaign policy changes have been saved.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}