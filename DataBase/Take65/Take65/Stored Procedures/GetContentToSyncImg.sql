CREATE PROCEDURE GetContentToSyncImg
	@XmlReturn XML OUTPUT
AS
	
	SET @XmlReturn = (SELECT 0 AS 'TotalResults',
	(SELECT TOP 400
		
		FeedContentId AS 'Id'
		,[TrustedSourceFeedId] AS 'TrustedSourceFeedId'
		,[FeedTitle] AS 'Title'
		,[FeedLink] AS 'Link'
		,[FeedDescription] AS 'Description'
		,[FeedPubDate] AS 'PublishedDate'
		,[FeedLastModified] AS 'LastModified'
		,[FeedThumb] AS 'Thumb'
		,[FeedGuid] AS 'FeedGuid'
		,[register] AS 'Register'
      
		FROM [FeedContent]
			WHERE 
			deleted IS NULL
			AND inactive IS NULL
			AND tmpImageSync IS NULL
			AND [FeedThumb] IS NOT NULL
		FOR XML PATH('FeedContent'), TYPE, ROOT('FeedContentList')
		)
	FOR XML PATH('FeedContents')
)
		