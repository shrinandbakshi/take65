CREATE PROCEDURE [dbo].[GetSuggestionBoxTag]
	@SuggestionId NUMERIC(18, 0) = NULL,
	@ReturnXml XML OUTPUT
AS
	BEGIN
SET @ReturnXml =  (
			SELECT [SuggestionBoxTagId] AS 'Id'
				  ,[SuggestionId] AS 'SuggestionId'
				  ,[SystemTagId] AS 'SystemTagId'
			FROM [dbo].[SuggestionBoxTag]
			WHERE [SuggestionId]=@SuggestionId
		FOR XML PATH('SuggestionBoxTag'), TYPE, ROOT('SuggestionBoxTagList'))
	END

