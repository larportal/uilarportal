using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports.FifthGate
{
    public partial class FifthGateCharacterSkills : System.Web.UI.Page
    {
        //string _UserName = "";
        //int _UserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["SecurityRole"] == null)
            //{
            //    Response.Redirect("~/login.aspx");
            //}
            //Session["ActiveLeftNav"] = "N/A";
            //if (Session["Username"] != null)
            //    _UserName = Session["Username"].ToString();
            //if (Session["UserID"] != null)
            //    int.TryParse(Session["UserID"].ToString(), out _UserID);
            BuildCharacterSkillTable();
        }

        private void BuildCharacterSkillTable()
        {
            int CharacterSkillRowCounter = 1;
            int iTemp = 0;
            string CharacterAKA = "";
            string FirstName = "";
            string LastName = "";
            string World = "";
            string DescriptorValue = "";
            string SkillName = "";
            int SkillTypeID = 0;
            int SkillID = 0;
            string OrderOrigin = "";
            string AttributeDesc = "";
            string TeamName = "";
            string dq = "\"";
            string TableCode = "<table id=" + dq + "events-table" + dq + "class=" + dq + "table table-striped table-bordered table-hover table-condensed" + dq;
            TableCode = TableCode + " data-toggle=" + dq + "table" + dq + " data-height=" + dq + "250" + dq + " data-sort-name=" + dq;
            TableCode = TableCode + " date-earned" + dq + " data-sort-order=" + dq + " desc" + dq + " data-click-to-select=" + dq + "true" + dq;
            TableCode = TableCode + " data-select-item-name=" + dq + "total-point" + dq + "><tr><td>";
            Classes.cAdminViews Chars5G = new Classes.cAdminViews();
            DataTable dtChars5G = new DataTable();
            dtChars5G = Chars5G.FifthGateCharacterSkillList();
            if (Chars5G.CharacterSkillCount == 0)
            {
                // Build table with no records
                lblCharacterSkills.Text = TableCode + "There are no Fifth Gate character skills</td></tr></table>";
            }
            else
            {
                foreach (DataRow dRow in dtChars5G.Rows)
                {
                    if (CharacterSkillRowCounter == 1)
                    {
                        // Build the header row
                        TableCode = TableCode + "CharacterAKA</td><td>FirstName</td><td>LastName</td><td>World</td><td>Team</td><td>SkillTypeID</td>";
                        TableCode = TableCode + "<td>Order Origin</td><td>Skill</td><td>Attribute</td><td>DescriptorValue</td><td>SkillID</td></tr>";
                    }
                    CharacterAKA = dRow["CharacterAKA"].ToString();
                    TableCode = TableCode + "<td>" + CharacterAKA + "</td>";
                    FirstName = dRow["FirstName"].ToString();
                    TableCode = TableCode + "<td>" + FirstName + "</td>";
                    LastName = dRow["LastName"].ToString();
                    TableCode = TableCode + "<td>" + LastName + "</td>";
                    World = dRow["World"].ToString();
                    TableCode = TableCode + "<td>" + World + "</td>";
                    TeamName = dRow["Team"].ToString();
                    TableCode = TableCode + "<td>" + TeamName + "</td>";
                    if (int.TryParse(dRow["SkillTypeID"].ToString(), out iTemp))
                        SkillTypeID = iTemp;
                    TableCode = TableCode + "<td>" + SkillTypeID + "</td>";
                    OrderOrigin = dRow["OrderOrigin"].ToString();
                    TableCode = TableCode + "<td>" + OrderOrigin + "</td>";
                    SkillName = dRow["SkillName"].ToString();
                    TableCode = TableCode + "<td>" + SkillName + "</td>";
                    AttributeDesc = dRow["AttributeDesc"].ToString();
                    TableCode = TableCode + "<td>" + AttributeDesc + "</td>";
                    DescriptorValue = dRow["DescriptorValue"].ToString();
                    TableCode = TableCode + "<td>" + DescriptorValue + "</td>";
                    if (int.TryParse(dRow["SkillID"].ToString(), out iTemp))
                        SkillID = iTemp;
                    TableCode = TableCode + "<td>" + SkillID + "</td></tr>";
                    CharacterSkillRowCounter++;
                }
                // Build the table close
                lblCharacterSkills.Text = TableCode + "</table>";
            }
        }
    }
}
