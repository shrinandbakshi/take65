using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Dal
{
    public class TrustedSource
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public Model.Category[] GetCategory()
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetCategory");

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.Category[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.Category[])));
            else
                return null;
        }

        public Model.Category[] GetCategory(long pUserId, long pUserWidgetId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetCategory");

            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, pUserId);
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int64, pUserWidgetId);

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.Category[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.Category[])));
            else
                return null;
        }

        public Model.TrustedSource[] GetTrustedSource(Model.Enum.enTrustedSourceType pSourceType, long pSystemTagId, long pUserWidgetId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetTrustedSource");

            if (pSourceType != Model.Enum.enTrustedSourceType.DEFAULT)
                DbFactory.AddInParameter(cmd, "TrustedSourceTypeId", DbType.Int32, Convert.ToInt32(pSourceType));

            if (pSystemTagId > 0)
                DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int32, pSystemTagId);

            if (pUserWidgetId > 0)
                DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int32, pUserWidgetId);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.TrustedSource[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.TrustedSource[])));
            else
                return null;
        }

        public Model.TrustedSource GetTrustedSourceById(long pTrustedSourceId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetTrustedSource");

            DbFactory.AddInParameter(cmd, "TrustedSourceId", DbType.Int64, pTrustedSourceId);


            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.TrustedSource)(Model.Util.Deserialize(sXmlReturn, typeof(Model.TrustedSource)));
            else
                return null;
        }


        public Model.TrustedSourceFeed[] GetTrustedSourceFeedList(long pTrustedSourceId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetTrustedSourceFeed");
            DbFactory.AddInParameter(cmd, "TrustedSourceId", DbType.Int64, pTrustedSourceId);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.TrustedSourceFeed[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.TrustedSourceFeed[])));
            else
                return null;
        }


        public long Save(Model.TrustedSource pTrustedSource)
        {
            if (pTrustedSource.TypeId == Convert.ToInt64(Model.Enum.enTrustedSourceType.BOOKMARK))
            {
                DbCommand cmd = DbFactory.GetStoredProcCommand("SaveTrustedSourceBookmark");

                if (pTrustedSource.Id != 0)
                    DbFactory.AddInParameter(cmd, "TrustedSourceId", DbType.Int64, pTrustedSource.Id);

                DbFactory.AddInParameter(cmd, "TrustedSourceName", DbType.String, pTrustedSource.Name);
                DbFactory.AddInParameter(cmd, "TrustedSourceUrl", DbType.String, pTrustedSource.Url);
                if (pTrustedSource.CurrentTag == null && pTrustedSource.deleted != DateTime.MinValue)
                {
                    DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int64, 0);
                }
                else
                {
                    DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int64, pTrustedSource.CurrentTag.Id);
                }
                DbFactory.AddInParameter(cmd, "TrustedSourceIcon", DbType.String, pTrustedSource.Icon);
                DbFactory.AddInParameter(cmd, "TrustedSourceOpenIFrame", DbType.Boolean, pTrustedSource.OpenIFrame);
                if (pTrustedSource.deleted != DateTime.MinValue)
                    DbFactory.AddInParameter(cmd, "deleted", DbType.DateTime, pTrustedSource.deleted);


                DbFactory.AddOutParameter(cmd, "ReturnId", DbType.Int64, int.MaxValue);
                DbFactory.ExecuteNonQuery(cmd);

                return Convert.ToInt64(DbFactory.GetParameterValue(cmd, "ReturnId"));

            }
            else
            {
                return 0;
            }
        }
    }
}
