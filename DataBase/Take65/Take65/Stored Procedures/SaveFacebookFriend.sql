CREATE PROCEDURE SaveFacebookFriend
	@UserId NUMERIC(18, 0),
	@FacebookFriendId NUMERIC(18, 0),
	@FacebookFriendName NVARCHAR(255),
	@FacebookPhotoCount INT
AS
	IF (NOT(EXISTS(SELECT UserFacebookFriendId FROM UserFacebookFriend WHERE [UserId] = @UserId AND [FacebookFriendId] = @FacebookFriendId)))
	BEGIN
		INSERT INTO [dbo].[UserFacebookFriend]
			   ([UserId]
			   ,[FacebookFriendId]
			   ,[FacebookFriendName]
			   ,[FacebookPhotoCount]
			   ,[register])
		 VALUES
			   (@UserId
			   ,@FacebookFriendId
			   ,@FacebookFriendName
			   ,@FacebookPhotoCount
			   ,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [dbo].[UserFacebookFriend] SET
			   [FacebookPhotoCount] = @FacebookPhotoCount
				WHERE [UserId] = @UserId AND [FacebookFriendId] = @FacebookFriendId
	END



