CREATE PROCEDURE [dbo].[GetUserEmailAccount]
	@UserId NUMERIC(18, 0),
	@ReturnXml XML OUTPUT
AS
BEGIN
	-- IT FORCES TOP 1 BECAUSE THE USER SUPPOSE TO ONLY HAVE ONE ACTIVE EMAIL ACCOUNT
	SET @ReturnXml = (
	SELECT  TOP 1 [UserEmailAccountId] AS 'Id'
			,(SELECT TOP 1 SystemTagNormalized FROM SystemTag WHERE SystemTag.SystemTagId = [UserEmailAccount].SystemTagId) AS 'EmailServer'
			,[UserEmailAccountUsername] AS 'Username'
			,[UserEmailAccountPassword] AS 'Password'
			,[lastupdate] AS 'LastUpdate'
		FROM [UserEmailAccount]
			WHERE [UserId] = @UserId
				AND deleted IS NULL
		FOR XML PATH('EmailAccount'), TYPE)
END
