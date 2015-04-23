CREATE PROCEDURE [dbo].[GetUserWidgetCategory]
	@UserId NUMERIC(18,0) = NULL,

	@XmlReturn XML OUTPUT
AS

BEGIN

SET @XmlReturn = (

	SELECT UWT.SystemTagId as 'Id', ST.SystemTagDisplay as 'Display' FROM UserWidgetTag UWT
	INNER JOIN SystemTag ST ON ST.SystemTagId = UWT.SystemTagId
	Where UWT.UserId = @UserId
	GROUP BY UWT.SystemTagId,ST.SystemTagDisplay

FOR XML PATH('Tag'), TYPE, ROOT('ArrayOfTag')
)



/*
select Id,Display
	FROM 
	(
		--feed
		select TSFT.SystemTagId as 'Id',ST.SystemTagDisplay as 'Display' from userwidgettrustedsourcefeed UWTSF
		INNER JOIN trustedsourcefeedtag TSFT ON TSFT.TrustedSourceFeedId = UWTSF.TrustedSourceFeedId
		INNER JOIN SystemTag ST ON ST.SystemTagId = TSFT.SystemTagId
		INNER JOIN userwidget UW ON UWTSF.UserWidgetId  = UW.UserWidgetId
		where UW.userid = @UserId
		UNION
		--bookmark
		SELECT TST.SystemTagId as 'Id',ST.SystemTagDisplay as 'Display' FROM trustedsourcetag TST
		INNER JOIN userwidgettrustedsource UWTS on UWTS.TrustedSourceId = TST.TrustedSourceId
		INNER JOIN SystemTag ST ON ST.SystemTagId = TST.SystemTagId
		INNER JOIN userwidget UW ON UWTS.UserWidgetId  = UW.UserWidgetId
		where UW.userid = @UserId
		
	) as a
	GROUP BY Id,Display
*/

/*
	SET @XmlReturn = (
			select ST.SystemTagId as 'Id', ST.SystemTagDisplay as 'Display' from userwidgettrustedsourcefeed UWTSF
			INNER JOIN trustedsourcefeedtag TSFT ON TSFT.TrustedSourceFeedId = UWTSF.TrustedSourceFeedId
			INNER JOIN systemtag ST ON ST.SystemTagId = TSFT.SystemTagId
			INNER JOIN userwidget UWTS ON UWTSF.UserWidgetId  = UWTS.UserWidgetId
			where UWTS.UserId = @UserId
			GROUP BY ST.SystemTagId, ST.SystemTagDisplay
			ORDER BY ST.SystemTagDisplay

			FOR XML PATH('Tag'), TYPE, ROOT('ArrayOfTag')
	 )
*/
		
END	
	