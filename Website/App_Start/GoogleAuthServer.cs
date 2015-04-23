using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using System.Threading;
using System.Configuration;
using Model;
using Google.Apis.Oauth2.v2;

using Google.Apis.Oauth2;
using Google.Apis.Oauth2.v2.Data;

namespace Website.App_Start
{
    public class GoogleAuthServer
    {
        public static GoogleAuthServer Instance = new GoogleAuthServer();
        private ClientSecrets _secrets = null;
        // Stores token response info such as the access token and refresh token.
        private TokenResponse _token = null;
        // Used to peform API calls against Google+.
        private PlusService _ps = null;
        private GoogleAuthServer()
        {
            _secrets = new ClientSecrets()
            {
                ClientId = ConfigurationManager.AppSettings["Google.OAuth2.CLIENT_ID"],
                ClientSecret = ConfigurationManager.AppSettings["Google.OAuth2.CLIENT_SECRET"]
            };
        }
        public string ExchangeCode(string authCode)
        {
            if (string.IsNullOrEmpty(authCode))
                throw new Exception("Authentication code can not be null or empty...");
            // Use the code exchange flow to get an access and refresh token.
            IAuthorizationCodeFlow flow =
                new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = _secrets,
                    Scopes = new string[] { PlusService.Scope.PlusLogin, 
                                            PlusService.Scope.PlusMe, 
                                            PlusService.Scope.UserinfoEmail, 
                                            PlusService.Scope.UserinfoProfile 
                                           }
                });

            _token = flow.ExchangeCodeForTokenAsync("me", authCode, "postmessage",
                    CancellationToken.None).Result;
            if (_token == null)
                throw new Exception("Error in getting access token.. invalid authentication code :   " + authCode);
            //// Get tokeninfo for the access token if you want to verify.
            Oauth2Service service = new Oauth2Service(new Google.Apis.Services.BaseClientService.Initializer());
            Oauth2Service.TokeninfoRequest request = service.Tokeninfo();
            request.AccessToken = _token.AccessToken;

            Tokeninfo info = request.Execute();
            return info != null ? info.UserId : string.Empty;
        }
        public GoogleAccount GetGoogleUserDetails(string userId)
        {
            GoogleAccount gacc = null;
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("userId can not be null or empty...");
                if (_token == null)
                    throw new Exception("Invalid Token. Please call ExchangeCode() for valid token...");

                IAuthorizationCodeFlow flow =
                    new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
                    {
                        ClientSecrets = _secrets,
                        Scopes = new string[] { PlusService.Scope.PlusLogin, 
                                            PlusService.Scope.PlusMe, 
                                            PlusService.Scope.UserinfoEmail, 
                                            PlusService.Scope.UserinfoProfile 
                                           }
                    });

                //var tToken;
                UserCredential credential = new UserCredential(flow, userId, _token);
                if (credential == null)
                    throw new Exception("Invalid userid :  " + userId + "   and Token :  " + _token.AccessToken);
                bool success = credential.RefreshTokenAsync(CancellationToken.None).Result;
                //_token = credential.Token;
                _ps = new PlusService(
                    new Google.Apis.Services.BaseClientService.Initializer()
                    {
                        ApplicationName = "Take65Web",
                        HttpClientInitializer = credential
                    });
                var me = _ps.People.Get("me").Execute();
                if (me == null)
                    throw new Exception("Error accessting google plus service.....");

                gacc = new GoogleAccount()
                {
                    UserId = me.Id,
                    UserDisplayName = me.DisplayName,
                    UserBirthday = me.Birthday,
                    UserGender = me.Gender,
                    AccessToken = _token.AccessToken,
                    Expiry = _token.ExpiresInSeconds,
                    RefreshToken = _token.RefreshToken,
                    EmailId = me.Emails.FirstOrDefault().Value,
                    LastUpdated = DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"))
                };
            }
            catch (Exception e)
            {
                throw;
            }
            return gacc;
        }
    }
}