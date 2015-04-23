
CREATE PROCEDURE [dbo].[SaveUserWidgetTag]
	@UserId DECIMAL(18,0)	= NULL,
	@UserWidgetId DECIMAL(18,0) = NULL,
	@SystemTagId INT = NULL

AS

BEGIN
	


	INSERT INTO [dbo].[UserWidgetTag]
			   ([UserId]
			   ,[UserWidgetId]
			   ,[SystemTagId])
		VALUES
			(@UserId
			,@UserWidgetId
			,@SystemTagId)




END

