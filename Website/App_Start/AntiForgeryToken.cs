using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.App_Start
{
    public class AntiForgeryToken
    {
        public static AntiForgeryToken Instance = new AntiForgeryToken();
        private string _referenceToken = string.Empty;
        private AntiForgeryToken()
        {
            _referenceToken = Guid.NewGuid().ToString();
        }
        public string ReferenceToken
        {
            get { return _referenceToken; }
        }
    }
}