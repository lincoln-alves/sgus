-----------------------------------------------------------
-- Remocao dos resitros que possuem a Forma de Aquisicao: Comunidade Virtual conforme Task #569 Item 5.
-- Data: 26/07/2016
-- Autor: Jair Foro
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[DASHBOARD_REL_MatriculasPorMes] 
 @pDataInicio dateTime = NULL,
 @pDataFim dateTime = NULL,
 @IdUf INT = NULL AS
BEGIN

	-- Recupera o ID da Forma de Aquisicao Comunidade Virtual
	DECLARE @ID_Comunidade_Virtual INT = (SELECT TOP 1 VL_REGISTRO FROM TB_ConfiguracaoSistema WHERE ID_ConfiguracaoSistema = 39);

	DECLARE @StatusAprovados TABLE (ID_StatusMatricula INT) INSERT INTO @StatusAprovados SELECT ID_StatusMatricula FROM FN_StatusAprovado ()
	
	SELECT
		LEFT (DATENAME(MONTH,((CASE WHEN COUNT (MO_Inscricao.ID_MatriculaOferta) > 0 THEN MAX (MT.DT_Matricula) ELSE MAX (MT.DT_Termino) END))),3) AS Mes,
		COUNT (MO_Inscricao.ID_MatriculaOferta) AS TotalMatriculas,
		COUNT (MO_Aprovacao.ID_MatriculaOferta) AS TotalAprovados
	FROM TB_MatriculaTurma MT
		LEFT JOIN TB_MatriculaOferta MO_Inscricao ON MT.ID_MatriculaOferta = MO_Inscricao.ID_MatriculaOferta AND MO_Inscricao.ID_StatusMatricula = 2
		LEFT JOIN TB_Oferta OFE ON OFE.ID_Oferta = MO_Inscricao.ID_Oferta
		LEFT JOIN TB_SolucaoEducacional SEI ON SEI.ID_SolucaoEducacional = OFE.ID_SolucaoEducacional AND SEI.ID_FormaAquisicao <> @ID_Comunidade_Virtual
		LEFT JOIN TB_MatriculaOferta MO_Aprovacao ON MT.ID_MatriculaOferta = MO_Aprovacao.ID_MatriculaOferta
		AND MO_Aprovacao.ID_StatusMatricula IN (SELECT ID_StatusMatricula FROM @StatusAprovados)
		LEFT JOIN TB_Oferta OFEA ON OFEA.ID_Oferta = MO_Aprovacao.ID_Oferta
		LEFT JOIN TB_SolucaoEducacional SEA ON SEA.ID_SolucaoEducacional = OFEA.ID_SolucaoEducacional AND SEA.ID_FormaAquisicao <> @ID_Comunidade_Virtual

		JOIN TB_MatriculaOferta MO ON MT.ID_MatriculaOferta = MO.ID_MatriculaOferta
		JOIN TB_Oferta O ON MO.ID_Oferta = O.ID_Oferta
		JOIN TB_SolucaoEducacional SE ON O.ID_SolucaoEducacional = SE.ID_SolucaoEducacional
		JOIN TB_CategoriaConteudo CA ON SE.ID_CategoriaConteudo = CA.ID_CategoriaConteudo
	WHERE
		-- Somente as matrículas de Inscrição ou Aprovações.
		(MO_Inscricao.ID_MatriculaOferta IS NOT NULL OR MO_Aprovacao.ID_MatriculaOferta IS NOT NULL)
		AND (@pDataInicio IS NULL OR CAST((CASE WHEN MO_Inscricao.ID_MatriculaOferta IS NOT NULL THEN MT.DT_Matricula WHEN MO_Aprovacao.ID_MatriculaOferta IS NOT NULL THEN MT.DT_Termino END) AS DATE) > @pDataInicio)
		AND (@pDataFim IS NULL OR CAST ((CASE WHEN MO_Inscricao.ID_MatriculaOferta IS NOT NULL THEN MT.DT_Matricula WHEN MO_Aprovacao.ID_MatriculaOferta IS NOT NULL THEN MT.DT_Termino END) AS DATE) < @pDataFim)
		AND (@IdUf IS NULL OR (MO_Inscricao.ID_UF = @IdUf OR MO_Aprovacao.ID_UF = @IdUf))
		-- Excluir categorias de Comunidade
        AND (CA.IN_Comunidade IS NULL OR CA.IN_Comunidade = 0)

	GROUP BY
		-- Fazer um agrupamento por uma string com o mês e o ano da data, de acordo com o tipo de matrícula.
		(
			CAST(MONTH((CASE WHEN MO_Inscricao.ID_MatriculaOferta IS NOT NULL THEN MT.DT_Matricula ELSE MT.DT_Termino END)) AS VARCHAR (100)) +
			CAST(YEAR((CASE WHEN MO_Inscricao.ID_MatriculaOferta IS NOT NULL THEN MT.DT_Matricula ELSE MT.DT_Termino END)) AS VARCHAR (100))
		)
	ORDER BY
		(CASE WHEN COUNT (MO_Inscricao.ID_MatriculaOferta) > 0 THEN MAX (MT.DT_Matricula) ELSE MAX (MT.DT_Termino) END)
END