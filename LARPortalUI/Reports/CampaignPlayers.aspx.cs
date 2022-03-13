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
    public partial class CampaignPlayers : System.Web.UI.Page
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

        protected void LoadData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            DataTable dtCampaignPlayers = Classes.cUtilities.LoadDataTable("uspRptCampaignPlayers", sParams, "LARPortal", Master.UserName, lsRoutineName);
            gvCampaignPlayers.DataSource = dtCampaignPlayers;
            gvCampaignPlayers.DataBind();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            LoadData();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CampaignPlayers.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            gvCampaignPlayers.AllowPaging = false;
            gvCampaignPlayers.DataBind();
            StringBuilder columnbind = new StringBuilder();
            string CellText = "";
            for (int k = 0; k < gvCampaignPlayers.Columns.Count; k++)
            {
                columnbind.Append(gvCampaignPlayers.Columns[k].HeaderText + ',');
            }
            columnbind.Append("\r\n");
            foreach (GridViewRow row in gvCampaignPlayers.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    CellText = "";
                    if (cell.Controls.Count > 0)
                    {
                        foreach (Control control in cell.Controls)
                        {
                            switch (control.GetType().Name)
                            {
                                case "HyperLink":
                                    CellText = (control as HyperLink).Text;
                                    break;
                                case "TextBox":
                                    CellText = (control as TextBox).Text;
                                    break;
                                case "LinkButton":
                                    CellText = (control as LinkButton).Text;
                                    break;
                                case "Button":
                                    CellText = (control as Button).Text;
                                    break;
                                case "CheckBox":
                                    CellText = (control as CheckBox).Text;
                                    break;
                                case "RadioButton":
                                    CellText = (control as RadioButton).Text;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        CellText = cell.Text;
                    }
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
            //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "LARPCalendar.xls"));
            //Response.ContentType = "application/ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //gvCampaignPlayers.AllowPaging = false;
            //form.Attributes["runat"] = "server";
            //form.Controls.Add(gvCampaignPlayers);
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
