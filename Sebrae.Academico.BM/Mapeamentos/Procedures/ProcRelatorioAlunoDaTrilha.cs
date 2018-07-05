using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    /// <summary>
    /// Classe utilizada para gerar dados referentes ao relatório de aluno da trilha
    /// </summary>
    public sealed class ProcRelatorioAlunoDaTrilha : ProcBase<DTOAlunoDaTrilha>
    {

        public IList<DTOAlunoDaTrilha> ListarRelatorioDoAlunoDaTrilha(int idUsuarioTrilha)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_REL_ALUNO_TRILHA");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ID_UsuarioTrilha", idUsuarioTrilha));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOAlunoDaTrilha> lstResult = new List<DTOAlunoDaTrilha>();
                DTOAlunoDaTrilha alunoDaTrilha = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    alunoDaTrilha = ObterObjetoDTO(dr);
                    lstResult.Add(alunoDaTrilha);
                }

                return lstResult;

            }
            catch 
            {
                throw new AcademicoException("Ocorreu um erro na execução da Solicitação");
            }
            finally
            {
                base.FecharConexao();
            }
        }

        private DTOAlunoDaTrilha ObterObjetoDTO(System.Data.IDataReader dr)
        {
            DTOAlunoDaTrilha alunoDaTrilha = new DTOAlunoDaTrilha();

            alunoDaTrilha.IdTopicoTematico = int.Parse(dr["ID_TrilhaTopicoTematico"].ToString());
            alunoDaTrilha.TopicoTematico = dr["NomeTopicoTematico"].ToString();
            alunoDaTrilha.NomeItemTrilha = dr["NomeSolucao"].ToString();
            enumSimNao autoIndicativa = (enumSimNao)int.Parse(dr["EhAutoIndivativa"].ToString());
            //Enum.Parse(typeof(enumSimNao), int.Parse(dr["EhAutoIndivativa"].ToString()));

            if (autoIndicativa.Equals(enumSimNao.Sim))
            {
                alunoDaTrilha.IsAutoIndicativa = true;
            }
            else if (autoIndicativa.Equals(enumSimNao.Nao))
            {
                alunoDaTrilha.IsAutoIndicativa = false;
            }

            enumSimNao temParticipacaoNaSolucao = (enumSimNao)int.Parse(dr["TemParticipacaoNaSolucao"].ToString());

            if (temParticipacaoNaSolucao.Equals(enumSimNao.Sim))
            {
                alunoDaTrilha.TemParticipacaoNaSolucao = true;
            }
            else if (temParticipacaoNaSolucao.Equals(enumSimNao.Nao))
            {
                alunoDaTrilha.TemParticipacaoNaSolucao = false;
            }

            enumSimNao temParticipacaoNoTopico = (enumSimNao)int.Parse(dr["TemParticipacaoNoTopico"].ToString());

            if (temParticipacaoNoTopico.Equals(enumSimNao.Sim))
            {
                alunoDaTrilha.TemParticipacaoNoTopico = true;
            }
            else if (temParticipacaoNoTopico.Equals(enumSimNao.Nao))
            {
                alunoDaTrilha.TemParticipacaoNoTopico = false;
            }
            
            //Informações do usuário Trilha - Início
            
            if (dr["TRILHA"] != DBNull.Value)
            {
                alunoDaTrilha.Trilha = dr["TRILHA"].ToString();
            }

            if (dr["NOMETRILHANIVEL"] != DBNull.Value)
            {
                alunoDaTrilha.Nivel = dr["NOMETRILHANIVEL"].ToString();
            }

            if (dr["STATUSMATRICULA"] != DBNull.Value)
            {
                alunoDaTrilha.StatusMatricula = dr["STATUSMATRICULA"].ToString();
            }

            if (dr["ID_TrilhaTopicoTematico"] != DBNull.Value)
            {
                alunoDaTrilha.IdTopicoTematico = int.Parse(dr["ID_TrilhaTopicoTematico"].ToString());
            }

            if (dr["DT_Inicio"] != DBNull.Value)
            {
                alunoDaTrilha.DataInicio = DateTime.Parse(dr["DT_Inicio"].ToString());
            }

            if (dr["DT_Limite"] != DBNull.Value)
            {
                alunoDaTrilha.DataLimite = DateTime.Parse(dr["DT_Limite"].ToString());
            }

            if (dr["UF"] != DBNull.Value)
            {
                alunoDaTrilha.UF = dr["UF"].ToString();
            }

            if (dr["NIVELOCUPACIONAL"] != DBNull.Value)
            {
                alunoDaTrilha.NivelOcupacional = dr["NIVELOCUPACIONAL"].ToString();
            }

            if (dr["NOMETOPICOTEMATICO"] != DBNull.Value)
            {
                alunoDaTrilha.TopicoTematico = dr["NOMETOPICOTEMATICO"].ToString();
            }

            if (dr["NOMEUSUARIO"] != DBNull.Value)
            {
                alunoDaTrilha.Nome = dr["NOMEUSUARIO"].ToString();
            }
            
            if (dr["DE_OBJETIVO"] != DBNull.Value)
            {
                alunoDaTrilha.Objetivo = dr["DE_OBJETIVO"].ToString();
            }
           
            //Informações do usuário Trilha -Fim


            return alunoDaTrilha;
        }



    }
}
