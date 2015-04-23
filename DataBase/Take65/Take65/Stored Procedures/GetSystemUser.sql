CREATE PROCEDURE [dbo].[GetSystemUser]
	@UserId  NUMERIC(18,0) = NULL,
	@UserName NVARCHAR(255) = NULL,
	@UserFirstName NVARCHAR(255) = NULL,
	@UserLastName NVARCHAR(255) = NULL,
	@UserEmail NVARCHAR(255) = NULL,
	@UserPassword NVARCHAR(255) = NULL,
	@UserLastLogin SMALLDATETIME = NULL,
	@GUID UNIQUEIDENTIFIER = NULL,
	@UserLoginCount INT = NULL,
	@ForgotPassword UNIQUEIDENTIFIER = NULL,
	@lastupdate DATETIME = NULL,
	@inactive DATETIME = NULL,
	@deleted DATETIME = NULL,
	@register DATETIME = NULL,
	@XmlReturn XML OUTPUT
AS
BEGIN
	IF (@UserId IS NOT NULL OR @GUID IS NOT NULL)
	BEGIN
		SET @XmlReturn = (SELECT     [UserId] AS 'Id'
									,[UserName] AS 'UserName'
									,[UserFirstName] AS 'FirstName'
									,[UserLastName] AS 'LastName'
									,[UserEmail] AS 'Email'
									,[UserPassword] AS 'Password'
									,[UserLastLogin] AS 'LastLogin'
									,[UserGUID] AS 'GUID'
									,[ForgotPassword]
									,[register]
									,[lastupdate]
									,[inactive]
									,[deleted]
					  FROM [dbo].[SystemUser]
							WHERE (
								(@UserId IS NULL OR [UserId] = @UserId)
							AND (@UserName IS NULL OR [UserName]  = @UserName)
							AND (@GUID IS NULL OR [UserGUID] = @GUID) 
							AND (@inactive IS NULL AND [inactive] IS  NULL )
							AND [deleted] IS NULL)
							ORDER BY [UserName]
							FOR XML PATH('Page'))
	END
	ELSE
	BEGIN
		SET @XmlReturn = (SELECT     [UserId] AS 'Id'
									,[UserName] AS 'UserName'
									,[UserFirstName] AS 'FirstName'
									,[UserLastName] AS 'LastName'
									,[UserEmail] AS 'Email'
									,[UserPassword] AS 'Password'
									,[UserLastLogin] AS 'LastLogin'
									,[UserGUID] AS 'GUID'
									,[ForgotPassword]
									,[register]
									,[lastupdate]
									,[inactive]
									,[deleted]
					  FROM [dbo].[SystemUser]
							WHERE (
							    (@UserFirstName IS NULL OR [UserFirstName]  = @UserFirstName)
							AND (@UserLastName IS NULL OR [UserLastName]  = @UserLastName)
							AND (@UserEmail IS NULL OR [UserEmail]  = @UserEmail)
							AND (@UserLastLogin IS NULL OR [UserLastLogin]  = @UserLastLogin)
							AND (@register IS NULL OR [register]  = @register)
							AND (@inactive IS NULL AND [inactive] IS  NULL )
							AND [deleted] IS NULL)
							ORDER BY [UserName]
							FOR XML PATH('Page'), TYPE, ROOT('PageList'))
	END
END
