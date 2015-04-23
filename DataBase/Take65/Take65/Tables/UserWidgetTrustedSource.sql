CREATE TABLE [dbo].[UserWidgetTrustedSource] (
    [UserWidgetId]                NUMERIC (18)   NOT NULL,
    [SystemTagId]                 NUMERIC (18)   NULL,
    [TrustedSourceId]             NUMERIC (18)   NULL,
    [UserWidgetTrustedSourceName] NVARCHAR (250) NULL,
    [UserWidgetTrustedSourceUrl]  NVARCHAR (250) NULL,
    [register]                    DATETIME       CONSTRAINT [DF_UserWidgetTrustedSource_register] DEFAULT (getdate()) NOT NULL
);


GO
CREATE TRIGGER [dbo].[UserWidgetTrustedSourceInsert] ON dbo.UserWidgetTrustedSource 
	AFTER INSERT
AS 

BEGIN

	CREATE TABLE #Temp ([UserWidgetId] DECIMAL(18,0), [TrustedSourceId] DECIMAL(18,0), [SystemTagId] DECIMAL(18,0))

	INSERT #Temp 
	SELECT [UserWidgetId]
		  ,[TrustedSourceId]
		  ,[SystemTagId]
	FROM INSERTED


	INSERT INTO [UserWidgetTrustedSourceFeed]
	SELECT (SELECT [UserWidgetId] FROM #Temp)
		  ,TSF.[TrustedSourceFeedId]
	FROM [dbo].[TrustedSourceFeed] TSF
	INNER JOIN [TrustedSourceFeedTag] TSFT ON TSF.[TrustedSourceFeedId] = TSFT.[TrustedSourceFeedId]
	WHERE TSFT.[SystemTagId] = (SELECT [SystemTagId] FROM #Temp)
	AND TSF.[TrustedSourceId] = (SELECT [TrustedSourceId] FROM #Temp)



  


END
