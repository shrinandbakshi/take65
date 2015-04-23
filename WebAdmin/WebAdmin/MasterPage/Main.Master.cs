using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.MasterPage
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigAdmin();
            CreateEventHandler();
            CheckLogin();
        }

        private void ConfigAdmin()
        {
            imgLogoAdmin.ImageUrl = "~/Img/logo-main.png";
            imgUserImage.ImageUrl = "~/Img/user.png";
            lblUserName.Text = "User Name";
            lblUserEmail.Text = "email@userdomain.com";
            lblTextFooter.Text = "Take65 | Panel Admin © " + DateTime.Now.Year;
            btnLogoff.Text = Resources.Language.Logoff;
        }

        private void CreateEventHandler()
        {
            btnLogoff.Click += new EventHandler(btnLogoff_Click);
        }

        private void CheckLogin()
        {
            if (Session["login"] != null)
            {
                lblUserEmail.Text = ((Layers.Admin.Model.SystemUser)Session["login"]).Email;
                lblUserName.Text = ((Layers.Admin.Model.SystemUser)Session["login"]).UserName;
                pnlUsuario.Visible = true;
            }
            else
            {
                pnlUsuario.Visible = false;
                if (Request.Url.Segments[Request.Url.Segments.Length - 1] != "Login.aspx")
                {
                    Response.Redirect("../Login/Login.aspx");
                }
            }
        }

        protected void btnLogoff_Click(object sender, EventArgs e)
        {
            Session["login"] = null;
            Response.Redirect("../Login/Login.aspx");
        }
    }
}