using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class GoogleAccount
    {
        public string UserId { get; set; } //correspond to Id
        public string UserDisplayName { get; set; }  // DisplayName
        public string UserBirthday { get; set; } //Birthday
        public string UserGender { get; set; } //Gender
        public string AccessToken { get; set; }
        public long? Expiry { get; set; }
        public string RefreshToken { get; set; }
        public string EmailId { get; set; }  // Emails[0].value -- IList
        public DateTime LastUpdated { get; set; }  //datetime.now

        //rest of profile info
    }
}
