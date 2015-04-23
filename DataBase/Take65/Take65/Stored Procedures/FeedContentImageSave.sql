CREATE PROCEDURE FeedContentImageSave
	@FeedContentId numeric(18,0),
    @FeedContentImageUrl nvarchar(250),
    @FeedContentImageTitle nvarchar(25)
AS
BEGIN
	IF (NOT(EXISTS(SELECT TOP 1 FeedContentImageId FROM [FeedContentImage] WHERE [FeedContentId] = @FeedContentId AND [FeedContentImageUrl] = @FeedContentImageUrl)))
	BEGIN
		INSERT INTO [dbo].[FeedContentImage]
			   ([FeedContentId]
			   ,[FeedContentImageUrl]
			   ,[FeedContentImageTitle]
			   ,[register]
			   )
		 VALUES
			   (@FeedContentId
			   ,@FeedContentImageUrl
			   ,@FeedContentImageTitle
			   ,GETDATE()
			   )
    END
END

