using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TrustedSourceFeed
    {
        public long Id { get; set; }
        public long TrustedSourceId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime LastSync { get; set; }

        public DateTime Register { get; set; }
    }
}
