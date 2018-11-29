using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

namespace LarpPortal.Reports
{
    public partial class HistoryDisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int HistorySearchID;
                string Keyword1 = "";
                if (Session["Keyword"] != null)
                    Keyword1 = Session["Keyword"].ToString();
                string DetailedDescription = "";
                string TableCode = "";
                if ((Request.QueryString["HistoryID"] == null))
                {
                    HistorySearchID = 0;  // Set a default if we somehow get here with a NULL ID
                }
                else
                {
                    int.TryParse(Request.QueryString["HistoryID"].ToString(), out HistorySearchID);

                }
                string stStoredProc = "uspGetWholeHistory";
                string stCallingMethod = "HistoryDisplay.aspx.Page_PreRender";
                DataSet dsWholeHistory = new DataSet();
                SortedList sParams = new SortedList();
                sParams.Add("@Keyword1", Keyword1);
                sParams.Add("@CharacterID", HistorySearchID);
                dsWholeHistory = Classes.cUtilities.LoadDataSet(stStoredProc, sParams, "LARPortal", Session["UserName"].ToString(), stCallingMethod);
                dsWholeHistory.Tables[0].TableName = "History";
                TableCode = "<table>";
                foreach (DataRow dRow in dsWholeHistory.Tables["History"].Rows)
                {
                    DetailedDescription = dRow["History"].ToString();
                    DetailedDescription = DetailedDescription.ToString().Replace(Environment.NewLine, "<br>");
                    if (dRow["ColumnSort1"].ToString() == "0" && dRow["ColumnSort2"].ToString() == "0")
                    {
                        lblPanelHeader.Text = "History" + " - " + DetailedDescription;
                    }

                    TableCode = TableCode + "<tr><td>" + DetailedDescription + "</td></tr>";
                }
                TableCode = TableCode + "</Table>";
                lblWhatsNewDetail.Text = TableCode;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }

        protected void btnCloseTop_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }

    }
}