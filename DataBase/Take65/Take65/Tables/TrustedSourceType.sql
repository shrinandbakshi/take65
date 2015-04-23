CREATE TABLE [dbo].[TrustedSourceType] (
    [TrustedSourceTypeId]   NUMERIC (18)   NOT NULL,
    [TrustedSourceTypeName] NVARCHAR (120) NOT NULL,
    [register]              DATETIME       CONSTRAINT [DF_TrustedSourceType_register] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_TrustedSourceType] PRIMARY KEY CLUSTERED ([TrustedSourceTypeId] ASC)
);

