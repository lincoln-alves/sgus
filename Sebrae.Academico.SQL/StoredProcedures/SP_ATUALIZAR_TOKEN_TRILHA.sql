-- =============================================
-- Author:		Edgar Froes
-- Create date: 31/08/2017
-- Description:	Obtém o ranking do nível.
--				
-- ATENÇÃO: Atualizar o token e o expiration
--			de trilhas, pois o NHibernate não
--			atualiza campos isolados.
--
-- =============================================
CREATE PROCEDURE SP_ATUALIZAR_TOKEN_TRILHA
	@ID_Usuario INT,
	@TrilhaToken VARCHAR(200) = NULL,
	@TrilhaTokenExpiry DATETIME
AS
BEGIN
	UPDATE TB_Usuario
	SET NM_TrilhaToken = CASE WHEN @TrilhaToken IS NOT NULL THEN @TrilhaToken ELSE (SELECT NM_TrilhaToken FROM TB_Usuario WHERE ID_Usuario = @ID_Usuario) END,
	DT_TrilhaTokenExpiry = @TrilhaTokenExpiry
	WHERE ID_Usuario = @ID_Usuario
END