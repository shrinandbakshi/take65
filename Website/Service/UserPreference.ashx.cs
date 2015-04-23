using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserPreference
    /// </summary>
    public class UserPreference : REST
    {

        /// <summary>
        /// Return a list of trusted sources , by type and category (wagner.leonardi@netbiis.com)
        /// </summary>
        protected override void Get()
        {
            //Retrieve data
            Bll.Tag bllTag = new Bll.Tag();
            List<Model.Tag> trustedSourceList = null;
            try
            {
                if (HttpRuntime.Cache["UserPreference"] == null || HttpContext.Current.Request["cache"] == "false")
                {
                    trustedSourceList = bllTag.GetSystemTag(Model.Enum.enSystemTagType.PREFERENCE);
                    HttpRuntime.Cache.Insert("UserPreference", trustedSourceList, null, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    trustedSourceList = (List<Model.Tag>)HttpRuntime.Cache["UserPreference"];
                }

                
            }
            catch { }

            this.Response<Model.REST.Preference>(this.ModelListToRESTModelList(trustedSourceList));

        }

        protected override void Get(string parameter)
        {
            throw new NotImplementedException();
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

        private List<Model.REST.Preference> ModelListToRESTModelList(List<Model.Tag> modelList)
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

        private Model.REST.Preference ModelToRESTModel(Model.Tag model)
        {
            Model.REST.Preference restModel = new Model.REST.Preference();
            restModel.id = model.Id;
            restModel.title = model.Display;

            return restModel;
        }
    }
}