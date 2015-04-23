CREATE FUNCTION [dbo].[NormalizeChar]
(@input NVARCHAR (4000))
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [Functions].[UserDefinedFunctions].[NormalizeChar]

