using System;
using System.Data;
using System.Reflection;
using System.Web.UI;

using LarpPortal.Classes;

namespace LarpPortal.Profile
{
    public partial class MedicalInfo : System.Web.UI.Page
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

            btnCloseMedical.Attributes.Add("data-dismiss", "modal");
            btnCancelDeleteMedical.Attributes.Add("data-dismiss", "modal");
            btnCloseLimit.Attributes.Add("data-dismiss", "modal");
            btnCancelDeleteLimit.Attributes.Add("data-dismiss", "modal");

            if (!IsPostBack)
            {
                lblModalMessage.Text = "";
            }

            tbMedicalComments.Attributes.Add("PlaceHolder", "Add any comments about your health that others may find useful.");
            tbAllergies.Attributes.Add("PlaceHolder", "Add any allergies someone may need to know. Make sure to include both food and medicine allergies.");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            Classes.cPlayer PlayerInfo = new cPlayer(Master.UserID, Master.UserName);
            hidPlayerProfileID.Value = PlayerInfo.PlayerProfileID.ToString();

            DataTable dtMedical = cUtilities.CreateDataTable(PlayerInfo.PlayerMedical);
            if (dtMedical.Columns["JavaScriptEdit"] == null)
                dtMedical.Columns.Add("JavaScriptEdit", typeof(string));
            if (dtMedical.Columns["JavaScriptDelete"] == null)
                dtMedical.Columns.Add("JavaScriptDelete", typeof(string));

            foreach (DataRow dRow in dtMedical.Rows)
            {
                DateTime? StartDate;
                DateTime? EndDate;

                DateTime dtTemp;

                if (DateTime.TryParse(dRow["StartDate"].ToString(), out dtTemp))
                    StartDate = dtTemp;
                else
                    StartDate = null;
                if (DateTime.TryParse(dRow["EndDate"].ToString(), out dtTemp))
                    EndDate = dtTemp;
                else
                    EndDate = null;

                dRow["JavaScriptEdit"] = string.Format("openMedical({0}, \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\"); return false;",
                    dRow["PlayerMedicalID"].ToString(), cUtilities.ReplaceQuotes(dRow["Description"]), cUtilities.ReplaceQuotes(dRow["Medication"]),
                    dRow["ShareInfo"].ToString(), dRow["PrintOnCard"].ToString(),
                    (StartDate.HasValue ? StartDate.Value.ToShortDateString() : ""),
                    (EndDate.HasValue ? EndDate.Value.ToShortDateString() : ""));
                dRow["JavaScriptDelete"] = string.Format("openMedicalDelete({0}, \"{1}\", \"{2}\"); return false;",
                    dRow["PlayerMedicalID"].ToString(), cUtilities.ReplaceQuotes(dRow["Description"]), cUtilities.ReplaceQuotes(dRow["Medication"]));
            }

            gvMedical.DataSource = dtMedical;
            gvMedical.DataBind();

            DataTable dtLimitations = cUtilities.CreateDataTable(PlayerInfo.PlayerLimitation);
            if (dtLimitations.Columns["JavaScriptEdit"] == null)
                dtLimitations.Columns.Add("JavaScriptEdit", typeof(string));
            if (dtLimitations.Columns["JavaScriptDelete"] == null)
                dtLimitations.Columns.Add("JavaScriptDelete", typeof(string));

            foreach (DataRow dRow in dtLimitations.Rows)
            {
                DateTime? StartDate;
                DateTime? EndDate;
                DateTime dtTemp;

                if (DateTime.TryParse(dRow["StartDate"].ToString(), out dtTemp))
                    StartDate = dtTemp;
                else
                    StartDate = null;
                if (DateTime.TryParse(dRow["EndDate"].ToString(), out dtTemp))
                    EndDate = dtTemp;
                else
                    EndDate = null;

                dRow["JavaScriptEdit"] = string.Format("openLimitations({0}, \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\"); return false;",
                    dRow["PlayerLimitationID"].ToString(), cUtilities.ReplaceQuotes(dRow["Description"]), dRow["ShareInfo"].ToString(),
                    dRow["PrintOnCard"].ToString(),
                    (StartDate.HasValue ? StartDate.Value.ToShortDateString() : ""),
                    (EndDate.HasValue ? EndDate.Value.ToShortDateString() : ""));
                dRow["JavaScriptDelete"] = string.Format("openLimitationsDelete({0}, \"{1}\"); return false;",
                    dRow["PlayerLimitationID"].ToString(), cUtilities.ReplaceQuotes(dRow["Description"]));
            }

