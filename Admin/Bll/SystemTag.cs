using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layers.Admin.Bll
{
    public class SystemTag
    {
        public Int32 Save(int pSystemTagId, string pSystemTagParentId, string pSystemTagParentIdList, String pSystemTagNormalized, string pSystemTagDisplay, String pSystemTagIcon, int pSystemTagOrder, int pTagTypeId, string pDeleted = null)
        {
            return new Dal.SystemTag().Save(pSystemTagId, pSystemTagParentId, pSystemTagParentIdList, pSystemTagNormalized, pSystemTagDisplay, pSystemTagIcon, pSystemTagOrder, pTagTypeId, pDeleted);
        }

        public Model.SystemTagList Get(int pSystemTagId, string pSystemTagNormalized, int pSystemTagParentId, int pTagTypeId, int pTagTypeParentId)
        {
            return new Dal.SystemTag().Get(pSystemTagId, pSystemTagNormalized, pSystemTagParentId, pTagTypeId, pTagTypeParentId);
        }
        public Model.SystemTag Get(int pSystemTagId)
        {
            return new Dal.SystemTag().Get(pSystemTagId);
        }
    }
}