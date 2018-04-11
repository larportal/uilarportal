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

namespace LarpPortal.Character
{
    public partial class CharSkills : System.Web.UI.Page
    {
        protected DataTable _dtCampaignSkills = new DataTable();
        private bool _Reload = false;

		protected void Page_PreInit(object sender, EventArgs e)
		{
			// Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
			Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
		}

		protected void Page_Load(object sender, EventArgs e)
        {
			oCharSelect.CharacterChanged += oCharSelect_CharacterChanged;
			if (!IsPostBack)
            {
                tvDisplaySkills.Attributes.Add("onclick", "postBackByObject()");
            }
            btnCloseMessage.Attributes.Add("data-dismiss", "modal");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if ((!IsPostBack) || (_Reload))
            {
                oCharSelect.LoadInfo();

                if (oCharSelect.CharacterID.HasValue)
                {
                    double TotalCP = 0.0;
                    lblMessage.Text = "";
                    TotalCP = oCharSelect.CharacterInfo.TotalCP;
                    Session["TotalCP"] = TotalCP;

                    DataTable dtCharSkills = Classes.cUtilities.CreateDataTable(oCharSelect.CharacterInfo.CharacterSkills);

                    if (oCharSelect.CharacterInfo.CharacterSkills.Count > 0)
                    {
                        List<int> SkillList = new List<int>();
                        foreach (Classes.cCharacterSkill dSkill in oCharSelect.CharacterInfo.CharacterSkills)
                        {
                            SkillList.Add(dSkill.CampaignSkillNodeID);
                        }
                        Session["SkillList"] = SkillList;
                    }

                    DataSet dsSkillSets = new DataSet();
                    SortedList sParam = new SortedList();

                    // Request # 1293 Single Character Rebuild   J.Bradshaw 7/10/2016
                    Classes.cCampaignBase cCampaign = new Classes.cCampaignBase(oCharSelect.CharacterInfo.CampaignID, Master.UserName, Master.UserID);
                    if ((cCampaign.AllowCharacterRebuild) ||
                        (oCharSelect.CharacterInfo.AllowCharacterRebuild))
                    {
                        hidAllowCharacterRebuild.Value = "1";
                        lblSkillsLocked.Visible = false;
                        if (oCharSelect.CharacterInfo.AllowCharacterRebuild)
                        {
                            if (lblMessage.Text.Length > 0)
                                lblMessage.Text += ", ";
                            lblMessage.Text += "You can rebuild this character until " + oCharSelect.CharacterInfo.AllowCharacterRebuildToDate.Value.ToShortDateString();
                        }
                    }
                    else
                    {
                        hidAllowCharacterRebuild.Value = "0";
                        lblSkillsLocked.Visible = true;
                    }

                    sParam.Add("@CampaignID", oCharSelect.CharacterInfo.CampaignID);
                    sParam.Add("@CharacterID", oCharSelect.CharacterID.Value);
                    dsSkillSets = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillsWithNodes", sParam, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignSkillsWithNodes");

                    _dtCampaignSkills = dsSkillSets.Tables[0];
                    Session["SkillNodes"] = _dtCampaignSkills;
                    Session["NodePrerequisites"] = dsSkillSets.Tables[1];
                    Session["SkillTypes"] = dsSkillSets.Tables[2];
                    Session["NodeExclusions"] = dsSkillSets.Tables[3];

                    tvDisplaySkills.Nodes.Clear();

                    DataView dvTopNodes = new DataView(_dtCampaignSkills, "ParentSkillNodeID is null", "DisplayOrder", DataViewRowState.CurrentRows);
                    foreach (DataRowView dvRow in dvTopNodes)
                    {
                        TreeNode NewNode = new TreeNode();
                        NewNode.ShowCheckBox = true;

                        NewNode.Text = FormatDescString(dvRow);
                        NewNode.SelectAction = TreeNodeSelectAction.None;

                        int iNodeID;
                        if (int.TryParse(dvRow["CampaignSkillNodeID"].ToString(), out iNodeID))
                        {
                            NewNode.Expanded = false;
                            NewNode.Value = iNodeID.ToString();
                            if (dvRow["CharacterHasSkill"].ToString() == "1")
                            {
                                NewNode.Checked = true;
                            }
                            NewNode.SelectAction = TreeNodeSelectAction.None;
                            PopulateTreeView(iNodeID, NewNode);
                            tvDisplaySkills.Nodes.Add(NewNode);
                        }
                    }
                    CheckExclusions();
                }
                ListSkills();
            }
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
                    childNode.ShowCheckBox = true;
                    childNode.Text = FormatDescString(dr);

