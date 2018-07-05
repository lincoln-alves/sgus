using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcTermoAceite : ProcBase<DTOTermoAceite>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOTermoAceite> ObterTermoAdesao()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_OBTER_TERMOACEITE_DE_SOLUCACAOEDUCACIONAL");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOTermoAceite> lstResult = new List<DTOTermoAceite>();
                DTOTermoAceite registro = null;

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

        private DTOTermoAceite ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOTermoAceite registro = new DTOTermoAceite();

            registro.ID = int.Parse(dr["ID_TermoAceiteCategoriaConteudo"].ToString());
            registro.Nome = dr["NM_TermoAceite"].ToString();
            registro.TextoPoliticaConsequencia = dr["TX_TermoAceite"].ToString();
            registro.TextoTermoAceite = dr["TX_PoliticaConsequencia"].ToString();
            registro.IdTermoAceite = int.Parse(dr["ID_TermoAceite"].ToString());

            int idSolucaoEducacional = 0;
            int.TryParse(dr["ID_SolucaoEducacional"].ToString(), out idSolucaoEducacional);
            registro.IdSolucaoEducacional = idSolucaoEducacional;
            registro.IdCategoriaConteudo = int.Parse(dr["ID_CategoriaConteudo"].ToString());

            return registro;
        }
    }
}
