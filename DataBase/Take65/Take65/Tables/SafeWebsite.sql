CREATE TABLE [dbo].[SafeWebsite]
(
	[SafeWebsiteId] NUMERIC NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [SafeWebsiteUrl] NVARCHAR(255) NOT NULL, 
	[TrustedSourceOpenIFrame] BIT NULL,
    [SafeWebsiteGUID] UNIQUEIDENTIFIER NULL DEFAULT newid(), 
    [register] DATETIME NULL DEFAULT getdate(), 
    [lastupdate] DATETIME NULL, 
    [deleted] DATETIME NULL
)
