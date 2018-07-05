using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{

    /// <summary>
    /// Classe utilizada para gerar dados referentes aos logs de acessos às funcionalidades.
    /// </summary>
    public sealed class ProcLogAcessoFuncionalidade : ProcBase<DTOFuncionalidade>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOFuncionalidade.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário. Parâmetro da Procedure</param>
        /// <returns>Lista de objetos da classe DTOFuncionalidade</returns>
        public IList<DTOFuncionalidade> ListarFuncionalidadesMaisAcessadasPorUsuario(int idUsuario)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_FUNCIONALIDADES_MAIS_ACESSOS_POR_USUARIO");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ID_USUARIO", idUsuario));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOFuncionalidade> lstResult = new List<DTOFuncionalidade>();
                DTOFuncionalidade funcionalidade = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    funcionalidade = ObterObjetoDTO(dr);
                    lstResult.Add(funcionalidade);
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

        private DTOFuncionalidade ObterObjetoDTO(System.Data.IDataReader dr)
        {

            DTOFuncionalidade funcionalidade = new DTOFuncionalidade();

            funcionalidade.NomeFuncionalidade = dr["NM_Funcionalidade"].ToString();
            funcionalidade.LinkFuncionalidade = dr["LK_Funcionalidade"].ToString();
            funcionalidade.Qtd = int.Parse(dr["QTD"].ToString());

            return funcionalidade;
        }


    }
}
