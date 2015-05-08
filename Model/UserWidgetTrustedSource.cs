using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UserWidgetTrustedSource
    {
        public long UserWidgetId { get; set; }
        public int TrustedSourceId { get; set; }
        public int SystemTagId { get; set; } //Feed Category
        public int CategoryId { get; set; } //Feed Category
        public string Category { get; set; } //Feed Category for bookmark
        public String Name { get; set; }
        public String Url { get; set; }
        public String Icon { get; set; }
        public bool OpenIFrame { get; set; }

    }

}

