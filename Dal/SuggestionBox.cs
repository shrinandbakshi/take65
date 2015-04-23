using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Dal
{
    public class SuggestionBox
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public Model.SuggestionBox[] Get(int pSuggestionId, string pSuggestionName, string pSuggestionUrl, string pSuggestionDescription, string pSuggestionImage, long pUserId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetSuggestionBox");

            if (pSuggestionId != 0)
                DbFactory.AddInParameter(cmd, "SuggestionId", DbType.Int32, pSuggestionId);
            if (!string.IsNullOrEmpty(pSuggestionName))
                DbFactory.AddInParameter(cmd, "SuggestionName", DbType.Int32, pSuggestionName);
            if (!string.IsNullOrEmpty(pSuggestionUrl))
                DbFactory.AddInParameter(cmd, "SuggestionUrl", DbType.String, pSuggestionUrl);
            if (!string.IsNullOrEmpty(pSuggestionDescription))
                DbFactory.AddInParameter(cmd, "SuggestionDescription", DbType.String, pSuggestionDescription);
            if (!string.IsNullOrEmpty(pSuggestionImage))
                DbFactory.AddInParameter(cmd, "SuggestionImage", DbType.String, pSuggestionImage);
            if(pUserId > 0)
                DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, pUserId);

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.SuggestionBox[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.SuggestionBox[])));
            else
                return null;
        }

        public Model.SuggestionBox Get(int pSuggestionId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetSuggestionBox");

            if (pSuggestionId != 0)
                DbFactory.AddInParameter(cmd, "SuggestionId", DbType.Int32, pSuggestionId);

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.SuggestionBox)(Model.Util.Deserialize(sXmlReturn, typeof(Model.SuggestionBox)));
            else
                return null;
        }

        public long Save(Model.SuggestionBox suggestionBox)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveSuggestionBox");
            if (suggestionBox.Id > 0)
                DbFactory.AddInParameter(cmd, "SuggestionId", DbType.Int64, suggestionBox.Id);
            if (!string.IsNullOrEmpty(suggestionBox.Name))
                DbFactory.AddInParameter(cmd, "SuggestionName", DbType.String, suggestionBox.Name);
            if (!String.IsNullOrEmpty(suggestionBox.Url))
                DbFactory.AddInParameter(cmd, "SuggestionUrl", DbType.String, suggestionBox.Url);

            DbFactory.AddInParameter(cmd, "SuggestionDescription", DbType.String, suggestionBox.Description);
            if (!string.IsNullOrEmpty(suggestionBox.Image))
                DbFactory.AddInParameter(cmd, "SuggestionImage", DbType.String, suggestionBox.Image);
            if (suggestionBox.deleted != null && DateTime.MinValue != suggestionBox.deleted)
                DbFactory.AddInParameter(cmd, "deleted", DbType.DateTime, suggestionBox.deleted);

            DbFactory.AddOutParameter(cmd, "ReturnId", DbType.Int32, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            return Int64.Parse(DbFactory.GetParameterValue(cmd, "ReturnId").ToString());
        }

        public void IgnoreSuggestion(long pUserId, long pSuggestionBoxId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("IgnoreSuggestion");

            DbFactory.AddInParameter(cmd, "SuggestionBoxId", DbType.Int64, pSuggestionBoxId);
            DbFactory.AddInParameter(cmd, "UserId", DbType.Int64, pUserId);

            DbFactory.ExecuteNonQuery(cmd);
        }
    }
}

