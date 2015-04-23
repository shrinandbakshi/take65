CREATE PROCEDURE [dbo].[SaveFile]
	@FileId int = NULL,
	@FileName nvarchar(255) = NULL,
	@FileDescription nvarchar(MAX) = NULL,
	@FileType nvarchar(255) = NULL,
	@FileLink nvarchar(255) = NULL,
	@Deleted datetime,
	@ReturnId NUMERIC(18 ,0) OUTPUT
AS

BEGIN
	
	IF @FileId IS NOT NULL
		BEGIN
			
			UPDATE [dbo].[File]
			   SET [FileName] = ISNULL(@FileName, [FileName])
				  ,[FileDescription] = ISNULL(@FileDescription, [FileDescription])
				  ,[FileType] = ISNULL(@FileType, [FileType])
				  ,[FileLink] = ISNULL(@FileLink, [FileLink])
				  ,[Deleted] = ISNULL(@Deleted, [Deleted])
			 WHERE [FileId] = @FileId
						
			SET @ReturnId = @FileId

		END
	ELSE
		BEGIN
		
			INSERT INTO [dbo].[File]
					   ([FileName]
					   ,[FileDescription]
					   ,[FileType]
					   ,[FileLink]
					   ,[Deleted])
				 VALUES
					   (@FileName
					   ,@FileDescription
					   ,@FileType
					   ,@FileLink
					   ,@Deleted)
			   
			SET @ReturnId = @@IDENTITY
		END	
END