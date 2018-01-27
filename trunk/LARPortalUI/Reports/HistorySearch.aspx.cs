using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
            HtmlForm form = new HtmlForm();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "HistorySearch.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvHistorySearch.AllowPaging = false;
            form.Attributes["runat"] = "server";
            form.Controls.Add(gvHistorySearch);
            this.Controls.Add(form);
            form.RenderControl(hw);
            string style = @"<!--mce:2-->";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}
