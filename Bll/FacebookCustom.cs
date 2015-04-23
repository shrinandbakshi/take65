using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Bll
{
    public class FacebookCustom
    {
        private string AppId { get; set; }
        private string AppSecret { get; set; }
        private string Token { get; set; }

        public FacebookCustom(string pAppId, string pAppSecret)
        {
            AppId = pAppId;
            AppSecret = pAppSecret;
        }

        public FacebookCustom(string pAppId, string pAppSecret, string pToken)
        {
            AppId = pAppId;
            AppSecret = pAppSecret;
            Token = pToken;
        }

        public string GetTokenLongLive(long pUserId, string pFacebookTokenShortLived)
        {
            Bll.User bllUser = new User();
            Model.User user = bllUser.Get(pUserId);
            if (user.FacebookTokenShortLived != pFacebookTokenShortLived)
            {
                var client = new WebClient();
                string html = client.DownloadString(string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=fb_exchange_token&fb_exchange_token={2}", AppId, AppSecret, pFacebookTokenShortLived));

                NameValueCollection qsToken = HttpUtility.ParseQueryString(html);
                DateTime expiresDate = DateTime.UtcNow;
                bllUser.UpdateFacebookToken(pUserId, pFacebookTokenShortLived, qsToken["access_token"], Convert.ToInt64(qsToken["expires"]), expiresDate.AddSeconds(Convert.ToDouble(qsToken["expires"])));
                return qsToken["access_token"];
            }
            else
            {
                return user.FacebookTokenLongLived;
            }
        }

        public bool IsValidToken(long pUserId, string pFacebookToken)
        {
            Bll.User bllUser = new User();

            

            try
            {
                var client = new WebClient();
                string html = client.DownloadString(string.Format("https://graph.facebook.com/me?access_token={0}", pFacebookToken));
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                dynamic routes_list =
                       (object)json_serializer.DeserializeObject(html);

                if (routes_list["id"] != null)
                {
                    return true;
                }
                else
                {
                    bllUser.UpdateFacebookToken(pUserId, null, null, 0, DateTime.MinValue);
                    return false;
                }
            }
            catch
            {
                bllUser.UpdateFacebookToken(pUserId, null, null, 0, DateTime.MinValue);
                return false;
            }
        }

        public List<Model.FacebookProfile> GetFacebookFriends()
        {
            var client = new WebClient();
            string html = client.DownloadString(string.Format("https://graph.facebook.com/me/friends?limit=5000&access_token={0}", Token));
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            dynamic jsonFriends =
                   (object)json_serializer.DeserializeObject(html);

            /* Get Current Logged User Info */
            string htmlMe = client.DownloadString(string.Format("https://graph.facebook.com/me?access_token={0}", Token));
            JavaScriptSerializer json_serializerMe = new JavaScriptSerializer();
            dynamic jsonFriendsMe =
                   (object)json_serializerMe.DeserializeObject(htmlMe);
            /* Get Current Logged User Info */


            List<Model.FacebookProfile> ltFriends = new List<Model.FacebookProfile>();
            ltFriends.Add(new Model.FacebookProfile
            {
                Id = Convert.ToInt64(jsonFriendsMe["id"].ToString().Replace("\"", "")),
                Name = jsonFriendsMe["name"].ToString().Replace("\"", "") + " (You)",
                PhotoCount = -1
            });

            foreach (var friend in jsonFriends["data"])
            {
                ltFriends.Add(new Model.FacebookProfile
                {
                    Id = Convert.ToInt64(friend["id"].ToString().Replace("\"", "")),
                    Name = friend["name"].ToString().Replace("\"", ""),
                    PhotoCount = -1
                });
            }
            return ltFriends;
        }

        public List<Model.FacebookPhoto> GetPhotos(string pFacebookUserId)
        {

            string FQL = string.Format("SELECT pid, caption, src_big, src_big_height, src_big_width, created FROM photo WHERE aid IN (SELECT aid FROM album WHERE owner={0}) ORDER BY created DESC", pFacebookUserId);

            var client = new WebClient();
            string html = client.DownloadString(string.Format("https://graph.facebook.com/fql?q={0}&access_token={1}", FQL, Token));
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            dynamic jsonPhotos =
                   (object)json_serializer.DeserializeObject(html);

            List<Model.FacebookPhoto> ltPhotos = new List<Model.FacebookPhoto>();
            foreach (var photo in jsonPhotos["data"])
            {
                try
                {
                    Model.FacebookPhoto newPhoto = new Model.FacebookPhoto();

                    newPhoto.Id = Convert.ToInt64(photo["pid"].ToString());
                    if (photo["caption"] != null)
                    {
                        newPhoto.Name = photo["caption"].ToString().Replace("\"", "");
                    }
                    
                    //newPhoto.Thumb = photo["picture"].ToString().Replace("\"", "");
                    newPhoto.Photo = photo["src_big"].ToString().Replace("\"", "");
                    newPhoto.Width = Convert.ToInt32(photo["src_big_width"].ToString());
                    newPhoto.Height = Convert.ToInt32(photo["src_big_height"].ToString());
                    newPhoto.CreatedTime = Bll.Util.UnixTimeStampToDateTime(Convert.ToDouble(photo["created"]));
                    ltPhotos.Add(newPhoto);
                }
                catch
                {

                }
            }

            return ltPhotos;
        }
    }
}
