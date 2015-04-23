using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Bll
{
    public class Tag
    {
        public List<Model.Tag> GetSystemTag(Model.Enum.enSystemTagType tagType)
        {
            Model.TagList tagList = new Dal.Tag().GetSystemTag(0, 0, (int)tagType, 0);
            if (tagList != null && tagList.Tag.Count > 0)
            {
                return tagList.Tag;
            }
            else
            {
                return null;
            }
        }

        public Model.TagList GetSystemTag(Int64 pId)
        { return new Dal.Tag().GetSystemTag(pId, 0, 0, 0); }

        public Model.TagList GetSystemTag(Int64 pParentId, int pTypeId, short pLocaleId)
        { return new Dal.Tag().GetSystemTag(0, pParentId, pTypeId, pLocaleId); }

        

        public Model.TagList GetSystemTag(int pId, Int64 pParentId, int pTypeId, short pLocaleId)
        { return new Dal.Tag().GetSystemTag(pId, pParentId, pTypeId, pLocaleId); }

        public List<Model.Tag> TextToTag(String Text, int TagTypeId)
        {
            if (String.IsNullOrEmpty(Text))
            {
                return null;
            }

            List<Model.Tag> ReturnList = new List<Model.Tag>();

            String[] Word = this.CleanText(Normalize(Text.ToUpper())).Split(' ');

            for (int i = 0; i < Word.Length; i++)
            {
                if (this.Normalize(Word[i]).Trim() != "")
                {
                    Model.Tag oTag = new Model.Tag();
                    oTag.Display = Word[i].Trim();
                    oTag.Normalized = this.CleanText(this.Normalize(Word[i])).Trim();
                    oTag.TagTypeId = TagTypeId;
                    ReturnList.Add(oTag);
                }
            }

            return ReturnList;
        }

        public String Normalize(String Text)
        {
            String specialCharacter = @"!@#$%¨&*()?:{}][.;'`~^+_/[}\+";
            for (Int32 i = 0; i < specialCharacter.Length; i++)
                Text = Text.Replace(specialCharacter[i].ToString(), "");

            String accent = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÑñÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            String unaccented = "AAAAAAaaaaaEEEEeeeeIIIIiiiiNnOOOOOoooooUUUuuuuCc";

            for (Int32 i = 0; i < accent.Length; i++)
                Text = Text.Replace(accent[i].ToString(), unaccented[i].ToString()).Trim();

            return Text.ToLower();
        }

        private String CleanText(String Text)
        {

            //Clean Text
            NameValueCollection ltClean = new NameValueCollection();
            ltClean.Add(".", ""); //Perguntar pro zoma pq antes nao tirava "." , pq tem muitas palavras q pega na tag
            ltClean.Add(",", "");
            ltClean.Add("-", " ");
            ltClean.Add("´", "");
            ltClean.Add("`", "");
            ltClean.Add(";", "");
            ltClean.Add("...", "");
            ltClean.Add(" C/ ", " ");
            ltClean.Add(" P/ ", " ");
            ltClean.Add(" E ", " ");
            ltClean.Add(" / ", " ");
            ltClean.Add(@" \ ", " ");
            ltClean.Add(" ( ", " ");
            ltClean.Add(" ) ", " ");
            ltClean.Add(" DE ", " ");
            ltClean.Add(" DA ", " ");
            ltClean.Add(" DAS ", " ");
            ltClean.Add(" DO ", " ");
            ltClean.Add(" DOS ", " ");
            ltClean.Add(" A ", " ");
            ltClean.Add(" AS ", " ");
            ltClean.Add(" O ", " ");
            ltClean.Add(" OS ", " ");
            ltClean.Add(" NA ", " ");
            ltClean.Add(" NO ", " ");
            ltClean.Add(" NAS ", " ");
            ltClean.Add(" NOS ", " ");
            ltClean.Add(" OU ", " ");
            ltClean.Add(" EM ", " ");
            ltClean.Add(" ! ", " ");
            ltClean.Add(" ? ", " ");
            ltClean.Add(" * ", " ");
            ltClean.Add(" : ", " ");
            ltClean.Add(" & ", " ");
            ltClean.Add(" AO ", " ");
            ltClean.Add(" AOS ", " ");
            ltClean.Add(" LO ", " ");
            ltClean.Add(" LA ", " ");
            ltClean.Add(" LOS ", " ");
            ltClean.Add(" LAS ", " ");
            ltClean.Add(" VOL. ", " ");
            ltClean.Add(" COL. ", " ");
            ltClean.Add(" ED. ", " ");
            ltClean.Add(" THE ", " ");
            ltClean.Add(" OF ", " ");
            ltClean.Add(" IN ", " ");
            ltClean.Add("   ", " ");
            ltClean.Add("  ", " ");

            //acrescentar espeço nas tags, pra nao remover tags e jutnar palavras
            Text = Text.Replace(">", "> ");
            Text = Text.Replace("<", " <");

            Text = this.StripTagsCharArray(Text);
            Text = Text.ToUpper();

            foreach (string sName in ltClean)
                Text = Text.Replace(sName, ltClean[sName]);

            return Text;
        }

        private string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }


    }
}
