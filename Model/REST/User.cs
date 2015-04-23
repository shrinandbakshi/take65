using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class User
    {
        public long id { get; set; }
        public String facebookId { get; set; }
        public String facebookToken { get; set; }
        public string googleId { get; set; }
        public string googleAccessToken { get; set; }
        public string googleRefreshToken { get; set; }
        public long googleAccessTokenExpires { get; set; }
        public String login { get; set; }
        public String password { get; set; }
        public String name { get; set; }
        public String email { get; set; }
        public DateTime birthdate { get; set; }
        public string yearofbirth { get; set; }
        public String gender { get; set; }
        public String state { get; set; }
        public String city { get; set; }
        public String postalCode { get; set; }

        public Preference[] preference { get; set; }
    }
}
