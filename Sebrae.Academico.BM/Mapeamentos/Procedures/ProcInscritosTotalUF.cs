using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcInscritosTotalUF : ProcBase<DTOInscritosTotalUF>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOInscritosTotalUF.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOTotalMatriculaOfertaPorAno</returns>
        public IList<DTOInscritosTotalUF> ObterTotalMatriculasNoAnoPorUf(int? ano)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_REL_INSCRITOS_TOTAL_UF");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ANO", ano));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOInscritosTotalUF> lstResult = new List<DTOInscritosTotalUF>();
                DTOInscritosTotalUF dtoResultado = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    dtoResultado = ObterObjetoDTO(dr);
                    lstResult.Add(dtoResultado);
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

        private DTOInscritosTotalUF ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOInscritosTotalUF resultado = new DTOInscritosTotalUF();

            resultado.UF = dr["SG_UF"].ToString();
            resultado.QuantidadeInscritos = int.Parse(dr["Inscritos"].ToString());
            resultado.QuantidadeTotal = int.Parse(dr["Total"].ToString());

            return resultado;
        }
    }
}
