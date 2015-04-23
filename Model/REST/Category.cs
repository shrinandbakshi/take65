using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class Category
    {
        public int id { get; set; }
        public String title { get; set; }
        public String image { get; set; }

        public TrustedSource[] trustedSource { get; set; }
    }
}
