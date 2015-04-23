using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Model
{
    public class SuggestionBox
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime register { get; set; }
        public DateTime lastupdate { get; set; }
        public DateTime deleted { get; set; }
        public List<SystemTag> SystemTagList { get; set; }
        public string RandomOrder { get; set; }
        public int Preferred { get; set; }
    }
}
