using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class EmailAccount
    {
        public long Id { get; set; }
        public Model.Enum.enEmailServer EmailServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime LastUpdate { get; set; }

        public DateTime Deleted { get; set; }
    }
}
