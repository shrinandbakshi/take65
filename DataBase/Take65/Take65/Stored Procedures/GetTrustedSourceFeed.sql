CREATE PROCEDURE [dbo].[GetTrustedSourceFeed]
	@TrustedSourceId numeric(18, 0),
	@XmlReturn XML OUTPUT
AS
	SET @XmlReturn =  (
			SELECT [TrustedSourceFeedId] AS 'Id'
			  ,[TrustedSourceId] AS 'TrustedSourceId'
			  ,[TrustedSourceFeedName] AS 'Name'
			  ,[TrustedSourceFeedUrl] AS 'Url'
			  ,[lastsync] AS 'LastSync'
			  ,[register] AS 'Register'
			  ,[lastupdate] AS 'LastUpdate'
		  FROM [dbo].[TrustedSourceFeed]
			WHERE [deleted] IS NULL
			AND [TrustedSourceId] = @TrustedSourceId
		FOR XML PATH('TrustedSourceFeed'), TYPE, ROOT('ArrayOfTrustedSourceFeed'))