using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


namespace Website.Service
{
    /// <summary>
    /// Summary description for SuggestionBox
    /// </summary>
    public class SuggestionBox : REST
    {


        protected override void Get(string parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets Suggestions for logged in User.
        /// </summary>
        protected override void Get()
        {
            List<Model.SuggestionBox> ltSuggetions;
            Model.User user = this.GetSessionUser();

            Bll.SuggestionBox bllSuggestionBox = new Bll.SuggestionBox();
            ltSuggetions = bllSuggestionBox.Get((user == null) ? 0 : user.Id);
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
                this.Response<Model.REST.SuggestionBox>(this.ModelListToRESTModelList(ltSuggetions));
            }          
        }
      
        protected override void Post(string parameter)
        {
            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                if (parameter.ToUpper() == "IGNORE")
                {
                    Model.SuggestionBox response = this.ReadJsonRequest<Model.SuggestionBox>();

                    Bll.SuggestionBox bllSuggestionBox = new Bll.SuggestionBox();
                    bllSuggestionBox.IgnoreSuggestion(user.Id, response.Id);
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "Suggestion Ignored"
                    });
                }

            }
        }

        protected override void Post()
        {
            throw new NotImplementedException();
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        public List<Model.REST.SuggestionBox> ModelListToRESTModelList(List<Model.SuggestionBox> modelList)
        {
            List<Model.REST.SuggestionBox> restModelList = new List<Model.REST.SuggestionBox>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    restModelList.Add(this.ModelToRESTModel(modelList[i]));
                }
            }
            return restModelList;
        }

        private Model.REST.SuggestionBox ModelToRESTModel(Model.SuggestionBox model)
        {
            Model.REST.SuggestionBox restModel = new Model.REST.SuggestionBox();            
            restModel.Id = model.Id;
            restModel.Name = model.Name;
            restModel.Url = model.Url;
            restModel.Description = model.Description;
            restModel.Image = model.Image;
            restModel.register = model.register;
            restModel.lastupdate = model.lastupdate;
            restModel.deleted = model.deleted;
            restModel.SystemTagList = model.SystemTagList;
            restModel.RandomOrder = model.RandomOrder;
            restModel.Preferred = model.Preferred;

            return restModel;
        }
    }
}