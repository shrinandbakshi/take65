CREATE PROCEDURE [dbo].[GetSuggestionBox]
	@SuggestionId NUMERIC(18, 0) = NULL,
	@UserId NUMERIC(18, 0) = NULL,
	@ReturnXml XML OUTPUT
AS
IF (@SuggestionId IS NULL)
BEGIN
	IF (@UserId IS NULL)
	BEGIN
		SET @ReturnXml =  (
			SELECT 
				[SuggestionId] AS 'Id',
				[SuggestionName] AS 'Name',
				[SuggestionUrl] AS 'Url',
				[SuggestionDescription] AS 'Description',
				[SuggestionImage] AS 'Image',
				[lastupdate] AS 'lastupdate',
				[register] AS 'register',
				0 AS 'Preferred'
			FROM 
				[dbo].[SuggestionBox]
			WHERE 
				[deleted] IS NULL
			ORDER BY 
				register DESC
			FOR 
				XML PATH('SuggestionBox'), TYPE, ROOT('ArrayOfSuggestionBox'))
	END
	ELSE
	BEGIN
		SET @ReturnXml = (
			SELECT
				[SuggestionBox].[SuggestionId] AS 'Id',
				[SuggestionName] AS 'Name',
				[SuggestionUrl] AS 'Url',
				[SuggestionDescription] AS 'Description',
				[SuggestionImage] AS 'Image',
				[lastupdate] AS 'lastupdate',
				[SuggestionBox].[register] AS 'register',
				(
					SELECT 
						CAST(COUNT(1) AS BIT) 
					FROM 
						[dbo].[SuggestionBoxTag]					
					INNER JOIN
						[dbo].[UserPreference]
					ON
						[SuggestionBoxTag].[SystemTagId] = [UserPreference].[SystemTagId]
					AND
						[UserPreference].[UserId] = @UserId
					LEFT OUTER JOIN
						[dbo].[UserSuggestionBox]
					ON
						[UserSuggestionBox].[SuggestionBoxId] = [SuggestionBoxTag].[SuggestionId]
					AND
						[UserSuggestionBox].[UserId] = @UserId					
					WHERE
						[SuggestionBox].[SuggestionId] = [SuggestionBoxTag].[SuggestionId]
					AND
						(
							[UserSuggestionBox].[UserSuggestionBoxAvoid] IS NULL
						OR
							[UserSuggestionBox].[UserSuggestionBoxAvoid] = 0
						)
				) AS 'Preferred'
			FROM 
				[dbo].[SuggestionBox]
			WHERE 
				[SuggestionBox].[deleted] IS NULL			
			AND NOT EXISTS
				(
					SELECT 
						SuggestionBoxId 
					FROM 
						[dbo].[UserSuggestionBox]
					WHERE 
						[UserSuggestionBox].[SuggestionBoxId] = [SuggestionBox].[SuggestionId]
					AND
						UserSuggestionBox.UserId = @UserId 
					AND
						UserSuggestionBoxAvoid = 1
					AND 
						register > DATEADD(DAY, -5, GETDATE())
				)		
			AND NOT EXISTS
				(
					SELECT 
						[UserWidget].[UserWidgetId] 
					FROM 
						[dbo].[UserWidget]
					INNER JOIN
						[dbo].[UserWidgetTrustedSource]
					ON
						[UserWidgetTrustedSource].[UserWidgetId] = [UserWidget].[UserWidgetId]
					WHERE
						[UserWidget].[UserId] = @UserId
					AND
						[SuggestionBox].[SuggestionUrl] = [UserWidgetTrustedSource].[UserWidgetTrustedSourceUrl]
				)
			ORDER BY
				Preferred DESC
			FOR 
				XML PATH('SuggestionBox'), TYPE, ROOT('ArrayOfSuggestionBox')
		)
	END
END
ELSE
BEGIN	
	SET @ReturnXml = (
		SELECT 
			[SuggestionId] AS 'Id',
			[SuggestionName] AS 'Name',
			[SuggestionUrl] AS 'Url',
			[SuggestionDescription] AS 'Description',
			[SuggestionImage] AS 'Image',
			[lastupdate] AS 'lastupdate',
			[register] AS 'register',
			(
				SELECT 
					* 
				FROM 
					[SystemTag] 
				WHERE 
					EXISTS (
						SELECT 
							[SystemTagId] 
						FROM 
							[SuggestionBoxTag],[SuggestionBox] 
						WHERE 
							[SuggestionBoxTag].[SuggestionId] = @SuggestionId 
						AND	
							[SuggestionBox].[SuggestionId] = [SuggestionBoxTag].[SuggestionId]
					) 
				FOR 
					XML PATH('SystemTag'), TYPE, ROOT('SystemTagList')
			) AS 'tags',
			0 AS 'Preferred'
		FROM 
			[dbo].[SuggestionBox]
		WHERE 
			[deleted] IS NULL
		AND 
			SuggestionId = @SuggestionId
		FOR 
			XML PATH('SuggestionBox'), TYPE
	)
END
