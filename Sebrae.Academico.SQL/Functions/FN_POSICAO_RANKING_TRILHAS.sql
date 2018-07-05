-- =============================================
-- Author:		Edgar Froes
-- Create date: 12/09/2017
-- Description:	Obtém a posição de um usuário no
--				ranking do nível.
--				
-- ATENÇÃO: Qualquer mudança aqui precisa ser
-- refletida na procedure SP_RANKING_TRILHAS
--
-- =============================================
ALTER FUNCTION [dbo].[FN_POSICAO_RANKING_TRILHAS]
(
	@ID_TrilhaNivel INT,
	@ID_Usuario INT
)
RETURNS INT
AS
BEGIN
	DECLARE @Ranking TABLE (Ordem INT, ID_Usuario INT)
	;WITH Ranking AS 
	(
		SELECT
			UT.ID_Usuario,
			MIN(U.Nome) as Nome,
			(SELECT SUM(mm.QT_MEDALHAS) FROM TB_MissaoMedalha mm WHERE mm.ID_UsuarioTrilha = UT.ID_UsuarioTrilha) as Medalhas,
			(CASE WHEN SUM(UTM.QT_MoedasOuro) IS NULL THEN 0 ELSE SUM(UTM.QT_MoedasOuro) END) as MoedasOuro,
			(CASE WHEN SUM(UTM.QT_MoedasPratas) IS NULL THEN 0 ELSE SUM(UTM.QT_MoedasPratas) END) as MoedasPrata,
			dbo.FN_ObterTrofeu(@ID_TrilhaNivel, (CASE WHEN SUM(UTM.QT_MoedasOuro) IS NULL THEN 0 ELSE SUM(UTM.QT_MoedasOuro) END)) as Trofeu,
			(SELECT COUNT(DISTINCT _ITP.ID_ItemTrilha) FROM TB_ItemTrilhaParticipacao _ITP JOIN TB_ItemTrilha _IT ON _IT.ID_ItemTrilha = _ITP.ID_ItemTrilha AND _IT.ID_Usuario IS NULL WHERE _ITP.IN_Autorizado = 1 AND _ITP.ID_UsuarioTrilha = MIN(UT.ID_UsuarioTrilha)) as SolucoesSebrae,
			(SELECT COUNT(DISTINCT _ITP.ID_ItemTrilha) FROM TB_ItemTrilhaParticipacao _ITP JOIN TB_UsuarioTrilha _UT ON _UT.ID_UsuarioTrilha = _ITP.ID_UsuarioTrilha AND _UT.ID_Usuario = UT.ID_Usuario JOIN TB_ItemTrilha _IT ON _IT.ID_ItemTrilha = _ITP.ID_ItemTrilha AND _IT.ID_Usuario = UT.ID_Usuario WHERE _UT.IN_NovasTrilhas = 1) as SolucoesTrilheiro
			-- Orderby bem ruim pra pegar a Ordem, mas é necessário pois da versão do SQLSERVER é anterior à 2012.
			,ROW_NUMBER() OVER
			(
				ORDER BY (CASE WHEN SUM(UTM.QT_MoedasOuro) IS NULL THEN 0 ELSE SUM(UTM.QT_MoedasOuro) END) DESC,
				(CASE WHEN SUM(UTM.QT_MoedasPratas) IS NULL THEN 0 ELSE SUM(UTM.QT_MoedasPratas) END) DESC,
				(SELECT COUNT(DISTINCT _ITP.ID_ItemTrilha) FROM TB_ItemTrilhaParticipacao _ITP JOIN TB_ItemTrilha _IT ON _IT.ID_ItemTrilha = _ITP.ID_ItemTrilha AND _IT.ID_Usuario IS NULL WHERE _ITP.IN_Autorizado = 1 AND _ITP.ID_UsuarioTrilha = MIN(UT.ID_UsuarioTrilha)) DESC,
				(SELECT COUNT(DISTINCT _ITP.ID_ItemTrilha) FROM TB_ItemTrilhaParticipacao _ITP JOIN TB_UsuarioTrilha _UT ON _UT.ID_UsuarioTrilha = _ITP.ID_UsuarioTrilha AND _UT.ID_Usuario = UT.ID_Usuario JOIN TB_ItemTrilha _IT ON _IT.ID_ItemTrilha = _ITP.ID_ItemTrilha AND _IT.ID_Usuario = UT.ID_Usuario WHERE _UT.IN_NovasTrilhas = 1) DESC,
				MIN(U.Nome)
			) AS Ordem
		FROM TB_UsuarioTrilha UT
		JOIN TB_Usuario U ON U.ID_Usuario = UT.ID_Usuario
		JOIN TB_UF UF ON UF.ID_UF = U.ID_UF
		LEFT JOIN TB_UsuarioTrilhaMoedas UTM ON UTM.ID_UsuarioTrilha = UT.ID_UsuarioTrilha
		WHERE UT.ID_TrilhaNivel = @ID_TrilhaNivel AND UT.IN_NovasTrilhas = 1
		GROUP BY UT.ID_Usuario, UT.ID_UsuarioTrilha
	)
	-- Critérios de ordenação no Ranking:
	-- 1º Quantidade de moedas de ouro adquiridas em todos os níveis da trilha;
	-- 2º Quantidade de moedas de prata adquiridas em todos os níveis da trilha;
	-- 3º Quantidade de Soluções Sebrae concluídas com sucesso em todos os níveis da trilha;
	-- 4º Quantidade de Soluções de Trilheiros criadas em todos os níveis da trilha;
	-- 5º Ordem alfabética crescente do nome do Trilheiro.
	INSERT INTO @Ranking (Ordem, ID_Usuario)
		SELECT Ordem, ID_Usuario FROM Ranking ORDER BY MoedasOuro DESC, MoedasPrata DESC, SolucoesSebrae DESC, SolucoesTrilheiro DESC, Nome
	RETURN (SELECT TOP 1 Ordem FROM @Ranking WHERE ID_Usuario = @ID_Usuario)
END
