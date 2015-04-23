using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Dal
{
    public class Widget
    {

        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        
        

        public Model.UserWidget GetUserWidgetById(long pWidgetId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserWidget");
            DbFactory.AddInParameter(cmd, "Id", DbType.Int64, pWidgetId);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            return null;

            /*
            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.Widget)(Model.Util.Deserialize(sXmlReturn, typeof(Model.Widget)));
            else
                return null;
             */
        }
    }
}
