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
	public partial class AssignHousing : System.Web.UI.Page
	{
		public bool _Reload = false;
		public DataTable _dtRegStatus = new DataTable();
		public DataTable _dtCampaignHousing = new DataTable();
		public DataTable _dtCampaignPaymentTypes = new DataTable();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ViewState ["SortField"] = "PlayerName";
				ViewState ["SortDir"] = "asc";
			}
			btnClose.Attributes.Add("data-dismiss", "modal");
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			if ((!IsPostBack) || (_Reload))
			{
				SortedList sParams = new SortedList();
				sParams.Add("@CampaignID", Master.CampaignID);
				DataTable dtEvents = Classes.cUtilities.LoadDataTable("uspGetCampaignEvents", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignEvents");

				DataView dvEvents = new DataView(dtEvents, "", "StartDate desc", DataViewRowState.CurrentRows);

				if (dtEvents.Columns ["DisplayValue"] == null)
					dtEvents.Columns.Add("DisplayValue", typeof(string));

				DateTime dtMostReventEvent = DateTime.MaxValue;
				string sMostRecentEventID = "";

				foreach (DataRow dRow in dtEvents.Rows)
				{
					DateTime dtTemp;
					if (DateTime.TryParse(dRow ["StartDate"].ToString(), out dtTemp))
					{
						dRow ["DisplayValue"] = dRow ["EventName"].ToString() + " " + dtTemp.ToShortDateString();
						if ((dtTemp > DateTime.Today) &&
							(dtTemp <= dtMostReventEvent))
						{
							dtMostReventEvent = dtTemp;
							sMostRecentEventID = dRow ["EventID"].ToString();
						}
					}
					else
						dRow ["DisplayValue"] = dRow ["EventName"].ToString();
				}

				ddlEvent.DataSource = dvEvents;
				ddlEvent.DataTextField = "DisplayValue";
				ddlEvent.DataValueField = "EventID";
				ddlEvent.DataBind();

				if (String.IsNullOrEmpty(sMostRecentEventID))
				{
					ddlEvent.ClearSelection();
					foreach (ListItem li in ddlEvent.Items)
						if (li.Value == sMostRecentEventID)
							li.Selected = true;
				}
				ddlEvent_SelectedIndexChanged(null, null);
			}
		}

		protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			PopulateGrid();
		}


		private void PopulateGrid()
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			sParams.Add("@EventID", ddlEvent.SelectedValue);
			DataSet dsRegistrations = Classes.cUtilities.LoadDataSet("uspGetEventRegistrations", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetEventRegistrations");

			foreach (DataRow dEventInfo in dsRegistrations.Tables [0].Rows)
			{
				string sEventInfo = "";

				DateTime dtEventDate;

				sEventInfo = "<b>Event Date: </b> ";
				if (DateTime.TryParse(dEventInfo ["StartDate"].ToString(), out dtEventDate))
					sEventInfo += dtEventDate.ToShortDateString();
				else
					sEventInfo += "No start date";

				sEventInfo += " - ";

				if (DateTime.TryParse(dEventInfo ["EndDate"].ToString(), out dtEventDate))
					sEventInfo += dtEventDate.ToShortDateString();
				else
					sEventInfo += "No end date";

				sEventInfo += "&nbsp&nbsp&nbsp<b>Site: </b> ";

				if (dEventInfo ["SiteName"].ToString().Length > 0)
					sEventInfo += dEventInfo ["SiteName"].ToString();
				else
					sEventInfo += "No site chosen";

				int iNumReg = dsRegistrations.Tables [1].Select("RegistrationStatus = 'Approved'").Length;
				int iNumNotReg = dsRegistrations.Tables [1].Select("RegistrationStatus <> 'Approved'").Length;

				sEventInfo += "&nbsp;&nbsp;<b>Number Registered: </b>" + dsRegistrations.Tables [1].Rows.Count.ToString() +
					"&nbsp;&nbsp;<b>Number Approved: </b>" + iNumReg.ToString() +
					"&nbsp;&nbsp;<b>Number Not Approved: </b>" + iNumNotReg.ToString();

				lblEventInfo.Text = sEventInfo;
			}

			if (dsRegistrations.Tables [1].Columns ["OrigHousing"] == null)
				dsRegistrations.Tables [1].Columns.Add("OrigHousing", typeof(string));

			foreach (DataRow dRow in dsRegistrations.Tables [1].Rows)
			{
				dRow ["OrigHousing"] = dRow ["AssignHousing"].ToString();
			}

			Session ["HousingRegs"] = dsRegistrations.Tables [1];
			gvRegistrations.DataSource = dsRegistrations.Tables [1];
			gvRegistrations.DataBind();
			DisplaySorting();
			_Reload = false;
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				foreach (GridViewRow gvRow in gvRegistrations.Rows)
				{
					HiddenField hidRegistrationID = (HiddenField) gvRow.FindControl("hidRegistrationID");
					HiddenField hidOrigHousing = (HiddenField) gvRow.FindControl("hidOrigHousing");
					TextBox tbAssignHousing = (TextBox) gvRow.FindControl("tbAssignHousing");
					if ((hidRegistrationID != null) &&
						(hidOrigHousing != null) &&
						(tbAssignHousing != null))
					{
						if (hidOrigHousing.Value.Trim() != tbAssignHousing.Text.Trim())
						{
							SortedList sParams = new SortedList();
							sParams.Add("@RegistrationID", hidRegistrationID.Value);
							sParams.Add("@AssignHousing", tbAssignHousing.Text.Trim());
							Classes.cUtilities.PerformNonQuery("uspInsUpdCMRegistrations", sParams, "LARPortal", Master.UserName);
							_Reload = true;
						}
					}
				}

				lblmodalMessage.Text = "The event housing has been saved.";
				ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
			}
			catch
			{
			}
		}

		protected void gvRegistrations_Sorting(object sender, GridViewSortEventArgs e)
		{
			DataTable dtRegs = Session ["HousingRegs"] as DataTable;

			foreach (GridViewRow i in gvRegistrations.Rows)
			{
				HiddenField hidRegistrationID = (HiddenField) i.FindControl("hidRegistrationID");
				TextBox tbAssignHousing = (TextBox) i.FindControl("tbAssignHousing");
				DataRow [] FoundRows = dtRegs.Select("RegistrationID = " + hidRegistrationID.Value);
				if (FoundRows.Length > 0)
					FoundRows [0] ["AssignHousing"] = tbAssignHousing.Text;
			}
			Session ["HousingRegs"] = dtRegs;

			if (e.SortExpression == ViewState ["SortField"].ToString())
			{
				if (!String.IsNullOrEmpty(ViewState ["SortDir"].ToString()))
					ViewState ["SortDir"] = "DESC";
				else
					ViewState ["SortDir"] = "";
			}
			else
			{
				ViewState ["SortField"] = e.SortExpression;
				ViewState ["SortDir"] = "";
			}

			string sSort = ViewState ["SortField"].ToString() + " " + ViewState ["SortDir"].ToString();

			DataView dvDisplay = new DataView(dtRegs, "", sSort, DataViewRowState.CurrentRows);
			gvRegistrations.DataSource = dvDisplay;
			gvRegistrations.DataBind();
			DisplaySorting();
		}

		private void DisplaySorting()
		{
			if (gvRegistrations.HeaderRow != null)
			{
				int iSortedColumn = 0;
				foreach (DataControlFieldHeaderCell cHeaderCell in gvRegistrations.HeaderRow.Cells)
				{
					if (cHeaderCell.ContainingField.SortExpression == ViewState ["SortField"].ToString())
					{
						iSortedColumn = gvRegistrations.HeaderRow.Cells.GetCellIndex(cHeaderCell);
					}
				}
				if (iSortedColumn != 0)
				{
					Label lblArrowLabel = new Label();
					if (ViewState ["SortDir"].ToString().Length == 0)
						lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-up'> </span></a>";
					else
						lblArrowLabel.Text = "<a><span class='glyphicon glyphicon-arrow-down'> </span></a>";
					gvRegistrations.HeaderRow.Cells [iSortedColumn].Controls.Add(lblArrowLabel);
				}
			}
		}
	}
}