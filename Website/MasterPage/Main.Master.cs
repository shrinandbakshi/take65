using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.App_Start;

namespace Website.MasterPage
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected string languageJson = "{}";
        protected Model.User User = null;
        protected List<Model.Tag> CategoryList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["gstate"] = AntiForgeryToken.Instance.ReferenceToken;
            if (!string.IsNullOrEmpty(Request["PublicHomePageToken"]))
            {
                if (Session["user"] == null)
                {
                    Bll.User bllUser = new Bll.User();
                    Model.User userDefault = bllUser.Get(Convert.ToInt32(ConfigurationManager.AppSettings["Application.DefaultUserId"]));
                    if (userDefault != null){
                        Session["User"] = userDefault;
                        Session["IsEditingPublicHomePage"] = 1;
                        Response.Redirect("/");
                    }
                }
            }
            this.LoadCategory();

            if (Session["user"] != null)
            {
                try
                {
                    this.User = (Model.User)Session["user"];
                }
                catch { }
            }

            //btnLogout.Click += new EventHandler(btnLogout_Click);
            //btnLogout2.Click += new EventHandler(btnLogout_Click);
        }

        void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            this.User = null;
        }

        /// <summary>
        /// List the available categories to create a new widget (wagner.leonardi@netbiis.com)
        /// </summary>
        private void LoadCategory()
        {
            Bll.Tag bllTag = new Bll.Tag();
            try
            {
                this.CategoryList = bllTag.GetSystemTag(Model.Enum.enSystemTagType.CATEGORY);
            }
            catch { }

            // Prevent error
            if (this.CategoryList == null)
            {
                this.CategoryList = new List<Model.Tag>();
            }
        }// End LoadCategory

        protected bool IsEditingPublicHomePage()
        {
            if (Session["IsEditingPublicHomePage"] != null)
            {
                try
                {
                    return Convert.ToBoolean(Convert.ToInt16(Session["IsEditingPublicHomePage"]));
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }
    }
}
