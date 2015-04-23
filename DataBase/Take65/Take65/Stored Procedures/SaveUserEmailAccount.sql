CREATE PROCEDURE [dbo].[SaveUserEmailAccount]
	@UserId DECIMAL(18,0),
	@EmailServerType NVARCHAR(50),
	@Username NVARCHAR(255),
	@Password NVARCHAR(250) = NULL,
	@Deleted DATETIME = NULL
AS

BEGIN

	DECLARE @ServerType NUMERIC(18, 0)
	SELECT @ServerType = SystemTagId FROM SystemTag WHERE 
		TagTypeId = 4 --EMAIL SERVER TYPE
		AND SystemTagNormalized = @EmailServerType
		AND deleted IS NULL

	IF (@ServerType IS NOT NULL)
	BEGIN
		IF (@Deleted IS NOT NULL)
		BEGIN
			UPDATE [dbo].[UserEmailAccount]
				SET deleted = GETDATE()
					WHERE UserId = @UserId AND SystemTagId = @ServerType
					AND deleted IS NULL
		END
		ELSE
		BEGIN
		
			-- USER ONLY CAN HAVE ONE EMAIL ACCOUNT SET UP
			-- IT WILL DELETE ALL OTHER ACCOUNTS FROM THE USER
				UPDATE [dbo].[UserEmailAccount]
					SET deleted = GETDATE()
						WHERE UserId = @UserId AND SystemTagId <> @ServerType
						AND deleted IS NULL
						
			IF (EXISTS(SELECT [UserEmailAccountId] FROM [UserEmailAccount] WHERE UserId = @UserId AND SystemTagId = @ServerType AND deleted IS NULL))
			BEGIN
				-- UPDATING AN EXISTING ACCOUNT
				UPDATE [dbo].[UserEmailAccount]
					SET lastupdate = GETDATE()
						,[UserEmailAccountUsername] = @Username
						,[UserEmailAccountPassword] = @Password
						WHERE UserId = @UserId AND SystemTagId = @ServerType AND deleted IS NULL
			END
			ELSE
			BEGIN
				-- NEW ACCOUNT
				
				INSERT INTO [dbo].[UserEmailAccount]
					([UserId], [SystemTagId], [UserEmailAccountUsername], [UserEmailAccountPassword], [register])
						VALUES (@UserId, @ServerType, @Username, @Password, GETDATE())
					
			END
		END
	END
END
