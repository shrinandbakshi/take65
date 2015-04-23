
CREATE PROCEDURE [dbo].[SaveUser]
	@Id DECIMAL(18,0)	= NULL,
	@FacebookId NVARCHAR(50) = NULL,
	@Name NVARCHAR(250) = NULL,
	@Email NVARCHAR(250) = NULL,
	@Login NVARCHAR(250) = NULL,
	@Password NVARCHAR(250) = NULL,
	@ChangePassword DATETIME = NULL,
	@Birthdate DATETIME = NULL,
	@Gender CHAR(1) = NULL,
	@AddressState NVARCHAR(50) = NULL,
	@AddressCity NVARCHAR(50) = NULL,
	@AddressPostalCode NVARCHAR(20) = NULL,
	@Preferences VARCHAR(500) = NULL,
	@deleted DATETIME = NULL,

	@ReturnId NUMERIC(18 ,0) OUTPUT
AS

BEGIN
	IF @Id IS NOT NULL
	BEGIN

		UPDATE [dbo].[User]
		   SET 
			  [UserName] = ISNULL(@Name,[UserName])
			  ,[UserEmail] = ISNULL(@Email,[UserEmail])
			  --,[UserLogin] = ISNULL(@Login,[UserLogin])
			  ,[UserPassword] = ISNULL(@Password,[UserPassword])
			  ,[UserChangePasswordNextLogin] = @ChangePassword
			  ,[UserBirthdate] = ISNULL(@Birthdate,[UserBirthdate])
			  ,[UserGender] = ISNULL(@Gender,[UserGender])
			  ,[UserAddressState] = ISNULL(@AddressState,[UserAddressState])
			  ,[UserAddressCity] = ISNULL(@AddressCity,[UserAddressCity])
			  ,[UserAddressPostalCode] = ISNULL(@AddressPostalCode,[UserAddressPostalCode])
			  ,[lastupdate] = GETDATE()
			  ,[deleted] = ISNULL(@deleted,[deleted])
		 WHERE [UserId] = @Id

		SET @ReturnId = @Id
	
	END
	ELSE
	BEGIN

		INSERT INTO [dbo].[User]
           (
           [UserName]
		   ,[UserFacebookId]
           ,[UserEmail]
           ,[UserLogin]
           ,[UserPassword]
           ,[UserBirthdate]
           ,[UserGender]
           ,[UserAddressState]
           ,[UserAddressCity]
           ,[UserAddressPostalCode]
           ,[register]
           )
     VALUES
           (
           @Name
		   ,@FacebookId
           ,@Email
           ,@Login
           ,@Password
           ,@Birthdate
           ,@Gender
           ,@AddressState
           ,@AddressCity
           ,@AddressPostalCode
           ,GETDATE()
           )

		SET @ReturnId = @@IDENTITY
		
	END
	
	/* INSERT / UPDATE INTERESTS */
	DELETE UserPreference WHERE UserId = @ReturnId
	
	DECLARE @pos INT
	DECLARE @len INT
	DECLARE @value varchar(8000)
		
		
	set @pos = 0
	set @len = 0

	IF (@Preferences IS NOT NULL)
	BEGIN
		SET @Preferences = @Preferences + ','
		WHILE CHARINDEX(',', @Preferences, @pos+1)>0
		BEGIN
			set @len = CHARINDEX(',', @Preferences, @pos+1) - @pos
			set @value = SUBSTRING(@Preferences, @pos, @len)
			IF (@value <> '')
			BEGIN
				INSERT INTO UserPreference (UserId, SystemTagId, register) VALUES (@ReturnId, @value, GETDATE())
			END

			set @pos = CHARINDEX(',', @Preferences, @pos+@len) +1
		END
	END
	/* INSERT / UPDATE INTERESTS */

END






