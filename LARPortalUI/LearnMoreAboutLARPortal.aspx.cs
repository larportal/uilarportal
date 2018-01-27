using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
    public partial class LearnMoreAboutLARPortal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Classes.cLogin LearnMore = new Classes.cLogin();
            LearnMore.getLearnMore();
            lblWhatIsLARPortal.Text = LearnMore.LearnMoreText;
        }
    }
}