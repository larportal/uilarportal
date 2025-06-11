using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using LarpPortal.Classes;

namespace LarpPortal.Campaigns.Setup
{
    public partial class SystemMenus : System.Web.UI.Page
    {
        DataTable _dtSystemMenus;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Setting the event for the master page so that if the campaign changes, we will reload this page and also reload who the character is.
            Master.CampaignChanged += new EventHandler(MasterPage_CampaignChanged);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            btnCloseMessage.Attributes.Add("data-dismiss", "modal");
            if (!IsPostBack)
            {
                tvSystemMenus.Attributes.Add("onclick", "postBackByObject()");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSystemMenusTreeView();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "temp", "<script language='javascript'>scrollTree();</script>", false);
        }

        protected void LoadSystemMenusTreeView()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            lblCampaignName.Text = Master.CampaignName;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            _dtSystemMenus = cUtilities.LoadDataTable("uspGetCampaignSystemMenus", sParams, "LARPortal", Master.UserName, lsRoutineName);

            tvSystemMenus.Nodes.Clear();

            DataView dvTopNodes = new DataView(_dtSystemMenus, "ParentNodeID is null and editable = 1", "DisplayOrder", DataViewRowState.CurrentRows);
            foreach (DataRowView dvRow in dvTopNodes)
            {
                TreeNode NewNode = new TreeNode();
                NewNode.ShowCheckBox = true;

                NewNode.Text = dvRow["MenuOption"].ToString();// FormatDescString(dvRow);
                NewNode.SelectAction = TreeNodeSelectAction.None;

                int iNodeID;
                if (int.TryParse(dvRow["MenuID"].ToString(), out iNodeID))
                {
                    NewNode.Expanded = false;
                    NewNode.Value = iNodeID.ToString();
                    bool bChecked;
                    if (bool.TryParse(dvRow["Display"].ToString(), out bChecked))
                    {
                        NewNode.Checked = bChecked;
                        if (bChecked)
                            NewNode.Expanded = true;
                    }
                    NewNode.SelectAction = TreeNodeSelectAction.None;
                    PopulateChildNodes(iNodeID, NewNode);
                    tvSystemMenus.Nodes.Add(NewNode);
                }
            }
        }

        private void PopulateChildNodes(int parentId, TreeNode parentNode)
        {
            DataView dvChild = new DataView(_dtSystemMenus, "ParentNodeID = " + parentId.ToString() + " and editable = 1", "DisplayOrder", DataViewRowState.CurrentRows);
            foreach (DataRowView dr in dvChild)
            {
                int iNodeID;
                if (int.TryParse(dr["MenuID"].ToString(), out iNodeID))
                {
                    TreeNode childNode = new TreeNode();
                    childNode.ShowCheckBox = true;
                    childNode.Text = dr["MenuOption"].ToString().Replace("&", "&amp;");
                    childNode.Value = iNodeID.ToString();
                    bool bChecked;
                    if (bool.TryParse(dr["Display"].ToString(), out bChecked))
                    {
                        childNode.Checked = bChecked;
                        if (bChecked)
                            childNode.Expanded = true;
                    }
                    childNode.SelectAction = TreeNodeSelectAction.None;
                    parentNode.ChildNodes.Add(childNode);
                    PopulateChildNodes(iNodeID, childNode);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DataTable dtOrigNodes = Classes.cUtilities.LoadDataTable("uspGetCampaignSystemMenus", sParams, "LARPortal", Master.UserName, lsRoutineName);

            bool bChecked = false;
            foreach (TreeNode tNode in tvSystemMenus.Nodes)
            {
                DataView dvNode = new DataView(dtOrigNodes, "MenuID = " + tNode.Value.ToString(), "", DataViewRowState.CurrentRows);
                if (dvNode.Count > 0)
                {
                    bChecked = false;
                    if (bool.TryParse(dvNode[0]["Display"].ToString(), out bChecked))
                    {
                        if (bChecked != tNode.Checked)
                        {
                            sParams = new SortedList();
                            sParams.Add("@CampaignID", Master.CampaignID);
                            sParams.Add("@MenuID", tNode.Value.ToString());
                            sParams.Add("@Display", bChecked);
                            sParams.Add("@Comment", "Changed by " + Master.UserName);
                            cUtilities.PerformNonQuery("uspInsUpdCMCampaignSystemMenus", sParams, "LARPortal", Master.UserName);
                        }
                    }
                }
                SaveNodes(tNode, dtOrigNodes);
            }

            lblSaveMessage.Text = "Your campaign menus have been saved.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSaveMessage();", true);
            Session["ReloadCampaigns"] = "Y";
        }

        protected void SaveNodes(TreeNode MainNode, DataTable dtOrigNodes)
        {
            DataView dvNode = new DataView(dtOrigNodes, "MenuID = " + MainNode.Value.ToString(), "", DataViewRowState.CurrentRows);
            if (dvNode.Count > 0)
            {
                bool bChecked = false;
                if (bool.TryParse(dvNode[0]["Display"].ToString(), out bChecked))
                {
                    if (bChecked != MainNode.Checked)
                    {

                        SortedList sParams = new SortedList();
                        sParams.Add("@CampaignID", Master.CampaignID);
                        sParams.Add("@MenuID", MainNode.Value.ToString());
                        sParams.Add("@Display", MainNode.Checked);
                        sParams.Add("@Comment", "Changed by " + Master.UserName);
                        cUtilities.PerformNonQuery("uspInsUpdCMCampaignSystemMenus", sParams, "LARPortal", Master.UserName);
                    }
                }
            }
            foreach (TreeNode tChild in MainNode.ChildNodes)
                SaveNodes(tChild, dtOrigNodes);
        }



            protected void tvSystemMenus_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            //string sAddInfo = "";
            //string sDisplayNodeText = Regex.Replace(e.Node.Text, "<.*?>", String.Empty);
            //sAddInfo = String.Format("NodeID {0}/{1}, ", e.Node.Value, sDisplayNodeText);

            if (e.Node.Checked)
            {
                // Save tree nodes so if they don't have enough points to buy the skill, we have the old one.
                TreeView OrigTreeView = new TreeView();
                TreeNode FoundNode = tvSystemMenus.FindNode(e.Node.ValuePath);
                string sNodeValue = e.Node.ValuePath;

                while (FoundNode.Parent != null)
                {
                    FoundNode.Parent.Checked = true;
                    FoundNode = FoundNode.Parent;
                }

            }
            else
            {
                SortedList sParams = new SortedList();
                DataTable dtSystemMenus = cUtilities.LoadDataTable("uspGetSystemMenus", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetSystemMenus");
                TreeNode FoundNode = e.Node;
                DataView dvSystemNodes = new DataView(dtSystemMenus, "MenuID = " + e.Node.Value.ToString(), "", DataViewRowState.CurrentRows);
                if (dvSystemNodes.Count > 0)
                {
                    bool bHidable = false;
                    if (bool.TryParse(dvSystemNodes[0].Row["hidable"].ToString(), out bHidable))
                    {
                        if (bHidable)
                        {
                            e.Node.Checked = false;
                            if (FoundNode.ChildNodes.Count > 0)
                            {
                                DeselectChildren(FoundNode);
                            }
                        }
                        else
                        {
                            e.Node.Checked = true;
                            lblmodalMessage.Text = "You cannot deselect " + e.Node.Text;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMessage();", true);
                        }
                    }
                }
            }
        }

        protected void DeselectChildren(TreeNode tNode)
        {
            if (tNode.ChildNodes.Count > 0)
            {
                foreach (TreeNode child in tNode.ChildNodes)
                {
                    child.Checked = false;
                    DeselectChildren(child);
                }
            }
        }

        protected void MasterPage_CampaignChanged(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            LoadSystemMenusTreeView();
        }

        protected void btnCloseSaveMessage_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnCloseMessage_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/Default.aspx", true);
        }
    }
}