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
    public partial class HistorySearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            Bindgrid();
        }

        protected void Bindgrid()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (tbSearchString.Text.Trim().Length == 0)
                return;

            DataTable dtHistorySearch = new DataTable();
            SortedList sParams = new SortedList();

            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@Keyword1", tbSearchString.Text.Trim());
            sParams.Add("@PadLength", 60);
            dtHistorySearch = Classes.cUtilities.LoadDataTable("uspGetHistoriesWithKeywords", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetHistoriesWithKeywords");
            foreach (DataRow dRow in dtHistorySearch.Rows)
            {
                dRow["HistoryString"] = dRow["HistoryString"].ToString().Replace(tbSearchString.Text.Trim(), "<span class=\"lead\">" + tbSearchString.Text.Trim() + "</span>");
            }

            gvHistorySearch.DataSource = dtHistorySearch;
            gvHistorySearch.DataBind();
            btnExportExcel.Visible = true;
            pnlReport.Visible = true;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            Bindgrid();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=HistorySearch.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            gvHistorySearch.AllowPaging = false;
            gvHistorySearch.DataBind();
            StringBuilder columnbind = new StringBuilder();
            string CellText = "";
            for (int k = 0; k < gvHistorySearch.Columns.Count; k++)
            {
                columnbind.Append(gvHistorySearch.Columns[k].HeaderText + ',');
            }
            columnbind.Append("\r\n");
            for (int i = 0; i < gvHistorySearch.Rows.Count; i++)
            {
                for (int k = 0; k < gvHistorySearch.Columns.Count; k++)
                {
                    CellText = gvHistorySearch.Rows[i].Cells[k].Text;
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
            Response.Output.Write(columnbind.ToString());
            Response.Flush();
            Response.End();

            //HtmlForm form = new HtmlForm();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "HistorySearch.xls"));
            //Response.ContentType = "application/ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //gvHistorySearch.AllowPaging = false;
            //form.Attributes["runat"] = "server";
            //form.Controls.Add(gvHistorySearch);
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
