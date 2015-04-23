CREATE PROCEDURE [dbo].[GetUserAll]
	@Active BIT = NULL,
	@ReturnXml XML OUTPUT
AS
	SET @ReturnXml =  (

		SELECT [UserId] as 'Id'
			,[UserFacebookId] as 'FacebookId'
			,[UserGUID] as 'GUID'
			,[UserName] as 'Name'
			,[UserLogin] as 'Login'
			,[UserEmail] as 'Email'
			,[UserPassword] as 'Password'
			,[UserBirthdate] as 'Birthdate'
			,[UserGender] as 'Gender'
			,[UserAddressState] as 'AddressState'
			,[UserAddressCity] as 'AddressCity'
			,[UserAddressPostalCode] as 'AddressPostalCode'
			,[register] as 'Register'
			,[lastupdate]
			,[inactive]
			,[deleted]
		FROM [dbo].[User]
		WHERE [deleted] IS NULL
		AND (@Active IS NULL OR (@Active IS NOT NULL AND [inactive] IS NULL))
		ORDER BY register DESC

		FOR XML PATH('User'), TYPE, ROOT('ArrayOfUser')
	)