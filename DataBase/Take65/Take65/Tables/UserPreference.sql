CREATE TABLE [dbo].[UserPreference] (
    [UserId]      NUMERIC (18) NOT NULL,
    [SystemTagId] NUMERIC (18) NOT NULL,
    [register]    DATETIME     CONSTRAINT [DF_UserPreference_register] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserPreference] PRIMARY KEY CLUSTERED ([UserId] ASC, [SystemTagId] ASC),
    CONSTRAINT [FK_UserPreference_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_UserPreference_UserPreference] FOREIGN KEY ([SystemTagId]) REFERENCES [dbo].[SystemTag] ([SystemTagId])
);

