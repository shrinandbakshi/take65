using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidgetFacebook
    /// </summary>
    public class UserWidgetFacebook : REST
    {


        protected override void Get(string parameter)
        {
            throw new NotImplementedException();
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
            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            

            long userWidgetId = bllUserWidget.Save(new Model.UserWidget()
            {
                Name = "Facebook News Feed",
                Size = 1,
                SystemTagId = (int)Model.Enum.enWidgetType.FACEBOOK,
                UserId = this.GetSessionUser().Id
            });

            /* reordering widgets */
            List<Model.UserWidget> widgetList = bllUserWidget.GetUserWidget(this.GetSessionUser().Id, 0).ToList();
            widgetList = bllUserWidget.OrderWidgets(widgetList);
            Model.UserWidget userNewWidget = widgetList.ToList().Where(x => x.Id == userWidgetId).First();
            /* reordering widgets */

            this.Response<Model.REST.Response>(new Model.REST.Response()
            {
                status = true,
                response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ModelUserWidgetToRESTModel(userNewWidget))
            });

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