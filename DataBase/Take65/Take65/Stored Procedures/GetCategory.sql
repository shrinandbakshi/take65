
CREATE PROCEDURE [dbo].[GetCategory]
	@UserId NUMERIC(18, 0) = NULL,
	@UserWidgetId NUMERIC(18, 0) = NULL,
	@ReturnXml XML OUTPUT
AS
	SET @ReturnXml =  ( 
						SELECT SystemTagId as 'Id',
						   SystemTagDisplay as 'Name',
						   SystemTagIcon as 'Icon',
						   (
							SELECT 
								 TS.TrustedSourceId as 'Id'
								,TS.TrustedSourceName as 'Name'
								,TS.TrustedSourceIcon as 'Icon' 
								,ISNULL((SELECT TOP 1 1 FROM UserWidgetTrustedSource WHERE UserWidgetTrustedSource.SystemTagId = ST.[SystemTagId] AND UserWidgetTrustedSource.TrustedSourceId = TS.TrustedSourceId  AND UserWidgetTrustedSource.UserWidgetId = @UserWidgetId), 0) AS 'UserWidgetSelected'
							FROM TRUSTEDSOURCE TS
							INNER JOIN TRUSTEDSOURCEFEED TSF ON TSF.TrustedSourceId = TS.TrustedSourceId
							INNER JOIN TRUSTEDSOURCEFEEDTAG TSFT ON TSFT.TrustedsourceFeedId = TSF.TrustedsourceFeedId
							WHERE SYSTEMTAGID = ST.[SystemTagId]
							GROUP BY TS.TrustedSourceId, TS.TRustedSourceName, TS.TrustedSourceIcon
							FOR XML PATH('TrustedSource'), TYPE
						   ) as 'TrustedSources'

					FROM SYSTEMTAG ST
					WHERE TAGTypeId = 3
					FOR XML PATH('Category'), ROOT('ArrayOfCategory')
)


