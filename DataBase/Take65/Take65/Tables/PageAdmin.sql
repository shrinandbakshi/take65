CREATE TABLE [dbo].[PageAdmin]
(
	[PageAdminId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PageAdminName] NVARCHAR(255) NOT NULL, 
    [PageAdminLink] NVARCHAR(MAX) NOT NULL, 
    [PageAdminOrder] INT NOT NULL, 
    [PageParentId] INT NULL, 
    [Inactive] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_PageAdmin_PageAdminId] FOREIGN KEY ([PageParentId]) REFERENCES [PageAdmin]([PageAdminId])
)