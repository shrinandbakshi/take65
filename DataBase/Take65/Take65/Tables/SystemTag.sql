CREATE TABLE [dbo].[SystemTag] (
    [SystemTagId]           NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SystemTagParentId]     NUMERIC (18)   NULL,
    [SystemTagParentIdList] NVARCHAR (MAX) NULL,
    [SystemTagNormalized]   VARCHAR (50)   NOT NULL,
    [SystemTagDisplay]      NVARCHAR (50)  NULL,
    [SystemTagIcon]         NVARCHAR (500) NULL,
    [SystemTagOrder]        INT            NULL,
    [TagTypeId]             INT            NULL,
    [register]              DATETIME       CONSTRAINT [DF_SystemTag_register] DEFAULT (getdate()) NOT NULL,
    [lastupdate]            DATETIME       NULL,
    [inactive]              DATETIME       NULL,
    [deleted]               DATETIME       NULL,
    CONSTRAINT [PK_SystemTag] PRIMARY KEY CLUSTERED ([SystemTagId] ASC),
    CONSTRAINT [FK_SystemTag_SystemTag] FOREIGN KEY ([SystemTagParentId]) REFERENCES [dbo].[SystemTag] ([SystemTagId]),
    CONSTRAINT [FK_SystemTag_TagType] FOREIGN KEY ([TagTypeId]) REFERENCES [dbo].[TagType] ([TagTypeId])
);

