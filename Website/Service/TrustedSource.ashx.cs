using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// REST services for TrustedSource entity (wagner.leonardi@netbiis.com)
    /// </summary>
    public class TrustedSource : REST
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


            this.Response<Model.REST.TrustedSource>(this.GetTrustedSource(categoryId));
        }

        public List<Model.REST.TrustedSource> GetTrustedSource(int categoryId)
        {
            //Retrieve data
            Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
            List<Model.TrustedSource> trustedSourceList = null;
            try
            {
                trustedSourceList = bllTrustedSource.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED, categoryId).ToList();
            }
            catch { }

            return this.ModelListToRESTModelList(trustedSourceList);
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Post(string parameter)
        {

            Model.REST.WidgetFeed userWidgetFeed = this.ReadJsonRequest<Model.REST.WidgetFeed>();
            Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();

            List<Model.TrustedSource> trustedSourceList = new List<Model.TrustedSource>();

            if (userWidgetFeed != null && userWidgetFeed.category != null)
            {
                for (int i = 0; i < userWidgetFeed.category.Length; i++)
                {
                    Model.TrustedSource[] trustedSource = (bllTrustedSource.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED, userWidgetFeed.category[i].id));
                    if (trustedSource != null && trustedSource.Length > 0)
                        trustedSourceList.AddRange(trustedSource.ToList());
                }
            }


            List<int> originalI = new List<int>();
            List<Model.TrustedSource> duplicatedSource = new List<Model.TrustedSource>();
            for (int i = 0; i < trustedSourceList.Count(); i++)
            {
                if (!originalI.Contains(trustedSourceList[i].Id))
                {
                    originalI.Add(trustedSourceList[i].Id);
                }
                else
                {
                    duplicatedSource.Add(trustedSourceList[i]);
                }
            }

            for (int i = 0; i < duplicatedSource.Count(); i++)
            {
                trustedSourceList.Remove(duplicatedSource[i]);
            }

            
            if (parameter.ToUpper() == "NEW")
            {
                this.Response<Model.REST.TrustedSource[]>(this.ModelListToRESTModelList(trustedSourceList));
            }
            else if (parameter.ToUpper() == "EDIT")
            {
                long widgetUserId = Int64.Parse(this.GetParameterValue("p2"));
                Model.TrustedSource[] trustedSourceSelected = bllTrustedSource.GetTrustedSource(widgetUserId);
                foreach (Model.TrustedSource tsCats in trustedSourceList)
                {
                    if (trustedSourceSelected.Where(x => x.Id == tsCats.Id).Count() > 0)
                    {
                        tsCats.UserWidgetSelected = true;
                    }
                }
                this.Response<Model.REST.TrustedSource[]>(this.ModelListToRESTModelList(trustedSourceList));

            }
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
            restModel.chk = model.UserWidgetSelected;
            return restModel;
        }

    }
}