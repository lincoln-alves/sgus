-- =============================================
-- Author:		Edgar Froes
-- Create date: 06/12/2016
-- Description:	Retorna uma lista de usuários que serão notificados de acordo com os filtros informados.
-- =============================================
CREATE PROCEDURE [dbo].[SP_COMPILAR_USUARIOS_NOTIFICACAO]
	@Ufs VARCHAR(MAX) = NULL,
	@NiveisOcupacionais VARCHAR(MAX) = NULL,
	@Perfis VARCHAR(MAX) = NULL,
	@Status VARCHAR(MAX) = NULL,
	@Turma INT = NULL,
	@Usuarios VARCHAR(MAX) = NULL

AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @UfsTable TABLE(ID INT)
	DECLARE @UsuariosTable TABLE (ID INT)
	DECLARE @NiveisOcupacionaisTable TABLE(ID INT)
	DECLARE @PerfisTable TABLE(ID INT)
	DECLARE @StatusTable TABLE(ID INT)

	IF @Usuarios IS NOT NULL
	BEGIN
		INSERT INTO @UsuariosTable 
		SELECT CAST(Data as INT) FROM FN_Split(@Usuarios)
	END

	IF @Ufs IS NOT NULL
	BEGIN
		INSERT INTO @UfsTable 
		SELECT CAST(Data as INT) FROM FN_Split(@Ufs)
	END

	IF @NiveisOcupacionais IS NOT NULL
	BEGIN
		INSERT INTO @NiveisOcupacionaisTable 
		SELECT CAST(Data as INT) FROM FN_Split(@NiveisOcupacionais)
	END

	IF @Perfis IS NOT NULL
	BEGIN
		INSERT INTO @PerfisTable 
		SELECT CAST(Data as INT) FROM FN_Split(@Perfis)
	END

	IF @Status IS NOT NULL
	BEGIN
		INSERT INTO @StatusTable 
		SELECT CAST(Data as INT) FROM FN_Split(@Status)
	END

	-- QUERY
	SELECT
		U.ID_Usuario as ID,
		MIN(U.CPF) as CPF
	FROM
		TB_Usuario U
		LEFT JOIN TB_UsuarioPerfil UP ON UP.ID_Usuario = U.ID_Usuario
		LEFT JOIN TB_MatriculaOferta MO ON MO.ID_Usuario = U.ID_Usuario
		LEFT JOIN TB_MatriculaTurma MT ON MT.ID_MatriculaOferta = MO.ID_MatriculaOferta
	WHERE
		(@Usuarios IS NULL OR (SELECT COUNT(ID) FROM @UsuariosTable WHERE ID = U.ID_Usuario) > 0) AND
		(@Turma IS NULL OR MT.ID_Turma = @Turma) AND
		(@Ufs IS NULL OR (SELECT COUNT(ID) FROM @UfsTable WHERE ID = U.ID_UF) > 0) AND
		(@NiveisOcupacionais IS NULL OR (SELECT COUNT(ID) FROM @NiveisOcupacionaisTable WHERE ID = U.ID_NivelOcupacional) > 0) AND
		(@Perfis IS NULL OR (SELECT COUNT(ID) FROM @PerfisTable WHERE ID = UP.ID_Perfil) > 0) AND
		(@Status IS NULL OR (SELECT COUNT(ID) FROM @StatusTable WHERE ID = MO.ID_StatusMatricula) > 0) AND 
		LOWER(U.Situacao) = 'ativo'
	GROUP BY
		U.ID_Usuario
END