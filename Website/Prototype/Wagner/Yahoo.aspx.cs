using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;

namespace Website.Prototype.Wagner
{
    public partial class Yahoo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Bll.Invite.OAuth oAuth = new Bll.Invite.OAuth(new Bll.Invite.OAuthSetting(){
                UrlRequest = "https://api.login.yahoo.com/oauth/v2/get_request_token",
                UrlAccess = "https://api.login.yahoo.com/oauth/v2/get_token",
                UrlAuthorize = "https://api.login.yahoo.com/oauth/v2/request_auth",
                ConsumerKey = "dj0yJmk9SXZabHJzY3VGakNaJmQ9WVdrOU1GbGFRekZ1TmpRbWNHbzlNVE01TVRJMU16WTJNZy0tJnM9Y29uc3VtZXJzZWNyZXQmeD1kOA--",
                ConsumerKeySecret = "9346e320ff15668d7815352b61c5e8811a3ff92c"
            });


            String x = oAuth.GetAccessUrl(Bll.Invite.OAuthPermission.READ);



        }
    }
}