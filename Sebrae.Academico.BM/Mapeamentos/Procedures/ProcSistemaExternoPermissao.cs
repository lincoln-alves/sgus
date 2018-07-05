using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    /// <summary>
    /// Classe utilizada para mapear a procedure SP_SistemaExternoPermissao
    /// </summary>
    public sealed class ProcSistemaExternoPermissao : ProcBaseDTO<DTOSistemaExternoPermissao>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOSistemaExternoPermissao.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário. Parâmetro da Procedure</param>
        /// <returns>Lista de objetos da classe DTOSistemaExternoPermissao</returns>
        public IList<DTOSistemaExternoPermissao> Executar(int idUsuario)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_SistemaExternoPermissao");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDUSUARIO", idUsuario));

                //Passa o id do sistema externo com o valor igual a zero no parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDSISTEMAEXTERNO", decimal.Zero));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOSistemaExternoPermissao> lstResult = new List<DTOSistemaExternoPermissao>();

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    lstResult.Add(ObterObjetoDTO(dr));
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

        #region "Métodos Protected"

        protected override DTOSistemaExternoPermissao ObterObjetoDTO(IDataReader dr)
        {
            DTOSistemaExternoPermissao sistemaExternoPermissao = new DTOSistemaExternoPermissao();


            if (dr["NU_Linha"] != null)
            {
                sistemaExternoPermissao.NumeroLinha = int.Parse(dr["NU_Linha"].ToString());
            }

            this.PreencherObjetoSistemaExternoPermissao(dr, sistemaExternoPermissao);

            if (dr["Id_Usuario"] != null)
            {
                sistemaExternoPermissao.IdUsuario = int.Parse(dr["Id_Usuario"].ToString());
            }

            return sistemaExternoPermissao;
        }

        private void PreencherObjetoSistemaExternoPermissao(IDataReader dr, DTOSistemaExternoPermissao sistemaExternoPermissao)
        {
            sistemaExternoPermissao.SistemaExterno = new SistemaExterno()
            {
                ID = int.Parse(dr["Id_SistemaExterno"].ToString()),
                Nome = dr["NM_SistemaExterno"].ToString(),
                LinkSistemaExterno = dr["LK_SistemaExterno"].ToString(),
                Descricao = dr["NM_Descricao"].ToString(),
                EnglishTown = dr["IN_EnglishTown"].ToString() == "True" ? true : (bool?) null,
                MesmaJanela =
                    dr["IN_MesmaJanela"] == null
                        ? null
                        : dr["IN_MesmaJanela"].ToString() == "True"
                                ? true
                                : (dr["IN_MesmaJanela"].ToString() == "False" ? false : (bool?) null)
            };

            sistemaExternoPermissao.SistemaExterno.Auditoria = new Auditoria();

            if (dr["DT_UltimaAtualizacao"] != DBNull.Value)
            {
                sistemaExternoPermissao.SistemaExterno.Auditoria.DataAuditoria = DateTime.Parse(dr["DT_UltimaAtualizacao"].ToString());
            }

            if (dr["NM_UsuarioAtualizacao"] != DBNull.Value)
            {
                sistemaExternoPermissao.SistemaExterno.Auditoria.UsuarioAuditoria = dr["NM_UsuarioAtualizacao"].ToString();
            }

        }

        #endregion
    }
}
