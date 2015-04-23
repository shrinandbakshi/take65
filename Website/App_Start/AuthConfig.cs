using Facebook;
using System;
using System.Configuration;

namespace Website.App_Start
{
    public static class AuthConfig
    {
        public static FacebookClient AuthFacebookClient()
        {
            return new FacebookClient(Facebook.FacebookApplication.Current.AppId, Facebook.FacebookApplication.Current.AppSecret);
            // auth.RequestAuthentication(HttpContext, authUrl);
        }
    }
}