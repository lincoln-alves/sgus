using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{

    /// <summary>
    /// Classe utilizada para gerar dados referentes a tabela de notificacoes
    /// </summary>
    public sealed class ProcNotificacoes : ProcBase<DTONotificacao>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTONotificacao.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário. Parâmetro da Procedure</param>
        /// <returns>Lista de objetos da classe DTONotificacao</returns>
        public IList<DTONotificacao> ListaNotificacoesMaisAcessadasPorUsuario(int idUsuario)
        {
            try
            {
                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_NOTIFICACOES_NAO_VISUALIZADAS_POR_USUARIO");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ID_USUARIO", idUsuario));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTONotificacao> lstResult = new List<DTONotificacao>();
                DTONotificacao notificacao = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    notificacao = ObterObjetoDTO(dr);
                    lstResult.Add(notificacao);
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

        private DTONotificacao ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTONotificacao notificacao = new DTONotificacao();

            notificacao.TextoNotificacao = dr["TX_Notificacao"].ToString();
            notificacao.DataNotificacao = DateTime.Parse(dr["DT_Notificacao"].ToString());
            notificacao.Visualizado = bool.Parse(dr["IN_Visualizado"].ToString());

            return notificacao;
        }


    }
}
