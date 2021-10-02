using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal.Reports
{
    public partial class ProfilePics : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SortedList Params = new SortedList();
            DataTable dtPics = Classes.cUtilities.LoadDataTable("uspGetProfilePictures", Params, "LARPortal", "Master", "");
            rptrImages.DataSource = dtPics;
            rptrImages.DataBind();
            //gvImages.DataSource = dtPics;
            //gvImages.DataBind();
        }
    }
}