                    childNode.Expanded = false;
                    childNode.Value = iNodeID.ToString();
                    if (dr["CharacterHasSkill"].ToString() == "1")
                    {
                        childNode.Checked = true;
                        parentNode.Expand();
                    }
                    childNode.SelectAction = TreeNodeSelectAction.None;
                    parentNode.ChildNodes.Add(childNode);
                    PopulateTreeView(iNodeID, childNode);
                }
            }
        }


        /// <summary>
        /// Tree node changed event. Skill being selected/deselected. Note - this happens AFTER the person clicks. Which means the node is already checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvSkills_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            //if (Session["tvSkills"] == null)
            //    return;

            //_tvSkills = Session["tvSkills"] as TreeView;

            //SaveNodeState(_tvSkills);

            oCharSelect.LoadInfo();

            TreeView OrigTree = new TreeView();
            CopyTreeNodes(tvDisplaySkills, OrigTree);       // _tvSkills);

            if (e.Node.Checked)
            {
                // Save tree nodes so if they don't have enough points to buy the skill, we have the old one.
                TreeView OrigTreeView = new TreeView();
                //CopyTreeNodes(tvDisplaySkills, Orig
                //CopyTreeNodes(_tvSkills, OrigTreeView);

                TreeNode FoundNode = tvDisplaySkills.FindNode(e.Node.ValuePath);
                MarkParentNodes(FoundNode);

                //                List<cSkillPool> oSkillPools = Session["CharacterSkillPools"] as List<cSkillPool>;

                DataTable dtPointsSpent = new DataTable();
                dtPointsSpent.Columns.Add(new DataColumn("PoolID", typeof(int)));
                dtPointsSpent.Columns.Add(new DataColumn("PoolName", typeof(string)));
                dtPointsSpent.Columns.Add(new DataColumn("CPSpent", typeof(double)));
                dtPointsSpent.Columns.Add(new DataColumn("TotalCP", typeof(double)));

                // Go through all of the pools so we have the list on the screen.
                foreach (cSkillPool cSkill in oCharSelect.CharacterInfo.SkillPools)                      //oSkillPools)
                {
                    DataRow dNewRow = dtPointsSpent.NewRow();
                    dNewRow["PoolID"] = cSkill.PoolID;
                    dNewRow["PoolName"] = cSkill.PoolDescription;
                    dNewRow["TotalCP"] = cSkill.TotalPoints;
                    dNewRow["CPSpent"] = 0.0;

                    dtPointsSpent.Rows.Add(dNewRow);
                }

                DataTable dtAllSkills = Session["SkillNodes"] as DataTable;

                int iPool = 0;

                foreach (TreeNode SkillNode in tvDisplaySkills.CheckedNodes)            // _tvSkills.CheckedNodes)
                {
                    int iSkillID;
                    if (int.TryParse(SkillNode.Value, out iSkillID))
                    {
                        double SkillCost = 0.0;

                        DataRow[] drSkillRow = dtAllSkills.Select("CampaignSkillNodeID = " + iSkillID.ToString());
                        if (drSkillRow.Length > 0)
                        {
                            int.TryParse(drSkillRow[0]["CampaignSkillPoolID"].ToString(), out iPool);
                            {
                                if (drSkillRow[0]["CharacterHasSkill"].ToString() == "1")
                                    double.TryParse(drSkillRow[0]["CPCostPaid"].ToString(), out SkillCost);
                                else
                                    double.TryParse(drSkillRow[0]["SkillCPCost"].ToString(), out SkillCost);
                            }
                        }

                        if (iPool != 0)
                        {
                            DataRow[] dPool = dtPointsSpent.Select("PoolID = " + iPool.ToString());
                            if (dPool.Length > 0)
                            {
                                dPool[0]["CPSpent"] = (double)(dPool[0]["CPSpent"]) + SkillCost;
                            }
                        }
                    }
                }

                bool bSpentTooMuch = false;

                foreach (DataRow dCostRow in dtPointsSpent.Rows)
                {
                    double CPSpent;
                    double TotalCPForPool;
                    if ((double.TryParse(dCostRow["TotalCP"].ToString(), out TotalCPForPool)) &&
                         (double.TryParse(dCostRow["CPSpent"].ToString(), out CPSpent)))
                    {
                        if (CPSpent > TotalCPForPool)
                            bSpentTooMuch = true;
                    }
                }

                if (bSpentTooMuch)
                {
                    //_tvSkills.Nodes.Clear();
                    //TreeView OrigTree = Session["CurrentSkillTree"] as TreeView;
                    //CopyTreeNodes(OrigTree, _tvSkills);
                    e.Node.Checked = false;
                    CopyTreeNodes(OrigTree, tvDisplaySkills);
                    DisplayErrorMessage("You do not have enough points to buy that.");
                    return;
                }
                else
                {
                    if (!CheckForRequirements(e.Node.Value))
                    {
                        //_tvSkills.Nodes.Clear();
                        //TreeView OrigTree = Session["CurrentSkillTree"] as TreeView;
                        //CopyTreeNodes(OrigTree, _tvSkills);
                        e.Node.Checked = false;
                        CopyTreeNodes(OrigTree, tvDisplaySkills);
                        DisplayErrorMessage("You do not have all the requirements to purchase that item.");
                        return;
                    }
                    else
                    {
                        CheckAllNodesWithValue(e.Node.Value, true);
                    }
                }
                List<TreeNode> FoundNodes = FindNodesByValue(e.Node.Value);
                foreach (TreeNode t in FoundNodes)
                {
                    t.ShowCheckBox = false;
                    EnableNodeAndChildren(t);
                }
            }
            else
            {
                TreeNode tnCopy = tvDisplaySkills.FindNode(e.Node.ValuePath);       // _tvSkills.FindNode(e.Node.ValuePath);

                // Check to see if we should not allow them to sell it back.
                //if (ViewState["SkillList"] != null)
                //{
                int iSkillID;
                if (int.TryParse(tnCopy.Value, out iSkillID))
                {
                    ////                        List<int> SkillList = oCharSelect.CharacterInfo.CharacterSkills;        // ViewState["SkillList"] as List<int>;
                    List<cCharacterSkill> cSkillList = oCharSelect.CharacterInfo.CharacterSkills.Where(x => x.CampaignSkillNodeID == iSkillID).ToList();

                    //                        //if (SkillList.Contains(iSkillID))
                    //                        //{
                    if (cSkillList.Count > 0)
                    {
                        if (hidAllowCharacterRebuild.Value == "0")
                        {
                            tnCopy.Checked = true;
                            DisplayErrorMessage("This campaign is not allowing skills to be rebuilt at this time.  Once a skill is selected and saved, it cannot be changed.");
                            return;
                        }
                    }
                }
                //               }

                CheckSkillRequirementExclusions();

                DeselectChildNodes(tnCopy);
                CheckAllNodesWithValue(tnCopy.Value, false);

                List<TreeNode> FoundNodes = FindNodesByValue(tnCopy.Value);
                foreach (TreeNode t in FoundNodes)
                {
                    t.Text = t.Text.Replace("grey", "black");
                    t.ImageUrl = "";
                    t.ShowCheckBox = true;
                    EnableNodeAndChildren(t);
                }
            }

            ListSkills();
