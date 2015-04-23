using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidget
    /// </summary>
    public class UserWidget : REST
    {
        private bool IsDeletable = true;

        protected override void Get(string parameter)
        {
            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            Model.User user = this.GetSessionUser();
            List<Model.UserWidget> widgetList = null;

            //If no one is logged, load default user
            long userId = this.DEFAUT_USER_ID;
            if (user != null)
            {
                userId = user.Id;
            }
            else
            {
                this.IsDeletable = false;
            }

            int categoryId = 0;
            try
            {
                categoryId = Int32.Parse(this.GetParameterValue("categoryId"));
            }
            catch { }

            try
            {
                widgetList = bllUserWidget.GetUserWidget(userId, categoryId).ToList();
            }
            catch { widgetList = new List<Model.UserWidget>(); }


            this.Response<List<Model.REST.UserWidget>>(this.ModelListToRESTModelList(widgetList));
        }

        protected override void Get()
        {
            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            Model.User user = this.GetSessionUser();
            List<Model.UserWidget> widgetList = null;

            //If no one is logged, load default user
            long userId = this.DEFAUT_USER_ID;
            if (user != null)
            {
                userId = user.Id;
            }
            else
            {
                this.IsDeletable = false;
            }

            int categoryId = 0;
            try
            {
                categoryId = Int32.Parse(this.GetParameterValue("categoryId"));
            }
            catch { }

            try
            {
                widgetList = bllUserWidget.GetUserWidget(userId, categoryId).ToList();
            }
            catch { widgetList = new List<Model.UserWidget>(); }

            this.Response<List<Model.REST.UserWidget>>(this.ModelListToRESTModelList(widgetList));
        }

        protected override void Post(string parameter)
        {
            Model.User user = this.GetSessionUser();

            if (user != null)
            {
                if (parameter.ToUpper() == "DELETE")
                {
                    long userWidgetId = Int64.Parse(this.GetParameterValue("p2"));

                    Bll.UserWidget bllUserWidget = new Bll.UserWidget();

                    try
                    {
                        bllUserWidget.Delete(userWidgetId);
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = "User Widget deleted"
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
                else if (parameter.ToUpper() == "UPDATETITLE")
                {
                    Model.REST.UserWidget restUserWidget = this.ReadJsonRequest<Model.REST.UserWidget>();
                    Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                    bllUserWidget.Save(new Model.UserWidget
                    {
                        Id = restUserWidget.id,
                        Name = restUserWidget.title
                    });

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "Widget updated"
                    });
                }
                else if (parameter.ToUpper() == "SETPOSITION")
                {
                    List<Model.REST.UserWidget> restUserWidget = this.ReadJsonRequest<List<Model.REST.UserWidget>>();
                    Bll.UserWidget bllUserWidget = new Bll.UserWidget();
                    List<Model.UserWidget> ltWidgets = new List<Model.UserWidget>();
                    foreach(Model.REST.UserWidget uw in  restUserWidget){
                        ltWidgets.Add(new Model.UserWidget{
                            Id = uw.id,
                            UserId = user.Id,
                            Row = uw.row,
                            Col = uw.col
                        });
                    }
                    bllUserWidget.SavePosition(ltWidgets);

                }
                else if (parameter.ToUpper() == "CREATEDEFAULT")
                {
                    try
                    {
                        this.CreateDefaultWidget();

                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = "Okie Dokie"
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
            else
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "Not Authorized "
                });
            }
        }

        protected override void Post()
        {
            throw new NotImplementedException();
        }

        protected override void Delete(string parameter)
        {
            Model.User user = this.GetSessionUser();

            if (user != null)
            {
                long userWidgetId = Int64.Parse(parameter);

                Bll.UserWidget bllUserWidget = new Bll.UserWidget();

                try
                {
                    Model.UserWidget userWidget = bllUserWidget.Get(userWidgetId);
                    if (userWidget.UserId == user.Id)
                    {
                        bllUserWidget.Delete(userWidgetId);
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = "User Widget deleted"
                        });
                    }
                    else
                    {
                        throw new Exception("Not Authorized");
                    }
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

        private List<Model.REST.UserWidget> ModelListToRESTModelList(List<Model.UserWidget> modelList)
        {
            List<Model.REST.UserWidget> restModelList = new List<Model.REST.UserWidget>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    restModelList.Add(this.ModelToRESTModel(modelList[i]));
                }
            }

            return restModelList;
        }

        public Model.REST.UserWidget ModelToRESTModel(Model.UserWidget model)
        {
            Model.REST.UserWidget restModel = new Model.REST.UserWidget();
            restModel.id = model.Id;
            restModel.title = model.Name;
            restModel.typeId = model.SystemTagId;
            restModel.typeName = Bll.Util.EnumToDescription((Model.Enum.enWidgetType)model.SystemTagId);

            restModel.isDeletable = this.IsDeletable;
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

        public void CreateDefaultWidget()
        {
            Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            Bll.UserWidgetTrustedSource bllUWTS = new Bll.UserWidgetTrustedSource();

            Model.UserWidget[] userWidget = bllUserWidget.GetUserWidget(this.DEFAUT_USER_ID);
            for (int i = 0; i < userWidget.Length; i++)
            {
                if (userWidget[i].SystemTagId != (int)Model.Enum.enWidgetType.FACEBOOK && userWidget[i].SystemTagId != (int)Model.Enum.enWidgetType.EMAIL && userWidget[i].SystemTagId != (int)Model.Enum.enWidgetType.WEATHER)
                {
                    long savedId = bllUserWidget.Save(new Model.UserWidget()
                    {
                        Size = userWidget[i].Size,
                        SystemTagId = userWidget[i].SystemTagId,
                        UserId = this.GetSessionUser().Id,
                        Name = userWidget[i].Name
                    });


                    Model.UserWidgetTrustedSource[] uwts = bllUWTS.Get(userWidget[i].Id);
                    if (uwts != null && uwts.Length > 0)
                    {
                        for (int j = 0; j < uwts.Count(); j++)
                        {
                            bllUWTS.SaveTrustedSource(new Model.UserWidgetTrustedSource()
                            {
                                UserWidgetId = savedId,
                                Name = uwts[j].Name,
                                Url = uwts[j].Url,
                                TrustedSourceId = uwts[j].TrustedSourceId,
                                CategoryId = uwts[j].SystemTagId
                            });
                        }
                    }
                }

            }



        }

    }
}