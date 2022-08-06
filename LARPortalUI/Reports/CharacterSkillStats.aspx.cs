using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class CharacterSkillStats : System.Web.UI.Page
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
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@StatusID", 50); // 50 = Scheduled
            sParams.Add("@EventLength", 35); // How many characters of Event Name/date to return with ellipsis
            DataTable dtEvent = Classes.cUtilities.LoadDataTable("uspGetCampaignEvents", sParams, "LARPortal", Master.UserName, lsRoutineName);
            ddlEventDate.ClearSelection();
            ddlEventDate.DataTextField = "EventNameDate";
            ddlEventDate.DataValueField = "EventID";
            ddlEventDate.DataSource = dtEvent;
            ddlEventDate.DataBind();
            ddlEventDate.Items.Insert(0, new ListItem("Select Event - Date", "0"));
            ddlEventDate.Items.Insert(1, new ListItem("All primary character skill sets", "1"));
            ddlEventDate.SelectedIndex = 0;
            ddlReportType.SelectedIndex = 0;
            ddlReportType.Visible = false;
            mvReport.SetActiveView(vwNoReport);

            ddlReportType.SelectedIndex = 0;
            lblReportType.Visible = false;
            ddlReportType.Visible = false;
        }

        protected void ddlEventDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvReport.SetActiveView(vwNoReport);
            btnExportExcel.Visible = false;
            ddlReportType.SelectedIndex = 0;
            if(Session["ddReportTypeReset"] as string == "true")
            {
                ddlReportType.Items.Add("SkillDetail");
                ddlReportType.Items.Add("SkillTypeCount");
                ddlReportType.Items.Add("SkillTypeDetail");
            }
            Session["ddReportTypeReset"] = "false";

            if (ddlEventDate.SelectedIndex == 0)
            {
                lblReportType.Visible = false;
                ddlReportType.Visible = false;
            }
            else
            {
                if (ddlEventDate.SelectedIndex == 1)
                {
                    ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("SkillDetail"));
                    ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("SkillTypeCount"));
                    ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("SkillTypeDetail"));
                    Session["ddReportTypeReset"] = "true";
                }
                lblReportType.Visible = true;
                ddlReportType.Visible = true;
            }

        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bindgrid();
        }

        protected void Bindgrid()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            DataTable dtSkillStats = new DataTable();
            SortedList sParams = new SortedList();

            int EventID = 0;
            int.TryParse(ddlEventDate.SelectedValue.ToString(), out EventID);
            string SkillCountProc;

            switch (ddlEventDate.SelectedValue)
            {
                case "1":
                    SkillCountProc = "uspRptCharacterCampaignSkillCount";
                    break;

                default:
                    SkillCountProc = "uspRptCharacterEventSkillCount";
                    sParams.Add("@EventID", EventID);
                    break;
            }
            sParams.Add("@CampaignID", Master.CampaignID);

            switch (ddlReportType.SelectedValue.ToUpper())
            {
                case "SKILLCOUNT":
                    mvReport.SetActiveView(vwSkillCount);
                    dtSkillStats = Classes.cUtilities.LoadDataTable(SkillCountProc, sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspRptCharacterEventSkillCount");
                    gvSkillCount.DataSource = dtSkillStats;
                    gvSkillCount.DataBind();
                    btnExportExcel.Visible = true;
                    break;

                case "SKILLDETAIL":
                    mvReport.SetActiveView(vwSkillDetail);
                    dtSkillStats = Classes.cUtilities.LoadDataTable("uspRptCharacterEventSkillDetail", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspRptCharacterEventSkillDetail");
                    gvSkillDetail.DataSource = dtSkillStats;
                    gvSkillDetail.DataBind();
                    btnExportExcel.Visible = true;
                    break;

                case "SKILLTYPECOUNT":
                    mvReport.SetActiveView(vwSkillTypeCount);
                    dtSkillStats = Classes.cUtilities.LoadDataTable("uspRptCharacterEventSkillTypeCount", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspRptCharacterEventSkillTypeCount");
                    gvSkillTypeCount.DataSource = dtSkillStats;
                    gvSkillTypeCount.DataBind();
                    btnExportExcel.Visible = true;
                    break;

                case "SKILLTYPEDETAIL":
                    mvReport.SetActiveView(vwSkillTypeDetail);
                    dtSkillStats = Classes.cUtilities.LoadDataTable("uspRptCharacterEventSkillTypeDetail", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspRptCharacterEventSkillTypeDetail");
                    gvSkillTypeDetail.DataSource = dtSkillStats;
                    gvSkillTypeDetail.DataBind();
                    btnExportExcel.Visible = true;
                    break;

                default:
                    mvReport.SetActiveView(vwNoReport);
                    btnExportExcel.Visible = false;
                    return;
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            Bindgrid();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + ddlReportType.SelectedItem.Text + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder columnbind = new StringBuilder();
            string CellText = "";
            switch (ddlReportType.SelectedValue.ToUpper())
            {
                case "SKILLCOUNT":
                    gvSkillCount.AllowPaging = false;
                    //form.Controls.Add(gvSkillCount);
                    gvSkillCount.DataBind();
                    //StringBuilder columnbind = new StringBuilder();
                    //string CellText = "";
                    for (int k = 0; k < gvSkillCount.Columns.Count; k++)
                    {
                        columnbind.Append(gvSkillCount.Columns[k].HeaderText + ',');
                    }
                    columnbind.Append("\r\n");
                    for (int i = 0; i < gvSkillCount.Rows.Count; i++)
                    {
                        for (int k = 0; k < gvSkillCount.Columns.Count; k++)
                        {
                            CellText = gvSkillCount.Rows[i].Cells[k].Text;
                            // Take out commas because they screw up the comma delimited csv string
                            CellText = CellText.Replace(",", "");
                            // Replace HTML characters with real counterparts; &nbsp -> space / &#39; -> apostrophe / &amp; -> & / &quot; -> " / &lt; -> < / &gt; -> >
                            CellText = CellText.Replace("&nbsp;", "");
                            CellText = CellText.Replace("&#39;", "'");
                            CellText = CellText.Replace("&amp;", " and ");
                            CellText = CellText.Replace("&quot;", "\"");
                            CellText = CellText.Replace("&lt;", "<");
                            CellText = CellText.Replace("&gt;", ">");
                            CellText = CellText + ",";
                            columnbind.Append(CellText);
                        }
                        columnbind.Append("\r\n");
                    }
                    break;

                case "SKILLDETAIL":
                    gvSkillDetail.AllowPaging = false;
                    //form.Controls.Add(gvSkillDetail);
                    gvSkillDetail.DataBind();
                    //StringBuilder columnbind = new StringBuilder();
                    //string CellText = "";
                    for (int k = 0; k < gvSkillDetail.Columns.Count; k++)
                    {
                        columnbind.Append(gvSkillDetail.Columns[k].HeaderText + ',');
                    }
                    columnbind.Append("\r\n");
                    for (int i = 0; i < gvSkillDetail.Rows.Count; i++)
                    {
                        for (int k = 0; k < gvSkillDetail.Columns.Count; k++)
                        {
                            CellText = gvSkillDetail.Rows[i].Cells[k].Text;
                            // Take out commas because they screw up the comma delimited csv string
                            CellText = CellText.Replace(",", "");
                            // Replace HTML characters with real counterparts; &nbsp -> space / &#39; -> apostrophe / &amp; -> & / &quot; -> " / &lt; -> < / &gt; -> >
                            CellText = CellText.Replace("&nbsp;", "");
                            CellText = CellText.Replace("&#39;", "'");
                            CellText = CellText.Replace("&amp;", " and ");
                            CellText = CellText.Replace("&quot;", "\"");
                            CellText = CellText.Replace("&lt;", "<");
                            CellText = CellText.Replace("&gt;", ">");
                            CellText = CellText + ",";
                            columnbind.Append(CellText);
                        }
                        columnbind.Append("\r\n");
                    }
                    break;

                case "SKILLTYPECOUNT":
                    gvSkillTypeCount.AllowPaging = false;
                    //form.Controls.Add(gvSkillTypeCount);
                    gvSkillTypeCount.DataBind();
                    //StringBuilder columnbind = new StringBuilder();
                    //string CellText = "";
                    for (int k = 0; k < gvSkillTypeCount.Columns.Count; k++)
                    {
                        columnbind.Append(gvSkillTypeCount.Columns[k].HeaderText + ',');
                    }
                    columnbind.Append("\r\n");
                    for (int i = 0; i < gvSkillTypeCount.Rows.Count; i++)
                    {
                        for (int k = 0; k < gvSkillTypeCount.Columns.Count; k++)
                        {
                            CellText = gvSkillTypeCount.Rows[i].Cells[k].Text;
                            // Take out commas because they screw up the comma delimited csv string
                            CellText = CellText.Replace(",", "");
                            // Replace HTML characters with real counterparts; &nbsp -> space / &#39; -> apostrophe / &amp; -> & / &quot; -> " / &lt; -> < / &gt; -> >
                            CellText = CellText.Replace("&nbsp;", "");
                            CellText = CellText.Replace("&#39;", "'");
                            CellText = CellText.Replace("&amp;", " and ");
                            CellText = CellText.Replace("&quot;", "\"");
                            CellText = CellText.Replace("&lt;", "<");
                            CellText = CellText.Replace("&gt;", ">");
                            CellText = CellText + ",";
                            columnbind.Append(CellText);
                        }
                        columnbind.Append("\r\n");
                    }
                    break;

                case "SKILLTYPEDETAIL":
                    gvSkillTypeDetail.AllowPaging = false;
                    //form.Controls.Add(gvSkillTypeDetail);
                    gvSkillTypeDetail.DataBind();
                    //StringBuilder columnbind = new StringBuilder();
                    //string CellText = "";
                    for (int k = 0; k < gvSkillTypeDetail.Columns.Count; k++)
                    {
                        columnbind.Append(gvSkillTypeDetail.Columns[k].HeaderText + ',');
                    }
                    columnbind.Append("\r\n");
                    for (int i = 0; i < gvSkillTypeDetail.Rows.Count; i++)
                    {
                        for (int k = 0; k < gvSkillTypeDetail.Columns.Count; k++)
                        {
                            CellText = gvSkillTypeDetail.Rows[i].Cells[k].Text;
                            // Take out commas because they screw up the comma delimited csv string
                            CellText = CellText.Replace(",", "");
                            // Replace HTML characters with real counterparts; &nbsp -> space / &#39; -> apostrophe / &amp; -> & / &quot; -> " / &lt; -> < / &gt; -> >
                            CellText = CellText.Replace("&nbsp;", "");
                            CellText = CellText.Replace("&#39;", "'");
                            CellText = CellText.Replace("&amp;", " and ");
                            CellText = CellText.Replace("&quot;", "\"");
                            CellText = CellText.Replace("&lt;", "<");
                            CellText = CellText.Replace("&gt;", ">");
                            CellText = CellText + ",";
                            columnbind.Append(CellText);
                        }
                        columnbind.Append("\r\n");
                    }
                    break;

                default:
                    break;
            }
            //gvCheckList.DataBind();
            //StringBuilder columnbind = new StringBuilder();
            //string CellText = "";
            //for (int k = 0; k < gvCheckList.Columns.Count; k++)
            //{
            //    columnbind.Append(gvCheckList.Columns[k].HeaderText + ',');
            //}
            //columnbind.Append("\r\n");
            //for (int i = 0; i < gvCheckList.Rows.Count; i++)
            //{
            //    for (int k = 0; k < gvCheckList.Columns.Count; k++)
            //    {
            //        CellText = gvCheckList.Rows[i].Cells[k].Text;
            //        // Take out commas because they screw up the comma delimited csv string
            //        CellText = CellText.Replace(",", "");
            //        // Replace HTML characters with real counterparts; &nbsp -> space / &#39; -> apostrophe / &amp; -> & / &quot; -> " / &lt; -> < / &gt; -> >
            //        CellText = CellText.Replace("&nbsp;", "");
            //        CellText = CellText.Replace("&#39;", "'");
            //        CellText = CellText.Replace("&amp;", " and ");
            //        CellText = CellText.Replace("&quot;", "\"");
            //        CellText = CellText.Replace("&lt;", "<");
            //        CellText = CellText.Replace("&gt;", ">");
            //        CellText = CellText + ",";
            //        columnbind.Append(CellText);
            //    }
            //    columnbind.Append("\r\n");
            //}
            Response.Output.Write(columnbind.ToString());
            Response.Flush();
            Response.End();

            //HtmlForm form = new HtmlForm();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", ddlReportType.SelectedItem.Text + ".xls"));
            //Response.ContentType = "application/ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //form.Attributes["runat"] = "server";
            //switch (ddlReportType.SelectedValue.ToUpper())
            //{
            //    case "SKILLCOUNT":
            //        gvSkillCount.AllowPaging = false;
            //        form.Controls.Add(gvSkillCount);
            //        break;

            //    case "SKILLDETAIL":
            //        gvSkillDetail.AllowPaging = false;
            //        form.Controls.Add(gvSkillDetail);
            //        break;

            //    case "SKILLTYPECOUNT":
            //        gvSkillTypeCount.AllowPaging = false;
            //        form.Controls.Add(gvSkillTypeCount);
            //        break;

            //    case "SKILLTYPEDETAIL":
            //        gvSkillTypeDetail.AllowPaging = false;
            //        form.Controls.Add(gvSkillTypeDetail);
            //        break;

            //    default:
            //        break;
            //}
            //this.Controls.Add(form);
            //form.RenderControl(hw);
            //string style = @"<!--mce:2-->";
            //Response.Write(style);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();
        }
    }
}