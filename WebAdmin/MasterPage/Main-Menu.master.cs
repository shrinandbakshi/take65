using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.MasterPage
{
    public partial class Main_Menu : System.Web.UI.MasterPage
    {
        protected String CurrentPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateEventHandler();
            LoadPage();
            LoadMenu();
            
        }
        private void LoadPage()
        {
            CurrentPage = Request.Url.PathAndQuery;

            Layers.Admin.Bll.PageAdmin bllPageAdmin = new Layers.Admin.Bll.PageAdmin();
            Layers.Admin.Model.PageAdmin oPagesAdmin = new Layers.Admin.Model.PageAdmin();
            oPagesAdmin = bllPageAdmin.Get(CurrentPage);
            if (oPagesAdmin != null)
            {
                CurrentPage = oPagesAdmin.Link;
            }
        }
        private void CreateEventHandler()
        {
            rptMainMenuFirstLevel.ItemDataBound += new RepeaterItemEventHandler(rptMainMenuFirstLevel_ItemDataBound);
        }
        private void LoadMenu()
        {
            Layers.Admin.Bll.PageAdmin bllPageAdmin = new Layers.Admin.Bll.PageAdmin();
            Layers.Admin.Model.PageAdminList oPagesAdmin = new Layers.Admin.Model.PageAdminList();
            oPagesAdmin = bllPageAdmin.Get();
            if (oPagesAdmin != null)
            {
                rptMainMenuFirstLevel.DataSource = oPagesAdmin.PageAdmin;
                rptMainMenuFirstLevel.DataBind();
            }
        }
        protected void rptMainMenuFirstLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptMainMenuSecondLevel = (Repeater)e.Item.FindControl("rptMainMenuSecondLevel");

                if (((Layers.Admin.Model.PageAdminList)(DataBinder.Eval(e.Item.DataItem, "PageAdminList"))) != null && ((Layers.Admin.Model.PageAdminList)(DataBinder.Eval(e.Item.DataItem, "PageAdminList"))).PageAdmin.Count > 0)
                {
                    //rptMainMenuSecondLevel.ItemDataBound += new RepeaterItemEventHandler(rptMainMenuSecondLevel_ItemDataBound);
                    rptMainMenuSecondLevel.DataSource = ((Layers.Admin.Model.PageAdminList)(DataBinder.Eval(e.Item.DataItem, "PageAdminList"))).PageAdmin;
                    rptMainMenuSecondLevel.DataBind();
                }
            }
        }
        protected void rptMainMenuSecondLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptMainMenuThirdLevel = (Repeater)e.Item.FindControl("rptMainMenuThirdLevel");
                if (((List<Layers.Admin.Model.Menu>)(DataBinder.Eval(e.Item.DataItem, "SubMenu"))).Count > 0)
                {
                    rptMainMenuThirdLevel.DataSource = ((List<Layers.Admin.Model.Menu>)(DataBinder.Eval(e.Item.DataItem, "SubMenu")));
                    rptMainMenuThirdLevel.DataBind();
                }
            }
        }
    }
}