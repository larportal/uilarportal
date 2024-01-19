using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Campaigns.Setup.Skills
{
    public partial class MasterSkillsEdit : System.Web.UI.Page
    {
        DataTable _dtCurrentSelectedPELs = new DataTable();
        DataTable _dtCampaignPELs = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //tbStartDateTime.Attributes.Add("onChange", "CalcDates();");
//            btnCloseMessage.Attributes.Add("data-dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            //DateTime dtTempDate;
            //DateTime dtTempTime;
            //int iTemp;

            if (Request.QueryString["CampaignSkillsID"] == null)
                Response.Redirect("MasterSkills.aspx", true);

            if (!IsPostBack)
            {

                if (Session["CampaignID"] != null)
                {
                    int iCampaignID;
                    if (int.TryParse(Session["CampaignID"].ToString(), out iCampaignID))
                    {
                        hidCampaignID.Value = iCampaignID.ToString();
                    }
                }

                SortedList sParams = new SortedList();
                sParams.Add("@CampaignID", hidCampaignID.Value);
                //DataTable dtRegStatuses = Classes.cUtilities.LoadDataTable("uspGetStatus", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetStatus");
                //DataView dvRegStatus = new DataView(dtRegStatuses, "(StatusName = 'Approved') or (StatusName = 'Wait List')", "StatusName", DataViewRowState.CurrentRows);
                //ddlDefaultRegStatus.DataSource = dvRegStatus;
                //ddlDefaultRegStatus.DataTextField = "StatusName";
                //ddlDefaultRegStatus.DataValueField = "StatusID";
                //ddlDefaultRegStatus.DataBind();

                //ddlDefaultRegStatus.SelectedIndex = 0;

                //SortedList sParams = new SortedList();
                DataTable dtSkillTypes = Classes.cUtilities.LoadDataTable("uspGetCampaignSkillTypes", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignSkillTypes");
                DataView dvSkillTypes = new DataView(dtSkillTypes, "", "", DataViewRowState.CurrentRows);
                // Need to bind the data now
                ddlSkillType.DataSource = dvSkillTypes;
                ddlSkillType.DataTextField = "SkillTypeDescription";
                ddlSkillType.DataValueField = "SkillTypeID";
                ddlSkillType.DataBind();

                bool bRecordLoaded = false;
                if (Request.QueryString["CampaignSkillsID"] != null)
                {
                    int iSkillID = 0;
                    if (int.TryParse(Request.QueryString["CampaignSkillsID"], out iSkillID))
                    {
                        if (iSkillID != -1)
                        {
                            sParams = new SortedList();
                            sParams.Add("@CampaignSkillsID", iSkillID);
                            DataSet dsSkillInfo = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillByID", sParams, "LARPortal", Session["UserName"].ToString(), lsRoutineName + ".uspGetCampaignSkillByID");

                            foreach (DataRow dSkillInfo in dsSkillInfo.Tables[0].Rows)
                            {
                                hidCampaignSkillsID.Value = iSkillID.ToString();
                                sParams = new SortedList();
                                bRecordLoaded = true;
                                sParams.Add("@CampaignSkillsID", iSkillID);
                                tbSkillName.Text = dSkillInfo["SkillName"].ToString();
                                tbLongDescription.Text = dSkillInfo["SkillLongDescription"].ToString();
                                tbShortDescription.Text = dSkillInfo["SkillShortDescription"].ToString();
                                tbCardDescription.Text = dSkillInfo["SkillCardDescription"].ToString();
                                tbIncant.Text = dSkillInfo["SkillIncant"].ToString();

                                if (dSkillInfo["SkillTypeID"] != DBNull.Value)
                                    foreach (ListItem liSkillType in ddlSkillType.Items)
                                        if (liSkillType.Value == dSkillInfo["SkillTypeID"].ToString())
                                        {
                                            ddlSkillType.ClearSelection();
                                            liSkillType.Selected = true;
                                        }

                                chkCanUsePassively.Checked = Convert.ToBoolean(dSkillInfo["CanBeUsedPassively"]);

                                //ddlSkillType.ClearSelection();

                                //tbPaymentInstructions.Text = dSkillInfo["PaymentInstructions"].ToString();

                                //if (dSkillInfo["SiteID"] != DBNull.Value)
                                //    foreach (ListItem liSite in ddlSiteList.Items)
                                //        if (liSite.Value == dSkillInfo["SiteID"].ToString())
                                //        {
                                //            ddlSiteList.ClearSelection();
                                //            liSite.Selected = true;
                                //        }

                                //foreach (ListItem liStatus in ddlDefaultRegStatus.Items)
                                //    if (liStatus.Value == dSkillInfo["DefaultRegistrationStatusID"].ToString())
                                //    {
                                //        ddlDefaultRegStatus.ClearSelection();
                                //        liStatus.Selected = true;
                                //    }

                                //if ((DateTime.TryParse(dSkillInfo["StartDate"].ToString(), out dtTempDate)) &&
                                //    (DateTime.TryParse(dSkillInfo["StartTime"].ToString(), out dtTempTime)))
                                //{
                                //    DateTime dtCombined = dtTempDate.Date.Add(dtTempTime.TimeOfDay);
                                //    tbStartDateTime.Text = dtCombined.ToString("yyyy-MM-ddTHH:mm");
                                //}

                                //if ((DateTime.TryParse(dSkillInfo["EndDate"].ToString(), out dtTempDate)) &&
                                //    (DateTime.TryParse(dSkillInfo["EndTime"].ToString(), out dtTempTime)))
                                //{
                                //    DateTime dtCombined = dtTempDate.Date.Add(dtTempTime.TimeOfDay);
                                //    tbEndDateTime.Text = dtCombined.ToString("yyyy-MM-ddTHH:mm");
                                //}

                                //foreach (ListItem liSite in ddlSiteList.Items)
                                //{
                                //    if (liSite.Value == dSkillInfo["SiteID"].ToString())
                                //    {
                                //        ddlSiteList.ClearSelection();
                                //        liSite.Selected = true;
                                //    }
                                //}

                                //GetDBInt(dSkillInfo["MaximumPCCount"], tbMaxPCCount, true);
                                //GetDBInt(dSkillInfo["BaseNPCCount"], tbBaseNPCCount, true);
                                //GetDBInt(dSkillInfo["NPCOverrideRatio"], tbOverrideRatio, true);
                                //GetDBInt(dSkillInfo["CapThresholdNotification"], tbCapThresholdNotification, true);

                                //GetDBDateTime(dSkillInfo["RegistrationOpenDateTime"], tbOpenRegDateTime);
                                //GetDBDateTime(dSkillInfo["RegistrationCloseDateTime"], tbCloseRegDateTime);
                                //GetDBDate(dSkillInfo["PreregistrationDeadline"], tbPreRegDeadline);
                                //GetDBDate(dSkillInfo["InfoSkillDeadlineDate"], tbInfoSkillDue);
                                //GetDBDate(dSkillInfo["PELDeadlineDate"], tbPELDue);
                                //GetDBDate(dSkillInfo["PaymentDueDate"], tbPaymentDate);

                                //GetDBMoney(dSkillInfo["PreregistrationPrice"], tbPreRegistrationPrice, true);
                                //GetDBMoney(dSkillInfo["LateRegistrationPrice"], tbRegPrice, true);
                                //GetDBMoney(dSkillInfo["CheckinPrice"], tbAtDoorPrice, true);

                                //GetDBBool(dSkillInfo["CapMetNotification"], ddlCapNearNotification);
                                //GetDBBool(dSkillInfo["AutoApproveWaitListOpenings"], ddlAutoApproveWaitlist);
                                //GetDBBool(dSkillInfo["PCFoodService"], ddlPCFoodService);
                                //GetDBBool(dSkillInfo["NPCFoodService"], ddlNPCFoodService);

                                //if (DateTime.TryParse(dEventInfo ["RegistrationOpenTime"].ToString(), out dtTemp))
                                //	tbOpenRegTime.Text = dtTemp.ToString("HH:mm");

                                //if (DateTime.TryParse(dEventInfo ["RegistrationCloseTime"].ToString(), out dtTemp))
                                //	tbCloseRegTime.Text = dtTemp.ToString("HH:mm");
                            }
                            //if (dsSkillInfo.Tables.Count >= 4)
                            //{
                            //    _dtCurrentSelectedPELs = dsSkillInfo.Tables[4];
                            //    _dtCampaignPELs = dsSkillInfo.Tables[3];
                            //}
                        }
                    }
                }

                if (!bRecordLoaded)
                {

                    if (Session["CampaignID"] != null)
                    {
                        int iCampaignID;
                        if (int.TryParse(Session["CampaignID"].ToString(), out iCampaignID))
                        {
                            hidCampaignID.Value = iCampaignID.ToString();
                        }
                    }

                    if (Session["CampaignSkillsID"] != null)
                    {
                        int iSkillID;
                        if (int.TryParse(Session["CampaignSkillsID"].ToString(), out iSkillID))
                        {
                            hidCampaignSkillsID.Value = iSkillID.ToString();
                            //sParams = new SortedList();
                            //sParams.Add("@CampaignSkillsID", iSkillID);

                            //DataSet dsDefaults = Classes.cUtilities.LoadDataSet("uspGetCampaignEventDefaults", sParams, "LARPortal", Session["UserName"].ToString(), "EventDefaults.Page_PreRender");

                            //_dtCampaignPELs = dsDefaults.Tables[1];
                            //_dtCurrentSelectedPELs = new DataTable();

                            //foreach (DataRow dRow in dsDefaults.Tables[0].Rows)
                            //{
                            //    hidEventID.Value = "-1";

                            //    //if (dRow ["EventStartTime"] != DBNull.Value)
                            //    //{
                            //    //	if (DateTime.TryParse(dRow ["EventStartTime"].ToString(), out dtTemp))
                            //    //	{
                            //    //		string sTime;
                            //    //		sTime = dtTemp.ToString("HH:mm");
                            //    //		tbStartTime.Text = sTime;
                            //    //	}
                            //    //}

                            //    //if (dRow ["EventEndTime"] != DBNull.Value)
                            //    //{
                            //    //	if (DateTime.TryParse(dRow ["EventEndTime"].ToString(), out dtTemp))
                            //    //	{
                            //    //		string sTime;
                            //    //		sTime = dtTemp.ToString("HH:mm");
                            //    //		tbEndTime.Text = sTime;
                            //    //	}
                            //    //}

                            //    //if (dRow ["RegistrationOpenTime"] != DBNull.Value)
                            //    //{
                            //    //	if (DateTime.TryParse(dRow ["RegistrationOpenTime"].ToString(), out dtTemp))
                            //    //	{
                            //    //		string sTime;
                            //    //		sTime = dtTemp.ToString("HH:mm");
                            //    //		tbOpenRegTime.Text = sTime;

                            //    //		tbCloseRegTime.Text = sTime;
                            //    //	}
                            //    //}

                            //    if (dRow["DaysToPaymentDue"] != DBNull.Value)
                            //        if (int.TryParse(dRow["DaysToPaymentDue"].ToString(), out iTemp))
                            //            hidDaysToPaymentDue.Value = iTemp.ToString();

                            //    if (dRow["DaysToInfoSkillDeadlineDate"] != DBNull.Value)
                            //        if (int.TryParse(dRow["DaysToInfoSkillDeadlineDate"].ToString(), out iTemp))
                            //            hidDaysToInfoSkillDeadlineDate.Value = iTemp.ToString();

                            //    if (dRow["DefaultRegistrationStatusID"] != DBNull.Value)
                            //        foreach (ListItem liStatus in ddlDefaultRegStatus.Items)
                            //            if (liStatus.Value == dRow["DefaultRegistrationStatusID"].ToString())
                            //            {
                            //                ddlDefaultRegStatus.ClearSelection();
                            //                liStatus.Selected = true;
                            //            }

                            //    if (dRow["PrimarySiteID"] != DBNull.Value)
                            //        foreach (ListItem liSite in ddlSiteList.Items)
                            //            if (liSite.Value == dRow["PrimarySiteID"].ToString())
                            //            {
                            //                ddlSiteList.ClearSelection();
                            //                liSite.Selected = true;
                            //            }

                            //    tbPaymentInstructions.Text = dRow["PaymentInstructions"].ToString();

                            //    GetDBInt(dRow["MaximumPCCount"], tbMaxPCCount, true);
                            //    GetDBInt(dRow["BaseNPCCount"], tbBaseNPCCount, true);
                            //    GetDBInt(dRow["DaysToRegistrationOpenDate"], hidDaysToRegistrationOpenDate, true);
                            //    GetDBInt(dRow["DaysToPreregistrationDeadline"], hidDaysToPreregistrationDeadline, true);
                            //    GetDBInt(dRow["DaysToPaymentDue"], hidDaysToPaymentDue, true);
                            //    GetDBMoney(dRow["PreregistrationPrice"], tbPreRegistrationPrice, true);
                            //    GetDBMoney(dRow["LateRegistrationPrice"], tbRegPrice, true);
                            //    GetDBMoney(dRow["CheckinPrice"], tbAtDoorPrice, true);
                            //    GetDBInt(dRow["DaysToInfoSkillDeadlineDate"], hidDaysToInfoSkillDeadlineDate, true);
                            //    GetDBInt(dRow["DaysToPELDeadlineDate"], hidDaysToPELDeadlineDate, true);
                            //    GetDBInt(dRow["NPCOverrideRatio"], tbOverrideRatio, true);
                            //    GetDBInt(dRow["CapThresholdNotification"], tbCapThresholdNotification, true);
                            //    GetDBBool(dRow["CapMetNotification"], ddlCapNearNotification);
                            //    GetDBBool(dRow["AutoApproveWaitListOpenings"], ddlAutoApproveWaitlist);
                            //    GetDBBool(dRow["PCFoodService"], ddlPCFoodService);
                            //    GetDBBool(dRow["NPCFoodService"], ddlNPCFoodService);
                            //}
                        }
                    }
                }

                //DataTable dtPELTypeList = new DataView(_dtCampaignPELs, "", "", DataViewRowState.CurrentRows).ToTable(true, "TemplateTypeDescription", "PELTemplateTypeID");

                //dlPELTypes.DataSource = dtPELTypeList;
                //dlPELTypes.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iTemp;
//            bool blTemp;

            SortedList sParams = new SortedList();

            sParams.Add("@UserID", Session["UserID"].ToString());
            sParams.Add("@GameSystemID", 0);
            sParams.Add("@CampaignID", Session["CampaignID"].ToString());
            sParams.Add("@SkillTypeID", ddlSkillType.SelectedValue);

            // RP - 5/10/2018 - For the time being we'll just skip passing this in until we get the checkbox fixed - No changing existing values and adds will go in as unchecked per the stored proc
            //if (chkCanUsePassively.Checked)
            //    blTemp = true;
            //else
            //    blTemp = false;

            //sParams.Add("@CanBeUsedPassively", blTemp);

            sParams.Add("@SkillName", tbSkillName.Text.Trim());
            sParams.Add("@SkillShortDescription", tbShortDescription.Text.Trim());
            sParams.Add("@SkillLongDescription", tbLongDescription.Text.Trim());
            sParams.Add("@SkillCardDescription", tbCardDescription.Text.Trim());
            sParams.Add("@SkillIncant", tbIncant.Text.Trim());

            if (int.TryParse(hidCampaignSkillsID.Value, out iTemp))          // Making sure there is an skill ID. If no skill ID set it to -1.
                sParams.Add("@CampaignSkillsID", iTemp);
            else
                sParams.Add("@CampaignSkillsID", "-1");

            try
            {
                DataTable dtSkillInfo = Classes.cUtilities.LoadDataTable("uspInsUpdCHCampaignSkills", sParams, "LARPortal", Session["UserName"].ToString(), "MasterSkillsEdit.btnSave");

                if (Request.QueryString["SkillID"] != null)
                {
                    int iSkillID;
                    if (int.TryParse(Request.QueryString["SkillID"], out iSkillID))
                        if (iSkillID == -1)
                            iSkillID = -1;
                }

                lblmodalMessage.Text = "The skill has been saved.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception ex)
            {
                string t = ex.Message;
            }

            Response.Redirect("MasterSkills.aspx", true);
        }

        private int GetDBInt(object oValue, TextBox sValue, bool bSetDefaultValue)
        {
            int iValue = 0;
            sValue.Text = "";
            if (oValue != DBNull.Value)
            {
                if (int.TryParse(oValue.ToString(), out iValue))
                    sValue.Text = iValue.ToString();
                else
                    if (bSetDefaultValue)
                        sValue.Text = "0";
            }
            else
                if (bSetDefaultValue)
                    sValue.Text = "0";
            return iValue;
        }

        private int GetDBInt(object oValue, HiddenField hField, bool bSetDefaultValue)
        {
            int iValue = 0;
            hField.Value = "";
            if (oValue != DBNull.Value)
            {
                if (int.TryParse(oValue.ToString(), out iValue))
                    hField.Value = iValue.ToString();
                else
                    if (bSetDefaultValue)
                        hField.Value = "0";
            }
            else
                if (bSetDefaultValue)
                    hField.Value = "0";
            return iValue;
        }

        private double GetDBDouble(object oValue, TextBox sValue)
        {
            double dValue = 0;
            sValue.Text = "";
            if (oValue != DBNull.Value)
            {
                if (double.TryParse(oValue.ToString(), out dValue))
                    sValue.Text = dValue.ToString();
            }
            return dValue;
        }

        private double GetDBMoney(object oValue, TextBox sValue, bool bSetDefaultValue)
        {
            double dValue = 0;
            sValue.Text = "";
            if (oValue != DBNull.Value)
            {
                if (double.TryParse(oValue.ToString(), out dValue))
                    sValue.Text = string.Format("{0:0.00}", dValue);
                else
                    if (bSetDefaultValue)
                        sValue.Text = "0.00";
            }
            else
                if (bSetDefaultValue)
                    sValue.Text = "0.00";
            return dValue;
        }

        private string GetDBDate(object oValue, TextBox sValue)
        {
            sValue.Text = "";
            DateTime dtValue;

            if (oValue != DBNull.Value)
            {
                if (DateTime.TryParse(oValue.ToString(), out dtValue))
                    sValue.Text = dtValue.ToString("yyyy-MM-dd");
            }
            return sValue.Text;
        }

        private string GetDBDateTime(object oValue, TextBox sValue)
        {
            sValue.Text = "";
            DateTime dtValue;

            if (oValue != DBNull.Value)
            {
                if (DateTime.TryParse(oValue.ToString(), out dtValue))
                    sValue.Text = dtValue.ToString("yyyy-MM-ddTHH:mm");
            }
            return sValue.Text;
        }

        private void GetDBBool(object oValue, DropDownList ddlListToSet)
        {
            bool bTemp;

            string sSearchValue = "No";
            if (oValue != DBNull.Value)
            {
                if (bool.TryParse(oValue.ToString(), out bTemp))
                {
                    sSearchValue = "No";
                    if (bTemp)
                        sSearchValue = "Yes";
                }
                else
                    if (oValue.ToString() == "1")
                        sSearchValue = "Yes";
            }
            ListItem SelectItem = ddlListToSet.Items.FindByText(sSearchValue);
            if (SelectItem != null)
            {
                SelectItem.Selected = true;
            }
        }
    }
}