using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class CharacterCardPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlCardsFor.Attributes.Add("OnChange", "CardsForChange(); return false;");
            ddlEvent.Attributes.Add("OnChange", "EventChange(); return false;");
            ddlCharacter.Attributes.Add("OnChange", "CharacterChange(); return false;");

            if (!IsPostBack)
            {
                lblCharacter.Style["Display"] = "none";
                ddlCharacter.Style["Display"] = "none";
                lblEvent.Style["Display"] = "none";
                ddlEvent.Style["Display"] = "none";
                btnRunReport.Style["Display"] = "none";
            }
        }

        void Page_PreRender(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlCardsFor.SelectedValue))
            {
                switch (ddlCardsFor.SelectedValue.ToUpper())
                {
                    case "CAMPAIGN":
                        btnRunReport.Style["Display"] = "inline";
                        break;

                    case "EVENT":
                        lblEvent.Style["Display"] = "inline";
                        ddlEvent.Style["Display"] = "inline";
                        //if (String.IsNullOrEmpty(ddlEvent.SelectedValue))
                            btnRunReport.Style["Display"] = "inline";
                        break;

                    case "CHARACTER":
                        lblCharacter.Style["Display"] = "inline";
                        ddlCharacter.Style["Display"] = "inline";
                        if (String.IsNullOrEmpty(ddlCharacter.SelectedValue))
                            btnRunReport.Style["Display"] = "inline";
                        break;
                }
            }
            else
            {
                lblCharacter.Style["Display"] = "none";
                ddlCharacter.Style["Display"] = "none";
                lblEvent.Style["Display"] = "none";
                ddlEvent.Style["Display"] = "none";
                btnRunReport.Style["Display"] = "none";
            }
            if (!IsPostBack)
                LoadData();
        }

        public void LoadData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            ddlCardsFor.SelectedIndex = 0;

                ddlCardsFor.SelectedIndex = 0;

                SortedList sParams = new SortedList();
                sParams.Add("@CampaignID", Master.CampaignID);
                sParams.Add("@StatusID", 50); // 50 = Scheduled
                sParams.Add("@EventLength", 50); // How many characters of Event Name/date to return with ellipsis
                DataTable dtEvent = Classes.cUtilities.LoadDataTable("uspGetCampaignEvents", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignEvents");
                ddlEvent.ClearSelection();
                ddlEvent.DataTextField = "EventNameDate";
                ddlEvent.DataValueField = "EventID";
                ddlEvent.DataSource = dtEvent;
                ddlEvent.DataBind();
                ddlEvent.Items.Insert(0, new ListItem("Select Event - Date", ""));
                ddlEvent.SelectedIndex = 0;

                sParams = new SortedList();
                sParams.Add("@CampaignID", Master.CampaignID);
                DataTable dtCharacter = Classes.cUtilities.LoadDataTable("uspGetCampaignCharacters", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignCharacters");
                ddlCharacter.ClearSelection();
                ddlCharacter.DataTextField = "CharacterAKA";
                ddlCharacter.DataValueField = "CharacterID";
                ddlCharacter.DataSource = dtCharacter;
                ddlCharacter.DataBind();
                ddlCharacter.Items.Insert(0, new ListItem("Select Character", ""));
                ddlCharacter.SelectedIndex = 0;
        }

        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            string URLPath = "/Character/EventCharCards.aspx?";
            int EventID = 0;
            int CharacterID = 0;
            int.TryParse(ddlEvent.SelectedValue, out EventID);
            int.TryParse(ddlCharacter.SelectedValue, out CharacterID);

            switch (ddlCardsFor.SelectedValue.ToUpper())
            {
                case "CAMPAIGN":
                    URLPath += "CampaignID=" + Master.CampaignID;
                    break;

                case "EVENT":
                    URLPath += "EventID=" + EventID.ToString();
                    break;

                case "CHARACTER":
                    URLPath += "EventID=" + CharacterID.ToString();
                    break;

                default:
                    URLPath = "";
                    break;
            }

            if (!string.IsNullOrEmpty(URLPath))
            {
                //Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl(URLPath)));
                Response.Redirect(URLPath);
            
            }
        }
    }
}