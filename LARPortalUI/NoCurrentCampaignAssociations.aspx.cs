using System;

public partial class NoCurrentCampaignAssociations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("NewUserSelectCampaign.aspx", true);
    }
}