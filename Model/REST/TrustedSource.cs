using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class TrustedSource
    {
        public int id { get; set; }
        public int categoryId { get; set; }
        public String title { get; set; }
        public String image { get; set; }
        public String link { get; set; }
        public bool chk { get; set; }

    }
}
