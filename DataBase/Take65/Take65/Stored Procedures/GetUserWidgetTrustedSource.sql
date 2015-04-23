CREATE PROCEDURE [dbo].[GetUserWidgetTrustedSource]
	@UserWidgetId NUMERIC(18,0) = NULL,
	@XmlReturn XML OUTPUT
AS
BEGIN

	SET @XmlReturn = (
		SELECT UWTS.[UserWidgetId]
			,UWTS.[TrustedSourceId]
			,UWTS.SystemTagId
			,ISNULL(TS.TrustedSourceName, UWTS.[UserWidgetTrustedSourceName]) as 'Name'
			,ISNULL(TS.TrustedSourceUrl, UWTS.[UserWidgetTrustedSourceUrl]) as 'Url'
			,UWTS.[register]
		FROM [dbo].[UserWidgetTrustedSource] UWTS
		LEFT OUTER JOIN [TrustedSource] TS ON UWTS.TrustedSourceId = TS.TrustedSourceId
		WHERE [UserWidgetId] = @UserWidgetId
		FOR XML PATH('UserWidgetTrustedSource'), TYPE, ROOT('ArrayOfUserWidgetTrustedSource')
	)

END

	 
	 