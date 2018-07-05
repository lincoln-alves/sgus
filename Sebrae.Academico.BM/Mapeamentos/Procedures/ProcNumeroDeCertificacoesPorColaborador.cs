using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcNumeroDeCertificacoesPorColaborador : ProcBase<DTONumeroDeCertificacoesPorColaborador>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTONumeroDeCertificacoesPorColaborador> ObterNumeroDeCertificacoesPorColaborador()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_OBTER_NUMERO_DE_CERTIFICACOES_POR_COLABORADOR");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTONumeroDeCertificacoesPorColaborador> lstResult = new List<DTONumeroDeCertificacoesPorColaborador>();
                DTONumeroDeCertificacoesPorColaborador registro = null;

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

        private DTONumeroDeCertificacoesPorColaborador ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTONumeroDeCertificacoesPorColaborador registro = new DTONumeroDeCertificacoesPorColaborador();

            registro.Certificacoes = int.Parse(dr["Certificacoes"].ToString());
            registro.Quantidade = int.Parse(dr["Quantidade"].ToString());

            return registro;
        }
    }
}
