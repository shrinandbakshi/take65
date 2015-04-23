CREATE TABLE [dbo].[User] (
    [UserId]                NUMERIC (18)     IDENTITY (1, 1) NOT NULL,
    [UserGUID]              UNIQUEIDENTIFIER CONSTRAINT [DF_User_UserGUID] DEFAULT (newid()) NULL,
    [UserFacebookId]        NVARCHAR (50)    NULL,
	[UserFacebookTokenShort]		NVARCHAR (255)	 NULL,
	[UserFacebookTokenLong]		NVARCHAR (255)	 NULL,
	[UserFacebookTokenLongExpires]		BIGINT	 NULL,
    [UserName]              NVARCHAR (250)   NULL,
    [UserEmail]             NVARCHAR (250)   NULL,
    [UserLogin]             NVARCHAR (250)   NULL,
    [UserPassword]          NVARCHAR (50)    NULL,
	[UserChangePasswordNextLogin] DATETIME	 NULL,
    [UserBirthdate]         DATETIME         NULL,
    [UserGender]            CHAR (1)         NULL,
    [UserAddressState]      NVARCHAR (50)    NULL,
    [UserAddressCity]       NVARCHAR (50)    NULL,
    [UserAddressPostalCode] NVARCHAR (20)    NULL,
    [register]              DATETIME         CONSTRAINT [DF_User_register] DEFAULT (getdate()) NULL,
    [lastupdate]            DATETIME         NULL,
    [inactive]              DATETIME         NULL,
    [deleted]               DATETIME         NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

