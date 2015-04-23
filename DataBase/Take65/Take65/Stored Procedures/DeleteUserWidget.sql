CREATE PROCEDURE [dbo].[DeleteUserWidget]
	@UserWidgetId DECIMAL(18,0)	= NULL

AS

BEGIN

	UPDATE UserWidget 
		SET deleted = GETDATE()
			WHERE [UserWidgetId] = @UserWidgetId

	DECLARE @SystemTagId INT
	DECLARE @UserId NUMERIC(18, 0)
	SELECT @SystemTagId = SystemTagId, @UserId = UserId FROM UserWidget		
		WHERE [UserWidgetId] = @UserWidgetId
	
	IF (@SystemTagId = 5)
	BEGIN
	-- Social Media Photo
		DELETE UserFacebookFriend WHERE UserId = @UserId
	END
	

/*
	DELETE FROM [UserWidget]
	WHERE [UserWidgetId] = @UserWidgetId

	DELETE FROM [UserWidgetTrustedSource]
	WHERE [UserWidgetId] = @UserWidgetId

	DELETE FROM [UserWidgetTrustedSourceFeed]
	WHERE [UserWidgetId] = @UserWidgetId

	DELETE FROM [UserWidgetTag]
	WHERE [UserWidgetId] = @UserWidgetId
*/
END



