CREATE PROCEDURE [dbo].[SP_Protocolo]
	@P_CpfUsuario VARCHAR(20) = NULL,
	@P_NumProtocolo VARCHAR(20) = NULL,
	@P_TxtDiscriminacao VARCHAR(100) = NULL,
	@P_DtaEnvio VARCHAR(10) = NULL,
	@P_DtaRecebimento VARCHAR(10) = NULL,
	@P_NomRemetente VARCHAR(100) = NULL,
	@P_NomDestinatario VARCHAR(100) = NULL,
	@P_Historico int = 1   --0 Aguardando
AS BEGIN

	IF (@P_Historico = 1) 
		BEGIN
			SELECT 
				P.ID_Protocolo AS ID, 
				P.Numero AS Numero, 
				P.NM_Descricao AS Descricao,
				P.ID_UsuarioRementente, 
				P.ID_UsuarioDestinatario,
				(SELECT CONVERT (VARCHAR (10),P1.DT_DataEnvio,103) + ' ' + CONVERT (VARCHAR (8),P1.DT_DataEnvio,14) FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
				) AS DataEnvio,
				(SELECT CONVERT (VARCHAR (10),P1.DT_DataRecebimento,103) + ' ' + CONVERT (VARCHAR (8),P1.DT_DataRecebimento,14) FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
				) AS DataRecebimento,
				(SELECT userDest.Nome FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
					INNER JOIN TB_Usuario userDest ON userDest.ID_Usuario = P1.ID_UsuarioDestinatario
				) AS NomeDestinatario,
				(SELECT userRemet.Nome FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
					INNER JOIN TB_Usuario userRemet ON userRemet.ID_Usuario = P1.ID_UsuarioRementente
				) AS NomeRemetente,
				--userDest.Nome AS NomeDestinatario,
				--userRemet.Nome AS NomeRemetente,
				(SELECT COUNT(ID_Protocolo) FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero AND p1.ID_ProtocoloPai IS NOT NULL) AS Reencaminhado
			FROM TB_Protocolo P
			INNER JOIN TB_Usuario userDest ON userDest.ID_Usuario = P.ID_UsuarioDestinatario
			INNER JOIN TB_Usuario userRemet ON userRemet.ID_Usuario = P.ID_UsuarioRementente
			WHERE P.ID_ProtocoloPai IS NULL 
			-- LISTA PROTOCOLOS ENVIADOS OU ENCAMINHADOS PELO USUARIO
			AND (
				@P_CpfUsuario IS NULL OR 
				((SELECT FOO.CPF FROM (SELECT top 1 userRemet1.CPF as CPF
							FROM TB_Protocolo P1 
							INNER JOIN TB_Usuario userRemet1 ON userRemet1.ID_Usuario = P1.ID_UsuarioRementente
							WHERE P1.Numero = P.Numero
							ORDER BY P1.ID_Protocolo ASC 
						) AS FOO
						WHERE FOO.CPF = @P_CpfUsuario
					) IS NOT NULL
					OR @P_CpfUsuario IN (
						SELECT userRemet.CPF
						FROM TB_Protocolo P1 
							INNER JOIN TB_Usuario userRemet ON userRemet.ID_Usuario = P1.ID_UsuarioRementente
						WHERE P1.Numero = P.Numero)
				)
			)
			-- REMOVE PROTOCOLOS QUE ESTAO NA CAIXA DE ENTRADA DO USUARIO
			AND (@P_CpfUsuario IS NULL 
						OR P.Numero NOT IN (
							SELECT P.Numero FROM TB_Protocolo P
							WHERE P.ID_ProtocoloPai IS NULL 
							AND (SELECT FOO.CPF FROM (
											SELECT top 1 userDest1.CPF as CPF
												FROM TB_Protocolo P1 
												INNER JOIN TB_Usuario userDest1 ON userDest1.ID_Usuario = P1.ID_UsuarioDestinatario
												WHERE P1.Numero = P.Numero
												ORDER BY P1.ID_Protocolo DESC 
										) AS FOO
										WHERE FOO.CPF = @P_CpfUsuario
								) IS NOT NULL
						)
			)
			AND (
				(@P_NumProtocolo IS NULL OR P.Numero LIKE '%' +@P_NumProtocolo+ '%') AND
				(@P_txtDiscriminacao IS NULL OR P.NM_Descricao LIKE '%' + @P_txtDiscriminacao + '%' ) AND
				(@P_nomRemetente IS NULL OR (SELECT userRemet.Nome FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
					INNER JOIN TB_Usuario userRemet ON userRemet.ID_Usuario = P1.ID_UsuarioRementente
				) LIKE '%' + @P_nomRemetente + '%' ) AND
				(@P_nomDestinatario IS NULL OR (SELECT userDest.Nome FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
					INNER JOIN TB_Usuario userDest ON userDest.ID_Usuario = P1.ID_UsuarioDestinatario) LIKE '%' + @P_nomDestinatario + '%' ) AND
				(@P_dtaEnvio IS NULL OR CONVERT(VARCHAR(10), P.DT_DataEnvio, 103) LIKE '%' + @P_dtaEnvio + '%' ) AND
				(@P_dtaRecebimento IS NULL OR CONVERT(VARCHAR(10), P.DT_DataRecebimento, 103) LIKE '%' + @P_dtaRecebimento + '%' )
			)
		END
	ELSE
		BEGIN
			SELECT 
				P.ID_Protocolo AS ID, 
				P.Numero AS Numero, 
				P.NM_Descricao AS Descricao,
				P.ID_UsuarioRementente, 
				P.ID_UsuarioDestinatario, 
				(SELECT CONVERT (VARCHAR (10),P1.DT_DataEnvio,103) + ' ' + CONVERT (VARCHAR (8),P1.DT_DataEnvio,14) FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
				) AS DataEnvio,
				(SELECT CONVERT (VARCHAR (10),P1.DT_DataRecebimento,103) + ' ' + CONVERT (VARCHAR (8),P1.DT_DataRecebimento,14) FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
				) AS DataRecebimento,
				(SELECT userDest.Nome FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
					INNER JOIN TB_Usuario userDest ON userDest.ID_Usuario = P1.ID_UsuarioDestinatario
				) AS NomeDestinatario,
				(SELECT userRemet.Nome FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
					INNER JOIN TB_Usuario userRemet ON userRemet.ID_Usuario = P1.ID_UsuarioRementente
				) AS NomeRemetente,
				(SELECT COUNT(ID_Protocolo) FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero AND p1.ID_ProtocoloPai IS NOT NULL) AS Reencaminhado
			FROM TB_Protocolo P
			INNER JOIN TB_Usuario userDest ON userDest.ID_Usuario = P.ID_UsuarioDestinatario
			INNER JOIN TB_Usuario userRemet ON userRemet.ID_Usuario = P.ID_UsuarioRementente 
			WHERE P.ID_ProtocoloPai IS NULL 
			AND (
				@P_CpfUsuario IS NULL OR (
				SELECT FOO.CPF FROM (
					SELECT top 1
						userDest1.CPF as CPF
					FROM TB_Protocolo P1 
					INNER JOIN TB_Usuario userDest1 ON userDest1.ID_Usuario = P1.ID_UsuarioDestinatario
					WHERE P1.Numero = P.Numero
					ORDER BY P1.ID_Protocolo DESC 
				) AS FOO
				WHERE FOO.CPF = @P_CpfUsuario
			) IS NOT NULL)
			AND (
				(@P_NumProtocolo IS NULL OR P.Numero LIKE '%' +@P_NumProtocolo+ '%') AND
				(@P_txtDiscriminacao IS NULL OR P.NM_Descricao LIKE '%' + @P_txtDiscriminacao + '%' ) AND
				(@P_nomRemetente IS NULL OR (SELECT userRemet.Nome FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
					INNER JOIN TB_Usuario userRemet ON userRemet.ID_Usuario = P1.ID_UsuarioRementente
				) LIKE '%' + @P_nomRemetente + '%' ) AND
				(@P_nomDestinatario IS NULL OR (SELECT userDest.Nome FROM
					(SELECT TOP 1 * FROM TB_Protocolo p1 WHERE p1.Numero = P.Numero ORDER BY p1.ID_Protocolo DESC) P1
					INNER JOIN TB_Usuario userDest ON userDest.ID_Usuario = P1.ID_UsuarioDestinatario) LIKE '%' + @P_nomDestinatario + '%' ) AND
				(@P_dtaEnvio IS NULL OR CONVERT(VARCHAR(10), P.DT_DataEnvio, 103) LIKE '%' + @P_dtaEnvio + '%' ) AND
				(@P_dtaRecebimento IS NULL OR CONVERT(VARCHAR(10), P.DT_DataRecebimento, 103) LIKE '%' + @P_dtaRecebimento + '%' )
			)
			ORDER BY P.Numero ASC
	END
END
GO
