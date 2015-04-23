CREATE TABLE [dbo].[TrustedSourceFeed] (
    [TrustedSourceFeedId]   NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [TrustedSourceId]       NUMERIC (18)   NOT NULL,
    [TrustedSourceFeedName] NVARCHAR (250) NULL,
    [TrustedSourceFeedUrl]  NVARCHAR (250) NOT NULL,
    [lastsync]              DATETIME       NULL,
    [register]              DATETIME       CONSTRAINT [DF_TrustedSourceFeed_register] DEFAULT (getdate()) NULL,
    [lastupdate]            DATETIME       NULL,
    [inactive]              DATETIME       NULL,
    [deleted]               DATETIME       NULL,
    CONSTRAINT [PK_TrustedSourceFeed] PRIMARY KEY CLUSTERED ([TrustedSourceFeedId] ASC),
    CONSTRAINT [FK_TrustedSourceFeed_TrustedSource] FOREIGN KEY ([TrustedSourceId]) REFERENCES [dbo].[TrustedSource] ([TrustedSourceId])
);

