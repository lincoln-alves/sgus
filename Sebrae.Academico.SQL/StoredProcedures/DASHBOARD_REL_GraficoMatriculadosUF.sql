-----------------------------------------------------------
-- Remocao dos resitros que possuem a Forma de Aquisicao: Comunidade Virtual conforme Task #569 Item 5.
-- Data: 26/07/2016
-- Autor: Jair Foro
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[DASHBOARD_REL_GraficoMatriculadosUF] 
 @pDataInicio dateTime,
 @pDataFim dateTime AS
BEGIN

-- Recupera o ID da Forma de Aquisicao Comunidade Virtual
DECLARE @ID_Comunidade_Virtual INT = (SELECT TOP 1 VL_REGISTRO FROM TB_ConfiguracaoSistema WHERE ID_ConfiguracaoSistema = 39);

SET NOCOUNT ON ; SELECT DISTINCT
	mo.id_uf AS Uf,
	u.NM_UF AS Estado,
	dbo.fnDashboardPercentualMatriculadosUF (
		(
			SELECT
				COUNT (DISTINCT _mo.id_usuario) AS total
			FROM
				TB_Matriculaoferta _MO
		INNER JOIN TB_Oferta O ON O.ID_Oferta = _MO.ID_Oferta
		INNER JOIN TB_SolucaoEducacional SE ON SE.ID_SolucaoEducacional = O.ID_SolucaoEducacional AND SE.ID_FormaAquisicao <> @ID_Comunidade_Virtual
			WHERE
				_mo.id_uf = mo.id_uf
			AND (
				DT_Solicitacao BETWEEN @pDataInicio
				AND @pDataFim
			)
		),
		mo.id_uf ,@pDataFim
	) AS Porcentagem,
	(
		SELECT
			COUNT (DISTINCT _mo.id_usuario) AS total
		FROM
			TB_Matriculaoferta _MO
		INNER JOIN TB_Oferta O ON O.ID_Oferta = _MO.ID_Oferta
		INNER JOIN TB_SolucaoEducacional SE ON SE.ID_SolucaoEducacional = O.ID_SolucaoEducacional AND SE.ID_FormaAquisicao <> @ID_Comunidade_Virtual
		WHERE
			_MO.id_uf = mo.id_uf
		AND (
			DT_Solicitacao BETWEEN @pDataInicio
			AND @pDataFim
		)
	) AS QuantidadePorUf,
	(
		SELECT
			COUNT (id_usuario)
		FROM
			tb_usuario us
		WHERE
			us.id_uf = mo.id_uf
		AND us.situacao = 'ativo'
		AND DataAdmissao <= @pDataFim
	) AS QuantidadeTotalUsuariosPorUF
FROM
	TB_Matriculaoferta MO
INNER JOIN tb_uf u ON u.ID_UF = mo.ID_UF
WHERE
	(
		DT_Solicitacao BETWEEN @pDataInicio
		AND @pDataFim
	)
GROUP BY
	mo.id_uf,
	u.NM_UF
ORDER BY
	mo.id_uf ASC,
	QuantidadePorUf DESC
END