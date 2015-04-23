CREATE TABLE [dbo].[UserEmailAccount](
	[UserEmailAccountId] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [numeric](18, 0) NOT NULL,
	[SystemTagId] [numeric](18, 0) NOT NULL,
	[UserEmailAccountUsername] [nvarchar](255) NOT NULL,
	[UserEmailAccountPassword] [nvarchar](255) NOT NULL,
	[register] [datetime] NOT NULL,
	[lastupdate] [datetime] NULL,
	[deleted] [datetime] NULL,
 CONSTRAINT [PK_UserEmailAccount] PRIMARY KEY CLUSTERED 
(
	[UserEmailAccountId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserEmailAccount] ADD  CONSTRAINT [DF_UserEmailAccount_register]  DEFAULT (getdate()) FOR [register]
GO

