using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LarpPortal.Classes;

namespace LarpPortal.Campaigns.Setup.Skills
{
	public partial class SkillQualifiers : System.Web.UI.Page
	{
		protected DataTable _dtCampaignSkills = new DataTable();
		private bool _Reload = false;
		private int iAddInfoValueCount = 0;

		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				tvDisplaySkills.Attributes.Add("onclick", "postBackByObject()");
			}
			btnCloseMessage.Attributes.Add("data-dismiss", "modal");
			ddlAddInfoType.Attributes.Add("onchange", "infoTypeChanged();");
			tbNewValue.Attributes.Add("placeholder", "Enter new value");
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			if ((!IsPostBack) || (_Reload))
			{
				MasterPage_CampaignChanged(null, null);
			}
			if (tvDisplaySkills.SelectedNode != null)
			{
				divSkillItems.Attributes["display"] = "block";
				string sNodeValue = tvDisplaySkills.SelectedNode.Value;
			}

			if (Session["AllowAdditionalInfo"] == null)
				Response.Redirect("/default.aspx", true);
			if (!Session["AllowAdditionalInfo"].ToString().ToUpper().StartsWith("T"))
				Response.Redirect("/default.aspx", true);
		}

		private void PopulateTreeView(int parentId, TreeNode parentNode)
		{
			DataView dvChild = new DataView(_dtCampaignSkills, "ParentSkillNodeID = " + parentId.ToString(), "DisplayOrder", DataViewRowState.CurrentRows);
			foreach (DataRowView dr in dvChild)
			{
				int iNodeID;
				if (int.TryParse(dr["CampaignSkillNodeID"].ToString(), out iNodeID))
				{
					TreeNode childNode = new TreeNode();
					childNode.ShowCheckBox = false;
					childNode.Text = FormatDescString(dr);

					childNode.Expanded = false;
					childNode.Value = iNodeID.ToString();
					childNode.SelectAction = TreeNodeSelectAction.Select;
					parentNode.ChildNodes.Add(childNode);
					PopulateTreeView(iNodeID, childNode);
				}
			}
		}


		/// <summary>
		/// Format the text of the nide so it calls the Javascript that will run the web service and get the skill info.
		/// </summary>
		/// <param name="dTreeNode"></param>
		/// <returns></returns>
		protected string FormatDescString(DataRowView dTreeNode)
		{
			string sTreeNode = dTreeNode["SkillName"].ToString() + dTreeNode["AddInfoTypeDisplay"].ToString();
			return sTreeNode;
		}

		protected void tvDisplaySkills_SelectedNodeChanged(object sender, EventArgs e)
		{
			try
			{
				MethodBase lmth = MethodBase.GetCurrentMethod();
				string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

				TreeNode tnSelectedNode = tvDisplaySkills.SelectedNode;

				SortedList sParams = new SortedList();
				sParams.Add("@SkillNodeID", tnSelectedNode.Value);
				hidCampaignSkillNodeID.Value = tnSelectedNode.Value;

				divDropDown.Style.Add("display", "none");
				chkAllowChanges.Checked = false;

				string NodeAddInfoType = "";
				DataSet dsNodeInfo = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillByNodeID", sParams, "LARPortal", Master.UserName, lsRoutineName);
				if (dsNodeInfo.Tables.Count > 0)
					if (dsNodeInfo.Tables[0].Rows.Count > 0)
					{
						NodeAddInfoType = dsNodeInfo.Tables[0].Rows[0]["AddInfoType"].ToString();
						lblSkillName.Text = dsNodeInfo.Tables[0].Rows[0]["SkillName"].ToString();
						string sChangeable = dsNodeInfo.Tables[0].Rows[0]["Changeable"].ToString();
						if (string.IsNullOrEmpty(NodeAddInfoType))
							NodeAddInfoType = "0";
						if (sChangeable.ToUpper().StartsWith("T"))
							chkAllowChanges.Checked = true;

						int iNodeAddInfoType;
						{
							if (int.TryParse(NodeAddInfoType, out iNodeAddInfoType))
							{
								foreach (ListItem litem in ddlAddInfoType.Items)
								{
									if (litem.Value == NodeAddInfoType) // enumAddInfoType.DropDown.ToString())
									{
										ddlAddInfoType.ClearSelection();
										litem.Selected = true;
									}
								}
								DataView dvSource = new DataView(dsNodeInfo.Tables[3], "", "SortOrder", DataViewRowState.CurrentRows);
								PrepareAndBindDataTable(dsNodeInfo.Tables[3]);
								switch (iNodeAddInfoType)
								{
									case (int) enumAddInfoType.DropDown:
										divDropDown.Style.Add("display", "block");
										btnAddNewValue.Style.Add("display", "block");
										break;

									default:
										divDropDown.Style.Add("display", "none");
										btnAddNewValue.Style.Add("display", "none");
										break;
								}
							}
						}
					}
				divSkillItems.Style.Add("display", "block");
			}
			catch (Exception ex)
			{
				string t = ex.Message;
			}
		}

		string GetParents(string CampNodeID, DataTable dtSkills)
		{
			DataView dvSkills = new DataView(dtSkills, "CampaignSkillNodeID = " + CampNodeID, "", DataViewRowState.CurrentRows);
			string RetValue = "";

			foreach (DataRowView dItem in dvSkills)
			{
				if (dItem["ParentSkillNodeID"] == DBNull.Value)
					RetValue = dItem["SkillName"].ToString();
				else
					RetValue = GetParents(dItem["ParentSkillNodeID"].ToString(), dtSkills) + @" \ " + dItem["SkillName"].ToString();
			}
			return RetValue;
		}

		protected void gvDropDownItems_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				iAddInfoValueCount++;
				int iNumRows = 0;
				int.TryParse(hidNumItems.Value, out iNumRows);
				if (iAddInfoValueCount == 1)
				{
					foreach (Control c in e.Row.Cells[1].Controls)
					{
						c.Visible = false;
					}
				}
				if (iAddInfoValueCount == iNumRows)
				{
					foreach (Control c in e.Row.Cells[2].Controls)
					{
						c.Visible = false;
					}
				}
			}
		}

		protected void gvDropDownItems_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				int iRowItem = 0;
				int rowIndex = 0;
				if (int.TryParse(e.CommandArgument.ToString(), out rowIndex))
					if (int.TryParse(gvDropDownItems.DataKeys[rowIndex].Values[0].ToString(), out iRowItem))
					{
						DataView dvSource = (DataView) Session["ItemValues"];
						if ((rowIndex >= 0) &&
							(rowIndex < dvSource.Count))
						{
							int iCurrentSort = 0;
							if (int.TryParse(dvSource[rowIndex]["SortOrder"].ToString(), out iCurrentSort))
							{
								if (e.CommandName.ToUpper().Contains("UP"))
									dvSource[rowIndex]["SortOrder"] = iCurrentSort - 15;
								else if (e.CommandName.ToUpper().Contains("DOWN"))
									dvSource[rowIndex]["SortOrder"] = iCurrentSort + 15;
								else if (e.CommandName.ToUpper().Contains("DELETE"))
								{
									dvSource.Delete(rowIndex);
									gvDropDownItems.DeleteRow(rowIndex);
								}
							}
							Session["ItemValues"] = dvSource;
							PrepareAndBindDataTable(dvSource.Table);
						}
					}
			}
			catch (Exception ex)
			{
				string s = ex.Message;
			}
		}

		private void PrepareAndBindDataTable(DataTable dtItems)
		{
			try
			{
				gvDropDownItems.DataSource = null;
				gvDropDownItems.DataBind();
				int iSortOrder = 10;
				DataView dvSortOrder = new DataView(dtItems, "", "SortOrder", DataViewRowState.CurrentRows);
				hidNumItems.Value = dvSortOrder.Count.ToString();
				foreach (DataRowView dRow in dvSortOrder)
				{
					dRow["SortOrder"] = iSortOrder;
					iSortOrder += 10;
				}
				dvSortOrder = new DataView(dtItems, "", "SortOrder", DataViewRowState.CurrentRows);
				if (dvSortOrder.Count == 0)
				{
					gvDropDownItems.DataSource = null;
				}
				else
					gvDropDownItems.DataSource = dvSortOrder;
				gvDropDownItems.DataBind();
				Session["ItemValues"] = dvSortOrder;
			}
			catch (Exception ex)
			{
				string t = ex.Message;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParams = new SortedList();
			int iCampaignSkillNodeID = 0;
			int iAddInfoType = 0;
			if ((int.TryParse(hidCampaignSkillNodeID.Value, out iCampaignSkillNodeID)) &&
				(int.TryParse(ddlAddInfoType.SelectedValue, out iAddInfoType)))
			{
				sParams.Add("@CampaignSkillNodeID", iCampaignSkillNodeID);
				sParams.Add("@AddInfoType", iAddInfoType);
				if (chkAllowChanges.Checked)
					sParams.Add("@Changeable", 1);
				else
					sParams.Add("@Changeable", 0);

				sParams.Add("@UserName", Master.UserName);

				try
				{
					DataTable dtSkillInfo = Classes.cUtilities.LoadDataTable("uspUpdateCampaignSkillsAddInfoType", sParams, "LARPortal", Session["UserName"].ToString(), lsRoutineName + ".uspInsUpdCHCampaignSkills");
					DataView dvSource = (DataView) Session["ItemValues"];
					if (((int)enumAddInfoType.DropDown).ToString() == ddlAddInfoType.SelectedValue)
					{
						sParams = new SortedList();
						sParams.Add("@CampaignSkillNodeID", hidCampaignSkillNodeID.Value);
						sParams.Add("@UserName", Master.UserName);
						cUtilities.PerformNonQuery("uspDelAddInfoValuesForSkill", sParams, "LARPortal", Master.UserName);
						foreach (DataRowView dRow in dvSource)
						{
							sParams = new SortedList();
							sParams.Add("@CampaignSkillNodeID", hidCampaignSkillNodeID.Value);
							sParams.Add("@DisplayValue", dRow["DisplayValue"].ToString());
							sParams.Add("@SortOrder", dRow["SortOrder"].ToString());
							sParams.Add("@UserName", Master.UserName);
							cUtilities.PerformNonQuery("uspInsUpdCMCampaignAddInfoValues", sParams, "LARPortal", Master.UserName);
						}
					}

					lblmodalMessage.Text = "Addition Information Saved.";
					ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);

					tvDisplaySkills_SelectedNodeChanged(null, null);
				}
				catch (Exception ex)
				{
					string t = ex.Message;
				}
			}
		}

		protected void btnNewValue_Click(object sender, EventArgs e)
		{
			try
			{
				DataView dvSource = (DataView) Session["ItemValues"];
				DataTable dtSource = dvSource.ToTable();
				DataRow dNewRow = dtSource.NewRow();
				dNewRow["DisplayValue"] = tbNewValue.Text;
				dNewRow["AddInfoValuesID"] = -1;
				dNewRow["SortOrder"] = int.MaxValue;
				dtSource.Rows.Add(dNewRow);
				hidNumItems.Value = dtSource.Rows.Count.ToString();
				PrepareAndBindDataTable(dtSource);
				divSkillItems.Style.Add("display", "block");
				divDropDown.Style.Add("display", "block");
				tbNewValue.Text = "";
			}
			catch (Exception ex)
			{
				string t = ex.Message;
			}
		}

		protected void gvDropDownItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			// Need this but don't have to actually do anything.
		}

		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			DataSet dsSkillSets = new DataSet();
			SortedList sParam = new SortedList();

			sParam.Add("@CampaignID", Master.CampaignID);
			dsSkillSets = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillsWithNodes", sParam, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignSkillsWithNodes");

			_dtCampaignSkills = dsSkillSets.Tables[0];

			Session["SkillNodes"] = _dtCampaignSkills;
			Session["NodePrerequisites"] = dsSkillSets.Tables[1];
			Session["SkillTypes"] = dsSkillSets.Tables[2];
			Session["NodeExclusions"] = dsSkillSets.Tables[3];

			tvDisplaySkills.Nodes.Clear();
			tvDisplaySkills.ShowCheckBoxes = TreeNodeTypes.None;

			DataView dvTopNodes = new DataView(_dtCampaignSkills, "ParentSkillNodeID is null", "DisplayOrder", DataViewRowState.CurrentRows);
			foreach (DataRowView dvRow in dvTopNodes)
			{
				TreeNode NewNode = new TreeNode();
				NewNode.ShowCheckBox = false;

				NewNode.Text = FormatDescString(dvRow);
				NewNode.SelectAction = TreeNodeSelectAction.None;

				int iNodeID;
				if (int.TryParse(dvRow["CampaignSkillNodeID"].ToString(), out iNodeID))
				{
					NewNode.Expanded = false;
					NewNode.Value = iNodeID.ToString();

					NewNode.SelectAction = TreeNodeSelectAction.Select;
					PopulateTreeView(iNodeID, NewNode);
					tvDisplaySkills.Nodes.Add(NewNode);
				}
			}
			divSkillItems.Style.Add("display", "none");
		}
	}
}
