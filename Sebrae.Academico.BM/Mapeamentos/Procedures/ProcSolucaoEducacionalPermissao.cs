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
    /// Classe utilizada para mapear a procedure SP_SolucaoEducacionalPermissao
    /// </summary>
    public sealed class ProcSolucaoEducacionalPermissao : ProcBaseDTO<DTOSolucaoEducacionalPermissao>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOSolucaoEducacionalPermissao.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário. Parâmetro da Procedure</param>
        /// <param name="solucaoId">ID da SE</param>
        /// <returns>Lista de objetos da classe DTOSolucaoEducacionalPermissao</returns>
        public IList<DTOSolucaoEducacionalPermissao> Executar(int idUsuario, int solucaoId)
        {
            try
            {
                AbrirConexao();

                DefinirNomeProcedure("SP_SolucaoEducacionalPermissao");

                //Passa o id do usuário para o parâmetro da procedure
                sqlCmd.Parameters.Add(new SqlParameter("@P_IDUSUARIO", idUsuario));
                sqlCmd.Parameters.Add(new SqlParameter("@P_IDSOLUCAOEDUCACIONAL", solucaoId));

                //Executa a procedure e obtém os dados em um datareader.
                dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DTOSolucaoEducacionalPermissao>();

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    try
                    {
                        var solucaoEducacionalPermissao = ObterObjetoDTO(dr);
                        lstResult.Add(solucaoEducacionalPermissao);
                    }
                    catch
                    {
                    }
                }

                return lstResult;

            }
            catch (Exception)
            {
                throw new AcademicoException("Ocorreu um erro na execução da Solicitação");
            }
            finally
            {
                FecharConexao();
            }
        }

        #endregion

        #region "Métodos Protected"

        protected override DTOSolucaoEducacionalPermissao ObterObjetoDTO(IDataReader dr)
        {
            DTOSolucaoEducacionalPermissao solucaoEducacionalPermissao = new DTOSolucaoEducacionalPermissao();

            if (dr["NU_Linha"] != null)
            {
                solucaoEducacionalPermissao.NumeroLinha = int.Parse(dr["NU_Linha"].ToString());
            }

            this.PreencherObjetoSolucaoEducacional(dr, solucaoEducacionalPermissao);


            if (dr["Id_Usuario"] != null)
            {
                solucaoEducacionalPermissao.IdUsuario = int.Parse(dr["Id_Usuario"].ToString());
            }

            return solucaoEducacionalPermissao;
        }

        private void PreencherObjetoSolucaoEducacional(IDataReader dr, DTOSolucaoEducacionalPermissao solucaoEducacionalPermissao)
        {
            //Solucao Educacional
            solucaoEducacionalPermissao.SolucaoEducacional = new SolucaoEducacional()
            {
                ID = int.Parse(dr["Id_SolucaoEducacional"].ToString()),
                Fornecedor = new Fornecedor() { ID = int.Parse(dr["ID_Fornecedor"].ToString()) },
                FormaAquisicao = new FormaAquisicao() { ID = int.Parse(dr["ID_FormaAquisicao"].ToString()) },
                CategoriaConteudo = new CategoriaConteudo() { ID = int.Parse(dr["ID_CATEGORIACONTEUDO"].ToString()) },
            };

            if (dr["ID_ChaveExterna"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.IDChaveExterna = dr["ID_ChaveExterna"].ToString();
            }

            if (dr["NM_SolucaoEducacional"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.Nome = dr["NM_SolucaoEducacional"].ToString();
            }

            if (dr["TX_Ementa"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.Ementa = dr["TX_Ementa"].ToString();
            }

            if (dr["DT_Cadastro"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.DataCadastro = DateTime.Parse(dr["DT_Cadastro"].ToString());
            }

            if (dr["IN_TemMaterial"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.TemMaterial = bool.Parse(dr["IN_TemMaterial"].ToString());
            }

            //Autor
            if (dr["NM_Autor"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.Autor = dr["NM_Autor"].ToString();
            }

            //Apresentacao
            if (dr["TX_Apresentacao"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.Apresentacao = dr["TX_Apresentacao"].ToString();
            }

            //Objetivo
            if (dr["TX_Objetivo"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.Objetivo = dr["TX_Objetivo"].ToString();
            }

            //Ativo
            if (dr["IN_Ativo"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.Ativo = bool.Parse(dr["IN_Ativo"].ToString());
            }

            //iDNode
            if (dr["ID_Node"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.IdNode = int.Parse(dr["ID_Node"].ToString());
            }

            if (dr["DT_UltimaAtualizacao"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.Auditoria.DataAuditoria = DateTime.Parse(dr["DT_UltimaAtualizacao"].ToString());
            }

            if (dr["NM_UsuarioAtualizacao"] != DBNull.Value)
            {
                solucaoEducacionalPermissao.SolucaoEducacional.Auditoria.UsuarioAuditoria = dr["NM_UsuarioAtualizacao"].ToString();
            }
        }

        #endregion
    }
}
