using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class WidgetBookmarkList
    {
        public List<WidgetBookmark> trustedSource { get; set; }
        public List<WidgetBookmark> source { get; set; }
    }

    public class WidgetBookmark
    {
        public int trustedSourceId { get; set; }
        public int categoryId { get; set; }
        public String title { get; set; }
        public String link { get; set; }
        public String image { get; set; }
        public bool openIFrame { get; set; }
    }
}
