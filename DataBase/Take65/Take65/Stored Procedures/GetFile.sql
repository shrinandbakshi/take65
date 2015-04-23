CREATE PROCEDURE [dbo].[GetFile]
(@FileId int = NULL,
 @FileName nvarchar(255) = NULL,
 @FileDescription nvarchar(MAX) = NULL,
 @FileType nvarchar(255) = NULL,
 @FileLink nvarchar(255) = NULL,
 @XmlReturn XML OUTPUT)
AS
BEGIN

	IF (@FileId IS NOT NULL)
	BEGIN
		SET @XmlReturn = (SELECT     [FileId]			   AS 'Id'
									,[FileName]			   AS 'Name'
									,[FileDescription]     AS 'Description'
									,[FileType]            AS 'Type'
									,[FileLink]            AS 'Link'
					  FROM [dbo].[File]
							WHERE (
								(@FileId IS NULL OR [FileId] = @FileId)
							AND [Deleted] IS NULL)
							ORDER BY [FileName]
							FOR XML PATH('File'))
	END
	ELSE
	BEGIN
		SET @XmlReturn = (SELECT     [FileId]			   AS 'Id'
									,[FileName]			   AS 'Name'
									,[FileDescription]     AS 'Description'
									,[FileType]            AS 'Type'
									,[FileLink]            AS 'Link'
					  FROM [dbo].[File]
							WHERE (
								(@FileName IS NULL OR [FileName]  = @FileName)
							AND (@FileDescription IS NULL OR [FileDescription]  = @FileDescription)
							AND (@FileType IS NULL OR [FileType]  = @FileType)
							AND (@FileLink IS NULL OR [FileLink]  = @FileLink)
							AND [deleted] IS NULL)
							ORDER BY [FileName]
							FOR XML PATH('File'), TYPE, ROOT('FileList'))
	END
END

