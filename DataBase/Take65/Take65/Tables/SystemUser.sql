CREATE TABLE [dbo].[SystemUser] (
    [UserId]         DECIMAL (18)     IDENTITY (1, 1) NOT NULL,
    [UserName]       NVARCHAR (250)   NULL,
    [UserFirstName]  NVARCHAR (150)   NULL,
    [UserLastName]   NVARCHAR (250)   NULL,
    [UserEmail]      NVARCHAR (250)   NULL,
    [UserPassword]   VARCHAR (50)     NULL,
    [UserLastLogin]  SMALLDATETIME    NULL,
    [UserGUID]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [ForgotPassword] UNIQUEIDENTIFIER NULL,
    [register]       DATETIME         DEFAULT (getdate()) NOT NULL,
    [lastupdate]     DATETIME         NULL,
    [inactive]       DATETIME         NULL,
    [deleted]        DATETIME         NULL,
    CONSTRAINT [PK_SystemUser] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

