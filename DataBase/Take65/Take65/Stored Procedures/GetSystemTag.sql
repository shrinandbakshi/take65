CREATE PROCEDURE [dbo].[GetSystemTag]
	@SystemTagId NUMERIC(18,0) = NULL,
	@SystemTagNormalized NVARCHAR(50) = NULL,
	@SystemTagParentId NUMERIC(18,0) = NULL,
	@TagTypeId NUMERIC(18,0) = NULL,
	@TagTypeParentId NUMERIC(18,0) = NULL,
	@HasNullParent BIT = NULL,
	@XmlReturn XML OUTPUT
AS

BEGIN

	IF	@SystemTagId IS NOT NULL OR @SystemTagNormalized IS NOT NULL		
	 
	BEGIN
		SET @XmlReturn = (SELECT TOP 1 [SystemTagId] AS 'Id'
								  ,[SystemTagParentId] AS 'ParentId'
								  ,[SystemTagNormalized] AS 'Normalized'
								  ,[SystemTagDisplay] AS 'Display'
								  ,[SystemTagIcon] AS 'Icon'
								  ,[TagTypeId] AS 'TagTypeId'
							  FROM [dbo].[SystemTag]
								WHERE ( @SystemTagId IS  NULL OR [SystemTagId] = @SystemTagId )
								AND  ( @SystemTagNormalized IS  NULL OR [SystemTagNormalized] = @SystemTagNormalized )
								AND [inactive] IS NULL AND [deleted] IS NULL
								FOR XML PATH('SystemTag'), TYPE)
						
	END
	ELSE
	BEGIN
		SET @XmlReturn = (SELECT TotalResults = (
							
							(SELECT ST.[SystemTagId] AS 'Id'
								  ,[SystemTagParentId] AS 'ParentId'
								  ,[SystemTagNormalized] AS 'Normalized'
								  ,[SystemTagDisplay] AS 'Display'
								  ,[SystemTagIcon] AS 'Icon'
								  ,ST.[TagTypeId] AS 'TagTypeId'
							  FROM [dbo].[SystemTag] ST
								WHERE 

								(@SystemTagParentId IS NULL OR (ST.[SystemTagId] IN (SELECT [SystemTagId] FROM [SystemTag] WHERE [SystemTagParentId] = @SystemTagParentId)))

								AND (@TagTypeId IS NULL OR ST.[TagTypeId] = @TagTypeId)
								AND (@HasNullParent IS NULL OR  ST.[SystemTagParentId] IS NULL)
								AND (@TagTypeParentId IS NULL OR (ST.[TagTypeId] IN (SELECT [TagTypeId] FROM [TagType] WHERE [TagTypeParentId] = @TagTypeParentId)))
								
								AND ST.[inactive] IS NULL AND ST.[deleted] IS NULL
								ORDER BY ISNULL([SystemTagOrder],9999) ASC
								FOR XML PATH('SystemTag'), TYPE, ROOT('SystemTagList'))
							))
	END
END	
	
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON