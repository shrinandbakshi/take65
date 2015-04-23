
CREATE PROCEDURE [dbo].[SaveUserWidget]
	@Id DECIMAL(18,0)	= NULL,
	@UserId DECIMAL(18,0) = NULL,
	@Name NVARCHAR(250) = NULL,
	@Size INT = NULL,
	@Order INT = NULL,
	@SystemTagId DECIMAL(18,0) = NULL,
	@DefaultWidget BIT = NULL,
	@deleted DATETIME = NULL,

	@ReturnId NUMERIC(18 ,0) OUTPUT
AS

BEGIN
	IF @Id IS NOT NULL
	BEGIN

		UPDATE [dbo].[UserWidget]
		SET 
			
			[UserWidgetName] = ISNULL(@Name,[UserWidgetName])
			,[lastupdate] = GETDATE()
			,[UserWidgetOrder] = ISNULL(@Order,[UserWidgetOrder])
			,[deleted] = ISNULL(@deleted,[deleted])
		WHERE [UserWidgetId] = @Id

		SET @ReturnId = @Id
	
	END
	ELSE
	BEGIN

		INSERT INTO [dbo].[UserWidget]
			([UserId]
			,[UserWidgetName]
			,[SystemTagId]
			,[UserWidgetSize]
			,[UserWidgetOrder]
			,[UserWidgetDefault]
			,[register]
			)
		VALUES
			(@UserId
			,@Name
			,@SystemTagId
			,@Size
			,@Order
			,@DefaultWidget
			,GETDATE()
			)

		SET @ReturnId = @@IDENTITY
		
	END
END


