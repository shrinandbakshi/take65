CREATE PROCEDURE [dbo].[SaveTrustedSourceBookmark]
	@TrustedSourceId NUMERIC(18,0)	= NULL,
	@TrustedSourceName NVARCHAR(255),
	@TrustedSourceUrl NVARCHAR(250),
	@SystemTagId NUMERIC(18, 0),
	@TrustedSourceIcon NVARCHAR(255) = NULL,
	@TrustedSourceOpenIFrame BIT,
	@deleted DATETIME = NULL
	
	,@ReturnId NUMERIC(18 ,0) OUTPUT
AS

BEGIN
	IF @TrustedSourceId IS NOT NULL
	BEGIN
	
		IF (@deleted IS NULL)
		BEGIN
			UPDATE [dbo].[TrustedSource]
			   SET [TrustedSourceName] = ISNULL(@TrustedSourceName, [TrustedSourceName])
				  ,[TrustedSourceIcon] = ISNULL(@TrustedSourceIcon, [TrustedSourceIcon])
				  ,[TrustedSourceUrl] = ISNULL(@TrustedSourceUrl, [TrustedSourceUrl])
				  ,[TrustedSourceOpenIFrame] = ISNULL(@TrustedSourceOpenIFrame, [TrustedSourceOpenIFrame])
				  ,[lastupdate] = GETDATE()
				  ,[deleted] = ISNULL(@deleted, [deleted])
			 WHERE [TrustedSourceId] = @TrustedSourceId
			
			 SET @ReturnId = @TrustedSourceId
			  
			 DELETE TrustedSourceTag WHERE [TrustedSourceId] = @TrustedSourceId
			 
			 INSERT INTO TrustedSourceTag (TrustedSourceId, SystemTagId) VALUES (@TrustedSourceId, @SystemTagId)
			
		END
		ELSE
		BEGIN
			UPDATE [dbo].[TrustedSource]
			   SET [deleted] = ISNULL(@deleted, [deleted])
			 WHERE [TrustedSourceId] = @TrustedSourceId
			 
			 SET @ReturnId = @TrustedSourceId
		END
	
	END
	ELSE
	BEGIN
	
	INSERT INTO [dbo].[TrustedSource]
           ([TrustedSourceName]
           ,[TrustedSourceIcon]
           ,[TrustedSourceUrl]
           ,[TrustedSourceOpenIFrame]
           ,[TrustedSourceTypeId]
           ,[register]
           )
     VALUES
           (@TrustedSourceName
           ,@TrustedSourceIcon
           ,@TrustedSourceUrl
           ,@TrustedSourceOpenIFrame
           ,2
           ,GETDATE()
           )
           
		SET @ReturnId = @@IDENTITY
		
		INSERT INTO TrustedSourceTag (TrustedSourceId, SystemTagId) VALUES (@ReturnId, @SystemTagId)
		
	END
END