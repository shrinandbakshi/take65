using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidgetBookmark
    /// </summary>
    public class UserWidgetBookmark : REST
    {

        protected override void Get(string parameter)
        {
            
            int widgetId = 0;
            if (!String.IsNullOrEmpty(parameter))
            {
                widgetId = Int32.Parse(parameter);
            }

            Model.REST.WidgetBookmarkList bookmarkList;
            if (this.GetSessionUser() == null)
            {
                if (HttpRuntime.Cache["UserWidgetBookmark." + widgetId.ToString()] == null)
                {
                    bookmarkList = GetItems(widgetId);
                    HttpRuntime.Cache.Insert(("UserWidgetBookmark." + widgetId.ToString()), bookmarkList, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    bookmarkList = (Model.REST.WidgetBookmarkList)HttpRuntime.Cache["UserWidgetBookmark." + widgetId.ToString()];
                }
            }
            else
            {
                bookmarkList = GetItems(widgetId);
            }

            this.Response<Model.REST.WidgetBookmarkList>(bookmarkList);
        }

        private Model.REST.WidgetBookmarkList GetItems(int pWidgetId)
        {
            Bll.FeedContent bllFeedContent = new Bll.FeedContent();

            Model.REST.WidgetBookmarkList bookmarkList = new Model.REST.WidgetBookmarkList();
            bookmarkList.trustedSource = new List<Model.REST.WidgetBookmark>();
            bookmarkList.source = new List<Model.REST.WidgetBookmark>();

            Model.UserWidgetTrustedSource[] s1 = bllFeedContent.GetBookmark(pWidgetId, true);
            Model.UserWidgetTrustedSource[] s2 = bllFeedContent.GetBookmark(pWidgetId, false);

            if (s1 != null)
                bookmarkList.trustedSource = this.ModelListToRESTModelList(s1.ToList());

            if (s2 != null)
                bookmarkList.source = this.ModelListToRESTModelList(s2.ToList());

            return bookmarkList;
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
                if (parameter.ToUpper() == "TRAVEL")
                {
                    Model.REST.WidgetBookmarkSave response = new Model.REST.WidgetBookmarkSave();

                    Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                    Bll.UserWidgetTrustedSource bllUserWidgetTrustedSource = new Bll.UserWidgetTrustedSource();

                    long userWidgetId = bllUserWidget.Save(new Model.UserWidget()
                    {
                        Name = "Travel",
                        Size = 1,
                        SystemTagId = (int)Model.Enum.enWidgetType.BOOKMARK,
                        UserId = this.GetSessionUser().Id
                    });

                    for (int i = 65; i <= 75; i++)
                    {
                        bllUserWidgetTrustedSource.SaveTrustedSource(new Model.UserWidgetTrustedSource()
                        {

                            CategoryId = 39,
                            TrustedSourceId = i,
                            UserWidgetId = userWidgetId
                        });
                    }



                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ModelUserWidgetToRESTModel(bllUserWidget.GetUserWidget(user.Id, 0).ToList().Where(x => x.Id == userWidgetId).First()))

                    });
                }
                else if (parameter.ToUpper() == "ADDSUGGESTIONBOX")
                {
                    Model.REST.WidgetBookmarkSave response = this.ReadJsonRequest<Model.REST.WidgetBookmarkSave>();
                    Bll.UserWidgetTrustedSource bllUserWidgetTrustedSource = new Bll.UserWidgetTrustedSource();

                    Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                    long userWidgetId = response.id;
                    long suggestionBoxId = Convert.ToInt64(this.GetParameterValue("p2"));

                    if (userWidgetId == 0)
                    {
                        userWidgetId = bllUserWidget.Save(new Model.UserWidget()
                        {
                            Name = response.title,
                            Size = 1,
                            SystemTagId = (int)Model.Enum.enWidgetType.BOOKMARK,
                            UserId = user.Id
                        });

                        List<Model.UserWidget> widgetList = bllUserWidget.GetUserWidget(this.GetSessionUser().Id, 0).ToList();
                        widgetList = bllUserWidget.OrderWidgets(widgetList);                        
                    }

                    Bll.SuggestionBox bllSuggestionBox = new Bll.SuggestionBox();
                    Model.SuggestionBox suggestionBox = bllSuggestionBox.GetById(Convert.ToInt32(suggestionBoxId));


                    bllUserWidgetTrustedSource.SaveTrustedSource(new Model.UserWidgetTrustedSource()
                    {

                        CategoryId = 0,
                        TrustedSourceId = 0,
                        Name = suggestionBox.Name,
                        Url = suggestionBox.Url,
                        UserWidgetId = userWidgetId
                    });

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ModelUserWidgetToRESTModel(bllUserWidget.GetUserWidget(user.Id, 0).ToList().Where(x => x.Id == userWidgetId).First()))
                    });

                }
                else if (parameter.ToUpper() == "EDIT")
                {
                    long userWidgetId = Convert.ToInt64(this.GetParameterValue("p2"));

                    Model.REST.WidgetBookmarkSave response = this.ReadJsonRequest<Model.REST.WidgetBookmarkSave>();

                    Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                    Bll.UserWidgetTrustedSource bllUserWidgetTrustedSource = new Bll.UserWidgetTrustedSource();

                    userWidgetId = bllUserWidget.Save(new Model.UserWidget()
                   {
                       Id = userWidgetId,
                       Name = response.title,
                       Size = 1,
                       SystemTagId = (int)Model.Enum.enWidgetType.BOOKMARK,
                       UserId = user.Id
                   });

                    bllUserWidgetTrustedSource.DeleteTrustedSource(userWidgetId);

                    List<int> categoryList = new List<int>();

                    for (int i = 0; i < response.trustedSource.Length; i++)
                    {
                        bllUserWidgetTrustedSource.SaveTrustedSource(new Model.UserWidgetTrustedSource()
                        {

                            CategoryId = response.trustedSource[i].categoryId,
                            TrustedSourceId = response.trustedSource[i].id,
                            Name = response.trustedSource[i].title,
                            Url = response.trustedSource[i].link,
                            UserWidgetId = userWidgetId
                        });

                        if (!categoryList.Contains(response.trustedSource[i].categoryId))
                        {
                            categoryList.Add(response.trustedSource[i].categoryId);
                        }
                    }

                    Bll.UserWidgetTag bllUserWidgetTag = new Bll.UserWidgetTag();

                    for (int i = 0; i < categoryList.Count(); i++)
                    {
                        bllUserWidgetTag.Save(new Model.UserWidgetTag()
                        {
                            UserId = this.GetSessionUser().Id,
                            UserWidgetId = userWidgetId,
                            SystemTagId = categoryList[i]
                        });
                    }

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ModelUserWidgetToRESTModel(bllUserWidget.GetUserWidget(user.Id, 0).ToList().Where(x => x.Id == userWidgetId).First()))
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

        private long NewBookmarkWidget(long pUserId, Model.REST.WidgetBookmarkSave pWidget)
        {
            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            Bll.UserWidgetTrustedSource bllUserWidgetTrustedSource = new Bll.UserWidgetTrustedSource();

            long userWidgetId = bllUserWidget.Save(new Model.UserWidget()
            {
                Name = pWidget.title,
                Size = 1,
                SystemTagId = (int)Model.Enum.enWidgetType.BOOKMARK,
                UserId = pUserId
            });

            List<int> categoryList = new List<int>();

            for (int i = 0; i < pWidget.trustedSource.Length; i++)
            {
                bllUserWidgetTrustedSource.SaveTrustedSource(new Model.UserWidgetTrustedSource()
                {

                    CategoryId = pWidget.trustedSource[i].categoryId,
                    TrustedSourceId = pWidget.trustedSource[i].id,
                    Name = pWidget.trustedSource[i].title,
                    Url = pWidget.trustedSource[i].link,
                    UserWidgetId = userWidgetId
                });

                if (!categoryList.Contains(pWidget.trustedSource[i].categoryId))
                {
                    categoryList.Add(pWidget.trustedSource[i].categoryId);
                }
            }

            Bll.UserWidgetTag bllUserWidgetTag = new Bll.UserWidgetTag();

            for (int i = 0; i < categoryList.Count(); i++)
            {
                bllUserWidgetTag.Save(new Model.UserWidgetTag()
                {
                    UserId = this.GetSessionUser().Id,
                    UserWidgetId = userWidgetId,
                    SystemTagId = categoryList[i]
                });
            }

            return userWidgetId;
        }

        protected override void Post()
        {

            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                Model.REST.WidgetBookmarkSave response = this.ReadJsonRequest<Model.REST.WidgetBookmarkSave>();
                Bll.UserWidget bllUserWidget = new Bll.UserWidget();

                long userWidgetId = NewBookmarkWidget(user.Id, response);

                /* reordering widgets */
                List<Model.UserWidget> widgetList = bllUserWidget.GetUserWidget(user.Id, 0).ToList();
                widgetList = bllUserWidget.OrderWidgets(widgetList);
                Model.UserWidget userNewWidget = widgetList.ToList().Where(x => x.Id == userWidgetId).First();
                /* reordering widgets */

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
                    status = false,
                    response = "User not logged"
                });
            }
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        public List<Model.REST.WidgetBookmark> ModelListToRESTModelList(List<Model.UserWidgetTrustedSource> modelList)
        {
            List<Model.REST.WidgetBookmark> restModelList = new List<Model.REST.WidgetBookmark>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    restModelList.Add(this.ModelToRESTModel(modelList[i]));
                }
            }

            return restModelList;
        }

        private Model.REST.WidgetBookmark ModelToRESTModel(Model.UserWidgetTrustedSource model)
        {

            Model.REST.WidgetBookmark restModel = new Model.REST.WidgetBookmark();
            restModel.title = model.Name;
            restModel.link = Bll.Util.ReturnFullUrl(model.Url);

            restModel.trustedSourceId = model.TrustedSourceId;

            if(!String.IsNullOrEmpty(model.Icon))
                restModel.image = ConfigurationManager.AppSettings["Application.Upload.Image.Source"] + model.Icon;
            else
                restModel.image = null;

            restModel.openIFrame = model.OpenIFrame;
            
            return restModel;
        }

        public Model.REST.UserWidget ModelUserWidgetToRESTModel(Model.UserWidget model)
        {
            Model.REST.UserWidget restModel = new Model.REST.UserWidget();
            restModel.id = model.Id;
            restModel.title = model.Name;
            restModel.typeId = model.SystemTagId;
            restModel.typeName = Bll.Util.EnumToDescription((Model.Enum.enWidgetType)model.SystemTagId);

            restModel.isDeletable = true;
            restModel.col = model.Col;
            restModel.row = model.Row;
            if (!String.IsNullOrEmpty(model.Category))
            {
                String[] category = model.Category.Split(',');
                restModel.categoryId = new int[category.Length];
                for (int i = 0; i < category.Length; i++)
                {
                    /*
                    string[] category2 = category[i].Split('|');

                    restModel.category[i] = new Model.REST.TrustedSource();
                    restModel.category[i].id = Int32.Parse(category2[0]);
                    restModel.category[i].title = category2[1];
                     */

                    restModel.categoryId[i] = Int32.Parse(category[i]);
                }
            }

            restModel.size = model.Size;

            return restModel;
        }
    }
}