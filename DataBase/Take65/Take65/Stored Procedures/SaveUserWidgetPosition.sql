create procedure SaveUserWidgetPosition
	@UserWidgetId NUMERIC(18, 0),
	@UserId NUMERIC(18, 0),
	@WidgetRow INT,
	@WidgetColumn INT
AS
	UPDATE UserWidget SET
		UserWidgetRow = @WidgetRow
		,UserWidgetColumn = @WidgetColumn
	WHERE UserWidgetId = @UserWidgetId
	AND UserId = @UserId
	AND deleted IS NULL