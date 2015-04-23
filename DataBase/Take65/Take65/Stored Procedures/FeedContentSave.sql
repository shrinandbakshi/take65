CREATE PROCEDURE FeedContentSave
	@Id NUMERIC(18, 0) = NULL,
	@TrustedSourceFeedId numeric(18,0),
	@FeedTitle nvarchar(max),
	@FeedLink nvarchar(250),
	@FeedDescription nvarchar(max),
	@FeedPubDate datetime,
	@FeedLastModified datetime = NULL,
	@FeedThumb nvarchar(250) = NULL,
	@FeedGuid nvarchar(250),
	@FeedContentId numeric(18, 0) OUTPUT
AS

IF (@Id IS NULL)
BEGIN
	SET @FeedContentId = 0
	IF (NOT(EXISTS(SELECT TOP 1 FeedContentId FROM FeedContent WHERE [deleted] IS NULL AND [FeedLink] = @FeedLink)))
	BEGIN
	
		IF (NOT(EXISTS(SELECT TOP 1 FeedContentId FROM FeedContent WHERE [deleted] IS NULL AND [FeedTitle] = @FeedTitle AND [FeedPubDate] = @FeedPubDate)))
		BEGIN
			INSERT INTO [dbo].[FeedContent]
				   ([TrustedSourceFeedId]
				   ,[FeedTitle]
				   ,[FeedLink]
				   ,[FeedDescription]
				   ,[FeedPubDate]
				   ,[FeedLastModified]
				   ,[FeedThumb]
				   ,[FeedGuid]
				   ,[FeedViewCount]
				   ,[register]
				   )
			 VALUES
				   (@TrustedSourceFeedId
				   ,@FeedTitle
				   ,@FeedLink
				   ,@FeedDescription
				   ,@FeedPubDate
				   ,@FeedLastModified
				   ,@FeedThumb
				   ,@FeedGuid
				   ,0
				   ,GETDATE()
				   )

			SET @FeedContentId = @@IDENTITY
		END
	END
END
ELSE
BEGIN

	UPDATE [dbo].[FeedContent]
		   SET [TrustedSourceFeedId] = ISNULL(@TrustedSourceFeedId, [TrustedSourceFeedId])
			  ,[FeedTitle] = ISNULL(@FeedTitle, [FeedTitle])
			  ,[FeedLink] = ISNULL(@FeedLink, [FeedLink])
			  ,[FeedDescription] = ISNULL(@FeedDescription, [FeedDescription])
			  ,[FeedPubDate] = ISNULL(@FeedPubDate, [FeedPubDate])
			  ,[FeedLastModified] = ISNULL(@FeedLastModified, [FeedLastModified])
			  ,[FeedThumb] = @FeedThumb
			  ,[lastmodified] = GETDATE()
			  ,[tmpImageSync] = GETDATE() -- TEP
		 WHERE FeedContentId = @Id

	SET @FeedContentId = @Id
END





