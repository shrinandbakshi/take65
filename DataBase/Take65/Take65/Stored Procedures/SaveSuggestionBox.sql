CREATE PROCEDURE [dbo].[SaveSuggestionBox]
	@SuggestionId NUMERIC(18,0)	= NULL,
	@SuggestionName NVARCHAR(255) = NULL,
	@SuggestionUrl NVARCHAR(250) = NULL,
	@SuggestionDescription NVARCHAR(max) = NULL,
	@SuggestionImage NVARCHAR(255) = NULL,
	@deleted DATETIME = NULL
	
	,@ReturnId NUMERIC(18 ,0) OUTPUT
AS

BEGIN
	IF @SuggestionId IS NOT NULL
	BEGIN
	
	
	UPDATE [dbo].[SuggestionBox]
	   SET [SuggestionName] = ISNULL(@SuggestionName, [SuggestionName])
		  ,[SuggestionUrl] = ISNULL(@SuggestionUrl, [SuggestionUrl])
		  ,[SuggestionDescription] = ISNULL(@SuggestionDescription, [SuggestionDescription])
		  ,[SuggestionImage] = ISNULL(@SuggestionImage, [SuggestionImage])
		  ,[lastupdate] = GETDATE()
		  ,[deleted] = ISNULL(@deleted, [deleted])
	 WHERE [SuggestionId] = @SuggestionId
	
	SET @ReturnId = @SuggestionId
	
	END
	ELSE
	BEGIN
	
	INSERT INTO [dbo].[SuggestionBox]
           ([SuggestionName]
           ,[SuggestionUrl]
           ,[SuggestionDescription]
           ,[SuggestionImage]
           ,[register]
           )
     VALUES
           (@SuggestionName
           ,@SuggestionUrl
           ,@SuggestionDescription
           ,@SuggestionImage
           ,GETDATE()
           )
           
		SET @ReturnId = @@IDENTITY
		
	END
END