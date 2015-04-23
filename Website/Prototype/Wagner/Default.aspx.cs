using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website.Prototype.Wagner
{
    public partial class Home : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.LoadPreferences();
            }

            this.btnRegister.Click += new EventHandler(btnRegister_Click);
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.fldLoginName.Text))
            {
                try
                {
                    Bll.User bllUser = new Bll.User();
                    Model.User user = bllUser.Get(this.fldLoginName.Text, Bll.Util.EncodeMD5(this.fldLoginPassword.Text));

                    if (user != null)
                    {
                        Session.Timeout = 120;
                        Session["User"] = user;
                        Response.Redirect("Home.aspx");
                    }
                    else
                    {
                        this.lblLoginError.Text = "Invalid user or password.";
                    }
                }
                catch (Exception error)
                {
                    this.lblLoginError.Text = "Login unavailable: " + error.Message;
                }
            }
            else
            {
                this.lblLoginError.Text = "Username is required.";
            }
        }

        void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                Bll.User bllUser = new Bll.User();
                Model.User user = new Model.User();

                user.Name = this.fldName.Text;
                user.Email = this.fldEmail.Text;

                Model.User userEmailCheck = bllUser.Get(user.Email);
                if (userEmailCheck == null)
                {
                    user.Password = Bll.Util.EncodeMD5(this.fldPassword.Text);
                    user.Id = bllUser.Save(user);

                    this.SavePreferences(user.Id);

                    Session.Timeout = 120;
                    Session["User"] = user;
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    this.lblRegisterError.Text = "This e-mail is already in use.";
                }
            }
            catch (Exception error)
            {
                this.lblRegisterError.Text = "Registration unavailable: " + error.Message;
            }
        }

        private void SavePreferences(long userId)
        {
            try
            {
                Bll.UserPreference bllUserPreference = new Bll.UserPreference();

                String[] preferences = this.hidPreference.Value.Split(',');
                for (int i = 0; i < preferences.Length; i++)
                {
                    bllUserPreference.Save(userId, Int32.Parse(preferences[i]));
                }
            }
            catch { }
        }

        private void LoadPreferences()
        {
            Bll.Tag bllTag = new Bll.Tag();
            List<Model.Tag> tagList = bllTag.GetSystemTag(Model.Enum.enSystemTagType.PREFERENCE);

            if (tagList != null)
            {
                rptPreference.DataSource = tagList;
                rptPreference.DataBind();
            }
        }
         
    }
}
