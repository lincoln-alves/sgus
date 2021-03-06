-----------------------------------------------------------
-- Remocao dos resitros que possuem a Forma de Aquisicao: Comunidade Virtual conforme Task #569 Item 5.
-- Data: 26/07/2016
-- Autor: Jair Foro
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[DASHBOARD_REL_IndiceSatisfacaoCredenciados] 
 @pDataInicio dateTime,
 @pDataFim dateTime,
 @IdUf INT = NULL AS
BEGIN

-- Recupera o ID da Forma de Aquisicao Comunidade Virtual
DECLARE @ID_Comunidade_Virtual INT = (SELECT TOP 1 VL_REGISTRO FROM TB_ConfiguracaoSistema WHERE ID_ConfiguracaoSistema = 39);

SET NOCOUNT ON ; -- Buscar as categorias de Formação de Formadores de acordo com o ID informado na TB_ConfiguracaoSistema
DECLARE @IdsForacaoMultiplicadores TABLE (ID_CategoriaConteudo INT) INSERT INTO @IdsForacaoMultiplicadores SELECT
	ID_CategoriaConteudo
FROM
	FN_ObterCategoriasFilhas (
		(
			SELECT
				TOP 1 CAST (VL_Registro AS INT)
			FROM
				TB_ConfiguracaoSistema
			WHERE
				ID_ConfiguracaoSistema = 33
		)
	) SELECT
		CONVERT (
			DECIMAL (10, 2),
			AVG (
				CAST (
					dbo.fnRemoveNonNumericCharacters (IQPO.NM_Opcao) AS DECIMAL
				)
			)
		) AS AvaliacaoMedia
	FROM
		TB_QuestionarioParticipacao QP
	JOIN TB_Questionario Q ON QP.ID_Questionario = Q.ID_Questionario
	JOIN TB_ItemQuestionarioParticipacao IQP ON QP.ID_QuestionarioParticipacao = IQP.ID_QuestionarioParticipacao
	JOIN TB_ItemQuestionarioParticipacaoOpcoes IQPO ON IQP.ID_ItemQuestionarioParticipacao = IQPO.ID_ItemQuestionarioParticipacao	
	JOIN TB_Turma T ON QP.ID_Turma = T.ID_Turma
	JOIN TB_Oferta O ON T.ID_Oferta = O.ID_Oferta
	JOIN TB_SolucaoEducacional S ON O.ID_SolucaoEducacional = S.ID_SolucaoEducacional AND S.ID_FormaAquisicao <> @ID_Comunidade_Virtual
	JOIN TB_Usuario U ON U.ID_Usuario = QP.ID_Usuario
	LEFT JOIN TB_UF UF ON QP.ID_UF = UF.ID_UF
	WHERE
		Q.ID_TipoQuestionario = 2 -- Somente pesquisas
	AND IQPO.IN_RespostaSelecionada = 1
	AND IQPO.NM_Opcao NOT LIKE '%NA - Não se Aplica%' -- Remover opção de "Não se aplica" da média.
	AND S.ID_CategoriaConteudo IN (
		SELECT
			ID_CategoriaConteudo
		FROM
			@IdsForacaoMultiplicadores
	) -- Somente Formação de Multiplicadores
	AND QP.DT_Participacao IS NOT NULL -- Somente questionários respondidos
	AND (
		CAST (T.DT_Inicio AS DATE) BETWEEN @pDataInicio
		AND @pDataFim
	)
	AND (
		@IdUf IS NULL
		OR QP.ID_UF = @IdUf
	)
END