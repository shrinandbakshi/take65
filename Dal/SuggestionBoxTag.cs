using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Dal
{
    public class SuggestionBoxTag
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public long Save(int pSuggestionId, int pSystemTagId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveSuggestionBoxTag");
            if (pSuggestionId > 0)
                DbFactory.AddInParameter(cmd, "SuggestionId", DbType.Int32, pSuggestionId);
            if (pSystemTagId > 0)
                DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int32, pSystemTagId);

            DbFactory.AddOutParameter(cmd, "ReturnId", DbType.Int32, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            return Int64.Parse(DbFactory.GetParameterValue(cmd, "ReturnId").ToString());
        }

        public Model.SuggestionBoxTagList Get(int pSuggestionId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetSuggestionBoxTag");

            if (pSuggestionId != 0)
                DbFactory.AddInParameter(cmd, "SuggestionId", DbType.Int32, pSuggestionId);

            DbFactory.AddOutParameter(cmd, "ReturnXml", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "ReturnXml").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.SuggestionBoxTagList)(Model.Util.Deserialize(sXmlReturn, typeof(Model.SuggestionBoxTagList)));
            else
                return null;
        }

        public bool DeleteBySuggestionId(int pSuggestionId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("DeleteSuggestionBoxTag");
            if (pSuggestionId > 0)
                DbFactory.AddInParameter(cmd, "SuggestionId", DbType.Int32, pSuggestionId);

            DbFactory.ExecuteNonQuery(cmd);

            return true;
        }
    }
}
