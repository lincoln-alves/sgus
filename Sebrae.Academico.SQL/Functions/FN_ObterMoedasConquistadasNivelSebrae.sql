CREATE FUNCTION [dbo].FN_ObterMoedasConquistadasNivelSebrae
( 
	@ID_TrilhaNivel INT,
	@ID_UsuarioTrilha INT
)
RETURNS INT
AS
BEGIN
	RETURN (SELECT SUM(dbo.FN_ObterMoedasConquistadasPontoSebrae(@ID_UsuarioTrilha, ID_PontoSebrae)) FROM TB_PontoSebrae WHERE ID_TrilhaNivel = @ID_TrilhaNivel AND IN_Ativo = 1)
END
