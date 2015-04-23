using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website.Prototype.Wagner
{
    public partial class Home1 : System.Web.UI.Page
    {
        /*
        protected Model.User UserLogged = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try{
                if (Session["User"] != null)
                {
                    this.UserLogged = (Model.User)Session["User"];
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }catch{
                Response.Redirect("Default.aspx");
            }

            this.LoadUserWidget(this.UserLogged.Id);
        }

        private void LoadUserWidget(long userId)
        {
            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            Model.UserWidget[] WidgetList = bllUserWidget.GetUserWidget(userId);

            if (WidgetList != null && WidgetList.Length > 0)
            {
                this.rptWidget.DataSource = WidgetList;
                this.rptWidget.DataBind();
            }


        }
        */
    }
}