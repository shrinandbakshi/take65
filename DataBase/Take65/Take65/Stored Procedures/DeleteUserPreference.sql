

CREATE PROCEDURE DeleteUserPreference
	@UserId DECIMAL(18,0)
AS
BEGIN
	
	DELETE [dbo].[UserPreference]
    WHERE [UserId] = @UserId
           
END
