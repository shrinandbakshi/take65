using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Page
{
    public partial class RegisteredUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadPostBack();
        }

        private void LoadPostBack()
        {
            if (!IsPostBack)
            {
                LoadData();

                lblSubTitle.Text = "User Signed Up";
                lblTitle.Text = "Users List";
            }
        }

        private void LoadData()
        {
            Bll.User bllUser = new Bll.User();
            List<Model.User> lUsers = bllUser.GetAllRegisteredUsers();
            //if (lUsers != null)
            //{
            //    lUsers = lUsers.Where(x => x.Id != Convert.ToInt32(ConfigurationManager.AppSettings["Application.DefaultUserId"])).ToList();
            //}

            if (lUsers != null)
            {
                lblTotalUsers.Text = lUsers.Count.ToString();
                gdvUserAdmin.DataSource = lUsers;
                gdvUserAdmin.DataBind();
                pnlItemsNotFound.Visible = false;
                gdvUserAdmin.Visible = true;
            }
            else
            {
                pnlItemsNotFound.Visible = true;
                gdvUserAdmin.Visible = false;
            }
        }
    }
}