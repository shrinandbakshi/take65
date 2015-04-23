using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class WidgetBookmarkSave
    {
        public long id { get; set; }
        public String title { get; set; }
        public TrustedSource[] trustedSource { get; set; }
    }
}
