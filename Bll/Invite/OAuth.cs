using System;
using System.Text;

using System.Collections.Generic;
using System.Collections.Specialized;

using System.Net;
using System.Web;

using System.IO;
using System.Globalization; //oAuth timestamp #todo check if can be used without it

namespace Bll.Invite
{
    public class OAuthSetting
    {
        public String UrlRequest { get; set; }
        public String UrlAuthorize { get; set; }
        public String UrlAccess { get; set; }

        public String ConsumerKey { get; set; }
        public String ConsumerKeySecret { get; set; }
        public String ConsumerCallback { get; set; }

        public String OAuthAccessToken { get; set; }
        public String OAuthAccessTokenSecret { get; set; }
    }

    public class OAuth
    {
        private OAuthSetting OAuthSetting = null;
        
        public OAuth(OAuthSetting oAuthSetting)
        {
            this.OAuthSetting = oAuthSetting;
        }

        private Dictionary<String, String> GetOAuthBasicParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("oauth_nonce", this.GetOAuthNonce());
            parameters.Add("oauth_timestamp", this.GetOAuthTimestamp());
            parameters.Add("oauth_version", "1.0");
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_consumer_key", this.OAuthSetting.ConsumerKey);

            return parameters;
        }

        private String GetOAuthNonce()
        {
            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        }

        private String GetOAuthTimestamp()
        {
            return Convert.ToInt64(((TimeSpan)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds).ToString(CultureInfo.CurrentCulture);
        }

        private string OAuthCalculateSignature(String url, Dictionary<string, string> parameters)
        {
            string baseString = "";
            string key = this.OAuthSetting.ConsumerKeySecret + "&" + this.OAuthSetting.OAuthAccessTokenSecret;
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            SortedList<string, string> sorted = new SortedList<string, string>();
            foreach (KeyValuePair<string, string> pair in parameters) { sorted.Add(pair.Key, pair.Value); }


            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in sorted)
            {
                sb.Append(pair.Key);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(pair.Value));
                sb.Append("&");
            }

            sb.Remove(sb.Length - 1, 1);

            baseString = "POST" + "&" + Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(sb.ToString());

            System.Security.Cryptography.HMACSHA1 sha1 = new System.Security.Cryptography.HMACSHA1(keyBytes);

            byte[] hashBytes = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(baseString));

            string hash = Convert.ToBase64String(hashBytes);

            return hash;
        }

        public String GetAccessUrl(OAuthPermission permission)
        {
            String permissionString = "";
            switch (permission)
            {
                case OAuthPermission.READ:
                    permissionString = "read";
                    break;
                case OAuthPermission.WRITE:
                    permissionString = "write";
                    break;
            }

            return this.OAuthSetting.UrlAuthorize + "?oauth_token=" + this.OAuthSetting.OAuthAccessToken + "&perms=" + permissionString;
        }

        public OAuthSetting GetSecretToken()
        {
            string url = this.OAuthSetting.UrlRequest;

            Dictionary<string, string> parameters = this.GetOAuthBasicParameters();

            parameters.Add("oauth_callback", this.OAuthSetting.ConsumerCallback);

            string sig = OAuthCalculateSignature(this.OAuthSetting.UrlRequest, parameters);

            parameters.Add("oauth_signature", sig);

            string response = this.getDataResponse(url, parameters);

            if (response.Length > 0)
            {
                NameValueCollection query = HttpUtility.ParseQueryString(response);

                if (query["oauth_token"] != null)
                    this.OAuthSetting.OAuthAccessToken = query["oauth_token"];

                if (query["oauth_token_secret"] != null)
                    this.OAuthSetting.OAuthAccessTokenSecret = query["oauth_token_secret"];
            }

            return this.OAuthSetting;
        }

        private String getDataResponse(string baseUrl, Dictionary<string, string> parameters)
        {

            // Calculate post data, content header and auth header
            string data = OAuthCalculatePostData(parameters);
            string authHeader = OAuthCalculateAuthHeader(parameters);

            // Download data.
            try
            {

                return DownloadData(baseUrl, data, "application/x-www-form-urlencoded", authHeader);
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.ProtocolError) throw;

                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (response == null) throw;

                if (response.StatusCode != HttpStatusCode.BadRequest && response.StatusCode != HttpStatusCode.Unauthorized) throw;

                using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                {
                    string responseData = responseReader.ReadToEnd();
                    responseReader.Close();

                    return responseData;
                }
            }
        }

        private static string DownloadData(string baseUrl, string data, string contentType, string authHeader)
        {
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Netbiis / Social Cast Alpha");
            if (!String.IsNullOrEmpty(contentType)) client.Headers.Add("Content-Type", contentType);
            if (!String.IsNullOrEmpty(authHeader)) client.Headers.Add("Authorization", authHeader);

            return client.UploadString(baseUrl, data);
        }

        /// <summary>
        /// Returns the string for the Authorisation header to be used for OAuth authentication.
        /// Parameters other than OAuth ones are ignored.
        /// </summary>
        /// <param name="parameters">OAuth and other parameters.</param>
        /// <returns></returns>
        private string OAuthCalculateAuthHeader(Dictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder("OAuth ");
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                if (pair.Key.StartsWith("oauth"))
                {
                    sb.Append(pair.Key + "=\"" + Uri.EscapeDataString(pair.Value) + "\",");
                }
            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        /// <summary>
        /// Calculates for form encoded POST data to be included in the body of an OAuth call.
        /// </summary>
        /// <remarks>This will include all non-OAuth parameters. The OAuth parameter will be included in the Authentication header.</remarks>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string OAuthCalculatePostData(Dictionary<string, string> parameters)
        {
            string data = String.Empty;
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                if (!pair.Key.StartsWith("oauth"))
                {
                    data += pair.Key + "=" + Uri.EscapeDataString(pair.Value) + "&";
                }
            }
            return data;
        }

        public String OAuthGetAuthorizeToken(string token, string tokenSecret, string verifier)
        {
            this.OAuthSetting.OAuthAccessToken = token;
            this.OAuthSetting.OAuthAccessTokenSecret = tokenSecret;

            Dictionary<string, string> parameters = this.GetOAuthBasicParameters();

            parameters.Add("oauth_verifier", verifier);
            parameters.Add("oauth_token", token);

            string sig = OAuthCalculateSignature(this.OAuthSetting.OAuthAccessToken, parameters);
                
            parameters.Add("oauth_signature", sig);

            string response = this.getDataResponse(this.OAuthSetting.OAuthAccessToken, parameters);

            if (response.Length > 0)
            {
                NameValueCollection query = HttpUtility.ParseQueryString(response);


                return query["oauth_token_secret"];

            }




            return response;
        }

    }

    public enum OAuthPermission
    {
        DEFAULT = 0,
        READ = 1,
        WRITE = 2,
        DELETE = 3
    }
}