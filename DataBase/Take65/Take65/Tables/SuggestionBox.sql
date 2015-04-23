CREATE TABLE [dbo].[SuggestionBox]
(
	[SuggestionId] NUMERIC IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [SuggestionName] NVARCHAR(255) NULL, 
    [SuggestionUrl] NVARCHAR(255) NOT NULL, 
    [SuggestionDescription] NVARCHAR(MAX) NULL, 
    [SuggestionImage] NVARCHAR(255) NULL, 
    [register] DATETIME NULL DEFAULT getdate(), 
	[lastupdate] DATETIME NULL, 
    [deleted] DATETIME NULL
)
