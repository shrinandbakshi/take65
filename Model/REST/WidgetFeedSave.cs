using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class WidgetFeedSave
    {
        public String title { get; set; }
        public TrustedSource[] trustedSource { get; set; }
    }
}
