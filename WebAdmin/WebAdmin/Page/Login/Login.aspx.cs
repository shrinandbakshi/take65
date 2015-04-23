using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Login
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateEventHandler();
            LoadPage();
        }

        private void CreateEventHandler()
        {
            btnLogin.Click += new EventHandler(btnLogin_Click);
        }

        private void LoadPage()
        {
            lblTitle.Text = "Login";
            lblSubTitle.Text = "Take65";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtSenha.Text, "SHA1");
            Layers.Admin.Model.SystemUser systemUser = new Layers.Admin.Bll.SystemUser().AuthenticateSystemUser(txtLogin.Text.ToLower(), password);
            if (systemUser == null)
            {
                lblAvisoLogin.Text = "Invalid User/Password.";
            }
            else
            {
                Session["login"] = systemUser;
                Response.Redirect("../Page/HomeAdmin.aspx");
            }
        }
    }
}