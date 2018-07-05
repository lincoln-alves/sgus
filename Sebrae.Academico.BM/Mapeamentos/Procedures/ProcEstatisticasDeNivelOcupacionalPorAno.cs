using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{


    public sealed class ProcEstatisticasDeNivelOcupacionalPorAno : ProcBase<DTOEstatisticaNivelOcupacional>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOEstatisticaNivelOcupacional.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOEstatisticaNivelOcupacional</returns>
        public IList<DTOEstatisticaNivelOcupacional> ObterParticipacaoProporcionalAoNumeroDeFuncionarios(int? ano, int? idUf)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_ObterEstatisticasDeNivelOcupacionalPorAnoUf");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ANO", ano));
                this.sqlCmd.Parameters.Add(new SqlParameter("@ID_UF", idUf));
                 
                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOEstatisticaNivelOcupacional> lstResult = new List<DTOEstatisticaNivelOcupacional>();
                DTOEstatisticaNivelOcupacional matriculaOfertaNoAno = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    matriculaOfertaNoAno = ObterObjetoDTO(dr);
                    lstResult.Add(matriculaOfertaNoAno);
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

        private DTOEstatisticaNivelOcupacional ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOEstatisticaNivelOcupacional estatisticaNivelOcupacional = new DTOEstatisticaNivelOcupacional();

            estatisticaNivelOcupacional.NomeNivelOcupacional = dr["NomeNivelOcupacional"].ToString();
            estatisticaNivelOcupacional.QuantidadeAtivos = int.Parse(dr["QTDAtivos"].ToString());
            estatisticaNivelOcupacional.QuantidadeAtivosComInscricao = int.Parse(dr["QTDAtivosComInscricao"].ToString());
            estatisticaNivelOcupacional.Percentual = double.Parse(dr["Percentual"].ToString());
            
            return estatisticaNivelOcupacional;
        }


    }
}
