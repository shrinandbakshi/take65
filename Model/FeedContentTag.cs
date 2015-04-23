using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FeedContentTag
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public long FeedContentId { get; set; }

        public String Normalized { get; set; }
        public String Display { get; set; }
        public long TagTypeId { get; set; }
        public long SystemTagId { get; set; }

        public DateTime Register { get; set; }
    }
}
