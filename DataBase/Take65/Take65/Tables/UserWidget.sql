CREATE TABLE [dbo].[UserWidget] (
    [UserWidgetId]    NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [UserId]          NUMERIC (18)   NOT NULL,
    [SystemTagId]     NUMERIC (18)   NOT NULL,
    [UserWidgetName]  NVARCHAR (250) NULL,
	[UserWidgetExtraInfoXML] NVARCHAR(MAX) NULL,
    [UserWidgetSize]  INT            NULL,
    [UserWidgetOrder] INT            NULL,
	[UserWidgetDefault] BIT			 NULL,
    [register]        DATETIME       CONSTRAINT [DF_UserWidget_register] DEFAULT (getdate()) NOT NULL,
    [lastupdate]      DATETIME       NULL,
    [deleted]         DATETIME       NULL,
    CONSTRAINT [PK_UserWidget] PRIMARY KEY CLUSTERED ([UserWidgetId] ASC)
);

