using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Dal
{
    public class UserPreference
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public void Delete(int userId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("DeleteUserPreference");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int32, userId);
            DbFactory.ExecuteNonQuery(cmd);
        }

        public void Save(long userId, int systemTagId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserPreference");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int32, userId);
            DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int32, systemTagId);
            DbFactory.ExecuteNonQuery(cmd);
        }
    }
}
