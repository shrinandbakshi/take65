using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Dal
{

    public class FeedContent
    {
        private Database DbFactory = DatabaseFactory.CreateDatabase("Take65");

        public long Save(Model.FeedContent pContent)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("FeedContentSave");

            if (pContent.Id != 0)
                DbFactory.AddInParameter(cmd, "Id", DbType.Int64, pContent.Id);

            DbFactory.AddInParameter(cmd, "TrustedSourceFeedId", DbType.Int64, pContent.TrustedSourceFeedId);
            DbFactory.AddInParameter(cmd, "FeedTitle", DbType.String, pContent.Title);
            DbFactory.AddInParameter(cmd, "FeedLink", DbType.String, pContent.Link);
            DbFactory.AddInParameter(cmd, "FeedDescription", DbType.String, pContent.Description);
            DbFactory.AddInParameter(cmd, "FeedPubDate", DbType.DateTime, pContent.PublishedDate);
            if (pContent.LastModified != DateTime.MinValue)
                DbFactory.AddInParameter(cmd, "FeedLastModified", DbType.DateTime, pContent.LastModified);

            if (!string.IsNullOrEmpty(pContent.Thumb))
                DbFactory.AddInParameter(cmd, "FeedThumb", DbType.String, pContent.Thumb);

            DbFactory.AddInParameter(cmd, "FeedGuid", DbType.String, pContent.Guid);
            DbFactory.AddOutParameter(cmd, "FeedContentId", DbType.Int64, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            return Convert.ToInt64(DbFactory.GetParameterValue(cmd, "FeedContentId"));
        }

        public void SaveFeedContentTag(long feedContentId, Model.Tag feedContentTag)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SaveFeedContentTag");


            DbFactory.AddInParameter(cmd, "FeedContentId", DbType.Int64, feedContentId);
            DbFactory.AddInParameter(cmd, "Normalized", DbType.String, feedContentTag.Normalized);
            DbFactory.AddInParameter(cmd, "Display", DbType.String, feedContentTag.Display);
            DbFactory.AddInParameter(cmd, "TagTypeId", DbType.Int32, feedContentTag.TagTypeId);

            //if(feedContentTag.SystemTagId > 0)
            //DbFactory.AddInParameter(cmd, "SystemTagId", DbType.Int32, feedContentTag.SystemTagId);

            DbFactory.ExecuteNonQuery(cmd);
        }

        public void SaveFeedContentImage(long pFeedContentId, Model.FeedContentImage pContentImage)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("FeedContentImageSave");
            DbFactory.AddInParameter(cmd, "FeedContentId", DbType.Int64, pFeedContentId);
            DbFactory.AddInParameter(cmd, "FeedContentImageUrl", DbType.String, pContentImage.Url);
            DbFactory.AddInParameter(cmd, "FeedContentImageTitle", DbType.String, pContentImage.Title);
            DbFactory.ExecuteNonQuery(cmd);

        }

        public Model.FeedContents Get(long pTrustedSourceId, int? pCurrentPage, int? pItemPerPage)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetFeedContentList");

            DbFactory.AddInParameter(cmd, "TrustedSourceId", DbType.Int64, pTrustedSourceId);
            if (pCurrentPage != null)
                DbFactory.AddInParameter(cmd, "CurrentPage", DbType.Int32, pCurrentPage);

            if (pItemPerPage != null)
                DbFactory.AddInParameter(cmd, "ItemsPerPage", DbType.Int32, pItemPerPage);


            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.FeedContents)(Model.Util.Deserialize(sXmlReturn, typeof(Model.FeedContents)));
            else
                return null;
        }

        public Model.FeedContents GetContentToSync()
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetContentToSyncImg");
            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.FeedContents)(Model.Util.Deserialize(sXmlReturn, typeof(Model.FeedContents)));
            else
                return null;
        }

        public Model.FeedContents GetContentWidget(long userWidgetId, int skip, int count, long trustedSourceId)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetFeedContentListWidget");

            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int64, userWidgetId);

            if (skip > 0)
                DbFactory.AddInParameter(cmd, "PageStart", DbType.Int32, skip);

            if (count > 0)
                DbFactory.AddInParameter(cmd, "ItemsPerPage", DbType.Int32, count);

            if (trustedSourceId > 0)
                DbFactory.AddInParameter(cmd, "TrustedSourceTypeId", DbType.Int32, trustedSourceId);



            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.FeedContents)(Model.Util.Deserialize(sXmlReturn, typeof(Model.FeedContents)));
            else
                return null;
        }

        public Model.FeedContent[] GetUserWidgetContent(long userWidgetId, int trustedSourceId, bool hasThumb, String search)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserWidgetContent");

            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int64, userWidgetId);

            if (trustedSourceId > 0)
                DbFactory.AddInParameter(cmd, "TrustedSourceId", DbType.Int32, trustedSourceId);

            if (hasThumb)
                DbFactory.AddInParameter(cmd, "HasThumb", DbType.Int32, 1);

            if (!String.IsNullOrEmpty(search))
                DbFactory.AddInParameter(cmd, "Search", DbType.String, search);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.FeedContent[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.FeedContent[])));
            else
                return null;
        }

        public Model.UserWidgetTrustedSource[] GetBookmark(long userWidgetId, bool isTrusted)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("GetUserWidgetBookmark_NEW");
            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int64, userWidgetId);

            if (isTrusted)
                DbFactory.AddInParameter(cmd, "IsTrusted", DbType.Byte, 1);
            else
                DbFactory.AddInParameter(cmd, "IsTrusted", DbType.Byte, null);

            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.UserWidgetTrustedSource[])(Model.Util.Deserialize(sXmlReturn, typeof(Model.UserWidgetTrustedSource[])));
            else
                return null;
        }

        public Model.FeedContents GetContentWidgetSearch(long pUserWidgetId, string pSearchKeyword, int? pCurrentPage, int? pItemPerPage)
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("SearchFeedContent");

            DbFactory.AddInParameter(cmd, "UserWidgetId", DbType.Int64, pUserWidgetId);


            DbFactory.AddInParameter(cmd, "SearchArray", DbType.String, pSearchKeyword);

            /*
            if (pCurrentPage != null)
                DbFactory.AddInParameter(cmd, "CurrentPage", DbType.Int32, pCurrentPage);

            if (pItemPerPage != null)
                DbFactory.AddInParameter(cmd, "ItemsPerPage", DbType.Int32, pItemPerPage);
             * */


            DbFactory.AddOutParameter(cmd, "XmlReturn", DbType.Xml, int.MaxValue);
            DbFactory.ExecuteNonQuery(cmd);

            string sXmlReturn = DbFactory.GetParameterValue(cmd, "XmlReturn").ToString();
            if (!string.IsNullOrEmpty(sXmlReturn))
                return (Model.FeedContents)(Model.Util.Deserialize(sXmlReturn, typeof(Model.FeedContents)));
            else
                return null;
        }

        public void GenerateContentTag()
        {
            DbCommand cmd = DbFactory.GetStoredProcCommand("CreateFeedContentTag");
            DbFactory.ExecuteNonQuery(cmd);
        }
    }
}
