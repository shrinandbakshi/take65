CREATE PROCEDURE SaveUserWidgetExtraInfo
	@UserWidgetId NUMERIC(18, 0),
	@UserId  NUMERIC(18, 0),
	@UserWidgetExtraInfoXML NVARCHAR(MAX)
AS
	UPDATE [dbo].[UserWidget] SET
		UserWidgetExtraInfoXML = @UserWidgetExtraInfoXML
		,lastupdate = GETDATE()
			WHERE UserWidgetId = @UserWidgetId AND UserId = @UserId