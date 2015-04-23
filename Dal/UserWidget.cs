using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Dal
{
    public class UserWidget
    {

        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public Model.UserWidget GetUserWidget(long id)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserWidget");
            DbFactory.AddInParameter(cmd, "Id", DbType.Int64, id);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.UserWidget)(Model.Util.Deserialize(sXmlReturn, typeof(Model.UserWidget)));
            else
                return null;
        }

        public Model.Tag[] GetUserWidgetCategory(long userId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserWidgetCategory");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, userId);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.Tag[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.Tag[])));
            else
                return null;
        }

        public Model.UserWidget[] GetUserWidget(long userId, int systemTagId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserWidget");
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, userId);

            if (systemTagId > 0)
                DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int64, systemTagId);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);


            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.UserWidget[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.UserWidget[])));
            else
                return null;
        }

        public long Save(Model.UserWidget userWidget)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserWidget");
            if (userWidget.Id > 0)
                DbFactory.AddInParameter(cmd, "Id", DbType.Int32, userWidget.Id);
            if (userWidget.UserId > 0)
                DbFactory.AddInParameter(cmd, "UserId", DbType.Int32, userWidget.UserId);
            if (!String.IsNullOrEmpty(userWidget.Name))
                DbFactory.AddInParameter(cmd, "Name", DbType.String, userWidget.Name);
            if (userWidget.Size > 0)
                DbFactory.AddInParameter(cmd, "Size", DbType.Int32, userWidget.Size);
            if (userWidget.Order > 0)
                DbFactory.AddInParameter(cmd, "Order", DbType.Int32, userWidget.Order);
            if (userWidget.SystemTagId > 0)
                DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int32, userWidget.SystemTagId);

            DbFactory.AddInParameter(cmd, "DefaultWidget", DbType.Boolean, userWidget.DefaultWidget);


            DbFactory.AddOutParameter(cmd, "ReturnId", DbType.Int32, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            return Int64.Parse(DbFactory.GetParameterValue(cmd, "ReturnId").ToString());
        }

        public void SavePosition(Model.UserWidget userWidget)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserWidgetPosition");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int32, userWidget.Id);
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int32, userWidget.UserId);

            DbFactory.AddInParameter(cmd, "WidgetRow", DbType.Int32, userWidget.Row);
            DbFactory.AddInParameter(cmd, "WidgetColumn", DbType.Int32, userWidget.Col);
            
            DbFactory.ExecuteNonQuery(cmd);
        }

        public void Delete(long userWidgetId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("DeleteUserWidget");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int32, userWidgetId);
            DbFactory.ExecuteNonQuery(cmd);
        }

        public void DeleteTrustedSource(long userWidgetId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("DeleteUserWidgetTrustedSource");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int32, userWidgetId);
            DbFactory.ExecuteNonQuery(cmd);
        }

        public void SaveTrustedSource(long userWidgetId, int trustedSourceId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserWidgetTrustedSource");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int32, userWidgetId);
            DbFactory.AddInParameter(cmd, "TrustedSourceId", DbType.Int32, trustedSourceId);
            DbFactory.ExecuteNonQuery(cmd);
        }


        public void SaveExtraInfo(long pUserId, long pUserWidgetId, string pExtraInfo)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveUserWidgetExtraInfo");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int32, pUserWidgetId);
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int32, pUserId);
            DbFactory.AddInParameter(cmd, "UserWidgetExtraInfoXML", DbType.String, pExtraInfo);
            DbFactory.ExecuteNonQuery(cmd);
        }
    }
}
