DECLARE @CT INT = 1
DECLARE @ID_Cargo INT
DECLARE @ID_CargoPai INT
DECLARE @ID_Cargo_New INT
DECLARE @ID_CargoPai_New INT

WHILE @CT <= (SELECT COUNT(ID_Cargo) FROM TB_Cargo)
BEGIN
	DECLARE @Cargos TABLE(ID_Cargo INT, ID_CargoPai INT NULL);
	DELETE FROM @Cargos;
	INSERT INTO @Cargos (ID_Cargo, ID_CargoPai) SELECT TOP(@CT) ID_Cargo, ID_CargoPai FROM TB_Cargo

	SET @ID_Cargo = (SELECT TOP(1) ID_Cargo FROM @Cargos ORDER BY ID_Cargo DESC)
	SET @ID_CargoPai = (SELECT TOP(1) ID_CargoPai FROM @Cargos ORDER BY ID_Cargo DESC)

	IF @ID_CargoPai IS NOT NULL
	BEGIN
		SET @ID_CargoPai_New = (SELECT TOP(1) ID_Cargo_New FROM TB_Cargo WHERE ID_Cargo = @ID_CargoPai);

		DECLARE @SQL NVARCHAR(MAX) = (N'UPDATE TB_Cargo SET ID_CargoPai_New = ' + CAST(@ID_CargoPai_New AS VARCHAR(255)) + ' WHERE ID_Cargo = ' + CAST(@ID_Cargo AS VARCHAR(255)));

		EXEC (@SQL)
	END

	DECLARE @UsuariosCargo TABLE(ID_UsuarioCargo INT)
	DELETE FROM @UsuariosCargo
	INSERT INTO @UsuariosCargo SELECT ID_UsuarioCargo FROM TB_UsuarioCargo WHERE ID_Cargo = @ID_Cargo ORDER BY ID_UsuarioCargo

	IF (SELECT COUNT(ID_UsuarioCargo) FROM @UsuariosCargo) > 0
	BEGIN
		SET @ID_Cargo_New = (SELECT TOP(1) ID_Cargo_New FROM TB_Cargo WHERE ID_Cargo = @ID_Cargo);

		DECLARE @CT_U INT = 1;

		WHILE @CT_U <= (SELECT COUNT(ID_UsuarioCargo) FROM @UsuariosCargo)
		BEGIN
			DECLARE @UCargos TABLE(ID_UsuarioCargo INT);
			DELETE FROM @UCargos;
			INSERT INTO @UCargos (ID_UsuarioCargo) SELECT TOP(@CT_U) ID_UsuarioCargo FROM @UsuariosCargo

			DECLARE @ID_UsuarioCargo INT = (SELECT TOP(1) ID_UsuarioCargo FROM @UCargos ORDER BY ID_UsuarioCargo DESC)

			SET @SQL = 'UPDATE TB_UsuarioCargo SET ID_Cargo = ' + CAST(@ID_Cargo_New AS VARCHAR(255)) + ' WHERE ID_UsuarioCargo = ' + CAST(@ID_UsuarioCargo AS VARCHAR(255))

			EXEC (@SQL)
			SET @CT_U = @CT_U + 1;
		END
	END

	SET @CT = @CT + 1
END