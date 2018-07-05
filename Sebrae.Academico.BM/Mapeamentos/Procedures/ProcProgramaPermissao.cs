using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{

    /// <summary>
    /// Classe utilizada para mapear a procedure SP_ProgramaPermissao
    /// </summary>
    public sealed class ProcProgramaPermissao : ProcBaseDTO<DTOProgramaPermissao>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOProgramaPermissao.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário. Parâmetro da Procedure</param>
        /// <returns>Lista de objetos da classe DTOProgramaPermissao</returns>
        public IList<DTOProgramaPermissao> Executar(int idUsuario)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_ProgramaPermissao");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDUSUARIO", idUsuario));
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDPROGRAMA", decimal.Zero));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOProgramaPermissao> lstResult = new List<DTOProgramaPermissao>();
                DTOProgramaPermissao ofertaPermissao = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    ofertaPermissao = ObterObjetoDTO(dr);
                    lstResult.Add(ofertaPermissao);
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

        protected override DTOProgramaPermissao ObterObjetoDTO(IDataReader dr)
        {
            DTOProgramaPermissao programaPermissao = new DTOProgramaPermissao();

            if (dr["NU_Linha"] != DBNull.Value)
            {
                programaPermissao.NumeroLinha = int.Parse(dr["NU_Linha"].ToString());
            }

            this.PreencherObjetoProgramaPermissao(dr, programaPermissao);

            if (dr["Id_Usuario"] != DBNull.Value)
            {
                programaPermissao.IdUsuario = int.Parse(dr["Id_Usuario"].ToString());
            }

            return programaPermissao;
        }

        private void PreencherObjetoProgramaPermissao(IDataReader dr, DTOProgramaPermissao programaPermissao)
        {
            programaPermissao.Programa = new Programa()
            {
                ID = int.Parse(dr["ID_Programa"].ToString()),
                Ativo = bool.Parse(dr["IN_Ativo"].ToString())
            };

            if (dr["DT_UltimaAtualizacao"] != DBNull.Value)
            {
                programaPermissao.Programa.Auditoria.DataAuditoria = DateTime.Parse(dr["DT_UltimaAtualizacao"].ToString());
            }

            if (dr["NM_UsuarioAtualizacao"] != DBNull.Value)
            {
                programaPermissao.Programa.Auditoria.UsuarioAuditoria = dr["NM_UsuarioAtualizacao"].ToString();
            }

        }

        #endregion
    }
}
