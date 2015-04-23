CREATE TABLE [dbo].[FeedContentImage] (
    [FeedContentImageId]    NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [FeedContentId]         NUMERIC (18)   NULL,
    [FeedContentImageUrl]   NVARCHAR (250) NOT NULL,
    [FeedContentImageTitle] NVARCHAR (25)  NULL,
    [register]              DATETIME       CONSTRAINT [DF_FeedContentImage_register] DEFAULT (getdate()) NULL,
    [inactive]              DATETIME       NULL,
    [deleted]               DATETIME       NULL,
    CONSTRAINT [PK_FeedContentImage] PRIMARY KEY CLUSTERED ([FeedContentImageId] ASC)
);

