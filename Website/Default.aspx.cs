using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected readonly int DEFAUT_USER_ID = Convert.ToInt32(ConfigurationManager.AppSettings["Application.DefaultUserId"]);
        protected bool defaultHome = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                defaultHome = true;
                if (Request.Cookies["Take65.User"] != null)
                {
                    if (!string.IsNullOrEmpty(Request.Cookies["Take65.User"].Value.ToString()))
                    {
                        Bll.User bllUser = new Bll.User();
                        Model.User user = bllUser.GetByGUID(Request.Cookies["Take65.User"].Value.ToString());
                        if (user != null)
                        {
                            Session["User"] = user;
                            Response.Redirect("/");
                        }

                    }
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PreLoad_init", "_page = { trustedSource : {}, widgets : {}, suggestions : {}, userWidgetBookmark : [], userWidgetFeed : [] };", true);
            UserWidgetCategory();
            List<Model.REST.UserWidget> userWidgets = UserWidget();
            if (userWidgets != null)
            {
                foreach (Model.REST.UserWidget userWidgetREST in userWidgets)
                {
                    Model.Enum.enWidgetType widgetType = (Model.Enum.enWidgetType)userWidgetREST.typeId;
                    if (widgetType == Model.Enum.enWidgetType.BOOKMARK)
                        UserWidgetBookmark(userWidgetREST.id);
                    else if (widgetType == Model.Enum.enWidgetType.FEED)
                        UserWidgetFeedContent(userWidgetREST.id);
                }
            }
            SuggestionBox();

        }

        protected void UserWidgetCategory()
        {
            Model.User user = GetSessionUser();
            long userId = this.DEFAUT_USER_ID;
            if (user != null)
                userId = user.Id;

            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            Model.Tag[] tagList = bllUserWidget.GetUserWidgetCategory(userId);

            if (tagList == null) tagList = new Model.Tag[0];
            List<Model.REST.Category> restModelList = new List<Model.REST.Category>();

            if (tagList != null)
            {
                for (int i = 0; i < tagList.Length; i++)
                {
                    Model.REST.Category restModel = new Model.REST.Category();
                    restModel.id = tagList[i].Id;
                    restModel.title = tagList[i].Display;
                    restModel.image = tagList[i].Icon;
                    restModelList.Add(restModel);
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UserWidgetCategory_init", "_page.trustedSource = " + (new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(restModelList)) + ";", true);
        }

        protected void UserWidgetBookmark(long widgetId)
        {
            Model.REST.WidgetBookmarkList bookmarkList;
            if (this.GetSessionUser() == null)
            {
                if (HttpRuntime.Cache["UserWidgetBookmark." + widgetId.ToString()] == null)
                {
                    bookmarkList = UserWidgetBookmarkGetItems(widgetId);
                    HttpRuntime.Cache.Insert(("UserWidgetBookmark." + widgetId.ToString()), bookmarkList, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    bookmarkList = (Model.REST.WidgetBookmarkList)HttpRuntime.Cache["UserWidgetBookmark." + widgetId.ToString()];
                }
            }
            else
            {
                bookmarkList = UserWidgetBookmarkGetItems(widgetId);
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UserWidgetBookmark_" + widgetId.ToString() + "_init", "_page.userWidgetBookmark['items_" + widgetId.ToString() + "'] = " + (new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(bookmarkList)) + ";", true);
        }

        private void UserWidgetFeedContent(long widgetId)
        {
            int count = 10;
            int skip = 0;
            int trustedSourceId = 0;
            string search = "";

            List<Model.REST.WidgetFeedContent> ltContent = new List<Model.REST.WidgetFeedContent>();
            if (this.GetSessionUser() == null)
            {
                //Load Public Home page, from initial box
                if (HttpRuntime.Cache["UserWidgetFeedContent." + widgetId.ToString() + ".Home"] == null)
                {
                    ltContent = this.GetUserWidgetFeedContent(widgetId, count, skip, trustedSourceId, search);
                    HttpRuntime.Cache.Insert(("UserWidgetFeedContent." + widgetId.ToString() + ".Home"), ltContent, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    ltContent = (List<Model.REST.WidgetFeedContent>)HttpRuntime.Cache["UserWidgetFeedContent." + widgetId.ToString() + ".Home"];
                }
            }
            else
            {
                ltContent = this.GetUserWidgetFeedContent(widgetId, count, skip, trustedSourceId, search);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UserWidgetFeed_" + widgetId.ToString() + "_init", "_page.userWidgetFeed['items_" + widgetId.ToString() + "'] = " + (new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ltContent)) + ";", true);
        }

        public List<Model.REST.WidgetFeedContent> GetUserWidgetFeedContent(long userWidgetId, int count, int skip, int trustedSourceId, String search)
        {
            Bll.FeedContent bllFeedContent = new Bll.FeedContent();

            bool onlyFeedWithThumb = false;
            if (this.GetSessionUser() == null)
            {
                onlyFeedWithThumb = true;
            }

            if (trustedSourceId > 0)
            {
                onlyFeedWithThumb = false;
            }

            Model.FeedContent[] feedContentList = bllFeedContent.GetUserWidgetContent(userWidgetId, trustedSourceId, onlyFeedWithThumb, search);
            if (feedContentList == null)
            {
                feedContentList = new Model.FeedContent[0];
            }

            feedContentList = feedContentList.Distinct(Model.FeedContentComparer.Instance).ToArray();



            if (feedContentList.Length > count)
            {
                feedContentList = feedContentList.Skip(skip).Take(count).ToArray();
            }


            List<Model.REST.WidgetFeedContent> restModelList = new List<Model.REST.WidgetFeedContent>();

            if (feedContentList != null)
            {
                for (int i = 0; i < feedContentList.Length; i++)
                {
                    Model.FeedContent model = feedContentList[i];
                    Model.REST.WidgetFeedContent restModel = new Model.REST.WidgetFeedContent();
                    restModel.id = model.Id;
                    restModel.trustedSourceId = model.TrustedSourceId;
                    restModel.trustedSourceName = model.TrustedSourceName;
                    restModel.title = model.Title;
                    restModel.categoryName = model.Category.Split(',');
                    restModel.description = Bll.Util.ShowPreviewContent(model.Description, 100);
                    restModel.link = model.Link;
                    if (!String.IsNullOrEmpty(model.Thumb))
                    {
                        restModel.image = "/Content/FeedContentImage/" + model.Thumb;
                    }
                    else
                    {
                        restModel.image = null;
                    }
                    restModel.publishDate = model.PublishedDate;
                    restModel.publishDateRelative = Bll.Util.RelativeTime(model.PublishedDate);
                    restModel.openIFrame = model.OpenIFrame;

                    restModelList.Add(restModel);
                }
            }

            return restModelList;
        }

        private Model.REST.WidgetBookmarkList UserWidgetBookmarkGetItems(long pWidgetId)
        {
            Bll.FeedContent bllFeedContent = new Bll.FeedContent();

            Model.REST.WidgetBookmarkList bookmarkList = new Model.REST.WidgetBookmarkList();
            bookmarkList.trustedSource = new List<Model.REST.WidgetBookmark>();
            bookmarkList.source = new List<Model.REST.WidgetBookmark>();

            Model.UserWidgetTrustedSource[] s1 = bllFeedContent.GetBookmark(pWidgetId, true);
            Model.UserWidgetTrustedSource[] s2 = bllFeedContent.GetBookmark(pWidgetId, false);

            if (s1 != null)
                bookmarkList.trustedSource = UserWidgetBookmarkModelListToRESTModelList(s1.ToList());

            if (s2 != null)
                bookmarkList.source = UserWidgetBookmarkModelListToRESTModelList(s2.ToList());

            return bookmarkList;
        }

        public List<Model.REST.WidgetBookmark> UserWidgetBookmarkModelListToRESTModelList(List<Model.UserWidgetTrustedSource> modelList)
        {
            List<Model.REST.WidgetBookmark> restModelList = new List<Model.REST.WidgetBookmark>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    Model.UserWidgetTrustedSource model = modelList[i];
                    Model.REST.WidgetBookmark restModel = new Model.REST.WidgetBookmark();
                    restModel.title = model.Name;
                    restModel.link = Bll.Util.ReturnFullUrl(model.Url);

                    restModel.trustedSourceId = model.TrustedSourceId;

                    if (!String.IsNullOrEmpty(model.Icon))
                        restModel.image = ConfigurationManager.AppSettings["Application.Upload.Image.Source"] + model.Icon;
                    else
                        restModel.image = null;

                    restModel.openIFrame = model.OpenIFrame;
                    restModelList.Add(restModel);
                }
            }

            return restModelList;
        }

        protected List<Model.REST.UserWidget> UserWidget()
        {
            bool IsDeletable = true;
            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            Model.User user = this.GetSessionUser();
            List<Model.UserWidget> widgetList = null;

            //If no one is logged, load default user
            long userId = this.DEFAUT_USER_ID;
            if (user != null)
                userId = user.Id;
            else
                IsDeletable = false;

            int categoryId = 0;
            try
            {
                widgetList = bllUserWidget.GetUserWidget(userId, categoryId).ToList();
                widgetList = bllUserWidget.OrderWidgets(widgetList);
            }
            catch { widgetList = new List<Model.UserWidget>(); }

            List<Model.REST.UserWidget> restModelList = new List<Model.REST.UserWidget>();

            if (widgetList != null)
            {
                for (int i = 0; i < widgetList.Count; i++)
                {
                    Model.UserWidget model = widgetList[i];
                    Model.REST.UserWidget restModel = new Model.REST.UserWidget();
                    restModel.id = model.Id;
                    restModel.title = model.Name;
                    restModel.typeId = model.SystemTagId;
                    restModel.typeName = Bll.Util.EnumToDescription((Model.Enum.enWidgetType)model.SystemTagId);

                    restModel.isDeletable = IsDeletable;
                    restModel.isDefault = (this.GetSessionUser() == null) ? true : false; //change this variable name to isPublicWidget
                    restModel.isSystemDefault = model.DefaultWidget;
                    if (restModel.isDeletable && restModel.isSystemDefault)
                        restModel.isDeletable = false;

                    if ((Model.Enum.enWidgetType)model.SystemTagId == Model.Enum.enWidgetType.FACEBOOK && restModel.isDefault)
                    {
                        restModel.token = this.GetPublicFacebookToken();
                    }

                    if ((Model.Enum.enWidgetType)model.SystemTagId == Model.Enum.enWidgetType.WEATHER)
                    {
                        if (!string.IsNullOrEmpty(model.UserWidgetExtraInfoXML))
                        {
                            try
                            {
                                Model.Widget.ExtraInfo.Weather extraInfo = (Model.Widget.ExtraInfo.Weather)Model.Util.Deserialize(model.UserWidgetExtraInfoXML, typeof(Model.Widget.ExtraInfo.Weather));
                                restModel.zipCode = extraInfo.PreferredZipCode;
                            }
                            catch { }
                        }
                    }

                    if (!String.IsNullOrEmpty(model.Category))
                    {
                        String[] category = model.Category.Split(',');
                        restModel.categoryId = new int[category.Length];
                        for (int c = 0; c < category.Length; c++)
                        {
                            restModel.categoryId[c] = Int32.Parse(category[c]);
                        }
                    }
                    restModel.size = model.Size;
                    restModel.col = model.Col;
                    restModel.row = model.Row;
                    restModelList.Add(restModel);
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UserWidget_init", "_page.widgets = " + (new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(restModelList)) + ";", true);
            return restModelList;
        }

        protected void SuggestionBox()
        {
            var cacheSuggestionBox = HttpRuntime.Cache["SuggestionBox"];
            List<Model.SuggestionBox> ltSuggetions;
            Model.User user = this.GetSessionUser();
            if ((user != null) && (user.Id > 0))
            {
                Bll.SuggestionBox bllSuggestionBox = new Bll.SuggestionBox();
                ltSuggetions = bllSuggestionBox.Get((user == null) ? 0 : user.Id);
            }
            else
            {
                ltSuggetions = null;
            }
            if (ltSuggetions != null)
            {
                foreach (Model.SuggestionBox sg in ltSuggetions)
                {
                    if (!string.IsNullOrEmpty(sg.Image))
                    {
                        string[] arrImageUrl = sg.Image.Split(new string[] { "/" }, StringSplitOptions.None);
                        sg.Image = ConfigurationManager.AppSettings["Application.Upload.Image.SuggestionBox"] + arrImageUrl[arrImageUrl.Length - 1];
                    }
                    sg.RandomOrder = Guid.NewGuid().ToString();
                }
                ltSuggetions = ltSuggetions.OrderByDescending(x => x.Preferred).ThenBy(x => x.RandomOrder).ToList();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SuggestionBox_init", "_page.suggestions = " + (new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ltSuggetions)) + ";", true);
            }
        }

        protected Model.User GetSessionUser()
        {
            if (this.Context != null)
            {
                if (this.Context.Session["User"] != null)
                {
                    return (Model.User)this.Context.Session["User"];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        protected string GetPublicFacebookToken()
        {
            if (HttpRuntime.Cache["System.FacebookPublicToken"] == null)
            {
                Facebook.FacebookClient client = new Facebook.FacebookClient(Facebook.FacebookApplication.Current.AppId, Facebook.FacebookApplication.Current.AppSecret);
                HttpRuntime.Cache.Insert("System.FacebookPublicToken", client.AccessToken, null, DateTime.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return HttpRuntime.Cache["System.FacebookPublicToken"].ToString();
        }

    }
}