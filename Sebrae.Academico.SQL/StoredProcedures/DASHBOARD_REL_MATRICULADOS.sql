-----------------------------------------------------------
-- Remocao dos resitros que possuem a Forma de Aquisicao: Comunidade Virtual conforme Task #569 Item 5.
-- Data: 26/07/2016
-- Autor: Jair Foro
-----------------------------------------------------------


CREATE PROCEDURE [dbo].[DASHBOARD_REL_MATRICULADOS] 
 @DataInicio DATE = NULL,
 @DataFim DATE = NULL,
 @IdUf INT = NULL AS
BEGIN

-- Recupera o ID da Forma de Aquisicao Comunidade Virtual
DECLARE @ID_Comunidade_Virtual INT = (SELECT TOP 1 VL_REGISTRO FROM TB_ConfiguracaoSistema WHERE ID_ConfiguracaoSistema = 39);

-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON ;
	SELECT
	(
		SELECT COUNT (a.ID_MatriculaTurma)
		FROM TB_MatriculaTurma A
			INNER JOIN TB_Turma B ON A.ID_Turma = B.ID_Turma
			INNER JOIN TB_Oferta O ON O.ID_Oferta = B.ID_Oferta
			INNER JOIN TB_MatriculaOferta CC ON CC.ID_MatriculaOferta = A.ID_MatriculaOferta
			INNER JOIN TB_StatusMatricula SM ON SM.ID_StatusMatricula = CC.ID_StatusMatricula
			INNER JOIN TB_SolucaoEducacional SE ON SE.ID_SolucaoEducacional = O.ID_SolucaoEducacional AND SE.ID_FormaAquisicao <> @ID_Comunidade_Virtual
			INNER JOIN TB_CategoriaConteudo CA ON SE.ID_CategoriaConteudo = CA.ID_CategoriaConteudo
			INNER JOIN TB_Fornecedor FO ON FO.ID_Fornecedor = SE.ID_Fornecedor
			INNER JOIN TB_FormaAquisicao FA ON FA.ID_FormaAquisicao = SE.ID_FormaAquisicao
			INNER JOIN TB_Usuario U ON U.Id_Usuario = CC.Id_Usuario
			INNER JOIN TB_UF UF ON UF.Id_UF = CC.ID_UF
			INNER JOIN TB_NivelOcupacional NO ON NO .ID_NivelOcupacional = CC.ID_NivelOcupacional
		WHERE
			CC.ID_StatusMatricula = 2
		-- Somente Empregados
		AND CC.ID_NivelOcupacional IN (SELECT ID_NivelOcupacional FROM FN_ObterNiveisOcupacionaisInternos())
		-- Somente no ano informado
		AND ((@DataInicio IS NULL OR @DataFim IS NULL) OR (A.DT_Matricula IS NOT NULL AND CAST (A.DT_Matricula AS DATE) BETWEEN @DataInicio AND @DataFim))
		-- Excluir categorias de Comunidade
		AND (CA.IN_Comunidade IS NULL OR CA.IN_Comunidade = 0)
		AND (@IdUf IS NULL OR cc.ID_UF = @IdUf)
	) AS Internos,
	(
		SELECT COUNT (a.ID_MatriculaTurma)
		FROM TB_MatriculaTurma A
			INNER JOIN TB_Turma B ON A.ID_Turma = B.ID_Turma
			INNER JOIN TB_Oferta O ON O.ID_Oferta = B.ID_Oferta
			INNER JOIN TB_MatriculaOferta CC ON CC.ID_MatriculaOferta = A.ID_MatriculaOferta
			INNER JOIN TB_StatusMatricula SM ON SM.ID_StatusMatricula = CC.ID_StatusMatricula
			INNER JOIN TB_SolucaoEducacional SE ON SE.ID_SolucaoEducacional = O.ID_SolucaoEducacional AND SE.ID_FormaAquisicao <> @ID_Comunidade_Virtual
			INNER JOIN TB_CategoriaConteudo CA ON SE.ID_CategoriaConteudo = CA.ID_CategoriaConteudo
			INNER JOIN TB_Fornecedor FO ON FO.ID_Fornecedor = SE.ID_Fornecedor
			INNER JOIN TB_FormaAquisicao FA ON FA.ID_FormaAquisicao = SE.ID_FormaAquisicao
			INNER JOIN TB_Usuario U ON U.Id_Usuario = CC.Id_Usuario
			INNER JOIN TB_UF UF ON UF.Id_UF = CC.ID_UF
			INNER JOIN TB_NivelOcupacional NO ON NO .ID_NivelOcupacional = CC.ID_NivelOcupacional
		WHERE
		-- Somente colaboradores externos
		CC.ID_NivelOcupacional IN (SELECT ID_NivelOcupacional FROM FN_ObterNiveisOcupacionaisExternos ())
		-- Somente no ano informado
		AND ((@DataInicio IS NULL OR @DataFim IS NULL) OR (A.DT_Matricula IS NOT NULL AND CAST (A.DT_Matricula AS DATE) BETWEEN @DataInicio AND @DataFim))
		-- Excluir categorias de Comunidade
		AND (CA.IN_Comunidade IS NULL OR CA.IN_Comunidade = 0)
		AND (@IdUf IS NULL OR cc.ID_UF = @IdUf)
	) AS Externos
	
	OPTION (MAXRECURSION 0)
END