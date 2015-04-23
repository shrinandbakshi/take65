using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Page
{
    public partial class SuggestionBox_Admin : System.Web.UI.Page
    {
        String ImageSessionName;
        protected void Page_Load(object sender, EventArgs e)
        {
            ImageSessionName = "SuggestionBoxImage";

            CreateEventHandler();

            if (!IsPostBack)
            {
                timerCheckLink.Interval = Int32.MaxValue;
                lblSubTitle.Text = Request.QueryString["SuggestionBoxId"] != null ? "Edit" : "Add";
                lblTitle.Text = "Suggestion Box";

                cblRelatedInterests.DataSource = new Layers.Admin.Bll.SystemTag().Get(0, null, 0, 2, 0).SystemTag;
                cblRelatedInterests.DataValueField = "Id";
                cblRelatedInterests.DataTextField = "Display";
                cblRelatedInterests.DataBind();

                if (Request.QueryString["SuggestionBoxId"] != null)
                {
                    Model.SuggestionBox oSuggestionBox = new Bll.SuggestionBox().GetById(Convert.ToInt32(Request.QueryString["SuggestionBoxId"]));
                    txtSuggestionBoxName.Text = oSuggestionBox.Name;
                    txtSuggestionBoxUrl.Text = oSuggestionBox.Url;
                    txtSuggestionBoxDescription.Text = oSuggestionBox.Description;
                    txtUploadSuggestionBoxImage.Text = oSuggestionBox.Image;

                    if (!string.IsNullOrEmpty(oSuggestionBox.Image))
                    {
                        imgPreview.ImageUrl = ConfigurationManager.AppSettings["Application.Upload.Image.SuggestionBox"] + oSuggestionBox.Image;
                        imgPreview.Visible = true;
                    }
                    else
                    {
                        imgPreview.Visible = false;
                    }

                    Model.SuggestionBoxTagList oSuggestionBoxTag = new Bll.SuggestionBoxTag().Get(Convert.ToInt32(Request.QueryString["SuggestionBoxId"]));

                    if (cblRelatedInterests.Items != null && cblRelatedInterests.Items.Count > 0)
                    {
                        foreach (ListItem item in cblRelatedInterests.Items)
                        {
                            if (oSuggestionBoxTag != null && oSuggestionBoxTag.SuggestionBoxTag.Count > 0)
                            {
                                foreach (Model.SuggestionBoxTag suggestionBoxTag in oSuggestionBoxTag.SuggestionBoxTag)
                                {
                                    if (suggestionBoxTag.SystemTagId.ToString() == item.Value)
                                    {
                                        item.Selected = true;
                                    }
                                }
                            }
                        }
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
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "$.colorbox({ iframe: true, innerWidth: 800, innerHeight: 550, href:'/Page/Toolbox/Upload_SimpleImage.aspx?path=SuggestionBox&imgSessionName=" + ImageSessionName + "&height=377&width=1020' });", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NetBiis.Library.Validate.Data validateData = new NetBiis.Library.Validate.Data();
            NetBiis.Library.Validate.ReturnMessage validateReturnMessage = new NetBiis.Library.Validate.ReturnMessage();
            lblName.Text = validateReturnMessage.RequiredField(txtSuggestionBoxName.Text);
            lblUrl.Text = validateReturnMessage.RequiredField(txtSuggestionBoxUrl.Text);

            if (validateReturnMessage.IsResultValid == true)
            {
                Bll.SuggestionBox bllSuggestionBox = new Bll.SuggestionBox();
                Model.SuggestionBox userSuggestion = new Model.SuggestionBox();
                Bll.SuggestionBoxTag bllSuggestionBoxTag = new Bll.SuggestionBoxTag();
                List<Model.SystemTag> oSystemTagList = new List<Model.SystemTag>();
                int item = 0;
                if (Request.QueryString["SuggestionBoxId"] != null && Request.QueryString["SuggestionBoxId"] != "0")
                {
                    item = Convert.ToInt32(Request.QueryString["SuggestionBoxId"]);
                    userSuggestion.Id = item;
                }


                userSuggestion.Name = txtSuggestionBoxName.Text;
                userSuggestion.Url = txtSuggestionBoxUrl.Text;
                userSuggestion.Description = txtSuggestionBoxDescription.Text;
                userSuggestion.Image = txtUploadSuggestionBoxImage.Text;
                foreach (ListItem RelatedInterest in cblRelatedInterests.Items)
                {
                    if (RelatedInterest.Selected)
                    {
                        Model.SystemTag oSystemTag = new Model.SystemTag();
                        oSystemTag.Id = Convert.ToInt32(RelatedInterest.Value);
                        oSystemTagList.Add(oSystemTag);
                    }
                }



                userSuggestion.SystemTagList = oSystemTagList;
                long oSuggestionId = bllSuggestionBox.Save(userSuggestion);

                bllSuggestionBoxTag.DeleteBySuggestionId(Convert.ToInt32(oSuggestionId));

                foreach (Model.SystemTag itemSystemTag in userSuggestion.SystemTagList)
                {
                    bllSuggestionBoxTag.Save(Convert.ToInt32(oSuggestionId), itemSystemTag.Id);
                }

                Response.Redirect("SuggestionBox.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SuggestionBox.aspx");
        }

        private void LoadVisibleGroup()
        {
        }

        protected void timerCheckLink_Tick(object sender, EventArgs e)
        {
            if (Session[ImageSessionName] != null)
            {
                txtUploadSuggestionBoxImage.Text = Convert.ToString(Session[ImageSessionName]);
                Session[ImageSessionName] = null;
                timerCheckLink.Interval = Int32.MaxValue;
            }
        }
    }
}