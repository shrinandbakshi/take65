CREATE FUNCTION [dbo].[RemoveHtml]
(@pInput NVARCHAR (4000))
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [Functions].[UserDefinedFunctions].[RemoveHtml]

