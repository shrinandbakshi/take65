using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Model
{
    public class FeedContent
    {
        public long Id { get; set; }
        public long TrustedSourceFeedId { get; set; }
        public long TrustedSourceId { get; set; }
        public String TrustedSourceName { get; set; }
        public String Category { get; set; } //CategoryId separated by list
        
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public DateTime LastModified { get; set; }
        public string Thumb { get; set; }
        public String Guid { get; set; }

        public bool OpenIFrame { get; set; }

        public List<FeedContentImage> ImageList { get; set; }

        public DateTime Register { get; set; }
        public DateTime PublishedDate { get; set; }
    }


    [Serializable]
    public class FeedContents
    {
        [XmlElement("TotalResults")]
        public int TotalResults { get; set; }

        public List<Model.FeedContent> FeedContentList { get; set; }
    }

    public class FeedContentComparer : IEqualityComparer<FeedContent>
    {
        public static readonly FeedContentComparer Instance = new FeedContentComparer();

        public bool Equals(FeedContent x, FeedContent y)
        {
            return x.Title.Equals(y.Title);
        }

        public int GetHashCode(FeedContent obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    
}
