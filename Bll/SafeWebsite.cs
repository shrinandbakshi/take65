using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class SafeWebsite
    {
        private Dal.SafeWebsite dalSafeWebsite = new Dal.SafeWebsite();
        public List<Model.SafeWebsite> Get()
        {
            Model.SafeWebsite[] ltSuggestion = dalSafeWebsite.Get();
            if (ltSuggestion != null)
                return ltSuggestion.ToList();
            else
                return null;
        }

        public List<Model.SafeWebsite> GetUnmapped()
        {
            Model.SafeWebsite[] ltSuggestion = dalSafeWebsite.GetUnmapped();
            if (ltSuggestion != null)
                return ltSuggestion.ToList();
            else
                return null;
        }

        public Model.SafeWebsite Get(int pSafeWebsiteId)
        {
            Model.SafeWebsite ltSafeWebsite = dalSafeWebsite.Get(pSafeWebsiteId);
            if (ltSafeWebsite != null)
                return ltSafeWebsite;
            else
                return null;
        }

        public Model.SafeWebsite[] Get(string pSafeWebsiteUrl)
        {
            Model.SafeWebsite[] ltSafeWebsite = dalSafeWebsite.Get(pSafeWebsiteUrl);
            if (ltSafeWebsite != null)
                return ltSafeWebsite;
            else
                return null;
        }

        public long Save(Model.SafeWebsite pSafeWebsite)
        {
            return dalSafeWebsite.Save(pSafeWebsite);
        }
    }
}
