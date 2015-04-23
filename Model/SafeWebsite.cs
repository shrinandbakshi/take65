using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SafeWebsite
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public bool OpenIFrame { get; set; }
        public DateTime register { get; set; }
        public DateTime lastupdate { get; set; }
        public DateTime deleted { get; set; }
        public Model.Tag CurrentTag { get; set; }
    }
}
