using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class UserPreference
    {
        private Dal.UserPreference DalUserPreference = new Dal.UserPreference();

        public void Delete(int userId)
        {
            this.DalUserPreference.Delete(userId);
        }

        public void Save(long userId, int systemTagId)
        {
            this.DalUserPreference.Save(userId, systemTagId);
        }
    }
}
