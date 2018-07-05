-----------------------------------------------------------
-- Remocao dos resitros que possuem a Forma de Aquisicao: Comunidade Virtual conforme Task #569 Item 5.
-- Data: 26/07/2016
-- Autor: Jair Foro
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[DASHBOARD_REL_CONCLUINTES_EMPREGADOS] 
 @DataInicio AS DATE,
 @DataFim AS DATE,
 @IdUf INT = NULL AS
BEGIN

	-- Recupera o ID da Forma de Aquisicao Comunidade Virtual
	DECLARE @ID_Comunidade_Virtual INT = (SELECT TOP 1 VL_REGISTRO FROM TB_ConfiguracaoSistema WHERE ID_ConfiguracaoSistema = 39);

	SET NOCOUNT ON;

	DECLARE @NiveisInternos TABLE (ID_NivelOcupacional INT) INSERT INTO @NiveisInternos SELECT ID_NivelOcupacional FROM dbo.FN_ObterNiveisOcupacionaisInternos ()
	DECLARE @StatusAprovado TABLE (ID_StatusMatricula INT) INSERT INTO @StatusAprovado SELECT ID_StatusMatricula FROM FN_StatusAprovado()

	SELECT
		ISNULL(MIN(N.NM_Nome), '') as NomeNivelOcupacional,
		COUNT (DISTINCT MO.ID_Usuario) AS QTDConcluintes
	FROM TB_MatriculaTurma MT
		JOIN TB_MatriculaOferta MO ON MT.ID_MatriculaOferta = MO.ID_MatriculaOferta
		JOIN TB_Oferta O ON O.ID_Oferta = MO.ID_Oferta
		JOIN TB_SolucaoEducacional SE ON SE.ID_SolucaoEducacional = O.ID_SolucaoEducacional AND SE.ID_FormaAquisicao <> @ID_Comunidade_Virtual
		JOIN TB_CategoriaConteudo CA ON SE.ID_CategoriaConteudo = CA.ID_CategoriaConteudo
		JOIN TB_NivelOcupacional N ON MO.ID_NivelOcupacional = N.ID_NivelOcupacional

	WHERE
		MO.ID_NivelOcupacional IN (SELECT ID_NivelOcupacional FROM @NiveisInternos)
		AND (@IdUf IS NULL OR MO.ID_UF = @IdUf)
		AND CAST (MT.DT_Termino AS DATE) BETWEEN @DataInicio AND @DataFim
		AND MO.ID_StatusMatricula IN (SELECT ID_StatusMatricula FROM @StatusAprovado)
		-- Excluir categorias de Comunidade
		AND (CA.IN_Comunidade IS NULL OR CA.IN_Comunidade = 0)

	GROUP BY N.ID_NivelOcupacional

	ORDER BY 1
END