using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAdmin.Page.Toolbox
{
    public partial class Upload_SimpleImage : System.Web.UI.Page
    {
        string nomeArquivo;
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateEventHandler();

            if (Request.QueryString["height"] != null && Request.QueryString["width"] != null)
            {
                wci1.Ratio = Request.QueryString["width"] + "/" + Request.QueryString["height"];
            }
            else
            {
                wci1.FixedAspectRatio = false;
            }
        }

        private void CreateEventHandler()
        {
            btnUpload.Click += new EventHandler(btnUpload_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NetBiis.Library.Validate.Data validateData = new NetBiis.Library.Validate.Data();
            NetBiis.Library.Validate.ReturnMessage validateReturnMessage = new NetBiis.Library.Validate.ReturnMessage();
            lblNome.Text = validateReturnMessage.RequiredField(txtNome.Text);
            lblDescricao.Text = validateReturnMessage.RequiredField(txtDescricao.Text);

            if (validateReturnMessage.IsResultValid == true)
            {
                string nomeArquivoTranformado = Guid.NewGuid().ToString().ToUpper();
                if (!string.IsNullOrEmpty(hfNameImge.Value) && (Request.QueryString["item"] != null || Request.QueryString["item"] != "0"))
                {
                    string sPath = ImagePath;
                    if (string.IsNullOrEmpty(sPath))
                    {
                        sPath = Server.MapPath("../../Img/Upload/Image/");
                    }
                    if (Request.QueryString["item"] == null || Request.QueryString["item"] == "0")
                    {
                        wci1.Crop(Server.MapPath("../../Img/Upload/Temp/" + hfNameImge.Value));

                        System.Drawing.Image vImagem;
                        vImagem = System.Drawing.Image.FromFile(Server.MapPath("../../Img/Upload/Temp/" + hfNameImge.Value));
                        vImagem = vImagem.GetThumbnailImage(100, 100, null, IntPtr.Zero);
                        
                        vImagem.Save(sPath + nomeArquivoTranformado + "_thumb" + hfExtensionImage.Value);

                        vImagem = System.Drawing.Image.FromFile(Server.MapPath("../../Img/Upload/Temp/" + hfNameImge.Value));
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[string.Format("Application.Upload.Image.{0}.Width", Request["Path"])]) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[string.Format("Application.Upload.Image.{0}.Height", Request["Path"])]))
                        {
                            vImagem = vImagem.GetThumbnailImage(Convert.ToInt32(ConfigurationManager.AppSettings[string.Format("Application.Upload.Image.{0}.Width", Request["Path"])]), Convert.ToInt32(ConfigurationManager.AppSettings[string.Format("Application.Upload.Image.{0}.Height", Request["Path"])]), null, IntPtr.Zero);
                        }
                        vImagem.Save(sPath + nomeArquivoTranformado + hfExtensionImage.Value);
                    }
                    else
                    {
                        hfNameImge.Value = null;
                    }

                    Layers.Admin.Bll.File file = new Layers.Admin.Bll.File();
                    file.Save(hfId.Value != "" ? Convert.ToInt32(hfId.Value) : 0, txtNome.Text, hfExtensionImage.Value != string.Empty ? hfExtensionImage.Value : null, !string.IsNullOrEmpty(hfNameImge.Value) ? nomeArquivoTranformado + hfExtensionImage.Value : null, txtDescricao.Text, null);
                    string teste = Request["imgSessionName"];
                    Session[Convert.ToString(Request["imgSessionName"])] = (!string.IsNullOrEmpty(hfNameImge.Value) ? nomeArquivoTranformado + hfExtensionImage.Value : null);

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "parent.$.colorbox.close();", true);
                }
                else
                {
                    lblImagem.Text = "* Please select your image.";
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fuImagem.PostedFile.FileName))
            {
                lblImagem.Text = "* Please select your image.";
            }
            else
            {
                string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg" };
                string ext = System.IO.Path.GetExtension(fuImagem.PostedFile.FileName).ToLower();
                bool isValidFile = false;
                for (int i = 0; i < validFileTypes.Length; i++)
                {
                    if (ext == "." + validFileTypes[i])
                    {
                        isValidFile = true;
                        break;
                    }
                }

                if (isValidFile)
                {
                    

                        if (!Directory.Exists(Server.MapPath("../../Img/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("../../Img/"));
                        }
                        if (!Directory.Exists(Server.MapPath("../../Img/Upload/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("../../Img/Upload/"));
                        }
                        if (!Directory.Exists(Server.MapPath("../../Img/Upload/Image")))
                        {
                            Directory.CreateDirectory(Server.MapPath("../../Img/Upload/Image"));
                        }
                        if (!Directory.Exists(Server.MapPath("../../Img/Upload/Temp")))
                        {
                            Directory.CreateDirectory(Server.MapPath("../../Img/Upload/Temp"));
                        }
                        if (!Directory.Exists(Server.MapPath("../../Img/Upload/Thumbnail")))
                        {
                            Directory.CreateDirectory(Server.MapPath("../../Img/Upload/Thumbnail"));
                        }
                    

                    hfNameImge.Value = nomeArquivo = fuImagem.PostedFile.FileName.Substring(fuImagem.PostedFile.FileName.LastIndexOf("\\") + 1).Replace("%20", " ");
                    fuImagem.SaveAs(Server.MapPath(("../../Img/upload/Temp/" + nomeArquivo)));
                    imgRed.ImageUrl = "../../Img/upload/Temp/" + nomeArquivo;
                    lblImagem.Text = "";
                    imgRed.Visible = true;
                    pnlUpload.Visible = false;
                    hfExtensionImage.Value = System.IO.Path.GetExtension(fuImagem.FileName).ToString();
                }
                else
                {
                    lblImagem.Text = "This file is not a valid image type.";
                }
            }
        }

        protected string ImagePath
        {
            get
            {
                string sPath = string.Empty;
                if (!string.IsNullOrEmpty(Request["Path"]))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[string.Format("Application.Upload.Image.{0}.Path", Request["Path"])]))
                    {
                        sPath = ConfigurationManager.AppSettings[string.Format("Application.Upload.Image.{0}.Path", Request["Path"])];
                    }
                }
                return sPath;
            }
        }
    }
}