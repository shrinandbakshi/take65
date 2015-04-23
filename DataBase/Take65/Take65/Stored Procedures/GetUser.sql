
CREATE PROCEDURE [dbo].[GetUser]
	@Id DECIMAL(18, 0) = NULL,
	@FacebookId NVARCHAR(50) = NULL,
	@GUID VARCHAR(100) = NULL,
	@Login NVARCHAR(250) = NULL,
	@Password NVARCHAR(250) = NULL,
	@Email NVARCHAR(250) = NULL,
	@ReturnXml XML OUTPUT
AS
	SET @ReturnXml =  (

		SELECT TOP 1 [UserId] as 'Id'
			,[UserFacebookId] as 'FacebookId'
			,[UserFacebookTokenShort] as 'FacebookTokenShortLived'
			,[UserFacebookTokenLong] as 'FacebookTokenLongLived'
			,[UserFacebookTokenLongExpires] as 'FacebookTokenLongExpires'
			,[UserGUID] as 'GUID'
			,[UserName] as 'Name'
			,[UserLogin] as 'Login'
			,[UserEmail] as 'Email'
			,[UserPassword] as 'Password'
			,[UserChangePasswordNextLogin] as 'ChangePasswordNextLogin'
			,[UserBirthdate] as 'Birthdate'
			,[UserGender] as 'Gender'
			,[UserAddressState] as 'AddressState'
			,[UserAddressCity] as 'AddressCity'
			,[UserAddressPostalCode] as 'AddressPostalCode'
			,(SELECT SystemTagId as 'int' FROM UserPreference WHERE UserPreference.UserId = [User].UserId FOR XML PATH(''), TYPE) AS 'Preferences'
			,[register]
			,[lastupdate]
			,[inactive]
			,[deleted]
		FROM [dbo].[User]
		WHERE [deleted] IS NULL
		AND (@Id IS NULL OR @Id = [UserId])
		AND (@Login IS NULL OR @Login = [UserLogin])
		AND (@Password IS NULL OR @Password = [UserPassword])
		AND (@FacebookId IS NULL OR @FacebookId = [UserFacebookId])
		AND (@Email IS NULL OR @Email = [UserEmail])
		AND (@GUID IS NULL OR @GUID = [UserGUID])

		FOR XML PATH('User')
	)





