CREATE PROCEDURE [dbo].[GetSafeWebsiteUnmapped]
	@ReturnXml XML OUTPUT
AS

		SET @ReturnXml =  (

			SELECT 0 AS 'Id'
				  ,[UserWidgetTrustedSourceUrl] AS 'Url'
				  ,'0' AS 'OpenIFrame'
				  ,[register] AS 'register'
			FROM [dbo].UserWidgetTrustedSource
			WHERE TrustedSourceId IS NULL AND SystemTagId IS NULL
			AND [UserWidgetTrustedSourceUrl] NOT IN(SELECT TrustedSourceUrl FROM TrustedSource WHERE TrustedSourceTypeId = 2 AND deleted IS NULL)
			ORDER BY register DESC
			FOR XML PATH('SafeWebsite'), TYPE, ROOT('ArrayOfSafeWebsite'))
	