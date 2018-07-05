DECLARE @NAME NVARCHAR(MAX) = (SELECT TOP 1 NAME FROM sys.objects WHERE object_id = OBJECT_ID(N'DASHBOARD_REL_MatriculasTurmasTotais') AND TYPE IN ( N'P', N'PC' ));
DECLARE @DROP_COMMAND NVARCHAR(MAX) = 'DROP PROCEDURE ' + @NAME;
IF @NAME IS NOT NULL BEGIN EXECUTE sp_executesql @DROP_COMMAND END
GO

-- =============================================
-- 08/11/2016 - Jair Santos - #1364
-- Adicionado campo UFResponsavel
-- =============================================

CREATE PROCEDURE [dbo].[DASHBOARD_REL_MatriculasTurmasTotais]
  @IdCategoriaConteudo AS VARCHAR(max) = null,
  @IdStatusTurma AS VARCHAR(max) = null,
  @dataInicio AS DATE = null,
  @dataFim as DATE = null,
	@P_UFResponsavel VARCHAR(1000) = NULL

AS
BEGIN

DECLARE @TotalTurmas INT = (SELECT 
								COUNT(T.ID_Turma) 
							FROM 
								TB_Turma T
							WHERE 
								T.VL_Status IS NOT NULL AND 
								( (@dataInicio IS NULL AND @dataFim IS NULL) OR  ( CAST(T.DT_Inicio as DATE) BETWEEN @dataInicio AND @dataFim ) )
							);
DECLARE @TotalMatriculas INT = (SELECT 
									COUNT(ID_MatriculaTurma) 
								FROM 
									TB_MatriculaTurma MT 
								JOIN TB_Turma T2 ON MT.ID_Turma = T2.ID_Turma
								WHERE 
									VL_Status IS NOT NULL AND 
									( (@dataInicio IS NULL AND @dataFim IS NULL) OR  ( CAST(MT.DT_Matricula as DATE) BETWEEN @dataInicio AND @dataFim ) )
								)

SELECT
	T.VL_Status as Status,
	COUNT( DISTINCT T.ID_Turma ) as TotalTurmas,
	COUNT(MT.ID_MatriculaTurma) as TotalMatriculados,
	@TotalTurmas as TotalTurmasComStatus,
	@TotalMatriculas as TotalMatriculasComStatus,
	(
		CASE
			WHEN (@dataInicio IS NULL AND @dataFim IS NULL) THEN 0
			WHEN T.VL_Status IS NOT NULL THEN (SELECT 
												COUNT(DISTINCT __T.ID_Turma) 
												FROM TB_Turma __T 
												WHERE __T.VL_Status = T.VL_Status AND 
												( CAST(__T.DT_Inicio as DATE) BETWEEN @dataInicio AND @dataFim ))
		END
	) AS TotalTurmasAno,

	(
		CASE
			WHEN (@dataInicio IS NULL AND @dataFim IS NULL) THEN 0
			WHEN T.VL_Status IS NOT NULL THEN
				(SELECT COUNT(__MT.ID_MatriculaTurma) FROM TB_MatriculaTurma __MT JOIN TB_Turma T2 ON __MT.ID_Turma = T2.ID_Turma
				WHERE T2.VL_Status = T.VL_Status AND ( CAST(__MT.DT_Matricula as DATE) BETWEEN @dataInicio AND @dataFim ) )
		END
	) AS TotalMatriculadosAno
FROM
	TB_Turma T
INNER JOIN TB_Oferta O ON O.ID_Oferta = T.ID_Oferta
INNER JOIN TB_SolucaoEducacional SE ON SE.ID_SolucaoEducacional = O.ID_SolucaoEducacional
LEFT JOIN TB_MatriculaTurma MT ON MT.ID_Turma = T.ID_Turma

WHERE 
	( @IdCategoriaConteudo IS NULL OR se.ID_CategoriaConteudo IN (SELECT data FROM FN_Split(@IdCategoriaConteudo)) ) AND
	( (@dataInicio IS NULL AND @dataFim IS NULL) OR  ( CAST(T.DT_Inicio as DATE) BETWEEN @dataInicio AND @dataFim ) ) AND
	( (@dataInicio IS NULL AND @dataFim IS NULL) OR  ( CAST(MT.DT_Matricula as DATE) BETWEEN @dataInicio AND @dataFim ) ) AND
	( @IdStatusTurma IS NULL OR t.VL_Status IN (SELECT data FROM FN_Split(@IdStatusTurma))) AND
	((@P_UFResponsavel IS NULL OR LEN(@P_UFResponsavel) = 0) OR SE.ID_UFGestor IN (SELECT Data FROM FN_Split(@P_UFResponsavel)))

GROUP BY t.VL_Status

END