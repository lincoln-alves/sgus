using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcInscritosPorQuadroTotal : ProcBase<DTOInscritosPorQuadroTotal>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOInscritosPorQuadroTotal> ObterInscritosPorQuadroTotal()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_OBTER_INSCRITOS_POR_QUADRO_TOTAL");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOInscritosPorQuadroTotal> lstResult = new List<DTOInscritosPorQuadroTotal>();
                DTOInscritosPorQuadroTotal registro = null;

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

        private DTOInscritosPorQuadroTotal ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOInscritosPorQuadroTotal registro = new DTOInscritosPorQuadroTotal();

            registro.UF = dr["SG_UF"].ToString();
            registro.Inscritos = int.Parse(dr["Inscritos"].ToString());
            registro.QuadroTotal = int.Parse(dr["Total"].ToString());

            return registro;
        }
    }
}
