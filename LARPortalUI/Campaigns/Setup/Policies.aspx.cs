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
    public partial class Policies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
//            btnClose.Attributes.Add("data-dismiss", "modal");
            lblmodalMessage.Text = "";
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
            chkAllowCharacterRebuilds.Checked = Campaigns.AllowCharacterRebuild;
            chkAllowCPDonation.Checked = Campaigns.AllowCPDonation;
            tbCrossCampaignPosting.Text = Campaigns.CrossCampaignPosting;
            chkNPCApprovalRequired.Checked = Campaigns.NPCApprovalRequired;
            chkPCApprovalRequired.Checked = Campaigns.PlayerApprovalRequired;
            chkShareLocationUseNotes.Checked = Campaigns.ShareLocationUseNotes;
            chkUseCampaignCharacters.Checked = Campaigns.UseCampaignCharacters;
			chkAllowAddInfo.Checked = Campaigns.AllowAdditionalInfo;
            tbEarliestCPApplicationYear.Text = Campaigns.EarliestCPApplicationYear.ToString();
            if (tbEarliestCPApplicationYear.Text == null)
            {
                tbEarliestCPApplicationYear.Text = string.Format("{0:yyyy}", DateTime.Today);
            } 
            tbEventCharacterCap.Text = Campaigns.EventCharacterCPCap.ToString();
            tbCrossCampaignPosting.Text = Campaigns.CrossCampaignPosting;
            tbMaximumCPPerYear.Text = Campaigns.MaximumCPPerYear.ToString();
            tbTotalCharacterCap.Text = Campaigns.TotalCharacterCPCap.ToString();
            LoadApprovalLevel("Character", Campaigns.CharacterApprovalLevel);
            LoadApprovalLevel("PEL", Campaigns.PELApprovalLevel);
        }

        protected void LoadApprovalLevel(string ApprovalTypeDescription, int CurrentLevel)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@LevelTypeDescription", ApprovalTypeDescription);
            DataTable dtApproval = Classes.cUtilities.LoadDataTable("uspGetApprovalLevels", sParams, "LARPortal", Master.UserName, lsRoutineName + "-" + ApprovalTypeDescription);
            switch (ApprovalTypeDescription)
            {
                case "PEL":
                    ddlPELApprovalLevel.DataTextField = "ApprovalLevelDescription";
                    ddlPELApprovalLevel.DataValueField = "CampaignApprovalLevel";
                    ddlPELApprovalLevel.DataSource = dtApproval;
                    ddlPELApprovalLevel.DataBind();
                    ddlPELApprovalLevel.SelectedValue = CurrentLevel.ToString();
                    break;

                case "Character":
                    ddlCharacterApprovalLevel.DataTextField = "ApprovalLevelDescription";
                    ddlCharacterApprovalLevel.DataValueField = "CampaignApprovalLevel";
                    ddlCharacterApprovalLevel.DataSource = dtApproval;
                    ddlCharacterApprovalLevel.DataBind();
                    ddlCharacterApprovalLevel.SelectedValue = CurrentLevel.ToString();
                    break;

                default:
                    break;
            }
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            int iTemp = 0;
            Classes.cCampaignBase Campaigns = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);
            Campaigns.AllowCharacterRebuild = chkAllowCharacterRebuilds.Checked;
            Campaigns.AllowCPDonation = chkAllowCPDonation.Checked;
            Campaigns.ShareLocationUseNotes = chkShareLocationUseNotes.Checked;
            Campaigns.NPCApprovalRequired = chkNPCApprovalRequired.Checked;
            Campaigns.UseCampaignCharacters = chkUseCampaignCharacters.Checked;
            Campaigns.PlayerApprovalRequired = chkPCApprovalRequired.Checked;
			Campaigns.AllowAdditionalInfo = chkAllowAddInfo.Checked;

			if (chkAllowAddInfo.Checked)
				Session["AllowAdditionalInfo"] = "T";
			else
				Session["AllowAdditionalInfo"] = "F";

            if (int.TryParse(ddlPELApprovalLevel.SelectedValue, out iTemp))
            Campaigns.PELApprovalLevel = iTemp;
            if (int.TryParse(ddlCharacterApprovalLevel.SelectedValue, out iTemp))
                Campaigns.CharacterApprovalLevel = iTemp;
            if (int.TryParse(tbEarliestCPApplicationYear.Text, out iTemp))
                Campaigns.EarliestCPApplicationYear = iTemp;
            if (int.TryParse(tbEventCharacterCap.Text, out iTemp))
                Campaigns.EventCharacterCPCap = iTemp;
            if (int.TryParse(tbMaximumCPPerYear.Text, out iTemp))
                Campaigns.MaximumCPPerYear = iTemp;
            if (int.TryParse(tbTotalCharacterCap.Text, out iTemp))
                Campaigns.TotalCharacterCPCap = iTemp;
            Campaigns.CrossCampaignPosting = tbCrossCampaignPosting.Text;
            Campaigns.Save();

            lblmodalMessage.Text = "Campaign policy changes have been saved.";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
        }

		protected void btnClose_Click(object sender, EventArgs e)
		{
			Response.Redirect(Request.RawUrl);
		}
    }
    }
