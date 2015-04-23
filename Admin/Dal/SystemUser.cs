using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Layers.Admin.Dal
{
    public class SystemUser
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("AdminTest");
        private new NetBiis.Library.Serialization.Deserialize oDeserialize = new NetBiis.Library.Serialization.Deserialize();

        public Model.SystemUser AuthenticateSystemUser(string pUserName, string pPassword)
        {
            DbCommand command = DbFactory.GetStoredProcCommand("AuthenticateSystemUser");

            DbFactory.AddInParameter(command, "UserName", DbType.String, pUserName);
            DbFactory.AddInParameter(command, "Password", DbType.String, pPassword);
            DbFactory.AddOutParameter(command, "XmlReturn", DbType.Xml, int.MaxValue);

            DbFactory.ExecuteNonQuery(command);

            String sXmlReturn = DbFactory.GetParameterValue(command, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.SystemUser)(oDeserialize.Deserializer(sXmlReturn, typeof(Model.SystemUser)));
            else
                return null;

        }
    }
}
