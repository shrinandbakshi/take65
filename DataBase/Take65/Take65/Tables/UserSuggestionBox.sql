CREATE TABLE [dbo].[UserSuggestionBox](
	[UserSuggestionBoxId] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [numeric](18, 0) NOT NULL,
	[SuggestionBoxId] [numeric](18, 0) NOT NULL,
	[UserSuggestionBoxAvoid] [bit] NOT NULL,
	[register] [datetime] NOT NULL,
	[deleted] [datetime] NULL,
 CONSTRAINT [PK_UserSuggestionBox] PRIMARY KEY CLUSTERED 
(
	[UserSuggestionBoxId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserSuggestionBox] ADD  CONSTRAINT [DF_UserSuggestionBox_UserSuggestionBoxAvoid]  DEFAULT ((1)) FOR [UserSuggestionBoxAvoid]
GO

ALTER TABLE [dbo].[UserSuggestionBox] ADD  CONSTRAINT [DF_UserSuggestionBox_register]  DEFAULT (getdate()) FOR [register]
GO

