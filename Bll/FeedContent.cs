using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class FeedContent
    {
        Dal.FeedContent dalFeedContent = new Dal.FeedContent();
        
        public Model.FeedContent Save(Model.FeedContent pContent)
        {
            long FeedContentId = dalFeedContent.Save(pContent);
            if (FeedContentId != 0)
            {
                pContent.Id = FeedContentId;
                /*
                foreach (Model.FeedContentImage FeedImg in pContent.Images)
                {
                    dalFeedContent.SaveFeedContentImage(FeedContentId, FeedImg);
                }
                 */
            }
            return pContent;
        }

      

        public Model.FeedContents GetContent(long pTrustedSourceId)
        {
            return dalFeedContent.Get(pTrustedSourceId, null, null);
        }

        public Model.FeedContents GetContent(long pTrustedSourceId, int pCurrentPage, int pItemPerPage)
        {
            return dalFeedContent.Get(pTrustedSourceId, pCurrentPage, pItemPerPage);
        }

        public Model.FeedContents GetContentWidget(long widgetId, int skip, int count)
        {
            return this.GetContentWidget(widgetId, skip, count,0);
        }

        public Model.FeedContents GetContentWidget(long widgetId, int skip, int count, long trustedSourceId)
        {
            return dalFeedContent.GetContentWidget(widgetId, skip, count, trustedSourceId);
        }

        public Model.FeedContents GetContentWidgetSearch(long pWidgetId, string pSearchKeyword, long pTrustedSourceId, int pCurrentPage, int pItemPerPage)
        {
            Model.FeedContents SearchReturn = dalFeedContent.GetContentWidgetSearch(pWidgetId, pSearchKeyword, pCurrentPage, pItemPerPage);
            return SearchReturn;
        }

        public Model.FeedContent[] GetUserWidgetContent(long userWidgetId, int trustedSourceId,bool hasThumb, String search)
        {
            return dalFeedContent.GetUserWidgetContent(userWidgetId, trustedSourceId,hasThumb, search);
        }

        public Model.UserWidgetTrustedSource[] GetBookmark(long userWidgetId, bool isTrusted)
        {
            return dalFeedContent.GetBookmark(userWidgetId, isTrusted);
        }

        /* TEMP */
        public Model.FeedContents GetContentToSync()
        {
            return dalFeedContent.GetContentToSync();
        }

        public void GenerateContentTag()
        {
            dalFeedContent.GenerateContentTag();
        }
        /* TEMP */

        public void SaveFeedContentTag(long feedContentId, List<Model.Tag> feedContentTagList)
        {
            if (feedContentTagList != null)
            {
                foreach (Model.Tag feedContentTag in feedContentTagList)
                {
                    dalFeedContent.SaveFeedContentTag(feedContentId, feedContentTag);
                }
            }
            
        }









        
    }
}
