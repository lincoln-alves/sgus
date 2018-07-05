using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public sealed class ProcPerfilDoPublicAtendido : ProcBase<DTOPerfilDoPublicoAtendido>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOMatriculasPorMes.
        /// </summary>
        /// <param name="ano">Ano</param>
        /// <param name="idUf">Uf</param>
        /// <returns>Lista de objetos da classe DTOMatriculaOfertaNoAno</returns>
        public IList<DTOPerfilDoPublicoAtendido> ObterPerfilDoPublicoAtendido(int ano = 0)
        {
            try
            {
                base.AbrirConexao();

                if (ano > 0)
                {
                    Dictionary<string, object> lista = new Dictionary<string, object>();
                    lista.Add("ANO", ano);
                    return this.ExecutarProcedure("SP_OBTER_PERFIL_DO_PUBLICO_ATENDIDO", lista);
                }
                else
                {
                    this.DefinirNomeProcedure("SP_OBTER_PERFIL_DO_PUBLICO_ATENDIDO");
                }

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOPerfilDoPublicoAtendido> lstResult = new List<DTOPerfilDoPublicoAtendido>();
                DTOPerfilDoPublicoAtendido registro = null;

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

        private DTOPerfilDoPublicoAtendido ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOPerfilDoPublicoAtendido registro = new DTOPerfilDoPublicoAtendido();

            registro.Quantidade = int.Parse(dr["Quantidade"].ToString());
            registro.Perfil = dr["Perfil"].ToString();

            return registro;
        }
    }
}
