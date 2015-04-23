using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layers.Admin.Bll
{
    public class PageAdmin
    {
        public Model.PageAdmin Get(string AdminLink = null)
        {
            return new Dal.PageAdmin().Get(AdminLink);
        }

        public Model.PageAdminList Get()
        {
            return new Dal.PageAdmin().Get();
        }
    }
}
