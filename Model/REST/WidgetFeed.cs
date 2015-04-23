using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class WidgetFeed
    {
        public UserWidget userWidget { get; set; }
        public Category[] category { get; set; }
        public TrustedSource[] trustedSource { get; set; }
        public bool addAllTrustedSource { get; set; }
        //WidgetFeedContent[] widgetFeedContent { get; set; }
        public string title { get; set; }
    }
}
