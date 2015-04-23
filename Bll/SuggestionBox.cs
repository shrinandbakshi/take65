using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class SuggestionBox
    {
        private Dal.SuggestionBox dalSuggestionBox = new Dal.SuggestionBox();
        public List<Model.SuggestionBox> Get(long pUserId)
        {
            Model.SuggestionBox[] ltSuggestion = dalSuggestionBox.Get(0, null, null, null, null, pUserId);
            if (ltSuggestion != null)
                return ltSuggestion.ToList();
            else
                return null;
        }

        public Model.SuggestionBox GetById(int pSuggestionId)
        {
            Model.SuggestionBox ltSuggestion = dalSuggestionBox.Get(pSuggestionId);
            if (ltSuggestion != null)
                return ltSuggestion;
            else
                return null;
        }

        public long Save(Model.SuggestionBox suggestionBox)
        {
            return dalSuggestionBox.Save(suggestionBox);
        }

        public void IgnoreSuggestion(long pUserId, long pSuggestionBoxId)
        {
            dalSuggestionBox.IgnoreSuggestion(pUserId, pSuggestionBoxId);
        }
    }
}
