using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Controls
{
    public partial class PlayVideo : System.Web.UI.UserControl
    {
        public int VideoID = 0;
        public string VideoDescription = "";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            if (!IsPostBack)
            {
                if (VideoID != 0)
                {
                    SortedList sParams = new SortedList();
                    DataTable dtVideoList = Classes.cUtilities.LoadDataTable("uspGetVideos", sParams, "LARPortal", "PlayVideo", lsRoutineName + ".GetVideos");
                    DataView dvVideo = new DataView(dtVideoList, "ID = " + VideoID.ToString(), "", DataViewRowState.CurrentRows);
                    if (dvVideo.Count > 0)
                        hlVideo.NavigateUrl = dvVideo[0]["YouTubeLink"].ToString();
                }
                if (VideoDescription.Length > 0)
                    lblText.Text = " " + VideoDescription;
            }
        }
    }
}