using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layers.Admin.Bll
{
    public class SystemUser
    {
        public Model.SystemUser AuthenticateSystemUser(string pUserName, string pPassword)
        {
            return new Dal.SystemUser().AuthenticateSystemUser(pUserName, pPassword);
        }
    }
}
