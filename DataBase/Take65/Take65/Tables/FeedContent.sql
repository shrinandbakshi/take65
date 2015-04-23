CREATE TABLE [dbo].[FeedContent] (
    [FeedContentId]             NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [TrustedSourceFeedId]       NUMERIC (18)   NOT NULL,
    [TrustedSourceFeedCategory] NVARCHAR (255) NULL,
    [TrustedSourceId]           NUMERIC (18)   NULL,
    [TrustedSourceName]         NVARCHAR (250) NULL,
    [TrustedSourceOpenIFrame]   BIT            NULL,
    [FeedTitle]                 NVARCHAR (MAX) NOT NULL,
    [FeedLink]                  NVARCHAR (250) NOT NULL,
    [FeedDescription]           NVARCHAR (MAX) NULL,
    [FeedPubDate]               DATETIME       NULL,
    [FeedLastModified]          DATETIME       NULL,
    [FeedThumb]                 NVARCHAR (250) NULL,
    [FeedGuid]                  NVARCHAR (250) NULL,
    [FeedViewCount]             NUMERIC (18)   NULL,
    [register]                  DATETIME       CONSTRAINT [DF_FeedContent_register] DEFAULT (getdate()) NULL,
    [inactive]                  DATETIME       NULL,
    [lastmodified]              DATETIME       NULL,
    [deleted]                   DATETIME       NULL,
    [tmpImageSync]              DATETIME       NULL,
    CONSTRAINT [PK_FeedContent] PRIMARY KEY CLUSTERED ([FeedContentId] ASC)
);


GO
CREATE TRIGGER [dbo].[FeedContentInsert] ON [dbo].[FeedContent] 
	AFTER INSERT
AS 

BEGIN

	CREATE TABLE #Temp ([TrustedSourceId] DECIMAL(18,0), [TrustedSourceName] NVARCHAR(250), [TrustedSourceOpenIFrame] BIT, [TrustedSourceFeedCategory] NVARCHAR(250))

	INSERT #Temp 
	
	SELECT TS.[TrustedSourceId], TS.[TrustedSourceName],TS.[TrustedSourceOpenIFrame], STUFF(
															(
																SELECT ',' + ST.SystemTagDisplay
																from TrustedSourceFeedTag TSFT
																INNER JOIN SystemTag ST ON TSFT.SystemTagId = ST.SystemTagId
																where TrustedSourceFeedId = TSF.TrustedSourceFeedId

																FOR XML PATH('')
															),1, 1, ''
														)
	FROM [dbo].[TrustedSourceFeed] TSF
	INNER JOIN [dbo].[TrustedSource] TS ON TS.[TrustedSourceId] = TSF.[TrustedSourceId]
	WHERE TSF.[TrustedSourceFeedId] = (SELECT [TrustedSourceFeedId] FROM INSERTED)
	

	UPDATE [FeedContent]
	SET [TrustedSourceId] = (SELECT [TrustedSourceId] FROM #Temp),
		[TrustedSourceName] = (SELECT [TrustedSourceName] FROM #Temp),
		[TrustedSourceOpenIFrame] = (SELECT [TrustedSourceOpenIFrame] FROM #Temp),
		[TrustedSourceFeedCategory] = (SELECT [TrustedSourceFeedCategory] FROM #Temp)
	WHERE [FeedContentId] = (SELECT [FeedContentId] FROM INSERTED)


END
