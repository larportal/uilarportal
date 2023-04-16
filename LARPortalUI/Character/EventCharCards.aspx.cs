using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace LarpPortal.Character
{
    public partial class EventCharCards : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<int> lCharIDs = new List<int>();
            string sProcedureName = "";

            SortedList sParams = new SortedList();
            if (Request.QueryString["EventID"] != null)
            {
                int iEventID = 0;
                if (int.TryParse(Request.QueryString["EventID"], out iEventID))
                    sParams.Add("@EventID", iEventID);
                sProcedureName = "uspGetCharactersForEvent";
            }
            else if (Request.QueryString["CampaignID"] != null)
            {
                int iCampaignID = 0;
                if (int.TryParse(Request.QueryString["CampaignID"], out iCampaignID))
                    sParams.Add("@CampaignID", iCampaignID);
                sProcedureName = "uspGetCharactersForCampaign";
            }
            else if (Request.QueryString["CharacterID"] != null)
            {
                int iCharacterID = 0;
                if (int.TryParse(Request.QueryString["CharacterID"], out iCharacterID))
                    sParams.Add("@CharacterID", iCharacterID);
                sProcedureName = "uspGetCharactersForCampaign";
            }

            if (sParams.Count == 0)
                return;

            DataTable dtChars = new DataTable();

            dtChars = Classes.cUtilities.LoadDataTable(sProcedureName, sParams, "LARPortal", Session["UserID"].ToString(), "CharCard.Page_Load.GetChar");

            rptrCharacter.DataSource = dtChars;
            rptrCharacter.DataBind();
        }

        protected void rptrCharacter_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hidSkillSetID = (HiddenField)e.Item.FindControl("hidSkillSetID");
            if (hidSkillSetID != null)
            {
                int iSkillSetID;
                if (int.TryParse(hidSkillSetID.Value, out iSkillSetID))
                {

                    Classes.cCharacter cChar = new Classes.cCharacter();
                    cChar.LoadCharacterBySkillSetID(iSkillSetID);

                    Label lblTotalCP = (Label)e.Item.FindControl("lblTotalCP");
                    if (lblTotalCP != null)
                        lblTotalCP.Text = cChar.TotalCP.ToString("0.00");

                    DataTable dtCharacterSkills = new DataTable();
                    SortedList sParams = new SortedList();
                    sParams.Add("@SkillSetID", iSkillSetID);

                    MethodBase lmth = MethodBase.GetCurrentMethod();
                    string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

                    dtCharacterSkills = Classes.cUtilities.LoadDataTable("uspGetCharCardSkills", sParams, "LARPortal", Session["UserName"].ToString(), lsRoutineName);

                    if (dtCharacterSkills.Columns["FullDescription"] == null)
                        dtCharacterSkills.Columns.Add(new DataColumn("FullDescription", typeof(string)));

                    double CPCost;
                    double CPSpent = 0.0;

                    foreach (DataRow dSkillRow in dtCharacterSkills.Rows)
                    {
                        if (double.TryParse(dSkillRow["CPCostPaid"].ToString(), out CPCost))
                            CPSpent += CPCost;

                        string FullDesc = "";
                        bool bDisplay;
                        if (bool.TryParse(dSkillRow["CardDisplayDescription"].ToString(), out bDisplay))
                        {
                            if (bDisplay)
                            {
                                if (dSkillRow["SkillCardDescription"].ToString().Trim().Length > 0)
                                    FullDesc += dSkillRow["SkillCardDescription"].ToString().Trim() + "; ";
                            }
                        }

                        if (dSkillRow["PlayerDescription"].ToString().Trim().Length > 0)
                            FullDesc += dSkillRow["PlayerDescription"].ToString().Trim() + "; ";

                        if (bool.TryParse(dSkillRow["CardDisplayIncant"].ToString(), out bDisplay))
                        {
                            if (bDisplay)
                            {
                                if (dSkillRow["SkillIncant"].ToString().Trim().Length > 0)
                                    FullDesc += "<i>" + dSkillRow["SkillIncant"].ToString().Trim() + "</i>; ";
                            }
                        }

                        if (dSkillRow["PlayerIncant"].ToString().Trim().Length > 0)
                            FullDesc += "<b><i>" + dSkillRow["PlayerIncant"].ToString().Trim() + "</b></i>";

                        FullDesc = FullDesc.Trim();
                        if (FullDesc.EndsWith(";"))
                            FullDesc = FullDesc.Substring(0, FullDesc.Length - 1);

                        dSkillRow["FullDescription"] = FullDesc;
                    }

                    Label lblCPSpent = (Label)e.Item.FindControl("lblCPSpent");
                    if (lblCPSpent != null)
                        lblCPSpent.Text = CPSpent.ToString("0.00");

                    Label lblCPAvail = (Label)e.Item.FindControl("lblCPAvail");
                    if (lblCPAvail != null)
                        lblCPAvail.Text = (cChar.TotalCP - CPSpent).ToString("0.00");

                    Dictionary<string, string> NonCost = new Dictionary<string, string>();

                    foreach (Classes.cDescriptor Desc in cChar.Descriptors)
                    {
                        if (NonCost.ContainsKey(Desc.CharacterDescriptor))
                            NonCost[Desc.CharacterDescriptor] += Desc.DescriptorValue + ", ";
                        else
                            NonCost.Add(Desc.CharacterDescriptor, Desc.DescriptorValue + ", ");
                    }

                    foreach (string KeyValue in NonCost.Keys.ToList())
                    {
                        if (NonCost[KeyValue].EndsWith(", "))
                            NonCost[KeyValue] = NonCost[KeyValue].Substring(0, NonCost[KeyValue].Length - 2);
                    }

                    DataView dvSkills = new DataView(dtCharacterSkills, "", "DisplayOrder", DataViewRowState.CurrentRows);
                    GridView gvSkills = (GridView)e.Item.FindControl("gvSkills");
                    if (gvSkills != null)
                    {
                        gvSkills.DataSource = dvSkills;
                        gvSkills.DataBind();
                    }

                    GridView gvNonCost = (GridView)e.Item.FindControl("gvNonCost");
                    if (gvNonCost != null)
                    {
                        gvNonCost.DataSource = NonCost;
                        gvNonCost.DataBind();
                    }
                }
            }
        }
    }
}
