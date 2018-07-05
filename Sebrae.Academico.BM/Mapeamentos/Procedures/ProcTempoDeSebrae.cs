using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcTempoDeSebrae : ProcBase<DTOTempoDeSebrae>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOTempoDeSebrae> ObterTempoDeSebrae()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_OBTER_TEMPO_DE_SEBRAE");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOTempoDeSebrae> lstResult = new List<DTOTempoDeSebrae>();
                DTOTempoDeSebrae registro = null;

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

        private DTOTempoDeSebrae ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOTempoDeSebrae registro = new DTOTempoDeSebrae();

            registro.Tempo = int.Parse(dr["Tempo"].ToString());
            registro.Quantidade = int.Parse(dr["Quantidade"].ToString());

            return registro;
        }
    }
}
