CREATE PROCEDURE [dbo].[SaveSafeWebsite]
	@SafeWebsiteId NUMERIC(18,0)	= NULL,
	@SafeWebsiteUrl NVARCHAR(250) = NULL,
	@SafeWebsiteOpenIFrame BIT,
	@deleted DATETIME = NULL
	
	,@ReturnId NUMERIC(18 ,0) OUTPUT
AS

BEGIN
	IF @SafeWebsiteId IS NOT NULL
	BEGIN
	
		IF (@deleted IS NULL)
		BEGIN
			UPDATE [dbo].[SafeWebsite]
			   SET [SafeWebsiteUrl] = ISNULL(@SafeWebsiteUrl, [SafeWebsiteUrl])
			      ,[SafeWebsiteOpenIFrame] = ISNULL(@SafeWebsiteOpenIFrame, [SafeWebsiteOpenIFrame])
				  ,[lastupdate] = GETDATE()
				  ,[deleted] = ISNULL(@deleted, [deleted])
			 WHERE [SafeWebsiteId] = @SafeWebsiteId
			
			SET @ReturnId = @SafeWebsiteId
			
		END
		ELSE
		BEGIN
			UPDATE [dbo].[SafeWebsite]
			   SET [deleted] = ISNULL(@deleted, [deleted])
			 WHERE [SafeWebsiteId] = @SafeWebsiteId
			 
			 SET @ReturnId = @SafeWebsiteId
		END
		
	END
	ELSE
	BEGIN
	
	INSERT INTO [dbo].[SafeWebsite]
           (SafeWebsiteUrl
           ,SafeWebsiteOpenIFrame
           ,SafeWebsiteGUID
           ,[register]
           )
     VALUES
           (@SafeWebsiteUrl
           ,@SafeWebsiteOpenIFrame
           ,NEWID()
           ,GETDATE()
           )
           
		SET @ReturnId = @@IDENTITY
		
	END
END

