using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LarpPortal.Profile
{
    public partial class LARPResume : System.Web.UI.Page
    {
        //protected int _UserID = 0;
        //protected //string _UserName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            //if (Session["Username"] != null)
            //    _UserName = Session["Username"].ToString();
            //if (Session["UserID"] != null)
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);

            btnCancelDelete.Attributes.Add("data-dismiss", "modal");
            if (!IsPostBack)
            {
                lblModalMessage.Text = "";
            }

            tbLARPResumeComments.Attributes.Add("PlaceHolder", "Add any comments others may find useful.");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            //Session["ActiveLeftNav"] = "Demographics";

            if (!IsPostBack)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand CmdStyle = new SqlCommand("select * from CMStyles where DateDeleted is null", conn))
                    {
                        SqlDataAdapter SDAStyle = new SqlDataAdapter(CmdStyle);
                        DataTable dtGameStyle = new DataTable();
                        SDAStyle.Fill(dtGameStyle);
                        ddlStyle.DataSource = dtGameStyle;
                        ddlStyle.DataTextField = "StyleName";
                        ddlStyle.DataValueField = "StyleID";
                        ddlStyle.DataBind();
                    }

                    using (SqlCommand CmdGenre = new SqlCommand("select * from CMGenres where DateDeleted is null", conn))
                    {
                        SqlDataAdapter SDAGenre = new SqlDataAdapter(CmdGenre);
                        DataTable dtGenre = new DataTable();
                        SDAGenre.Fill(dtGenre);
                        ddlGenre.DataSource = dtGenre;
                        ddlGenre.DataTextField = "GenreName";
                        ddlGenre.DataValueField = "GenreID";
                        ddlGenre.DataBind();
                    }

                    using (SqlCommand CmdRoleAlignment = new SqlCommand("select * from MDBRoleAlignments where DateDeleted is null", conn))
                    {
                        SqlDataAdapter SDARoles = new SqlDataAdapter(CmdRoleAlignment);
                        DataTable dtRoles = new DataTable();
                        SDARoles.Fill(dtRoles);
                        ddlRole.DataSource = dtRoles;
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "RoleAlignmentID";
                        ddlRole.DataBind();
                    }
                }
            }

            Classes.cPlayer PlayerInfo = new Classes.cPlayer(Master.UserID, Master.UserName);
            hidPlayerProfileID.Value = PlayerInfo.PlayerProfileID.ToString();
            
            var ResumeList = PlayerInfo.PlayerLARPResumes.OrderByDescending(x => x.StartDate);
            gvResumeItems.DataSource = ResumeList;
            gvResumeItems.DataBind();

            tbLARPResumeComments.Text = PlayerInfo.LARPResumeComments;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Classes.cPlayerLARPResume cResumeItem = new Classes.cPlayerLARPResume();

            int iResumeItemID;
            int iGameStyleID;
            int iGenreID;
            int iRoleID;
            int iPlayerProfileID;

            if ((int.TryParse(hidPlayerLARPResumeID.Value, out iResumeItemID)) &&
                (int.TryParse(ddlStyle.SelectedValue, out iGameStyleID)) &&
                (int.TryParse(ddlGenre.SelectedValue, out iGenreID)) &&
                (int.TryParse(ddlRole.SelectedValue, out iRoleID)) &&
                (int.TryParse(hidPlayerProfileID.Value, out iPlayerProfileID)))
            {
                cResumeItem.PlayerLARPResumeID = iResumeItemID;
                cResumeItem.StyleID = iGameStyleID;
                cResumeItem.GenreID = iGenreID;
                cResumeItem.RoleID = iRoleID;
                cResumeItem.PlayerProfileID = iPlayerProfileID;

                DateTime dtTemp;
                if (DateTime.TryParse(tbStartDate.Text, out dtTemp))
                    cResumeItem.StartDate = dtTemp;
                if (DateTime.TryParse(tbEndDate.Text, out dtTemp))
                    cResumeItem.EndDate = dtTemp;

                cResumeItem.GameSystem = tbGameSystem.Text;
                cResumeItem.Campaign = tbCampaign.Text;
                cResumeItem.AuthorGM = tbAuthorGM.Text;
                cResumeItem.PlayerComments = tbComments.Text;

                cResumeItem.Save(Master.UserName, Master.UserID);

                lblModalMessage.Text = "Your resume item has been saved.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
            }
        }

        protected void btnDeleteResumeItem_Click(object sender, EventArgs e)
        {
            int iDeleteResumeItemID;
            if (int.TryParse(hidDeleteResumeItemID.Value, out iDeleteResumeItemID))
            {
                Classes.cPlayerLARPResume cResumeItem = new Classes.cPlayerLARPResume();
                cResumeItem.PlayerLARPResumeID = iDeleteResumeItemID;

                cResumeItem.Delete(Master.UserName, Master.UserID);

                lblModalMessage.Text = "Your resume item has been deleted.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
            }
        }

        protected void btnSaveComments_Click(object sender, EventArgs e)
        {
            Classes.cPlayer PlayerInfo = new Classes.cPlayer(Master.UserID, Master.UserName);
            PlayerInfo.LARPResumeComments = tbLARPResumeComments.Text;
            PlayerInfo.Save();
            lblModalMessage.Text = "Your comments have been saved.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
        }
    }
}