using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FacebookPhoto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Photo { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
