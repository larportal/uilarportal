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
    public partial class PELDisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int PELSearchID;
                string Keyword1 = "";
                if (Session["Keyword"] != null)
                    Keyword1 = Session["Keyword"].ToString();
                string DetailedDescription = "";
                string TableCode = "";
                if ((Request.QueryString["PELID"] == null))
                {
                    PELSearchID = 0;  // Set a default if we somehow get here with a NULL ID
                }
                else
                {
                    int.TryParse(Request.QueryString["PELID"].ToString(), out PELSearchID);

                }
                string stStoredProc = "uspGetWholePEL";
                string stCallingMethod = "PELDisplay.aspx.Page_PreRender";
                DataSet dsWholePEL = new DataSet();
                SortedList sParams = new SortedList();
                sParams.Add("@Keyword1", Keyword1);
                sParams.Add("@PELID", PELSearchID);
                dsWholePEL = Classes.cUtilities.LoadDataSet(stStoredProc, sParams, "LARPortal", Session["UserName"].ToString(), stCallingMethod);
                dsWholePEL.Tables[0].TableName = "PEL";
                TableCode = "<table>";
                foreach (DataRow dRow in dsWholePEL.Tables["PEL"].Rows)
                {
                    DetailedDescription = dRow["PEL"].ToString();
                    DetailedDescription = DetailedDescription.ToString().Replace(Environment.NewLine, "<br>");
                    if(dRow["ColumnSort1"].ToString() == "0")
                    {
                        lblPanelHeader.Text = "PEL" + " - " + DetailedDescription;
                    }
                    
                    TableCode = TableCode + "<tr><td>" + DetailedDescription + "</td></tr>";
                }
                TableCode = TableCode +  "</Table>";
                lblWhatsNewDetail.Text = TableCode;
                //txtPEL.Text = TableCode;
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