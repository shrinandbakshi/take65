using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidget_News
    /// </summary>
    public class UserWidget_News : REST
    {

        protected override void Get(string parameter)
        {
            Bll.UserWidget bllUserWiget = new Bll.UserWidget();
            Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
            Bll.FeedContent bllFeedContent = new Bll.FeedContent();
            int widgetId = 0;

            try
            {
                widgetId = Int32.Parse(this.GetParameterValue("p1"));
            }
            catch { }

            Model.UserWidget userWidget = bllUserWiget.Get(widgetId);
            
            Website.Service.TrustedSourceWidget source = new TrustedSourceWidget();
            Website.Service.UserWidget webUser = new Website.Service.UserWidget();


            List<Model.REST.TrustedSource> trustedSource = source.GetTrustedSourceWidget(widgetId);

            Model.REST.WidgetFeed userFeed = new Model.REST.WidgetFeed();
            userFeed.userWidget = webUser.ModelToRESTModel(userWidget);
            userFeed.trustedSource = trustedSource.ToArray();

            this.Response<Model.REST.WidgetFeed>(userFeed);

        }

        protected override void Get()
        {
            throw new NotImplementedException();
        }

        protected override void Post( string parameter)
        {
            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                Bll.UserWidgetTag bllUserWidgetTag = new Bll.UserWidgetTag();
                Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
                Bll.UserWidgetTrustedSource bllUserWidgetTrustedSource = new Bll.UserWidgetTrustedSource();

                long userWidgetId = Convert.ToInt64(parameter);

                try
                {
                    
                    Model.REST.WidgetFeed widgetFeed = this.ReadJsonRequest<Model.REST.WidgetFeed>();

                    if (widgetFeed.category != null)// && widgetFeed.trustedSource != null
                    {
                        bllUserWidgetTrustedSource.DeleteTrustedSource(userWidgetId);

                        List<Model.TrustedSource> trustedSourceList = new List<Model.TrustedSource>();

                        if (widgetFeed.category != null)
                        {
                            for (int i = 0; i < widgetFeed.category.Length; i++)
                            {
                                Model.TrustedSource[] trustedSource = (bllTrustedSource.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED, widgetFeed.category[i].id));
                                if (trustedSource != null && trustedSource.Length > 0)
                                    trustedSourceList.AddRange(trustedSource.ToList());
                            }
                        }

                        widgetFeed.trustedSource = trustedSourceList.Select(x => new Model.REST.TrustedSource { id = x.Id }).ToArray();

                        userWidgetId = bllUserWidget.Save(new Model.UserWidget()
                        {
                            Id = userWidgetId,
                            Name = widgetFeed.title,
                            SystemTagId = (int)Model.Enum.enWidgetType.FEED,
                            Size = 3,
                            UserId = user.Id
                        });

                        for (int t = 0; t < widgetFeed.trustedSource.Length; t++)
                        {
                            for (int c = 0; c < widgetFeed.category.Length; c++)
                            {
                                bllUserWidgetTrustedSource.SaveTrustedSourceFeed(userWidgetId, widgetFeed.trustedSource[t].id, widgetFeed.category[c].id);

                                if (t == 0)
                                {
                                    bllUserWidgetTag.Save(new Model.UserWidgetTag()
                                    {
                                        UserId = user.Id,
                                        UserWidgetId = userWidgetId,
                                        SystemTagId = widgetFeed.category[c].id
                                    });
                                }
                            }
                        }

                    }

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ModelToRESTModel(bllUserWidget.GetUserWidget(user.Id, 0).ToList().Where(x => x.Id == userWidgetId).First()))
                    });
                }
                catch (Exception e)
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "Error: " + e.Message
                    });
                }
            }
        }


        /// <summary>
        /// Insert new feed widget
        /// </summary>
        protected override void Post()
        {

            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                Bll.UserWidgetTag bllUserWidgetTag = new Bll.UserWidgetTag();
                Bll.UserWidgetTrustedSource bllUserWidgetTrustedSource = new Bll.UserWidgetTrustedSource();

                long userWidgetId = 0;

                try
                {
                    Model.REST.WidgetFeed widgetFeed = this.ReadJsonRequest<Model.REST.WidgetFeed>();
                    Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();

                    if (widgetFeed.category != null) // && widgetFeed.trustedSource != null
                    {
                        List<Model.TrustedSource> trustedSourceList = new List<Model.TrustedSource>();

                        if (widgetFeed.category != null)
                        {
                            for (int i = 0; i < widgetFeed.category.Length; i++)
                            {
                                Model.TrustedSource[] trustedSource = (bllTrustedSource.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED, widgetFeed.category[i].id));
                                if (trustedSource != null && trustedSource.Length > 0)
                                    trustedSourceList.AddRange(trustedSource.ToList());
                            }
                        }

                        widgetFeed.trustedSource = trustedSourceList.Select(x => new Model.REST.TrustedSource { id = x.Id }).ToArray();

                        userWidgetId = bllUserWidget.Save(new Model.UserWidget()
                        {
                            Name = widgetFeed.title,
                            SystemTagId = (int)Model.Enum.enWidgetType.FEED,
                            Size = 3,
                            UserId = user.Id
                        });
                        
                            for (int t = 0; t < widgetFeed.trustedSource.Length; t++)
                            {
                                for (int c = 0; c < widgetFeed.category.Length; c++)
                                {
                                    bllUserWidgetTrustedSource.SaveTrustedSourceFeed(userWidgetId, widgetFeed.trustedSource[t].id, widgetFeed.category[c].id);

                                    if (t == 0)
                                    {
                                        bllUserWidgetTag.Save(new Model.UserWidgetTag()
                                        {
                                            UserId = user.Id,
                                            UserWidgetId = userWidgetId,
                                            SystemTagId = widgetFeed.category[c].id
                                        });
                                    }
                                }
                            }
                        
                    }

                    /* reordering widgets */
                    List<Model.UserWidget> widgetList = bllUserWidget.GetUserWidget(user.Id, 0).ToList();
                    widgetList = bllUserWidget.OrderWidgets(widgetList);
                    Model.UserWidget userNewWidget = widgetList.ToList().Where(x => x.Id == userWidgetId).First();
                    /* reordering widgets */


                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ModelToRESTModel(userNewWidget))
                    });
                }
                catch (Exception e)
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "Error: " + e.Message
                    });
                }
            }
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }


        public Model.REST.UserWidget ModelToRESTModel(Model.UserWidget model)
        {
            Model.REST.UserWidget restModel = new Model.REST.UserWidget();
            restModel.id = model.Id;
            restModel.title = model.Name;
            restModel.typeId = model.SystemTagId;
            restModel.typeName = Bll.Util.EnumToDescription((Model.Enum.enWidgetType)model.SystemTagId);
            restModel.col = model.Col;
            restModel.row = model.Row;
            restModel.isDeletable = true;
            restModel.size = 3;
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