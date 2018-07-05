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
    /// Classe utilizada para mapear a procedure SP_TrilhaNivelPermissao
    /// </summary>
    public sealed class ProcTrilhaNivelPermissao : ProcBaseDTO<DTOTrilhaNivelPermissao>
    {
        public IList<DTOTrilhaNivelPermissao> Executar(int idUsuario,bool superAcesso = false)
        {
            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_TrilhaNivelPermissao");

                //Passa o id do usuário para o parâmetro da procedure
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDUSUARIO", idUsuario));
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDTRILHANIVEL", decimal.Zero));
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_SuperAcesso", superAcesso ? 1 : 0));

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOTrilhaNivelPermissao> lstResult = new List<DTOTrilhaNivelPermissao>();
                DTOTrilhaNivelPermissao trilhaNivelPermissao = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    trilhaNivelPermissao = ObterObjetoDTO(dr);
                    lstResult.Add(trilhaNivelPermissao);
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

        protected override DTOTrilhaNivelPermissao ObterObjetoDTO(IDataReader dr)
        {
            DTOTrilhaNivelPermissao trilhaNivelPermissao = new DTOTrilhaNivelPermissao();

            if (dr["NU_Linha"] != DBNull.Value)
            {
                trilhaNivelPermissao.NumeroLinha = int.Parse(dr["NU_Linha"].ToString());
            }

            this.PreencherObjetoTrilhaNivelPermissao(dr, trilhaNivelPermissao);

            if (dr["Id_Usuario"] != DBNull.Value)
            {
                trilhaNivelPermissao.IdUsuario = int.Parse(dr["Id_Usuario"].ToString());
            }

            return trilhaNivelPermissao;
        }

        private void PreencherObjetoTrilhaNivelPermissao(IDataReader dr, DTOTrilhaNivelPermissao trilhaNivelPermissao)
        {
            trilhaNivelPermissao.TrilhaNivel = new TrilhaNivel()
            {
                ID = int.Parse(dr["ID_TrilhaNivel"].ToString()),
                Trilha = new Trilha() { ID = int.Parse(dr["ID_Trilha"].ToString()) }
            };

            if (dr["ID_TrilhaNivelPreReq"] != DBNull.Value)
            {
                trilhaNivelPermissao.TrilhaNivel.PreRequisito = new TrilhaNivel() { ID = int.Parse(dr["ID_TrilhaNivelPreReq"].ToString()) };
            }

            if (dr["ID_CertificadoTemplate"] != DBNull.Value)
            {
                trilhaNivelPermissao.TrilhaNivel.CertificadoTemplate = new CertificadoTemplate()
                {
                    ID = int.Parse(dr["ID_CertificadoTemplate"].ToString())
                };
            };

            //Carga Horaria
            trilhaNivelPermissao.TrilhaNivel.CargaHoraria = int.Parse(dr["QT_CargaHoraria"].ToString());

            //Nome da Trilha Nivel
            trilhaNivelPermissao.TrilhaNivel.Nome = dr["NM_TrilhaNivel"].ToString();

            //Quantidade de dias de Prazo
            trilhaNivelPermissao.TrilhaNivel.QuantidadeDiasPrazo = int.Parse(dr["QT_DiasPrazo"].ToString());

            //Texto do termo de aceite
            if (dr["TX_TermoAceite"] != DBNull.Value)
            {
                trilhaNivelPermissao.TrilhaNivel.TextoTermoAceite = dr["TX_TermoAceite"].ToString();
            }

            //Valor da Nota Mínima
            if (dr["VL_NotaMinima"] != DBNull.Value)
            {
                trilhaNivelPermissao.TrilhaNivel.NotaMinima = decimal.Parse(dr["VL_NotaMinima"].ToString());
            }

            //Aceita Novas Matriculas ?
            if (dr["IN_AceitaNovasMatriculas"] != DBNull.Value)
            {
                trilhaNivelPermissao.TrilhaNivel.AceitaNovasMatriculas = bool.Parse(dr["IN_AceitaNovasMatriculas"].ToString());
            }

            //Valor Ordem
            trilhaNivelPermissao.TrilhaNivel.ValorOrdem = byte.Parse(dr["VL_Ordem"].ToString());

            if (dr["DT_UltimaAtualizacao"] != DBNull.Value)
            {
                trilhaNivelPermissao.TrilhaNivel.Auditoria.DataAuditoria = DateTime.Parse(dr["DT_UltimaAtualizacao"].ToString());
            }

            if (dr["NM_UsuarioAtualizacao"] != DBNull.Value)
            {
                trilhaNivelPermissao.TrilhaNivel.Auditoria.UsuarioAuditoria = dr["NM_UsuarioAtualizacao"].ToString();
            }
        }
    }
}
