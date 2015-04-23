using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Page
{
    public partial class SafeWebsites_Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateEventHandler();

            if (!IsPostBack)
            {
                lblSubTitle.Text = Request.QueryString["Id"] != null ? "Edit" : "Add";
                lblTitle.Text = "Safe Websites";

                Bll.Tag bllTag = new Bll.Tag();
                List<Model.Tag> ltTags = bllTag.GetSystemTag(Model.Enum.enSystemTagType.CATEGORY);
                ltTags = ltTags.OrderBy(x => x.Display).ToList();

                drpCategory.DataValueField = "Id";
                drpCategory.DataTextField = "Display";
                drpCategory.DataSource = ltTags;
                drpCategory.DataBind();

                if (Request.QueryString["Id"] != null)
                {
                    Model.SafeWebsite oItem = new Bll.SafeWebsite().Get(Convert.ToInt32(Request.QueryString["Id"]));

                    txtUrl.Text = oItem.Url;
                    if (oItem.OpenIFrame)
                    {
                        drpOpenIframe.Items.FindByValue("1").Selected = true;
                    }
                    else
                    {
                        drpOpenIframe.Items.FindByValue("0").Selected = true;
                    }

                    if (oItem.CurrentTag != null && drpCategory.Items.FindByValue(oItem.CurrentTag.Id.ToString()) != null)
                    {
                        drpCategory.Items.FindByValue(oItem.CurrentTag.Id.ToString()).Selected = true;
                    }
                }
            }
        }

        private void CreateEventHandler()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NetBiis.Library.Validate.Data validateData = new NetBiis.Library.Validate.Data();
            NetBiis.Library.Validate.ReturnMessage validateReturnMessage = new NetBiis.Library.Validate.ReturnMessage();
            lblUrl.Text = validateReturnMessage.RequiredField(txtUrl.Text);

            if (validateReturnMessage.IsResultValid == true)
            {
                Bll.SafeWebsite bllSafeWebsite = new Bll.SafeWebsite();
                Model.SafeWebsite userObject = new Model.SafeWebsite();

                if (Request.QueryString["Id"] != null && Request.QueryString["Id"] != "0")
                {
                    userObject.Id = Convert.ToInt32(Request.QueryString["Id"]);
                }
                userObject.Url = txtUrl.Text;
                userObject.OpenIFrame = Convert.ToBoolean(Convert.ToInt16(drpOpenIframe.SelectedValue));
                userObject.CurrentTag = new Model.Tag();
                userObject.CurrentTag.Id = Convert.ToInt32(drpCategory.SelectedValue);

                long ObjectIdSavedId = bllSafeWebsite.Save(userObject);

                Response.Redirect("SafeWebsites.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SafeWebsites.aspx");
        }
    }
}