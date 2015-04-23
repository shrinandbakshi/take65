using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class User
    {
        public long Id { get; set; }
        public String FacebookId { get; set; }
        public String FacebookTokenLongLived { get; set; }
        public String FacebookTokenShortLived { get; set; }
        public long FacebookTokenLongExpires { get; set; }
        public String GUID { get; set; }

        public string GoogleId { get; set; }
        public string GoogleAccessToken { get; set; }
        public string GoogleRefreshToken { get; set; }
        public long GoogleAccessTokenExpires { get; set; }

        public String Name { get; set; }
        public String Email { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public DateTime ChangePasswordNextLogin { get; set; }

        public DateTime Birthdate{ get; set; }
        public String Gender { get; set; }
        public String AddressState { get; set; }
        public String AddressCity { get; set; }
        public String AddressPostalCode { get; set; }
        public int[] Preferences { get; set; }

        public DateTime Register { get; set; }
        public DateTime Deleted { get; set; }
    }
}
