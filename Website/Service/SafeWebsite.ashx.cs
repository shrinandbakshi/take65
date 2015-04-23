using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for SafeWebsite
    /// </summary>
    public class SafeWebsite : REST
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
            Model.REST.SafeWebsite restSafeWebsite = this.ReadJsonRequest<Model.REST.SafeWebsite>();
            Bll.SafeWebsite bllSafeWebsite = new Bll.SafeWebsite();

            if (restSafeWebsite.url == null)
                restSafeWebsite.url = "";

            Model.SafeWebsite[] ltWebsite = bllSafeWebsite.Get(restSafeWebsite.url.ToLower().Replace("http://", "").Replace("https://", ""));

            if (ltWebsite != null)
            {
                if (ltWebsite.Count() > 0)
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "Safe Website"
                    });
                }
                else
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "Unsafe Website"
                    });
                }
            }
            else
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "Unsafe Website"
                });
            }
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }
    }
}