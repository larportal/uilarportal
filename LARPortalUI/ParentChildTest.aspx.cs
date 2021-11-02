using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace LarpPortal.ParentChildTest
{
    public partial class ParentChildTest : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvUsers.DataSource = GetData("select top 10 * from MDBUsers WHERE DateDeleted is NULL");
                gvUsers.DataBind();
            }
        }

        private static DataTable GetData(string query)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["LARPortal"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string currentUserId = gvUsers.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView gvCharacters = e.Row.FindControl("gvCharacters") as GridView;
                gvCharacters.DataSource = GetData(string.Format("select top 10 * from CHCharacters where CurrentUserID='{0}' ORDER BY CharacterAKA", currentUserId));
                gvCharacters.DataBind();
            }
        }
    }
}