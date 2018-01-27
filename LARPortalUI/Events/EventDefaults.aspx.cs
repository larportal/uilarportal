using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Events
{
    public partial class EventDefaults : System.Web.UI.Page
    {
        private DataTable _dtCampaignPELs = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();

            tbStartTime.Attributes.Add("Placeholder", "Time");
            tbEndTime.Attributes.Add("Placeholder", "Time");
            tbMaxPCCount.Attributes.Add("Placeholder", "#");
            tbBaseNPCCount.Attributes.Add("Placeholder", "#");
            tbOpenRegDate.Attributes.Add("Placeholder", "# days b4 event");
            tbCapThresholdNotification.Attributes.Add("Placeholder", "#");
            tbOpenRegTime.Attributes.Add("Placeholder", "Time");
            tbPreRegDeadline.Attributes.Add("Placeholder", "# days b4 event");
            tbPaymentDate.Attributes.Add("Placeholder", "# days b4 event");
            tbPreRegistrationPrice.Attributes.Add("Placeholder", "Price");
            tbRegPrice.Attributes.Add("Placeholder", "Price");
            tbAtDoorPrice.Attributes.Add("Placeholder", "Price");
            tbInfoSkillDue.Attributes.Add("Placeholder", "# days b4 event");
            tbPELDue.Attributes.Add("Placeholder", "# days post event");

            if (!IsPostBack)
            {
                sParams = new SortedList();
                sParams.Add("@StatusType", "Registration");
                DataTable dtRegStatuses = Classes.cUtilities.LoadDataTable("uspGetStatus", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetStatuses");

                DataView dvRegStatuses = new DataView(dtRegStatuses, "(StatusName = 'Approved') or (StatusName = 'Wait List')", "StatusName", DataViewRowState.CurrentRows);
                ddlDefaultRegStatus.DataSource = dvRegStatuses;
                ddlDefaultRegStatus.DataTextField = "StatusName";
                ddlDefaultRegStatus.DataValueField = "StatusID";
                ddlDefaultRegStatus.DataBind();
                ddlDefaultRegStatus.Items.Insert(0, new ListItem("No default", "0"));

                sParams = new SortedList();

                DataTable dtSites = Classes.cUtilities.LoadDataTable("uspGetSites", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetSites");

                DataView dvRegSites = new DataView(dtSites, "", "SiteNameAddress", DataViewRowState.CurrentRows);
                ddlSiteList.DataSource = dvRegSites;
                ddlSiteList.DataTextField = "SiteNameAddress";
                ddlSiteList.DataValueField = "SiteID";
                ddlSiteList.DataBind();
                ddlSiteList.Items.Insert(0, new ListItem("No default", "0"));
            }

            sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);

            DataSet dsDefaults = Classes.cUtilities.LoadDataSet("uspGetCampaignEventDefaults", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignEventDefaults");

            DateTime dtTemp;

            DataView vwAllPELTypes = new DataView(dsDefaults.Tables[1]);
            DataTable dtPELType = vwAllPELTypes.ToTable(true, "TemplateTypeDescription", "PELTemplateTypeID");

            _dtCampaignPELs = dsDefaults.Tables[1];
            rptPELTypes.DataSource = dtPELType;
            rptPELTypes.DataBind();

            foreach (DataRow dRow in dsDefaults.Tables[0].Rows)
            {
                hidDefaultID.Value = dRow["CMCampaignEventDefaultID"].ToString();

                if (dRow["EventStartTime"] != DBNull.Value)
                {
                    if (DateTime.TryParse(dRow["EventStartTime"].ToString(), out dtTemp))
                    {
                        string sTime;
                        sTime = dtTemp.ToString("HH:mm");
                        tbStartTime.Text = sTime;
                    }
                }

                if (dRow["EventEndTime"] != DBNull.Value)
                {
                    if (DateTime.TryParse(dRow["EventEndTime"].ToString(), out dtTemp))
                    {
                        string sTime;
                        sTime = dtTemp.ToString("HH:mm");
                        tbEndTime.Text = sTime;
                    }
                }

                if (dRow["RegistrationOpenTime"] != DBNull.Value)
                {
                    if (DateTime.TryParse(dRow["RegistrationOpenTime"].ToString(), out dtTemp))
                    {
                        string sTime;
                        sTime = dtTemp.ToString("HH:mm");
                        tbOpenRegTime.Text = sTime;
                    }
                }

                if (dRow["PrimarySiteID"] != DBNull.Value)
                {
                    int iSiteID;
                    if (int.TryParse(dRow["PrimarySiteID"].ToString(), out iSiteID))
                    {
                        foreach (ListItem lItem in ddlSiteList.Items)
                        {
                            if (lItem.Value == iSiteID.ToString())
                            {
                                ddlSiteList.ClearSelection();
                                lItem.Selected = true;
                            }
                        }
                    }
                }

                tbPaymentInstructions.Text = dRow["PaymentInstructions"].ToString();

                GetDBInt(dRow["MaximumPCCount"], tbMaxPCCount);
                GetDBInt(dRow["BaseNPCCount"], tbBaseNPCCount);
                GetDBInt(dRow["DaysToRegistrationOpenDate"], tbOpenRegDate);

                GetDBInt(dRow["DaysToPreregistrationDeadline"], tbPreRegDeadline);
                GetDBInt(dRow["DaysToPaymentDue"], tbPaymentDate);
                GetDBInt(dRow["DaysToInfoSkillDeadlineDate"], tbInfoSkillDue);
                GetDBInt(dRow["DaysToPELDeadlineDate"], tbPELDue);
                GetDBInt(dRow["MaximumPCCount"], tbMaxPCCount);
                GetDBInt(dRow["BaseNPCCount"], tbBaseNPCCount);
                GetDBInt(dRow["NPCOverrideRatio"], tbOverrideRatio);
                GetDBInt(dRow["CapThresholdNotification"], tbCapThresholdNotification);

                GetDBMoney(dRow["PreregistrationPrice"], tbPreRegistrationPrice);
                GetDBMoney(dRow["LateRegistrationPrice"], tbRegPrice);
                GetDBMoney(dRow["CheckinPrice"], tbAtDoorPrice);

                GetDBBool(dRow["CapMetNotification"], ddlCapNearNotification);
                GetDBBool(dRow["AutoApproveWaitListOpenings"], ddlAutoApproveWaitlist);
                GetDBBool(dRow["PCFoodService"], ddlPCFoodService);
                GetDBBool(dRow["NPCFoodService"], ddlNPCFoodService);

                foreach (ListItem liStatus in ddlDefaultRegStatus.Items)
                    if (liStatus.Value == dRow["DefaultRegistrationStatusID"].ToString())
                    {
                        ddlDefaultRegStatus.ClearSelection();
                        liStatus.Selected = true;
                    }
            }
            lblHeaderCampaignName.Text = Master.CampaignName;
        }


        //protected void SetCheckDisplay(object oBool, Image imgCheckBox)
        //{
        //    if (oBool == DBNull.Value)
        //        imgCheckBox.ImageUrl = "~/img/Unchecked-Checkbox-icon.png";
        //    else if (oBool == null)
        //        imgCheckBox.ImageUrl = "~/img/Unchecked-Checkbox-icon.png";
        //    else
        //    {
        //        bool bChecked;
        //        if (bool.TryParse(oBool.ToString(), out bChecked))
        //            if (bChecked)
        //                imgCheckBox.ImageUrl = "~/img/Checked-Checkbox-icon.png";
        //            else
        //                imgCheckBox.ImageUrl = "~/img/Unchecked-Checkbox-icon.png";
        //        else
        //            imgCheckBox.ImageUrl = "~/img/Checked-Checkbox-icon.png";
        //    }
        //}


        private int GetDBInt(object oValue, TextBox sValue)
        {
            int iValue = 0;
            sValue.Text = "";
            if (oValue != DBNull.Value)
            {
                if (int.TryParse(oValue.ToString(), out iValue))
                    sValue.Text = iValue.ToString();
            }
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

        private double GetDBMoney(object oValue, TextBox sValue)
        {
            double dValue = 0;
            sValue.Text = "";
            if (oValue != DBNull.Value)
            {
                if (double.TryParse(oValue.ToString(), out dValue))
                    sValue.Text = string.Format("{0:0.00}", dValue);
            }
            return dValue;
        }

        private void GetDBBool(object oValue, DropDownList ddlListToSet)
        {
            bool bTemp;

            string sSearchValue = "";
            if (oValue != DBNull.Value)
            {
                if (bool.TryParse(oValue.ToString(), out bTemp))
                {
                    sSearchValue = "No";
                    if (bTemp)
                        sSearchValue = "Yes";
                }
            }
            ListItem SelectItem = ddlListToSet.Items.FindByText(sSearchValue);
            if (SelectItem != null)
            {
                ddlListToSet.ClearSelection();
                SelectItem.Selected = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime dtTemp;
            double dTemp;
            int iTemp;

            SortedList sParams = new SortedList();

            sParams.Add("@UserID", Master.UserID);
            sParams.Add("@CMCampaignEventDefaultID", hidDefaultID.Value);

            if (ddlDefaultRegStatus.SelectedValue == "0")
                sParams.Add("@RegistrationStatusReset", 1);
            else
                sParams.Add("@RegistrationStatus", ddlDefaultRegStatus.SelectedValue);

            if (DateTime.TryParse(tbStartTime.Text, out dtTemp))
                sParams.Add("@EventStartTime", dtTemp.ToShortTimeString());
            else
                sParams.Add("@EventStartTimeReset", 1);

            if (DateTime.TryParse(tbEndTime.Text, out dtTemp))
                sParams.Add("@EventEndTime", dtTemp.ToShortTimeString());
            else
                sParams.Add("@EventEndTimeReset", 1);

            if (DateTime.TryParse(tbOpenRegTime.Text, out dtTemp))
                sParams.Add("@RegistrationOpenTime", dtTemp.ToShortTimeString());
            else
                sParams.Add("@RegistrationOpenTimeReset", 1);

            if (ddlSiteList.SelectedValue == "0")
                sParams.Add("@PrimarySiteIDReset", 1);
            else
                sParams.Add("@PrimarySiteID", ddlSiteList.SelectedValue);

            if (int.TryParse(tbMaxPCCount.Text, out iTemp))
                sParams.Add("@MaximumPCCount", iTemp);
            else
                sParams.Add("@MaximumPCCountReset", 1);

            if (int.TryParse(tbBaseNPCCount.Text, out iTemp))
                sParams.Add("@BaseNPCCount", iTemp);
            else
                sParams.Add("@BaseNPCCountReset", 1);

            if (int.TryParse(tbOverrideRatio.Text, out iTemp))
                sParams.Add("@NPCOverrideRatio", iTemp);
            else
                sParams.Add("@NPCOverrideRatioReset", 1);

            if (int.TryParse(tbOpenRegDate.Text, out iTemp))
                sParams.Add("@DaysToRegistrationOpenDate", iTemp);
            else
                sParams.Add("@DaysToRegistrationOpenDateReset", 1);

            if (int.TryParse(tbPreRegDeadline.Text, out iTemp))
                sParams.Add("@DaysToPreregistrationDeadline", iTemp);
            else
                sParams.Add("@DaysToPreregistrationDeadlineReset", 1);

            if (int.TryParse(tbPaymentDate.Text, out iTemp))
                sParams.Add("@DaysToPaymentDue", iTemp);
            else
                sParams.Add("@DaysToPaymentDueReset", 1);

            if (int.TryParse(tbInfoSkillDue.Text, out iTemp))
                sParams.Add("@DaysToInfoSkillDeadlineDate", iTemp);
            else
                sParams.Add("@DaysToInfoSkillDeadlineDateReset", 1);

            if (int.TryParse(tbPELDue.Text, out iTemp))
                sParams.Add("@DaysToPELDeadlineDate", iTemp);
            else
                sParams.Add("@DaysToPELDeadlineDateReset", 1);

            if (int.TryParse(tbCapThresholdNotification.Text, out iTemp))
                sParams.Add("@CapThresholdNotification", iTemp);
            else
                sParams.Add("@CapThresholdNotificationReset", 1);

            if (double.TryParse(tbPreRegistrationPrice.Text, out dTemp))
                sParams.Add("@PreregistrationPrice", dTemp);
            else
                sParams.Add("@PreregistrationPriceReset", 1);

            if (double.TryParse(tbRegPrice.Text, out dTemp))
                sParams.Add("@LateRegistrationPrice", dTemp);
            else
                sParams.Add("@LateRegistrationPriceReset", 1);

            if (double.TryParse(tbAtDoorPrice.Text, out dTemp))
                sParams.Add("@CheckinPrice", dTemp);
            else
                sParams.Add("@CheckinPriceReset", 1);

            if (ddlCapNearNotification.SelectedValue.ToUpper() == "YES")
                sParams.Add("@CapMetNotification", 1);
            else if (ddlCapNearNotification.SelectedValue.ToUpper() == "NO")
                sParams.Add("@CapMetNotification", 0);
            else
                sParams.Add("@CapMetNotificationReset", 1);

            if (ddlAutoApproveWaitlist.SelectedValue.ToUpper() == "YES")
                sParams.Add("@AutoApproveWaitListOpenings", 1);
            else if (ddlAutoApproveWaitlist.SelectedValue.ToUpper() == "NO")
                sParams.Add("@AutoApproveWaitListOpenings", 0);
            else
                sParams.Add("@AutoApproveWaitListOpeningsReset", 1);

            if (ddlPCFoodService.SelectedValue.ToUpper() == "YES")
                sParams.Add("@PCFoodService", 1);
            else if (ddlPCFoodService.SelectedValue.ToUpper() == "NO")
                sParams.Add("@PCFoodService", 0);
            else
                sParams.Add("@PCFoodServiceReset", 1);

            if (ddlNPCFoodService.SelectedValue.ToUpper() == "YES")
                sParams.Add("@NPCFoodService", 1);
            else if (ddlNPCFoodService.SelectedValue.ToUpper() == "NO")
                sParams.Add("@NPCFoodService", 0);
            else
                sParams.Add("@NPCFoodServiceReset", 1);

            if (String.IsNullOrEmpty(ddlDefaultRegStatus.SelectedValue))
                sParams.Add("@DefaultRegistrationStatusID", ddlDefaultRegStatus.SelectedValue);
            else
                sParams.Add("@DefaultRegistrationStatusIDReset", 1);

            sParams.Add("@PaymentInstructions", tbPaymentInstructions.Text);

            //@DaysToAutoCancelOtherPlayerRegistration
            //@DaysToAutoCancelOtherPlayerRegistrationReset
            //@DaysToInfoSkillDeadlineDate
            //@DaysToInfoSkillDeadlineDateReset

            Classes.cUtilities.PerformNonQuery("uspInsUpdCMCampaignEventDefaults", sParams, "LARPortal", Master.UserName);

            foreach (RepeaterItem rpItem in rptPELTypes.Items)
            {
                RadioButtonList rbl = (RadioButtonList)rpItem.FindControl("rblPELs");
                string sValue = rbl.SelectedValue;

                if (String.IsNullOrEmpty(sValue))
                {
                    SortedList sDefaultParams = new SortedList();
                    sDefaultParams.Add("@UserID", Master.UserID);
                    sDefaultParams.Add("@PELTemplateID", sValue);
                    sDefaultParams.Add("@IsCampaignDefault", true);

                    Classes.cUtilities.PerformNonQuery("uspInsUpdCMPELTemplate", sDefaultParams, "LARPortal", Master.UserName);
                }
                else
                {
                    HiddenField hidTemplateTypeID = (HiddenField)rpItem.FindControl("hidTemplateTypeID");
                    SortedList sDefaultParams = new SortedList();
                    sDefaultParams.Add("@UserID", Master.UserID);
                    sDefaultParams.Add("@CampaignID", Master.CampaignID);
                    sDefaultParams.Add("@PELTemplateTypeID", hidTemplateTypeID.Value);

                    Classes.cUtilities.PerformNonQuery("uspClearCMPELTemplateCampaignDefault", sDefaultParams, "LARPortal", Master.UserName);
                }
            }

            lblRegistrationMessage.Text = "The event defaults have been changed for the campaign.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        protected void rptPELTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dRow = (DataRowView)e.Item.DataItem;
                DataView dvPELS = new DataView(_dtCampaignPELs, "TemplateTypeDescription = '" + dRow["TemplateTypeDescription"].ToString() + "'", "", DataViewRowState.CurrentRows);
                RadioButtonList rblList = (RadioButtonList)e.Item.FindControl("rblPELs");
                rblList.BorderStyle = BorderStyle.Solid;
                rblList.BorderWidth = Unit.Pixel(1);
                rblList.DataTextField = "TemplateDescription";
                rblList.DataValueField = "PELTemplateID";
                rblList.DataSource = dvPELS;
                rblList.DataBind();
                rblList.Items.Add(new ListItem("No Default", ""));
                DataView dvDefault = new DataView(_dtCampaignPELs, "TemplateTypeDescription = '" + dRow["TemplateTypeDescription"].ToString() + "' and IsCampaignDefault = 1", "", DataViewRowState.CurrentRows);
                string sDefaultValue = "";
                if (dvDefault.Count > 0)
                    sDefaultValue = dvDefault[0]["PELTemplateID"].ToString();
                foreach (ListItem dItem in rblList.Items)
                {
                    if (dItem.Value == sDefaultValue)
                    {
                        rblList.ClearSelection();
                        dItem.Selected = true;
                    }
                }
            }
        }
    }
}
