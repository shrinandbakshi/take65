using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Dal
{
    public class Tag
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public Model.TagList GetSystemTag(Int64 pId, Int64 pParentId, int pTypeId, short pLocaleId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetSystemTag");

            if (pId > 0)
                DbFactory.AddInParameter(cmd, "Id", DbType.Int64, pId);
            if (pParentId > 0)
                DbFactory.AddInParameter(cmd, "ParentId", DbType.Int64, pParentId);
            if (pTypeId > 0)
                DbFactory.AddInParameter(cmd, "TagTypeId", DbType.Int32, pTypeId);
            if (pLocaleId > 0)
                DbFactory.AddInParameter(cmd, "LocaleId", DbType.Int16, pLocaleId);
            
            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.TagList)(Model.Util.Deserialize(sXmlReturn, typeof(Model.TagList)));
            else
                return null;
        }

        public Model.TagList GetTagType(int TagTypeId, string TagList)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetTagType");

            if (TagTypeId > 0)
                DbFactory.AddInParameter(cmd, "TagTypeId", DbType.Int32, TagTypeId);

            if (!string.IsNullOrEmpty(TagList))
                DbFactory.AddInParameter(cmd, "IdList", DbType.String, TagList);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);
            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.TagList)(Model.Util.Deserialize(sXmlReturn, typeof(Model.TagList)));
            else
                return null;
        }

        

    }
}
