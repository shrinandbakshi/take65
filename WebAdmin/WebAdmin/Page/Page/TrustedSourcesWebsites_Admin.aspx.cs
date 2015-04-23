using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Page
{
    public partial class TrustedSourcesWebsites_Admin : System.Web.UI.Page
    {
        String ImageSessionName;
        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSessionName = "TrustedSourceImage";
            CreateEventHandler();

            if (!IsPostBack)
            {
                timerCheckLink.Interval = Int32.MaxValue;
                lblSubTitle.Text = Request.QueryString["Id"] != null ? "Edit" : "Add";
                lblTitle.Text = "Trusted Sources - Websites";
                
                Bll.Tag bllTag = new Bll.Tag();
                List<Model.Tag> ltTags = bllTag.GetSystemTag(Model.Enum.enSystemTagType.CATEGORY);

                ltTags = ltTags.OrderBy(x => x.Display).ToList();

                drpCategory.DataValueField = "Id";
                drpCategory.DataTextField = "Display";

                drpCategory.DataSource = ltTags;
                drpCategory.DataBind();

                
                if (Request.QueryString["Id"] != null)
                {
                    Model.TrustedSource oItem = new Bll.TrustedSource().GetTrustedSourceById(Convert.ToInt64(Request.QueryString["Id"]));
                    txtName.Text = oItem.Name;
                    txtUrl.Text = oItem.Url;
                    txtUploadImage.Text = oItem.Icon;
                    if (oItem.OpenIFrame){
                        drpOpenIframe.Items.FindByValue("1").Selected = true;
                    }else{
                        drpOpenIframe.Items.FindByValue("0").Selected = true;
                    }

                    if (drpCategory.Items.FindByValue(oItem.CurrentTag.Id.ToString()) != null)
                    {
                        drpCategory.Items.FindByValue(oItem.CurrentTag.Id.ToString()).Selected = true;
                    }

                    if (!string.IsNullOrEmpty(oItem.Icon))
                    {
                        imgPreview.ImageUrl = ConfigurationManager.AppSettings["Application.Upload.Image.Source"] + oItem.Icon;
                        imgPreview.Visible = true;
                    }
                    else
                    {
                        imgPreview.Visible = false;
                    }
                    

                    
                }
            }
            LoadVisibleGroup();
        }

        private void CreateEventHandler()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSelectImages.Click += new EventHandler(btnSelectImages_Click);
        }

        protected void btnSelectImages_Click(object sender, EventArgs e)
        {
            timerCheckLink.Interval = 1000;
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "$.colorbox({ iframe: true, innerWidth: 800, innerHeight: 550, href:'/Page/Toolbox/Upload_SimpleImage.aspx?path=Source&imgSessionName=" + ImageSessionName + "&height=377&width=1020' });", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NetBiis.Library.Validate.Data validateData = new NetBiis.Library.Validate.Data();
            NetBiis.Library.Validate.ReturnMessage validateReturnMessage = new NetBiis.Library.Validate.ReturnMessage();
            lblName.Text = validateReturnMessage.RequiredField(txtName.Text);
            lblUrl.Text = validateReturnMessage.RequiredField(txtUrl.Text);

            if (validateReturnMessage.IsResultValid == true)
            {

                Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
                Model.TrustedSource userObject = new Model.TrustedSource();
                

                if (Request.QueryString["Id"] != null && Request.QueryString["Id"] != "0")
                {
                    userObject.Id = Convert.ToInt32(Request.QueryString["Id"]);
                }
                userObject.Name = txtName.Text;
                userObject.Url = txtUrl.Text;
                userObject.OpenIFrame = Convert.ToBoolean(Convert.ToInt16(drpOpenIframe.SelectedValue));
                userObject.CurrentTag = new Model.Tag();
                userObject.CurrentTag.Id = Convert.ToInt32(drpCategory.SelectedValue);
                userObject.Icon = txtUploadImage.Text;
                userObject.TypeId = Convert.ToInt16(Model.Enum.enTrustedSourceType.BOOKMARK);

                long ObjectIdSavedId = bllTrustedSource.Save(userObject);
               
                Response.Redirect("TrustedSourcesWebsites.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrustedSourcesWebsites.aspx");
        }

        private void LoadVisibleGroup()
        {
        }

        protected void timerCheckLink_Tick(object sender, EventArgs e)
        {
            if (Session[ImageSessionName] != null)
            {
                txtUploadImage.Text = Convert.ToString(Session[ImageSessionName]);
                Session[ImageSessionName] = null;
                timerCheckLink.Interval = Int32.MaxValue;
            }
        }
    }
}