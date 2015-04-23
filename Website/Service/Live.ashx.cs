using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Live;
using Website.App_Start;

namespace Website.Service
{
    /// <summary>
    /// Summary description for Live
    /// </summary>
    public class Live : REST
    {
        private string oauth_token_secret = string.Empty;
        private string client_id = ConfigurationManager.AppSettings["LIVE.OAUTH.CLIENT_ID"];
        private string client_secret = ConfigurationManager.AppSettings["LIVE.OAUTH.CLIENT_SECRET"];
        private string redirect_url = string.Empty;
        private LiveAuthClient liveAuthClient = null;
        private LiveConnectSession session = null;

        protected override void Get()
        {
            //TraceLog.Instance.log("LIVE GET", "called without param");
            string baseUrl = this.GetBaseUrl();
            redirect_url = baseUrl + ConfigurationManager.AppSettings["LIVE.OAUTH.REDIRECT_URI"];
            LiveAuthClient liveAuthClient = new LiveAuthClient(client_id, client_secret, redirect_url);
            try
            {
                var wrapper = new HttpContextWrapper(HttpContext.Current);
                //TraceLog.Instance.log("LIVE GET", "exchanging authcode for access token");
                Task<LiveLoginResult> lresult = liveAuthClient.ExchangeAuthCodeAsync(wrapper);
                LiveLoginResult result = lresult.Result;
                if (result.Status == LiveConnectSessionStatus.Connected)
                {
                    return;
                }
            }
            catch(Exception e)
            {
            }
        }

        protected override void Get(string parameter)
        {
            throw new NotImplementedException();

        }

        protected override void Post()
        {
            throw new NotImplementedException();
        }

        protected override void Post(string parameter)
        {
                throw new NotImplementedException();
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        
    }
}