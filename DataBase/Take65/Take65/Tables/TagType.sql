CREATE TABLE [dbo].[TagType] (
    [TagTypeId]       INT            IDENTITY (1, 1) NOT NULL,
    [TagTypeParentId] INT            NULL,
    [TagTypeName]     VARCHAR (50)   NULL,
    [TagTypeDisplay]  NVARCHAR (120) NULL,
    [register]        DATETIME       CONSTRAINT [DF_TagType_register] DEFAULT (getdate()) NOT NULL,
    [lastupdate]      DATETIME       NULL,
    [inactive]        DATETIME       NULL,
    [deleted]         DATETIME       NULL,
    CONSTRAINT [PK_TagType] PRIMARY KEY CLUSTERED ([TagTypeId] ASC)
);

