using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Model
{
    [XmlRoot("TrustedSource")]
    public class TrustedSource
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Icon { get; set; }
        public long TypeId { get; set; }
        public string Url { get; set; }
        public bool UserWidgetSelected { get; set; }
        public Model.Tag CurrentTag { get; set; }
        public bool OpenIFrame { get; set; }

        public DateTime Register { get; set; }
        public DateTime deleted { get; set; }
    }
}
