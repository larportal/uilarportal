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
    public partial class PELAnswerSummary : System.Web.UI.Page
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

        public void LoadData()
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            sParams.Add("@CampaignID", Master.CampaignID);
            sParams.Add("@StatusID", 51); // 51 = Completed
            sParams.Add("@EventLength", 35); // How many characters of Event Name/date to return with ellipsis
            DataTable dtEvent = Classes.cUtilities.LoadDataTable("uspGetCampaignEvents", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetCampaignEvents");
            ddlEvent.ClearSelection();
            ddlRole.ClearSelection();
            lblRole.Visible = false;
            ddlRole.Visible = false;
            ddlQuestion.ClearSelection();
            ddlEvent.DataTextField = "EventNameDate";
            ddlEvent.DataValueField = "EventID";
            ddlEvent.DataSource = dtEvent;
            ddlEvent.DataBind();
            ddlEvent.Items.Insert(0, new ListItem("Select Event - Date", "0"));
            ddlEvent.SelectedIndex = 0;

            btnRunReport.Visible = false;
            btnExportExcel.Visible = false;
        }

        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            int EventID = 0;
            int.TryParse(ddlEvent.SelectedValue.ToString(), out EventID);
            sParams.Add("@EventID", EventID);
            DataTable dtPELRoles = Classes.cUtilities.LoadDataTable("uspGetPELRolesForEvent", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELRolesForEvent");
            ddlRole.DataTextField = "PlayerType";
            ddlRole.DataValueField = "RoleID";
            ddlRole.DataSource = dtPELRoles;
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("Select Player Type", "0"));
            ddlRole.SelectedIndex = 0;

            lblRole.Visible = true;
            ddlRole.Visible = true;
            ddlRole.ClearSelection();

            ddlQuestion.ClearSelection();
            lblQuestion.Visible = false;
            ddlQuestion.Visible = false;

            btnRunReport.Visible = false;
            btnExportExcel.Visible = false;
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            int EventID = 0;
            int RoleID = 0;
            int.TryParse(ddlEvent.SelectedValue.ToString(), out EventID);
            int.TryParse(ddlRole.SelectedValue.ToString(), out RoleID);
            sParams.Add("@EventID", EventID);
            sParams.Add("@RoleID", RoleID);
            DataTable dtQuestions = Classes.cUtilities.LoadDataTable("uspGetPELQuestionsForEventRole", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELQuestionsForEventRole");
            ddlQuestion.ClearSelection();
            ddlQuestion.DataTextField = "Question";
            ddlQuestion.DataValueField = "PELQuestionID";
            ddlQuestion.DataSource = dtQuestions;
            ddlQuestion.DataBind();
            ddlQuestion.Items.Insert(0, new ListItem("Select Question", "0"));
            ddlQuestion.SelectedIndex = 0;
            ddlQuestion.Visible = true;
            lblQuestion.Visible = true;
            btnRunReport.Visible = true;
        }

        protected void btnRunReport_Click(object sender, EventArgs e)
        {
            btnExportExcel.Visible = true;
            pnlReport.Visible = true;

            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            int QuestionID = 0;
            int EventID = 0;
            int RoleID = 0;
            int.TryParse(ddlQuestion.SelectedValue.ToString(), out QuestionID);
            int.TryParse(ddlEvent.SelectedValue.ToString(), out EventID);
            int.TryParse(ddlRole.SelectedValue.ToString(), out RoleID);
            sParams.Add("@QuestionID", QuestionID);
            sParams.Add("@EventID", EventID);
            sParams.Add("@Role", RoleID);
            DataTable dtAnswers = Classes.cUtilities.LoadDataTable("uspGetPELAnswerSummary", sParams, "LARPortal", Master.UserName, lsRoutineName + ".uspGetPELAnswerSummary");

            gvAnswers.DataSource = dtAnswers;
            gvAnswers.DataBind();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            HtmlForm form = new HtmlForm();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "PELAnswerSummary.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvAnswers.AllowPaging = false;
            form.Attributes["runat"] = "server";
            form.Controls.Add(gvAnswers);
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