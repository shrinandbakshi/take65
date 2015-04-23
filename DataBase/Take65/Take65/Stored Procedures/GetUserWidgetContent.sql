CREATE PROCEDURE [dbo].[GetUserWidgetContent]
	@UserWidgetId NUMERIC(18, 0),
	@TrustedSourceId NUMERIC(18, 0) = NULL,
	@CurrentPage INT = NULL,
	@ItemsPerPage INT = NULL,
	@HasThumb INT = NULL,
	@Search NVARCHAR(100) = NULL,
	@XmlReturn XML OUTPUT
AS

SET @XmlReturn =	
(
	SELECT TOP 100
		FC.FeedContentId as 'Id',
		FC.TrustedSourceFeedId,
		FC.TrustedSourceId,
		FC.TrustedSourceFeedCategory as 'Category',
		FC.TrustedSourceName,
		FC.TrustedSourceOpenIFrame as 'OpenIFrame',
		FC.FeedTitle as 'Title',
		FC.FeedLink as 'Link',
		FC.FeedDescription as 'Description',
		FC.FeedPubDate as 'PublishedDate',
		FC.FeedThumb as 'Thumb' 
	FROM [dbo].[FeedContent] FC
	LEFT OUTER JOIN [UserWidgetTrustedSourceFeed] UWTSF ON FC.[TrustedSourceFeedId] = UWTSF.[TrustedSourceFeedId]
	WHERE UWTSF.UserWidgetId = @UserWidgetId
	AND FC.deleted IS NULL
	AND (@TrustedSourceId IS NULL OR FC.TrustedSourceId = @TrustedSourceId)
	AND (@HasThumb IS NULL OR FC.FeedThumb IS NOT NULL)
	AND (@Search IS NULL OR (
							 FC.FeedTitle LIKE '%' + @Search + '%'
							 OR FC.FeedDescription LIKE  '%' + @Search + '%'
							 )
		)

	ORDER BY FC.[FeedPubDate] DESC
	
	FOR XML PATH('FeedContent'), TYPE, ROOT('ArrayOfFeedContent')
)



/*

	--Pegar a categoria do widget do usuario (+1 query) OBS: Se o usuario pode selecionar N categorias, já nao funciona
	DECLARE @CategoryId INT
	SET  @CategoryId = (SELECT TOP 1 SystemTagId FROM UserWidget WHERE UserWidgetId = @UserWidgetId)

	SET @XmlReturn =	
		(		
				SELECT        TOP (100)
					FC.FeedContentId as 'Id',
					FC.TrustedSourceFeedId a,
					FC.FeedTitle as 'Title',
					FC.FeedLink as 'Link',
					FC.FeedDescription as 'Description',
					FC.FeedPubDate as 'PublishedDate',
					FC.FeedThumb as 'Thumb', 
					TS.TrustedSourceName as 'TrustedSourceName',
					FCT.FeedContentTagNormalized as 'CategoryName'
				FROM FeedContent FC
				INNER JOIN FeedContentTag FCT 
					ON FC.FeedContentId = FCT.FeedContentId --Inner join pra pegar só conteudo que tem a categoria que o usuario selecionou (+1 query)
				INNER JOIN TrustedSourceFeed TSF 
					ON FC.TrustedSourceFeedId = TSF.TrustedSourceFeedId --Inner join pra relacionar os feeds dos TrustedSourceFeed dos Trusted Source que o usuario escolheu (+1 query)
				INNER JOIN TrustedSource TS
					ON TS.TrustedSourceId = TSF.TrustedSourceId
					
				WHERE FCT.tagtypeid = 4 and FCT.Systemtagid = @CategoryId
				AND TSF.TrustedSourceId IN (select trustedsourceid from userwidgettrustedsource where userwidgetid = @UserWidgetId) --sub query pra pegar os trusteds sources que o usuario escolheu (+1 query)
				AND (@HasThumb IS NULL OR FC.FeedThumb IS NOT NULL)
				AND (@TrustedSourceId IS NULL OR TSF.TrustedSourceId = @TrustedSourceId)
				ORDER BY FC.FeedPubDate	DESC		

				--bom, como visto contei inner join como query, o que é bem proximo, apesar de nas minhas pesquisas
				--indicar que inner join é (um pouco?) mais performatico que subquery
				--deu 4 queries/relacionamento
				--e ainda ta pegando só 100 e fazendo paginação no back, o que eu acho q é mais performatico do que fazer tudo isso e ainda paginar no banco
			  
			  FOR XML PATH('FeedContent'), TYPE, ROOT('ArrayOfFeedContent')
			)


			*/



