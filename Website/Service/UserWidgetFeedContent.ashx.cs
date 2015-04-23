using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidgetFeedContent
    /// </summary>
    public class UserWidgetFeedContent : REST
    {
        protected override void Get(string parameter)
        {

            int widgetId = 0;
            int count = 10;
            int skip = 0;
            int trustedSourceId = 0;
            String search = "";

            try
            {
                widgetId = Int32.Parse(this.GetParameterValue("p1"));
                count = Int32.Parse(this.GetParameterValue("p2"));
                skip = Int32.Parse(this.GetParameterValue("p3"));
                trustedSourceId = Int32.Parse(this.GetParameterValue("p4"));
                search = this.GetParameterValue("p5");
            }
            catch { }

            List<Model.REST.WidgetFeedContent> ltContent = new List<Model.REST.WidgetFeedContent>();
            if (this.GetSessionUser() == null)
            {
                if (this.GetParameterValue("p3") == null)
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
            }
            else
            {
                ltContent = this.GetUserWidgetFeedContent(widgetId, count, skip, trustedSourceId, search);
            }

            this.Response<List<Model.REST.WidgetFeed>>(ltContent);
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
            feedContentList = feedContentList.Distinct(Model.FeedContentComparer.Instance).ToArray();

            if (feedContentList == null)
            {
                feedContentList = new Model.FeedContent[0];
            }

            if (feedContentList.Length > count)
            {
                feedContentList = feedContentList.Skip(skip).Take(count).ToArray();
            }


            return this.ModelListToRESTModelList(feedContentList.ToList());

        }

        protected override void Get()
        {
            throw new NotImplementedException();
        }

        protected override void Post(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Post()
        {
            throw new NotImplementedException();
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        private List<Model.REST.WidgetFeedContent> ModelListToRESTModelList(List<Model.FeedContent> modelList)
        {
            List<Model.REST.WidgetFeedContent> restModelList = new List<Model.REST.WidgetFeedContent>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    restModelList.Add(this.ModelToRESTModel(modelList[i]));
                }
            }

            return restModelList;
        }

        private Model.REST.WidgetFeedContent ModelToRESTModel(Model.FeedContent model)
        {
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

            return restModel;
        }
        
    }
}