            gvLimitations.DataSource = dtLimitations;
            gvLimitations.DataBind();

            tbMedicalComments.Text = PlayerInfo.MedicalComments;
            tbAllergies.Text = PlayerInfo.Allergies;
        }

        protected void btnSaveComments_Click(object sender, EventArgs e)
        {
            Classes.cPlayer PlayerInfo = new cPlayer(Master.UserID, Master.UserName);
            PlayerInfo.MedicalComments = tbMedicalComments.Text;
            PlayerInfo.Allergies = tbAllergies.Text;
            PlayerInfo.Save();
            lblModalMessage.Text = "Your information has been saved.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
        }

        protected void btnSaveLimit_Click(object sender, EventArgs e)
        {
            int iPlayerLimitID;
            int iPlayerProfileID;
            if ((int.TryParse(hidPlayerLimitID.Value, out iPlayerLimitID)) &&
                (int.TryParse(hidPlayerProfileID.Value, out iPlayerProfileID)))
            {
                Classes.cPlayerLimitation PlayerLimit = new cPlayerLimitation(iPlayerLimitID, Master.UserName);
                PlayerLimit.Description = tbLimitation.Text;
                PlayerLimit.ShareInfo = cbxLimitShareWithStaff.Checked;
                PlayerLimit.PrintOnCard = cbxLimitPrintOnCard.Checked;

                DateTime dtTemp;
                if (DateTime.TryParse(tbLimitationStartDate.Text, out dtTemp))
                    PlayerLimit.StartDate = dtTemp;
                else
                    PlayerLimit.EndDate = null;
                if (DateTime.TryParse(tbLimitationEndDate.Text, out dtTemp))
                    PlayerLimit.EndDate = dtTemp;
                else
                    PlayerLimit.EndDate = null;
                PlayerLimit.PlayerProfileID = iPlayerProfileID;
                PlayerLimit.Save(Master.UserName, Master.UserID);
            }
        }

        protected void btnDeleteLimit_Click(object sender, EventArgs e)
        {
            int iDeleteLimitID;
            if (int.TryParse(hidDeleteLimitID.Value, out iDeleteLimitID))
            {
                cPlayerLimitation PlayerLimit = new cPlayerLimitation(iDeleteLimitID, Master.UserName);
                PlayerLimit.RecordStatus = RecordStatuses.Delete;
                PlayerLimit.Delete(Master.UserName, Master.UserID);
                lblModalMessage.Text = "Your record has been deleted.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
            }
        }

        protected void btnSaveMedical_Click(object sender, EventArgs e)
        {
            int iPlayerMedicalID;
            int iPlayerProfileID;
            if ((int.TryParse(hidPlayerMedicalID.Value, out iPlayerMedicalID)) &&
                (int.TryParse(hidPlayerProfileID.Value, out iPlayerProfileID)))
            {
                Classes.cPlayerMedical PlayerMedical = new cPlayerMedical(iPlayerMedicalID, Master.UserName);
                PlayerMedical.PlayerMedicalID = iPlayerMedicalID;
                PlayerMedical.Medication = tbMedication.Text;
                PlayerMedical.Description = tbCondition.Text;
                PlayerMedical.PlayerProfileID = iPlayerProfileID;
                if (cbxPrintOnCard.Checked)
                    PlayerMedical.PrintOnCard = true;
                else
                    PlayerMedical.PrintOnCard = false;
                if (cbxShareWithStaff.Checked)
                    PlayerMedical.ShareInfo = true;
                else
                    PlayerMedical.ShareInfo = false;
                DateTime dtTemp;
                if (DateTime.TryParse(tbMedicalStartDate.Text, out dtTemp))
                    PlayerMedical.StartDate = dtTemp;
                else
                    PlayerMedical.StartDate = null;
                if (DateTime.TryParse(tbMedicalEndDate.Text, out dtTemp))
                    PlayerMedical.EndDate = dtTemp;
                else
                    PlayerMedical.EndDate = null;

                PlayerMedical.Save(Master.UserName, Master.UserID);
            }
        }

        protected void btnDeleteMedical_Click(object sender, EventArgs e)
        {
            int iDeleteMedicalID;
            if (int.TryParse(hidDeleteMedicalID.Value, out iDeleteMedicalID))
            {
                cPlayerMedical PlayerMedical = new cPlayerMedical(iDeleteMedicalID, Master.UserName);
                PlayerMedical.RecordStatus = RecordStatuses.Delete;
                PlayerMedical.Delete(Master.UserName, Master.UserID);
                lblModalMessage.Text = "Your medical record has been deleted.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalMessage();", true);
            }
        }
    }
}