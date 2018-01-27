using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class SiteList : System.Web.UI.Page
    {
        //string _UserName = "";
        //int _UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserID"].ToString() != null)
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);
            //if (Session["UserName"] != null)
            //    _UserName = Session["UserName"].ToString();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStateLoad();
                ddlCountryLoad();
                btnRunReport_Click(null, null);
            }
        }

        protected void ddlStateLoad()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            DataTable dtState = Classes.cUtilities.LoadDataTable("uspGetSiteStates", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetSiteStates");
            ddlState.DataTextField = "State";
            ddlState.DataValueField = "RowID";
            ddlState.DataSource = dtState;
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("All", "0"));
            ddlState.SelectedIndex = 0;
        }

        protected void ddlCountryLoad()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            DataTable dtCountry = new DataTable();
            SortedList sParams = new SortedList();
            dtCountry = Classes.cUtilities.LoadDataTable("uspGetSiteCountries", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetSiteCountries");
            ddlCountry.DataTextField = "Country";
            ddlCountry.DataValueField = "RowID";
            ddlCountry.DataSource = dtCountry;
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("All", "0"));
            ddlCountry.SelectedIndex = 0;
        }

        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            //btnExportCSV.Visible = true;
            btnExportExcel.Visible = true;
            pnlReport.Visible = true;

            SortedList sParams = new SortedList();
            string sState = ddlState.SelectedItem.Text;
            string sCountry = ddlCountry.SelectedItem.Text;
            if (sState == "All")
                sState = "0";
            if (sCountry == "All")
                sCountry = "0";
            sParams.Add("@State", sState);
            sParams.Add("@Country", sCountry);
            DataTable dtSites = Classes.cUtilities.LoadDataTable("uspGetSiteList", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetSiteList");
            gvSites.DataSource = dtSites;
            gvSites.DataBind();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            HtmlForm form = new HtmlForm();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "LARPCalendar.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvSites.AllowPaging = false;
            //BindGridDetails(gvCalendar);
            form.Attributes["runat"] = "server";
            form.Controls.Add(gvSites);
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