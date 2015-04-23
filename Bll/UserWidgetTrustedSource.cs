using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class UserWidgetTrustedSource
    {
        private Dal.UserWidgetTrustedSource DalUserWidgetTrustedSource = new Dal.UserWidgetTrustedSource();

        public Model.UserWidgetTrustedSource[] Get(long userWidgetId)
        {
            return this.DalUserWidgetTrustedSource.Get(userWidgetId);
        }

        public void DeleteTrustedSource(long userWidgetId)
        {
            this.DalUserWidgetTrustedSource.DeleteTrustedSource(userWidgetId);
        }

        public void SaveTrustedSource(Model.UserWidgetTrustedSource trustedSource)
        {
            this.DalUserWidgetTrustedSource.SaveTrustedSource(trustedSource);
        }

        

        public void SaveTrustedSourceFeed(long userWidgetId, int trustedSourceId, int categoryId)
        {
            this.DalUserWidgetTrustedSource.SaveTrustedSource(new Model.UserWidgetTrustedSource()
            {
                UserWidgetId = userWidgetId,
                TrustedSourceId = trustedSourceId,
                CategoryId = categoryId
            });
        }

        public void SaveTrustedSourceBookmark(long userWidgetId, String name, String url)
        {
            this.DalUserWidgetTrustedSource.SaveTrustedSource(new Model.UserWidgetTrustedSource()
            {
                UserWidgetId = userWidgetId,
                Name = name,
                Url = url
            });
        }
    }
}
