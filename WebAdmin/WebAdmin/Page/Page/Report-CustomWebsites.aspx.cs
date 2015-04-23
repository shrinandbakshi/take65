using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Page
{
    public partial class Report_CustomWebsites : System.Web.UI.Page
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
                lblTitle.Text = "Custom Websites";
            }
            CreateEventHandler();
        }

        private void LoadData()
        {
            Bll.SafeWebsite bllSafeWebsite = new Bll.SafeWebsite();
            List<Model.SafeWebsite> ltSafeWebsite = bllSafeWebsite.GetUnmapped();

            if (ltSafeWebsite != null)
            {

                var orderGroups =
                    from p in ltSafeWebsite
                    group p by p.Url into g
                    select new Model.SafeWebsiteUnMapped { Url = g.Key, TimesUsed = g.Count() };

                gdvAdmin.DataSource = orderGroups.ToList().OrderByDescending(x=> x.TimesUsed).ToList();
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
            btnAdd.Click += new EventHandler(btnAdd_Click);
        }

        protected void btnGoTo_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void btnBefore_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //Response.Redirect("SafeWebsites_Admin.aspx");
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
                    Bll.SafeWebsite bllSafeWebsite = new Bll.SafeWebsite();
                    Model.SafeWebsite userObject = new Model.SafeWebsite();
                    userObject.Id = Convert.ToInt32(lblid.Text);
                    userObject.deleted = DateTime.Now;
                    bllSafeWebsite.Save(userObject);
                }
            }
            Response.Redirect("SafeWebsites.aspx");
        }

        protected void gdvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                e.Row.Attributes.Add("onmouseover", "this.style.textDecoration='underline';");
                e.Row.Attributes.Add("onmouseout", "this.style.textDecoration='none';");
                for (int index = 1; index <= gdvAdmin.Columns.Count - 1; index++)
                {
                    e.Row.Cells[index].Attributes.Add("style", "cursor:pointer");
                    string paginaAtual = Request.CurrentExecutionFilePath.ToString().Remove(0, Request.CurrentExecutionFilePath.ToString().LastIndexOf("/") + 1);

                    //e.Row.Cells[index].Attributes.Add("onclick", "window.open('" + paginaAtual.Replace(".aspx", "") + "_Admin.aspx?Id=" + ((Label)e.Row.Cells[1].FindControl("lblID")).Text + "', '_self');");
                }
            }
        }
 
    }
}