using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class FacebookPhoto
    {
        public long id { get; set; }
        public string name { get; set; }
        public string thumb { get; set; }
        public string photo { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string createdTime { get; set; }
    }
}
