using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcConcluintesPorFormaAquisicao : ProcBase<DTOConcluintesPorFormaAquisicao>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOConcluintesPorFormaAquisicao> ObterConcluintesPorFormaAquisicao(int? ano, int? idUf)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_CONCLUINTES_FA_POR_ANO_UF");

                //Passa o id do usuário para o parâmetro da procedure                
                this.sqlCmd.Parameters.Add(new SqlParameter("@ANO", ano));
                if (idUf > 0) { this.sqlCmd.Parameters.Add(new SqlParameter("@ID_UF", idUf)); }

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOConcluintesPorFormaAquisicao> lstResult = new List<DTOConcluintesPorFormaAquisicao>();
                DTOConcluintesPorFormaAquisicao registro = null;

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

        private DTOConcluintesPorFormaAquisicao ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOConcluintesPorFormaAquisicao registro = new DTOConcluintesPorFormaAquisicao();

            registro.FormaAquisicao = dr["FormaAquisicao"].ToString();
            registro.Percentual = decimal.Parse(dr["Percentual"].ToString());

            return registro;
        }
    }
}
