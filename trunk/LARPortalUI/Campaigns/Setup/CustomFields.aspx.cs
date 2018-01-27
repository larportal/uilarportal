using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Campaigns.Setup
{
    public partial class CustomFields : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnClose.Attributes.Add("data-dismiss", "modal");
            lblmodalMessage.Text = "";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Classes.cCampaignBase Campaigns = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);
            tbCustomField1.Text = Campaigns.UserDefinedField1Value;
            cbxUseField1.Checked = Campaigns.UserDefinedField1Use;

            tbCustomField2.Text = Campaigns.UserDefinedField2Value;
            cbxUseField2.Checked = Campaigns.UserDefinedField2Use;

            tbCustomField3.Text = Campaigns.UserDefinedField3Value;
            cbxUseField3.Checked = Campaigns.UserDefinedField3Use;

            tbCustomField4.Text = Campaigns.UserDefinedField4Value;
            cbxUseField4.Checked = Campaigns.UserDefinedField4Use;

            tbCustomField5.Text = Campaigns.UserDefinedField5Value;
            cbxUseField5.Checked = Campaigns.UserDefinedField5Use;

            lblHeaderCampaignName.Text = " - " + Master.CampaignName;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Classes.cCampaignBase Campaigns = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);

            Campaigns.UserDefinedField1Value = tbCustomField1.Text;
            Campaigns.UserDefinedField1Use = cbxUseField1.Checked;

            Campaigns.UserDefinedField2Value = tbCustomField2.Text;
            Campaigns.UserDefinedField2Use = cbxUseField2.Checked;

            Campaigns.UserDefinedField3Value = tbCustomField3.Text;
            Campaigns.UserDefinedField3Use = cbxUseField3.Checked;

            Campaigns.UserDefinedField4Value = tbCustomField4.Text;
            Campaigns.UserDefinedField4Use = cbxUseField4.Checked;

            Campaigns.UserDefinedField5Value = tbCustomField5.Text;
            Campaigns.UserDefinedField5Use = cbxUseField5.Checked;

            Campaigns.Save();
            lblmodalMessage.Text = "Campaign custom fields changes have been saved.";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
        }
    }
}
