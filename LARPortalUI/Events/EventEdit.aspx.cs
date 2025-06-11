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
	public partial class EventEdit : System.Web.UI.Page
	{
		DataTable _dtCurrentSelectedPELs = new DataTable();
		DataTable _dtCampaignPELs = new DataTable();
		bool _CreateAnotherEvent = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			tbStartDateTime.Attributes.Add("onChange", "CalcDates();");
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			DateTime dtTempDate;
			DateTime dtTempTime;

			if (Request.QueryString ["EventID"] == null)
				Response.Redirect("EventList.aspx", true);

			if ((!IsPostBack) || (_CreateAnotherEvent))
			{
				SortedList sParams = new SortedList();
				sParams.Add("@StatusType", "Registration");
				DataTable dtRegStatuses = Classes.cUtilities.LoadDataTable("uspGetStatus", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetStatus");
				DataView dvRegStatus = new DataView(dtRegStatuses, "(StatusName = 'Approved') or (StatusName = 'Wait List')", "StatusName", DataViewRowState.CurrentRows);
				ddlDefaultRegStatus.DataSource = dvRegStatus;
				ddlDefaultRegStatus.DataTextField = "StatusName";
				ddlDefaultRegStatus.DataValueField = "StatusID";
				ddlDefaultRegStatus.DataBind();

				ddlDefaultRegStatus.SelectedIndex = 0;

				sParams = new SortedList();
				Classes.cUtilities.LoadDropDownList(ddlSiteList, "uspGetSites", sParams, "SiteName", "SiteID", "LARPortal", Master.UserName, lsRoutineName + ".uspGetSites");

				bool bRecordLoaded = false;
				if (Request.QueryString ["EventID"] != null)
				{
					int iEventID = 0;
					if ((int.TryParse(Request.QueryString ["EventID"], out iEventID)) || (_CreateAnotherEvent))
					{
						if (_CreateAnotherEvent)
							iEventID = -1;

						if (iEventID != -1)
						{
							sParams = new SortedList();
							sParams.Add("@EventID", iEventID);
							DataSet dsEventInfo = Classes.cUtilities.LoadDataSet("uspGetEventInfo", sParams, "LARPortal", Session ["UserName"].ToString(), lsRoutineName + ".uspGetEventInfo");

							foreach (DataRow dEventInfo in dsEventInfo.Tables [0].Rows)
							{
								hidEventID.Value = iEventID.ToString();
								sParams = new SortedList();
								bRecordLoaded = true;
								sParams.Add("@EventID", iEventID);
								tbEventName.Text = dEventInfo ["EventName"].ToString();
								tbGameLocation.Text = dEventInfo ["IGEventLocation"].ToString();
								tbEventDescription.Text = dEventInfo ["EventDescription"].ToString();
								ddlDefaultRegStatus.ClearSelection();

								tbPaymentInstructions.Text = dEventInfo ["PaymentInstructions"].ToString();

								if (dEventInfo ["SiteID"] != DBNull.Value)
									foreach (ListItem liSite in ddlSiteList.Items)
										if (liSite.Value == dEventInfo ["SiteID"].ToString())
										{
											ddlSiteList.ClearSelection();
											liSite.Selected = true;
										}

								foreach (ListItem liStatus in ddlDefaultRegStatus.Items)
									if (liStatus.Value == dEventInfo ["DefaultRegistrationStatusID"].ToString())
									{
										ddlDefaultRegStatus.ClearSelection();
										liStatus.Selected = true;
									}

								if ((DateTime.TryParse(dEventInfo ["StartDate"].ToString(), out dtTempDate)) &&
									(DateTime.TryParse(dEventInfo ["StartTime"].ToString(), out dtTempTime)))
								{
									tbStartDateTime.Text = dtTempDate.ToString("yyyy-MM-dd");
									tbStartTime.Text = dtTempTime.TimeOfDay.ToString();
								}

								if ((DateTime.TryParse(dEventInfo ["EndDate"].ToString(), out dtTempDate)) &&
									(DateTime.TryParse(dEventInfo ["EndTime"].ToString(), out dtTempTime)))
								{
									tbEndDateTime.Text = dtTempDate.ToString("yyyy-MM-dd");
									tbEndTime.Text = dtTempTime.TimeOfDay.ToString();
								}

								foreach (ListItem liSite in ddlSiteList.Items)
								{
									if (liSite.Value == dEventInfo ["SiteID"].ToString())
									{
										ddlSiteList.ClearSelection();
										liSite.Selected = true;
									}
								}

								GetDBInt(dEventInfo ["MaximumPCCount"], tbMaxPCCount, true);
								GetDBInt(dEventInfo ["BaseNPCCount"], tbBaseNPCCount, true);
								GetDBInt(dEventInfo ["NPCOverrideRatio"], tbOverrideRatio, true);
								GetDBInt(dEventInfo ["CapThresholdNotification"], tbCapThresholdNotification, true);

								GetDBDateTime(dEventInfo ["RegistrationOpenDateTime"], tbOpenRegDateTime);
								GetDBDateTime(dEventInfo ["RegistrationCloseDateTime"], tbCloseRegDateTime);
								GetDBDate(dEventInfo ["PreregistrationDeadline"], tbPreRegDeadline);
								GetDBDate(dEventInfo ["InfoSkillDeadlineDate"], tbInfoSkillDue);
								GetDBDate(dEventInfo ["PELDeadlineDate"], tbPELDue);
								GetDBDate(dEventInfo ["PaymentDueDate"], tbPaymentDate);

								GetDBMoney(dEventInfo ["PreregistrationPrice"], tbPreRegistrationPrice, true);
								GetDBMoney(dEventInfo ["LateRegistrationPrice"], tbRegPrice, true);
								GetDBMoney(dEventInfo ["CheckinPrice"], tbAtDoorPrice, true);

								GetDBBool(dEventInfo ["CapMetNotification"], ddlCapNearNotification);
								GetDBBool(dEventInfo ["AutoApproveWaitListOpenings"], ddlAutoApproveWaitlist);
								GetDBBool(dEventInfo ["PCFoodService"], ddlPCFoodService);
								GetDBBool(dEventInfo ["NPCFoodService"], ddlNPCFoodService);

								//if (DateTime.TryParse(dEventInfo ["RegistrationOpenTime"].ToString(), out dtTemp))
								//	tbOpenRegTime.Text = dtTemp.ToString("HH:mm");

								//if (DateTime.TryParse(dEventInfo ["RegistrationCloseTime"].ToString(), out dtTemp))
								//	tbCloseRegTime.Text = dtTemp.ToString("HH:mm");
							}
							if (dsEventInfo.Tables.Count >= 4)
							{
								_dtCurrentSelectedPELs = dsEventInfo.Tables [4];
								_dtCampaignPELs = dsEventInfo.Tables [3];
							}
						}
					}
				}

				if (!bRecordLoaded)
				{
					if (Session ["CampaignID"] != null)
					{
						int iCampaignID;
						if (int.TryParse(Session ["CampaignID"].ToString(), out iCampaignID))
						{
							tbEventName.Text = "";
							tbGameLocation.Text = "";
							tbEventDescription.Text = "";

							hidCampaignID.Value = iCampaignID.ToString();
							sParams = new SortedList();
							sParams.Add("@CampaignID", iCampaignID);

							DataSet dsDefaults = Classes.cUtilities.LoadDataSet("uspGetCampaignEventDefaults", sParams, "LARPortal", Session ["UserName"].ToString(), "EventDefaults.Page_PreRender");

							_dtCampaignPELs = dsDefaults.Tables [1];
							_dtCurrentSelectedPELs = new DataTable();

							foreach (DataRow dRow in dsDefaults.Tables [0].Rows)
							{
								hidEventID.Value = "-1";

								//if (dRow ["EventStartTime"] != DBNull.Value)
								//{
								//	if (DateTime.TryParse(dRow ["EventStartTime"].ToString(), out dtTemp))
								//	{
								//		string sTime;
								//		sTime = dtTemp.ToString("HH:mm");
								//		tbStartTime.Text = sTime;
								//	}
								//}

								//if (dRow ["EventEndTime"] != DBNull.Value)
								//{
								//	if (DateTime.TryParse(dRow ["EventEndTime"].ToString(), out dtTemp))
								//	{
								//		string sTime;
								//		sTime = dtTemp.ToString("HH:mm");
								//		tbEndTime.Text = sTime;
								//	}
								//}

								//if (dRow ["RegistrationOpenTime"] != DBNull.Value)
								//{
								//	if (DateTime.TryParse(dRow ["RegistrationOpenTime"].ToString(), out dtTemp))
								//	{
								//		string sTime;
								//		sTime = dtTemp.ToString("HH:mm");
								//		tbOpenRegTime.Text = sTime;

								//		tbCloseRegTime.Text = sTime;
								//	}
								//}

								if (dRow["EventStartTime"] != DBNull.Value)
								{
									hidEventStartTime.Value = dRow["EventStartTime"].ToString();
									tbStartTime.Text = dRow["EventStartTime"].ToString();
								}

								if (dRow["EventEndTime"] != DBNull.Value)
								{
									hidEventEndTime.Value = dRow["EventEndTime"].ToString();
									tbEndTime.Text = dRow["EventEndTime"].ToString ();
								}

								if (dRow["RegistrationOpenTime"] != DBNull.Value)
								{
									hidRegOpenTime.Value = dRow["RegistrationOpenTime"].ToString();
									tbOpenRegDateTime.Text = dRow["RegistrationOpenTime"].ToString();
								}

								if (dRow ["DefaultRegistrationStatusID"] != DBNull.Value)
									foreach (ListItem liStatus in ddlDefaultRegStatus.Items)
										if (liStatus.Value == dRow ["DefaultRegistrationStatusID"].ToString())
										{
											ddlDefaultRegStatus.ClearSelection();
											liStatus.Selected = true;
										}

								if (dRow ["PrimarySiteID"] != DBNull.Value)
									foreach (ListItem liSite in ddlSiteList.Items)
										if (liSite.Value == dRow ["PrimarySiteID"].ToString())
										{
											ddlSiteList.ClearSelection();
											liSite.Selected = true;
										}

								tbPaymentInstructions.Text = dRow ["PaymentInstructions"].ToString();

								GetDBInt(dRow ["MaximumPCCount"], tbMaxPCCount, true);
								GetDBInt(dRow ["BaseNPCCount"], tbBaseNPCCount, true);
								GetDBInt(dRow ["DaysToRegistrationOpenDate"], hidDaysToRegistrationOpenDate, true);
								GetDBInt(dRow ["DaysToPreregistrationDeadline"], hidDaysToPreregistrationDeadline, true);
								GetDBInt(dRow ["DaysToPaymentDue"], hidDaysToPaymentDue, true);
								GetDBMoney(dRow ["PreregistrationPrice"], tbPreRegistrationPrice, true);
								GetDBMoney(dRow ["LateRegistrationPrice"], tbRegPrice, true);
								GetDBMoney(dRow ["CheckinPrice"], tbAtDoorPrice, true);
								GetDBInt(dRow ["DaysToInfoSkillDeadlineDate"], hidDaysToInfoSkillDeadlineDate, true);
								GetDBInt(dRow ["DaysToPELDeadlineDate"], hidDaysToPELDeadlineDate, true);
								GetDBInt(dRow ["NPCOverrideRatio"], tbOverrideRatio, true);
								GetDBInt(dRow ["CapThresholdNotification"], tbCapThresholdNotification, true);
								GetDBBool(dRow ["CapMetNotification"], ddlCapNearNotification);
								GetDBBool(dRow ["AutoApproveWaitListOpenings"], ddlAutoApproveWaitlist);
								GetDBBool(dRow ["PCFoodService"], ddlPCFoodService);
								GetDBBool(dRow ["NPCFoodService"], ddlNPCFoodService);
							}
						}
					}
				}

				DataTable dtPELTypeList = new DataView(_dtCampaignPELs, "", "", DataViewRowState.CurrentRows).ToTable(true, "TemplateTypeDescription", "PELTemplateTypeID");

				dlPELTypes.DataSource = dtPELTypeList;
				dlPELTypes.DataBind();
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			DateTime dtTemp;
			double dTemp;
			int iTemp;

			SortedList sParams = new SortedList();

			sParams.Add("@UserID", Session ["UserID"].ToString());
			if (int.TryParse(hidEventID.Value, out iTemp))          // Making sure there is an event ID. If no event ID set it to -1.
				sParams.Add("@EventID", iTemp);
			else
				sParams.Add("@EventID", "-1");
			sParams.Add("@CampaignID", Session ["CampaignID"].ToString());
			sParams.Add("@EventName", tbEventName.Text.Trim());
			sParams.Add("@EventDescription", tbEventDescription.Text.Trim());
			sParams.Add("@IGEventLocation", tbGameLocation.Text.Trim());
			sParams.Add("@PaymentInstructions", tbPaymentInstructions.Text.ToString());
			sParams.Add("@SiteID", ddlSiteList.SelectedValue);

			if (DateTime.TryParse(tbStartDateTime.Text, out dtTemp))
			{
				sParams.Add("@StartDate", dtTemp.ToShortDateString());
				sParams.Add("@StartTime", dtTemp.ToShortTimeString());
			}
			if (DateTime.TryParse(tbEndDateTime.Text, out dtTemp))
			{
				sParams.Add("@EndDate", dtTemp.ToShortDateString());
				sParams.Add("@EndTime", dtTemp.ToShortTimeString());
			}
			if (DateTime.TryParse(tbPaymentDate.Text, out dtTemp))
				sParams.Add("@PaymentDueDate", dtTemp.ToShortDateString());
			if (int.TryParse(tbMaxPCCount.Text, out iTemp))
				sParams.Add("@MaximumPCCount", iTemp);
			if (int.TryParse(tbBaseNPCCount.Text, out iTemp))
				sParams.Add("@BaseNPCCount", iTemp);
			if (int.TryParse(tbOverrideRatio.Text, out iTemp))
				sParams.Add("@NPCOverrideRatio", iTemp);
			if (int.TryParse(tbCapThresholdNotification.Text, out iTemp))
				sParams.Add("@CapThresholdNotification", iTemp);
			sParams.Add("@CapMetNotification", ddlCapNearNotification.SelectedValue);
			if (DateTime.TryParse(tbOpenRegDateTime.Text, out dtTemp))
			{
				sParams.Add("@RegistrationOpenDate", dtTemp.ToShortDateString());
				sParams.Add("@RegistrationOpenTime", dtTemp.ToShortTimeString());
			}
			if (DateTime.TryParse(tbCloseRegDateTime.Text, out dtTemp))
			{
				sParams.Add("@RegistrationCloseDate", dtTemp.ToShortDateString());
				sParams.Add("@RegistrationCloseTime", dtTemp.ToShortTimeString());
			}
			if (DateTime.TryParse(tbPreRegDeadline.Text, out dtTemp))
				sParams.Add("@PreregistrationDeadline", dtTemp.ToShortDateString());
			if (Double.TryParse(tbPreRegistrationPrice.Text, out dTemp))
				sParams.Add("@PreregistrationPrice", dTemp);
			if (Double.TryParse(tbRegPrice.Text, out dTemp))
				sParams.Add("@LateRegistrationPrice", dTemp);
			if (Double.TryParse(tbAtDoorPrice.Text, out dTemp))
				sParams.Add("@CheckinPrice", dTemp);
			if (DateTime.TryParse(tbInfoSkillDue.Text, out dtTemp))
				sParams.Add("@InfoSkillDeadlineDate", dtTemp.ToShortDateString());
			if (DateTime.TryParse(tbPELDue.Text, out dtTemp))
				sParams.Add("@PELDeadlineDate", dtTemp.ToShortDateString());
			sParams.Add("@PCFoodService", ddlPCFoodService.SelectedValue);
			sParams.Add("@NPCFoodService", ddlNPCFoodService.SelectedValue);
			sParams.Add("@AutoApproveWaitListOpenings", ddlAutoApproveWaitlist.SelectedValue);

			// These do appear on the screen. - Left them as a reminder and can set where they should go.
			// @DaysToAutoCancelOtherPlayerRegistration INT = NULL,
			// @DecisionByDate                          DATE = NULL,
			// @NotificationDate                        DATE = NULL,
			// @SharePlanningInfo                       BIT = NULL,

			sParams.Add("@DefaultRegistrationStatusID", ddlDefaultRegStatus.SelectedValue);

			try
			{
				DataTable dtEventInfo = Classes.cUtilities.LoadDataTable("uspInsUpdCMEvents", sParams, "LARPortal", Session ["UserName"].ToString(), "EventEdit.btnSave");

				string sEventID = "";
				if (dtEventInfo.Rows.Count > 0)
				{
					sEventID = dtEventInfo.Rows [0] ["EventID"].ToString();

					foreach (DataListItem rpItem in dlPELTypes.Items)
					{
						RadioButtonList rbl = (RadioButtonList) rpItem.FindControl("rblPELs");
						string sValue = rbl.SelectedValue;

						HiddenField hidEventPELID = (HiddenField) rpItem.FindControl("hidEventPELID");
						if (!String.IsNullOrEmpty(sValue))
						{
							// A value was selected so we need to set it.
							SortedList sDefaultParams = new SortedList();
							sDefaultParams.Add("@UserID", Session ["UserID"].ToString());
							sDefaultParams.Add("@PELTemplateID", sValue);
							//  JB  5/14/2022  Originally was using the hidden value so save the templates. Problem was the hidden value was -1
							//                 because it was a new event. Now using event ID gotten back.
							sDefaultParams.Add("@EventID", sEventID);
							sDefaultParams.Add("@EventPELID", hidEventPELID.Value);
							Classes.cUtilities.PerformNonQuery("uspInsUpdCMEventPELTemplates", sDefaultParams, "LARPortal", Session ["UserName"].ToString());
						}
						else
						{
							if ((hidEventPELID.Value.Length > 0) && (hidEventPELID.Value != "-1"))
							{
								SortedList sDeleteParams = new SortedList();
								sDeleteParams.Add("@UserID", Session ["UserID"].ToString());
								sDeleteParams.Add("@EventPELID", hidEventPELID.Value);
								Classes.cUtilities.PerformNonQuery("uspDelCMEventPELTemplates", sDeleteParams, "LARPortal", Session ["UserName"].ToString());
							}
							// If the EventPELID is -1 and nothing is selected, it means there was nothing there for it so don't need to do anything.
						}
					}
				}

//				lblRegistrationMessage.Text = "The event has been updated for the campaign.";
				if (Request.QueryString ["EventID"] != null)
				{
					int iEventID;
					if (int.TryParse(Request.QueryString ["EventID"], out iEventID))
						if (iEventID == -1)
							iEventID = -1;
//							lblRegistrationMessage.Text = "The event has been created for the campaign.";
				}

				ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
			}
			catch (Exception ex)
			{
				string t = ex.Message;
			}
		}

		protected void rptPELTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{

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
				ddlListToSet.ClearSelection();
				SelectItem.Selected = true;
			}
		}

		protected void dlPELTypes_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DataRowView dRow = (DataRowView) e.Item.DataItem;
				DataView dvPELS = new DataView(_dtCampaignPELs, "TemplateTypeDescription = '" + dRow ["TemplateTypeDescription"].ToString() + "'", "", DataViewRowState.CurrentRows);
				RadioButtonList rblList = (RadioButtonList) e.Item.FindControl("rblPELs");
				//rblList.BorderStyle = BorderStyle.Solid;
				//rblList.BorderWidth = Unit.Pixel(1);
				rblList.DataTextField = "TemplateDescription";
				rblList.DataValueField = "PELTemplateID";
				rblList.DataSource = dvPELS;
				rblList.DataBind();
				rblList.Items.Add(new ListItem("No PEL", ""));
				//if (dvPELS.Count == 1)
					rblList.Items [0].Selected = true;

				HiddenField hidEventPELID = (HiddenField) e.Item.FindControl("hidEventPELID");
				string sDefaultValue = "";
				hidEventPELID.Value = "-1";

				if (_dtCurrentSelectedPELs.Rows.Count > 0)
				{
					DataView dvDefault = new DataView(_dtCurrentSelectedPELs, "TemplateTypeDescription = '" + dRow ["TemplateTypeDescription"].ToString() + "'", "", DataViewRowState.CurrentRows);

					if (dvDefault.Count > 0)
					{
						sDefaultValue = dvDefault [0] ["PELTemplateID"].ToString();
						hidEventPELID.Value = dvDefault [0] ["EventPELID"].ToString();
					}
				}

                //foreach (ListItem dItem in rblList.Items)
                //{
                //    if (dItem.Value == sDefaultValue)
                //    {
                //        rblList.ClearSelection();
                //        dItem.Selected = true;
                //    }
                //}
			}
		}

        protected void btnCreateAnotherEvent_Click(object sender, EventArgs e)
        {
			_CreateAnotherEvent = true;

			tbOpenRegDateTime.Text = "";
			tbCloseRegDateTime.Text = "";
			tbInfoSkillDue.Text = "";
			tbPELDue.Text = "";
			tbPreRegDeadline.Text = "";
			tbPaymentDate.Text = "";
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
			Response.Redirect("SetupEvent.aspx", true);
        }
    }
}