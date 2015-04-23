using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Layers.Admin.Dal
{
    public class PageAdmin
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("AdminTest");
        private new NetBiis.Library.Serialization.Deserialize oDeserialize = new NetBiis.Library.Serialization.Deserialize();

        public Model.PageAdmin Get(string AdminLink)
        {
            DbCommand command = DbFactory.GetStoredProcCommand("GetPageAdmin");

            if (!string.IsNullOrEmpty(AdminLink))
                DbFactory.AddInParameter(command, "PageAdminLink", DbType.String, AdminLink);

            DbFactory.AddOutParameter(command, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(command);

            String sXmlReturn = DbFactory.GetParameterValue(command, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.PageAdmin)(oDeserialize.Deserializer(sXmlReturn, typeof(Model.PageAdmin)));
            else
                return null;
        }

        public Model.PageAdminList Get()
        {
            DbCommand command = DbFactory.GetStoredProcCommand("GetPageAdmin");

            DbFactory.AddOutParameter(command, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(command);

            String sXmlReturn = DbFactory.GetParameterValue(command, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.PageAdminList)(oDeserialize.Deserializer(sXmlReturn, typeof(Model.PageAdminList)));
            else
                return null;
        }
    }
}
