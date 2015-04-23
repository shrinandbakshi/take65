CREATE FUNCTION GetDomainName (@URL VARCHAR(255))
	RETURNS NVARCHAR(255)
AS
BEGIN
	SET @URL = REPLACE(REPLACE(@URL, 'http://', ''), 'https://', '')
	--DECLARE @URL nvarchar(max) = @Url
	DECLARE @X xml = CONVERT(xml,'<root><part>' + REPLACE(@URL, '.','</part><part>') + '</part></root>')

	RETURN (SELECT [Domain] = T.c.value('.','varchar(20)')
	FROM @X.nodes('/root/part[position() = last() - 1]') T(c))
	
END