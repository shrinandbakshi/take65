CREATE TABLE [dbo].[UserFacebookFriend] (
    [UserFacebookFriendId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UserId]               NUMERIC (18) NOT NULL,
    [FacebookFriendId]     NUMERIC (18) NOT NULL,
	[FacebookFriendName]   NVARCHAR(255) NULL,
	[FacebookPhotoCount]	INT NULL,
    [register]             DATETIME     CONSTRAINT [DF_UserFacebookFriend_register] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserFacebookFriend] PRIMARY KEY CLUSTERED ([UserFacebookFriendId] ASC)
);

