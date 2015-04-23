using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class SuggestionBoxTag
    {
        private Dal.SuggestionBoxTag dalSuggestionBoxTag = new Dal.SuggestionBoxTag();
        public long Save(int pSuggestionId, int pSystemTagId)
        {
            return dalSuggestionBoxTag.Save(pSuggestionId, pSystemTagId);
        }

        public Model.SuggestionBoxTagList Get(int pSuggestionId)
        {
            Model.SuggestionBoxTagList ltSuggestion = dalSuggestionBoxTag.Get(pSuggestionId);
            if (ltSuggestion != null)
                return ltSuggestion;
            else
                return null;
        }
        public bool DeleteBySuggestionId(int pSuggestionId)
        {
            return dalSuggestionBoxTag.DeleteBySuggestionId(pSuggestionId);
        }
    }
}
