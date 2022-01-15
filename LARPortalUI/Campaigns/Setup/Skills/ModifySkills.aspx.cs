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
        private bool _Reload = false;

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

            if ((!IsPostBack) || (_Reload))
            {
                MasterPage_CampaignChanged(null, null);
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

            Session["Pools"] = dsSkillSets.Tables[4];

            tvDisplaySkills.Nodes.Clear();
            tvDisplaySkills.ShowCheckBoxes = TreeNodeTypes.None;

            ddlSkillType.DataTextField = "SkillTypeDescription";
            ddlSkillType.DataValueField = "SkillTypeID";
            ddlSkillType.DataSource = dsSkillSets.Tables[2];
            ddlSkillType.DataBind();

            //ddlPoolName.DataTextField = "PoolDisplay";
            //ddlPoolName.DataValueField = "CampaignSkillPoolID";
            //ddlPoolName.DataSource = dsSkillSets.Tables[4];
            //ddlPoolName.DataBind();

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
                    double dSkillCost = 0.0;
                    //if (double.TryParse(dRow["SkillCPCost"].ToString(), out dSkillCost))
                    //{
                    //    tbSkillCost.Text = string.Format("{0:N1}", dSkillCost);
                    //}
                    tbIncant.Text = dRow["SkillIncant"].ToString();
                    tbCardDescription.Text = dRow["SkillCardDescription"].ToString();
                    //                    tbLongDescription.Text = dRow["SkillLongDescription"].ToString();
                    CKLongDescription.Text = dRow["SkillLongDescription"].ToString();
                    CKShortDescription.Text = dRow["SkillShortDescription"].ToString();
                    tbSkillName.Text = dRow["SkillName"].ToString();

                    //ddlPoolName.ClearSelection();
                    //foreach (ListItem lItem in ddlPoolName.Items)
                    //{
                    //	if (lItem.Value == dRow["CampaignSkillPoolID"].ToString())
                    //	{
                    //		ddlPoolName.ClearSelection();
                    //		lItem.Selected = true;
                    //	}
                    //}
                    //if (ddlPoolName.SelectedIndex == -1)
                    //	ddlPoolName.SelectedIndex = 0;

                    ddlSkillType.ClearSelection();
                    foreach (ListItem lItem in ddlSkillType.Items)
                    {
                        if (lItem.Value == dRow["SkillTypeID"].ToString())
                        {
                            ddlSkillType.ClearSelection();
                            lItem.Selected = true;
                        }
                    }

                    DataTable dtSkills = Session["SkillNodes"] as DataTable;
                    DataView dvSkills = new DataView(dtSkills, "CampaignSkillID = " + dRow["CampaignSkillID"].ToString(), "", DataViewRowState.CurrentRows);

                    DataTable dtDisplay = new DataTable();
                    dtDisplay.Columns.Add("DisplayValue", typeof(string));
                    dtDisplay.Columns.Add("Pool", typeof(string));
                    dtDisplay.Columns.Add("Cost", typeof(double));
                    dtDisplay.Columns.Add("AllCosts", typeof(string));

                    DataTable dtNodeCost;
                    if (Session["NodeCost"] != null)
                        dtNodeCost = Session["NodeCost"] as DataTable;
                    else
                        dtNodeCost = null;

                    foreach (DataRowView dNodes in dvSkills)
                    {
                        string sDisplayValue = GetParents(dNodes["CampaignSkillNodeID"].ToString(), dtSkills);
                        DataRow dNewRow = dtDisplay.NewRow();
                        dNewRow["DisplayValue"] = sDisplayValue;
                        dNewRow["Pool"] = dNodes["PoolDescription"].ToString();
                        double dTemp;
                        if (double.TryParse(dNodes["SkillCPCost"].ToString(), out dTemp))
                            dNewRow["Cost"] = dTemp;

                        string sAllCosts = "";
                        if (dtNodeCost != null)
                        {
                            DataView dvCosts = new DataView(dtNodeCost, "CampaignSkillNodeID = " + dRow["CampaignSkillID"].ToString(), "", DataViewRowState.CurrentRows);
                            foreach (DataRowView dvCost in dvCosts)
                            {
                                double dCost;
                                if (double.TryParse(dRow["SkillCPCost"].ToString(), out dCost))
                                {
                                    if (sAllCosts != "")
                                        sAllCosts += "; ";
                                    sAllCosts += dvCost["PoolDescription"].ToString() + ": " + dCost.ToString("#0.00");
                                }
                            }
                        }
                        dNewRow["AllCosts"] = sAllCosts;
                        dtDisplay.Rows.Add(dNewRow);
                    }
                    //gvSkills.DataSource = dtDisplay;
                    //gvSkills.DataBind();

                    hidCampaignSkillNodeID.Value = dRow["CampaignSkillNodeID"].ToString();
                    hidCampaignSkillsID.Value = dRow["CampaignSkillID"].ToString();
                    divSkillItems.Visible = true;
                }
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
    }
}
