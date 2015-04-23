CREATE TABLE [dbo].[TrustedSource] (
    [TrustedSourceId]         NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [TrustedSourceName]       NVARCHAR (250) NOT NULL,
    [TrustedSourceIcon]       NVARCHAR (120) NULL,
    [TrustedSourceTypeId]     NUMERIC (18)   NOT NULL,
    [TrustedSourceUrl]        NVARCHAR (250) NULL,
    [TrustedSourceOpenIFrame] BIT            NULL,
    [register]                DATETIME       CONSTRAINT [DF_TrustedSource_register] DEFAULT (getdate()) NULL,
    [inactive]                DATETIME       NULL,
    [lastupdate]              DATETIME       NULL,
    [deleted]                 DATETIME       NULL,
    CONSTRAINT [PK_TrustedSource] PRIMARY KEY CLUSTERED ([TrustedSourceId] ASC),
    CONSTRAINT [FK_TrustedSource_TrustedSourceType] FOREIGN KEY ([TrustedSourceTypeId]) REFERENCES [dbo].[TrustedSourceType] ([TrustedSourceTypeId])
);

