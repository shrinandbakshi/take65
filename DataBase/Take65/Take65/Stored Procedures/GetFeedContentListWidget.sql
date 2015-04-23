CREATE PROCEDURE [dbo].[GetFeedContentListWidget]
	@UserWidgetId NUMERIC(18, 0),
	@PageStart INT = NULL,
	@ItemsPerPage INT = NULL,
	@XmlReturn XML OUTPUT
AS

	

	DECLARE @TotalResults INT
	

		
	
	
	SET @TotalResults = ( select '0'
								/*SELECT COUNT([FeedContentId]) FROM [FeedContent] FC
							  INNER JOIN [TrustedSourceFeed] TSF ON FC.[TrustedSourceFeedId] = TSF.[TrustedSourceFeedId]
							  INNER JOIN [TrustedSource] TS ON TSF.[TrustedSourceId] = TS.[TrustedSourceId]
							  INNER JOIN [UserWidgetTrustedSource] UWTS ON TS.[TrustedSourceId] = UWTS.[TrustedSourceId]
							  INNER JOIN [UserWidget] UW ON UWTS.[UserWidgetId] = UW.[UserWidgetId]
							  WHERE UWTS.[UserWidgetId] = @UserWidgetId
							  AND FC.[inactive] IS NULL AND FC.[deleted] IS NULL
							  AND TSF.[inactive] IS NULL AND TSF.[deleted] IS NULL
							  AND TS.[inactive] IS NULL AND TS.[deleted] IS NULL
							  AND (
								CHARINDEX(','+CONVERT(NVARCHAR,UW.[SystemTagId])+',',TSF.[SystemTagIdList]) > 0 
								or (CHARINDEX(CONVERT(NVARCHAR,UW.[SystemTagId])+',',TSF.[SystemTagIdList]) = 1) 
								or ((CHARINDEX(CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) > 0 and (CHARINDEX(CONVERT(NVARCHAR,','),TSF.[SystemTagIdList]) = 0)))
								or (CHARINDEX(','+CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) > 0 and CHARINDEX(','+CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) = (LEN(TSF.[SystemTagIdList])-LEN(UW.[SystemTagId]))) 
							  )*/
							  )


	SET @XmlReturn =	
		(					
			  SELECT @TotalResults AS 'TotalResults'
			  ,(SELECT 
					  	 result.[Id]
						,result.[TrustedSourceFeedId]
						,result.[Title]
						,result.[Link]
						,result.[Description]
						,result.[PublishedDate]
						,result.[LastModified]
						,result.[Thumb]
						,result.[FeedGuid]
						,result.[Register]
				   FROM
					   (SELECT 
							ROW_NUMBER() OVER(ORDER BY FC.[FeedPubDate] DESC) AS [SELECTED_ROWS]
							,FeedContentId AS 'Id'
							,FC.[TrustedSourceFeedId] AS 'TrustedSourceFeedId'
							,[FeedTitle] AS 'Title'
							,[FeedLink] AS 'Link'
							,[FeedDescription] AS 'Description'
							,[FeedPubDate] AS 'PublishedDate'
							,[FeedLastModified] AS 'LastModified'
							,[FeedThumb] AS 'Thumb'
							,[FeedGuid] AS 'FeedGuid'
							,FC.[register] AS 'Register'
							FROM [FeedContent] FC
							  INNER JOIN [TrustedSourceFeed] TSF ON FC.[TrustedSourceFeedId] = TSF.[TrustedSourceFeedId]
							  INNER JOIN [TrustedSource] TS ON TSF.[TrustedSourceId] = TS.[TrustedSourceId]
							  INNER JOIN [UserWidgetTrustedSource] UWTS ON TS.[TrustedSourceId] = UWTS.[TrustedSourceId]
							  INNER JOIN [UserWidget] UW ON UWTS.[UserWidgetId] = UW.[UserWidgetId]
							  WHERE UWTS.[UserWidgetId] = @UserWidgetId
							  AND FC.[inactive] IS NULL AND FC.[deleted] IS NULL
							  AND TSF.[inactive] IS NULL AND TSF.[deleted] IS NULL
							  AND TS.[inactive] IS NULL AND TS.[deleted] IS NULL
							 /* AND (
								CHARINDEX(','+CONVERT(NVARCHAR,UW.[SystemTagId])+',',TSF.[SystemTagIdList]) > 0 
								or (CHARINDEX(CONVERT(NVARCHAR,UW.[SystemTagId])+',',TSF.[SystemTagIdList]) = 1) 
								or ((CHARINDEX(CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) > 0 and LEN(TSF.[SystemTagIdList]) = 1))
								or (CHARINDEX(','+CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) > 0 and CHARINDEX(','+CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) = (LEN(TSF.[SystemTagIdList])-1)) 
							  )*/
						) AS result
						WHERE
							result.[SELECTED_ROWS] BETWEEN (@PageStart + 1) AND @PageStart + @ItemsPerPage
						ORDER BY
							result.[SELECTED_ROWS]
						FOR XML PATH('FeedContent'), TYPE, ROOT('FeedContentList')
					)
					FOR XML PATH('FeedContents')
				)
				
