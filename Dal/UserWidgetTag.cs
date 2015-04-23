using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Dal
{
    public class UserWidgetTag
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public void Save(Model.UserWidgetTag userWidgetTag)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserWidgetTag");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, userWidgetTag.UserId);
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int64, userWidgetTag.UserWidgetId);
            DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int64, userWidgetTag.SystemTagId);
            DbFactory.ExecuteNonQuery(cmd);
        }
    }
}
