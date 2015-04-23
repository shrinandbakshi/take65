

CREATE PROCEDURE [dbo].[DeleteUserWidgetTrustedSource]
	@UserWidgetId DECIMAL(18,0)
AS
BEGIN
	
	DELETE [dbo].[UserWidgetTrustedSource]
    WHERE [UserWidgetId] = @UserWidgetId

	DELETE [dbo].[UserWidgetTrustedSourceFeed]
	WHERE [UserWidgetId] = @UserWidgetId
           
END
