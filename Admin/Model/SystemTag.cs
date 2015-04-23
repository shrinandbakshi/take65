using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Layers.Admin.Model
{
    
    public class SystemTag
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Normalized { get; set; }
        public string Display { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public int TagTypeId { get; set; }
        public DateTime lastupdate { get; set; }
        public DateTime inactive { get; set; }
        public DateTime deleted { get; set; }
    }

    [XmlRoot("SystemTagList")]
    public class SystemTagList  
    {
        [XmlElement("SystemTag")]
        public List<SystemTag> SystemTag { get; set; }
    }
}
