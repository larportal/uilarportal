using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class CharacterRulebook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                mvReport.SetActiveView(vwNoReport);
            }
        }

        public void LoadData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@UserID", Master.UserID);
            DataTable dtCharacters = Classes.cUtilities.LoadDataTable("uspGetCharactersByUserID", sParams, "LARPortal", Master.UserName, lsRoutineName);
            ddlCharacter.ClearSelection();
            ddlCharacter.DataTextField = "CharacterAKA";
            ddlCharacter.DataValueField = "CharacterID";
            ddlCharacter.DataSource = dtCharacters;
            ddlCharacter.DataBind();
            ddlCharacter.Items.Insert(0, new ListItem("Select Character", "0"));
            ddlCharacter.SelectedIndex = 0;

            ddlSkillSet.SelectedIndex = 0;
            ddlSkillSet.Visible = false;
            mvReport.SetActiveView(vwNoReport);

            ddlSkillSet.SelectedIndex = 0;
            lblSkillSet.Visible = false;
            ddlSkillSet.Visible = false;
        }

        protected void ddlCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvReport.SetActiveView(vwNoReport);
            //btnExportExcel.Visible = false;
            ddlSkillSetBuild();

            if (ddlCharacter.SelectedIndex == 0)
            {
                lblSkillSet.Visible = false;
                ddlSkillSet.Visible = false;
            }
            else
            {
                lblSkillSet.Visible = true;
                ddlSkillSet.Visible = true;
                Session["CharacterID"] = ddlCharacter.SelectedValue;
            }
        }

        protected void ddlSkillSetBuild()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CharacterID", ddlCharacter.SelectedValue);
            DataTable dtSkillSets = Classes.cUtilities.LoadDataTable("uspGetCharactersSkillSets", sParams, "LARPortal", Master.UserName, lsRoutineName);
            ddlSkillSet.ClearSelection();
            ddlSkillSet.DataTextField = "SkillSetName";
            ddlSkillSet.DataValueField = "CharacterSkillSetID";
            ddlSkillSet.DataSource = dtSkillSets;
            ddlSkillSet.DataBind();
            ddlSkillSet.Items.Insert(0, new ListItem("Select Skill Set", "0"));
            ddlSkillSet.SelectedIndex = 0;
        }

        protected void ddlSkillSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CharacterSkillSetID"] = ddlSkillSet.SelectedValue;
            hlCharacterRulebookFinal.Visible = true;
            Bindgrid();
        }

        protected void Bindgrid()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            DataTable dtRulebook = new DataTable();
            SortedList sParams = new SortedList();

            int EventID = 0;
            int.TryParse(ddlCharacter.SelectedValue.ToString(), out EventID);

            sParams.Add("@CharacterID", ddlCharacter.SelectedValue);
            sParams.Add("@CharacterSkillSetID", ddlSkillSet.SelectedValue);

            mvReport.SetActiveView(vwRulebook);
            dtRulebook = Classes.cUtilities.LoadDataTable("uspRptCharacterRulebook", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspRptCharacterRulebook");
            gvRulebook.DataSource = dtRulebook;
            gvRulebook.DataBind();
            //btnExportExcel.Visible = true;

        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            Bindgrid();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + ddlCharacter.SelectedItem.Text + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder columnbind = new StringBuilder();
            string CellText = "";

                    gvRulebook.AllowPaging = false;
                    //form.Controls.Add(gvSkillCount);
                    gvRulebook.DataBind();
                    //StringBuilder columnbind = new StringBuilder();
                    //string CellText = "";
                    for (int k = 0; k < gvRulebook.Columns.Count; k++)
                    {
                        columnbind.Append(gvRulebook.Columns[k].HeaderText + ',');
                    }
                    columnbind.Append("\r\n");
                    for (int i = 0; i < gvRulebook.Rows.Count; i++)
                    {
                        for (int k = 0; k < gvRulebook.Columns.Count; k++)
                        {
                            CellText = gvRulebook.Rows[i].Cells[k].Text;
                            // Take out commas because they screw up the comma delimited csv string
                            CellText = CellText.Replace(",", "");
                            // Replace HTML characters with real counterparts; &nbsp -> space / &#39; -> apostrophe / &amp; -> & / &quot; -> " / &lt; -> < / &gt; -> >
                            CellText = CellText.Replace("&nbsp;", "");
                            CellText = CellText.Replace("&#39;", "'");
                            CellText = CellText.Replace("&amp;", " and ");
                            //CellText = CellText.Replace("&quot;", "\"");
                            //CellText = CellText.Replace("&lt;", "<");
                            //CellText = CellText.Replace("&gt;", ">");
                            CellText = CellText + ",";
                            columnbind.Append(CellText);
                        }
                        columnbind.Append("\r\n");
                    }
        
            Response.Output.Write(columnbind.ToString());
            Response.Flush();
            Response.End();

        }
    }
}