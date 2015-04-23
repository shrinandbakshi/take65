
CREATE PROCEDURE [dbo].[SaveFeedContentTag]


	@FeedContentId DECIMAL(18,0) = NULL,
	@Normalized NVARCHAR(50) = NULL,
	@Display NVARCHAR(50) = NULL,
	@TagTypeId INT = NULL,
	@SystemTagId INT = NULL

AS
BEGIN

	INSERT INTO [dbo].[FeedContentTag]
           ([FeedContentTagParentId]
           ,[FeedContentId]
           ,[FeedContentTagNormalized]
           ,[FeedContentTagDisplay]
           ,[TagTypeId]
           ,[SystemTagId]
           ,[register]
		   )
	VALUES
		(NULL
		,@FeedContentId
		,@Normalized
		,@Display
		,@TagTypeId
		,@TagTypeId
		,GETDATE()
		)

END