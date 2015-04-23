CREATE TABLE [dbo].[UserWidgetTag] (
    [UserWidgetTagId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UserId]          NUMERIC (18) NOT NULL,
    [UserWidgetId]    NUMERIC (18) NOT NULL,
    [SystemTagId]     NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_UserWidgetTag] PRIMARY KEY CLUSTERED ([UserWidgetTagId] ASC)
);

