
CREATE PROCEDURE [dbo].[SaveUserWidgetTrustedSource]
	@UserWidgetId DECIMAL(18,0)	= NULL,
	@TrustedSourceId DECIMAL(18,0) = NULL,
	@SystemTagId DECIMAL(18,0) = NULL,
	@Name NVARCHAR(250) = NULL,
	@Url NVARCHAR(250) = NULL
AS

BEGIN



INSERT INTO [dbo].[UserWidgetTrustedSource]
           ([UserWidgetId]
           ,[SystemTagId]
           ,[TrustedSourceId]
           ,[UserWidgetTrustedSourceName]
           ,[UserWidgetTrustedSourceUrl]
           ,[register])
     VALUES
           (@UserWidgetId
           ,@SystemTagId
           ,@TrustedSourceId
           ,@Name
           ,@Url
           ,getdate())



END

