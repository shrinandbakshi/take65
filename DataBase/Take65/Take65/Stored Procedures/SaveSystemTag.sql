CREATE PROCEDURE [dbo].[SaveSystemTag]
	@Id DECIMAL(18,0)	= NULL,
	@ParentId  DECIMAL(18,0)	= NULL,
	@ParentIdList DECIMAL(18,0)	= NULL,
	@Normalized NVARCHAR(250)	= NULL,
	@Display NVARCHAR(250)	= NULL,
	@Icon NVARCHAR(250)	= NULL,
	@Order DECIMAL(18,0)	= NULL,
	@TagTypeId DECIMAL(18,0)	= NULL,
	@register DATETIME = NULL,
	@lastupdate DATETIME = NULL,
	@inactive DATETIME = NULL,
	@deleted DATETIME = NULL,

	@ReturnId NUMERIC(18 ,0) OUTPUT
AS

BEGIN
	IF @Id IS NOT NULL
	BEGIN

		UPDATE [dbo].[SystemTag]
		   SET 
			  [SystemTagParentId] = ISNULL(@ParentId,[SystemTagParentId])
			  ,[SystemTagParentIdList] = ISNULL(@ParentIdList,[SystemTagParentIdList])
			  ,[SystemTagNormalized] = ISNULL(@Normalized,[SystemTagNormalized])
			  ,[SystemTagDisplay] = ISNULL(@Display,[SystemTagDisplay])
			  ,[SystemTagIcon] = ISNULL(@Icon,[SystemTagIcon])
			  ,[SystemTagOrder] = ISNULL(@Order,[SystemTagOrder])
			  ,[TagTypeId] = ISNULL(@TagTypeId,[TagTypeId])
			  ,[register] = ISNULL(@register,[register])
			  ,[lastupdate] = GETDATE()
			  ,[inactive] = ISNULL(@inactive,[inactive])
			  ,[deleted] = ISNULL(@deleted,[deleted])
		 WHERE [SystemTagId] = @Id

		SET @ReturnId = @Id
	
	END
	ELSE
	BEGIN

		INSERT INTO [dbo].[SystemTag]
           (
           [SystemTagParentId]
		   ,[SystemTagParentIdList]
           ,[SystemTagNormalized]
           ,[SystemTagDisplay]
           ,[SystemTagIcon]
           ,[SystemTagOrder]
           ,[TagTypeId]
           ,[register]
           ,[lastupdate]
           ,[inactive]
           ,[deleted]
           )
     VALUES
           (
           @ParentId
		   ,@ParentIdList
           ,@Normalized
           ,@Display
           ,@Icon
           ,@Order
           ,@TagTypeId
           ,@register
           ,GETDATE()
           ,@inactive
           ,@deleted
           )

		SET @ReturnId = @@IDENTITY	
	END
END