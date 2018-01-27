using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LarpPortal
{
    public partial class FAQs : System.Web.UI.Page
    {
        DataSet _dsFAQs = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MethodBase lmth = MethodBase.GetCurrentMethod();
            string lsRoutineName = lmth.DeclaringType + "." + lmth.Name;

            SortedList sParams = new SortedList();
            _dsFAQs = Classes.cUtilities.LoadDataSet("uspGetFAQCategories", sParams, "LARPortal", "Guest", lsRoutineName);

            rptrFAQ.DataSource = _dsFAQs.Tables[0];
            rptrFAQ.DataBind();
        }


        protected void rptrFAQ_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:

                    DataRowView dRow = (DataRowView)e.Item.DataItem;
                    DataView dvQuestions = new DataView(_dsFAQs.Tables[1], "FAQCategoryID = " + dRow["FAQCategoryID"].ToString(), "DisplayOrder", DataViewRowState.CurrentRows);

                    string sQuestions = "";
                    foreach (DataRowView dQuestion in dvQuestions)
                    {
                        sQuestions += @"<div class=""panel panel-default"">" +
                            @"<div class=""panel-heading"">" +
                            @"<h4 class=""panel-title""><a data-toggle=""collapse"" data-parent=""#accordion" + dRow["FAQCategoryID"].ToString() + @""" href=""#collapseInner" +
                                dQuestion["FAQID"].ToString() + @""">" + dQuestion["Question"].ToString() + "</a></h4>" +
                            "</div>" +
                            @"<div id=""collapseInner" + dQuestion["FAQID"].ToString() + @""" class=""panel-collapse collapse"">" +
                            @"<div class=""panel-body"">" +
                            dQuestion["Answer"].ToString() +
                            @"</div></div></div>";
                    }

                    Label lblMorePanels = (Label)e.Item.FindControl("lblMorePanels");
                    if ((sQuestions.Length > 0) &&
                        (lblMorePanels != null))
                    {
                        lblMorePanels.Text = sQuestions;
                    }
                    break;
            }






            //<div class="panel panel-default">
            //    <div class="panel-heading">
            //        <h4 class="panel-title"><a data-toggle="collapse" data-parent="#accordion2" href="#collapseInnerOne">Collapsible Inner Group Item #1
            //        </a></h4>
            //    </div>
            //    <div id="collapseInnerOne" class="panel-collapse collapse in">
            //        <div class="panel-body">
            //            Anim pariatur cliche...
            //        </div>
            //    </div>
            //</div

        }
    }
}