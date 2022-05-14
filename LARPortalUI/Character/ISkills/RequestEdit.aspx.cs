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
//            oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
            if (!IsPostBack)
            {
            //    tvDisplaySkills.Attributes.Add("onclick", "postBackByObject()");
            }
            //btnCloseMessage.Attributes.Add("data-dismiss", "modal");
            //btnCloseCantSave.Attributes.Add("data_dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

//            oCharSelect.LoadInfo();

            if (!IsPostBack)
            {
                if (Request.QueryString["RegID"] == null)
                    Response.Redirect("Requests.aspx", true);

            }
            int iRegID = 0;
            int iNodeID = 0;
            if ((!int.TryParse(Request.QueryString["RegID"], out iRegID)) ||
                (!int.TryParse(Request.QueryString["NodeID"], out iNodeID)))
                Response.Redirect("Requests.aspx", true);

            hidRegID.Value = iRegID.ToString();
            hidNodeID.Value = iNodeID.ToString();

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignSkillNodeID", iNodeID);
            sParams.Add("@RegistrationID", iRegID);

            DataSet dsEvent = Classes.cUtilities.LoadDataSet("uspGetIBGSRequest", sParams, "LARPortal", Master.UserName, lsRoutineName);

            foreach (DataRow dr in dsEvent.Tables[0].Rows)
            {
                lblEventName.Text = dr["EventName"].ToString();
                lblEventDate.Text = dr["StartDate"].ToString();
            }

            foreach (DataRow dr in dsEvent.Tables[1].Rows)
            {
                lblSkillName.Text = dr["BreadcrumbsText"].ToString();
                CKERequestText.Text = dr["RequestText"].ToString();
            }


            }

        protected void rbPayerOrChar_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region Disp1
        //private void PopulateTreeView(int parentId, TreeNode parentNode)
        //{
        //    DataView dvChild = new DataView(_dtCampaignSkills, "ParentSkillNodeID = " + parentId.ToString(), "DisplayOrder", DataViewRowState.CurrentRows);
        //    foreach (DataRowView dr in dvChild)
        //    {
        //        int iNodeID;
        //        if (int.TryParse(dr["CampaignSkillNodeID"].ToString(), out iNodeID))
        //        {
        //            TreeNode childNode = new TreeNode();
        //            childNode.ShowCheckBox = true;
        //            childNode.Text = FormatDescString(dr);

        //            childNode.Expanded = false;
        //            childNode.Value = iNodeID.ToString();
        //            if (dr["CharacterHasSkill"].ToString() == "1")
        //            {
        //                childNode.Checked = true;
        //                parentNode.Expand();
        //            }
        //            childNode.SelectAction = TreeNodeSelectAction.None;
        //            parentNode.ChildNodes.Add(childNode);
        //            PopulateTreeView(iNodeID, childNode);
        //        }
        //    }
        //}


        ///// <summary>
        ///// Tree node changed event. Skill being selected/deselected. Note - this happens AFTER the person clicks. Which means the node is already checked.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void tvSkills_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

        //    oCharSelect.LoadInfo();

        //    TreeView OrigTree = new TreeView();
        //    CopyTreeNodes(tvDisplaySkills, OrigTree);

        //    string sAddInfo = "";
        //    string sDisplayNodeText = Regex.Replace(e.Node.Text, "<.*?>", String.Empty);
        //    sAddInfo = String.Format("NodeID {0}/{1}, ", e.Node.Value, sDisplayNodeText);
        //    LogWriter oLog = new LogWriter();
        //    oLog.AddLogMessage("Skills Checked Clicked", Master.UserName, lsRoutineName, sAddInfo, Session.SessionID);

        //    if (e.Node.Checked)
        //    {
        //        // Save tree nodes so if they don't have enough points to buy the skill, we have the old one.
        //        TreeView OrigTreeView = new TreeView();
        //        TreeNode FoundNode = tvDisplaySkills.FindNode(e.Node.ValuePath);
        //        string sNodeValue = e.Node.ValuePath;

        //        if (!MarkParentNodes(FoundNode))
        //        {
        //            e.Node.Checked = false;
        //            CopyTreeNodes(OrigTree, tvDisplaySkills);
        //            TreeNode ToBeUnchecked = tvDisplaySkills.FindNode(sNodeValue);
        //            DisplayErrorMessage("You do not have all the requirements to purchase that item.");
        //            oLog.AddLogMessage("Skills Checked Clicked - MarkParentNodes", Master.UserName, lsRoutineName + ".MarkParentNodes", "Don't have the requirements.", Session.SessionID);
        //            return;
        //        }

        //        DataTable dtSkillList = new DataTable();
        //        dtSkillList.Columns.Add(new DataColumn("NodeID", typeof(int)));
        //        dtSkillList.Columns.Add(new DataColumn("NodeText", typeof(string)));
        //        dtSkillList.Columns.Add(new DataColumn("PoolID", typeof(int)));
        //        dtSkillList.Columns.Add(new DataColumn("PoolName", typeof(string)));
        //        dtSkillList.Columns.Add(new DataColumn("Cost", typeof(double)));

        //        DataTable dtPointsSpent = new DataTable();
        //        dtPointsSpent.Columns.Add(new DataColumn("PoolID", typeof(int)));
        //        dtPointsSpent.Columns.Add(new DataColumn("PoolName", typeof(string)));
        //        dtPointsSpent.Columns.Add(new DataColumn("CPSpent", typeof(double)));
        //        dtPointsSpent.Columns.Add(new DataColumn("TotalCP", typeof(double)));

        //        // Go through all of the pools so we have the list on the screen.
        //        foreach (cSkillPool cSkill in oCharSelect.CharacterInfo.SkillPools)
        //        {
        //            DataRow dNewRow = dtPointsSpent.NewRow();
        //            dNewRow["PoolID"] = cSkill.PoolID;
        //            dNewRow["PoolName"] = cSkill.PoolDescription;
        //            dNewRow["TotalCP"] = cSkill.TotalPoints;
        //            dNewRow["CPSpent"] = 0.0;

        //            dtPointsSpent.Rows.Add(dNewRow);
        //        }

        //        DataTable dtAllSkills = Session["SkillNodes"] as DataTable;
        //        DataTable dtCampaignSkillsCost = Session["CampaignSkillNodeCost"] as DataTable;
        //        DataTable dtCharacterSkillCost = Session["CharacterSkillCost"] as DataTable;

        //        int iPool = 0;

        //        foreach (TreeNode SkillNode in tvDisplaySkills.CheckedNodes)
        //        {
        //            int iSkillID;
        //            if (int.TryParse(SkillNode.Value, out iSkillID))
        //            {
        //                double SkillCost = 0.0;

        //                string sNodeText = SkillNode.Text;
        //                DataRow[] drSkillRow = dtAllSkills.Select("CampaignSkillNodeID = " + iSkillID.ToString());
        //                if (drSkillRow.Length > 0)
        //                {
        //                    // If it's in the CharacterSkillCost it means that have the skill.
        //                    DataView dvSkillCost = new DataView(dtCharacterSkillCost, "CampaignSkillNodeID = " + iSkillID.ToString(), "", DataViewRowState.CurrentRows);
        //                    if (dvSkillCost.Count > 0)
        //                    {
        //                        foreach (DataRowView drSkillCost in dvSkillCost)
        //                        {
        //                            if ((int.TryParse(drSkillCost["CampaignSkillPoolID"].ToString(), out iPool)) &&
        //                                (double.TryParse(drSkillCost["CPCostPaid"].ToString(), out SkillCost)))
        //                            {
        //                                if (iPool != 0)
        //                                {
        //                                    DataRow[] dPool = dtPointsSpent.Select("PoolID = " + iPool.ToString());
        //                                    if (dPool.Length > 0)
        //                                    {
        //                                        dPool[0]["CPSpent"] = (double)(dPool[0]["CPSpent"]) + SkillCost;
        //                                        if (SkillCost != 0)
        //                                        {
        //                                            DataRow dSkillRow = dtSkillList.NewRow();
        //                                            dSkillRow["NodeID"] = iSkillID;
        //                                            dSkillRow["NodeText"] = SkillNode.Text;
        //                                            dSkillRow["PoolID"] = iPool;
        //                                            dSkillRow["Cost"] = SkillCost;
        //                                            dtSkillList.Rows.Add(dSkillRow);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    else
        //                    {
        //                        // Means the person hasn't yet bought the skill.
        //                        DataView dvCampSkillCost = new DataView(dtCampaignSkillsCost, "CampaignSkillNodeID = " + iSkillID.ToString(), "", DataViewRowState.CurrentRows);
        //                        if (dvCampSkillCost.Count > 0)
        //                        {
        //                            foreach (DataRowView drSkillCost in dvCampSkillCost)
        //                            {
        //                                if ((int.TryParse(drSkillCost["CampaignSkillPoolID"].ToString(), out iPool)) &&
        //                                    (double.TryParse(drSkillCost["SkillCPCost"].ToString(), out SkillCost)))
        //                                {
        //                                    if (iPool != 0)
        //                                    {
        //                                        DataRow[] dPool = dtPointsSpent.Select("PoolID = " + iPool.ToString());
        //                                        if (dPool.Length > 0)
        //                                        {
        //                                            dPool[0]["CPSpent"] = (double)(dPool[0]["CPSpent"]) + SkillCost;
        //                                            if (SkillCost != 0)
        //                                            {
        //                                                DataRow dSkillRow = dtSkillList.NewRow();
        //                                                dSkillRow["NodeID"] = iSkillID;
        //                                                dSkillRow["NodeText"] = SkillNode.Text;
        //                                                dSkillRow["PoolID"] = iPool;
        //                                                dSkillRow["Cost"] = SkillCost;
        //                                                dtSkillList.Rows.Add(dSkillRow);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        bool bSpentTooMuch = false;

        //        foreach (DataRow dCostRow in dtPointsSpent.Rows)
        //        {
        //            double CPSpent;
        //            double TotalCPForPool;
        //            if ((double.TryParse(dCostRow["TotalCP"].ToString(), out TotalCPForPool)) &&
        //                 (double.TryParse(dCostRow["CPSpent"].ToString(), out CPSpent)))
        //            {
        //                if (CPSpent > TotalCPForPool)
        //                    bSpentTooMuch = true;
        //            }
        //        }

        //        if (bSpentTooMuch)
        //        {
        //            e.Node.Checked = false;
        //            CopyTreeNodes(OrigTree, tvDisplaySkills);
        //            TreeNode tnToUncheck = tvDisplaySkills.FindNode(sNodeValue);
        //            tnToUncheck.Checked = false;
        //            DisplayErrorMessage("You do not have enough points to buy that.");
        //            oLog.AddLogMessage("Skills Checked Clicked - SpentToMuch", Master.UserName, lsRoutineName, "Don't have enough points.", Session.SessionID);
        //            return;
        //        }
        //        else
        //        {
        //            if (!CheckForRequirements(e.Node.Value))
        //            {
        //                e.Node.Checked = false;
        //                CopyTreeNodes(OrigTree, tvDisplaySkills);
        //                TreeNode tnToUncheck = tvDisplaySkills.FindNode(sNodeValue);
        //                tnToUncheck.Checked = false;
        //                DisplayErrorMessage("You do not have all the requirements to purchase that item.");
        //                oLog.AddLogMessage("Skills Checked Clicked - CheckRequirements", Master.UserName, lsRoutineName, "Don't have requirements.", Session.SessionID);

        //                return;
        //            }
        //            else
        //            {
        //                CheckAllNodesWithValue(e.Node.Value, true);
        //            }
        //        }
        //        List<TreeNode> FoundNodes = FindNodesByValue(tvDisplaySkills, e.Node.Value);
        //        foreach (TreeNode t in FoundNodes)
        //        {
        //            t.ShowCheckBox = true;
        //            t.Checked = true;
        //            EnableNodeAndChildren(t);
        //        }

        //        // Just to make absolutely sure we have everything in the datatable with the current values - will now save the values in the session table.
        //        DataTable dtSkills = Session["SkillNodes"] as DataTable;
        //        DataView dv = new DataView(dtSkills, "CharacterHasSkill = true", "", DataViewRowState.CurrentRows);

        //        foreach (TreeNode tNodeSelected in tvDisplaySkills.CheckedNodes)
        //        {
        //            // JBradshaw  3/1/2019 - Changed from a select to a dataview because it was not saving to the table.
        //            DataView dvSkill = new DataView(dtSkills, "CampaignSkillNodeID = " + tNodeSelected.Value, "", DataViewRowState.CurrentRows);
        //            if (dvSkill.Count > 0)
        //            {
        //                int iHasSkill = 0;
        //                if (!int.TryParse(dvSkill[0]["CharacterHasSkill"].ToString(), out iHasSkill))
        //                    iHasSkill = 0;

        //                if (iHasSkill == 0)
        //                {
        //                    dvSkill[0]["CharacterHasSkill"] = 1;
        //                    double dSkillCPCost = 0.0;
        //                    if (double.TryParse(dvSkill[0]["SkillCPCost"].ToString(), out dSkillCPCost))
        //                        dvSkill[0]["CPCostPaid"] = dSkillCPCost;

        //                    //                            DataTable dtCharacterSkillCost = Session["CharacterSkillCost"] as DataTable;
        //                    DataView dvCost = new DataView(dtCampaignSkillsCost, "CampaignSkillNodeID = " + tNodeSelected.Value, "", DataViewRowState.CurrentRows);
        //                    foreach (DataRowView dRow in dvCost)
        //                    {
        //                        DataRow dNewRow = dtCharacterSkillCost.NewRow();
        //                        dNewRow["CampaignSkillNodeID"] = tNodeSelected.Value.ToString();
        //                        dNewRow["CampaignSkillPoolID"] = dRow["CampaignSkillPoolID"].ToString();
        //                        dNewRow["CPCostPaid"] = dRow["SkillCPCost"].ToString();
        //                        dNewRow["SkillName"] = dRow["SkillName"].ToString();
        //                        dNewRow["DisplayOrder"] = dvSkill[0]["DisplayOrder"].ToString();
        //                        dtCharacterSkillCost.Rows.Add(dNewRow);
        //                    }
        //                }
        //            }
        //        }

        //        DataView dvTest = new DataView(dtSkills, "CharacterHasSkill = '1'", "", DataViewRowState.CurrentRows);

        //        Session["SkillNodes"] = dtSkills;
        //        Session["CharacterSkillCost"] = dtCharacterSkillCost;
        //        RebuildTreeView();
        //        oLog.AddLogMessage("Skills Checked Clicked - RebuildTreeView", Master.UserName, lsRoutineName + ".RebuildTreeView", "", Session.SessionID);

        //    }
        //    else
        //    {
        //        // Person deselected a skill.
        //        TreeNode tnCopy = tvDisplaySkills.FindNode(e.Node.ValuePath);

        //        if (tnCopy == null)
        //        {
        //            return;
        //        }

        //        if (tnCopy.Value == null)
        //        {
        //            return;
        //        }
        //        int iSkillID;
        //        if (int.TryParse(tnCopy.Value, out iSkillID))
        //        {
        //            // Find the skill. If the skill exists, check to see if 
        //            List<cCharacterSkill> cSkillList = oCharSelect.CharacterInfo.CharacterSkills.Where(x => x.CampaignSkillNodeID == iSkillID).ToList();

        //            if (cSkillList.Count > 0)
        //            {
        //                if (hidAllowCharacterRebuild.Value == "0")
        //                {
        //                    tnCopy.Checked = true;
        //                    DisplayErrorMessage("This campaign is not allowing skills to be rebuilt at this time.  Once a skill is selected and saved, it cannot be changed.");
        //                    oLog.AddLogMessage("Skills Checked Clicked - Rebuild", Master.UserName, lsRoutineName + ".CharacterRebuild", "Campaign doesn't allow rebuild.", Session.SessionID);
        //                    return;
        //                }
        //            }
        //        }

        //        CheckSkillRequirementExclusions();

        //        DeselectChildNodes(tnCopy);
        //        CheckAllNodesWithValue(tnCopy.Value, false);

        //        List<TreeNode> FoundNodes = FindNodesByValue(tvDisplaySkills, tnCopy.Value);
        //        foreach (TreeNode t in FoundNodes)
        //        {
        //            // If the node is grey, change it to black. Show the checkbox, check it, enable all the children.
        //            t.Text = t.Text.Replace("grey", "black");
        //            t.ImageUrl = "";
        //            t.ShowCheckBox = true;
        //            EnableNodeAndChildren(t);
        //        }

        //        // Go through the table of skills marking all of them as not purchased.
        //        DataTable dtSkills = Session["SkillNodes"] as DataTable;
        //        foreach (DataRow dRow in dtSkills.Rows)
        //            dRow["CharacterHasSkill"] = false;


        //        DataTable dtCharacterSkillsCost = Session["CharacterSkillCost"] as DataTable;
        //        // We are going to have to go through all of the cost and if a skill is either not found or has not been checked, we don't want it anymore.
        //        DataTable dtNewCostList = dtCharacterSkillsCost.Clone();

        //        // Go through the selected tree nodes and mark the records in the table as the character hass them.
        //        foreach (TreeNode tNodeSelected in tvDisplaySkills.CheckedNodes)
        //        {
        //            DataRow[] FoundRows = dtSkills.Select("CampaignSkillNodeID = " + tNodeSelected.Value);
        //            if (FoundRows.Length > 0)
        //            {
        //                FoundRows[0]["CharacterHasSkill"] = true;
        //                DataRow[] CostRecs = dtCharacterSkillsCost.Select("CampaignSkillNodeID = " + tNodeSelected.Value);
        //                foreach (DataRow dRow in CostRecs)
        //                    dtNewCostList.ImportRow(dRow);
        //            }
        //        }
        //        Session["CharacterSkillCost"] = dtNewCostList;
        //    }

        //    ListSkills();

        //    lblMessage.Text = "Skills Changed";
        //    lblMessage.ForeColor = Color.Red;
        //}


        //protected void ListSkills()
        //{
        //    DataTable dtAllSkills = Session["SkillNodes"] as DataTable;
        //    DataTable dtCharacterSkillsCost = Session["CharacterSkillCost"] as DataTable;
        //    DataTable dtCampaignSkillsCost = Session["CampaignSkillNodeCost"] as DataTable;

        //    double TotalSpent = 0.0;

        //    DataTable dtSkillCosts = new DataTable();
        //    dtSkillCosts.Columns.Add(new DataColumn("PoolID", typeof(int)));
        //    dtSkillCosts.Columns.Add(new DataColumn("Skill", typeof(string)));
        //    dtSkillCosts.Columns.Add(new DataColumn("Cost", typeof(double)));
        //    dtSkillCosts.Columns.Add(new DataColumn("SortOrder", typeof(int)));
        //    dtSkillCosts.Columns.Add(new DataColumn("SkillID", typeof(int)));

        //    double TotalCP = 0.0;
        //    double.TryParse(Session["TotalCP"].ToString(), out TotalCP);

        //    foreach (TreeNode SkillNode in tvDisplaySkills.CheckedNodes)
        //    {
        //        int iSkillID;
        //        if (int.TryParse(SkillNode.Value, out iSkillID))
        //        {
        //            double SkillCost = 0.0;
        //            double DisplayOrder = 10;


        //            DataView dv = new DataView(dtCharacterSkillsCost, "CampaignSkillNodeID = " + iSkillID.ToString(), "", DataViewRowState.CurrentRows);
        //            DataView dvCosts = new DataView(dtCharacterSkillsCost, "CampaignSkillNodeID = " + iSkillID.ToString(), "", DataViewRowState.CurrentRows);

        //            foreach (DataRowView drSkillCost in (new DataView(dtCharacterSkillsCost, "CampaignSkillNodeID = " + iSkillID.ToString(), "", DataViewRowState.CurrentRows)))
        //            {

        //                double.TryParse(drSkillCost["CPCostPaid"].ToString(), out SkillCost);
        //                DisplayOrder = 10;
        //                double.TryParse(drSkillCost["DisplayOrder"].ToString(), out DisplayOrder);
        //                DataRow dNewRow = dtSkillCosts.NewRow();
        //                dNewRow["PoolID"] = drSkillCost["CampaignSkillPoolID"];
        //                dNewRow["Skill"] = drSkillCost["SkillName"].ToString();
        //                dNewRow["Cost"] = SkillCost;
        //                dNewRow["SortOrder"] = DisplayOrder;
        //                dNewRow["SkillID"] = iSkillID;
        //                dtSkillCosts.Rows.Add(dNewRow);
        //            }
        //            //                    }
        //        }
        //    }

        //    Session["SelectedSkills"] = dtSkillCosts;

        //    cSkillPool DefaultPool = oCharSelect.CharacterInfo.SkillPools.Find(x => x.DefaultPool == true);

        //    object oResult;
        //    if (DefaultPool != null)
        //    {
        //        string sFilter = "PoolID = " + DefaultPool.PoolID.ToString();
        //        oResult = dtSkillCosts.Compute("sum(Cost)", sFilter);
        //        double.TryParse(oResult.ToString(), out TotalSpent);
        //        TotalCP = DefaultPool.TotalPoints;
        //        DataRow[] dSkillRow = dtSkillCosts.Select("PoolID = " + DefaultPool.PoolID);
        //    }

        //    DataTable dtDisplay = new DataTable();
        //    dtDisplay.Columns.Add(new DataColumn("Skill", typeof(string)));
        //    dtDisplay.Columns.Add(new DataColumn("Cost", typeof(double)));
        //    dtDisplay.Columns.Add(new DataColumn("MainSort", typeof(int)));
        //    dtDisplay.Columns.Add(new DataColumn("SortOrder", typeof(int)));
        //    dtDisplay.Columns.Add(new DataColumn("Color", typeof(string)));

        //    DataRow NewRow = dtDisplay.NewRow();

        //    NewRow["Skill"] = "Total CP";
        //    NewRow["Cost"] = TotalCP;
        //    NewRow["MainSort"] = 1;
        //    NewRow["SortOrder"] = 1;
        //    NewRow["Color"] = DefaultPool.PoolDisplayColor;
        //    dtDisplay.Rows.Add(NewRow);

        //    NewRow = dtDisplay.NewRow();
        //    NewRow["Skill"] = "Total Spent";
        //    NewRow["Cost"] = TotalSpent;
        //    NewRow["MainSort"] = 1;
        //    NewRow["SortOrder"] = 2;
        //    NewRow["Color"] = DefaultPool.PoolDisplayColor;
        //    dtDisplay.Rows.Add(NewRow);

        //    NewRow = dtDisplay.NewRow();
        //    NewRow["Skill"] = "Total Avail";
        //    NewRow["Cost"] = (TotalCP - TotalSpent);
        //    NewRow["MainSort"] = 1;
        //    NewRow["SortOrder"] = 3;
        //    NewRow["Color"] = DefaultPool.PoolDisplayColor;
        //    dtDisplay.Rows.Add(NewRow);

        //    foreach (DataRowView dItem in new DataView(dtSkillCosts, "PoolID = " + DefaultPool.PoolID.ToString(), "SortOrder", DataViewRowState.CurrentRows))
        //    {
        //        NewRow = dtDisplay.NewRow();
        //        NewRow["Skill"] = dItem["Skill"].ToString();
        //        NewRow["MainSort"] = 1;
        //        NewRow["SortOrder"] = 10;
        //        NewRow["Cost"] = dItem["Cost"];
        //        NewRow["Color"] = DefaultPool.PoolDisplayColor;
        //        dtDisplay.Rows.Add(NewRow);
        //    }

        //    int PoolOrderOffset = 10;

        //    foreach (cSkillPool PoolItem in oCharSelect.CharacterInfo.SkillPools.OrderBy(x => x.PoolDescription))
        //    {
        //        PoolOrderOffset++;

        //        if (PoolItem.DefaultPool)       // We've already taken care of this before.
        //            continue;

        //        foreach (DataRowView dItem in new DataView(dtSkillCosts, "PoolID = " + PoolItem.PoolID.ToString(), "SortOrder", DataViewRowState.CurrentRows))
        //        {
        //            NewRow = dtDisplay.NewRow();
        //            NewRow["Skill"] = dItem["Skill"].ToString();
        //            NewRow["MainSort"] = PoolOrderOffset;
        //            NewRow["SortOrder"] = 10;
        //            NewRow["Cost"] = dItem["Cost"];
        //            NewRow["Color"] = PoolItem.PoolDisplayColor;
        //            dtDisplay.Rows.Add(NewRow);
        //        }

        //        string sFilter = "PoolID = " + PoolItem.PoolID.ToString();
        //        oResult = dtSkillCosts.Compute("sum(Cost)", sFilter);
        //        double.TryParse(oResult.ToString(), out TotalSpent);
        //        TotalCP = PoolItem.TotalPoints;

        //        NewRow = dtDisplay.NewRow();
        //        NewRow["Skill"] = PoolItem.PoolDescription;
        //        NewRow["MainSort"] = PoolOrderOffset;
        //        NewRow["SortOrder"] = 0;
        //        NewRow["Color"] = PoolItem.PoolDisplayColor;
        //        dtDisplay.Rows.Add(NewRow);

        //        NewRow = dtDisplay.NewRow();

        //        NewRow["Skill"] = "Total";
        //        NewRow["Cost"] = PoolItem.TotalPoints;
        //        NewRow["MainSort"] = PoolOrderOffset;
        //        NewRow["SortOrder"] = 1;
        //        NewRow["Color"] = PoolItem.PoolDisplayColor;
        //        dtDisplay.Rows.Add(NewRow);

        //        NewRow = dtDisplay.NewRow();
        //        NewRow["Skill"] = "Total Spent";
        //        NewRow["Cost"] = TotalSpent;
        //        NewRow["MainSort"] = PoolOrderOffset;
        //        NewRow["SortOrder"] = 2;
        //        NewRow["Color"] = PoolItem.PoolDisplayColor;
        //        dtDisplay.Rows.Add(NewRow);

        //        NewRow = dtDisplay.NewRow();
        //        NewRow["Skill"] = "Total Avail";
        //        NewRow["Cost"] = (TotalCP - TotalSpent);
        //        NewRow["MainSort"] = PoolOrderOffset;
        //        NewRow["SortOrder"] = 3;
        //        NewRow["Color"] = PoolItem.PoolDisplayColor;
        //        dtDisplay.Rows.Add(NewRow);

        //        NewRow = dtDisplay.NewRow();
        //        NewRow["Skill"] = "";
        //        NewRow["MainSort"] = PoolOrderOffset;
        //        NewRow["SortOrder"] = -1;
        //        NewRow["Color"] = PoolItem.PoolDisplayColor;
        //        dtDisplay.Rows.Add(NewRow);
        //    }
        //    DataView dvSkillCost = new DataView(dtDisplay, "", "MainSort, SortOrder", DataViewRowState.CurrentRows);
        //    gvCostList.DataSource = dvSkillCost;
        //    gvCostList.DataBind();
        //}

        //protected bool MarkParentNodes(TreeNode NodeToCheck)
        //{
        //    bool ReturnValue = true;

        //    if (NodeToCheck != null)
        //    {
        //        NodeToCheck.Checked = true;
        //        if (NodeToCheck.Parent != null)
        //        {
        //            if (hidAutoBuyParentSkills.Value == "Y")
        //                ReturnValue = MarkParentNodes(NodeToCheck.Parent);
        //            else
        //            {
        //                if (NodeToCheck.Parent.Checked)
        //                    return true;
        //                else
        //                    return false;
        //            }
        //        }
        //        else
        //            return ReturnValue;
        //    }
        //    return ReturnValue;
        //}

        //protected void DeselectChildNodes(TreeNode NodeToCheck)
        //{
        //    if (NodeToCheck != null)
        //    {
        //        NodeToCheck.Checked = false;
        //        foreach (TreeNode Child in NodeToCheck.ChildNodes)
        //        {
        //            Child.Checked = false;
        //            DeselectChildNodes(Child);
        //        }
        //    }
        //}


        ///// <summary>
        ///// Format the text of the nide so it calls the Javascript that will run the web service and get the skill info.
        ///// </summary>
        ///// <param name="dTreeNode"></param>
        ///// <returns></returns>
        //protected string FormatDescString(DataRowView dTreeNode)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

        //    string t = dTreeNode["AllowAdditionalInfo"].ToString();
        //    string s = dTreeNode["CharacterHasSkill"].ToString().ToString();

        //    string sTreeNode = @"<a onmouseover=""GetContent(" + dTreeNode["CampaignSkillNodeID"].ToString() + @"); """ +
        //           @"style=""text-decoration: none; color: black; margin-left: 0px; padding-left: 0px;"" > " + dTreeNode["SkillName"].ToString() + @"</a>";

        //    if (dTreeNode["AllowAdditionalInfo"].ToString().ToUpper() == "FALSE")
        //        return sTreeNode;

        //    int iAddInfoType;
        //    if (!int.TryParse(dTreeNode["AddInfoType"].ToString(), out iAddInfoType))
        //        return sTreeNode;

        //    if ((iAddInfoType != (int)enumAddInfoType.DropDown) &&
        //        (iAddInfoType != (int)enumAddInfoType.FreeText))
        //        return sTreeNode;

        //    if ((dTreeNode["Changeable"].ToString().ToUpper() == "FALSE") &&
        //        (dTreeNode["CharacterHasSkill"].ToString().ToString() == "TRUE"))
        //        return sTreeNode;

        //    // If we got this far - it means the add info is enabled, is either text or drop down and 
        //    // either the person doesn't have the skill or they have the skill and it's changeable.

        //    if (dTreeNode["AddInfoValue"].ToString().Length > 0)
        //    {
        //        sTreeNode += @" <a href='javascript:ChangeValue(""" + dTreeNode["CampaignSkillNodeID"].ToString() + @"""";
        //        if (dTreeNode["AddInfoValue"].ToString().Length > 0)
        //            sTreeNode += @",""" + dTreeNode["AddInfoValue"].ToString().Replace("'", "''") + @""");'>&nbsp;" +
        //                dTreeNode["AddInfoValue"].ToString().Replace("'", "''");
        //        sTreeNode += "</a>";
        //    }
        //    else if (iAddInfoType == (int)enumAddInfoType.DropDown)
        //    {
        //        sTreeNode += @" <a href='javascript:ChangeValue(""" + dTreeNode["CampaignSkillNodeID"].ToString() + @"""";
        //        sTreeNode += @", """");'>&nbsp;Select Value";
        //        sTreeNode += "</a>";
        //    }
        //    else if (iAddInfoType == (int)enumAddInfoType.FreeText)
        //    {
        //        sTreeNode += @" <a href='javascript:ChangeValue(""" + dTreeNode["CampaignSkillNodeID"].ToString() + @"""";
        //        sTreeNode += @", """");'>&nbsp;Add Value";
        //        sTreeNode += "</a>";
        //    }

        //    return sTreeNode;
        //}


        //protected double CalcSkillCost()
        //{
        //    double TotalCost = 0.0;

        //    DataTable dtSkills = Session["CharSkills"] as DataTable;
        //    foreach (DataRow dRow in dtSkills.Rows)
        //    {
        //        double CPCost;
        //        if (double.TryParse(dRow["SkillCPCost"].ToString(), out CPCost))
        //        {
        //            TotalCost += CPCost;
        //        }
        //    }
        //    return TotalCost;
        //}

        //protected void gvCostList_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if ((e.Row.Cells[0].Text.ToUpper() == "TOTAL CP") ||
        //             (e.Row.Cells[0].Text.ToUpper() == "TOTAL") ||
        //             (e.Row.Cells[0].Text.ToUpper() == "TOTAL SPENT") ||
        //             (e.Row.Cells[0].Text.ToUpper() == "TOTAL AVAIL"))
        //        {
        //            e.Row.Font.Bold = true;
        //        }
        //        if (e.Row.Cells[0].Text.ToUpper() == "TOTAL AVAIL")
        //        {
        //            e.Row.Style.Add("border-bottom", "2px solid black");
        //        }

        //        HiddenField hidColor = (HiddenField)e.Row.FindControl("hidColor");
        //        if (hidColor != null)
        //        {
        //            e.Row.ForeColor = Color.FromName(hidColor.Value);
        //        }
        //        HiddenField hidSortOrder = (HiddenField)e.Row.FindControl("hidSortOrder");
        //        if (hidSortOrder != null)
        //        {
        //            int iSortOrder;
        //            if (int.TryParse(hidSortOrder.Value, out iSortOrder))
        //            {
        //                if (iSortOrder < 10)
        //                {
        //                    e.Row.Font.Bold = true;
        //                    e.Row.Font.Size = new FontUnit(12, UnitType.Point);
        //                }
        //                if (iSortOrder < 0)
        //                {
        //                    e.Row.Font.Size = new FontUnit(12, UnitType.Point);
        //                }
        //            }
        //        }
        //    }
        //}

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

        //    LogWriter oLog = new LogWriter();

        //    oCharSelect.LoadInfo();

        //    if (!EnoughPointsToSave())
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openPointIssue();", true);
        //        return;
        //    }
        //    DataTable dtCampaignSkills = Session["SkillNodes"] as DataTable;
        //    DataTable dtCharacterSkillsCost = Session["CharacterSkillCost"] as DataTable;
        //    int CharacterSkillsSetID = -1;

        //    CharacterSkillsSetID = oCharSelect.CharacterInfo.SkillSetID;

        //    foreach (Classes.cCharacterSkill cSkill in oCharSelect.CharacterInfo.CharacterSkills)
        //    {
        //        cSkill.RecordStatus = Classes.RecordStatuses.Delete;
        //        cSkill.CharacterSkillSetID = CharacterSkillsSetID;
        //        //				CharacterSkillsSetID = cSkill.CharacterSkillSetID;
        //    }

        //    foreach (TreeNode SkillNode in tvDisplaySkills.CheckedNodes)
        //    {
        //        int iSkillNodeID;
        //        if (int.TryParse(SkillNode.Value, out iSkillNodeID))
        //        {
        //            var FoundRecord = oCharSelect.CharacterInfo.CharacterSkills.Find(x => x.CampaignSkillNodeID == iSkillNodeID);
        //            if (FoundRecord != null)
        //            {
        //                FoundRecord.RecordStatus = Classes.RecordStatuses.Active;
        //                DataView dvCampaignSkill = new DataView(dtCampaignSkills, "CampaignSkillNodeID = " + iSkillNodeID.ToString(), "", DataViewRowState.CurrentRows);
        //                if (dvCampaignSkill.Count > 0)
        //                {
        //                    FoundRecord.AddInfoValue = dvCampaignSkill[0]["AddInfoValue"].ToString();
        //                }
        //            }
        //            else
        //            {
        //                Classes.cCharacterSkill Newskill = new Classes.cCharacterSkill();
        //                Newskill.CharacterSkillID = -1;
        //                Newskill.CharacterID = Master.UserID;
        //                Newskill.CampaignSkillNodeID = iSkillNodeID;
        //                Newskill.CharacterSkillSetID = CharacterSkillsSetID;
        //                //                        Newskill.CPCostPaid = 0;
        //                DataView dvCampaignSkill = new DataView(dtCampaignSkills, "CampaignSkillNodeID = " + iSkillNodeID.ToString(), "", DataViewRowState.CurrentRows);
        //                if (dvCampaignSkill.Count > 0)
        //                {
        //                    //double dSkillCPCost = 0;
        //                    //if (double.TryParse(dvCampaignSkill[0]["SkillCPCost"].ToString(), out dSkillCPCost))
        //                    //    Newskill.CPCostPaid = dSkillCPCost;
        //                    Newskill.AddInfoValue = dvCampaignSkill[0]["AddInfoValue"].ToString();
        //                    DataView dvCosts = new DataView(dtCharacterSkillsCost, "CampaignSkillNodeID = " + iSkillNodeID.ToString(), "", DataViewRowState.CurrentRows);
        //                    foreach (DataRowView dCost in dvCosts)
        //                    {
        //                        cCharacterSkillCost NewCost = new cCharacterSkillCost();
        //                        NewCost.CharacterSkillNodeID = iSkillNodeID;
        //                        NewCost.CharacterSkillSetID = CharacterSkillsSetID;
        //                        NewCost.CampaignSkillPoolID = cUtilities.ParseStringToInt32(dCost["CampaignSkillPoolID"].ToString());
        //                        NewCost.CharacterSkillCostID = -1;
        //                        NewCost.WhenPurchased = DateTime.Now;
        //                        //bool bisDefaultPool;
        //                        //if (bool.TryParse(dCost["DefaultPool"].ToString(), out bisDefaultPool))
        //                        //    NewCost.isDefaultPool = bisDefaultPool;
        //                        //else
        //                        //    NewCost.isDefaultPool = false;

        //                        double CPCostPaid;
        //                        if (double.TryParse(dCost["CPCostPaid"].ToString(), out CPCostPaid))
        //                            NewCost.CPCostPaid = CPCostPaid;
        //                        else
        //                            NewCost.CPCostPaid = 0;
        //                        Newskill.SkillCost.Add(NewCost);
        //                    }
        //                }
        //                oCharSelect.CharacterInfo.CharacterSkills.Add(Newskill);
        //            }
        //        }
        //    }

        //    try
        //    {
        //        foreach (cCharacterSkill dSkill in oCharSelect.CharacterInfo.CharacterSkills)
        //        {
        //            if (dSkill.RecordStatus == RecordStatuses.Active)
        //            {
        //                try
        //                {
        //                    dSkill.Save(Master.UserName, Master.UserID);
        //                }
        //                catch (SqlException sEx)
        //                {
        //                    string t = sEx.Message;
        //                }
        //                catch (Exception ex)
        //                {
        //                    string t = ex.Message;
        //                }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    dSkill.Delete(Master.UserName, Master.UserID);
        //                }
        //                catch (SqlException sEx)
        //                {
        //                    string t = sEx.Message;
        //                }
        //                catch (Exception ex)
        //                {
        //                    string t = ex.Message;
        //                }

        //            }
        //        }
        //    }
        //    catch (SqlException sEx)
        //    {
        //        string t = sEx.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        string t = ex.Message;
        //    }

        //    //  JBradshaw  8/15/2021  Added call for when all skills have been saved.
        //    SortedList sSkillsComplete = new SortedList();
        //    sSkillsComplete.Add("@CharacterID", oCharSelect.CharacterID);
        //    sSkillsComplete.Add("@CampaignID", oCharSelect.CharacterInfo.CampaignID);
        //    sSkillsComplete.Add("@CharacterSkillSetID", oCharSelect.CharacterInfo.SkillSetID);
        //    cUtilities.PerformNonQuery("uspInsUpdCHCharactersSkillCompleted", sSkillsComplete, "LARPortal", Master.UserName);

        //    oLog.AddLogMessage("Skills - Save Button", Master.UserName, lsRoutineName, "", Session.SessionID);
        //    lblmodalMessage.Text = "Character " + oCharSelect.CharacterInfo.AKA + " has been saved.";
        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
        //    _Reload = true;
        //}


        //public void CopyTreeNodes(TreeView SourceTreeView, TreeView DesSourceView)
        //{
        //    TreeNode newTn = null;
        //    DesSourceView.Nodes.Clear();
        //    foreach (TreeNode tn in SourceTreeView.Nodes)
        //    {
        //        newTn = new TreeNode(tn.Text, tn.Value);
        //        newTn.Checked = tn.Checked;
        //        newTn.Expanded = tn.Expanded;
        //        newTn.ShowCheckBox = tn.ShowCheckBox;
        //        newTn.ImageUrl = tn.ImageUrl;

        //        CopyChildren(newTn, tn);
        //        DesSourceView.Nodes.Add(newTn);
        //    }
        //}
        //public void CopyChildren(TreeNode parent, TreeNode original)
        //{
        //    TreeNode newTn;
        //    foreach (TreeNode tn in original.ChildNodes)
        //    {
        //        newTn = new TreeNode(tn.Text, tn.Value);
        //        newTn.Checked = tn.Checked;
        //        newTn.Expanded = tn.Expanded;
        //        newTn.ShowCheckBox = tn.ShowCheckBox;
        //        newTn.ImageUrl = tn.ImageUrl;

        //        parent.ChildNodes.Add(newTn);
        //        CopyChildren(newTn, tn);
        //    }
        //}

        //private bool CheckForRequirements(string sValueToCheckFor)
        //{
        //    bool bMeetAllRequirements = true;

        //    SortedList sParams = new SortedList();
        //    sParams.Add("@SkillNodeID", sValueToCheckFor);
        //    DataSet dsRequire = cUtilities.LoadDataSet("uspGetNodeRequirements", sParams, "LARPortal", Master.UserName, "CharSkill.aspx_CheckForRequirements");

        //    // Get the list of items we can't have if we purchased the item.
        //    DataView dvExcludeRows = new DataView(dsRequire.Tables[0], "ExcludeFromPurchase = true", "SkillNodeID", DataViewRowState.CurrentRows);

        //    foreach (DataRowView dRow in dvExcludeRows)
        //    {
        //        if (dRow["PrerequisiteSkillNodeID"] != DBNull.Value)
        //        {
        //            int iPreReq;
        //            if (int.TryParse(dRow["PrerequisiteSkillNodeID"].ToString(), out iPreReq))
        //            {
        //                List<TreeNode> FoundNodes = FindNodesByValue(tvDisplaySkills, iPreReq.ToString());
        //                foreach (TreeNode tNode in FoundNodes)
        //                    DisableNodeAndChildren(tNode);
        //            }
        //        }
        //    }

        //    bMeetAllRequirements = true;

        //    DataView dvRequiredRows = new DataView(dsRequire.Tables[0], "ExcludeFromPurchase = false", "SkillNodeID", DataViewRowState.CurrentRows);

        //    foreach (DataRowView dRow in dvRequiredRows)
        //    {
        //        if (dRow["PrerequisiteSkillNodeID"] != DBNull.Value)
        //        {
        //            int iPreReq;
        //            if (int.TryParse(dRow["PrerequisiteSkillNodeID"].ToString(), out iPreReq))
        //            {
        //                if (iPreReq != 0)
        //                {
        //                    List<TreeNode> FoundNodes = FindNodesByValue(tvDisplaySkills, iPreReq.ToString());
        //                    if (FoundNodes.Count == 0)
        //                        bMeetAllRequirements = false;
        //                    else
        //                        foreach (TreeNode tNode in FoundNodes)
        //                            if (!tNode.Checked)
        //                                bMeetAllRequirements = false;
        //                }
        //            }
        //        }
        //    }

        //    // Go build the tables of pools and what they have already spent.
        //    DataTable dtPointsSpent = new DataTable();
        //    dtPointsSpent.Columns.Add(new DataColumn("PoolID", typeof(int)));
        //    dtPointsSpent.Columns.Add(new DataColumn("PoolName", typeof(string)));
        //    dtPointsSpent.Columns.Add(new DataColumn("CPSpent", typeof(double)));

        //    // Go through all of the pools so we have the list on the screen.
        //    foreach (cSkillPool cSkill in oCharSelect.CharacterInfo.SkillPools)
        //    {
        //        DataRow dNewRow = dtPointsSpent.NewRow();
        //        dNewRow["PoolID"] = cSkill.PoolID;
        //        dNewRow["PoolName"] = cSkill.PoolDescription;
        //        dNewRow["CPSpent"] = 0.0;
        //        dtPointsSpent.Rows.Add(dNewRow);
        //    }

        //    DataTable dtCharacterSkillCost = Session["CharacterSkillCost"] as DataTable;

        //    foreach (DataRow dSkillRec in dtCharacterSkillCost.Rows)
        //    {
        //        int CampaignSkillPoolID;
        //        double CPCostPaid;

        //        if ((int.TryParse(dSkillRec["CampaignSkillPoolID"].ToString(), out CampaignSkillPoolID)) &&
        //            (double.TryParse(dSkillRec["CPCostPaid"].ToString(), out CPCostPaid)))
        //        {
        //            DataRow[] dPool = dtPointsSpent.Select("PoolID = " + CampaignSkillPoolID.ToString());
        //            if (dPool.Length > 0)
        //            {
        //                dPool[0]["CPSpent"] = (double)(dPool[0]["CPSpent"]) + CPCostPaid;
        //            }
        //        }
        //    }

        //    foreach (DataRow dGroupID in dsRequire.Tables[2].Rows)
        //    {
        //        int iPreReqGroup;
        //        int iNumReqSkills;
        //        double iNumReqPoints;
        //        int iReqPool;
        //        bool bDoneChecking = false;
        //        if (int.TryParse(dGroupID["PrerequisiteGroupID"].ToString(), out iPreReqGroup))
        //        {
        //            string sRowFilter = "isnull(PrerequisiteGroupID, -1) = " + iPreReqGroup.ToString();
        //            DataView dvGroupRows = new DataView(dsRequire.Tables[0], sRowFilter, "", DataViewRowState.CurrentRows);

        //            if (dvGroupRows.Count > 0)
        //            {
        //                if (int.TryParse(dvGroupRows[0]["NumGroupSkillsRequired"].ToString(), out iNumReqSkills))
        //                {
        //                    if (iNumReqSkills > 0)
        //                    {
        //                        DataView dSkills = new DataView(dsRequire.Tables[1], "PrerequisiteGroupID = " + iPreReqGroup.ToString(), "", DataViewRowState.CurrentRows);
        //                        if (dSkills.Count > 0)
        //                        {
        //                            // There were records. Convert the dataview of reuired nodes convert to a list of string - easier to process.
        //                            List<string> ReqSkillNodes = dSkills.ToTable().AsEnumerable().Select(x => x[1].ToString()).ToList();
        //                            // If we find the value we are looking for - remove it.
        //                            ReqSkillNodes.Remove(sValueToCheckFor);
        //                            List<TreeNode> FoundNode = FindNodesByValueList(ReqSkillNodes);
        //                            if (FoundNode.Count < iNumReqSkills)
        //                                bMeetAllRequirements = false;
        //                        }
        //                        bDoneChecking = true;
        //                    }
        //                }
        //                if (!bDoneChecking)     // Means we have already check number of skills required.
        //                {
        //                    if ((double.TryParse(dvGroupRows[0]["NumPointsRequired"].ToString(), out iNumReqPoints)) &&
        //                        (int.TryParse(dvGroupRows[0]["PoolPoints"].ToString(), out iReqPool)))
        //                    {
        //                        foreach (DataRow PolRec in dtPointsSpent.Rows)
        //                        {
        //                            PolRec["CPSpent"] = 0.0;
        //                        }

        //                        DataView dSkills = new DataView(dsRequire.Tables[1], "PrerequisiteGroupID = " + iPreReqGroup.ToString(), "", DataViewRowState.CurrentRows);
        //                        if (dSkills.Count > 0)
        //                        {
        //                            foreach (DataRowView dRecRow in dSkills)
        //                            {
        //                                int iNodeID;
        //                                if (int.TryParse(dRecRow["SkillNodeID"].ToString(), out iNodeID))
        //                                {
        //                                    DataView dvSkillCost = new DataView(dtCharacterSkillCost, "CampaignSkillNodeID = " + iNodeID, "", DataViewRowState.CurrentRows);
        //                                    foreach (DataRowView drSkillCost in dvSkillCost)
        //                                    {
        //                                        int iCampaignSkillPoolID;
        //                                        double dCost;

        //                                        if ((int.TryParse(drSkillCost["CampaignSkillPoolID"].ToString(), out iCampaignSkillPoolID)) &&
        //                                            (double.TryParse(drSkillCost["CPCostPaid"].ToString(), out dCost)))
        //                                        {
        //                                            DataView PointsSpent = new DataView(dtPointsSpent, "PoolID = " + iReqPool, "", DataViewRowState.CurrentRows);
        //                                            if (PointsSpent.Count > 0)
        //                                            {
        //                                                //double dPointsSpent;
        //                                                //if (double.TryParse(PointsSpent[0]["CPSpent"].ToString(), out dPointsSpent))
        //                                                //{
        //                                                PointsSpent[0]["CPSpent"] = (double)PointsSpent[0]["CPSpent"] + dCost;
        //                                                //}
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        DataView dvFinalTotal = new DataView(dtPointsSpent, "PoolID = " + iReqPool, "", DataViewRowState.CurrentRows);
        //                        if (dvFinalTotal.Count > 0)
        //                        {
        //                            double dPointsPaid;
        //                            if (double.TryParse(dvFinalTotal[0]["CPSpent"].ToString(), out dPointsPaid))
        //                            {
        //                                if (iNumReqPoints > dPointsPaid)
        //                                    bMeetAllRequirements = false;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return bMeetAllRequirements;
        //}

        //private void CheckAllNodesWithValue(string sValueToCheckFor, bool bValueToSet)
        //{
        //    foreach (TreeNode trMainNodes in tvDisplaySkills.Nodes)     // _tvSkills.Nodes)
        //        SetChildNodes(trMainNodes, sValueToCheckFor, bValueToSet);
        //}

        //private void SetChildNodes(TreeNode NodeToCheck, string sValueToCheckFor, bool bValueToSet)
        //{
        //    if (NodeToCheck.Value == sValueToCheckFor)
        //        NodeToCheck.Checked = bValueToSet;

        //    foreach (TreeNode trChildNode in NodeToCheck.ChildNodes)
        //        SetChildNodes(trChildNode, sValueToCheckFor, bValueToSet);
        //}


        ///// <summary>
        ///// Go through a node and it's children and 'disable' them. Disabling really means add an image and remove the check box.
        ///// </summary>
        ///// <param name="tNode"></param>
        //private void DisableNodeAndChildren(TreeNode tNode)
        //{
        //    tNode.Text = tNode.Text.Replace("black", "grey");
        //    tNode.ImageUrl = "/img/delete.png";
        //    tNode.ShowCheckBox = false;
        //    foreach (TreeNode ChildNode in tNode.ChildNodes)
        //        DisableNodeAndChildren(ChildNode);
        //}


        ///// <summary>
        ///// Go through a node and it's children and 'enable' them. Enabling it removing the image and turning the check box on.
        ///// </summary>
        ///// <param name="tNode"></param>
        //private void EnableNodeAndChildren(TreeNode tNode)
        //{
        //    tNode.Text = tNode.Text.Replace("grey", "black");
        //    tNode.ImageUrl = "";
        //    tNode.ShowCheckBox = true;

        //    foreach (TreeNode tnChild in tNode.ChildNodes)
        //        EnableNodeAndChildren(tnChild);
        //}


        ///// <summary>
        ///// Given a tree node, see if it's value is in the value we are searching for. For each node, go through the child nodes.
        ///// </summary>
        ///// <param name="ValueToSearchFor">Single string value to look for. Value is stored in nodes .Value</param>
        ///// <returns>List of tree nodes with the value searching. It should only return a single node but use a list just in case.</returns>
        //private List<TreeNode> FindNodesByValue(TreeView tView, string ValueToSearchFor)
        //{
        //    List<TreeNode> FoundNodes = new List<TreeNode>();

        //    //			foreach (TreeNode tNode in tvDisplaySkills.Nodes)       // _tvSkills.Nodes)
        //    foreach (TreeNode tNode in tView.Nodes)       // _tvSkills.Nodes)
        //    {
        //        SearchChildren(tNode, FoundNodes, ValueToSearchFor);
        //    }

        //    return FoundNodes;
        //}


        ///// <summary>
        ///// Given a tree node, see if it is the value we are looking for. Have to go through all of the children's node.
        ///// </summary>
        ///// <param name="tNode">The node to check the value and to search the children off.</param>
        ///// <param name="FoundNodes">List of nodes to be returned.</param>
        ///// <param name="ValueToSearchFor">The value that we are going to search for.</param>
        //private void SearchChildren(TreeNode tNode, List<TreeNode> FoundNodes, string ValueToSearchFor)
        //{
        //    if (tNode.Value == ValueToSearchFor)
        //        FoundNodes.Add(tNode);

        //    foreach (TreeNode ChildNode in tNode.ChildNodes)
        //        SearchChildren(ChildNode, FoundNodes, ValueToSearchFor);
        //}


        ///// <summary>
        ///// Given a tree node, see if it's value is in the list we are searching for. For each node, go through the child nodes.
        ///// </summary>
        ///// <param name="lValueList">List of string values we are searching for.</param>
        ///// <returns>List of tree nodes with the value values we have found.</returns>
        //private List<TreeNode> FindNodesByValueList(List<string> lValueList)
        //{
        //    List<TreeNode> FoundNodes = new List<TreeNode>();

        //    foreach (TreeNode tNode in tvDisplaySkills.Nodes)       // _tvSkills.Nodes)
        //    {
        //        SearchChildrenList(tNode, FoundNodes, lValueList);
        //    }

        //    return FoundNodes;
        //}


        ///// <summary>
        ///// Given a tree node, see if it is one of the values we are looking for. Have to go through all of the children's node.
        ///// </summary>
        ///// <param name="tNode">The node to check the value and to search the children off.</param>
        ///// <param name="FoundNodes">List of nodes to be returned.</param>
        ///// <param name="ValueToSearchFor">List of values we are searching for.</param>
        //private void SearchChildrenList(TreeNode tNode, List<TreeNode> FoundNodes, List<string> lValueList)
        //{
        //    // See if the tree value is in the list we are search for. If so, add it to the nodes.
        //    if (tNode.Checked)
        //        if (lValueList.Exists(x => x == tNode.Value))
        //            FoundNodes.Add(tNode);

        //    foreach (TreeNode ChildNode in tNode.ChildNodes)
        //        SearchChildrenList(ChildNode, FoundNodes, lValueList);
        //}

        //protected void CheckExclusions()
        //{
        //    if (Session["NodeExclusions"] == null)
        //        return;

        //    foreach (TreeNode tNode in tvDisplaySkills.Nodes)
        //        EnableNodeAndChildren(tNode);

        //    DataTable dtExclusions;
        //    dtExclusions = Session["NodeExclusions"] as DataTable;

        //    foreach (TreeNode CheckedNode in tvDisplaySkills.CheckedNodes)
        //    {
        //        string sSkill = CheckedNode.Value;
        //        DataView dvPreReq = new DataView(dtExclusions, "PreRequisiteSkillNodeID = " + sSkill, "", DataViewRowState.CurrentRows);
        //        foreach (DataRowView dExclude in dvPreReq)
        //        {
        //            string sExc = dExclude["SkillNodeID"].ToString();
        //            List<TreeNode> ExcludedNodes = FindNodesByValue(tvDisplaySkills, sExc);
        //            foreach (TreeNode tnExc in ExcludedNodes)
        //                DisableNodeAndChildren(tnExc);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Add the Javascript to display an error alert.
        ///// </summary>
        ///// <param name="pvMessage"></param>
        //private void DisplayErrorMessage(string pvMessage)
        //{
        //    lblmodalMessage.Text = pvMessage;
        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
        //}


        ///// <summary>
        ///// Go through all of the checked nodes and make sure you have all the requirements. This is for when somebody unchecks something.
        ///// You then have to go through all of the nodes to make sure that you still have the requirements for everything else.
        ///// </summary>
        //private void CheckSkillRequirementExclusions()
        //{
        //    SortedList sParams = new SortedList();
        //    if (Session["CampaignID"] == null)
        //        Response.Redirect("/default.aspx", true);

        //    // Get all the prereqs/exclusions for the entire campaign so we don't have to keep reloading it.
        //    sParams.Add("@CampaignID", Session["CampaignID"].ToString());
        //    DataSet dsRequire = cUtilities.LoadDataSet("uspGetCampaignNodeRequirements", sParams, "LARPortal", Master.UserName, "CharSkill.aspx_CheckSkillRequirementExclusions");

        //    bool bChangesMade = false;

        //    // Enable everything. Then we will go through and disable nodes as needed.
        //    foreach (TreeNode tBaseNode in tvDisplaySkills.Nodes)       // _tvSkills.Nodes)
        //        EnableNodeAndChildren(tBaseNode);

        //    // As long as we have made a change to the tree, keep rechecking.
        //    do
        //    {
        //        bChangesMade = false;
        //        foreach (TreeNode tNode in tvDisplaySkills.CheckedNodes)        // _tvSkills.CheckedNodes)
        //        {
        //            // Do we have all of the requirements for this node?
        //            if (!CheckNodeRequirement(tNode, dsRequire))
        //            {
        //                // Don't have the requirements so the node has already been unchecked. We need to start over and check all the requirements.
        //                bChangesMade = true;
        //                break;
        //            }
        //        }
        //    } while (bChangesMade);
        //}


        ///// <summary>
        ///// Give a node and the dataset for the campaign, see if the node has all the requirements it needs. If it doesn't uncheck it.
        ///// </summary>
        ///// <param name="tNode">Checked node that needs to be check if all the requirements are met.</param>
        ///// <param name="dsRequire">The dataset with the prereqs/exclusions for the campaign.</param>
        ///// <returns>True means it has everything it needs, False it doesn't have all the requirements.</returns>
        //private bool CheckNodeRequirement(TreeNode tNode, DataSet dsRequire)
        //{
        //    bool bMetRequirements = true;

        //    try
        //    {
        //        // Table 0 is the prereq of a single node.
        //        DataView dvRequiredRows = new DataView(dsRequire.Tables[0], "ExcludeFromPurchase = false and PrerequisiteGroupID is null and SkillNodeID = " + tNode.Value,
        //            "SkillNodeID", DataViewRowState.CurrentRows);

        //        // Go through all of the single requirements and make sure they are all there.
        //        foreach (DataRowView dRow in dvRequiredRows)
        //        {
        //            if (dRow["PrerequisiteSkillNodeID"] != DBNull.Value)
        //            {
        //                int iPreReq;
        //                if (int.TryParse(dRow["PrerequisiteSkillNodeID"].ToString(), out iPreReq))
        //                {
        //                    if (iPreReq != 0)       // May be set to 0 by accident.
        //                    {
        //                        // Get the single pre/req skill from the nodes
        //                        List<TreeNode> tnFoundNode = FindNodesByValue(tvDisplaySkills, iPreReq.ToString());
        //                        if (tnFoundNode.Count == 0)
        //                        {
        //                            bMetRequirements = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        // Check to make sure the node has all the required skills for purchase based on a group of skills.
        //        dvRequiredRows = new DataView(dsRequire.Tables[0], "PrerequisiteGroupID is not null and SkillNodeID = " + tNode.Value, "", DataViewRowState.CurrentRows);

        //        foreach (DataRowView dRow in dvRequiredRows)
        //        {
        //            // Since there is at least one group process it.
        //            int iPreReqGroup;       // What's the number of the group to process.
        //            int iNumReq;            // How many of the requirements do we have to have?
        //            if ((int.TryParse(dRow["PrerequisiteGroupID"].ToString(), out iPreReqGroup)) &&
        //                (int.TryParse(dRow["NumGroupSkillsRequired"].ToString(), out iNumReq)))
        //            {
        //                // Get the items for the specific group. Table1 is the group requirements.
        //                DataView dReqGroup = new DataView(dsRequire.Tables[1], "PrerequisiteGroupID = " + iPreReqGroup.ToString(), "", DataViewRowState.CurrentRows);
        //                if (dReqGroup.Count > 0)
        //                {
        //                    // There were records. Convert the dataview of required nodes to a list of strings - easier to process. The 2nd field is the skill nodes.
        //                    List<string> ReqSkillNodes = dReqGroup.ToTable().AsEnumerable().Select(x => x[1].ToString()).ToList();
        //                    // If we find the value we are looking for - remove it.
        //                    ReqSkillNodes.Remove(tNode.Value);
        //                    List<TreeNode> FoundNode = FindNodesByValueList(ReqSkillNodes);
        //                    if (FoundNode.Count < iNumReq)
        //                        bMetRequirements = false;
        //                }
        //            }
        //        }

        //        // Only need to check exclusions if the all of the requirements have been met.
        //        if (bMetRequirements)
        //        {
        //            // Check exclusions. Disable all nodes sthat are excluded because of this.
        //            dvRequiredRows = new DataView(dsRequire.Tables[0], "ExcludeFromPurchase = true and SkillNodeID = " + tNode.Value, "SkillNodeID", DataViewRowState.CurrentRows);

        //            foreach (DataRowView dRow in dvRequiredRows)
        //            {
        //                if (dRow["PrerequisiteSkillNodeID"] != DBNull.Value)
        //                {
        //                    int iPreReq;
        //                    if (int.TryParse(dRow["PrerequisiteSkillNodeID"].ToString(), out iPreReq))
        //                    {
        //                        if (iPreReq.ToString() != tNode.Value)
        //                        {
        //                            // Get the node that has the value of the prereq and disable it and all of it's children.
        //                            List<TreeNode> tnFoundNode = FindNodesByValue(tvDisplaySkills, iPreReq.ToString());
        //                            foreach (TreeNode tnNodesToExclude in tnFoundNode)
        //                            {
        //                                DisableNodeAndChildren(tnNodesToExclude);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        if (!bMetRequirements)
        //            tNode.Checked = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        string l = ex.Message;
        //    }

        //    return bMetRequirements;
        //}

        //protected void RemoveExclusions()
        //{
        //    List<string> NodeList = new List<string>();

        //    foreach (TreeNode tNode in tvDisplaySkills.Nodes)
        //    {
        //        if (tNode.ImageUrl == "/img/delete.png")
        //        {
        //            NodeList.Add(tNode.ValuePath);
        //        }
        //        RemoveNodes(tNode, NodeList);
        //    }
        //    foreach (string t in NodeList)
        //    {
        //        TreeNode FoundID = tvDisplaySkills.FindNode(t);
        //        if (FoundID != null)
        //            if (FoundID.Depth == 0)
        //                tvDisplaySkills.Nodes.Remove(FoundID);
        //            else
        //                FoundID.Parent.ChildNodes.Remove(FoundID);
        //    }
        //}

        //protected void RemoveNodes(TreeNode tNode, List<string> NodeList)
        //{
        //    string t = tNode.Text;
        //    if (tNode.ImageUrl == "/img/delete.png")
        //    {
        //        NodeList.Add(tNode.ValuePath);
        //    }
        //    else
        //    {
        //        foreach (TreeNode tChild in tNode.ChildNodes)
        //            RemoveNodes(tChild, NodeList);
        //    }
        //}

        //public void SaveNodeState(TreeView tvMemorySkills)
        //{
        //    foreach (TreeNode tMainNode in tvDisplaySkills.Nodes)
        //    {
        //        TreeNode tCopy = tvMemorySkills.FindNode(tMainNode.ValuePath);
        //        if (tCopy != null)
        //        {
        //            tCopy.Expanded = tMainNode.Expanded;
        //            foreach (TreeNode tChild in tMainNode.ChildNodes)
        //            {
        //                ProcessChildren(tChild, tvMemorySkills);
        //            }
        //        }
        //    }
        //}

        //public void ProcessChildren(TreeNode tChild, TreeView tvMemorySkills)
        //{
        //    TreeNode tCopy = tvMemorySkills.FindNode(tChild.ValuePath);
        //    if (tCopy != null)
        //    {
        //        tCopy.Expanded = tChild.Expanded;
        //        foreach (TreeNode tNextNode in tChild.ChildNodes)
        //            ProcessChildren(tNextNode, tvMemorySkills);
        //    }
        //}

        //public void CopyDisplayTreeNodes(TreeView InMemory, TreeView DisplayTree)
        //{
        //    DisplayTree.Nodes.Clear();

        //    bool ReadOnly = false;

        //    if (Session["CharSkillReadOnly"] != null)
        //        if (Session["CharSkillReadOnly"].ToString() == "Y")
        //            ReadOnly = true;

        //    bool DisplayExcluded = false;
        //    if (Session["SkillShowExclusions"] != null)
        //        if (Session["SkillShowExclusions"].ToString() == "Y")
        //            DisplayExcluded = true;

        //    TreeNode tnDisplay = new TreeNode();

        //    foreach (TreeNode tnInMemory in InMemory.Nodes)
        //    {
        //        // If the node has an image of DELETE and don't want to show exclusions, don't copy the node.
        //        if (!DisplayExcluded)
        //            if (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG"))
        //                continue;

        //        if ((ReadOnly) &&           // JBradshaw 3/5/2017 If readonly (NPC) only display checked values.
        //            (!tnInMemory.Checked))
        //            continue;

        //        tnDisplay = new TreeNode(tnInMemory.Text, tnInMemory.Value);
        //        tnDisplay.Checked = tnInMemory.Checked;
        //        tnDisplay.Expanded = tnInMemory.Expanded;
        //        tnDisplay.ShowCheckBox = tnInMemory.ShowCheckBox;
        //        tnDisplay.ImageUrl = tnInMemory.ImageUrl;

        //        if (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG"))
        //            tnDisplay.ShowCheckBox = false;

        //        if ((ReadOnly) || (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG")))
        //            tnDisplay.ShowCheckBox = false;
        //        else
        //            tnDisplay.ShowCheckBox = true;

        //        CopyDisplayChildren(tnInMemory, tnDisplay, DisplayExcluded, ReadOnly);
        //        DisplayTree.Nodes.Add(tnDisplay);
        //    }
        //    if (ReadOnly)
        //        DisplayTree.ShowCheckBoxes = TreeNodeTypes.None;
        //    if (DisplayTree.Nodes.Count == 0)
        //        lblMessage.Text += " This character has no skills purchased.";
        //}

        //public void CopyDisplayChildren(TreeNode InMemory, TreeNode Display, bool DisplayExcluded, bool ReadOnly)
        //{
        //    foreach (TreeNode tnInMemory in InMemory.ChildNodes)
        //    {
        //        // If the node has an image of DELETE and don't want to show exclusions, don't copy the node.
        //        if (!DisplayExcluded)
        //            if (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG"))
        //                continue;

        //        TreeNode tnDisplay = new TreeNode();

        //        if ((ReadOnly) &&
        //            (!tnInMemory.Checked))
        //            continue;

        //        tnDisplay = new TreeNode(tnInMemory.Text, tnInMemory.Value);
        //        tnDisplay.Checked = tnInMemory.Checked;
        //        tnDisplay.Expanded = tnInMemory.Expanded;
        //        tnDisplay.ShowCheckBox = tnInMemory.ShowCheckBox;
        //        tnDisplay.ImageUrl = tnInMemory.ImageUrl;

        //        if ((ReadOnly) || (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG")))
        //            tnDisplay.ShowCheckBox = false;
        //        else
        //            tnDisplay.ShowCheckBox = true;

        //        Display.ChildNodes.Add(tnDisplay);
        //        CopyDisplayChildren(tnInMemory, tnDisplay, DisplayExcluded, ReadOnly);
        //    }
        //}

        //protected void cbxShowExclusions_CheckedChanged(object sender, EventArgs e)
        //{
        //    oCharSelect.LoadInfo();

        //    if (cbxShowExclusions.Checked)
        //        RebuildTreeView();
        //    else
        //    {
        //        RemoveExclusions();
        //    }
        //}
        #endregion

        #region Display2
        //protected void MasterPage_CampaignChanged(object sender, EventArgs e)
        //{
        //    MethodBase lmth = MethodBase.GetCurrentMethod();
        //    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

        //    LogWriter oLog = new LogWriter();
        //    oLog.AddLogMessage("Skills - Campaign Changed", Master.UserName, lsRoutineName, "", Session.SessionID);

        //    oCharSelect.Reset();
        //    _Reload = true;
        //    Classes.cUser user = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
        //    if (user.LastLoggedInCharacter == -1)
        //        Response.Redirect("/default.aspx");

        //    // Added setting 
        //    Classes.cCampaignBase cCampaign = new Classes.cCampaignBase(Master.CampaignID, Master.UserName, Master.UserID);
        //    hidAutoBuyParentSkills.Value = "Y";
        //    if (cCampaign.AutoBuyParentSkills.HasValue)
        //        if (!cCampaign.AutoBuyParentSkills.Value)
        //            hidAutoBuyParentSkills.Value = "N";
        //}


        //private void RebuildTreeView()
        //{
        //    _dtCampaignSkills = Session["SkillNodes"] as DataTable;
        //    DataTable dtCharacterSkillsCost = Session["CharacterSkillCost"] as DataTable;

        //    TreeView OrigTree = new TreeView();
        //    CopyTreeNodes(tvDisplaySkills, OrigTree);
        //    List<TreeNode> FlatTreeNodes = FlattenTree(tvDisplaySkills);

        //    tvDisplaySkills.Nodes.Clear();

        //    DataView dvTopNodes = new DataView(_dtCampaignSkills, "ParentSkillNodeID is null", "DisplayOrder", DataViewRowState.CurrentRows);
        //    foreach (DataRowView dvRow in dvTopNodes)
        //    {
        //        TreeNode NewNode = new TreeNode();
        //        NewNode.ShowCheckBox = true;

        //        NewNode.Text = FormatDescString(dvRow);
        //        NewNode.SelectAction = TreeNodeSelectAction.None;

        //        int iNodeID;
        //        if (int.TryParse(dvRow["CampaignSkillNodeID"].ToString(), out iNodeID))
        //        {
        //            NewNode.Expanded = false;
        //            NewNode.Value = iNodeID.ToString();
        //            if (dvRow["CharacterHasSkill"].ToString() == "1")
        //                NewNode.Checked = true;
        //            NewNode.SelectAction = TreeNodeSelectAction.None;
        //            List<TreeNode> OrigNode = FindNodesByValue(OrigTree, iNodeID.ToString());

        //            //					List<TreeNode> OrigNode = FindNodesByValue(tvDisplaySkills, iNodeID.ToString());
        //            if (OrigNode.Count > 0)
        //                NewNode.Expanded = OrigNode[0].Expanded;
        //            PopulateTreeView(iNodeID, NewNode);
        //            tvDisplaySkills.Nodes.Add(NewNode);
        //        }
        //    }
        //    CheckExclusions();
        //    if (!cbxShowExclusions.Checked)
        //        RemoveExclusions();

        //    var ExpandedNodes = from r in FlatTreeNodes where r.Expanded == true select r.Value;
        //    foreach (string ExpNode in ExpandedNodes)
        //    {
        //        List<TreeNode> tnNode = FindNodesByValue(tvDisplaySkills, ExpNode);
        //        if (tnNode.Count > 0)
        //            tnNode[0].Expanded = true;
        //    }
        //}

        //private List<TreeNode> FlattenTree(TreeView DataTree)
        //{
        //    List<TreeNode> FlatNodes = new List<TreeNode>();

        //    foreach (TreeNode tNode in tvDisplaySkills.Nodes)
        //    {
        //        GetAllNodes(FlatNodes, tNode);
        //        FlatNodes.Add(tNode);
        //    }
        //    return FlatNodes;
        //}

        //private void GetAllNodes(List<TreeNode> NodeList, TreeNode SourceNode)
        //{
        //    foreach (TreeNode tnChild in SourceNode.ChildNodes)
        //    {
        //        GetAllNodes(NodeList, tnChild);
        //        NodeList.Add(tnChild);
        //    }
        //}

        //public string GetValue(string it)
        //{
        //    return "Got it!" + it;
        //}

        //protected void btnSaveTextChanges_Click(object sender, EventArgs e)
        //{
        //    _dtCampaignSkills = Session["SkillNodes"] as DataTable;
        //    string sCampaignSkillNodeID = hidCampaignSkillNodeID.Value;
        //    DataView dv = new DataView(_dtCampaignSkills, "CampaignSkillNodeID = " + sCampaignSkillNodeID, "", DataViewRowState.CurrentRows);
        //    if (dv.Count > 0)
        //        dv[0]["AddInfoValue"] = tbNewValue.Text;
        //    Session["SkillNodes"] = _dtCampaignSkills;
        //    RebuildTreeView();
        //}

        //protected void btnSaveDropDownChanges_Click(object sender, EventArgs e)
        //{
        //    _dtCampaignSkills = Session["SkillNodes"] as DataTable;
        //    string sCampaignSkillNodeID = hidCampaignSkillNodeID.Value;
        //    DataView dv = new DataView(_dtCampaignSkills, "CampaignSkillNodeID = " + sCampaignSkillNodeID, "", DataViewRowState.CurrentRows);
        //    if (dv.Count > 0)
        //        dv[0]["AddInfoValue"] = hidNewDropDownValue.Value;
        //    Session["SkillNodes"] = _dtCampaignSkills;
        //    RebuildTreeView();
        //}






        //protected bool EnoughPointsToSave()
        //{
        //    DataTable dtPointsSpent = new DataTable();
        //    dtPointsSpent.Columns.Add(new DataColumn("PoolID", typeof(int)));
        //    dtPointsSpent.Columns.Add(new DataColumn("PoolName", typeof(string)));
        //    dtPointsSpent.Columns.Add(new DataColumn("CPSpent", typeof(double)));
        //    dtPointsSpent.Columns.Add(new DataColumn("TotalCP", typeof(double)));
        //    dtPointsSpent.Columns.Add(new DataColumn("AvailablePoints", typeof(double)));

        //    DataTable dtAllSkills = Session["SkillNodes"] as DataTable;
        //    DataTable dtCampaignSkillsCost = Session["CampaignSkillNodeCost"] as DataTable;
        //    DataTable dtCharacterSkillCost = Session["CharacterSkillCost"] as DataTable;

        //    int iPool = 0;

        //    foreach (cSkillPool c in oCharSelect.CharacterInfo.SkillPools)
        //    {
        //        DataRow dRow = dtPointsSpent.NewRow();
        //        dRow["PoolID"] = c.PoolID;
        //        dRow["PoolName"] = c.PoolDescription;
        //        dRow["CPSpent"] = 0;
        //        dRow["TotalCP"] = c.TotalPoints;
        //        dRow["AvailablePoints"] = 0;

        //        dtPointsSpent.Rows.Add(dRow);
        //    }

        //    foreach (TreeNode SkillNode in tvDisplaySkills.CheckedNodes)
        //    {
        //        int iSkillID;
        //        if (int.TryParse(SkillNode.Value, out iSkillID))
        //        {
        //            double SkillCost = 0.0;

        //            string sNodeText = SkillNode.Text;
        //            DataRow[] drSkillRow = dtAllSkills.Select("CampaignSkillNodeID = " + iSkillID.ToString());
        //            if (drSkillRow.Length > 0)
        //            {
        //                // If it's in the CharacterSkillCost it means that have the skill.
        //                DataView dvSkillCost = new DataView(dtCharacterSkillCost, "CampaignSkillNodeID = " + iSkillID.ToString(), "", DataViewRowState.CurrentRows);
        //                if (dvSkillCost.Count > 0)
        //                {
        //                    foreach (DataRowView drSkillCost in dvSkillCost)
        //                    {
        //                        if ((int.TryParse(drSkillCost["CampaignSkillPoolID"].ToString(), out iPool)) &&
        //                            (double.TryParse(drSkillCost["CPCostPaid"].ToString(), out SkillCost)))
        //                        {
        //                            if (iPool != 0)
        //                            {
        //                                DataRow[] dPool = dtPointsSpent.Select("PoolID = " + iPool.ToString());
        //                                if (dPool.Length > 0)
        //                                {
        //                                    dPool[0]["CPSpent"] = (double)(dPool[0]["CPSpent"]) + SkillCost;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    bool bEnoughPointsToSave = true;

        //    foreach (DataRow dCostRow in dtPointsSpent.Rows)
        //    {
        //        double CPSpent;
        //        double TotalCPForPool;
        //        if ((double.TryParse(dCostRow["TotalCP"].ToString(), out TotalCPForPool)) &&
        //             (double.TryParse(dCostRow["CPSpent"].ToString(), out CPSpent)))
        //        {
        //            dCostRow["AvailablePoints"] = TotalCPForPool - CPSpent;
        //            if (CPSpent > TotalCPForPool)
        //                bEnoughPointsToSave = false;
        //        }
        //    }

        //    gvPoolTotals.DataSource = dtPointsSpent;
        //    gvPoolTotals.DataBind();

        //    return bEnoughPointsToSave;
        //}

        //protected void gvPoolTotals_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        double dAvail;
        //        if (double.TryParse(e.Row.Cells[3].Text, out dAvail))
        //        {
        //            if (dAvail < 0)
        //            {
        //                e.Row.CssClass = "text-danger";
        //            }
        //        }
        //    }
        //}

        #endregion

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
            sParams.Add("@CampaignSkillNodeID", hidNodeID.Value);
            sParams.Add("@RegistrationID", hidRegID.Value);
            sParams.Add("@RequestText", CKERequestText.Text);
            if (e.CommandName == "SUBMIT")
                sParams.Add("@RequestStatus", "Submitted");
            else
                sParams.Add("@RequestStatus", "Saved");
            cUtilities.PerformNonQuery("uspInsUpdISkillRequestTable", sParams, "LARPortal", Master.UserName);
            Response.Redirect("Requests.aspx", true);
        }
    }
}
