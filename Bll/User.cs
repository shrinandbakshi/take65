using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class User
    {
        private Dal.User DalUser = new Dal.User();

        public Model.User GetUser(int id)
        {
            return null;
        }

        public Model.User Get(long id)
        {
            return this.Get(new Model.User()
            {
                Id = id
            });
        }

        public List<Model.User> GetAll()
        {
            Model.User[] arrUser = DalUser.GetAll();
            if (arrUser != null)
                return arrUser.ToList();
            else
                return null;
        }

        public List<Model.User> GetAllNonRegisteredUsers() 
        {
            Model.User[] arrUser = DalUser.GetAllNonRegisteredUsers();
            if (arrUser != null)
                return arrUser.ToList();
            else
                return null;
        }

        public List<Model.User> GetAllRegisteredUsers()
        {
            Model.User[] arrUser = DalUser.GetAllRegisteredUsers();
            if (arrUser != null)
                return arrUser.ToList();
            else
                return null;
        }


        public List<Model.FacebookProfile> GetFacebookFriends(long pUserId)
        {
            Model.FacebookProfile[] arrFriends = this.DalUser.GetFacebookFriends(new Model.User()
            {
                Id = pUserId
            });
            if (arrFriends != null)
            {
                return arrFriends.ToList();
            }
            else
            {
                return null;
            }
        }

        /*
        public void SetFacebookFriends(long id, long facebookFriendId)
        {
            this.DalUser.SetFacebookFriends(new Model.User()
            {
                Id = id
            }, facebookFriendId);
        }*/

        public void SaveFaebookFriends(long pUserId, List<Model.FacebookProfile> pLtFriends)
        {
            DalUser.CleanFacebookFriendsList(pUserId, string.Join(",", pLtFriends.Select(x => x.Id.ToString()).ToArray()));
            foreach (Model.FacebookProfile fbProfile in pLtFriends)
            {
                DalUser.SaveFacebookFriend(pUserId, fbProfile);
            }
        }

        public Model.User Get(String facebookId)
        {
            return this.Get(new Model.User()
            {
                FacebookId = facebookId
            });
        }

        public void RegisterNewUserEmail(String email)
        {
            this.RegisterNewUserEmail(new Model.User()
            {
                Email = email
            });
        }

        public Model.User GetGoogleUser(string googleId)
        {
            return this.Get(new Model.User()
            {
                GoogleId = googleId
            });
        }

        public Model.User GetByGUID(String guid)
        {
            return this.Get(new Model.User()
            {
                GUID = guid
            });
        }

        public Model.User GetByEmail(String email)
        {
            return this.Get(new Model.User()
            {
                Email = email
            });
        }

        public Model.User GetByLogin(String login)
        {
            return this.Get(new Model.User()
            {
                Login = login
            });
        }

        public Model.User Get(String email, String password)
        {
            return this.Get(new Model.User()
            {
                Email = email,
                Password = password
            });
        }

        private Model.User Get(Model.User user)
        {
            return this.DalUser.Get(user);
        }

        private void RegisterNewUserEmail(Model.User user)
        {
            this.DalUser.RegisterNewUserEmail(user);
        }

        public int Save(Model.User user)
        {

            return this.DalUser.Save(user);
        }

        /*
        public Model.Widget[] GetUserWidget(int pPreviewContent)
        {
            Dal.Widget dalWidget = new Dal.Widget();
            Model.Widget[] UserWidgets = dalWidget.GetUserWidget(userId);

            if (UserWidgets != null)
            {
                if (pPreviewContent > 0)
                {
                    Bll.FeedContent bllFeedContent = new FeedContent();
                    for (int iW = 0; iW < UserWidgets.Length; iW++)
                    {
                        UserWidgets[iW].Content = bllFeedContent.GetContentWidget(UserWidgets[iW].Id, 1, pPreviewContent);
                    }
                }
            }
            return UserWidgets;
             

        }
    /*
        public Model.Widget GetUserWidgetById(long pWidgetId, int pPreviewContent)
        {
            Model.Widget UserWidgets = dalWidget.GetUserWidgetById(pWidgetId);
            if (UserWidgets != null)
            {
                if (pPreviewContent > 0)
                {
                    Bll.FeedContent bllFeedContent = new FeedContent();
                    UserWidgets.Content = bllFeedContent.GetContentWidget(UserWidgets.Id, 1, pPreviewContent);
                }
            }
            return UserWidgets;

        }
          */

        internal void UpdateFacebookToken(long pUserId, string pFacebookTokenShortLived, string pFacebookTokenLongLived, long pFacebookTokenLongLivedExpires, DateTime pDateExpires)
        {
            DalUser.UpdateFacebookToken(pUserId, pFacebookTokenShortLived, pFacebookTokenLongLived, pFacebookTokenLongLivedExpires, pDateExpires);
        }

        #region DeleteUser
        /// <summary>
        /// Delete the User and his references from Database for given User ID.
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(long userId)
        {
            DalUser.DeleteUser(userId);
        }
        #endregion
    }
}
