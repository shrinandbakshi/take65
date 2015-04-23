using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for TrustedSourceCategory
    /// </summary>
    public class TrustedSourceCategory : REST
    {

        public void LoadCategory(bool pWithTrustedSource, long pUserId, long pUserWidgetId)
        {
            Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
            List<Model.Category> category;
            if (pUserId != 0 && pUserWidgetId != 0)
            {
                category = bllTrustedSource.GetCategory(pUserId, pUserWidgetId).ToList();
            }
            else
            {
                if (HttpRuntime.Cache["TrustedSourceCategoryNews"] == null)
                {
                    category = bllTrustedSource.GetCategory().ToList();
                    HttpRuntime.Cache.Insert("TrustedSourceCategoryNews", category, null, DateTime.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    category = (List<Model.Category>)HttpRuntime.Cache["TrustedSourceCategoryNews"];
                }
                
            }

            List<Model.REST.Category> categoryList = this.ModelListToRESTModelList(category);
            List<int> removeIdList = new List<int>();

            for(int i=0;i<categoryList.Count();i++)
            {
                if (categoryList[i].trustedSource == null || categoryList[i].trustedSource.Length == 0)
                {
                    removeIdList.Add(categoryList[i].id);
                }
            }

            //remove items without sources
            for (int i = 0; i < removeIdList.Count(); i++)
            {
                categoryList.RemoveAll(a => a.id.Equals(removeIdList[i]));
            }

            this.Response<List<Model.REST.Category>>(categoryList);

            
        }

        
        protected override void Get()
        {
            //this.LoadCategory(true);

            List<Model.REST.Category> ltCategories = new List<Model.REST.Category>();
            if (HttpRuntime.Cache["TrustedSourceCategory"] == null)
            {
                ltCategories = GetCategories(0, 0);
                HttpRuntime.Cache.Insert("TrustedSourceCategory", ltCategories, null, DateTime.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                ltCategories = (List<Model.REST.Category>)HttpRuntime.Cache["TrustedSourceCategory"];
            }
            this.Response<List<Model.REST.Category>>(ltCategories);

        }


        protected List<Model.REST.Category> GetCategories(long pUserId, long pUserWidgetId)
        {
            Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();

            List<Model.Category> category;
            if (pUserWidgetId != 0 && pUserId != 0)
            {
                category = bllTrustedSource.GetCategory(pUserId, pUserWidgetId).ToList();
            }
            else
            {
                category = bllTrustedSource.GetCategory().ToList();
            }

            List<Model.REST.Category> categoryList = this.ModelListToRESTModelList(category);
            List<int> removeIdList = new List<int>();


            for (int i = 0; i < categoryList.Count(); i++)
            {
                categoryList[i].trustedSource = null;

                Model.TrustedSource[] tslist = bllTrustedSource.GetTrustedSource(Model.Enum.enTrustedSourceType.BOOKMARK, categoryList[i].id);

                if (tslist != null && tslist.Length > 0)
                {
                    List<Model.TrustedSource> listSources = tslist.ToList();
                    if (pUserWidgetId != 0 && pUserId != 0)
                    {
                        Model.TrustedSource[] tslistUser = bllTrustedSource.GetTrustedSource(pUserWidgetId);
                        if (tslistUser != null)
                        {
                            foreach (Model.TrustedSource tsS in tslistUser)
                            {
                                if (listSources.Where(x => x.Id == tsS.Id).Count() > 0)
                                {
                                    listSources.Where(x => x.Id == tsS.Id).First().UserWidgetSelected = true;
                                }
                            }
                        }
                    }
                    categoryList[i].trustedSource = new Model.REST.TrustedSource[tslist.Length];
                    for (int k = 0; k < tslist.Length; k++)
                    {
                        categoryList[i].trustedSource[k] = new Model.REST.TrustedSource();
                        categoryList[i].trustedSource[k].categoryId = categoryList[i].id;
                        categoryList[i].trustedSource[k].id = listSources[k].Id;
                        categoryList[i].trustedSource[k].title = listSources[k].Name;
                        categoryList[i].trustedSource[k].image = listSources[k].Icon;
                        categoryList[i].trustedSource[k].chk = listSources[k].UserWidgetSelected;
                    }
                }

                if (categoryList[i].trustedSource == null || categoryList[i].trustedSource.Length == 0)
                {
                    removeIdList.Add(categoryList[i].id);
                }
            }

            //remove items without sources
            for (int i = 0; i < removeIdList.Count(); i++)
            {
                categoryList.RemoveAll(a => a.id.Equals(removeIdList[i]));
            }

            return categoryList;
        }

        /// <summary>
        /// deprecatad
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Get(string parameter)
        {
            Model.User user = this.GetSessionUser();
            if (parameter.ToUpper() == "EDITUSERWIDGETBOOKMARK")
            {
                
                if (user != null)
                {
                    Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();

                    long userWidgetId = Convert.ToInt64(this.GetParameterValue("p2"));
                    List<Model.REST.Category> ltCats = GetCategories(user.Id, userWidgetId);
                    
                    /* Custom Links */
                    Model.TrustedSource[] ltTs = bllTrustedSource.GetTrustedSource(userWidgetId);
                    if (ltTs != null)
                    {
                        //original //if (ltTs.ToList().Where(x => x.Id == 0).Count() > 0)
                        //added for custom sites
                        if (ltTs.ToList().Where(x => x.Icon == null).Count() > 0)
                        {
                            Model.Category customCat = new Model.Category();
                            customCat.Id = 0;
                            customCat.Name = "custom";
                            customCat.TrustedSources = ltTs.ToList().Where(x => x.Icon == null).ToArray();

                            ltCats.Add(this.ModelToRESTModel(customCat));
                        }
                    }
                    /* Custom Links */
                    this.Response<List<Model.REST.Category>>(ltCats);
                }
                else
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "User not logged"
                    });
                }
            }
            else if (parameter.ToUpper() == "EDITUSERWIDGETBOOKMARKCUSTOM")
            {

                if (user != null)
                {
                    Bll.FeedContent bllFeedContent = new Bll.FeedContent();
                    long userWidgetId = Convert.ToInt64(this.GetParameterValue("p2"));
                    Model.UserWidgetTrustedSource[] s2 = bllFeedContent.GetBookmark(userWidgetId, false);
                    this.Response<Model.UserWidgetTrustedSource[]>(s2);
                }
                else
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "User not logged"
                    });
                }
                
            }
            else if (parameter.ToUpper() == "EDITUSERWIDGETNEWS")
            {
                if (user != null)
                {
                    long userWidgetId = Convert.ToInt64(this.GetParameterValue("p2"));
                    this.LoadCategory(true, user.Id, userWidgetId);
                }
                else
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "User not logged"
                    });
                }
            }
            else
            {

                this.LoadCategory(true, 0, 0);
            }
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

        private List<Model.REST.Category> ModelListToRESTModelList(List<Model.Category> modelList)
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

        private Model.REST.Category ModelToRESTModel(Model.Category model)
        {
            Model.REST.Category restModel = new Model.REST.Category();
            restModel.id = model.Id;
            restModel.title = model.Name;
            restModel.image = model.Icon;
            

            if (model.TrustedSources != null && model.TrustedSources.Length > 0)
            {
                restModel.trustedSource = new Model.REST.TrustedSource[model.TrustedSources.Length];
                for (int i = 0; i < model.TrustedSources.Length; i++)
                {
                    restModel.trustedSource[i] = new Model.REST.TrustedSource();
                    restModel.trustedSource[i].id = model.TrustedSources[i].Id;
                    restModel.trustedSource[i].title = model.TrustedSources[i].Name;
                    restModel.trustedSource[i].image = model.TrustedSources[i].Icon;
                    restModel.trustedSource[i].categoryId = restModel.id;
                    restModel.trustedSource[i].chk = model.TrustedSources[i].UserWidgetSelected;
                    restModel.trustedSource[i].link = model.TrustedSources[i].Url;
                }
            }
            

            return restModel;
        }
    }
}