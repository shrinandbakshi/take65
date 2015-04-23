using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class WidgetFeedContent
    {
        public long id { get; set; }
        public long trustedSourceId { get; set; }
        public String trustedSourceName { get; set; }
        public String[] categoryName { get; set; }
        public String title { get; set; }
        public String link { get; set; }
        public String description { get; set; }
        public DateTime publishDate { get; set; }
        public String publishDateRelative { get; set; }
        public String image { get; set; }
        public bool openIFrame { get; set; }

        public List<TrustedSource> trustedSource { get; set; }
    }
}
