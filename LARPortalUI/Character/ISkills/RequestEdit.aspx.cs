using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LarpPortal.Classes;

// JBradshaw 3/30/2019  Ability to not automatically buy parent skill added.
// JBradshaw 8/8/2021   Added require # of points required for pre-reqs.
// JBradshaw 8/15/2021  Added uspInsUpdCHCharactersSkillCompleted call.
namespace LarpPortal.Character.ISkills
{
    public partial class RequestEdit : System.Web.UI.Page
    {
        protected DataTable _dtCampaignSkills = new DataTable();
        //        private bool _Reload = false;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
            //            Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            int iTemp;

            if (!IsPostBack)
            {
                hidSkillRequestID.Value = "";
                hidRegistrationID.Value = "";
                hidSkillNodeID.Value = "";

                if (Request.QueryString["ViewOnly"] != null)
                    hidViewOnly.Value = "1";
                else
                    hidViewOnly.Value = "";

                if (Request.QueryString["IBSkillRequestID"] == null)
                    Response.Redirect("Requests.aspx", true);
                else if (int.TryParse(Request.QueryString["IBSkillRequestID"], out iTemp))
                    hidSkillRequestID.Value = iTemp.ToString();
                else
                    Response.Redirect("Requests.aspx", true);

                if (Request.QueryString["RegistrationID"] != null)
                {
                    if (int.TryParse(Request.QueryString["RegistrationID"], out iTemp))
                        hidRegistrationID.Value = iTemp.ToString();
                }

                if (Request.QueryString["SkillNodeID"] != null)
                {
                    if (int.TryParse(Request.QueryString["SkillNodeID"], out iTemp))
                        hidSkillNodeID.Value = iTemp.ToString();
                }
            }
            int? iIBSkillRequestID = null;
            int? iRegistrationID = null;
            int? iSkillNodeID = null;

            if ((hidSkillRequestID.Value == "") &&
                (hidRegistrationID.Value == "") &&
                (hidSkillNodeID.Value == ""))
                Response.Redirect("Requests.aspx", true);

            if (int.TryParse(hidSkillRequestID.Value, out iTemp))
                iIBSkillRequestID = iTemp;
            if (int.TryParse(hidRegistrationID.Value, out iTemp))
                iRegistrationID = iTemp;
            if (int.TryParse(hidSkillNodeID.Value, out iTemp))
                iSkillNodeID = iTemp;

            SortedList sParams = new SortedList();
            if ((iIBSkillRequestID.HasValue) &&
                (iIBSkillRequestID.Value != -1))
                sParams.Add("@IBSkillRequestID", iIBSkillRequestID.Value);
            else
            {
                sParams.Add("@CampaignSkillNodeID", iSkillNodeID.Value);
                sParams.Add("@RegistrationID", iRegistrationID.Value);
            }

            DataSet dsEvent = Classes.cUtilities.LoadDataSet("uspGetIBRequest", sParams, "LARPortal", Master.UserName, lsRoutineName);

            foreach (DataRow dr in dsEvent.Tables[0].Rows)
            {
                lblEventName.Text = dr["EventName"].ToString();
                lblEventDate.Text = dr["StartDate"].ToString();
            }

            foreach (DataRow dr in dsEvent.Tables[1].Rows)
            {
                lblSkillName.Text = dr["SkillName"].ToString();
                lblShortSkillDesc.Text = dr["SkillShortDescription"].ToString();
                lblLongSkillDesc.Text = dr["SkillLongDescription"].ToString();
                lblLongSkillDesc.Attributes.Add("style", "display: none;");
                hidWhichDisplayed.Value = "S";

                if (dr["RequestText"] != null)
                    CKERequestText.Text = dr["RequestText"].ToString();

                if (dr["SkillSetID"] != null)
                    hidSkillSetID.Value = dr["SkillSetID"].ToString();

                if (hidViewOnly.Value.Length > 0)
                {
                    CKERequestText.ReadOnly = true;
                    btnSave.Visible = false;
                    btnSubmit.Visible = false;
                }
                else
                {
                    CKERequestText.ReadOnly = false;
                    btnSave.Visible = true;
                    btnSubmit.Visible = true;
                }
            }
        }

        protected void rbPayerOrChar_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Requests.aspx", true);
        }

        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
        }

        protected void btnSubmitSave_Command(object sender, CommandEventArgs e)
        {
            SortedList sParams = new SortedList();
            sParams.Add("@CampaignSkillNodeID", hidSkillNodeID.Value);
            sParams.Add("@RegistrationID", hidRegistrationID.Value);
            sParams.Add("@IBSkillRequestID", hidSkillRequestID.Value.ToString());
            sParams.Add("@RequestText", CKERequestText.Text);
            sParams.Add("@SkillSetID", hidSkillSetID.Value);
            if (e.CommandName == "SUBMIT")
            {
                lblmodalMessage.Text = "Your request has been submitted to the staff.";
                sParams.Add("@DateSubmitted", DateTime.Now);
                sParams.Add("@RequestStatus", "Submitted");
            }
            else
            {
                lblmodalMessage.Text = "Your request has been saved but <u>not</u> submitted to the staff.<br>" +
                    "<br>Make sure to submit the request to have the staff process it.";
                sParams.Add("@RequestStatus", "Saved");
            }
            cUtilities.PerformNonQuery("uspInsUpdIBSkillRequest", sParams, "LARPortal", Master.UserName);

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openRequestSaved();", true);

        }

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            Response.Redirect("Requests.aspx", true);
        }
    }
}
