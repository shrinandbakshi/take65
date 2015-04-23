CREATE TABLE [dbo].[UserWidgetTrustedSourceFeed] (
    [UserWidgetTrustedSourceFeedId] DECIMAL (18) IDENTITY (1, 1) NOT NULL,
    [UserWidgetId]                  DECIMAL (18) NULL,
    [TrustedSourceFeedId]           DECIMAL (18) NULL,
    CONSTRAINT [PK_UserWidgetTrustedSourceFeed] PRIMARY KEY CLUSTERED ([UserWidgetTrustedSourceFeedId] ASC)
);

