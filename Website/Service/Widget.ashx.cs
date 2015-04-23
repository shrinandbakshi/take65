using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for Widget
    /// </summary>
    public class Widget : REST
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
            int trustedSourceTypeId = 0;
            int categoryId = 0;

            try
            {
                trustedSourceTypeId = Int32.Parse(this.GetParameterValue("p1"));
            }
            catch { }

            try
            {
                categoryId = Int32.Parse(this.GetParameterValue("p2"));
            }
            catch { }

            //Retrieve data
            Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
            List<Model.TrustedSource> trustedSourceList = null;
            try
            {
                trustedSourceList = bllTrustedSource.GetTrustedSource((Model.Enum.enTrustedSourceType)trustedSourceTypeId, categoryId).ToList();
            }
            catch { }

            this.Response<Model.REST.Preference>(this.ModelListToRESTModelList(trustedSourceList));
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

        private List<Model.REST.Preference> ModelListToRESTModelList(List<Model.TrustedSource> modelList)
        {
            List<Model.REST.Preference> restModelList = new List<Model.REST.Preference>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    restModelList.Add(this.ModelToRESTModel(modelList[i]));
                }
            }

            return restModelList;
        }

        private Model.REST.Preference ModelToRESTModel(Model.TrustedSource model)
        {
            Model.REST.Preference restModel = new Model.REST.Preference();
            restModel.id = model.Id;
            restModel.title = model.Name;

            return restModel;
        }
    }
}