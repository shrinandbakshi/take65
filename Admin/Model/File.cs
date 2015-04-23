using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Layers.Admin.Model
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public DateTime Deleted { get; set; }
    }

    [XmlRoot("FileList")]
    public class Files
    {
        [XmlElement("File")]
        public List<File> File { get; set; }
    }
}
