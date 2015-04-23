using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Dal
{
    /// <summary>
    /// Manage User's Email Accounts
    /// </summary>
    public class UserEmailAccount
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");


        /// <summary>
        /// Get user's active email account
        /// </summary>
        /// <param name="pUserId">UserId</param>
        /// <returns></returns>
        public Model.EmailAccount Get(long pUserId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserEmailAccount");

            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, pUserId);

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.EmailAccount)(Model.Util.Deserialize(sXmlReturn, typeof(Model.EmailAccount)));
            else
                return null;
        }


        /// <summary>
        /// Save & Delete an Email Account
        /// Users only can have one active Email Account
        /// In case of trying to insert one of the same existing type, it will be updated
        /// In case of inserting a new email server type, the previous active account will be deleted
        /// </summary>
        /// <param name="pUserId"></param>
        /// <param name="pAccount"></param>
        public void Save(long pUserId, Model.EmailAccount pAccount)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserEmailAccount");

            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, pUserId);

            DbFactory.AddInParameter(cmd, "EmailServerType", DbType.String, pAccount.EmailServer.ToString());
            DbFactory.AddInParameter(cmd, "Username", DbType.String, pAccount.Username);
            DbFactory.AddInParameter(cmd, "Password", DbType.String, pAccount.Password);

            if (pAccount.Deleted != DateTime.MinValue)
                DbFactory.AddInParameter(cmd, "Deleted", DbType.String, pAccount.Deleted);

            
            DbFactory.ExecuteNonQuery(cmd);
        }
    }
}
