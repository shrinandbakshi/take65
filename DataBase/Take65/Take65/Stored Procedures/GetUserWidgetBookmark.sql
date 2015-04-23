CREATE PROCEDURE [dbo].[GetUserWidgetBookmark]
	@UserWidgetId NUMERIC(18, 0),
	@IsTrusted BIT = NULL,
	@XmlReturn XML OUTPUT
AS
		IF @IsTrusted IS NULL
			BEGIN

				SET @XmlReturn =	
				(	
				SELECT UWTS.[UserWidgetId]
					  ,UWTS.[TrustedSourceId]
					  ,UWTS.[UserWidgetTrustedSourceName] as 'Name'
					  ,UWTS.[UserWidgetTrustedSourceUrl] as 'Url'
					  ,(SELECT TOP 1 TrustedSourceIcon FROM [TrustedSource] WHERE dbo.GetDomainName([TrustedSource].TrustedSourceUrl) = dbo.GetDomainName(UWTS.[UserWidgetTrustedSourceUrl]) and deleted IS NULL) AS 'Icon'
					  ,'0' as 'OpenIFrame'
					  ,[register]
				FROM [dbo].[UserWidgetTrustedSource] UWTS
				WHERE TrustedSourceId IS NULL
				AND UserWidgetId = @UserWidgetId
				FOR XML PATH('UserWidgetTrustedSource'), TYPE, ROOT('ArrayOfUserWidgetTrustedSource')
				)
			END
			ELSE
			BEGIN

				SET @XmlReturn =	
				(
				SELECT UWTS.[UserWidgetId]
					  ,UWTS.[TrustedSourceId]
					  ,TS.TrustedSourceName as 'Name'
					  ,TS.TrustedSourceUrl as 'Url'
					  ,TS.TrustedSourceIcon as 'Icon'
					  ,TS.[TrustedSourceOpenIFrame] as 'OpenIFrame'
					  ,UWTS.[register]
				FROM [dbo].[UserWidgetTrustedSource] UWTS
				LEFT OUTER JOIN [TrustedSource] TS ON TS.TrustedSourceId = UWTS.TrustedSourceId
				WHERE UWTS.TrustedSourceId IS NOT NULL
				AND UserWidgetId = @UserWidgetId
				FOR XML PATH('UserWidgetTrustedSource'), TYPE, ROOT('ArrayOfUserWidgetTrustedSource')
				)
			END

			  
			
