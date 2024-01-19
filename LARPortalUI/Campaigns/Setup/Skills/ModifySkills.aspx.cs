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
    public partial class ModifySkills : System.Web.UI.Page
    {
        protected DataTable _dtCampaignSkills = new DataTable();

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
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (!IsPostBack)
            {
                MasterPage_CampaignChanged(null, null);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "temp", "<script language='javascript'>scrollTree();</script>", false);
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
            string sTreeNode = dTreeNode["SkillName"].ToString();
            return sTreeNode;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@UserID", Master.UserID);
            sParams.Add("@CampaignSkillNodeID", hidCampaignSkillNodeID.Value);

            //int iSkillPoolID;
            //if (int.TryParse(ddlPoolName.SelectedValue, out iSkillPoolID))
            //    sParams.Add("@CampaignSkillPoolID", iSkillPoolID);
            //double dSkillCost;
            //if (double.TryParse(tbSkillCost.Text, out dSkillCost))
            //    sParams.Add("@SkillCPCost", dSkillCost);

            DataTable dtNodeResults = Classes.cUtilities.LoadDataTable("uspInsUpdCHCampaignSkillNodes", sParams, "LARPortal", Master.UserName, lsRoutineName);

            sParams = new SortedList();
            sParams.Add("@UserID", Master.UserID);
            sParams.Add("@CampaignSkillsID", hidCampaignSkillsID.Value);
            sParams.Add("@SkillName", tbSkillName.Text);
            sParams.Add("@SkillTypeID", ddlSkillType.SelectedValue);
            sParams.Add("@SkillShortDescription", CKShortDescription.Text);
            //            sParams.Add("@SkillLongDescription", tbLongDescription.Text);
            sParams.Add("@SkillLongDescription", CKLongDescription.Text);
            sParams.Add("@SkillCardDescription", tbCardDescription.Text);
            sParams.Add("@SkillIncant", tbIncant.Text);

            DataTable dtSkillResults = Classes.cUtilities.LoadDataTable("uspInsUpdCHCampaignSkills", sParams, "LARPortal", Master.UserName, lsRoutineName);

            if ((dtNodeResults.Rows.Count > 0) &&
                (dtSkillResults.Rows.Count > 0))
            {
                lblmodalMessage.Text = "The skill has been saved.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
            }
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
            if (dsSkillSets.Tables.Count >= 7)
                Session["NodeCost"] = dsSkillSets.Tables[5];
            else
                Session.Remove("NodeCost");

            if (dsSkillSets.Tables[4].Columns["PoolDisplay"] == null)
                dsSkillSets.Tables[4].Columns.Add("PoolDisplay", typeof(string));

            foreach (DataRow dRow in dsSkillSets.Tables[4].Rows)
            {
                dRow["PoolDisplay"] = dRow["PoolDescription"].ToString();
                if (dRow["DefaultPool"].ToString() == "1")
                    dRow["PoolDisplay"] += " - Default Pool";
            }

            ddlAddPoolName.Items.Clear();
            ddlAddPoolName.DataSource = dsSkillSets.Tables[4];
            ddlAddPoolName.DataTextField = "PoolDisplay";
            ddlAddPoolName.DataValueField = "CampaignSkillPoolID";
            ddlAddPoolName.DataBind();

            tvDisplaySkills.Nodes.Clear();
            tvDisplaySkills.ShowCheckBoxes = TreeNodeTypes.None;

            ddlSkillType.DataTextField = "SkillTypeDescription";
            ddlSkillType.DataValueField = "SkillTypeID";
            ddlSkillType.DataSource = dsSkillSets.Tables[2];
            ddlSkillType.DataBind();

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
            divSkillItems.Visible = false;
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

                DataSet dsNodeInfo = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillByNodeID", sParams, "LARPortal", Master.UserName, lsRoutineName);

                foreach (DataRow dRow in dsNodeInfo.Tables[0].Rows)
                {
                    divSkillItems.Visible = true;
                    tbIncant.Text = dRow["SkillIncant"].ToString();
                    tbCardDescription.Text = dRow["SkillCardDescription"].ToString();
                    CKLongDescription.Text = dRow["SkillLongDescription"].ToString();
                    CKShortDescription.Text = dRow["SkillShortDescription"].ToString();
                    tbSkillName.Text = dRow["SkillName"].ToString();

                    ddlSkillType.ClearSelection();
                    foreach (ListItem lItem in ddlSkillType.Items)
                    {
                        if (lItem.Value == dRow["SkillTypeID"].ToString())
                        {
                            ddlSkillType.ClearSelection();
                            lItem.Selected = true;
                        }
                    }

                    bool bTemp;
                    swHasRole.Checked = false;
                    if (bool.TryParse(dRow["OpenToAll"].ToString(), out bTemp))
                        swHasRole.Checked = bTemp;
                    if (bool.TryParse(dRow["SuppressParentSkill"].ToString(), out bTemp))
                        swSuppressParent.Checked = bTemp;

                    DataTable dtSkills = Session["SkillNodes"] as DataTable;
                    DataView dvSkills = new DataView(dtSkills, "CampaignSkillID = " + dRow["CampaignSkillID"].ToString(), "", DataViewRowState.CurrentRows);

                    ViewState["Pools"] = dsNodeInfo.Tables[4];
                    BindPoolData();
                    //DataTable dtDisplay = new DataTable();
                    //dtDisplay.Columns.Add("DisplayValue", typeof(string));
                    //dtDisplay.Columns.Add("Pool", typeof(string));
                    //dtDisplay.Columns.Add("Cost", typeof(double));
                    //dtDisplay.Columns.Add("AllCosts", typeof(string));

                    //DataTable dtNodeCost;
                    //if (Session["NodeCost"] != null)
                    //    dtNodeCost = Session["NodeCost"] as DataTable;
                    //else
                    //    dtNodeCost = null;

                    //foreach (DataRowView dNodes in dvSkills)
                    //{
                    //    string sDisplayValue = GetParents(dNodes["CampaignSkillNodeID"].ToString(), dtSkills);
                    //    DataRow dNewRow = dtDisplay.NewRow();
                    //    dNewRow["DisplayValue"] = sDisplayValue;
                    //    dNewRow["Pool"] = dNodes["PoolDescription"].ToString();
                    //    double dTemp;
                    //    if (double.TryParse(dNodes["SkillCPCost"].ToString(), out dTemp))
                    //        dNewRow["Cost"] = dTemp;

                    //    string sAllCosts = "";
                    //    if (dtNodeCost != null)
                    //    {
                    //        DataView dvCosts = new DataView(dtNodeCost, "CampaignSkillNodeID = " + dRow["CampaignSkillID"].ToString(), "", DataViewRowState.CurrentRows);
                    //        foreach (DataRowView dvCost in dvCosts)
                    //        {
                    //            double dCost;
                    //            if (double.TryParse(dRow["SkillCPCost"].ToString(), out dCost))
                    //            {
                    //                if (sAllCosts != "")
                    //                    sAllCosts += "; ";
                    //                sAllCosts += dvCost["PoolDescription"].ToString() + ": " + dCost.ToString("#0.00");
                    //            }
                    //        }
                    //    }
                    //    dNewRow["AllCosts"] = sAllCosts;
                    //    dtDisplay.Rows.Add(dNewRow);
                    //}
                    //gvSkills.DataSource = dtDisplay;
                    //gvSkills.DataBind();

                    hidCampaignSkillNodeID.Value = dRow["CampaignSkillNodeID"].ToString();
                    hidCampaignSkillsID.Value = dRow["CampaignSkillID"].ToString();
                    divSkillItems.Visible = true;
                }

                //DataTable dtPools = new DataTable("Pools");
                //dtPools.Columns.Add("PoolID", typeof(int));
                //dtPools.Columns.Add("PoolDescription", typeof(string));
                //dtPools.Columns.Add("Cost", typeof(double));

                //foreach (DataRow dr in dsNodeInfo.Tables[4].Rows)
                //{
                //    int PoolID;
                //    double Cost;

                //    if ((int.TryParse(dr["CampaignSkillPoolID"].ToString(), out PoolID)) &&
                //        (double.TryParse(dr["SKillCPCost"].ToString(), out Cost)))
                //    {
                //        DataRow dNewRow = dtPools.NewRow();
                //        dNewRow["PoolID"] = PoolID;
                //        dNewRow["PoolDescription"] = dr["PoolDescription"].ToString();
                //        dNewRow["Cost"] = Cost;
                //        dtPools.Rows.Add(dNewRow);
                //    }
                //}
                //ViewState["Pools"] = dtPools;
                //BindPoolData();
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


        //protected void gvPoolData_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {

        //        if ((e.Row.RowState & DataControlRowState.Edit) > 0)
        //        {
        //            DropDownList ddlDisplayColor = e.Row.FindControl("ddlPoolList") as DropDownList;
        //            ddlDisplayColor.SelectedValue = DataBinder.Eval(e.Row.DataItem, "CampaignSkillPoolID").ToString();
        //        }
        //    }
        //}

        protected void BindPoolData()
        {
            DataTable Pools = (DataTable)ViewState["Pools"];
            gvPoolData.DataSource = Pools;
            gvPoolData.DataBind();
        }

        protected void btnDeleteCost_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignSkillNodeID", tvDisplaySkills.SelectedNode.Value);
            sParams.Add("@CampaignSkillPoolID", hidDeletePoolID.Value);
            sParams.Add("@UserName", Master.UserName);
            Classes.cUtilities.PerformNonQuery("uspDelCHCampaignSkillNodeCost", sParams, "LARPortal", Master.UserName);

            sParams = new SortedList();
            sParams.Add("@SkillNodeID", tvDisplaySkills.SelectedNode.Value);
            DataSet dsPools = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillByNodeID", sParams, "LARPortal", Master.UserName, lsRoutineName);
            ViewState["Pools"] = dsPools.Tables[4];
            BindPoolData();
        }

        protected void btnEditPoolSave_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignSkillNodeID", tvDisplaySkills.SelectedNode.Value);
            sParams.Add("@CampaignSkillPoolID", hidEditPoolID.Value);
            double dCost;
            if (double.TryParse(tbEditPoolCost.Text, out dCost))
            {
                sParams.Add("@SkillCPCost", dCost);
                Classes.cUtilities.PerformNonQuery("uspInsUpdCHCampaignSkillNodeCost", sParams, "LARPortal", Master.UserName);
            }

            sParams = new SortedList();
            sParams.Add("@SkillNodeID", tvDisplaySkills.SelectedNode.Value);
            DataSet dsPools = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillByNodeID", sParams, "LARPortal", Master.UserName, lsRoutineName);
            ViewState["Pools"] = dsPools.Tables[4];
            BindPoolData();
        }

        protected void btnAddPoolSave_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignSkillNodeID", tvDisplaySkills.SelectedNode.Value);
            sParams.Add("@CampaignSkillPoolID", ddlAddPoolName.SelectedValue);
            double dCost;
            if (double.TryParse(tbAddPoolCost.Text, out dCost))
            {
                sParams.Add("@SkillCPCost", dCost);
                Classes.cUtilities.PerformNonQuery("uspInsUpdCHCampaignSkillNodeCost", sParams, "LARPortal", Master.UserName);
            }

            sParams = new SortedList();
            sParams.Add("@SkillNodeID", tvDisplaySkills.SelectedNode.Value);
            DataSet dsPools = Classes.cUtilities.LoadDataSet("uspGetCampaignSkillByNodeID", sParams, "LARPortal", Master.UserName, lsRoutineName);
            ViewState["Pools"] = dsPools.Tables[4];
            BindPoolData();
        }

        protected void btn_SaveNode_Click(object sender, EventArgs e)
        {
            SortedList sParams = new SortedList();
            //sParams.Add("@CampaignSkillNodeID", tvDisplaySkills.SelectedNode.Value);
            //sParams.Add("@UserName", Master.UserName);
            //Classes.cUtilities.PerformNonQuery("uspClearCHCampaignSkillNodeCost", sParams, "LARPortal", Master.UserName);

            sParams = new SortedList();
            sParams.Add("@CampaignSkillNodeID", tvDisplaySkills.SelectedNode.Value);
            sParams.Add("@UserID", Master.UserID);
            if (swHasRole.Checked)
                sParams.Add("@OpenToAll", 1);
            else
                sParams.Add("@OpenToAll", 0);
            if (swSuppressParent.Checked)
                sParams.Add("@SuppressParentSkill", 1);
            else
                sParams.Add("@SuppressParentSkill", 0);
            Classes.cUtilities.PerformNonQuery("uspInsUpdCHCampaignSkillNodes", sParams, "LARPortal", Master.UserName);

            //foreach (DataRow dRow in ((DataTable)ViewState["Pools"]).Rows)
            //{
            //    sParams = new SortedList();
            //    sParams.Add("@CampaignSkillNodeID", tvDisplaySkills.SelectedNode.Value);
            //    sParams.Add("@CampaignSkillPoolID", dRow["PoolID"].ToString());
            //    double dCost;
            //    if (double.TryParse(dRow["Cost"].ToString(), out dCost))
            //        sParams.Add("@SkillCPCost", dCost);
            //    else
            //        sParams.Add("@SkillCPCost", dCost);

            //    Classes.cUtilities.PerformNonQuery("uspInsUpdCHCampaignSkillNodeCost", sParams, "LARPortal", Master.UserName);
            //}
            lblmodalMessage.Text = "The skill node has been saved.";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MyApplication", "openMessage();", true);
        }
    }
}
