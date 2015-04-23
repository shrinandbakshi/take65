using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Dal
{
    public class UserWidgetTrustedSource
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public Model.UserWidgetTrustedSource[] Get(long userWidgetId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserWidgetTrustedSourceNew");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int64, userWidgetId);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.UserWidgetTrustedSource[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.UserWidgetTrustedSource[])));
            else
                return null;
        }

        public void DeleteTrustedSource(long userWidgetId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("DeleteUserWidgetTrustedSource");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int32, userWidgetId);
            DbFactory.ExecuteNonQuery(cmd);
        }

        public void SaveTrustedSource(Model.UserWidgetTrustedSource uwts)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserWidgetTrustedSource");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int32, uwts.UserWidgetId);

            if (uwts.TrustedSourceId > 0)
                DbFactory.AddInParameter(cmd, "TrustedSourceId", DbType.Int32, uwts.TrustedSourceId);
            if (uwts.CategoryId > 0)
                DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int32, uwts.CategoryId);
            if (!String.IsNullOrEmpty(uwts.Name))
                DbFactory.AddInParameter(cmd, "Name", DbType.String, uwts.Name);
            if (!String.IsNullOrEmpty(uwts.Url))
                DbFactory.AddInParameter(cmd, "Url", DbType.String, uwts.Url);

            DbFactory.ExecuteNonQuery(cmd);
        }


    }
}
