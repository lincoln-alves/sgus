DECLARE @NAME NVARCHAR(MAX) = (SELECT TOP 1 NAME FROM sys.objects WHERE object_id = OBJECT_ID(N'DASHBOARD_REL_MatriculasTurmas') AND TYPE IN ( N'P', N'PC' ));
DECLARE @DROP_COMMAND NVARCHAR(MAX) = 'DROP PROCEDURE ' + @NAME;
IF @NAME IS NOT NULL BEGIN EXECUTE sp_executesql @DROP_COMMAND END
GO

-- =============================================
-- 08/11/2016 - Jair Santos - #1364
-- Adicionado campo UFResponsavel
-- =============================================

CREATE PROCEDURE [dbo].[DASHBOARD_REL_MatriculasTurmas]
  @IdCategoriaConteudo AS VARCHAR(max) = null,
	@P_UFResponsavel VARCHAR(1000) = NULL

AS
BEGIN

SELECT 
	t.ID_Turma, 
	t.NM_Turma NomeTurma, 
	se.NM_SolucaoEducacional as SolucaoEducacional,
	t.VL_Status Status, 
	t.DT_Inicio as DataInicio, 
	t.DT_Final as DataFim, 
	COUNT(mt.ID_Turma) TotalMatriculados 
FROM 
	TB_Turma t
INNER JOIN TB_Oferta o ON o.ID_Oferta = t.ID_Oferta
INNER JOIN TB_SolucaoEducacional se ON se.ID_SolucaoEducacional = o.ID_SolucaoEducacional 
	AND ((@P_UFResponsavel IS NULL OR LEN(@P_UFResponsavel) = 0) OR se.ID_UFGestor IN (SELECT Data FROM FN_Split(@P_UFResponsavel)))
LEFT JOIN TB_MatriculaTurma mt ON mt.ID_Turma = T.ID_Turma
WHERE 
	t.VL_Status IS NOT NULL OR 
	se.ID_CategoriaConteudo IN(SELECT data FROM FN_Split(@IdCategoriaConteudo))
GROUP BY 
	t.ID_Turma,  
	t.NM_Turma, 
	se.NM_SolucaoEducacional,
	t.VL_Status, 
	t.IN_Aberta, 
	t.DT_Inicio, 
	t.DT_Final
ORDER BY t.DT_Inicio DESC
END