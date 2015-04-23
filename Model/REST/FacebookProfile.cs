using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class FacebookProfile
    {
        public long id { get; set; }
        public string name { get; set; }
        public int photoCount { get; set; }
        public string photoProfile { get; set; }
        public bool addedByUser { get; set; }
        public bool chk { get; set; }
        public bool verifing { get; set; }
        public FacebookPhoto[] photos { get; set; }
    }
}
