using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class CharacterRulebookFinal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;
            string strCharacterID = Session["CharacterID"].ToString();
            string strCharacterSkillSetID = Session["CharacterSkillSetID"].ToString();

            DataTable dtCharacterRulebook = new DataTable();
            SortedList sParams = new SortedList();
            sParams.Add("@CharacterID", strCharacterID);
            sParams.Add("@CharacterSkillSetID", strCharacterSkillSetID);
            //DataSet dsCharacterRulebook = Classes.cUtilities.LoadDataSet("uspRptCharacterRulebook", sParams, "LARPortal", Session["UserName"].ToString(), lsRoutineName);
            //dtCharacterRulebook = dsCharacterRulebook.Tables[0];
            dtCharacterRulebook = Classes.cUtilities.LoadDataTable("uspRptCharacterRulebook", sParams, "LARPortal", Session["UserName"].ToString(), lsRoutineName);

            gvCharacterRulebook.DataSource = dtCharacterRulebook;
            gvCharacterRulebook.DataBind();


        }
    }
}
