CREATE FUNCTION FN_ObterCategoriasFilhas(@ID_CategoriaConteudo INT)
RETURNS @T TABLE (ID_CategoriaConteudo INT, Nivel INT)

AS

BEGIN

	DECLARE @Nivel INT;

	SET @Nivel = (SELECT COUNT(*) FROM FN_ObterHierarquiaPai(@ID_CategoriaConteudo))

	-- Inserir a própria pai na lista de retorno.
	INSERT INTO @T (ID_CategoriaConteudo, Nivel)
		VALUES(@ID_CategoriaConteudo, @Nivel)

	-- Busca todas as categorias filhas imediatas da categoria da função, juntamente com o índice de cada uma.
	DECLARE @ID_CategoriaConteudoFilhas TABLE(ID_CategoriaConteudo INT, RowNumber INT);
	INSERT INTO @ID_CategoriaConteudoFilhas (ID_CategoriaConteudo, RowNumber)
		SELECT ID_CategoriaConteudo, ROW_NUMBER() OVER (ORDER BY NM_CategoriaConteudo) FROM TB_CategoriaConteudo WHERE ID_CategoriaConteudoPai = @ID_CategoriaConteudo ORDER BY NM_CategoriaConteudo OPTION (MAXRECURSION 0);

	DECLARE @IDS TABLE(ID INT, RB INT);

	DECLARE @CT INT; SET @CT = 1;

	-- Cicla pelas categorias filhas e chama a recursividade.
	WHILE(@CT <= (SELECT COUNT(ID_CategoriaConteudo) FROM @ID_CategoriaConteudoFilhas))
	BEGIN
		INSERT INTO @T (ID_CategoriaConteudo, Nivel)
			SELECT ID_CategoriaConteudo, (SELECT COUNT(*) FROM FN_ObterHierarquiaPai(F2.ID_CategoriaConteudo)) as Nivel FROM FN_ObterCategoriasFilhas((SELECT TOP 1 F.ID_CategoriaConteudo FROM @ID_CategoriaConteudoFilhas F WHERE F.RowNumber = @CT)) F2 OPTION (MAXRECURSION 0);

		SET @CT = @CT + 1;
	END

	RETURN
END