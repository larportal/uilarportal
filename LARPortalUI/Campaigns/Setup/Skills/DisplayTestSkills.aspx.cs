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
	public partial class DisplayTestSkills : System.Web.UI.Page
	{
		protected DataTable _dtCampaignSkills = new DataTable();

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			MethodBase lmth = MethodBase.GetCurrentMethod();
			string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

			SortedList sParam = new SortedList();
			DataSet dsSkillSets = Classes.cUtilities.LoadDataSet("TestSkills.dbo.uspGetTestCampaignSkills", sParam, "LARPortal", Master.UserName, lsRoutineName + ".TestSkills.dbo.uspGetTestCampaignSkills");

			_dtCampaignSkills = dsSkillSets.Tables[0];

			tvDisplaySkills.Nodes.Clear();

			DataView dvTopNodes = new DataView(_dtCampaignSkills, "ParentSkillNodeID is null", "DisplayOrder", DataViewRowState.CurrentRows);
			foreach (DataRowView dvRow in dvTopNodes)
			{
				TreeNode NewNode = new TreeNode();
				NewNode.ShowCheckBox = false;

				NewNode.Text = FormatDescString(dvRow);
				NewNode.SelectAction = TreeNodeSelectAction.Expand;

				int iNodeID;
				if (int.TryParse(dvRow["CampaignSkillNodeID"].ToString(), out iNodeID))
				{
					NewNode.Expanded = true;
					NewNode.Value = iNodeID.ToString();
					NewNode.SelectAction = TreeNodeSelectAction.Expand;
					PopulateTreeView(iNodeID, NewNode);
					tvDisplaySkills.Nodes.Add(NewNode);
				}
			}
			tvDisplaySkills.ExpandAll();
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

					childNode.Expanded = true;
					childNode.Value = iNodeID.ToString();
					childNode.SelectAction = TreeNodeSelectAction.Expand;
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
			string sTreeNode = @"<a onmouseover=""GetContent(" + dTreeNode["CampaignSkillNodeID"].ToString() + @"); """ +
				   @"style=""text-decoration: none; color: black; margin-left: 0px; padding-left: 0px;"" > ( " + dTreeNode["CampaignSkillNodeID"].ToString() + " ) " +
				   dTreeNode["SkillName"].ToString();

			if (!String.IsNullOrEmpty(dTreeNode["ParentSkillNodeID"].ToString()))
				sTreeNode += ", ParentNodeID = " + dTreeNode["ParentSkillNodeID"].ToString();
			sTreeNode +=		//", SkillNodeID = " + dTreeNode["CampaignSkillNodeID"].ToString() +
				", MasterSkillNodeID = " + dTreeNode["CampaignSkillID"].ToString() + @"</a>";
			return sTreeNode;
		}
	}
}
