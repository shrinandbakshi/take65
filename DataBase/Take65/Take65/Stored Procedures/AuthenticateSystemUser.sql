CREATE PROCEDURE [dbo].[AuthenticateSystemUser]
	 @UserName NVARCHAR(255) = NULL
	,@Password NVARCHAR(50) = NULL
	,@XmlReturn XML OUTPUT
AS
BEGIN
	IF(@UserName IS NOT NULL AND @Password IS NOT NULL)
		BEGIN
		SET @XmlReturn = (SELECT [UserId] AS 'Id'
								,[UserName] AS 'UserName'
								,[UserFirstName] AS 'FirstName'
								,[UserLastName] AS 'LastName'
								,[UserEmail] AS 'Email'
								,[UserLastLogin] AS 'LastLogin'
								,[UserGUID] AS 'GUID'
								,[register] AS 'Register'
								,[lastupdate] AS 'LastUpdate'
								,[inactive] AS 'Inactive'
							FROM [dbo].[SystemUser]	
							WHERE [UserName] = @UserName AND [UserPassword] = @Password AND [deleted] IS NULL FOR XML PATH('SystemUser'), TYPE)

		UPDATE [dbo].[SystemUser] 
		SET [UserLastLogin] = GETDATE()
		WHERE [UserName] = @UserName AND [UserPassword] = @Password AND [deleted] IS NULL
		
		UPDATE [dbo].[SystemUser] 
		SET [ForgotPassword] = NULL 
		WHERE [ForgotPassword] IS NOT NULL AND [UserName] = @UserName AND [UserPassword] = @Password AND [deleted] IS NULL		
		END
END