using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LarpPortal.Classes;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

namespace LarpPortal.Points
{
    public partial class MemberPointsView : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Session["ActiveLeftNav"] = "PointsView";
            string uName = "";
            int uID = 0;
            if (Session["Username"] != null)
                uName = Session["Username"].ToString();
            if (Session["UserID"] != null)
                uID = (Session["UserID"].ToString().ToInt32());
            BuildCPAuditTable(uID);
        }

        private void BuildCPAuditTable(int UserID)
        {
            int CampaignID = 0;
            int CharacterID = 0;
            if (Session["CampaignID"] != null)
            {
                CampaignID = (Session["CampaignID"].ToString().ToInt32());
            }

            string CampaignDDL = "";
            if (Session["CampaignName"] != null)
                CampaignDDL = Session["CampaignName"].ToString();
            Classes.cTransactions CPAudit = new Classes.cTransactions();
            DataTable dtCPAudit = new DataTable();
            dtCPAudit = CPAudit.GetCPAuditList(UserID, CampaignID, CharacterID);
            DataView dvPoints = new DataView(dtCPAudit, "", "", DataViewRowState.CurrentRows);
            gvPointsList.DataSource = dvPoints;
            gvPointsList.DataBind();
            if(dvPoints.Count == 0)
            {
                mvPointList.SetActiveView(vwPointlessList);
            }
            else
            {
                mvPointList.SetActiveView(vwPointList);
            }
        }

        protected void gvPointsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find the status (fifth column 0 based).  If 'Banked' make the ddl and button visible
                string stat = e.Row.Cells[4].Text;
                string spentat = e.Row.Cells[8].Text;
                string currentcampaign = "";
                if (Session["CampaignName"] != null)
                    currentcampaign = Session["CampaignName"].ToString();
                string currentcampaignid = Session["CampaignID"].ToString();
                if (stat != "Banked" || spentat != currentcampaign)
                {
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false;
                }
                else
                {
                    int index = gvPointsList.EditIndex;
                    DropDownList ddlDropDownList = (DropDownList)e.Row.FindControl("ddlCharacters");
                    HiddenField hidCmpPlyrID = (HiddenField)e.Row.FindControl("hidSentToCampaignPlayerID");
                    int iTemp = 0;
                    int CampaignID = 0;
                    int CampaignPlayerID = 0;
                    int UserID = 0;
                    int.TryParse(Session["CampaignID"].ToString(), out CampaignID);
                    int.TryParse(e.Row.Cells[14].Text.ToString(), out CampaignPlayerID);
                    if (int.TryParse(hidCmpPlyrID.Value.ToString(), out iTemp))
                        CampaignPlayerID = iTemp;
                    int.TryParse(Session["UserID"].ToString(), out UserID);
                    string stStoredProc = "uspGetCampaignCharacters";
                    string stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowDataBound";
                    DataTable dtCharacters = new DataTable();
                    SortedList sParams = new SortedList();
                    sParams.Add("@CampaignID", CampaignID);
                    sParams.Add("@CampaignPlayerID", CampaignPlayerID);
                    dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", UserID.ToString(), stCallingMethod);
                    ddlDropDownList.DataTextField = "PointAppChooser";
                    ddlDropDownList.DataValueField = "CharacterID";
                    ddlDropDownList.DataSource = dtCharacters;
                    ddlDropDownList.DataBind();
                    if (dtCharacters.Rows.Count > 0)
                    {
                        //ddlDropDownList.Items.Insert(0, new ListItem("Select Character", "0"));
                        //ddlDropDownList.Items.Insert(1, new ListItem("Bank Points", "1"));  // There is no CharacterID 1
                        //ddlDropDownList.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlDropDownList.Items.Insert(0, new ListItem("No characters to apply", "0"));
                        ddlDropDownList.SelectedIndex = 0;
                    }
                }
            }
        }

        protected void gvPointsList_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ApplyBanked")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPointsList.Rows[index];
                DropDownList ddl = (DropDownList)gvPointsList.Rows[index].FindControl("ddlCharacters");
                int iTemp = 0;
                double dTemp = 0;
                int CharacterID = 0;
                double TotalCP = 0;
                double TotalCharacterCap = 0;
                double RemainingCP = 0;
                double BankedCP = 0;
                double ToBankCP = 0;
                if (int.TryParse(ddl.SelectedValue.ToString(), out iTemp))
                    CharacterID = iTemp;
                HiddenField hidTotalCharacterCapForCampaign = (HiddenField)row.FindControl("hidTotalCharacterCap");
                HiddenField hidCmpPlyrID = (HiddenField)row.FindControl("hidSentToCampaignPlayerID");
                HiddenField hidCPAuditID = (HiddenField)row.FindControl("hidPlayerCPAuditID");
                int CampaignID = 0;
                int CampaignPlayerID = 0;
                int UserID = 0;
                int RowCharacterID = 0;
                int PlayerCPAuditID = 0;
                double RowTotalCP = 0;
                int.TryParse(Session["CampaignID"].ToString(), out CampaignID);
                if (int.TryParse(hidCmpPlyrID.Value.ToString(), out iTemp))
                    CampaignPlayerID = iTemp;
                if (int.TryParse(hidCPAuditID.Value.ToString(), out iTemp))
                    PlayerCPAuditID = iTemp;
                int.TryParse(Session["UserID"].ToString(), out UserID);
                string stStoredProc = "uspGetCampaignCharacters";
                string stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowDataBound1";
                DataTable dtCharacters = new DataTable();
                SortedList sParams = new SortedList();
                sParams.Add("@CampaignID", CampaignID);
                sParams.Add("@CampaignPlayerID", CampaignPlayerID);
                dtCharacters = Classes.cUtilities.LoadDataTable(stStoredProc, sParams, "LARPortal", UserID.ToString(), stCallingMethod);
                foreach (DataRow dRow in dtCharacters.Rows)
                {
                    if (int.TryParse(dRow["CharacterID"].ToString(), out iTemp))
                        RowCharacterID = iTemp;
                    if (double.TryParse(dRow["TotalCP"].ToString(), out dTemp))
                        RowTotalCP = dTemp;
                    if (RowCharacterID == CharacterID)
                        TotalCP = RowTotalCP;
                }
                if (double.TryParse(hidTotalCharacterCapForCampaign.Value.ToString(), out dTemp))
                    TotalCharacterCap = dTemp;
                RemainingCP = TotalCharacterCap - TotalCP;
                if (double.TryParse(row.Cells[3].Text, out dTemp))
                    BankedCP = dTemp;
                ToBankCP = BankedCP - RemainingCP;
                if (RemainingCP - BankedCP >= 0)
                {
                    //Apply and display applied message
                    stStoredProc = "uspApplyBankedPoints";
                    stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowDataBound2";
                    sParams.Clear();
                    sParams.Add("@UserID", UserID);
                    sParams.Add("@CharacterID", CharacterID);
                    sParams.Add("@PlayerCPAuditID", PlayerCPAuditID);
                    cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", UserID.ToString());
                    string jsString = "alert('Banked points have been applied.');";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                            "MyApplication",
                            jsString,
                            true);
                    // Refresh page
                    BuildCPAuditTable(UserID);
                }
                else
                {
                    if (RemainingCP > 0)
                    {
                        // This is where we need to split the line into balance you can apply and the leftover and then apply the line.
                        stStoredProc = "uspApplyBankedPointsSplit";
                        stCallingMethod = "MemberPointsView.aspx.gvPointsList_RowDataBound3";
                        sParams.Clear();
                        sParams.Add("@UserID", UserID);
                        sParams.Add("@CharacterID", CharacterID);
                        sParams.Add("@PlayerCPAuditID", PlayerCPAuditID);
                        sParams.Add("@PointsToApply", RemainingCP);
                        sParams.Add("@PointsToBank", ToBankCP);
                        cUtilities.PerformNonQuery(stStoredProc, sParams, "LARPortal", UserID.ToString());
                        string jsString = "alert('Banked points have been applied up to the cap. The balance has been banked.');";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                                "MyApplication",
                                jsString,
                                true);
                        // Refresh page
                        BuildCPAuditTable(UserID);

                        //Leave banked and display over cap message
                        //string jsString = "alert('This entry would put this character over the maximum allowed points.');";
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                        //        "MyApplication",
                        //        jsString,
                        //        true);
                    }
                    else
                    {
                        //Leave banked and display at max message
                        string jsString = "alert('This character is already at maximum allowed points.');";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(),
                                "MyApplication",
                                jsString,
                                true);
                    }

                }
            }
        }

        protected void btnApplyBanked_Click(object sender, EventArgs e)
        {

        }

    }
}