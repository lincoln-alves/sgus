CREATE FUNCTION [dbo].[FN_ObterMoedasConquistadasPontoSebrae]
( 
	@ID_UsuarioTrilha INT,
	@ID_PontoSebrae INT
)
RETURNS int
AS
BEGIN
    DECLARE @MoedasConquistas TABLE(Moedas INT NULL)

	INSERT INTO @MoedasConquistas
		SELECT UTM.QT_MoedasOuro
		FROM TB_UsuarioTrilhaMoedas UTM
		JOIN TB_ItemTrilha IT ON IT.ID_ItemTrilha = UTM.ID_ItemTrilha
		JOIN TB_Missao M ON M.ID_Missao = IT.ID_Missao
		WHERE M.ID_PontoSebrae = @ID_PontoSebrae AND UTM.ID_UsuarioTrilha = @ID_UsuarioTrilha

	RETURN (SELECT SUM(ISNULL(Moedas, 0)) FROM @MoedasConquistas);
END