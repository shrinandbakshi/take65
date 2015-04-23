CREATE PROCEDURE [dbo].[GetPageAdmin]
	@PageAdminLink nvarchar(MAX) = NULL,
	@XmlReturn XML OUTPUT
AS
BEGIN

If @PageAdminLink IS NULL
	SET @XmlReturn = (
		SELECT     
			 [PageAdminId] AS 'Id'
			,[PageAdminName] AS 'Name'
			,[PageAdminLink] AS 'Link'
			, (SELECT  [PageAdminId] AS 'Id'
					  ,[PageAdminName] AS 'Name'
			          ,[PageAdminLink] AS 'Link'
				FROM [dbo].[PageAdmin] AS Child
				WHERE Child.PageParentId = Main.PageAdminId
				ORDER BY [PageAdminOrder]
				FOR XML PATH('PageAdmin'), TYPE, ROOT('PageAdminList'))
			FROM [dbo].[PageAdmin] AS Main
			WHERE PageParentId IS NULL AND PageAdminName <> 'HomeAdmin'
			ORDER BY [PageAdminOrder]
		FOR XML PATH('PageAdmin'), TYPE, ROOT('PageAdminList'))		
	ELSE
	SET @XmlReturn = (
		SELECT     
				 [PageAdminId] AS 'Id'
				,[PageAdminName] AS 'Name'
				,[PageAdminLink] AS 'Link'
				FROM [dbo].[PageAdmin] AS Main
				WHERE [PageAdminId] = (SELECT PageParentId FROM  [dbo].[PageAdmin] WHERE [PageAdminLink] = @PageAdminLink)
				ORDER BY [PageAdminOrder]
			FOR XML PATH('PageAdmin'))

END