using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Dal
{
    public class SafeWebsite
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public Model.SafeWebsite[] Get()
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetSafeWebsite");



            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.SafeWebsite[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.SafeWebsite[])));
            else
                return null;
        }

        public Model.SafeWebsite[] GetUnmapped()
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetSafeWebsiteUnmapped");

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.SafeWebsite[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.SafeWebsite[])));
            else
                return null;
        }

        public Model.SafeWebsite[] Get(string pSafeWebsiteUrl)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetSafeWebsite");

            DbFactory.AddInParameter(cmd, "SafeWebsiteUrl", DbType.String, pSafeWebsiteUrl);

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.SafeWebsite[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.SafeWebsite[])));
            else
                return null;
        }

        public Model.SafeWebsite Get(int pSafeWebsite)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetSafeWebsite");

            if (pSafeWebsite != 0)
                DbFactory.AddInParameter(cmd, "SafeWebsiteId", DbType.Int32, pSafeWebsite);

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.SafeWebsite)(Model.Util.Deserialize(sXmlReturn, typeof(Model.SafeWebsite)));
            else
                return null;
        }

        public long Save(Model.SafeWebsite pWebsite)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveSafeWebsite");
            if (pWebsite.Id > 0)
                DbFactory.AddInParameter(cmd, "SafeWebsiteId", DbType.Int64, pWebsite.Id);

            if (!String.IsNullOrEmpty(pWebsite.Url))
                DbFactory.AddInParameter(cmd, "SafeWebsiteUrl", DbType.String, pWebsite.Url);

            DbFactory.AddInParameter(cmd, "SafeWebsiteOpenIFrame", DbType.Boolean, pWebsite.OpenIFrame);

            //if (pWebsite.CurrentTag == null && pWebsite.deleted != DateTime.MinValue)
            //    DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int64, 0);
            //else
            //    DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int64, pWebsite.CurrentTag.Id);

            if (pWebsite.deleted != null && DateTime.MinValue != pWebsite.deleted)
                DbFactory.AddInParameter(cmd, "deleted", DbType.DateTime, pWebsite.deleted);

            DbFactory.AddOutParameter(cmd, "ReturnId", DbType.Int32, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            return Int64.Parse(DbFactory.GetParameterValue(cmd, "ReturnId").ToString());
        }
    }
}
