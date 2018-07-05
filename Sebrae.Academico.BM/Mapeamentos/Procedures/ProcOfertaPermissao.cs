using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{

    /// <summary>
    /// Classe utilizada para mapear a procedure SP_OfertaPermissao
    /// </summary>
    public sealed class ProcOfertaPermissao : ProcBaseDTO<DTOfertaPermissao>
    {

        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOfertaPermissao.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário. Parâmetro da Procedure</param>
        /// <returns>Lista de objetos da classe DTOfertaPermissao</returns>
        public IList<DTOfertaPermissao> Executar(int idUsuario)
        {
            try
            {
                AbrirConexao();

                DefinirNomeProcedure("SP_OfertaPermissao");

                //Passa o id do usuário para o parâmetro da procedure
                sqlCmd.Parameters.Add(new SqlParameter("@P_IDUSUARIO", idUsuario));
                sqlCmd.Parameters.Add(new SqlParameter("@P_IDOFERTA", decimal.Zero));

                //Executa a procedure e obtém os dados em um datareader.
                dr = sqlCmd.ExecuteReader();

                var lstResult = new List<DTOfertaPermissao>();

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    var ofertaPermissao = ObterObjetoDTO(dr);
                    lstResult.Add(ofertaPermissao);
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

        protected override DTOfertaPermissao ObterObjetoDTO(IDataReader dr)
        {
            DTOfertaPermissao ofertaPermissao = new DTOfertaPermissao();

            if (dr["NU_Linha"] != DBNull.Value)
            {
                ofertaPermissao.NumeroLinha = int.Parse(dr["NU_Linha"].ToString());
            }
            
            this.PreencherObjetoOfertaPermissao(dr, ofertaPermissao);

            if (dr["Id_Usuario"] != DBNull.Value)
            {
                ofertaPermissao.IdUsuario = int.Parse(dr["Id_Usuario"].ToString());
            }

            return ofertaPermissao;
        }

        private void PreencherObjetoOfertaPermissao(IDataReader dr, DTOfertaPermissao ofertaPermissao)
        {

            ofertaPermissao.IdOferta = int.Parse(dr["Id_Oferta"].ToString());
            ofertaPermissao.IdUsuario = int.Parse(dr["Id_Usuario"].ToString());
            ofertaPermissao.IdTipoOferta = int.Parse(dr["Id_TipoOferta"].ToString());
            if (!string.IsNullOrEmpty(dr["IN_MatriculaGestorUC"].ToString()))
            {
                ofertaPermissao.PermiteGestorUC = bool.Parse(dr["IN_MatriculaGestorUC"].ToString());
            }
            else
            {
                ofertaPermissao.PermiteGestorUC = false;
            }

            ofertaPermissao.IdSolucaoEducacional = int.Parse(dr["ID_SolucaoEducacional"].ToString());


            if (!string.IsNullOrEmpty(dr["DT_Inicio"].ToString()))
            {
                ofertaPermissao.DataInicio = DateTime.Parse(dr["DT_Inicio"].ToString());
            }

            if (!string.IsNullOrEmpty(dr["DT_Fim"].ToString()))
            {
                ofertaPermissao.DataFim = DateTime.Parse(dr["DT_Fim"].ToString());
            }
            if (!string.IsNullOrEmpty(dr["DT_InicioInscricoes"].ToString()))
            {
                ofertaPermissao.DataInicioInscricoes = DateTime.Parse(dr["DT_InicioInscricoes"].ToString());
            }

            if (!string.IsNullOrEmpty(dr["DT_FimInscricoes"].ToString()))
            {
                ofertaPermissao.DataFimInscricoes = DateTime.Parse(dr["DT_FimInscricoes"].ToString());
            }


            //ofertaPermissao.IdOferta = new Oferta()
            //{
            //    ID = int.Parse(dr["Id_Oferta"].ToString()),
            //    TipoOferta = (enumTipoOferta)int.Parse(dr["ID_TipoOferta"].ToString()),

            //    SolucaoEducacional = new SolucaoEducacional()
            //    {
            //        ID = int.Parse(dr["ID_SolucaoEducacional"].ToString())
            //    },

            //    IDChaveExterna = dr["ID_ChaveExterna"].ToString(),

            //    CargaHoraria = new CargaHoraria()
            //    {
            //        ID = int.Parse(dr["ID_CargaHoraria"].ToString())
            //    },

            //    Nome = dr["NM_Oferta"].ToString(),
            //    EmailResponsavel = dr["NM_EmailResponsavel"].ToString(),
            //    FiladeEspera = bool.Parse(dr["IN_FilaEspera"].ToString()),
            //    DataInicio = DateTime.Parse(dr["DT_Inicio"].ToString()),
            //};

            //if (!string.IsNullOrEmpty(dr["ID_CertificadoTemplate"].ToString()))
            //{
            //    ofertaPermissao.IdOferta.CertificadoTemplate = new CertificadoTemplate()
            //    {
            //        ID = int.Parse(dr["ID_CertificadoTemplate"].ToString())
            //    };
            //}

            //if (!string.IsNullOrEmpty(dr["DT_Fim"].ToString()))
            //{
            //    ofertaPermissao.IdOferta.DataFim = DateTime.Parse(dr["DT_Fim"].ToString());
            //}

            //if (!string.IsNullOrEmpty(dr["DT_InicioInscricoes"].ToString()))
            //{
            //    ofertaPermissao.IdOferta.DataInicioInscricoes = DateTime.Parse(dr["DT_InicioInscricoes"].ToString());
            //}

            //if (!string.IsNullOrEmpty(dr["DT_FimInscricoes"].ToString()))
            //{
            //    ofertaPermissao.IdOferta.DataFimInscricoes = DateTime.Parse(dr["DT_FimInscricoes"].ToString());
            //}
            //if (!string.IsNullOrEmpty(dr["QT_MaxInscricoes"].ToString()))
            //{
            //    ofertaPermissao.IdOferta.QuantidadeMaximaInscricoes = int.Parse(dr["QT_MaxInscricoes"].ToString());
            //}

        }

        #endregion
    }
}