//            Session["CurrentSkillTree"] = _tvSkills;

            lblMessage.Text = "Skills Changed";
            lblMessage.ForeColor = Color.Red;

            //tvDisplaySkills.Nodes.Clear();
            //CopyDisplayTreeNodes(_tvSkills, tvDisplaySkills);
            //Session["tvSkills"] = _tvSkills;
            //}
        }


        protected void ListSkills()
        {
            DataTable dtAllSkills = Session["SkillNodes"] as DataTable;

            double TotalSpent = 0.0;

            DataTable dtSkillCosts = new DataTable();
            dtSkillCosts.Columns.Add(new DataColumn("PoolID", typeof(int)));
            dtSkillCosts.Columns.Add(new DataColumn("Skill", typeof(string)));
            dtSkillCosts.Columns.Add(new DataColumn("Cost", typeof(double)));
            dtSkillCosts.Columns.Add(new DataColumn("SortOrder", typeof(int)));
            dtSkillCosts.Columns.Add(new DataColumn("SkillID", typeof(int)));

            double TotalCP = 0.0;
            double.TryParse(Session["TotalCP"].ToString(), out TotalCP);

            foreach (TreeNode SkillNode in tvDisplaySkills.CheckedNodes)        // _tvSkills.CheckedNodes)
            {
                int iSkillID;
                if (int.TryParse(SkillNode.Value, out iSkillID))
                {
                    double SkillCost = 0.0;
                    double DisplayOrder = 10;

                    DataRow[] drPrev = dtSkillCosts.Select("SkillID = " + iSkillID.ToString());
                    if (drPrev.Length == 0)
                    {
                        string sSkillName = "";
                        DataRow[] drCharSkills = dtAllSkills.Select("CampaignSkillNodeID = " + iSkillID.ToString());
                        if (drCharSkills.Length > 0)
                        {
                            if (drCharSkills[0]["CharacterHasSkill"].ToString() == "1")
                                double.TryParse(drCharSkills[0]["CPCostPaid"].ToString(), out SkillCost);
                            else
                                double.TryParse(drCharSkills[0]["SkillCPCost"].ToString(), out SkillCost);

                            sSkillName = drCharSkills[0]["SkillName"].ToString();
                            double.TryParse(drCharSkills[0]["DisplayOrder"].ToString(), out DisplayOrder);
                        }
                        DataRow dNewRow = dtSkillCosts.NewRow();
                        dNewRow["PoolID"] = drCharSkills[0]["CampaignSkillPoolID"];
                        dNewRow["Skill"] = sSkillName;
                        dNewRow["Cost"] = SkillCost;
                        dNewRow["SortOrder"] = DisplayOrder;
                        dNewRow["SkillID"] = iSkillID;
                        dtSkillCosts.Rows.Add(dNewRow);
                    }
                }
            }

            Session["SelectedSkills"] = dtSkillCosts;

            //List<cSkillPool> oCampaignPools = (List<cSkillPool>)Session["CharacterSkillPools"];
            cSkillPool DefaultPool = oCharSelect.CharacterInfo.SkillPools.Find(x => x.DefaultPool == true);         // oCampaignPools.Find(x => x.DefaultPool == true);

            DataRow[] dSkillRow = dtSkillCosts.Select("PoolID = " + DefaultPool.PoolID);

            object oResult;
            if (DefaultPool != null)
            {
                string sFilter = "PoolID = " + DefaultPool.PoolID.ToString();
                oResult = dtSkillCosts.Compute("sum(Cost)", sFilter);
                double.TryParse(oResult.ToString(), out TotalSpent);
                TotalCP = DefaultPool.TotalPoints;
            }

            DataTable dtDisplay = new DataTable();
            dtDisplay.Columns.Add(new DataColumn("Skill", typeof(string)));
            dtDisplay.Columns.Add(new DataColumn("Cost", typeof(double)));
            dtDisplay.Columns.Add(new DataColumn("MainSort", typeof(int)));
            dtDisplay.Columns.Add(new DataColumn("SortOrder", typeof(int)));
            dtDisplay.Columns.Add(new DataColumn("Color", typeof(string)));

            DataRow NewRow = dtDisplay.NewRow();
            //if (hidCharacterType.Value == "1")
            //{
            NewRow["Skill"] = "Total CP";
            NewRow["Cost"] = TotalCP;
            NewRow["MainSort"] = 1;
            NewRow["SortOrder"] = 1;
            NewRow["Color"] = DefaultPool.PoolDisplayColor;
            dtDisplay.Rows.Add(NewRow);
            //}

            NewRow = dtDisplay.NewRow();
            NewRow["Skill"] = "Total Spent";
            NewRow["Cost"] = TotalSpent;
            NewRow["MainSort"] = 1;
            NewRow["SortOrder"] = 2;
            NewRow["Color"] = DefaultPool.PoolDisplayColor;
            dtDisplay.Rows.Add(NewRow);

            //if (hidCharacterType.Value == "1")
            //{
            NewRow = dtDisplay.NewRow();
            NewRow["Skill"] = "Total Avail";
            NewRow["Cost"] = (TotalCP - TotalSpent);
            NewRow["MainSort"] = 1;
            NewRow["SortOrder"] = 3;
            NewRow["Color"] = DefaultPool.PoolDisplayColor;
            dtDisplay.Rows.Add(NewRow);
            //}

            foreach (DataRowView dItem in new DataView(dtSkillCosts, "PoolID = " + DefaultPool.PoolID.ToString(), "SortOrder", DataViewRowState.CurrentRows))
            {
                NewRow = dtDisplay.NewRow();
                NewRow["Skill"] = dItem["Skill"].ToString();
                NewRow["MainSort"] = 1;
                NewRow["SortOrder"] = 10;
                NewRow["Cost"] = dItem["Cost"];
                NewRow["Color"] = DefaultPool.PoolDisplayColor;
                dtDisplay.Rows.Add(NewRow);
            }

            int PoolOrderOffset = 10;

            foreach (cSkillPool PoolItem in oCharSelect.CharacterInfo.SkillPools.OrderBy(x => x.PoolDescription))
            {
                PoolOrderOffset++;

                if (PoolItem.DefaultPool)       // We've already taken care of this before.
                    continue;

                //if (PoolItem.TotalPoints > 0)
                //{
                foreach (DataRowView dItem in new DataView(dtSkillCosts, "PoolID = " + PoolItem.PoolID.ToString(), "SortOrder", DataViewRowState.CurrentRows))
                {
                    NewRow = dtDisplay.NewRow();
                    NewRow["Skill"] = dItem["Skill"].ToString();
                    NewRow["MainSort"] = PoolOrderOffset;
                    NewRow["SortOrder"] = 10;
                    NewRow["Cost"] = dItem["Cost"];
                    NewRow["Color"] = PoolItem.PoolDisplayColor;
                    dtDisplay.Rows.Add(NewRow);
                }

                string sFilter = "PoolID = " + PoolItem.PoolID.ToString();
                oResult = dtSkillCosts.Compute("sum(Cost)", sFilter);
                double.TryParse(oResult.ToString(), out TotalSpent);
                TotalCP = PoolItem.TotalPoints;

                NewRow = dtDisplay.NewRow();
                NewRow["Skill"] = PoolItem.PoolDescription;
                NewRow["MainSort"] = PoolOrderOffset;
                NewRow["SortOrder"] = 0;
                NewRow["Color"] = PoolItem.PoolDisplayColor;
                dtDisplay.Rows.Add(NewRow);

                NewRow = dtDisplay.NewRow();
                //if (hidCharacterType.Value == "1")
                //{
                NewRow["Skill"] = "Total CP";
                NewRow["Cost"] = PoolItem.TotalPoints;
                NewRow["MainSort"] = PoolOrderOffset;
                NewRow["SortOrder"] = 1;
                NewRow["Color"] = PoolItem.PoolDisplayColor;
                dtDisplay.Rows.Add(NewRow);
                //}

                NewRow = dtDisplay.NewRow();
                NewRow["Skill"] = "Total Spent";
                NewRow["Cost"] = TotalSpent;
                NewRow["MainSort"] = PoolOrderOffset;
                NewRow["SortOrder"] = 2;
                NewRow["Color"] = PoolItem.PoolDisplayColor;
                dtDisplay.Rows.Add(NewRow);

                //if (hidCharacterType.Value == "1")
                //{
                NewRow = dtDisplay.NewRow();
                NewRow["Skill"] = "Total Avail";
                NewRow["Cost"] = (TotalCP - TotalSpent);
                NewRow["MainSort"] = PoolOrderOffset;
                NewRow["SortOrder"] = 3;
                NewRow["Color"] = PoolItem.PoolDisplayColor;
                dtDisplay.Rows.Add(NewRow);
                //}

                NewRow = dtDisplay.NewRow();
                NewRow["Skill"] = "";
                NewRow["MainSort"] = PoolOrderOffset;
                NewRow["SortOrder"] = -1;
                NewRow["Color"] = PoolItem.PoolDisplayColor;
                dtDisplay.Rows.Add(NewRow);
                //}
            }
            DataView dvSkillCost = new DataView(dtDisplay, "", "MainSort, SortOrder", DataViewRowState.CurrentRows);
            gvCostList.DataSource = dvSkillCost;
            gvCostList.DataBind();
        }

        protected void MarkParentNodes(TreeNode NodeToCheck)
        {
            NodeToCheck.Checked = true;
            if (NodeToCheck.Parent != null)
                MarkParentNodes(NodeToCheck.Parent);
        }

        protected void DeselectChildNodes(TreeNode NodeToCheck)
        {
            NodeToCheck.Checked = false;
            foreach (TreeNode Child in NodeToCheck.ChildNodes)
            {
                Child.Checked = false;
                DeselectChildNodes(Child);
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
                   @"style=""text-decoration: none; color: black; margin-left: 0px; padding-left: 0px;"" > " + dTreeNode["SkillName"].ToString() + @"</a>";
            return sTreeNode;
        }

        protected double CalcSkillCost()
        {
            double TotalCost = 0.0;

            DataTable dtSkills = Session["CharSkills"] as DataTable;
            foreach (DataRow dRow in dtSkills.Rows)
            {
                double CPCost;
                if (double.TryParse(dRow["SkillCPCost"].ToString(), out CPCost))
                {
                    TotalCost += CPCost;
                }
            }
            return TotalCost;
        }

        protected void gvCostList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.Cells[0].Text.ToUpper() == "TOTAL CP") ||
                     (e.Row.Cells[0].Text.ToUpper() == "TOTAL SPENT") ||
                     (e.Row.Cells[0].Text.ToUpper() == "TOTAL AVAIL"))
                {
                    e.Row.Font.Bold = true;
                }
                HiddenField hidColor = (HiddenField)e.Row.FindControl("hidColor");
                if (hidColor != null)
                {
                    e.Row.ForeColor = Color.FromName(hidColor.Value);
                }
                HiddenField hidSortOrder = (HiddenField)e.Row.FindControl("hidSortOrder");
                if (hidSortOrder != null)
                {
                    int iSortOrder;
                    if (int.TryParse(hidSortOrder.Value, out iSortOrder))
                    {
                        if (iSortOrder < 10)
                        {
                            e.Row.Font.Bold = true;
                            e.Row.Font.Size = new FontUnit(12, UnitType.Point);
                        }
                        if (iSortOrder < 0)
                        {
                            e.Row.Font.Size = new FontUnit(12, UnitType.Point);
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //            int iCharID;

            //if ((Session["CharSkillCharacterID"] != null) &&
            //    (Session["SkillNodes"] != null) &&
            //    (Session["tvSkills"] != null))
            //{
            //    if (int.TryParse(Session["CharSkillCharacterID"].ToString(), out iCharID))
            //    {
            DataTable dtCampaignSkills = Session["SkillNodes"] as DataTable;
            //_tvSkills = Session["tvSkills"] as TreeView;

            //Classes.cCharacter Char = new Classes.cCharacter();
            //Char.LoadCharacter(iCharID);

            int CharacterSkillsSetID = -1;

            CharacterSkillsSetID = oCharSelect.CharacterInfo.CharacterSkillSetID;

            foreach (Classes.cCharacterSkill cSkill in oCharSelect.CharacterInfo.CharacterSkills)
            {
                cSkill.RecordStatus = Classes.RecordStatuses.Delete;
                CharacterSkillsSetID = cSkill.CharacterSkillSetID;
            }

            foreach (TreeNode SkillNode in tvDisplaySkills.CheckedNodes)
            {
                int iSkillNodeID;
                if (int.TryParse(SkillNode.Value, out iSkillNodeID))
                {
                    var FoundRecord = oCharSelect.CharacterInfo.CharacterSkills.Find(x => x.CampaignSkillNodeID == iSkillNodeID);
                    if (FoundRecord != null)
                        FoundRecord.RecordStatus = Classes.RecordStatuses.Active;
                    else
                    {
                        Classes.cCharacterSkill Newskill = new Classes.cCharacterSkill();
                        Newskill.CharacterSkillID = -1;
                        Newskill.CharacterID = Master.UserID;
                        Newskill.CampaignSkillNodeID = iSkillNodeID;
                        Newskill.CharacterSkillSetID = CharacterSkillsSetID;
                        Newskill.CPCostPaid = 0;
                        DataView dvCampaignSkill = new DataView(dtCampaignSkills, "CampaignSkillNodeID = " + iSkillNodeID.ToString(), "", DataViewRowState.CurrentRows);
                        if (dvCampaignSkill.Count > 0)
                        {
                            double dSkillCPCost = 0;
                            if (double.TryParse(dvCampaignSkill[0]["SkillCPCost"].ToString(), out dSkillCPCost))
                                Newskill.CPCostPaid = dSkillCPCost;
                        }
                        oCharSelect.CharacterInfo.CharacterSkills.Add(Newskill);
                    }
                }
            }

            //string sUserName = Session["UserName"].ToString();
            //int iUserID = (int)Session["UserID"];

            foreach (cCharacterSkill dSkill in oCharSelect.CharacterInfo.CharacterSkills)
            {
                if (dSkill.RecordStatus == RecordStatuses.Active)
                    dSkill.Save(Master.UserName, Master.UserID);
                else
                    dSkill.Delete(Master.UserName, Master.UserID);
            }

            lblmodalMessage.Text = "Character " + oCharSelect.CharacterInfo.AKA + " has been saved.";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
        }
        //    }
        //}


        public void CopyTreeNodes(TreeView SourceTreeView, TreeView DesSourceView)
        {
            TreeNode newTn = null;
            foreach (TreeNode tn in SourceTreeView.Nodes)
            {
                newTn = new TreeNode(tn.Text, tn.Value);
                newTn.Checked = tn.Checked;
                newTn.Expanded = tn.Expanded;
                newTn.ShowCheckBox = tn.ShowCheckBox;
                newTn.ImageUrl = tn.ImageUrl;

                CopyChildren(newTn, tn);
                DesSourceView.Nodes.Add(newTn);
            }
        }
        public void CopyChildren(TreeNode parent, TreeNode original)
        {
            TreeNode newTn;
            foreach (TreeNode tn in original.ChildNodes)
            {
                newTn = new TreeNode(tn.Text, tn.Value);
                newTn.Checked = tn.Checked;
                newTn.Expanded = tn.Expanded;
                newTn.ShowCheckBox = tn.ShowCheckBox;
                newTn.ImageUrl = tn.ImageUrl;

                parent.ChildNodes.Add(newTn);
                CopyChildren(newTn, tn);
            }
        }

        private bool CheckForRequirements(string sValueToCheckFor)
        {
            bool bMeetAllRequirements = true;

            SortedList sParams = new SortedList();
            sParams.Add("@SkillNodeID", sValueToCheckFor);
            DataSet dsRequire = cUtilities.LoadDataSet("uspGetNodeRequirements", sParams, "LARPortal", Master.UserName, "CharSkill.aspx_CheckForRequirements");

            // Get the list of items we can't have if we purchased the item.
            DataView dvExcludeRows = new DataView(dsRequire.Tables[0], "ExcludeFromPurchase = true", "SkillNodeID", DataViewRowState.CurrentRows);

            foreach (DataRowView dRow in dvExcludeRows)
            {
                if (dRow["PrerequisiteSkillNodeID"] != DBNull.Value)
                {
                    int iPreReq;
                    if (int.TryParse(dRow["PrerequisiteSkillNodeID"].ToString(), out iPreReq))
                    {
                        List<TreeNode> FoundNodes = FindNodesByValue(iPreReq.ToString());
                        foreach (TreeNode tNode in FoundNodes)
                            DisableNodeAndChildren(tNode);
                    }
                }
            }

            bMeetAllRequirements = true;

            DataView dvRequiredRows = new DataView(dsRequire.Tables[0], "ExcludeFromPurchase = false", "SkillNodeID", DataViewRowState.CurrentRows);

            foreach (DataRowView dRow in dvRequiredRows)
            {
                if (dRow["PrerequisiteSkillNodeID"] != DBNull.Value)
                {
                    int iPreReq;
                    if (int.TryParse(dRow["PrerequisiteSkillNodeID"].ToString(), out iPreReq))
                    {
                        if (iPreReq != 0)
                        {
                            List<TreeNode> FoundNodes = FindNodesByValue(iPreReq.ToString());
                            foreach (TreeNode tNode in FoundNodes)
                                if (!tNode.Checked)
                                    //                            if (FoundNodes.Count == 0)
                                    bMeetAllRequirements = false;
                        }
                    }
                }
            }

            dvRequiredRows = new DataView(dsRequire.Tables[0], "PrerequisiteGroupID is not null", "", DataViewRowState.CurrentRows);

            foreach (DataRowView dRow in dvRequiredRows)
            {
                // Since there is at least one group process it.
                int iPreReqGroup;
                int iNumReq;
                if ((int.TryParse(dRow["PrerequisiteGroupID"].ToString(), out iPreReqGroup)) &&
                    (int.TryParse(dRow["NumGroupSkillsRequired"].ToString(), out iNumReq)))
                {
                    // Get the items for the specific group.
                    DataView dReqGroup = new DataView(dsRequire.Tables[1], "PrerequisiteGroupID = " + iPreReqGroup.ToString(), "", DataViewRowState.CurrentRows);
                    if (dReqGroup.Count > 0)
                    {
                        // There were records. Convert the dataview of reuired nodes convert to a list of string - easier to process.
                        List<string> ReqSkillNodes = dReqGroup.ToTable().AsEnumerable().Select(x => x[1].ToString()).ToList();
                        // If we find the value we are looking for - remove it.
                        ReqSkillNodes.Remove(sValueToCheckFor);
                        List<TreeNode> FoundNode = FindNodesByValueList(ReqSkillNodes);
                        if (FoundNode.Count < iNumReq)
                            bMeetAllRequirements = false;
                    }
                }
            }
            return bMeetAllRequirements;
        }

        private void CheckAllNodesWithValue(string sValueToCheckFor, bool bValueToSet)
        {
            foreach (TreeNode trMainNodes in tvDisplaySkills.Nodes)     // _tvSkills.Nodes)
                SetChildNodes(trMainNodes, sValueToCheckFor, bValueToSet);
        }

        private void SetChildNodes(TreeNode NodeToCheck, string sValueToCheckFor, bool bValueToSet)
        {
            if (NodeToCheck.Value == sValueToCheckFor)
                NodeToCheck.Checked = bValueToSet;

            foreach (TreeNode trChildNode in NodeToCheck.ChildNodes)
                SetChildNodes(trChildNode, sValueToCheckFor, bValueToSet);
        }


        /// <summary>
        /// Go through a node and it's children and 'disable' them. Disabling really means add an image and remove the check box.
        /// </summary>
        /// <param name="tNode"></param>
        private void DisableNodeAndChildren(TreeNode tNode)
        {
            tNode.Text = tNode.Text.Replace("black", "grey");
            tNode.ImageUrl = "/img/delete.png";
            tNode.ShowCheckBox = false;
            foreach (TreeNode ChildNode in tNode.ChildNodes)
                DisableNodeAndChildren(ChildNode);
        }


        /// <summary>
        /// Go through a node and it's children and 'enable' them. Enabling it removing the image and turning the check box on.
        /// </summary>
        /// <param name="tNode"></param>
        private void EnableNodeAndChildren(TreeNode tNode)
        {
            tNode.Text = tNode.Text.Replace("grey", "black");
            tNode.ImageUrl = "";
            tNode.ShowCheckBox = true;

            foreach (TreeNode tnChild in tNode.ChildNodes)
                EnableNodeAndChildren(tnChild);
        }


        /// <summary>
        /// Given a tree node, see if it's value is in the value we are searching for. For each node, go through the child nodes.
        /// </summary>
        /// <param name="ValueToSearchFor">Single string value to look for. Value is stored in nodes .Value</param>
        /// <returns>List of tree nodes with the value searching. It should only return a single node but use a list just in case.</returns>
        private List<TreeNode> FindNodesByValue(string ValueToSearchFor)
        {
            List<TreeNode> FoundNodes = new List<TreeNode>();

            foreach (TreeNode tNode in tvDisplaySkills.Nodes)       // _tvSkills.Nodes)
            {
                SearchChildren(tNode, FoundNodes, ValueToSearchFor);
            }

            return FoundNodes;
        }


        /// <summary>
        /// Given a tree node, see if it is the value we are looking for. Have to go through all of the children's node.
        /// </summary>
        /// <param name="tNode">The node to check the value and to search the children off.</param>
        /// <param name="FoundNodes">List of nodes to be returned.</param>
        /// <param name="ValueToSearchFor">The value that we are going to search for.</param>
        private void SearchChildren(TreeNode tNode, List<TreeNode> FoundNodes, string ValueToSearchFor)
        {
            if (tNode.Value == ValueToSearchFor)
                FoundNodes.Add(tNode);

            foreach (TreeNode ChildNode in tNode.ChildNodes)
                SearchChildren(ChildNode, FoundNodes, ValueToSearchFor);
        }


        /// <summary>
        /// Given a tree node, see if it's value is in the list we are searching for. For each node, go through the child nodes.
        /// </summary>
        /// <param name="lValueList">List of string values we are searching for.</param>
        /// <returns>List of tree nodes with the value values we have found.</returns>
        private List<TreeNode> FindNodesByValueList(List<string> lValueList)
        {
            List<TreeNode> FoundNodes = new List<TreeNode>();

            foreach (TreeNode tNode in tvDisplaySkills.Nodes)       // _tvSkills.Nodes)
            {
                SearchChildrenList(tNode, FoundNodes, lValueList);
            }

            return FoundNodes;
        }


        /// <summary>
        /// Given a tree node, see if it is one of the values we are looking for. Have to go through all of the children's node.
        /// </summary>
        /// <param name="tNode">The node to check the value and to search the children off.</param>
        /// <param name="FoundNodes">List of nodes to be returned.</param>
        /// <param name="ValueToSearchFor">List of values we are searching for.</param>
        private void SearchChildrenList(TreeNode tNode, List<TreeNode> FoundNodes, List<string> lValueList)
        {
            // See if the tree value is in the list we are search for. If so, add it to the nodes.
            if (tNode.Checked)
                if (lValueList.Exists(x => x == tNode.Value))
                    FoundNodes.Add(tNode);

            foreach (TreeNode ChildNode in tNode.ChildNodes)
                SearchChildrenList(ChildNode, FoundNodes, lValueList);
        }

        protected void CheckExclusions()
        {
            if (Session["NodeExclusions"] == null)
                return;

            foreach (TreeNode tNode in tvDisplaySkills.Nodes)       // _tvSkills.Nodes)
                EnableNodeAndChildren(tNode);

            DataTable dtExclusions;
            dtExclusions = Session["NodeExclusions"] as DataTable;

            foreach (TreeNode CheckedNode in tvDisplaySkills.CheckedNodes)      // _tvSkills.CheckedNodes)
            {
                string sSkill = CheckedNode.Value;
                DataView dvPreReq = new DataView(dtExclusions, "PreRequisiteSkillNodeID = " + sSkill, "", DataViewRowState.CurrentRows);
                foreach (DataRowView dExclude in dvPreReq)
                {
                    string sExc = dExclude["SkillNodeID"].ToString();
                    List<TreeNode> ExcludedNodes = FindNodesByValue(sExc);
                    foreach (TreeNode tnExc in ExcludedNodes)
                        DisableNodeAndChildren(tnExc);
                }
            }
        }

        /// <summary>
        /// Add the Javascript to display an error alert.
        /// </summary>
        /// <param name="pvMessage"></param>
        private void DisplayErrorMessage(string pvMessage)
        {
            lblmodalMessage.Text = pvMessage;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
        }


        /// <summary>
        /// Go through all of the checked nodes and make sure you have all the requirements. This is for when somebody unchecks something.
        /// You then have to go through all of the nodes to make sure that you still have the requirements for everything else.
        /// </summary>
        private void CheckSkillRequirementExclusions()
        {
            SortedList sParams = new SortedList();
            if (Session["CampaignID"] == null)
                Response.Redirect("/default.aspx", true);

            // Get all the prereqs/exclusions for the entire campaign so we don't have to keep reloading it.
            sParams.Add("@CampaignID", Session["CampaignID"].ToString());
            DataSet dsRequire = cUtilities.LoadDataSet("uspGetCampaignNodeRequirements", sParams, "LARPortal", Master.UserName, "CharSkill.aspx_CheckSkillRequirementExclusions");

            bool bChangesMade = false;

            // Enable everything. Then we will go through and disable nodes as needed.
            foreach (TreeNode tBaseNode in tvDisplaySkills.Nodes)       // _tvSkills.Nodes)
                EnableNodeAndChildren(tBaseNode);

            // As long as we have made a change to the tree, keep rechecking.
            do
            {
                bChangesMade = false;
                foreach (TreeNode tNode in tvDisplaySkills.CheckedNodes)        // _tvSkills.CheckedNodes)
                {
                    // Do we have all of the requirements for this node?
                    if (!CheckNodeRequirement(tNode, dsRequire))
                    {
                        // Don't have the requirements so the node has already been unchecked. We need to start over and check all the requirements.
                        bChangesMade = true;
                        break;
                    }
                }
            } while (bChangesMade);
        }


        /// <summary>
        /// Give a node and the dataset for the campaign, see if the node has all the requirements it needs. If it doesn't uncheck it.
        /// </summary>
        /// <param name="tNode">Checked node that needs to be check if all the requirements are met.</param>
        /// <param name="dsRequire">The dataset with the prereqs/exclusions for the campaign.</param>
        /// <returns>True means it has everything it needs, False it doesn't have all the requirements.</returns>
        private bool CheckNodeRequirement(TreeNode tNode, DataSet dsRequire)
        {
            bool bMetRequirements = true;

            try
            {
                // Table 0 is the prereq of a single node.
                DataView dvRequiredRows = new DataView(dsRequire.Tables[0], "ExcludeFromPurchase = false and PrerequisiteGroupID is null and SkillNodeID = " + tNode.Value,
                    "SkillNodeID", DataViewRowState.CurrentRows);

                // Go through all of the single requirements and make sure they are all there.
                foreach (DataRowView dRow in dvRequiredRows)
                {
                    if (dRow["PrerequisiteSkillNodeID"] != DBNull.Value)
                    {
                        int iPreReq;
                        if (int.TryParse(dRow["PrerequisiteSkillNodeID"].ToString(), out iPreReq))
                        {
                            if (iPreReq != 0)       // May be set to 0 by accident.
                            {
                                // Get the single pre/req skill from the nodes
                                List<TreeNode> tnFoundNode = FindNodesByValue(iPreReq.ToString());
                                if (tnFoundNode.Count == 0)
                                {
                                    bMetRequirements = false;
                                }
                            }
                        }
                    }
                }

                // Check to make sure the node has all the required skills for purchase based on a group of skills.
                dvRequiredRows = new DataView(dsRequire.Tables[0], "PrerequisiteGroupID is not null and SkillNodeID = " + tNode.Value, "", DataViewRowState.CurrentRows);

                foreach (DataRowView dRow in dvRequiredRows)
                {
                    // Since there is at least one group process it.
                    int iPreReqGroup;       // What's the number of the group to process.
                    int iNumReq;            // How many of the requirements do we have to have?
                    if ((int.TryParse(dRow["PrerequisiteGroupID"].ToString(), out iPreReqGroup)) &&
                        (int.TryParse(dRow["NumGroupSkillsRequired"].ToString(), out iNumReq)))
                    {
                        // Get the items for the specific group. Table1 is the group requirements.
                        DataView dReqGroup = new DataView(dsRequire.Tables[1], "PrerequisiteGroupID = " + iPreReqGroup.ToString(), "", DataViewRowState.CurrentRows);
                        if (dReqGroup.Count > 0)
                        {
                            // There were records. Convert the dataview of required nodes to a list of strings - easier to process. The 2nd field is the skill nodes.
                            List<string> ReqSkillNodes = dReqGroup.ToTable().AsEnumerable().Select(x => x[1].ToString()).ToList();
                            // If we find the value we are looking for - remove it.
                            ReqSkillNodes.Remove(tNode.Value);
                            List<TreeNode> FoundNode = FindNodesByValueList(ReqSkillNodes);
                            if (FoundNode.Count < iNumReq)
                                bMetRequirements = false;
                        }
                    }
                }

                // Only need to check exclusions if the all of the requirements have been met.
                if (bMetRequirements)
                {
                    // Check exclusions. Disable all nodes sthat are excluded because of this.
                    dvRequiredRows = new DataView(dsRequire.Tables[0], "ExcludeFromPurchase = true and SkillNodeID = " + tNode.Value, "SkillNodeID", DataViewRowState.CurrentRows);

                    foreach (DataRowView dRow in dvRequiredRows)
                    {
                        if (dRow["PrerequisiteSkillNodeID"] != DBNull.Value)
                        {
                            int iPreReq;
                            if (int.TryParse(dRow["PrerequisiteSkillNodeID"].ToString(), out iPreReq))
                            {
                                if (iPreReq.ToString() != tNode.Value)
                                {
                                    // Get the node that has the value of the prereq and disable it and all of it's children.
                                    List<TreeNode> tnFoundNode = FindNodesByValue(iPreReq.ToString());
                                    foreach (TreeNode tnNodesToExclude in tnFoundNode)
                                    {
                                        DisableNodeAndChildren(tnNodesToExclude);
                                    }
                                }
                            }
                        }
                    }
                }

                if (!bMetRequirements)
                    tNode.Checked = false;
            }
            catch (Exception ex)
            {
                string l = ex.Message;
            }

            return bMetRequirements;
        }

        protected void RemoveExclusions()
        {
            List<string> NodeList = new List<string>();

            foreach (TreeNode tNode in tvDisplaySkills.Nodes)       // _tvSkills.Nodes)
            {
                if (tNode.ImageUrl == "/img/delete.png")
                {
                    NodeList.Add(tNode.ValuePath);
                }
                RemoveNodes(tNode, NodeList);
            }
            foreach (string t in NodeList)
            {
                TreeNode FoundID = tvDisplaySkills.FindNode(t);       // _tvSkills.FindNode(t);
                if (FoundID != null)
                    FoundID.Parent.ChildNodes.Remove(FoundID);
            }
        }

        protected void RemoveNodes(TreeNode tNode, List<string> NodeList)
        {
            if (tNode.ImageUrl == "/img/delete.png")
            {
                NodeList.Add(tNode.ValuePath);
            }
            else
            {
                foreach (TreeNode tChild in tNode.ChildNodes)
                    RemoveNodes(tChild, NodeList);
            }
        }

        public void SaveNodeState(TreeView tvMemorySkills)
        {
            foreach (TreeNode tMainNode in tvDisplaySkills.Nodes)
            {
                TreeNode tCopy = tvMemorySkills.FindNode(tMainNode.ValuePath);
                if (tCopy != null)
                {
                    tCopy.Expanded = tMainNode.Expanded;
                    foreach (TreeNode tChild in tMainNode.ChildNodes)
                    {
                        ProcessChildren(tChild, tvMemorySkills);
                    }
                }
            }
        }

        public void ProcessChildren(TreeNode tChild, TreeView tvMemorySkills)
        {
            TreeNode tCopy = tvMemorySkills.FindNode(tChild.ValuePath);
            if (tCopy != null)
            {
                tCopy.Expanded = tChild.Expanded;
                foreach (TreeNode tNextNode in tChild.ChildNodes)
                    ProcessChildren(tNextNode, tvMemorySkills);
            }
        }

        public void CopyDisplayTreeNodes(TreeView InMemory, TreeView DisplayTree)
        {
            DisplayTree.Nodes.Clear();

            bool ReadOnly = false;

            if (Session["CharSkillReadOnly"] != null)
                if (Session["CharSkillReadOnly"].ToString() == "Y")
                    ReadOnly = true;

            bool DisplayExcluded = false;
            if (Session["SkillShowExclusions"] != null)
                if (Session["SkillShowExclusions"].ToString() == "Y")
                    DisplayExcluded = true;

            TreeNode tnDisplay = new TreeNode();

            foreach (TreeNode tnInMemory in InMemory.Nodes)
            {
                // If the node has an image of DELETE and don't want to show exclusions, don't copy the node.
                if (!DisplayExcluded)
                    if (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG"))
                        continue;

                if ((ReadOnly) &&           // JBradshaw 3/5/2017 If readonly (NPC) only display checked values.
                    (!tnInMemory.Checked))
                    continue;

                tnDisplay = new TreeNode(tnInMemory.Text, tnInMemory.Value);
                tnDisplay.Checked = tnInMemory.Checked;
                tnDisplay.Expanded = tnInMemory.Expanded;
                tnDisplay.ShowCheckBox = tnInMemory.ShowCheckBox;
                tnDisplay.ImageUrl = tnInMemory.ImageUrl;

                if (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG"))
                    tnDisplay.ShowCheckBox = false;

                if ((ReadOnly) || (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG")))
                    tnDisplay.ShowCheckBox = false;
                else
                    tnDisplay.ShowCheckBox = true;

                CopyDisplayChildren(tnInMemory, tnDisplay, DisplayExcluded, ReadOnly);
                DisplayTree.Nodes.Add(tnDisplay);
            }
            if (ReadOnly)
                DisplayTree.ShowCheckBoxes = TreeNodeTypes.None;
            if (DisplayTree.Nodes.Count == 0)
                lblMessage.Text += " This character has no skills purchased.";
        }

        public void CopyDisplayChildren(TreeNode InMemory, TreeNode Display, bool DisplayExcluded, bool ReadOnly)
        {
            foreach (TreeNode tnInMemory in InMemory.ChildNodes)
            {
                // If the node has an image of DELETE and don't want to show exclusions, don't copy the node.
                if (!DisplayExcluded)
                    if (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG"))
                        continue;

                TreeNode tnDisplay = new TreeNode();

                if ((ReadOnly) &&
                    (!tnInMemory.Checked))
                    continue;

                tnDisplay = new TreeNode(tnInMemory.Text, tnInMemory.Value);
                tnDisplay.Checked = tnInMemory.Checked;
                tnDisplay.Expanded = tnInMemory.Expanded;
                tnDisplay.ShowCheckBox = tnInMemory.ShowCheckBox;
                tnDisplay.ImageUrl = tnInMemory.ImageUrl;

                if ((ReadOnly) || (tnInMemory.ImageUrl.ToUpper().Contains("DELETE.PNG")))
                    tnDisplay.ShowCheckBox = false;
                else
                    tnDisplay.ShowCheckBox = true;

                Display.ChildNodes.Add(tnDisplay);
                CopyDisplayChildren(tnInMemory, tnDisplay, DisplayExcluded, ReadOnly);
            }
        }

        protected void cbxShowExclusions_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxShowExclusions.Checked)
                Session["SkillShowExclusions"] = "Y";
            else
            {
                foreach (TreeNode tnNode in tvDisplaySkills.Nodes)
                {
                    if (tnNode.ImageUrl.ToUpper().Contains("DELETE.PNG"))
                        return;
                }
            }
        }

        protected void oCharSelect_CharacterChanged(object sender, EventArgs e)
        {
            oCharSelect.LoadInfo();

            if (oCharSelect.CharacterID.HasValue)
            {
                Session["CharSkillCharacterID"] = oCharSelect.CharacterID.Value;
                Session["ReloadCharacter"] = "Y";
                if ((oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters) &&
                    (oCharSelect.CharacterInfo.CharacterType != 1))
                {
                    Session["CharSkillReadOnly"] = "Y";
                    btnSave.Enabled = false;
                    btnSave.CssClass = "btn-default";
                    btnSave.Style["background-color"] = "grey";
                }
                else
                {
                    Session["CharSkillReadOnly"] = "N";
                    btnSave.Enabled = true;
                    btnSave.Style["background-color"] = null;
                }

                Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "PasswordNotNeeded", Session.SessionID);
                UserInfo.LastLoggedInCampaign = oCharSelect.CharacterInfo.CampaignID;
                UserInfo.LastLoggedInCharacter = oCharSelect.CharacterID.Value;
                UserInfo.LastLoggedInMyCharOrCamp = (oCharSelect.WhichSelected == LarpPortal.Controls.CharacterSelect.Selected.MyCharacters ? "M" : "C");
                UserInfo.Save();
				Master.ChangeSelectedCampaign();
            }
        }

		protected void MasterPage_CampaignChanged(object sender, EventArgs e)
		{
			string t = sender.GetType().ToString();
			oCharSelect.Reset();
			_Reload = true;
		}
	}
}
