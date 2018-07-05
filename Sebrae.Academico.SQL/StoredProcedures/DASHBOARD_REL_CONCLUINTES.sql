-----------------------------------------------------------
-- Remocao dos resitros que possuem a Forma de Aquisicao: Comunidade Virtual conforme Task #569 Item 5.
-- Data: 26/07/2016
-- Autor: Jair Foro
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[DASHBOARD_REL_CONCLUINTES] 
 @DataInicio DATE = NULL,
 @DataFim DATE = NULL,
 @IdUf INT = NULL AS
BEGIN

-- Recupera o ID da Forma de Aquisicao Comunidade Virtual
DECLARE @ID_Comunidade_Virtual INT = (SELECT TOP 1 VL_REGISTRO FROM TB_ConfiguracaoSistema WHERE ID_ConfiguracaoSistema = 39);

-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON ; SELECT
	(
		SELECT
			COUNT (MT.ID_MatriculaTurma)
		FROM TB_MatriculaTurma MT
			INNER JOIN TB_Turma T ON MT.ID_Turma = T.ID_Turma
			INNER JOIN TB_Oferta O ON O.ID_Oferta = T.ID_Oferta
			INNER JOIN TB_MatriculaOferta MO ON MO.ID_MatriculaOferta = MT.ID_MatriculaOferta
			INNER JOIN TB_StatusMatricula SM ON SM.ID_StatusMatricula = MO.ID_StatusMatricula
			INNER JOIN TB_SolucaoEducacional SE ON SE.ID_SolucaoEducacional = O.ID_SolucaoEducacional AND SE.ID_FormaAquisicao <> @ID_Comunidade_Virtual
			INNER JOIN TB_CategoriaConteudo CC ON SE.ID_CategoriaConteudo = CC.ID_CategoriaConteudo
			INNER JOIN TB_Fornecedor FO ON FO.ID_Fornecedor = SE.ID_Fornecedor
			INNER JOIN TB_FormaAquisicao FA ON FA.ID_FormaAquisicao = SE.ID_FormaAquisicao
			INNER JOIN TB_Usuario U ON U.ID_Usuario = MO.ID_Usuario
			INNER JOIN TB_UF UF ON UF.ID_UF = MO.ID_UF
			INNER JOIN TB_NivelOcupacional NO ON NO .ID_NivelOcupacional = MO.ID_NivelOcupacional
		WHERE
			-- Somente Empregados
			NO .ID_NivelOcupacional IN (SELECT ID_NivelOcupacional FROM dbo.FN_ObterNiveisOcupacionaisInternos ())
			-- Somente aprovados
			AND MO.ID_StatusMatricula IN (SELECT ID_StatusMatricula FROM FN_StatusAprovado ())
			-- Somente no ano informado
			AND CAST (MT.DT_Termino AS DATE) BETWEEN @DataInicio AND @DataFim
			-- Excluir categorias de Comunidade
			AND (CC.IN_Comunidade IS NULL OR CC.IN_Comunidade = 0)
			AND (@IdUf IS NULL OR MO.ID_UF = @IdUf)
	) AS Internos,
	(
		SELECT
			COUNT (MT.ID_MatriculaTurma)
		FROM TB_MatriculaTurma MT
			INNER JOIN TB_Turma T ON MT.ID_Turma = T.ID_Turma
			INNER JOIN TB_Oferta O ON O.ID_Oferta = T.ID_Oferta
			INNER JOIN TB_MatriculaOferta MO ON MO.ID_MatriculaOferta = MT.ID_MatriculaOferta
			INNER JOIN TB_StatusMatricula SM ON SM.ID_StatusMatricula = MO.ID_StatusMatricula
			INNER JOIN TB_SolucaoEducacional SE ON SE.ID_SolucaoEducacional = O.ID_SolucaoEducacional
			AND SE.ID_FormaAquisicao <> @ID_Comunidade_Virtual
			INNER JOIN TB_CategoriaConteudo CC ON SE.ID_CategoriaConteudo = CC.ID_CategoriaConteudo
			INNER JOIN TB_Fornecedor FO ON FO.ID_Fornecedor = SE.ID_Fornecedor
			INNER JOIN TB_FormaAquisicao FA ON FA.ID_FormaAquisicao = SE.ID_FormaAquisicao
			INNER JOIN TB_Usuario U ON U.ID_Usuario = MO.ID_Usuario
			INNER JOIN TB_UF UF ON UF.ID_UF = MO.ID_UF
			INNER JOIN TB_NivelOcupacional NO ON NO .ID_NivelOcupacional = MO.ID_NivelOcupacional
		WHERE
			-- Somente colaboradores externos
			NO .ID_NivelOcupacional IN (SELECT ID_NivelOcupacional FROM dbo.FN_ObterNiveisOcupacionaisExternos ())
			-- Somente aprovados
			AND MO.ID_StatusMatricula IN (SELECT ID_StatusMatricula FROM FN_StatusAprovado ())
			-- Somente no ano informado
			AND (CAST (MT.DT_Termino AS DATE) BETWEEN @DataInicio AND @DataFim)
			-- Excluir categorias de Comunidade
			AND (CC.IN_Comunidade IS NULL OR CC.IN_Comunidade = 0)
			AND (@IdUf IS NULL OR MO.ID_UF = @IdUf)
	) AS Externos OPTION (MAXRECURSION 0)
END
