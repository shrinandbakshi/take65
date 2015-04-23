

CREATE PROCEDURE SaveUserPreference
	@UserId DECIMAL(18,0),
	@SystemTagId DECIMAL(18,0)
AS
BEGIN
	
	INSERT INTO [dbo].[UserPreference]
           ([UserId]
           ,[SystemTagId]
           ,[register])
     VALUES
           (@UserId
           ,@SystemTagId
           ,GETDATE())



END
