-- =============================================
-- Author:		Edgar Froes
-- Create date: 12/09/2017
-- Description:	Obtém a quantidade de moedas nos itens trilha disponíveis.
--
-- =============================================

CREATE FUNCTION [dbo].[FN_ObterTotalMoedasTrilhaNivel]
(
	@ID_TrilhaNivel INT
)
RETURNS int
AS
BEGIN
	DECLARE @TotalMoedas INT =
	(
		SELECT SUM(IT.VL_Moedas)
		FROM TB_ItemTrilha IT
		JOIN TB_Missao M ON M.ID_Missao = IT.ID_Missao
		JOIN TB_PontoSebrae P ON P.ID_PontoSebrae = M.ID_PontoSebrae
		WHERE IT.ID_Usuario IS NULL AND IT.ID_TipoItemTrilha IS NOT NULL AND IT.VL_Moedas IS NOT NULL
	);

	RETURN @TotalMoedas;
END