CREATE PROCEDURE [dbo].[GetUserWidget]
	@Id NUMERIC(18,0) = NULL,
	@UserId NUMERIC(18, 0) = NULL,
	@SystemTagId NUMERIC(18, 0) = NULL,

	@XmlReturn XML OUTPUT
AS
BEGIN
	IF @Id IS NOT NULL
	BEGIN
		SET @XmlReturn =  ( SELECT [UserWidgetId] AS 'Id'
							  ,[UserWidgetName] AS 'Name'
							  ,[UserWidgetDefault] AS 'DefaultWidget'
							  ,[SystemTagId]
							  ,[UserWidgetExtraInfoXML] AS 'UserWidgetExtraInfoXML'
							  ,[UserWidget].[UserId] AS 'UserId'
							  ,[UserWidgetRow] AS 'Row'
							  ,[UserWidgetColumn] AS 'Col' 

							  ,[UserWidget].[register]
							  ,[UserWidget].[lastupdate]
						  FROM [dbo].[UserWidget]
						WHERE 
							[UserWidget].deleted IS NULL
							AND [UserWidgetId] = @Id
							AND (@SystemTagId IS NULL OR [SystemTagId] = @SystemTagId)
						FOR XML PATH('UserWidget'), TYPE)
	END
	ELSE IF @UserId IS NOT NULL
	BEGIN
		SET @XmlReturn =  ( SELECT [UserWidgetId] AS 'Id'
							  ,[UserWidgetName] AS 'Name'
							  ,[UserWidgetDefault] AS 'DefaultWidget'
							  ,[SystemTagId]
							  ,[UserWidgetExtraInfoXML] AS 'UserWidgetExtraInfoXML'
							  ,[UserWidget].[UserId] AS 'UserId'
							  ,[UserWidgetSize] as 'Size'
							  ,[UserWidgetRow] AS 'Row'
							  ,[UserWidgetColumn] AS 'Col' 
							  ,
							   STUFF((
								select distinct ',' + cast(Category as VARCHAR) 
								FROM 
								(
									SELECT SystemTagId as 'Category'
									FROM UserWidgetTag UWT
									where UWT.userwidgetid = [UserWidget].[UserWidgetId]
								) as a

								FOR XML PATH('')),1, 1, ''
							) as 'Category'
							  ,[UserWidget].[lastupdate]
						  FROM [dbo].[UserWidget]
						WHERE [UserWidget].deleted IS NULL
							AND (@SystemTagId IS NULL OR [SystemTagId] = @SystemTagId) 
							AND [UserId] = @UserId
							--ORDER BY [SystemTagId], [register] DESC
							ORDER BY [UserWidgetSize] DESC, UserWidgetOrder ASC, [register] DESC
						FOR XML PATH('UserWidget'), TYPE, ROOT('ArrayOfUserWidget'))
	END
END

/*

STUFF((
--feeds
select distinct ',' + cast(Category as VARCHAR) 
FROM 
(
	--feed
	select TSFT.SystemTagId as 'Category' from userwidgettrustedsourcefeed UWTSF
	INNER JOIN trustedsourcefeedtag TSFT ON TSFT.TrustedSourceFeedId = UWTSF.TrustedSourceFeedId
	where userwidgetid = [UserWidget].[UserWidgetId]
	UNION
	--bookmark
	SELECT TST.SystemTagId as 'Category' FROM trustedsourcetag TST
	INNER JOIN userwidgettrustedsource UWTS on UWTS.TrustedSourceId = TST.TrustedSourceId
	where UWTS.userwidgetid = [UserWidget].[UserWidgetId]
) as a

FOR XML PATH('')),1, 1, ''

*/






