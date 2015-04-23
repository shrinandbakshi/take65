using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Invite
{
    public interface IInvite
    {
        List<Model.Contact> GetContact();
    }
}
