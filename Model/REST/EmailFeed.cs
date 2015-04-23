using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class EmailFeed
    {
        public string id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        
        public DateTime lastmodified { get; set; }
        public string lastmodifiedLabel { get; set; }

        public string fromName { get; set; }
        public string fromEmail { get; set; }
        
    }
}
