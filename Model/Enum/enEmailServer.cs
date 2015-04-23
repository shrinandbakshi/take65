using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Model.Enum
{
    public enum enEmailServer
    {
        [XmlEnum("GMAIL")]
        [Description("GMAIL")]
        GMAIL = 1,
        //[XmlEnum("MSEXCHANGE")]
        //[Description("MSEXCHANGE")]
        //MSEXCHANGE = 2,
        [XmlEnum("HOTMAIL")]
        [Description("HOTMAIL")]
        HOTMAIL = 3,
        [XmlEnum("YAHOO")]
        [Description("YAHOO")]
        YAHOO = 4
    }
}
