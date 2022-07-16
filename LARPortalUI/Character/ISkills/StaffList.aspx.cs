using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character.ISkills
{
    public partial class StaffList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            DataTable dtPELs = new DataTable();
            SortedList sParams = new SortedList();

            sParams.Add("@CampaignID", Master.CampaignID);

            dtPELs = Classes.cUtilities.LoadDataTable("uspGetSubmittedISkills", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELsForUser");

            if (dtPELs.Rows.Count > 0)
            {
                mvISkillList.SetActiveView(vwISKillList);

                if (dtPELs.Columns["ButtonText"] == null)
                    dtPELs.Columns.Add(new DataColumn("ButtonText", typeof(string)));

                if (dtPELs.Columns["ShortRequest"] == null)
                    dtPELs.Columns.Add("ShortRequest", typeof(string));

                foreach (DataRow dRow in dtPELs.Rows)
                {
                    dRow["ButtonText"] = "View";
                    if (dRow["RequestText"].ToString().Length > 25)
                    {
                        dRow["ShortRequest"] = dRow["RequestText"].ToString().Substring(0, 25) + "...";
                    }
                    else
                        dRow["ShortRequest"] = dRow["RequestText"].ToString();
                //    dRow["DisplayAddendum"] = false;
                //    if (dRow["DateApproved"] != System.DBNull.Value)
                //    {
                //        dRow["PELStatus"] = "Approved";
                //        dRow["ButtonText"] = "View";
                //        dRow["DisplayAddendum"] = true;
                //    }
                //    else if (dRow["DateSubmitted"] != System.DBNull.Value)
                //    {
                //        dRow["PELStatus"] = "Submitted";
                //        dRow["ButtonText"] = "View";
                //        dRow["DisplayAddendum"] = true;
                //    }
                //    else if (dRow["DateStarted"] != System.DBNull.Value)
                //    {
                //        dRow["PELStatus"] = "Started";
                //        dRow["ButtonText"] = "Edit";
                //        dRow["DisplayAddendum"] = false;
                //    }
                //    else
                //    {
                //        dRow["PELStatus"] = "";
                //        dRow["ButtonText"] = "Create";
                //        dRow["DisplayAddendum"] = false;
                //        int iPELID;
                //        if (int.TryParse(dRow["RegistrationID"].ToString(), out iPELID))
                //            dRow["PELID"] = iPELID * -1;
                //    }
                }

                DataView dvPELs = new DataView(dtPELs, "", "", DataViewRowState.CurrentRows);       // "ActualArrivalDate desc, ActualArrivalTime desc, RegistrationID desc", DataViewRowState.CurrentRows);
                gvISkillList.DataSource = dvPELs;
                gvISkillList.DataBind();
            }
            else
            {
                mvISkillList.SetActiveView(vwNoISkills);
                //lblCampaignName.Text = Master.CampaignName;
            }

            if (Session["UpdatePELMessage"] != null)
            {
                string jsString = Session["UpdatePELMessage"].ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", jsString, true);
                Session.Remove("UpdatePELMessage");
            }
        }

        protected void gvISkillList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sRegistrationID = e.CommandArgument.ToString();
                Response.Redirect("StaffAnswerComment.aspx?SkillID=" + sRegistrationID, true);
        }

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
        }

    }
}