using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{

    /// <summary>
    /// Manage User's Email Accounts
    /// </summary>
    public class UserEmailAccount
    {
        Dal.UserEmailAccount dalEmailAccount = new Dal.UserEmailAccount();

        /// <summary>
        /// Get user's active email account
        /// </summary>
        /// <param name="pUserId"></param>
        /// <returns></returns>
        public Model.EmailAccount Get(long pUserId)
        {
            Model.EmailAccount account = dalEmailAccount.Get(pUserId);
            if (account != null)
            {
                if (!string.IsNullOrEmpty(account.Password))
                {
                    Bll.EncryptAES enc = new EncryptAES();
                    account.Password = enc.DecryptString(account.Password);
                }
                return account;
            }
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
            if (pUserId != 0)
            {
                if (!string.IsNullOrEmpty(pAccount.Password)){
                    Bll.EncryptAES enc = new EncryptAES();
                    pAccount.Password = enc.EncryptToString(pAccount.Password);
                }
                dalEmailAccount.Save(pUserId, pAccount);
            }else
                throw new InvalidOperationException("Missing UserId");
        }

    }
}
