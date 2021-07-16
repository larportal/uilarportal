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
using System.Configuration;
using System.Data.SqlClient;


namespace LarpPortal.Reports
{
    public partial class Medical : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlEvent.Attributes.Add("OnChange", "EventChange(); return false;");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnRunReport.Style["Display"] = "none";
                btnExportExcel.Style["Display"] = "none";
                LoadData();
            }
        }

        // This is the code to run the report and populate the grid
        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            btnExportExcel.Style["Display"] = "inline";
            btnRunReport.Style["Display"] = "inline";
            pnlReport.Visible = true;

            SortedList sParams = new SortedList();
            int EventID = 0;
            int.TryParse(ddlEvent.SelectedValue.ToString(), out EventID);
            sParams.Add("@EventID", EventID);
            
            System.Data.DataTable dtCheckList = Classes.cUtilities.LoadDataTable("uspRptMedical", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspRptMedical");

            gvCheckList.DataSource = dtCheckList;
            gvCheckList.DataBind();
        }

//==============================================
        // 1 - This is the new experimental code to try and export the grid as a real Excel
        protected void btnExportExcel_ClickNEW(object sender, EventArgs e)
        {
            //string fileName = "reportsSun.xls";
            //string Extension = ".xls";
            //if (Extension == ".xls")
            //{
            //    PrepareControlForExport(dtgrid);
            //    HttpContext.Current.Response.Clear();
            //    HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
            //    HttpContext.Current.Response.Charset = "";
            //    HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            //    HttpContext.Current.Response.ContentType = "application/ms-excel";
            //    try
            //    {
            //        using (StringWriter sw = new StringWriter())
            //        {
            //            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            //            {
            //                //  Create a form to contain the grid
            //                System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();
            //                table.GridLines = dtgrid.GridLines;

            //                //  add the header row to the table
            //                if (dtgrid.HeaderRow != null)
            //                {
            //                    PrepareControlForExport(dtgrid.HeaderRow);
            //                    table.Rows.Add(dtgrid.HeaderRow);
            //                }

            //                //  add each of the data rows to the table
            //                foreach (GridViewRow row in dtgrid.Rows)
            //                {
            //                    PrepareControlForExport(row);
            //                    table.Rows.Add(row);
            //                }

            //                //  add the footer row to the table
            //                if (dtgrid.FooterRow != null)
            //                {
            //                    PrepareControlForExport(dtgrid.FooterRow);
            //                    table.Rows.Add(dtgrid.FooterRow);
            //                }

            //                //  render the table into the htmlwriter
            //                dtgrid.GridLines = GridLines.Both;
            //                table.RenderControl(htw);

            //                //  render the htmlwriter into the response
            //                HttpContext.Current.Response.Write(sw.ToString());
            //                HttpContext.Current.Response.End();
            //            }
            //        }
            //    }
            //    catch (HttpException ex)
            //    {
            //        throw ex;
            //    }
            //}

        }
        // End 1 ===============================================



        //===============================================
        // This is the old code that exports in an HTML formatted spreadsheeted
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            HtmlForm form = new HtmlForm();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "CheckIn.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvCheckList.AllowPaging = false;
            form.Attributes["runat"] = "server";
            form.Controls.Add(gvCheckList);
            this.Controls.Add(form);
            form.RenderControl(hw);
            string style = @"<!--mce:2-->";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }


        public void LoadData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@StatusID", 50); // 50 = Scheduled
            sParams.Add("@EventLength", 35); // How many characters of Event Name/date to return with ellipsis
            System.Data.DataTable dtEvent = Classes.cUtilities.LoadDataTable("uspGetCampaignEvents", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignEvents");
            ddlEvent.ClearSelection();
            ddlEvent.DataTextField = "EventNameDate";
            ddlEvent.DataValueField = "EventID";
            ddlEvent.DataSource = dtEvent;
            ddlEvent.DataBind();
            ddlEvent.Items.Insert(0, new ListItem("Select Event - Date", ""));
            ddlEvent.SelectedIndex = 0;

            btnRunReport.Style["Display"] = "none";
            btnExportExcel.Style["Display"] = "none";
            pnlReport.Visible = false;
        }
    }
}
