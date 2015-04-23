using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Layers.Admin.Model
{
    [XmlRoot("PageAdmin")]
    public class PageAdmin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public PageAdminList PageAdminList { get; set; }
    }

    [XmlRoot("PageAdminList")]
    public class PageAdminList
    {
        [XmlElement("PageAdmin")]
        public List<PageAdmin> PageAdmin { get; set; }
    }
}
