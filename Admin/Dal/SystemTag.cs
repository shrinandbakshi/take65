using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Layers.Admin.Dal
{
    public class SystemTag
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("AdminTest");
        private new NetBiis.Library.Serialization.Deserialize oDeserialize = new NetBiis.Library.Serialization.Deserialize();

        public Int32 Save(int pSystemTagId, string pSystemTagParentId, string pSystemTagParentIdList, String pSystemTagNormalized, string pSystemTagDisplay, String pSystemTagIcon, int pSystemTagOrder, int pTagTypeId, string pDeleted = null)
        {
            DbCommand command = DbFactory.GetStoredProcCommand("SaveSystemTag");
            if (pSystemTagId != 0)
                DbFactory.AddInParameter(command, "SystemTagId", DbType.Int32, pSystemTagId);
            DbFactory.AddInParameter(command, "SystemTagParentId", DbType.String, pSystemTagParentId);
            DbFactory.AddInParameter(command, "SystemTagParentIdList", DbType.String, pSystemTagParentIdList);
            DbFactory.AddInParameter(command, "SystemTagNormalized", DbType.String, pSystemTagNormalized);
            DbFactory.AddInParameter(command, "SystemTagDisplay", DbType.String, pSystemTagDisplay);
            DbFactory.AddInParameter(command, "SystemTagIcon", DbType.String, pSystemTagIcon);
            DbFactory.AddInParameter(command, "SystemTagOrder", DbType.String, pSystemTagOrder);
            DbFactory.AddInParameter(command, "TagTypeId", DbType.String, pTagTypeId);
            DbFactory.AddInParameter(command, "Deleted", DbType.DateTime, pDeleted);
            DbFactory.AddOutParameter(command, "ReturnId", DbType.Int32, int.MaxValue);
            return DbFactory.ExecuteNonQuery(command);
        }

        public Model.SystemTag Get(int pSystemTagId)
        {
            DbCommand command = DbFactory.GetStoredProcCommand("GetSystemTag");

            if (pSystemTagId != 0)
                DbFactory.AddInParameter(command, "SystemTagId", DbType.Int32, pSystemTagId);

            DbFactory.AddOutParameter(command, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(command);

            String sXmlReturn = DbFactory.GetParameterValue(command, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.SystemTag)(oDeserialize.Deserializer(sXmlReturn, typeof(Model.SystemTag)));
            else
                return null;
        }

        public Model.SystemTagList Get(int pSystemTagId, string pSystemTagNormalized, int pSystemTagParentId, int pTagTypeId, int pTagTypeParentId)
        {
            DbCommand command = DbFactory.GetStoredProcCommand("GetSystemTag");

            if (pSystemTagId != 0)
                DbFactory.AddInParameter(command, "SystemTagId", DbType.Int32, pSystemTagId);

            DbFactory.AddInParameter(command, "SystemTagNormalized", DbType.String, pSystemTagNormalized);

            if (pSystemTagParentId != 0)
                DbFactory.AddInParameter(command, "SystemTagParentId", DbType.Int32, pSystemTagParentId);

            if (pTagTypeId != 0)
                DbFactory.AddInParameter(command, "TagTypeId", DbType.Int32, pTagTypeId);

            if (pTagTypeParentId != 0)
                DbFactory.AddInParameter(command, "TagTypeParentId", DbType.Int32, pTagTypeParentId);

            DbFactory.AddOutParameter(command, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(command);

            String sXmlReturn = DbFactory.GetParameterValue(command, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.SystemTagList)(oDeserialize.Deserializer(sXmlReturn, typeof(Model.SystemTagList)));
            else
                return null;
        }
    }
}