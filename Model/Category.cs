using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace Model
{
    public class Category
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Icon { get; set; }

       
        public TrustedSource[] TrustedSources { get; set; }
    }
}
