CREATE PROCEDURE [dbo].[SaveSuggestionBoxTag]


	@Id DECIMAL(18,0) = NULL,
	@SuggestionId DECIMAL(18,0) = NULL,
	@SystemTagId DECIMAL(18,0) = NULL,

	@ReturnId NUMERIC(18 ,0) OUTPUT

AS
BEGIN

	INSERT INTO [dbo].[SuggestionBoxTag]
           ([SuggestionId]
           ,[SystemTagId]
		   )
	VALUES
		(@SuggestionId
		,@SystemTagId
		)

	SET @ReturnId = @@IDENTITY	
END