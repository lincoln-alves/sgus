using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcCertificadosPorNaoCertificados : ProcBase<DTOCertificadosPorNaoCertificados>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOCertificadosPorNaoCertificados> ObterCertificadosPorNaoCertificados()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_OBTER_CERTIFICADOS_POR_NAO_CERTIFICADOS");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOCertificadosPorNaoCertificados> lstResult = new List<DTOCertificadosPorNaoCertificados>();
                DTOCertificadosPorNaoCertificados registro = null;

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

        private DTOCertificadosPorNaoCertificados ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOCertificadosPorNaoCertificados registro = new DTOCertificadosPorNaoCertificados();

            registro.Certificado = int.Parse(dr["Certificado"].ToString());
            registro.NaoCertificado = int.Parse(dr["NaoCertificado"].ToString());

            return registro;
        }
    }
}
