using System;
using System.Data;

namespace LarpPortal.Reports.FifthGate
{
    public partial class FifthGateCharacters : System.Web.UI.Page
    {
        //string _UserName = "";
        //int _UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["SecurityRole"] == null)
            //{
            //    Response.Redirect("~/login.aspx");
            //}
            //if (Session["Username"] != null)
            //    _UserName = Session["Username"].ToString();
            //if (Session["UserID"] != null)
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);
            BuildCharacterTable();
        }

        private void BuildCharacterTable()
        {
            int CharacterRowCounter = 1;
            int iTemp = 0;
            double dTemp;
            string CharacterAKA = "";
            string TeamName = "";
            string CharacterFirstName = "";
            string CharacterMiddleName = "";
            string CharacterLastName = "";
            int PlotLeadPerson = 0;
            string FirstName = "";
            string LastName = "";
            string DateOfBirth = "";
            string WhereFrom = "";
            string CurrentHome = "";
            double TotalCP = 0;
            string PlayerComments = "";
            int CharacterSkillSetID = 0;
            string dq = "\"";
            string TableCode = "<table id=" + dq + "events-table" + dq + "class=" + dq + "table table-striped table-bordered table-hover table-condensed" + dq;
            TableCode = TableCode + " data-toggle=" + dq + "table" + dq + " data-height=" + dq + "250" + dq + " data-sort-name=" + dq;
            TableCode = TableCode + " date-earned" + dq + " data-sort-order=" + dq + " desc" + dq + " data-click-to-select=" + dq + "true" + dq;
            TableCode = TableCode + " data-select-item-name=" + dq + "total-point" + dq + "><tr><td>";
            Classes.cAdminViews Chars5G = new Classes.cAdminViews();
            DataTable dtChars5G = new DataTable();
            dtChars5G = Chars5G.FifthGateCharacterList();
            if (Chars5G.CharacterCount == 0)
            {
                // Build table with no records
                lblCharacters.Text = TableCode + "There are no Fifth Gate characters</td></tr></table>";
            }
            else
            {
                foreach (DataRow dRow in dtChars5G.Rows)
                {
                    if (CharacterRowCounter == 1)
                    {
                        // Build the header row
                        TableCode = TableCode + "CharacterAKA</td><td>TeamName</td><td>CharacterFirstName</td><td>CharacterMiddleName</td><td>CharacterLastName</td>";
                        TableCode = TableCode + "<td>PlotLeadPerson</td><td>FirstName</td><td>LastName</td><td>DateOfBirth</td><td>WhereFrom</td>";
                        TableCode = TableCode + "<td>CurrentHome</td><td>TotalCP</td><td>PlayerComments</td><td>CharacterSkillSetID</td></tr>";
                    }
                    CharacterAKA = dRow["CharacterAKA"].ToString();
                    TableCode = TableCode + "<td>" + CharacterAKA + "</td>";
                    TeamName = dRow["TeamName"].ToString();
                    TableCode = TableCode + "<td>" + TeamName + "</td>";
                    CharacterFirstName = dRow["CharacterFirstName"].ToString();
                    TableCode = TableCode + "<td>" + CharacterFirstName + "</td>";
                    CharacterMiddleName = dRow["CharacterMiddleName"].ToString();
                    TableCode = TableCode + "<td>" + CharacterMiddleName + "</td>";
                    CharacterLastName = dRow["CharacterLastName"].ToString();
                    TableCode = TableCode + "<td>" + CharacterLastName + "</td>";
                    if (int.TryParse(dRow["PlotLeadPerson"].ToString(), out iTemp))
                        PlotLeadPerson = iTemp;
                    TableCode = TableCode + "<td>" + PlotLeadPerson + "</td>";
                    FirstName = dRow["FirstName"].ToString();
                    TableCode = TableCode + "<td>" + FirstName + "</td>";
                    LastName = dRow["LastName"].ToString();
                    TableCode = TableCode + "<td>" + LastName + "</td>";
                    DateOfBirth = dRow["DateOfBirth"].ToString();
                    TableCode = TableCode + "<td>" + DateOfBirth + "</td>";
                    WhereFrom = dRow["WhereFrom"].ToString();
                    TableCode = TableCode + "<td>" + WhereFrom + "</td>";
                    CurrentHome = dRow["CurrentHome"].ToString();
                    TableCode = TableCode + "<td>" + CurrentHome + "</td>";
                    if (double.TryParse(dRow["TotalCP"].ToString(), out dTemp))
                        TotalCP = dTemp;
                    TableCode = TableCode + "<td>" + TotalCP + "</td>";
                    PlayerComments = dRow["PlayerComments"].ToString();
                    TableCode = TableCode + "<td>" + PlayerComments + "</td>";
                    if (int.TryParse(dRow["CharacterSkillSetID"].ToString(), out iTemp))
                        CharacterSkillSetID = iTemp;
                    TableCode = TableCode + "<td>" + CharacterSkillSetID + "</td></tr>";
                    CharacterRowCounter++;
                }
                // Build the table close
                lblCharacters.Text = TableCode + "</table>";
            }
        }

        protected void btnExportcsv_Click(object sender, EventArgs e)
        {

        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

        }
    }
}