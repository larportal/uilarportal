using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Collections;
using System.IO;
using System.Data;
using System.Reflection;





using LarpPortal.Classes;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        string sUserName = Session["UserName"].ToString();
        int iUserID;
        int.TryParse(Session["UserID"].ToString(), out iUserID);

        cPlayer player = new cPlayer(iUserID, sUserName);
        if (player.HomeScreen.ToUpper().StartsWith("D"))
            Response.Redirect("/Dashboard.aspx", true);
    }

    protected void lbtnDashboard_Click(object sender, EventArgs e)
    {
        string sUserName = Session["UserName"].ToString();
        int iUserID;
        int.TryParse(Session["UserID"].ToString(), out iUserID);

        cPlayer player = new cPlayer(iUserID, sUserName);
        player.HomeScreen = "Dashboard";
        player.Save();
        Response.Redirect("/Dashboard.aspx", true);
    }
}