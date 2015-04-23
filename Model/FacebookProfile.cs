using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FacebookProfile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int PhotoCount { get; set; }
        public FacebookPhoto[] Photos { get; set; }
    }
}
