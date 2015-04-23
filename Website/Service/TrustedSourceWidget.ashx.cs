using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for TrustedSourceWidget
    /// </summary>
    public class TrustedSourceWidget : REST
    {

        /// <summary>
        /// Return a list of trusted sources , by type and category (wagner.leonardi@netbiis.com)
        /// </summary>
        protected override void Get()
        {
            throw new NotImplementedException();
        }

        protected override void Get(string parameter)
        {
            //Get parameters
            int widgetId = 0;

            try
            {
                widgetId = Int32.Parse(this.GetParameterValue("p1"));
            }
            catch { }


            this.Response<List<Model.REST.TrustedSource>>(this.GetTrustedSourceWidget(widgetId));
        }

        public List<Model.REST.TrustedSource> GetTrustedSourceWidget(long userWidgetId)
        {
            //Retrieve data
            Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
            List<Model.TrustedSource> trustedSourceList = null;
            try
            {
                trustedSourceList = bllTrustedSource.GetTrustedSource(userWidgetId).ToList();
            }
            catch { }

            return this.ModelListToRESTModelList(trustedSourceList);
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Post( string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Post()
        {
            throw new NotImplementedException();
        }

        private List<Model.REST.TrustedSource> ModelListToRESTModelList(List<Model.TrustedSource> modelList)
        {
            List<Model.REST.TrustedSource> restModelList = new List<Model.REST.TrustedSource>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    restModelList.Add(this.ModelToRESTModel(modelList[i]));
                }
            }

            return restModelList;
        }

        private Model.REST.TrustedSource ModelToRESTModel(Model.TrustedSource model)
        {
            Model.REST.TrustedSource restModel = new Model.REST.TrustedSource();
            restModel.id = model.Id;
            restModel.title = model.Name;
            restModel.image = model.Icon;
            restModel.link = model.Url;

            return restModel;
        }
    }
}