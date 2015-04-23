using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Page
{
    public partial class Report_Users : System.Web.UI.Page
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

                lblSubTitle.Text = "Manage";
                lblTitle.Text = "Users";
            }
            CreateEventHandler();
        }

        private void LoadData()
        {
            Bll.User bllUser = new Bll.User();
            List<Model.User> lUsers = bllUser.GetAll();
            if (lUsers != null)
            {
                lUsers = lUsers.Where(x => x.Id != Convert.ToInt32(ConfigurationManager.AppSettings["Application.DefaultUserId"])).ToList();
            }

            if (lUsers != null)
            {
                lblTotalUsers.Text = lUsers.Count.ToString();
                gdvAdmin.DataSource = lUsers;
                gdvAdmin.DataBind();
                pnlItemsNotFound.Visible = false;
                gdvAdmin.Visible = true;
            }
            else
            {
                pnlItemsNotFound.Visible = true;
                gdvAdmin.Visible = false;
            }
        }

        private void CreateEventHandler()
        {
            btnGoTo.Click += new ImageClickEventHandler(btnGoTo_Click);
            btnBefore.Click += new ImageClickEventHandler(btnBefore_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        protected void btnGoTo_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void btnBefore_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("");
        }

     
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            for (i = 0; i <= gdvAdmin.Rows.Count - 1; i++)
            {
                GridViewRow dgItem = gdvAdmin.Rows[i];
                Label lblid = (Label)dgItem.FindControl("lblID");
                CheckBox cb = (CheckBox)dgItem.FindControl("chkButton");
                if (cb.Checked)
                {
                    Bll.User bllUser = new Bll.User();
                    Model.User userObject = new Model.User();
                    userObject.Id = Convert.ToInt32(lblid.Text);
                    userObject.Deleted = DateTime.Now;
                    bllUser.Save(userObject);
                }
            }
            Response.Redirect("Report-Users.aspx");
        }

        protected void gdvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
            }
        }
    }
}