using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidgetFacebookPhotos
    /// </summary>
    public class UserWidgetFacebookPhotos : REST
    {

        protected override void Get(string parameter)
        {
            
            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                if (parameter.ToUpper() == "SOCIALMEDIAPHOTOFRAME") 
                {
                    Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                    List<Model.UserWidget> userWdgtList = bllUserWidget.GetUserWidget(user.Id).ToList();
                    List<Model.UserWidget> socialMediaWdgtList = userWdgtList.Where(x => x.Name == "Social Media Photos").ToList();
                    if (socialMediaWdgtList.Count > 0)
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = "Social Media Frame already added."
                        });
                    }
                    else
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "Social Media Frame not added."
                        });
                    }                   
                }
                else if (parameter.ToUpper() == "PREVIEWPHOTO") //New
                {
                    string photoUrl = this.GetParameterValue("u").ToString();
                    string type = string.Empty;
                    if (this.GetParameterValue("type") != null)
                    {
                        type = this.GetParameterValue("type").ToString();
                    }
                    string thumbDimension = this.GetParameterValue("p2").ToString();
                    try
                    {
                        
                        if (type.ToUpper() == "JSON")
                        {
                            var client = new WebClient();
                            string html = client.DownloadString(photoUrl);
                            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                            dynamic jsonPhotos =
                                   (object)json_serializer.DeserializeObject(html);
                            photoUrl = jsonPhotos["data"]["url"];
                        }

                        Uri uri = new Uri(photoUrl);

                        string fileName = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
                        string fileExtenstion = System.IO.Path.GetExtension(uri.LocalPath);

                        string initialFolder = fileName.Substring(0, 1);
                        string fileFolder = string.Empty;
                        string previewFolder = ConfigurationManager.AppSettings["Application.SocialMediaPhoto.PreviewFolder"];
                        if (previewFolder.IndexOf("{MapPath}") != -1)
                        {
                            previewFolder = previewFolder.Replace("{MapPath}", HttpContext.Current.Server.MapPath("../"));
                        }
                        fileFolder = previewFolder + user.Id.ToString() + "\\" + initialFolder;



                        if (!System.IO.Directory.Exists(fileFolder))
                            System.IO.Directory.CreateDirectory(fileFolder);

                        string pathNewFileName = fileFolder + "\\" + fileName + "_" + thumbDimension + fileExtenstion;
                        string newFileName = fileName + "_" + thumbDimension + fileExtenstion;

                        if (!System.IO.File.Exists(pathNewFileName))
                        {
                            string[] arrDimensions = thumbDimension.Split(new string[] { "x" }, StringSplitOptions.None);

                            using (System.Drawing.Image tempImage = DownloadImage(photoUrl))
                            {
                                using (System.Drawing.Image imageScale = Bll.Util.ScaleImage(tempImage, Convert.ToInt32(arrDimensions[0]), Convert.ToInt32(arrDimensions[1])))
                                {
                                    imageScale.Save(pathNewFileName);
                                }
                            }
                        }
                        HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["Application.SocialMediaPhoto.PreviewUrl"] + user.Id.ToString() + "/" + initialFolder + "/" + newFileName);

                    }
                    catch
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "Invalid image source"
                        });
                    }

                }else if (parameter.ToUpper() == "GETTOKEN") // new function
                {
                    if (!string.IsNullOrEmpty(user.FacebookTokenLongLived))
                    {
                        Bll.FacebookCustom bllFacebook = new Bll.FacebookCustom(Facebook.FacebookApplication.Current.AppId, Facebook.FacebookApplication.Current.AppSecret);
                        if (bllFacebook.IsValidToken(user.Id, user.FacebookTokenLongLived))
                        {
                            this.Response<Model.REST.Response>(new Model.REST.Response()
                            {
                                status = true,
                                response = user.FacebookTokenLongLived
                            });
                        }
                        else
                        {
                            this.Response<Model.REST.Response>(new Model.REST.Response()
                            {
                                status = false,
                                response = "Invalid or expired Facebook Token"
                            });
                        }
                    }
                    else
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "No Facebook Token"
                        });
                    }
                }
                else if (parameter.ToUpper() == "USERFRIENDS") //New
                {
                    Bll.User bllUser = new Bll.User();
                    List<Model.FacebookProfile> ltFacebookFriends = bllUser.GetFacebookFriends(user.Id);
                    List<Model.REST.FacebookProfile> ltFriends = new List<Model.REST.FacebookProfile>();
                    if (ltFacebookFriends != null)
                    {
                        foreach (Model.FacebookProfile userFriend in ltFacebookFriends)
                        {
                            ltFriends.Add(new Model.REST.FacebookProfile
                            {
                                id = userFriend.Id,
                                name = userFriend.Name,
                                photoCount = userFriend.PhotoCount
                            });
                        }
                    }
                    this.Response<List<Model.REST.FacebookProfile>>(ltFriends);
                }
                else if (parameter.ToUpper() == "FACEBOOKFRIENDS")
                {
                    Bll.FacebookCustom bllFacebook = new Bll.FacebookCustom(Facebook.FacebookApplication.Current.AppId, Facebook.FacebookApplication.Current.AppSecret, user.FacebookTokenLongLived);

                    List<Model.FacebookProfile> ltFacebookFriends = bllFacebook.GetFacebookFriends();
                    List<Model.REST.FacebookProfile> ltFriends = new List<Model.REST.FacebookProfile>();
                    if (ltFacebookFriends != null)
                    {
                        foreach (Model.FacebookProfile userFriend in ltFacebookFriends)
                        {
                            ltFriends.Add(new Model.REST.FacebookProfile
                            {
                                id = userFriend.Id,
                                name = userFriend.Name,
                                photoCount = userFriend.PhotoCount
                            });
                        }
                    }
                    this.Response<List<Model.REST.FacebookProfile>>(ltFriends);
                }
                else if (parameter.ToUpper() == "FACEBOOKPHOTOS")
                {
                    string facebookFriendId = this.GetParameterValue("p2").ToString();
                    if (!string.IsNullOrEmpty(facebookFriendId))
                    {
                        Bll.FacebookCustom bllFacebook = new Bll.FacebookCustom(Facebook.FacebookApplication.Current.AppId, Facebook.FacebookApplication.Current.AppSecret, user.FacebookTokenLongLived);
                        List<Model.FacebookPhoto> ltFacebookPhotos = bllFacebook.GetPhotos(facebookFriendId);

                        List<Model.REST.FacebookPhoto> ltPhotos = new List<Model.REST.FacebookPhoto>();

                        foreach (Model.FacebookPhoto photo in ltFacebookPhotos)
                        {
                            try
                            {
                                Model.REST.FacebookPhoto newPhoto = new Model.REST.FacebookPhoto();

                                newPhoto.id = photo.Id;

                                newPhoto.name = photo.Name;


                                newPhoto.thumb = photo.Thumb;
                                newPhoto.photo = HttpUtility.UrlEncode(photo.Photo);
                                newPhoto.width = photo.Width;
                                newPhoto.height = photo.Height;
                                newPhoto.createdTime = Bll.Util.RelativeTime(photo.CreatedTime);
                                ltPhotos.Add(newPhoto);
                            }
                            catch
                            {

                            }
                        }

                        this.Response<List<Model.REST.FacebookPhoto>>(ltPhotos);
                    }
                    else
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "No Facebook Id was provided"
                        });
                    }


                }



            }
            else
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "User not logged"
                });
            }
        }

        protected override void Get()
        {
            throw new NotImplementedException();
        }

        protected override void Post(string parameter)
        {
            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                if (parameter.ToUpper() == "SAVEFRIENDS")
                {
                    Bll.User bllUser = new Bll.User();

                    Model.REST.FacebookProfile[] restUser = this.ReadJsonRequest<Model.REST.FacebookProfile[]>();
                    bllUser.SaveFaebookFriends(user.Id, restUser.ToList().Select(x => new Model.FacebookProfile
                    {
                        Id = x.id,
                        Name = x.name,
                        PhotoCount = x.photoCount
                    }).ToList());
                    /*
                    long facebookFriendId = Int64.Parse(this.GetParameterValue("p2"));
                    bllUser.SetFacebookFriends(user.Id, facebookFriendId);
                    */
                    //this.Response<Model.REST.Response>(new Model.REST.Response()
                    //{
                    //    status = true,
                    //    response = "OK"
                    //});
                    CreateSocialMediaWidget();
                }
                else if (parameter.ToUpper() == "SETTOKEN")
                {
                    Model.REST.FacebookUser restFbUser = this.ReadJsonRequest<Model.REST.FacebookUser>();
                    if (!string.IsNullOrEmpty(restFbUser.facebookToken))
                    {
                        Bll.FacebookCustom bllFacebook = new Bll.FacebookCustom(Facebook.FacebookApplication.Current.AppId, Facebook.FacebookApplication.Current.AppSecret);
                        string FacebookToken = bllFacebook.GetTokenLongLive(user.Id, restFbUser.facebookToken);
                        user.FacebookTokenLongLived = FacebookToken;
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = user.FacebookTokenLongLived
                        });
                    }
                    else
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "No token was informed"
                        });
                    }
                }
            }
            else
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "User not logged"
                });
            }
        }
        #region CreateSocialMediaWidget
        /// <summary>
        /// Method used to create Social Media Widget.
        /// </summary>
        private void CreateSocialMediaWidget()
        {
            Model.User user = this.GetSessionUser();
            if(user != null)
            {
                bool blnCreateFrame = false;
                Bll.UserWidget bllUserWidget = new Bll.UserWidget();

                Model.UserWidget[] ltWidgets = bllUserWidget.GetUserWidget(user.Id, 0);
                if (ltWidgets != null)
                {
                    if (ltWidgets.ToList().Where(x => x.SystemTagId == Convert.ToInt16(Model.Enum.enWidgetType.FACEBOOKPHOTOS)).Count() > 0)
                        blnCreateFrame = false;
                    else
                        blnCreateFrame = true;
                }
                else
                    blnCreateFrame = true;

                if (blnCreateFrame)
                {
                    long userWidgetId = bllUserWidget.Save(new Model.UserWidget()
                    {
                        Name = "Social Media Photos",
                        Size = 3,
                        SystemTagId = (int)Model.Enum.enWidgetType.FACEBOOKPHOTOS,
                        UserId = user.Id
                    });

                    List<Model.UserWidget> widgetList = bllUserWidget.GetUserWidget(user.Id, 0).ToList();
                    widgetList = bllUserWidget.OrderWidgets(widgetList);
                    Model.UserWidget userNewWidget = widgetList.ToList().Where(x => x.Id == userWidgetId).First();

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ModelUserWidgetToRESTModel(userNewWidget))
                    });
                }
                else
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "User already have Facebook Photos Frame"
                    });
                }
            }
            else
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "User not logged"
                });
            }
        }
        #endregion

        protected override void Post()
        {
            CreateSocialMediaWidget();
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        public Model.REST.UserWidget ModelUserWidgetToRESTModel(Model.UserWidget model)
        {
            Model.REST.UserWidget restModel = new Model.REST.UserWidget();
            restModel.id = model.Id;
            restModel.title = model.Name;
            restModel.typeId = model.SystemTagId;
            restModel.typeName = Bll.Util.EnumToDescription((Model.Enum.enWidgetType)model.SystemTagId);
            restModel.col = model.Col;
            restModel.row = model.Row;
            restModel.isDeletable = true;

            if (!String.IsNullOrEmpty(model.Category))
            {
                String[] category = model.Category.Split(',');
                restModel.categoryId = new int[category.Length];
                for (int i = 0; i < category.Length; i++)
                {
                    restModel.categoryId[i] = Int32.Parse(category[i]);
                }
            }

            restModel.size = model.Size;

            return restModel;
        }

        private System.Drawing.Image DownloadImage(string pUrl)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Check that the remote file was found. The ContentType
                // check is performed since a request for a non-existent
                // image file might be redirected to a 404-page, which would
                // yield the StatusCode "OK", even though the image was not
                // found.
                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    string[] urlFile = pUrl.Split(new string[] { "." }, StringSplitOptions.None);
                    
                    // if the remote file was found, download oit
                    using (System.IO.Stream inputStream = response.GetResponseStream())
                    {
                        System.Drawing.Image ImgToSave = System.Drawing.Image.FromStream(inputStream);
                        return ImgToSave;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404") || ex.Message.Contains("400"))
                {
                    return null;
                }
                else
                    throw ex;
            }
        }
    }
}