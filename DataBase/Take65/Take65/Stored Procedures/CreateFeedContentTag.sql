CREATE PROCEDURE [dbo].[CreateFeedContentTag]
	 @Top INT = NULL
AS
BEGIN

	set NOCOUNT ON

	DECLARE @FeedContentId NUMERIC(18,0)
	DECLARE @SystemTagArray NVARCHAR(MAX)

	DECLARE @DELIMITER_POSITION INT
	DECLARE @ARRAY_VALUE VARCHAR(1000)
	DECLARE @DELIMITER CHAR(1)
	SET @DELIMITER = ','
	
	DECLARE CursorFeedContents CURSOR FOR 
		select [FeedContentId], [SystemTagIdList] from [FeedContent] FC
			inner join [TrustedSourceFeed] TSF on FC.[TrustedSourceFeedId] = TSF.[TrustedSourceFeedId]
			where FC.[deleted] is null and TSF.[deleted] is null
			and FC.[FeedContentId] not in
			(
				select [FeedContentId] from [FeedContentTag]
			)
			
	OPEN CursorFeedContents

	FETCH NEXT FROM CursorFeedContents 
	INTO @FeedContentId, @SystemTagArray

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		--KERNEL
		
		--DELETE FROM FeedContentTag WHERE FeedContentId = @FeedContentId

		-- CREATE SYSTEM TAGS LOOP
		SET @SystemTagArray = ISNULL(@SystemTagArray,'') + @DELIMITER
		print @SystemTagArray
		
		WHILE patindex('%' + @DELIMITER + '%' , @SystemTagArray) <> 0 
		BEGIN
			
			SELECT @DELIMITER_POSITION =  PATINDEX('%' + @DELIMITER + '%' , @SystemTagArray)
			SELECT @ARRAY_VALUE = LEFT(@SystemTagArray, @DELIMITER_POSITION - 1)
			
			IF @ARRAY_VALUE IS NOT NULL AND @ARRAY_VALUE > 0
			BEGIN

				INSERT INTO [dbo].[FeedContentTag] 
				(
					[FeedContentId]
 				   ,[FeedContentTagNormalized]
				   ,[FeedContentTagDisplay]
				   ,[SystemTagId]
				   ,[TagTypeId]
				)
				SELECT @FeedContentId
					  ,[SystemTagNormalized]
					  ,[SystemTagDisplay]
					  ,[SystemTagId]
					  ,[TagTypeId]
				  FROM [dbo].[SystemTag]
				  WHERE [SystemTagId] = @ARRAY_VALUE

			END
			
			SELECT @SystemTagArray = STUFF(@SystemTagArray, 1, @DELIMITER_POSITION, '')
			
		END
		
		--// CREATE SYSTEM TAGS LOOP

		
		DECLARE @FeedContentTitle VARCHAR(MAX)
		DECLARE @FeedContentDescription VARCHAR(MAX)

		--SET @FeedContentTitle = (SELECT [FeedTitle] FROM [FeedContent] WHERE [FeedContentId] = @FeedContentId)
		--SET @FeedContentDescription = (SELECT [FeedDescription] FROM [FeedContent] WHERE [FeedContentId] = @FeedContentId)

		/*IF @FeedContentTitle IS NOT NULL
		BEGIN
			EXEC [dbo].[Tegalize] @FeedContentId, @FeedContentTitle
		END
		*/
		/*
		IF @FeedContentDescription IS NOT NULL
		BEGIN
			EXEC [dbo].[Tegalize] @FeedContentId, @FeedContentDescription
		END
		*/
		--//KERNEL			
	
		FETCH NEXT FROM CursorFeedContents 
		INTO @FeedContentId, @SystemTagArray
	END 
	CLOSE CursorFeedContents
	DEALLOCATE CursorFeedContents
	
END