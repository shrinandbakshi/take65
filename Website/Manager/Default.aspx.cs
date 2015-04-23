using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Website.Manager
{
    public partial class Default : System.Web.UI.Page
    {
        Bll.TrustedSource bllTrustedSouce = new Bll.TrustedSource();
        Bll.FeedContent bllFeedContent = new Bll.FeedContent();

        protected string JsEnd = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Model.TrustedSource[] LtSources = bllTrustedSouce.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED, 0);
            rptMenu.DataSource = LtSources;
            rptMenu.DataBind();

            rptContentDiv.DataSource = LtSources;
            rptContentDiv.ItemDataBound += new RepeaterItemEventHandler(rptContentDiv_ItemDataBound);
            rptContentDiv.DataBind();

            
            
        }

        void rptContentDiv_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Model.TrustedSource Source = (Model.TrustedSource)e.Item.DataItem;
                Model.FeedContents AllContent = bllFeedContent.GetContent(Source.Id);

                Repeater rptContent = (Repeater)e.Item.FindControl("rptContent");
                rptContent.DataSource = AllContent.FeedContentList;
                rptContent.DataBind();

                JsEnd = JsEnd + string.Format("SetTotalNews('{0}', '{1}');", Source.Name.Replace(" ", ""), AllContent.TotalResults);
                
            }
        }


        protected string RemoveHtmlTags(string pContent)
        {
            pContent = Regex.Replace(pContent, "<.*?>", string.Empty);
            return pContent;
        }
    }
}