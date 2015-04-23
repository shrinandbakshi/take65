CREATE PROCEDURE IgnoreSuggestion
	@SuggestionBoxId NUMERIC(18, 0),
	@UserId NUMERIC(18, 0)
AS
	INSERT INTO dbo.UserSuggestionBox (UserId, SuggestionBoxId, UserSuggestionBoxAvoid, register)
		VALUES (@UserId, @SuggestionBoxId, 1, GETDATE())