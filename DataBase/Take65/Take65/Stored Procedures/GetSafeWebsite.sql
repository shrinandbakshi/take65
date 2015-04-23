CREATE PROCEDURE [dbo].[GetSafeWebsite]
	@SafeWebsiteId NUMERIC(18, 0) = NULL,
	@SafeWebsiteUrl NVARCHAR(255) = NULL,
	@ReturnXml XML OUTPUT
AS
IF (@SafeWebsiteId IS NULL)
BEGIN

	IF (@SafeWebsiteUrl IS NULL)
	BEGIN
		SET @ReturnXml =  (

			SELECT [SafeWebsiteId] AS 'Id'
				  ,[SafeWebsiteUrl] AS 'Url'
				  ,[SafeWebsiteOpenIFrame] AS 'OpenIFrame'
				  ,[lastupdate] AS 'lastupdate'
				  ,[register] AS 'register'
			FROM [dbo].[SafeWebsite]
			WHERE [deleted] IS NULL
			ORDER BY register DESC
			FOR XML PATH('SafeWebsite'), TYPE, ROOT('ArrayOfSafeWebsite'))
	END
	ELSE
	BEGIN
		SET @ReturnXml =  (
		SELECT [SafeWebsiteId] AS 'Id'
				  ,[SafeWebsiteUrl] AS 'Url'
				  ,[SafeWebsiteOpenIFrame] AS 'OpenIFrame'
				  ,[lastupdate] AS 'lastupdate'
				  ,[register] AS 'register'
			FROM [dbo].[SafeWebsite]
			WHERE [deleted] IS NULL
			AND REPLACE((REPLACE([SafeWebsiteUrl], 'http://', '')), 'https://', '') = @SafeWebsiteUrl
			ORDER BY register DESC
			FOR XML PATH('SafeWebsite'), TYPE, ROOT('ArrayOfSafeWebsite'))
	END
END
ELSE
BEGIN
SET @ReturnXml =  (
			SELECT [SafeWebsiteId] AS 'Id'
			  ,[SafeWebsiteUrl] AS 'Url'
			  ,[SafeWebsiteOpenIFrame] AS 'OpenIFrame'
			  ,[lastupdate] AS 'lastupdate'
			  ,[register] AS 'register'

			FROM [dbo].[SafeWebsite]
			WHERE [deleted] IS NULL
			AND [SafeWebsiteId] = @SafeWebsiteId
		FOR XML PATH('SafeWebsite'), TYPE)
END


