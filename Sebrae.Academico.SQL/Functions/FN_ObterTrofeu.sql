CREATE FUNCTION FN_ObterTrofeu
(
	@ID_TrilhaNivel INT,
	@QntMoedasOuroUsuario INT
)
RETURNS INT
AS
BEGIN
	DECLARE @PorcentagemTrofeusString VARCHAR(100) = (SELECT TOP 1 TX_PorcentagensTrofeus FROM TB_TrilhaNivel WHERE ID_TrilhaNivel = @ID_TrilhaNivel);

	SET @PorcentagemTrofeusString = (CASE WHEN @PorcentagemTrofeusString IS NULL OR (LTRIM(RTRIM(@PorcentagemTrofeusString))) LIKE '' THEN '33,66' ELSE @PorcentagemTrofeusString END);

	-- Obt�m os par�metros de porcentagem para definir os trof�us.
	DECLARE @Porcentagem1 INT, @Porcentagem2 INT;
	SET @Porcentagem1 = (SELECT CAST(SUBSTRING(@PorcentagemTrofeusString, 0, CHARINDEX(',', @PorcentagemTrofeusString)) AS INT))
	SET @Porcentagem2 = (SELECT CAST(SUBSTRING(@PorcentagemTrofeusString, CHARINDEX(',', @PorcentagemTrofeusString)+1, LEN(@PorcentagemTrofeusString)) AS INT))

	-- Obter o Total de moedas dispon�veis em todas as Solu��es Sebrae do n�vel.
	DECLARE @TotalMoedas INT =
		(SELECT SUM((CASE WHEN IT.VL_Moedas IS NULL THEN 0 ELSE IT.VL_Moedas END))
		FROM TB_ItemTrilha IT
		JOIN TB_Missao M ON M.ID_Missao = IT.ID_Missao
		JOIN TB_PontoSebrae P ON P.ID_PontoSebrae = M.ID_PontoSebrae
		WHERE ID_Usuario IS NULL AND P.ID_TrilhaNivel = @ID_TrilhaNivel);

	-- Calaular a porcentagem de moedas que o usu�rio conquistou com base no total de moedas das Solu��es Sebrae realizadas no n�vel.
	DECLARE @PorcentagemConquistada DECIMAL(10,2) =
		(CAST(@QntMoedasOuroUsuario AS DECIMAL(10,4)) * 100) / CAST((CASE WHEN @TotalMoedas = 0 THEN 1 ELSE @TotalMoedas END) AS DECIMAL(10,4))
	
	RETURN (CASE WHEN @PorcentagemConquistada <= @Porcentagem1 THEN 1 ELSE (CASE WHEN @PorcentagemConquistada > @Porcentagem2 THEN 3 ELSE 2 END) END)
END