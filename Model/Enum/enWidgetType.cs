using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace Model.Enum
{
    public enum enWidgetType
    {
        [Description("DEFAULT")]
        DEFAULT = 0,
        [Description("FEED")]
        FEED = 1,
        [Description("BOOKMARK")]
        BOOKMARK = 2,
        [Description("FACEBOOK")]
        FACEBOOK = 3,
        [Description("WEATHER")]
        WEATHER = 4,
        [Description("FACEBOOKPHOTOS")]
        FACEBOOKPHOTOS = 5,
        [Description("EMAIL")]
        EMAIL = 6
    }
}
