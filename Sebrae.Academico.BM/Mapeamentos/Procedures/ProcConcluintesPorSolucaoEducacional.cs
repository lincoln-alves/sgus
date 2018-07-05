using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcConcluintesPorSolucaoEducacional : ProcBase<DTOConcluintesPorSolucaoEducacional>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOConcluintesPorSolucaoEducacional> ObterConcluintesPorSolucaoEducacional(int ano = 0)
        {

            try
            {
                base.AbrirConexao();

                if (ano > 0)
                {
                    Dictionary<string, object> lista = new Dictionary<string, object>();
                    lista.Add("ANO", ano);
                    return this.ExecutarProcedure("SP_CONCLUINTES_POR_SOLUCAO_EDUCACIONAL", lista);      
                }
                else
                {
                    this.DefinirNomeProcedure("SP_CONCLUINTES_POR_SOLUCAO_EDUCACIONAL");
                }

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOConcluintesPorSolucaoEducacional> lstResult = new List<DTOConcluintesPorSolucaoEducacional>();
                DTOConcluintesPorSolucaoEducacional registro = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    registro = ObterObjetoDTO(dr);
                    lstResult.Add(registro);
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

        #endregion

        private DTOConcluintesPorSolucaoEducacional ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOConcluintesPorSolucaoEducacional registro = new DTOConcluintesPorSolucaoEducacional();

            registro.Concluintes = int.Parse(dr["Concluintes"].ToString());
            registro.SolucaoEducacional = dr["SolucaoEducacional"].ToString();

            return registro;
        }
    }
}
