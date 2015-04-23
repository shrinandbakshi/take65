CREATE TABLE [dbo].[File]
(
	[FileId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [FileName] NVARCHAR(512) NULL, 
    [FileDescription] NVARCHAR(MAX) NULL, 
    [FileType] NVARCHAR(50) NULL, 
    [FileLink] NVARCHAR(MAX) NULL, 
    [Deleted] DATETIME NULL
)
