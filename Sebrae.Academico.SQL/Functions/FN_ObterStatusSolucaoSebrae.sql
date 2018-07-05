-- =============================================
-- Author:		Edgar Froes
-- Create date: 14/09/2017
-- Description:	Obtém o status do usuário na SS.
--				
-- Status de retorno:
--
-- NaoInscrito = 1
-- EmAndamento = 2
-- Aprovado = 3
-- Pendente = 4
-- Revisar = 5
-- Abandono = 6
-- Reprovado = 7
--
-- =============================================
CREATE FUNCTION [dbo].[FN_ObterStatusSolucaoSebrae]
(
	@ID_ItemTrilha INT,
	@ID_TipoItemTrilha INT,
	@ID_UsuarioTrilha INT
)
RETURNS INT
AS
BEGIN
	
	-- Atividade ou Jogo.
	IF @ID_TipoItemTrilha = 1 OR @ID_TipoItemTrilha = 3
	BEGIN
		DECLARE @Autorizado BIT = (SELECT TOP 1 IN_Autorizado FROM TB_ItemTrilhaParticipacao WHERE ID_ItemTrilha = @ID_ItemTrilha AND ID_UsuarioTrilha = @ID_UsuarioTrilha ORDER BY ID_ItemTrilhaParticipacao DESC);
		 
		 -- NaoInscrito.
		IF @Autorizado IS NULL
		BEGIN
			RETURN 1;
		END

		-- Aprovado.
		IF @Autorizado = 1
		BEGIN
			RETURN 3;
		END

		-- Reprovado.
		IF @Autorizado = 0
		BEGIN
			RETURN 2;
		END
	END
	
	-- Discursiva.
	IF @ID_TipoItemTrilha = 2
	BEGIN
		DECLARE @Participacoes TABLE(ID_ItemTrilhaParticipacao INT, IN_Autorizado BIT, TP_Participacao TINYINT);

		INSERT INTO @Participacoes (ID_ItemTrilhaParticipacao, IN_Autorizado, TP_Participacao)
			SELECT ID_ItemTrilhaParticipacao, IN_Autorizado, TP_Participacao
			FROM TB_ItemTrilhaParticipacao
			WHERE ID_ItemTrilha = @ID_ItemTrilha AND ID_UsuarioTrilha = @ID_UsuarioTrilha

		-- NaoInscrito.
		IF (SELECT COUNT(ID_ItemTrilhaParticipacao) FROM @Participacoes) = 0
		BEGIN
			RETURN 1;
		END

		-- Aprovado.
		IF (SELECT COUNT(ID_ItemTrilhaParticipacao) FROM @Participacoes WHERE IN_Autorizado = 1) > 0
		BEGIN
			RETURN 3;
		END

		DECLARE @TipoUltimaParticipacao TINYINT = (SELECT TOP 1 TP_Participacao FROM @Participacoes ORDER BY ID_ItemTrilhaParticipacao DESC)

		-- Pendente.
		IF @TipoUltimaParticipacao = 1 -- Participação trilheiro
		BEGIN
			RETURN 4;
		END

		DECLARE @UltimaParticipacaoTrilheiroAutorizado BIT =
		(
			SELECT TOP 1 IN_Autorizado
			FROM @Participacoes
			WHERE TP_Participacao = 1 -- Participação trilheiro
			ORDER BY ID_ItemTrilhaParticipacao DESC
		)

		-- Revisar.
		IF
			@TipoUltimaParticipacao = 2 -- Iteração Monitor
			AND @UltimaParticipacaoTrilheiroAutorizado = 0 -- Não autorizado.
		BEGIN
			RETURN 5;
		END
	END
	
	-- Soluções.
	IF @ID_TipoItemTrilha = 4
	BEGIN
		-- Embora seja Table, só armazena uma linha (TOP 1).
		DECLARE @Matricula TABLE(IN_Autorizado BIT, ID_MatriculaOferta INT, ID_StatusMatricula INT, IsOfertaFechada BIT, IsTurmaFechada BIT);
		
		-- Obtém os dados necessários da matrícula do indivíduo.
		INSERT INTO @Matricula (IN_Autorizado, ID_MatriculaOferta, ID_StatusMatricula, IsOfertaFechada, IsTurmaFechada)
			SELECT
				TOP 1
				IN_Autorizado,
				MO.ID_MatriculaOferta,
				MO.ID_StatusMatricula,
				CASE WHEN DATEADD(day, CASE WHEN O.QT_DiasPrazo IS NULL THEN 0 ELSE O.QT_DiasPrazo END, O.DT_FimInscricoes) >= GETDATE() THEN 1 ELSE 0 END,
				CASE WHEN T.DT_Final IS NOT NULL AND CAST(T.DT_Final AS DATE) < CAST(GETDATE() AS DATE) THEN 0 ELSE 1 END
			FROM TB_ItemTrilhaParticipacao ITP
			JOIN TB_MatriculaOferta MO ON MO.ID_MatriculaOferta = ITP.ID_MatriculaOferta
			LEFT JOIN TB_MatriculaTurma MT ON MT.ID_MatriculaOferta = MO.ID_MatriculaOferta
			LEFT JOIN TB_Turma T ON T.ID_Turma = MT.ID_Turma
			JOIN TB_Oferta O ON O.ID_Oferta = MO.ID_Oferta
			WHERE ITP.ID_ItemTrilha = @ID_ItemTrilha AND ITP.ID_UsuarioTrilha = @ID_UsuarioTrilha AND ITP.ID_MatriculaOferta IS NOT NULL
			ORDER BY ID_MatriculaOferta DESC
			
		-- NaoInscrito.
		IF
			(SELECT COUNT(ID_MatriculaOferta) FROM @Matricula) > 0
			-- Caso a matrícula não estiver aprovada e a oferta estiver fechada, considerar como Não Inscrito.
			AND (SELECT COUNT(ID_StatusMatricula) FROM dbo.FN_StatusAprovado() WHERE ID_StatusMatricula = (SELECT TOP 1 ID_StatusMatricula FROM @Matricula)) = 0
			AND (SELECT TOP 1 IsOfertaFechada FROM @Matricula) = 1
		BEGIN
			RETURN 1;
		END

		-- Abandono.
		IF (SELECT TOP 1 ID_StatusMatricula FROM @Matricula) = 5 -- Abandono.
		BEGIN
			RETURN 6;
		END


		-- Reprovado.
		IF	(SELECT TOP 1 IN_Autorizado FROM @Matricula) != 1 AND (SELECT TOP 1 IsTurmaFechada FROM @Matricula) = 1
		BEGIN
			RETURN 7;
		END
		
		-- EmAndamento.
		IF	(SELECT TOP 1 IN_Autorizado FROM @Matricula) != 1
		BEGIN
			RETURN 2;
		END

		-- Aprovado.
		RETURN 3;
	END

	RETURN NULL;
END