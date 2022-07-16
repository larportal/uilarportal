using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Character.ISkills
{
    public partial class StaffAnswerComment : System.Web.UI.Page
    {
        public bool TextBoxEnabled = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(Session["Username"].ToString()))
            //    _UserName = Session["Username"].ToString();
            //if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (!IsPostBack)
            {
                if (Request.QueryString["SkillID"] != null)
                    hidRegistrationID.Value = Request.QueryString["SkillID"];
                else
                    Response.Redirect("StaffList.aspx", true);

                Classes.cUser UserInfo = new Classes.cUser(Master.UserName, "NOPASSWORD", Session.SessionID);
                if (UserInfo.NickName.Length > 0)
                    hidAuthorName.Value = UserInfo.NickName + " " + UserInfo.LastName;
                else
                    hidAuthorName.Value = UserInfo.FirstName + " " + UserInfo.LastName;

                SortedList sParams = new SortedList();
                sParams.Add("@ISkillID", hidRegistrationID.Value);

                DataTable dtSkillInfo = Classes.cUtilities.LoadDataTable("uspGetSubmittedISkills", sParams, "LARPortal", Master.UserName, lsRoutineName);
                DataTable dtComments = new DataTable();

                if (dtSkillInfo.Rows.Count > 0)
                {
                    lblRequest.Text = dtSkillInfo.Rows[0]["RequestText"].ToString();
                    lblEventInfo.Text = dtSkillInfo.Rows[0]["EventName"].ToString();
                    sParams = new SortedList();
                    sParams.Add("@ISkillID", hidRegistrationID.Value);
                    dtComments = Classes.cUtilities.LoadDataTable("uspGetISkillStaffComments", sParams, "LARPortal", Master.UserName, lsRoutineName + ".GetISkillComments");
                    rptQuestions.DataSource = dtComments;
                    rptQuestions.DataBind();
                }

            }
        }

        //protected void ProcessButton(object sender, CommandEventArgs e)
        //{
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        //protected void rptQuestions_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
