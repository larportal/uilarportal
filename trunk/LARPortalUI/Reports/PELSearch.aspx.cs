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
    public partial class PELSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
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
            sParams.Add("@PadLength", 30);
            dtHistorySearch = Classes.cUtilities.LoadDataTable("uspGetPELsWithKeywords", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELsWithKeywords");
            foreach (DataRow dRow in dtHistorySearch.Rows)
            {
                dRow["PELString"] = dRow["PELString"].ToString().Replace(tbSearchString.Text.Trim(), "<span class=\"lead\">" + tbSearchString.Text.Trim() + "</span>");
            }

            gvPELSearch.DataSource = dtHistorySearch;
            gvPELSearch.DataBind();
            btnExportExcel.Visible = true;
            pnlReport.Visible = true;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            HtmlForm form = new HtmlForm();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "PELSearch.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvPELSearch.AllowPaging = false;
            form.Attributes["runat"] = "server";
            form.Controls.Add(gvPELSearch);
            this.Controls.Add(form);
            form.RenderControl(hw);
            string style = @"<!--mce:2-->";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        void LoadData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            pnlReport.Visible = false;
            tbSearchString.Text = "";
        }
    }
}