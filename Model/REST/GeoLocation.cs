using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class GeoLocation
    {
        public string zipcode { get; set; }
        public string location { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public DateTime lastupdate { get; set; }

        public string timezone { get; set; }
        public int offset { get; set; }

        public Model.REST.Weather[] weather { get; set; }
    }
}
