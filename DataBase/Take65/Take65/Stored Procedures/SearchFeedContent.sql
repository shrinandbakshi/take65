
CREATE PROCEDURE [dbo].[SearchFeedContent]
(
	@SearchArray VARCHAR(1000),
	@DELIMITER CHAR(1) = ',',
	
	@UserWidgetId NUMERIC(18,0),
	
	@RequiredCategory VARCHAR(500) = NULL,
	@CategoryNotIn VARCHAR(500) = NULL,
	@SystemTagFilter VARCHAR(500) = NULL,
	@TagTypeFilter VARCHAR(500) = NULL,
	
	@Top INT = NULL,
	
	@XmlReturn XML OUTPUT
)
AS
BEGIN

	SET @Top = ISNULL(@Top,100)
	
	SET NOCOUNT ON

	DECLARE @DELIMITER_POSITION INT
	DECLARE @ARRAY_VALUE NVARCHAR(1000)
	DECLARE @QUERY NVARCHAR(MAX)
	DECLARE @NoOrderCheck NVARCHAR(1000)
	
	SET @QUERY = ''
	SET @NoOrderCheck = @SearchArray

	-- CREATE BUFFER TABLES FOR SEARCH RESULTS
	
	CREATE TABLE #UserWidgetFeedContent
	(
			[FeedContentId] [numeric](18, 0),
			[TrustedSourceFeedId] [numeric](18, 0),
			[FeedTitle] [nvarchar](max),
			[FeedLink] [nvarchar](250),
			[FeedDescription] [nvarchar](max),
			[FeedPubDate] [datetime],
			[FeedLastModified] [datetime],
			[FeedThumb] [nvarchar](250),
			[FeedGuid] [nvarchar](250),
			[register] [datetime],
			[inactive] [datetime]
	)
	
	INSERT INTO #UserWidgetFeedContent
           ([FeedContentId]
           ,[TrustedSourceFeedId]
           ,[FeedTitle]
           ,[FeedLink]
           ,[FeedDescription]
           ,[FeedPubDate]
           ,[FeedLastModified]
           ,[FeedThumb]
           ,[FeedGuid]
           ,[register])
     SELECT FeedContentId
			,FC.[TrustedSourceFeedId]
			,[FeedTitle]
			,[FeedLink]
			,[FeedDescription]
			,[FeedPubDate]
			,[FeedLastModified]
			,[FeedThumb]
			,[FeedGuid]
			,FC.[register]
			FROM [FeedContent] FC
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
				or ((CHARINDEX(CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) > 0 and LEN(TSF.[SystemTagIdList]) = 1))
				or (CHARINDEX(','+CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) > 0 and CHARINDEX(','+CONVERT(NVARCHAR,UW.[SystemTagId]),TSF.[SystemTagIdList]) = (LEN(TSF.[SystemTagIdList])-1)) 
			  )			
	
	
	CREATE TABLE #SearchResult
	(
			[FeedContentId] [numeric](18, 0),
			[TrustedSourceFeedId] [numeric](18, 0),
			[FeedTitle] [nvarchar](max),
			[FeedLink] [nvarchar](250),
			[FeedDescription] [nvarchar](max),
			[FeedPubDate] [datetime],
			[FeedLastModified] [datetime],
			[FeedThumb] [nvarchar](250),
			[FeedGuid] [nvarchar](250),
			[register] [datetime]
	)

	-- SET SEARCH_ARRAY

	SET @SearchArray = ISNULL(@SearchArray,'') + @DELIMITER

	-- LOOP TAGS
	WHILE patindex('%' + @DELIMITER + '%' , @SearchArray) <> 0 
	BEGIN
		
		SELECT @DELIMITER_POSITION =  PATINDEX('%' + @DELIMITER + '%' , @SearchArray)
		SELECT @ARRAY_VALUE = LEFT(@SearchArray, @DELIMITER_POSITION - 1)
		
		SET @QUERY = '
		INSERT #SearchResult
		  SELECT 
			F.[FeedContentId]
		   ,F.[TrustedSourceFeedId]
		   ,F.[FeedTitle]
		   ,F.[FeedLink]
		   ,F.[FeedDescription]
		   ,F.[FeedPubDate]
		   ,F.[FeedLastModified]
		   ,F.[FeedThumb]
		   ,F.[FeedGuid]
		   ,F.[register]
	    FROM #UserWidgetFeedContent F
			INNER JOIN [FeedContentTag] AS FT ON F.[FeedContentId] = FT.[FeedContentId]
		WHERE
			F.[inactive] IS NULL'

			IF @CategoryNotIn IS NOT NULL
			BEGIN
				SET @QUERY = @QUERY + ' AND ( F.[FeedContentId] NOT IN (SELECT DISTINCT [FeedContentId] FROM [FeedContentTag] WHERE [SystemTagId] IN ('+@CategoryNotIn+')))'
			END
			IF @RequiredCategory IS NOT NULL
			BEGIN
				SET @QUERY = @QUERY + ' AND ( F.[FeedContentId] IN (SELECT DISTINCT [FeedContentId] FROM [FeedContentTag] WHERE [SystemTagId] IN ('+@RequiredCategory+')))'
			END
			IF @SystemTagFilter IS NOT NULL
			BEGIN
				SET @QUERY = @QUERY + ' AND ( FT.[SystemTagId] IN ('+@SystemTagFilter+') )'
			END
			IF @TagTypeFilter IS NOT NULL
			BEGIN
				SET @QUERY = @QUERY + ' AND ( FT.[TagTypeId] IN ('+@TagTypeFilter+') )'
			END
			IF @ARRAY_VALUE IS NOT NULL AND @ARRAY_VALUE <> ''
			BEGIN
				SET @QUERY = @QUERY + ' AND UPPER(FT.FeedContentTagNormalized) LIKE ''' + [dbo].[NormalizeChar](@ARRAY_VALUE) + '%'''
			END

		EXEC (@QUERY)
		
		SELECT @SearchArray = STUFF(@SearchArray, 1, @DELIMITER_POSITION, '')
		
	END

	SET @QUERY = ''
	SET @QUERY = 'SET @XmlReturn = (SELECT
								(	
									SELECT  TOP ' + CONVERT(VARCHAR(10),@Top) + '
										SR.[FeedContentId] AS ''Id''
									   ,COUNT(SR.[FeedContentId]) AS ''Rating''
									   ,SR.[TrustedSourceFeedId] AS ''TrustedSourceFeedId''
									   ,SR.[FeedTitle] AS ''Title''
									   ,SR.[FeedLink] AS ''Link''
									   ,SR.[FeedDescription] AS ''Description''
									   ,SR.[FeedPubDate] AS ''PublishedDate''
									   ,SR.[FeedLastModified] AS ''LastModified''
									   ,SR.[FeedThumb] AS ''Thumb''
									   ,SR.[FeedGuid] AS ''FeedGuid''
									   ,SR.[register] AS ''Register''
									   ,TS.[TrustedSourceName] AS ''TrustedSourceName''
									FROM 
										#SearchResult SR
									LEFT JOIN [TrustedSourceFeed] TSF ON SR.[TrustedSourceFeedId] = TSF.[TrustedSourceFeedId]
									LEFT JOIN [TrustedSource] TS ON TSF.[TrustedSourceId] = TS.[TrustedSourceId]
									GROUP BY
										SR.[FeedContentId]
									   ,SR.[TrustedSourceFeedId]
									   ,SR.[FeedTitle]
									   ,SR.[FeedLink]
									   ,SR.[FeedDescription]
									   ,SR.[FeedPubDate]
									   ,SR.[FeedLastModified]
									   ,SR.[FeedThumb]
									   ,SR.[FeedGuid]
									   ,SR.[register]
									   ,TS.[TrustedSourceName]'
		
	IF @NoOrderCheck IS NULL OR @NoOrderCheck = ''
	BEGIN
		SET @QUERY = @QUERY + ' ORDER BY [register] DESC'
	END
	ELSE
	BEGIN
		SET @QUERY = @QUERY + ' ORDER BY [Rating] DESC'
	END
	
	SET @QUERY = @QUERY + ' FOR XML PATH(''FeedContent''), TYPE, ROOT(''FeedContentList''))
							FOR XML PATH(''FeedContents''))'
	
	EXEC sp_executesql @QUERY, N'@XmlReturn XML OUTPUT'
							   ,@XmlReturn = @XmlReturn OUTPUT

	-- DROP BUFFER TABLE
	DROP TABLE #SearchResult
	DROP TABLE #UserWidgetFeedContent
END
