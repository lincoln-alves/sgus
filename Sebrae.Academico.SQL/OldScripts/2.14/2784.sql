DECLARE @NAME NVARCHAR(MAX) = (SELECT TOP 1 NAME FROM sys.objects WHERE object_id = OBJECT_ID(N'SP_REL_DADOS_PESSOAIS') AND TYPE IN ( N'P', N'PC' ));
DECLARE @DROP_COMMAND NVARCHAR(MAX) = 'DROP PROCEDURE ' + @NAME;
IF @NAME IS NOT NULL BEGIN EXECUTE sp_executesql @DROP_COMMAND END
GO

-- =============================================
-- Author:		Edgar Froes
-- Create date: 16/09/2016
-- Description:	Buscar dados pessoais dos alunos.

-- Removido a nacionalizacao
-- Author: Jair Santos
-- Alterado: 14/07/2016
-- =============================================
CREATE PROCEDURE [dbo].[SP_REL_DADOS_PESSOAIS]
	-- Add the parameters for the stored procedure here
	@p_Nome varchar(200),
	@p_CPF varchar(30),
	@p_UF varchar(1000),
	@p_Nivel_Ocupacional varchar(1000),
	@p_Perfil varchar(1000)
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	  COALESCE(NULLIF(U.Nome,''),'--') as Nome,
      COALESCE(NULLIF(U.CPF,''),'--') as CPF,
      UF,
      NivelOcupacional,
      COALESCE(NULLIF(U.Matricula,''),'--') as Matricula,
      COALESCE(NULLIF(U.Situacao,''),'--') as Situacao,
      COALESCE(NULLIF(U.Email,''),'--') as Email,
      COALESCE(NULLIF(U.Unidade,''),'--') as Unidade,
      COALESCE(NULLIF(U.Endereco,''),'--') as Endereco,
      COALESCE(NULLIF(U.Bairro,''),'--') as Bairro,
      COALESCE(NULLIF(U.Cidade,''),'--') as Cidade,
      COALESCE(NULLIF(U.CEP,''),'--') as CEP,
      COALESCE(NULLIF(U.Estado,''),'--') as Estado,
      COALESCE(NULLIF(U.TelResidencial,''),'--') as TelResidencial,
      COALESCE(NULLIF(U.TelCelular,''),'--') as TelCelular,
      DataAdmissao,
      COALESCE(NULLIF(U.Sexo,''),'--') as Sexo,
      COALESCE(NULLIF(U.EstadoCivil,''),'--') as EstadoCivil,
      DataNascimento,
      COALESCE(NULLIF(U.Estado2,''),'--') as Estado2,
      COALESCE(NULLIF(U.Escolaridade,''),'--') as Escolaridade,
      UltimoAcesso,
      QtdAcessos,
      DataAtualizacao,
      DT_Insercao,
      SUBSTRING(perfil, 0, len(perfil)) Perfil
	FROM (
		SELECT
		MIN(U.Nome) as Nome,
		MIN(U.CPF) as CPF,
		MIN(UF.SG_UF) as UF,
		MIN(NI.NM_Nome) as NivelOcupacional,
		MIN(U.Matricula) as Matricula,
		MIN(U.Situacao) as Situacao,
		MIN(U.Email) as Email,
		MIN(U.Unidade) as Unidade,
		MIN(U.Endereco) as Endereco,
		MIN(U.Bairro) as Bairro,
		MIN(U.Cidade) Cidade,
		MIN(U.CEP) as CEP,
		MIN(U.Estado) as Estado,
		MIN(U.TelResidencial) as TelResidencial,
		MIN(U.TelCelular) as TelCelular, 
		MIN(U.DataAdmissao) as DataAdmissao,
		MIN(U.Sexo) as Sexo,
		MIN(U.EstadoCivil) as EstadoCivil,
		MIN(U.DataNascimento) as DataNascimento,
		MIN(U.Estado2) as Estado2,
		MIN(U.Escolaridade) as Escolaridade,
		MAX(LA.DT_Acesso) as UltimoAcesso,
		COUNT(LA.ID_Usuario) as QtdAcessos,
		MIN(U.DataAtualizacao) as DataAtualizacao,
		MIN(U.DT_Insercao) as DT_Insercao,
		(SELECT stuff( (SELECT p.NM_Perfil + ', ' FROM TB_Perfil p JOIN TB_UsuarioPerfil up ON up.ID_Perfil = p.ID_Perfil WHERE up.ID_Usuario = U.ID_Usuario  FOR XML PATH('')),1,0,'')) perfil
	
		FROM TB_Usuario U
		INNER JOIN TB_UF UF ON U.ID_UF = UF.ID_UF
		INNER JOIN TB_NivelOcupacional NI ON U.ID_NivelOcupacional = NI.ID_NivelOcupacional
		INNER JOIN TB_UsuarioPerfil UP ON U.ID_Usuario = UP.ID_Usuario
		INNER JOIN TB_Perfil P ON UP.ID_Perfil = P.ID_Perfil
		LEFT OUTER JOIN LG_Acesso LA ON U.ID_Usuario = LA.ID_Usuario

		WHERE
		(@p_Nome IS NULL OR U.Nome LIKE '%' + @p_Nome + '%') AND
		(@p_CPF IS NULL OR U.CPF LIKE @p_CPF) AND
		((@p_UF IS NULL OR LEN(@p_UF) = 0) OR UF.ID_UF IN (SELECT Data FROM FN_Split(@p_UF))) AND
		((@p_Nivel_Ocupacional IS NULL OR LEN(@p_Nivel_Ocupacional) = 0) OR U.ID_NivelOcupacional IN (SELECT Data FROM FN_Split(@p_Nivel_Ocupacional)))	AND
		((@p_Perfil IS NULL OR LEN(@p_Perfil) = 0) OR up.ID_Perfil IN (SELECT Data FROM FN_Split(@p_Perfil)))
	 

	GROUP BY U.ID_Usuario) u

	ORDER BY u.Nome
END

