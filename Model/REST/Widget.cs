using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class Widget
    {
        public int typeId { get; set; }
        public String title { get; set; }
        public String image { get; set; }

        public bool isDeletable { get; set; }
    }
}
