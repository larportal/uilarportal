using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
    public partial class HowToVideos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Classes.cUtilities cUtilities = new Classes.cUtilities();
            DataTable dtVideos = new DataTable();
            SortedList sParams = new SortedList();
            dtVideos = Classes.cUtilities.LoadDataTable("uspGetVideos", sParams, "LARPortal", "", "");

            DataView dvChar = new DataView(dtVideos, "Module = 'Character'", "ID", DataViewRowState.CurrentRows);
            gvChar.DataSource = dvChar;
            gvChar.DataBind();

            DataView dvPlayer = new DataView(dtVideos, "Module = 'Player'", "ID", DataViewRowState.CurrentRows);
            gvPlayer.DataSource = dvPlayer;
            gvPlayer.DataBind();

            DataView dvEvent = new DataView(dtVideos, "Module = 'Event'", "ID", DataViewRowState.CurrentRows);
            gvEvent.DataSource = dvEvent;
            gvEvent.DataBind();

            DataView dvPoint = new DataView(dtVideos, "Module = 'Points'", "ID", DataViewRowState.CurrentRows);
            gvPoints.DataSource = dvPoint;
            gvPoints.DataBind();
        }
    }
}