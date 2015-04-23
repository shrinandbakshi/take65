using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Model
{
    public class SuggestionBoxTag
    {
        public int Id { get; set; }
        public int SuggestionId { get; set; }
        public int SystemTagId { get; set; }
    }

    [XmlRoot("SuggestionBoxTagList")]
    public class SuggestionBoxTagList
    {
        [XmlElement("SuggestionBoxTag")]
        public List<SuggestionBoxTag> SuggestionBoxTag { get; set; }
    }
}
