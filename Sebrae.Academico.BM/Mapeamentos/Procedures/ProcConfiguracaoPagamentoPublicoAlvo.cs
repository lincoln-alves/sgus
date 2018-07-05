using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO.Filtros;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public class ProcConfiguracaoPagamentoPublicoAlvo : ProcBaseDTO<DTOConfiguracaoPagamentoPublicoAlvo>
    {
        #region "Métodos Públicos"

        /// <summary>
        /// Executa a procedure e obtém uma lista de DTO's do tipo DTOConfiguracaoPagamentoPublicoAlvo.
        /// </summary>
        /// <param name="idUsuario">Id do Usuário. Parâmetro da Procedure</param>
        /// <returns>Lista de objetos da classe DTOConfiguracaoPagamentoPublicoAlvo</returns>
        public IList<DTOConfiguracaoPagamentoPublicoAlvo> BuscarPorFiltro(ConfiguracaoPagamentoDTOFiltro filtro)
        {

            try
            {

                base.AbrirConexao();

                this.DefinirNomeProcedure("SP_ConfiguracaoPagamentoPublicoAlvo");

                //Passa o id do usuário para o parâmetro da procedure

                //Configuração Pagamento
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDUSUARIO", filtro.IdUsuario));
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDCONFIGURACAOPAGAMENTO", filtro.IdConfiguracaoPagamento));
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDUF", filtro.IdUF));
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDPERFIL", filtro.IdPerfil));
                this.sqlCmd.Parameters.Add(new SqlParameter("@P_IDNIVELOCUPACIONAL", filtro.IdNivelOcupacional));

                if (!string.IsNullOrWhiteSpace(filtro.NomeUsuario))
                {
                    this.sqlCmd.Parameters.Add(new SqlParameter("@P_NOME", filtro.NomeUsuario));
                }
                else
                {
                    this.sqlCmd.Parameters.Add(new SqlParameter("@P_NOME", DBNull.Value));
                }

                //Executa a procedure e obtém os dados em um datareader.
                this.dr = sqlCmd.ExecuteReader();

                IList<DTOConfiguracaoPagamentoPublicoAlvo> lstResult = new List<DTOConfiguracaoPagamentoPublicoAlvo>();
                DTOConfiguracaoPagamentoPublicoAlvo configuracaoPagamentoPublicoAlvo = null;

                //Faz o mapeamento objeto Relacional
                while (dr.Read())
                {
                    configuracaoPagamentoPublicoAlvo = ObterObjetoDTO(dr);
                    lstResult.Add(configuracaoPagamentoPublicoAlvo);
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

        protected override DTOConfiguracaoPagamentoPublicoAlvo ObterObjetoDTO(IDataReader dr)
        {
            DTOConfiguracaoPagamentoPublicoAlvo configuracaoPagamentoPublicoAlvo = new DTOConfiguracaoPagamentoPublicoAlvo();

            if (dr["NU_Linha"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.NumeroLinha = int.Parse(dr["NU_Linha"].ToString());
            }

            this.PreencherObjetoConfiguracaoPagamento(dr, configuracaoPagamentoPublicoAlvo);


            if (dr["Id_Usuario"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.IdUsuario = int.Parse(dr["Id_Usuario"].ToString());
            }

            return configuracaoPagamentoPublicoAlvo;
        }

        private void PreencherObjetoConfiguracaoPagamento(IDataReader dr, DTOConfiguracaoPagamentoPublicoAlvo configuracaoPagamentoPublicoAlvo)
        {
            //Solucao Educacional
            configuracaoPagamentoPublicoAlvo.ConfiguracaoPagamento = new ConfiguracaoPagamento()
            {
                ID = int.Parse(dr["ID_ConfiguracaoPagamento"].ToString()),
                TipoPagamento = new TipoPagamento() { ID = int.Parse(dr["ID_TipoPagamento"].ToString()) },
                DataInicioCompetencia = DateTime.Parse(dr["DT_InicioCompetencia"].ToString()),
                DataFimCompetencia = DateTime.Parse(dr["DT_FimCompetencia"].ToString()),
                ValorAPagar = decimal.Parse(dr["VL_ValorAPagar"].ToString()),
                BloqueiaAcesso = bool.Parse(dr["IN_BloqueiaAcesso"].ToString()),
                Recursiva = bool.Parse(dr["IN_Recursiva"].ToString()),
                QuantidadeDiasValidade = int.Parse(dr["QT_DiasValidade"].ToString()),
                QuantidadeDiasRenovacao = int.Parse(dr["QT_DiasRenovacao"].ToString()),
                QuantidadeDiasInadimplencia = int.Parse(dr["QT_DiasInadimplencia"].ToString()),
                QuantidadeDiasPagamento = int.Parse(dr["QT_DiasPagamento"].ToString()),
                Ativo = bool.Parse(dr["IN_Ativo"].ToString()),
                Nome = dr["NM_ConfiguracaoPagamento"].ToString(),
                TextoTermoAdesao = dr["TX_TermoAdesao"].ToString()
            };


            //Informações do usuário - Inicio


            //Nome
            if (dr["Nome"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.NomeUsuario = dr["Nome"].ToString();
            }

            //Endereço
            if (dr["Endereco"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.EnderecoUsuario = dr["Endereco"].ToString();
            }

            //Cidade
            if (dr["Cidade"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.CidadeDoUsuario = dr["Cidade"].ToString();
            }

            //Cep
            if (dr["Cep"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.CepDoUsuario = dr["Cep"].ToString();
            }

            //ID da Uf do Usuario
            if (dr["ID_UF"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.IDUfDoUsuario = int.Parse(dr["ID_UF"].ToString());
            }

            //Id do Nível Ocupacional
            if (dr["ID_NivelOcupacional"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.IDNivelOcupacional = int.Parse(dr["ID_NivelOcupacional"].ToString());
            }

            //Nome do Nível Ocupacional
            if (dr["NivelOcupacional"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.NomeNivelOcupacional = dr["NivelOcupacional"].ToString();
            }

            //Sigla da uf
            if (dr["SG_UF"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.SiglaUfDoUsuario = dr["SG_UF"].ToString();
            }

            //Nome da Uf
            if (dr["NM_UF"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.UfDoUsuarioPorExtenso = dr["NM_UF"].ToString();
            }

            //Informações do usuário - Fim


            if (dr["DT_UltimaAtualizacao"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.ConfiguracaoPagamento.Auditoria.DataAuditoria = DateTime.Parse(dr["DT_UltimaAtualizacao"].ToString());
            }

            if (dr["NM_UsuarioAtualizacao"] != DBNull.Value)
            {
                configuracaoPagamentoPublicoAlvo.ConfiguracaoPagamento.Auditoria.UsuarioAuditoria = dr["NM_UsuarioAtualizacao"].ToString();
            }
        }

        #endregion
    }
}

