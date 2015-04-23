using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using Website.App_Start;
using Model;


namespace Website.Service
{
    /// <summary>
    /// This class provides all RESTful standardization and behaviours,
    /// every RESTful service MUST implement this class.
    /// </summary>
    public abstract class REST : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        //private HttpContext _context = null;
        public HttpContext Context { get; private set; }
        protected readonly int DEFAUT_USER_ID = Convert.ToInt32(ConfigurationManager.AppSettings["Application.DefaultUserId"]);

        public void ProcessRequest(HttpContext context)
        {
            String httpMethod = context.Request.HttpMethod.ToUpper();
            String parameter = context.Request["p1"];

            this.Context = context;
            this.Context.Response.ContentType = "application/json";

            if (httpMethod == "GET")
            {
                if (!String.IsNullOrEmpty(parameter))
                {
                    this.Get(parameter);
                }
                else
                {
                    this.Get();
                }
            }
            else if (httpMethod == "POST")
            {
                if (!String.IsNullOrEmpty(parameter))
                {
                    this.Post(parameter);
                }
                else
                {
                    this.Post();
                }
            }
            else if (httpMethod == "DELETE")
            {
                this.Delete(parameter);
            }
        }// end ProcessRequest

        //RESTful operations (wagner.leonardi@netbiis.com)
        protected abstract void Get(String parameter);
        protected abstract void Get();
        protected abstract void Post(String parameter);
        protected abstract void Post();
        protected abstract void Delete(String parameter);

        protected bool IsLegitimateUser(string stateToken)
        {
            if (stateToken.CompareTo(AntiForgeryToken.Instance.ReferenceToken) == 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Get a parameter value by given keyname (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected String GetParameterValue(String parameterName)
        {
            return this.Context.Request[parameterName];
        }

        /// <summary>
        /// Get user from session (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        protected Model.User GetSessionUser()
        {
            if (this.Context != null)
            {
                if (this.Context.Session["User"] != null)
                {
                    return (Model.User)this.Context.Session["User"];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        protected string GetPublicFacebookToken()
        {
            if (HttpRuntime.Cache["System.FacebookPublicToken"] == null)
            {
                Facebook.FacebookClient client = new Facebook.FacebookClient(Facebook.FacebookApplication.Current.AppId, Facebook.FacebookApplication.Current.AppSecret);

                HttpRuntime.Cache.Insert("System.FacebookPublicToken", client.AccessToken, null, DateTime.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return HttpRuntime.Cache["System.FacebookPublicToken"].ToString();

        }

        protected string GetPublicGoogleToken()
        {
            if (HttpRuntime.Cache["System.GooglePublicToken"] == null)
            {
                AuthorizationServerDescription server = new AuthorizationServerDescription
                {
                    AuthorizationEndpoint = new Uri("https://accounts.google.com/o/oauth2/auth"),
                    TokenEndpoint = new Uri("https://accounts.google.com/o/oauth2/token"),
                    ProtocolVersion = ProtocolVersion.V20,
                };

                List<string> scope = new List<string> 
                { 
                    "https://mail.google.com/", 
                    "https://www.googleapis.com/auth/userinfo.email" 
                };

                WebServerClient consumer = new WebServerClient(server, ConfigurationManager.AppSettings["Google.CLIENT_ID"], ConfigurationManager.AppSettings["Google.CLIENT_SECRET"]);

                HttpRuntime.Cache.Insert("System.GooglePublicToken", consumer.GetClientAccessToken(), null, DateTime.Now.AddHours(3), System.Web.Caching.Cache.NoSlidingExpiration);

                consumer.RequestUserAuthorization(scope, new Uri("/googleoauth2callback"));
            }

            return HttpRuntime.Cache["System.GooglePublicToken"].ToString();

        }

        /// <summary>
        /// Set user into session (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <param name="user"></param>
        protected void SetSessionUser(Model.User user)
        {
            this.Context.Session["User"] = user;




            if (user == null)
            {
                this.Context.Session["IsEditingPublicHomePage"] = null;
                if (this.Context.Response.Cookies["Take65.User"] != null)
                {
                    this.Context.Response.Cookies["Take65.User"].Expires = DateTime.Now.AddDays(-1);
                }
            }
            else
            {
                this.Context.Response.Cookies["Take65.User"].Value = user.GUID;
                this.Context.Response.Cookies["Take65.User"].Expires = DateTime.Now.AddMonths(6);
            }
        }

        protected void SetSessionFacebookToken(string token)
        {
            this.Context.Session["User.FacebookToken"] = token;
        }

        protected string GetSessionFacebookToken()
        {
            if (this.Context.Session["User.FacebookToken"] != null)
            {
                return this.Context.Session["User.FacebookToken"].ToString();
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Print C# object as RESTful response (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseObject"></param>
        protected void Response<T>(object responseObject)
        {
            String parsedObject = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(responseObject);
            this.Context.Response.Write(parsedObject);
        }

        /// <summary>
        /// TRY to Convert JSON object into a C# object (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <typeparam name="T">C# object type</typeparam>
        /// <param name="jsonString">JSON object</param>
        /// <returns></returns>
        protected T ReadJsonRequest<T>()
        {
            //Read json response
            String jsonString = new System.IO.StreamReader(this.Context.Request.InputStream).ReadToEnd();
            T parsedObject = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<T>(jsonString);
            return parsedObject;
        }

        /// <summary>
        /// IHttpHandler interface implementation (wagner.leonardi@netbiis.com)
        /// </summary>
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        protected void SetSessionState(string state)
        {
            this.Context.Session["authstate"] = state;
        }
        protected string GetSessionState()
        {
            if (this.Context.Session["authstate"] != null)
                return this.Context.Session["authstate"].ToString();
            else
                return string.Empty;
        }
        protected void SetGoogleData(string key, GoogleAccount gAccount)
        {
            this.Context.Session[key] = gAccount;
        }
        protected GoogleAccount GetGoogleData(string key)
        {
            if (this.Context.Session[AntiForgeryToken.Instance.ReferenceToken] != null)
                return (GoogleAccount)this.Context.Session[AntiForgeryToken.Instance.ReferenceToken];
            else
                return null;
        }
        protected string GetBaseUrl()
        {
            string baseUrl = Context.Request.Url.Scheme + "://" + Context.Request.Url.Authority
                + Context.Request.ApplicationPath.TrimEnd('/');
            return baseUrl;
        }
    }// end class
}// end namespace
