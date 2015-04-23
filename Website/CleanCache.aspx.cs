using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website
{
    public partial class CleanCache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (DictionaryEntry dCache in HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove(dCache.Key.ToString());
            }
            Response.Redirect("/");
        }
    }
}