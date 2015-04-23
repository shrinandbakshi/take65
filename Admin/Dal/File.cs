using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Layers.Admin.Dal
{
    public class File
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("AdminTest");
        private new NetBiis.Library.Serialization.Deserialize oDeserialize = new NetBiis.Library.Serialization.Deserialize();

        public Int32 Save(int pFileId, string pFileName, string pFileDescription, String pFileType, String pFileLink, string pDeleted = null)
        {
            DbCommand command = DbFactory.GetStoredProcCommand("SaveFile");
            if (pFileId != 0)
                DbFactory.AddInParameter(command, "FileId", DbType.Int32, pFileId);
            DbFactory.AddInParameter(command, "FileName", DbType.String, pFileName);
            DbFactory.AddInParameter(command, "FileDescription", DbType.String, pFileDescription);
            DbFactory.AddInParameter(command, "FileType", DbType.String, pFileType);
            DbFactory.AddInParameter(command, "FileLink", DbType.String, pFileLink);
            DbFactory.AddInParameter(command, "Deleted", DbType.DateTime, pDeleted);
            DbFactory.AddOutParameter(command, "ReturnId", DbType.Int32, int.MaxValue);
            return DbFactory.ExecuteNonQuery(command);
        }

        public Model.File Get(int pFileId)
        {
            DbCommand command = DbFactory.GetStoredProcCommand("GetFile");

            DbFactory.AddInParameter(command, "FileId", DbType.Int32, pFileId);
            DbFactory.AddOutParameter(command, "XmlReturn", DbType.Xml, int.MaxValue);

            DbFactory.ExecuteNonQuery(command);

            String sXmlReturn = DbFactory.GetParameterValue(command, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.File)(oDeserialize.Deserializer(sXmlReturn, typeof(Model.File)));
            else
                return null;

        }


        public Model.Files Get(String pFileName, String pFileDescription, String pFileType, String pFileLink)
        {
            DbCommand command = DbFactory.GetStoredProcCommand("GetFile");

            DbFactory.AddInParameter(command, "FileName", DbType.String, pFileName);
            DbFactory.AddInParameter(command, "FileDescription", DbType.String, pFileDescription);
            DbFactory.AddInParameter(command, "FileType", DbType.String, pFileType);
            DbFactory.AddInParameter(command, "FileLink", DbType.String, pFileLink);

            DbFactory.AddOutParameter(command, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(command);

            String sXmlReturn = DbFactory.GetParameterValue(command, "XmlReturn").ToString();

            if (!String.IsNullOrEmpty(sXmlReturn))
                return (Model.Files)(oDeserialize.Deserializer(sXmlReturn, typeof(Model.Files)));
            else
                return null;
        }
    }
}
