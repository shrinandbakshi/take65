using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace Model
{
    [XmlRoot("Tag")]
    public class Tag
    {
        public int Id { get; set; }
        public String Normalized { get; set; }
        public String Display { get; set; }
        public int TagTypeId { get; set; }
        public String Icon { get; set; }
    }

    [XmlRoot("SystemTagList")]
    public class TagList    
    {
        [XmlElement("SystemTag")]
        public List<Tag> Tag { get; set; }
    }
}
