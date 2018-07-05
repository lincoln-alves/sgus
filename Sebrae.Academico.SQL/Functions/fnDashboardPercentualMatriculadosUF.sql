CREATE FUNCTION [dbo].[fnDashboardPercentualMatriculadosUF]
(
	@pQuantidade  decimal(10,2),
	@pUfId  int,
	@pDataLimite dateTime
)
RETURNS decimal(10,2)
AS
BEGIN
	declare @resultado decimal(10,2) = 0;
	declare @totalUsuariosUF decimal(10,2) = 0;
	set @totalUsuariosUF = (select count(id_usuario) from tb_usuario where id_uf = @pUfId and situacao = 'ativo' and DataAdmissao <= @pDataLimite);
	set @resultado = (@pQuantidade*100)/@totalUsuariosUF;
	RETURN @resultado;
END