using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{

    /// <summary>
    /// Classe utilizada para gerar dados referentes aos relat[orios 
    /// </summary>
    public sealed class ProcRelatoriosMaisAcessados : ProcBase<DTOGeracaoRelatorio>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOGeracaoRelatorio.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário. Parâmetro da Procedure</param>
        /// <returns>Lista de objetos da classe DTOGeracaoRelatorio</returns>
        public IList<DTOGeracaoRelatorio> ListarRelatoriosMaisAcessadosPorUsuario(int idUsuario)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_RELATORIOS_MAIS_ACESSOS_POR_USUARIO");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@ID_USUARIO", idUsuario));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOGeracaoRelatorio> lstResult = new List<DTOGeracaoRelatorio>();
                DTOGeracaoRelatorio geracaoRelatorio = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    geracaoRelatorio = ObterObjetoDTO(dr);
                    lstResult.Add(geracaoRelatorio);
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

        private DTOGeracaoRelatorio ObterObjetoDTO(System.Data.IDataReader dr)
        {
         
            DTOGeracaoRelatorio geracaoRelatorio = new  DTOGeracaoRelatorio();

            //geracaoRelatorio.IdRelatorio = int.Parse(dr["ID_RELATORIO"].ToString());
            geracaoRelatorio.NomeRelatorio = dr["NM_Relatorio"].ToString();
            geracaoRelatorio.LinkRelatorio = dr["LK_Relatorio"].ToString();
            geracaoRelatorio.Qtd = int.Parse(dr["QTD"].ToString());

            return geracaoRelatorio;
        }


    }
}
