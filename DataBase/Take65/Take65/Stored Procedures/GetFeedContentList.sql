
CREATE PROCEDURE [dbo].[GetFeedContentList]
	@TrustedSourceId NUMERIC(18, 0),
	@CurrentPage INT = NULL,
	@ItemsPerPage INT = NULL,
	@XmlReturn XML OUTPUT
AS

	DECLARE @PageStart INT
	DECLARE @TotalResults INT
	

	SET @CurrentPage = ISNULL(@CurrentPage, 1)
	SET @ItemsPerPage = ISNULL(@ItemsPerPage, 25)
	SET @PageStart = ((@CurrentPage-1) * @ItemsPerPage)
	
	SET @TotalResults = ( SELECT COUNT([FeedContentId]) FROM [FeedContent] WHERE TrustedSourceFeedId = @TrustedSourceId	AND deleted IS NULL AND inactive IS NULL)


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
							ROW_NUMBER() OVER(ORDER BY FeedContent.[FeedPubDate] DESC) AS [SELECTED_ROWS]
							,FeedContentId AS 'Id'
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
								TrustedSourceFeedId = @TrustedSourceId
								AND deleted IS NULL
								AND inactive IS NULL
						) AS result
						WHERE
							result.[SELECTED_ROWS] BETWEEN (@PageStart + 1) AND @PageStart + @ItemsPerPage
						ORDER BY
							result.[SELECTED_ROWS]
						FOR XML PATH('FeedContent'), TYPE, ROOT('FeedContentList')
					)
					FOR XML PATH('FeedContents')
				)
				
