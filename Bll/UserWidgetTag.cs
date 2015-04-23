using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class UserWidgetTag
    {
        public void Save(Model.UserWidgetTag userWidgetTag)
        {
            Dal.UserWidgetTag dalUWT = new Dal.UserWidgetTag();
            dalUWT.Save(userWidgetTag);
        }
    }
}
