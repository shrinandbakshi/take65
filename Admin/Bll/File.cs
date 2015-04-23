using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layers.Admin.Bll
{
    public class File
    {
        public Int32 Save(int pFileId, string pFileName, string pFileType, string pFileLink, string pFileDescription = null, string pDeleted = null)
        {
            return new Dal.File().Save(pFileId, pFileName, pFileDescription, pFileType, pFileLink, pDeleted);
        }

        public Model.File Get(int pFileId)
        {
            return new Dal.File().Get(pFileId);
        }

        public Model.Files Get(String pFileName, String pFileDescription, String pFileType, String pFileLink)
        {
            return new Dal.File().Get(pFileName, pFileDescription, pFileType, pFileLink);
        }
    }
}
