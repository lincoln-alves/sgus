using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcCertificadosPorNivelOperacional : ProcBase<DTOCertificadosPorNivelOperacional>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOCertificadosPorNivelOperacional> ObterCertificadosPorNivelOperacional()
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_OBTER_QUANTIDADE_DE_CERTIFICADOS_POR_NIVEL_OPERACIONAL");

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOCertificadosPorNivelOperacional> lstResult = new List<DTOCertificadosPorNivelOperacional>();
                DTOCertificadosPorNivelOperacional registro = null;

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

        private DTOCertificadosPorNivelOperacional ObterObjetoDTO(System.Data.IDataReader dr)
        {
            DTOCertificadosPorNivelOperacional registro = new DTOCertificadosPorNivelOperacional();

            registro.Nome= dr["NM_Nome"].ToString();
            registro.Certificados = int.Parse(dr["Certificacoes"].ToString());
            registro.Quantidade = int.Parse(dr["Quantidade"].ToString());

            return registro;
        }
    }
}
