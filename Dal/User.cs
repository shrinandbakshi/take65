using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Dal
{
    public class User
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");


        public Model.User Get(Model.User pUser)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUser");

            if (pUser.Id > 0)
                DbFactory.AddInParameter(cmd, "Id", DbType.Int32, pUser.Id);
            if (!String.IsNullOrEmpty(pUser.Login))
                DbFactory.AddInParameter(cmd, "Login", DbType.String, pUser.Login);
            if (!String.IsNullOrEmpty(pUser.Password))
                DbFactory.AddInParameter(cmd, "Password", DbType.String, pUser.Password);
            if (!String.IsNullOrEmpty(pUser.FacebookId))
                DbFactory.AddInParameter(cmd, "FacebookId", DbType.String, pUser.FacebookId);
            if (!String.IsNullOrEmpty(pUser.GoogleId))
                DbFactory.AddInParameter(cmd, "GoogleId", DbType.String, pUser.GoogleId);
            if (!String.IsNullOrEmpty(pUser.Email))
                DbFactory.AddInParameter(cmd, "Email", DbType.String, pUser.Email);
            if (!String.IsNullOrEmpty(pUser.GUID))
                DbFactory.AddInParameter(cmd, "GUID", DbType.String, pUser.GUID);

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.User)(Model.Util.Deserialize(sXmlReturn, typeof(Model.User)));
            else
                return null;
        }

        public void RegisterNewUserEmail(Model.User pUser)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("usp_RegisterNewUserEmail");
            if (!String.IsNullOrEmpty(pUser.Email))
                DbFactory.AddInParameter(cmd, "email", DbType.String, pUser.Email);
            DbFactory.ExecuteNonQuery(cmd);
        }

        public Model.User[] GetAll()
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserAll");


            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.User[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.User[])));
            else
                return null;
        }

        public Model.User[] GetAllNonRegisteredUsers()
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetAllNonRegisteredUsers");


            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.User[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.User[])));
            else
                return null;
        }

        public Model.User[] GetAllRegisteredUsers()
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetAllRegisteredUsers");


            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.User[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.User[])));
            else
                return null;
        }

        public Model.FacebookProfile[] GetFacebookFriends(Model.User user)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserFacebookFriends");
            DbFactory.AddInParameter(cmd, "Id", DbType.Int64, user.Id);
            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);

            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();

            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.FacebookProfile[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.FacebookProfile[])));
            else
                return null;
        }

        public void CleanFacebookFriendsList(long pUserId, string pFacebookIdList)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("UserFacebookFriendClean");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, pUserId);
            if (!string.IsNullOrEmpty(pFacebookIdList))
            {
                DbFactory.AddInParameter(cmd, "FacebookFriendList", DbType.String, pFacebookIdList);
            }
            DbFactory.ExecuteNonQuery(cmd);
        }

        public void SaveFacebookFriend(long pUserId, Model.FacebookProfile pProfile)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveFacebookFriend");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, pUserId);
            DbFactory.AddInParameter(cmd, "FacebookFriendId", DbType.Int64, pProfile.Id);
            DbFactory.AddInParameter(cmd, "FacebookFriendName", DbType.String, pProfile.Name);
            DbFactory.AddInParameter(cmd, "FacebookPhotoCount", DbType.Int32, pProfile.PhotoCount);

            DbFactory.ExecuteNonQuery(cmd);
        }

        public int Save(Model.User user)
        {
            DbCommand cmd = this.DbFactory.GetStoredProcCommand("SaveUser");

            if (user.Id > 0)
                this.DbFactory.AddInParameter(cmd, "Id", DbType.Int32, user.Id);
            if (!String.IsNullOrEmpty(user.FacebookId))
                this.DbFactory.AddInParameter(cmd, "FacebookId", DbType.String, user.FacebookId);
            if (!String.IsNullOrEmpty(user.GoogleId))
                this.DbFactory.AddInParameter(cmd, "GoogleId", DbType.String, user.GoogleId);
            if (!String.IsNullOrEmpty(user.GoogleAccessToken))
                this.DbFactory.AddInParameter(cmd, "GoogleAccessToken", DbType.String, user.GoogleAccessToken);
            if (!String.IsNullOrEmpty(user.GoogleRefreshToken))
                this.DbFactory.AddInParameter(cmd, "GoogleRefreshToken", DbType.String, user.GoogleRefreshToken);
            if (!String.IsNullOrEmpty(user.GoogleId))
                this.DbFactory.AddInParameter(cmd, "GoogleAccessTokenExpires", DbType.Int64, user.GoogleAccessTokenExpires);
            if (!String.IsNullOrEmpty(user.Name))
                this.DbFactory.AddInParameter(cmd, "Name", DbType.String, user.Name);
            if (!String.IsNullOrEmpty(user.Login))
                this.DbFactory.AddInParameter(cmd, "Login", DbType.String, user.Login);
            if (!String.IsNullOrEmpty(user.Email))
                this.DbFactory.AddInParameter(cmd, "Email", DbType.String, user.Email);
            if (!String.IsNullOrEmpty(user.Password))
                this.DbFactory.AddInParameter(cmd, "Password", DbType.String, user.Password);

            if (user.ChangePasswordNextLogin != DateTime.MinValue)
                this.DbFactory.AddInParameter(cmd, "ChangePassword", DbType.DateTime, user.ChangePasswordNextLogin);


            if (user.Birthdate != DateTime.MinValue)
                this.DbFactory.AddInParameter(cmd, "Birthdate", DbType.DateTime, user.Birthdate);

            if (!string.IsNullOrEmpty(user.Gender))
                this.DbFactory.AddInParameter(cmd, "Gender", DbType.String, user.Gender);

            if (!String.IsNullOrEmpty(user.AddressState))
                this.DbFactory.AddInParameter(cmd, "AddressState", DbType.String, user.AddressState);
            if (!String.IsNullOrEmpty(user.AddressCity))
                this.DbFactory.AddInParameter(cmd, "AddressCity", DbType.String, user.AddressCity);
            if (!String.IsNullOrEmpty(user.AddressPostalCode))
                this.DbFactory.AddInParameter(cmd, "AddressPostalCode", DbType.String, user.AddressPostalCode);

            if (user.Preferences != null)
                this.DbFactory.AddInParameter(cmd, "Preferences", DbType.String, string.Join(",", user.Preferences));

            if (user.Deleted != DateTime.MinValue)
                this.DbFactory.AddInParameter(cmd, "deleted", DbType.DateTime, user.Deleted);

            this.DbFactory.AddOutParameter(cmd, "ReturnId", DbType.Int32, int.MaxValue);
            this.DbFactory.ExecuteNonQuery(cmd);

            return Int32.Parse(this.DbFactory.GetParameterValue(cmd, "ReturnId").ToString());
        }

        public void UpdateFacebookToken(long pUserId, string pFacebookTokenShortLived, string pFacebookTokenLongLived, long pFacebookTokenLongLivedExpires, DateTime pDateExpires)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserFacebookToken");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, pUserId);
            DbFactory.AddInParameter(cmd, "FacebookTokenShort", DbType.String, pFacebookTokenShortLived);
            DbFactory.AddInParameter(cmd, "FacebookTokenLong", DbType.String, pFacebookTokenLongLived);
            DbFactory.AddInParameter(cmd, "FacebookTokenLongExpires", DbType.Int64, pFacebookTokenLongLivedExpires);
            if (pDateExpires != DateTime.MinValue)
                DbFactory.AddInParameter(cmd, "FacebookTokenLongExpiresDate", DbType.DateTime, pDateExpires);


            DbFactory.ExecuteNonQuery(cmd);
        }

        #region DeleteUser
        /// <summary>
        /// Delete the User and his references from Database for given User ID.
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(long userId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SpDeleteUser");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int32, userId);
            DbFactory.ExecuteNonQuery(cmd);
        }
        #endregion
    }
}
