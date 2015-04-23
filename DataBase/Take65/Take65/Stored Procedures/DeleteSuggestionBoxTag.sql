CREATE PROCEDURE [dbo].[DeleteSuggestionBoxTag]
	@SuggestionId NUMERIC(18, 0) = NULL
AS
	BEGIN
			DELETE FROM [dbo].[SuggestionBoxTag]
			WHERE [SuggestionId]=@SuggestionId
	END