using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidgetCategory
    /// </summary>
    public class UserWidgetCategory : REST
    {

        /// <summary>
        /// Return a list of trusted sources , by type and category (wagner.leonardi@netbiis.com)
        /// </summary>
        protected override void Get()
        {
            //If no one is logged, load default user
            Model.User user = this.GetSessionUser();
            long userId = this.DEFAUT_USER_ID;
            if (user != null)
            {
                userId = user.Id;
            }

            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            Model.Tag[] tagList = bllUserWidget.GetUserWidgetCategory(userId);

            if (tagList == null) tagList = new Model.Tag[0];

            this.Response<List<Model.REST.Category>>(this.ModelListToRESTModelList(tagList.ToList()));
        }

        protected override void Get(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Delete(string parameter)
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

        private List<Model.REST.Category> ModelListToRESTModelList(List<Model.Tag> modelList)
        {
            List<Model.REST.Category> restModelList = new List<Model.REST.Category>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    restModelList.Add(this.ModelToRESTModel(modelList[i]));
                }
            }

            return restModelList;
        }

        private Model.REST.Category ModelToRESTModel(Model.Tag model)
        {
            Model.REST.Category restModel = new Model.REST.Category();
            restModel.id = model.Id;
            restModel.title = model.Display;
            restModel.image = model.Icon;

            return restModel;
        }
    }
}