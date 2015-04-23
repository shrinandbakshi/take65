CREATE TABLE [dbo].[SuggestionBoxTag]
(
	[SuggestionBoxTagId] NUMERIC IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [SuggestionId] NUMERIC NOT NULL, 
    [SystemTagId] NUMERIC NOT NULL
)
