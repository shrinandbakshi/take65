CREATE PROCEDURE [dbo].[GetTrustedSource]
@TrustedSourceId NUMERIC(18, 0) = NULL,
	@UserWidgetId NUMERIC(18, 0) = NULL,
	@SystemTagId INT = NULL,
	@TrustedSourceTypeId numeric(18, 0) = NULL,
	@XmlReturn XML OUTPUT
AS

IF @TrustedSourceId IS NOT NULL
BEGIN
	SET @XmlReturn =  ( 
				SELECT TS.[TrustedSourceId] AS 'Id'
								,TS.[TrustedSourceName] AS 'Name'
								,TS.[TrustedSourceIcon] AS 'Icon'
								,TS.[TrustedSourceTypeId] AS 'SourceTypeId'
								,TS.[TrustedSourceUrl] AS 'Url'		
								,(SELECT TOP 1 ST.SystemTagId AS 'Id', ST.SystemTagDisplay AS 'Display' FROM TrustedSourceTag TST INNER JOIN SystemTag ST ON ST.SystemTagId = TST.SystemTagId WHERE TST.TrustedSourceId = TS.TrustedSourceId FOR XML PATH('CurrentTag'), TYPE)
								,TS.TrustedSourceOpenIFrame AS 'OpenIFrame'
							FROM [dbo].[TrustedSource] TS
							
							INNER JOIN [dbo].[TrustedSourceTag] TST
							ON TS.TrustedSourceId = TST.TrustedSourceId
							WHERE	TS.deleted IS NULL
							AND		(TS.TrustedSourceId = @TrustedSourceId)
							FOR XML PATH('TrustedSource'), TYPE
							)
END
ELSE
BEGIN
	IF @UserWidgetId IS NOT NULL
		BEGIN

			SET @XmlReturn =  ( 

				SELECT TS.[TrustedSourceId] AS 'Id'
				,ISNULL(TS.[TrustedSourceName], UWTS.UserWidgetTrustedSourceName) AS 'Name'
				,TS.[TrustedSourceIcon] AS 'Icon'
				,TS.[TrustedSourceTypeId] AS 'SourceTypeId'
				,ISNULL(TS.[TrustedSourceUrl], UWTS.UserWidgetTrustedSourceUrl)  AS 'Url'		
				,(SELECT TOP 1 ST.SystemTagId AS 'Id', ST.SystemTagDisplay AS 'Display' FROM TrustedSourceTag TST INNER JOIN SystemTag ST ON ST.SystemTagId = TST.SystemTagId WHERE TST.TrustedSourceId = TS.TrustedSourceId FOR XML PATH('CurrentTag'), TYPE)
				,TS.TrustedSourceOpenIFrame AS 'OpenIFrame'
				
				FROM [dbo].[TrustedSource] TS
				RIGHT OUTER JOIN [userwidgettrustedsource] UWTS
				ON UWTS.TrustedSourceId = TS.[TrustedSourceId]
				WHERE UserWidgetId = @UserWidgetId
				GROUP BY TS.[TrustedSourceId]
				,TS.[TrustedSourceName] 
				,TS.[TrustedSourceIcon]
				,TS.[TrustedSourceTypeId] 
				,TS.[TrustedSourceUrl]
				,TS.TrustedSourceOpenIFrame
				,UWTS.UserWidgetTrustedSourceUrl
				,UWTS.UserWidgetTrustedSourceName

				FOR XML PATH('TrustedSource'), TYPE, ROOT('ArrayOfTrustedSource')
			)
		END
	ELSE
		BEGIN

			IF @TrustedSourceTypeId  IS NOT NULL AND @TrustedSourceTypeId = 2 --2 = BOOKMARK
			BEGIN
				SET @XmlReturn =  ( 
				SELECT TS.[TrustedSourceId] AS 'Id'
								,TS.[TrustedSourceName] AS 'Name'
								,TS.[TrustedSourceIcon] AS 'Icon'
								,TS.[TrustedSourceTypeId] AS 'SourceTypeId'
								,TS.[TrustedSourceUrl] AS 'Url'		
								,(SELECT TOP 1 ST.SystemTagId AS 'Id', ST.SystemTagDisplay AS 'Display' FROM TrustedSourceTag TST INNER JOIN SystemTag ST ON ST.SystemTagId = TST.SystemTagId WHERE TST.TrustedSourceId = TS.TrustedSourceId FOR XML PATH('CurrentTag'), TYPE)
								,TS.TrustedSourceOpenIFrame AS 'OpenIFrame'
							FROM [dbo].[TrustedSource] TS
							
							INNER JOIN [dbo].[TrustedSourceTag] TST
							ON TS.TrustedSourceId = TST.TrustedSourceId
							WHERE	TS.deleted IS NULL
							AND		(@SystemTagId IS NULL OR TST.SystemTagId =@SystemTagId)
							AND		(@TrustedSourceTypeId IS NULL OR @TrustedSourceTypeId = TS.[TrustedSourceTypeId])
							FOR XML PATH('TrustedSource'), TYPE, ROOT('ArrayOfTrustedSource')
							)
			END
			ELSE
			BEGIN

				SET @XmlReturn =  ( 
								SELECT TS.[TrustedSourceId] AS 'Id'
									,TS.[TrustedSourceName] AS 'Name'
									,TS.[TrustedSourceIcon] AS 'Icon'
									,TS.[TrustedSourceTypeId] AS 'SourceTypeId'
									,TS.[TrustedSourceUrl] AS 'Url'		
									,TS.TrustedSourceOpenIFrame AS 'OpenIFrame'
								FROM [dbo].[TrustedSource] TS
								INNER JOIN [dbo].[TrustedSourceFeed] TSF
								ON TS.[TrustedSourceId] = TSF.[TrustedSourceId]
								INNER JOIN [dbo].[TrustedSourceFeedTag] TSFT
								ON TSF.TrustedSourceFeedId = TSFT.TrustedSourceFeedId

								WHERE	TS.deleted IS NULL
								AND		(@SystemTagId IS NULL OR TSFT.SystemTagId = @SystemTagId)
								AND		(@TrustedSourceTypeId IS NULL OR @TrustedSourceTypeId = TS.[TrustedSourceTypeId])
								GROUP BY TS.[TrustedSourceId]
									,TS.[TrustedSourceName] 
									,TS.[TrustedSourceIcon]
									,TS.[TrustedSourceTypeId]
									,TS.[TrustedSourceUrl]
									,TS.TrustedSourceOpenIFrame
								FOR XML PATH('TrustedSource'), TYPE, ROOT('ArrayOfTrustedSource')
							)
				END
		END
END
					



						
							



