using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace LarpPortal
{
    public partial class NoCurrentCampaignAssociations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("NewUserSelectCampaign.aspx", true);
        }
    }
}