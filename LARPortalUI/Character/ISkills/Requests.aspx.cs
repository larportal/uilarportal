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
    public partial class Requests : System.Web.UI.Page
    {
        protected DataTable _dtCampaignSkills = new DataTable();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
            //            Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
            //if (!IsPostBack)
            //{
            //    tvDisplaySkills.Attributes.Add("onclick", "postBackByObject()");
            //}
            //btnCloseMessage.Attributes.Add("data-dismiss", "modal");
            //btnCloseCantSave.Attributes.Add("data_dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }



        protected void oCharSelect_CharacterChanged(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@SkillSetID", oCharSelect.SkillSetID);
            DataSet dsEvent = Classes.cUtilities.LoadDataSet("uspGetIBSkillForSkillSet", sParams, "LARPortal", Master.UserName, lsRoutineName);

            dsEvent.Tables[0].TableName = "Skills";
            //dsEvent.Tables[1].TableName = "Registrations";
            //dsEvent.Tables[2].TableName = "InfoRequest";

            DataTable dtEvents = dsEvent.Tables[0];

            if (dtEvents.Columns["StatusVisible"] == null)
                dtEvents.Columns.Add("StatusVisible", typeof(string));

            if (dtEvents.Columns["ButtonText"] == null)
                dtEvents.Columns.Add("ButtonText", typeof(string));

            if (dtEvents.Columns["KeyValue"] == null)
                dtEvents.Columns.Add("KeyValue", typeof(string));

            foreach (DataRow dr in dtEvents.Rows)
            {
                if (DBNull.Value.Equals(dr["IBSkillRequestID"]))
                {
                    dr["KeyValue"] = "IBSkillRequestID=-1&RegistrationID=" + dr["RegistrationID"].ToString() + "&SkillNodeID=" + dr["CampaignSkillNodeID"].ToString();
                    dr["ButtonText"] = "New Request";
                    dr["StatusVisible"] = "0";
                }
                else
                {
                    dr["KeyValue"] = "IBSkillRequestID=" + dr["IBSkillRequestID"].ToString();
                    if (dr["CharacterStatus"].ToString().ToUpper() == "SUBMITTED")
                    {
                        dr["ButtonText"] = "View Request";
                        dr["StatusVisible"] = "1";
                    }
                    else
                    {
                        dr["ButtonText"] = "Edit Request";
                        dr["StatusVisible"] = "1";
                    }
                }
            }

            gvRegisteredEvents.DataSource = dtEvents;
            gvRegisteredEvents.DataBind();

        }

        protected void gvAvailableSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlRequestInfo.Visible = true;
        }

        protected void gvRegisteredEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sAction = e.CommandName.ToString();
            string URLParameters = e.CommandArgument.ToString();
            if (sAction.ToUpper().StartsWith("VIEW"))
                URLParameters += "&ViewOnly=1";

            Response.Redirect("RequestEdit.aspx?" + URLParameters, true);
        }

    }
}
