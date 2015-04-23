using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class TrustedSource
    {
        private Dal.TrustedSource dalTrustedSource = new Dal.TrustedSource();

        public Model.Category[] GetCategory()
        {
            return dalTrustedSource.GetCategory();
        }

        public Model.Category[] GetCategory(long pUserId, long pUserWidgetId)
        {
            return dalTrustedSource.GetCategory(pUserId, pUserWidgetId);
        }

        public Model.TrustedSource[] GetTrustedSource(Model.Enum.enTrustedSourceType pSourceType)
        {
            return dalTrustedSource.GetTrustedSource(pSourceType,0,0);
        }

        public Model.TrustedSource[] GetTrustedSource(Model.Enum.enTrustedSourceType pSourceType, int systemTagId)
        {
            return dalTrustedSource.GetTrustedSource(pSourceType, systemTagId, 0);
        }

        public Model.TrustedSource[] GetTrustedSource(long widgetUserId)
        {
            return dalTrustedSource.GetTrustedSource(Model.Enum.enTrustedSourceType.DEFAULT,0, widgetUserId);
        }
        public Model.TrustedSource GetTrustedSourceById(long pTrustedSourceId)
        {
            return dalTrustedSource.GetTrustedSourceById(pTrustedSourceId);
        }

        public Model.TrustedSourceFeed[] GetTrustedSourceFeedList(long pTrustedSourceId)
        {
            return dalTrustedSource.GetTrustedSourceFeedList(pTrustedSourceId);
        }

        public long Save(Model.TrustedSource pTrustedSource)
        {
            return dalTrustedSource.Save(pTrustedSource);
        }
    }
}
