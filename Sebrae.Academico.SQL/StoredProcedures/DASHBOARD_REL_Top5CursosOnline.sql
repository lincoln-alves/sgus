-- Procedure usada na home para recuperar o top 5 de cursos online com usuários inscritos
-- Data: 18/04/2016
-- Alterações
-- Detalhes na Demanda #3442
-----------------------------------------------------------
-- Remocao dos resitros que possuem a Forma de Aquisicao: Comunidade Virtual conforme Task #569 Item 5.
-- Data: 26/07/2016
-- Autor: Jair Foro
-----------------------------------------------------------
CREATE PROCEDURE [dbo].[DASHBOARD_REL_Top5CursosOnline] 
 @DataInicio DATE = NULL,
 @DataFim DATE = NULL,
 @IdUf INT = NULL AS
BEGIN

-- Recupera o ID da Forma de Aquisicao Comunidade Virtual
DECLARE @ID_Comunidade_Virtual INT = (SELECT TOP 1 VL_REGISTRO FROM TB_ConfiguracaoSistema WHERE ID_ConfiguracaoSistema = 39);

SET LANGUAGE Brazilian 
SELECT
	TOP 5 o.ID_SolucaoEducacional,
	se.NM_SolucaoEducacional NomeSolucaoEducacional,
	cc.NM_CategoriaConteudo Categoria,
	COUNT (DISTINCT mo.ID_Usuario) QuantidadeDeUsuariosInscritos
FROM
	TB_SolucaoEducacional se
INNER JOIN TB_Oferta o ON o.ID_SolucaoEducacional = se.ID_SolucaoEducacional
INNER JOIN TB_MatriculaOferta mo ON mo.ID_Oferta = o.ID_Oferta
INNER JOIN TB_MatriculaTurma mt ON mt.ID_MatriculaOferta = mo.ID_MatriculaOferta
INNER JOIN TB_CategoriaConteudo cc ON cc.ID_CategoriaConteudo = se.ID_CategoriaConteudo
INNER JOIN TB_FormaAquisicao fa ON fa.ID_FormaAquisicao = se.ID_FormaAquisicao AND fa.ID_FormaAquisicao <> @ID_Comunidade_Virtual
WHERE
	mo.ID_StatusMatricula IN (
		SELECT
			ID_StatusMatricula
		FROM
			FN_StatusAprovado ()
	)
AND (
	CAST (mt.DT_Termino AS DATE) BETWEEN @DataInicio
	AND @DataFim
) -- Regras para capacitação online 
AND fa.NM_FormaAquisicao LIKE 'Curso on-line'
AND (
	@IdUf IS NULL
	OR mo.ID_UF = @IdUf
)
GROUP BY
	o.ID_SolucaoEducacional,
	se.NM_SolucaoEducacional,
	cc.NM_CategoriaConteudo
ORDER BY
	COUNT (o.ID_SolucaoEducacional) DESC
END