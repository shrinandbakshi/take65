CREATE TABLE [dbo].[FeedContentTag] (
    [FeedContentTagId]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [FeedContentTagParentId]   NUMERIC (18)  NULL,
    [FeedContentId]            NUMERIC (18)  NULL,
    [FeedContentTagNormalized] VARCHAR (50)  NOT NULL,
    [FeedContentTagDisplay]    NVARCHAR (50) NULL,
    [TagTypeId]                INT           NULL,
    [SystemTagId]              NUMERIC (18)  NULL,
    [register]                 DATETIME      CONSTRAINT [DF_FeedContentTag_register] DEFAULT (getdate()) NOT NULL,
    [inactive]                 DATETIME      NULL,
    CONSTRAINT [PK_FeedContentTag] PRIMARY KEY CLUSTERED ([FeedContentTagId] ASC),
    CONSTRAINT [FK_FeedContentTag_FeedContentTag] FOREIGN KEY ([FeedContentTagParentId]) REFERENCES [dbo].[FeedContentTag] ([FeedContentTagId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FeedContentTage_FeedContentId]
    ON [dbo].[FeedContentTag]([FeedContentId] ASC);

