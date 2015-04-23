using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Page
{
    public partial class RelatedInterests_Admin : System.Web.UI.Page
    {
        String ImageSessionName;
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateEventHandler();

            if (!IsPostBack)
            {
                timerCheckLink.Interval = Int32.MaxValue;
                lblSubTitle.Text = Request.QueryString["SystemTagId"] != null ? "Editar" : "Adicionar";

                lblTitle.Text = "Related Interests";

                if (Request.QueryString["SystemTagId"] != null)
                {
                    Layers.Admin.Model.SystemTag systemTag = new Layers.Admin.Bll.SystemTag().Get(Convert.ToInt32(Request.QueryString["SystemTagId"]));
                    txtName.Text = systemTag.Display;
                    txtIcon.Text = systemTag.Icon;
                }
            }
        }

        private void CreateEventHandler()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSelectImages.Click += new EventHandler(btnSelectImages_Click);
        }

        protected void btnSelectImages_Click(object sender, EventArgs e)
        {
            ImageSessionName = "SystemTagIcon";
            timerCheckLink.Interval = 1000;
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "$.colorbox({ iframe: true, innerWidth: 800, innerHeight: 550, href:'/Page/Toolbox/Upload_SimpleImage.aspx?imgSessionName=" + ImageSessionName + "&height=377&width=1020' });", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NetBiis.Library.Validate.Data validateData = new NetBiis.Library.Validate.Data();
            NetBiis.Library.Validate.ReturnMessage validateReturnMessage = new NetBiis.Library.Validate.ReturnMessage();
            lblName.Text = validateReturnMessage.RequiredField(txtName.Text);

            if (validateReturnMessage.IsResultValid == true)
            {
                int item = 0;
                if (Request.QueryString["SystemTagId"] != null && Request.QueryString["SystemTagId"] != "0")
                {
                    item = Convert.ToInt32(Request.QueryString["SystemTagId"]);
                }

                Layers.Admin.Bll.SystemTag bllSystemTag = new Layers.Admin.Bll.SystemTag();
                bllSystemTag.Save(Convert.ToInt32(item), null, null, txtName.Text, txtIcon.Text, 0, 2, null);
                Response.Redirect("RelatedInterests.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("RelatedInterests.aspx");
        }

        protected void timerCheckLink_Tick(object sender, EventArgs e)
        {
            if (Session["SystemTagIcon"] != null)
            {
                txtIcon.Text = Convert.ToString(Session["SystemTagIcon"]);
                Session["SystemTagIcon"] = null;
                timerCheckLink.Interval = Int32.MaxValue;
            }
        }
    }
}